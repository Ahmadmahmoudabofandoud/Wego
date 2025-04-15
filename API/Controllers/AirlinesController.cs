using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wego.Core;
using Wego.Core.Models;
using Wego.Core.Repositories.Contract;
using Wego.Core.Services;
using Wego.Core.Specifications.AirlineSpecification;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Helpers;
using API.Errors;
using System.IO;
using Wego.Core.Models.Flights;
using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Identity;

namespace Wego.API.Controllers
{
    public class AirlineController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AirlineController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<AirlineDto>>> GetAllAirlines([FromQuery] AirlineSpecParams specParams)
        {
            var spec = new AirlineWithAirplanesAndFlightsSpecification(specParams);
            var airlines = await _unitOfWork.Repository<Airline>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Airline>().GetCountWithSpecAsync(spec);

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteAirlineIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.AirlineId != null))
                    .Select(f => f.AirlineId)
                    .ToList()
                : new List<int?>();

            var data = _mapper.Map<IReadOnlyList<Airline>, IReadOnlyList<AirlineDto>>(airlines);
            data = data.Select(a =>
            {
                a.IsFavorite = favoriteAirlineIds.Contains(a.Id);
                a.Image = string.IsNullOrEmpty(a.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{a.Image}";
                return a;
            }).ToList();

            return Ok(new Pagination<AirlineDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AirlineDto>> GetAirlineById(int id)
        {
            var spec = new AirlineWithAirplanesAndFlightsSpecification(id);
            var airline = await _unitOfWork.Repository<Airline>().GetEntityWithSpecAsync(spec);
            if (airline == null) return NotFound(new ApiResponse(404));

            var currentUser = await _userManager.GetUserAsync(User);
            var isFavorite = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.AirlineId == id))
                    .Any()
                : false;

            var result = _mapper.Map<AirlineDto>(airline);
            result.IsFavorite = isFavorite;
            result.Image = string.IsNullOrEmpty(result.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{result.Image}";

            return Ok(result);
        }



        //public class AirlineController : BaseApiController
        //{
        //    private readonly IUnitOfWork _unitOfWork;
        //    private readonly IMapper _mapper;

        //    public AirlineController(IUnitOfWork unitOfWork, IMapper mapper)
        //    {
        //        _unitOfWork = unitOfWork;
        //        _mapper = mapper;
        //    }

        //    [HttpGet]
        //    public async Task<ActionResult<Pagination<AirlineDto>>> GetAllAirlines([FromQuery] AirlineSpecParams specParams)
        //    {
        //        var spec = new AirlineWithAirplanesAndFlightsSpecification(specParams);
        //        var airlines = await _unitOfWork.Repository<Airline>().GetAllWithSpecAsync(spec);
        //        var totalCount = await _unitOfWork.Repository<Airline>().GetCountWithSpecAsync(spec);

        //        var data = _mapper.Map<IReadOnlyList<Airline>, IReadOnlyList<AirlineDto>>(airlines);
        //        data = data.Select(a =>
        //        {
        //            a.Image = string.IsNullOrEmpty(a.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{a.Image}";
        //            return a;
        //        }).ToList();

        //        return Ok(new Pagination<AirlineDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        //    }

        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<AirlineDto>> GetAirlineById(int id)
        //    {
        //        var spec = new AirlineWithAirplanesAndFlightsSpecification(id);
        //        var airline = await _unitOfWork.Repository<Airline>().GetEntityWithSpecAsync(spec);
        //        if (airline == null) return NotFound(new ApiResponse(404));

        //        var result = _mapper.Map<AirlineDto>(airline);
        //        result.Image = string.IsNullOrEmpty(result.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{result.Image}";

        //        return Ok(result);
        //    }
        [HttpPost]
        public async Task<ActionResult<AirlineDto>> AddAirline([FromForm] AirlinePostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var airline = _mapper.Map<Airline>(dto);
            airline.Image = await ProcessLocationImageAsync(dto.ImageFile, "airlinesImg");

            await _unitOfWork.Repository<Airline>().Add(airline);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAirlineById), new { id = airline.Id }, _mapper.Map<AirlineDto>(airline));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AirlineDto>> UpdateAirline(int id, [FromForm] AirlinePutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var airline = await _unitOfWork.Repository<Airline>().GetByIdAsync(dto.Id);
            if (airline == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, airline);
            airline.Image = await ProcessLocationImageAsync(dto.Image, "airlinesImg", airline.Image);

            _unitOfWork.Repository<Airline>().Update(airline);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<AirlineDto>(airline));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAirline(int id)
        {
            var airline = await _unitOfWork.Repository<Airline>().GetByIdAsync(id);
            if (airline == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Airline>().Delete(airline);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Airline '{airline.Name}' has been deleted successfully." });
        }

        private async Task<string?> ProcessLocationImageAsync(IFormFile? imageFile, string folder, string? existingImage = null)
        {
            if (imageFile == null) return existingImage;

            if (!string.IsNullOrEmpty(existingImage))
            {
                var oldImageName = Path.GetFileName(existingImage);
                ImageHelper.RemoveImage(folder, oldImageName);
            }

            string newImageName = $"location-{Guid.NewGuid()}.jpg";
            await ImageHelper.UploadImageAsync(imageFile, folder, newImageName);

            return $"/imgs/{folder}/{newImageName}";
        }
    }
}
