using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core.Models;
using Microsoft.AspNetCore.Identity;
using API.Errors;
using Wego.API.Controllers;
using Wego.API.Helpers;
using Wego.API.Models.DTOS;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;
using Wego.Core.Specifications;
using Wego.Core;

[ApiController]
[Route("api/[controller]")]
public class AttractionsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AttractionsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<AttractionDto>>> GetAllAttractions([FromQuery] AppSpecParams specParams)
    {
        var spec = new AttractionSpecification(specParams);
        var attractions = await _unitOfWork.Repository<Attraction>().GetAllWithSpecAsync(spec);
        var totalCount = await _unitOfWork.Repository<Attraction>().GetCountWithSpecAsync(spec);

        var currentUser = await _userManager.GetUserAsync(User);
        var favoriteAttractionIds = currentUser != null
            ? (await _unitOfWork.Repository<Favorite>().GetUserIdAsync(currentUser.Id))
                .Select(f => f.Id)
                .Cast<int?>()
                .ToList()
            : new List<int?>();

        var data = _mapper.Map<IReadOnlyList<Attraction>, IReadOnlyList<AttractionDto>>(attractions);
        data = data.Select(attr =>
        {
            attr.IsFavorite = favoriteAttractionIds.Contains(attr.Id);
            attr.Image = string.IsNullOrEmpty(attr.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{attr.Image}";
            return attr;
        }).ToList();

        return Ok(new Pagination<AttractionDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AttractionDto), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<ActionResult<AttractionDto>> GetAttractionById(int id)
    {
        var spec = new AttractionSpecification(id);
        var attraction = await _unitOfWork.Repository<Attraction>().GetEntityWithSpecAsync(spec);
        if (attraction == null) return NotFound(new ApiResponse(404));

        var currentUser = await _userManager.GetUserAsync(User);
        var isFavorite = currentUser != null
            ? (await _unitOfWork.Repository<Favorite>().GetUserIdAsync(currentUser.Id))
                .Any(f => f.Id == id)
            : false;

        var res = _mapper.Map<AttractionDto>(attraction);
        res.IsFavorite = isFavorite;
        res.Image = string.IsNullOrEmpty(res.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{res.Image}";

        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<AttractionDto>> AddAttraction([FromForm] AttractionPostDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var attraction = _mapper.Map<Attraction>(dto);
        attraction.Image = await ProcessAttractionImageAsync(dto.ImageFile, "attractionsImg");

        if (dto.LocationId.HasValue)
        {
            var location = await _unitOfWork.Repository<Location>().GetByIdAsync(dto.LocationId.Value);
            if (location == null)
            {
                return NotFound(new ApiResponse(404, "Location not found"));
            }
            attraction.Location = location;
        }

        await _unitOfWork.Repository<Attraction>().Add(attraction);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetAttractionById), new { id = attraction.Id }, _mapper.Map<AttractionDto>(attraction));
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<AttractionDto>> UpdateAttraction(int id, [FromForm] AttractionPutDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != dto.Id) return BadRequest("ID mismatch");

        var attraction = await _unitOfWork.Repository<Attraction>().GetByIdAsync(dto.Id);
        if (attraction == null) return NotFound(new ApiResponse(404));

        _mapper.Map(dto, attraction);
        attraction.Image = await ProcessAttractionImageAsync(dto.ImageFile, "attractionsImg", attraction.Image);

        _unitOfWork.Repository<Attraction>().Update(attraction);
        await _unitOfWork.CompleteAsync();

        return Ok(_mapper.Map<AttractionDto>(attraction));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAttraction(int id)
    {
        var attraction = await _unitOfWork.Repository<Attraction>().GetByIdAsync(id);
        if (attraction == null) return NotFound(new ApiResponse(404));

        _unitOfWork.Repository<Attraction>().Delete(attraction);
        await _unitOfWork.CompleteAsync();

        return Ok(new { message = $"Attraction '{attraction.Name}' has been deleted successfully." });
    }

    private async Task<string?> ProcessAttractionImageAsync(IFormFile? imageFile, string folder, string? existingImage = null)
    {
        if (imageFile == null) return existingImage;

        if (!string.IsNullOrEmpty(existingImage))
        {
            var oldImageName = Path.GetFileName(existingImage);
            ImageHelper.RemoveImage(folder, oldImageName);
        }

        string newImageName = $"attraction-{Guid.NewGuid()}.jpg";
        await ImageHelper.UploadImageAsync(imageFile, folder, newImageName);

        return $"/imgs/{folder}/{newImageName}";
    }
}