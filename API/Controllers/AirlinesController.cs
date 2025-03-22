using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.Core;
using Wego.Core.Models.Flights;
using Wego.Core.Repositories.Contract;
using Wego.Core.Services;

namespace Wego.API.Controllers
{
    //public class AirlinesController : BaseApiController
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IAirlineService _airlineService;
    //    private readonly IMapper _mapper;

    //    public AirlinesController(IUnitOfWork unitOfWork, IAirlineService airlineService, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //        _airlineService = airlineService;
    //    }

    //    [HttpGet("GetAllAirline")]
    //    public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 10, string search = "")
    //    {
    //        var airlineRepository = _unitOfWork.Repository<Airline>();

    //        var result = await airlineRepository.GetPaginatedAsync(pageIndex, pageSize,
    //            a => string.IsNullOrEmpty(search) || a.Name.ToLower().Contains(search.ToLower()));

    //        var resCount = await airlineRepository.CountAsync(a => string.IsNullOrEmpty(search) || a.Name.ToLower().Contains(search.ToLower()));

    //        var res = _mapper.Map<List<AirlineGetDto>>(result);

    //        return Ok(new { data = res, Total = resCount });
    //    }

    //    [HttpGet("{routeId:int}")]
    //    public async Task<IActionResult> GetById(int routeId)
    //    {
    //        var airlineRepository = _unitOfWork.Repository<Airline>();
    //        var airline = await airlineRepository.GetByIdAsync(routeId);

    //        if (airline != null)
    //        {
    //            var res = _mapper.Map<AirlineGetDto>(airline);
    //            return Ok(res);
    //        }

    //        return NotFound();
    //    }

    //    [HttpPost("CreateAirLine")]
    //    public async Task<IActionResult> NewAirline([FromForm] AirlinePostDto dto)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var airline = _mapper.Map<Airline>(dto);
    //        var airlineRepository = _unitOfWork.Repository<Airline>();

    //        try
    //        {
    //            await airlineRepository.AddAsync(airline);
    //            await _unitOfWork.CompleteAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            // تسجيل الخطأ
    //            return BadRequest("Error occurred while adding the airline.");
    //        }

    //        try
    //        {
    //            var request = HttpContext.Request;

    //            if (dto.Image != null)
    //            {
    //                airline.Image = await _airlineService.UpdateAirlineImageAsync(dto.Image, airline, request);
    //                await airlineRepository.UpdateAsync(airline);
    //                await _unitOfWork.CompleteAsync();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // التحقق مما إذا كان الكيان مضافًا قبل حذفه
    //            if (airline.Id > 0)
    //            {
    //                await airlineRepository.DeleteAsync(airline);
    //            }
    //            return BadRequest("Error occurred while updating images.");
    //        }

    //        var res = _mapper.Map<AirlinePostDto>(airline);
    //        return CreatedAtAction("GetById", new { routeId = airline.Id }, res);
    //    }


    //    [HttpPut("{routeId:int}")]
    //    public async Task<IActionResult> UpdateAirline(int routeId, AirlinePutDto dto)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        if (routeId != dto.Id)
    //            return BadRequest("Route id and Airline Id did not match");

    //        var airlineRepository = _unitOfWork.Repository<Airline>();
    //        var airline = await airlineRepository.GetByIdAsync(dto.Id);

    //        if (airline == null)
    //            return NotFound();

    //        airline.Name = dto.Name ?? airline.Name;
    //        airline.Code = dto.Code ?? airline.Code;

    //        if (dto.Image != null)
    //        {
    //            var request = HttpContext.Request;
    //            _airlineService.RemoveAirlineImage(airline);
    //            airline.Image = await _airlineService.UpdateAirlineImageAsync(dto.Image, airline, request);
    //        }

    //        try
    //        {
    //            await airlineRepository.UpdateAsync(airline);
    //            await _unitOfWork.CompleteAsync();
    //        }
    //        catch
    //        {
    //            return BadRequest("Error occurred while updating");
    //        }

    //        var res = _mapper.Map<AirlinePutDto>(airline);
    //        return Ok(res);
    //    }

    //    [HttpDelete("{routeId:int}")]
    //    public async Task<IActionResult> DeleteAirline(int routeId)
    //    {
    //        var airlineRepository = _unitOfWork.Repository<Airline>();
    //        var airline = await airlineRepository.GetByIdAsync(routeId);

    //        if (airline != null)
    //        {
    //            if (airline.Airplanes.Any())
    //                return BadRequest("You have to remove airplanes associated with this airline first");

    //            _airlineService.RemoveAirlineImage(airline);
    //            await airlineRepository.DeleteAsync(airline);
    //            await _unitOfWork.CompleteAsync();
    //            return NoContent();
    //        }

    //        return NotFound();
    //    }

    //    [HttpGet("{routeId:int}/flights")]
    //    public async Task<IActionResult> AirlineFlights(int routeId)
    //    {
    //        var airlineRepository = _unitOfWork.Repository<Airline>();
    //        var airline = await airlineRepository.GetByIdAsync(routeId);

    //        if (airline != null)
    //        {
    //            var flights = airline.Flights;
    //            var res = flights
    //                .Select(f => new
    //                {
    //                    Id = f.Id,
    //                    DepartureTime = f.DepartureTime.ToString("hh:mm tt"),
    //                    ArrivalTime = f.ArrivalTime.ToString("hh:mm tt"),
    //                    DepartureAirport = f.DepartureTerminal.Airport.Name,
    //                    ArrivalAirport = f.ArrivalTerminal.Airport.Name,
    //                    From = f.DepartureTerminal.Airport.Location.Country,
    //                    To = f.ArrivalTerminal.Airport.Location.Country
    //                });

    //            return Ok(res);
    //        }

    //        return NotFound();
    //    }

    //    [HttpGet("{routeId:int}/airplanes")]
    //    public async Task<IActionResult> AirlineAirplanes(int routeId)
    //    {
    //        var airlineRepository = _unitOfWork.Repository<Airline>();
    //        var airline = await airlineRepository.GetByIdAsync(routeId);

    //        if (airline != null)
    //        {
    //            var airplanes = airline.Airplanes;
    //            var res = airplanes
    //                .Select(a => new
    //                {
    //                    a.Id,
    //                    a.Type,
    //                    a.Code,
    //                    Features = a.Feature,
    //                });

    //            return Ok(res);
    //        }

    //        return NotFound();
    //    }

    //    [HttpGet("popular")]
    //    public async Task<IActionResult> PopularAirline(string country, int count = 5)
    //    {
    //        var airlines = await _airlineService.GetFamousAirlineAsync(country, count);
    //        var res = _mapper.Map<List<AirlineGetDto>>(airlines);

    //        return Ok(res);
    //    }
    //}
}
