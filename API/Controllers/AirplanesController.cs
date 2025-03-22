using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.Core.Models.Flights;
using Wego.Core.Repositories.Contract;
using Wego.Core;
using Microsoft.EntityFrameworkCore;

namespace Wego.API.Controllers
{

    //public class AirplanesController : BaseApiController
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IAirplaneService _airplaneService;
    //    private readonly IMapper _mapper;

    //    public AirplanesController(IUnitOfWork unitOfWork, IAirplaneService airplaneService, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //        _airplaneService = airplaneService;
    //    }
    //    [HttpGet("GetAllAirplanes")]
    //    public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 10, string search = "")
    //    {
    //        var airplaneRepo = _unitOfWork.Repository<Airplane>();

    //        var airplanes = await airplaneRepo.GetPaginatedAsync(
    //            pageIndex, pageSize,
    //            a => string.IsNullOrEmpty(search) || a.Code.ToLower().Contains(search.ToLower())
    //        );

    //        var totalCount = await airplaneRepo.CountAsync(
    //            a => string.IsNullOrEmpty(search) || a.Code.ToLower().Contains(search.ToLower())
    //        );

    //        var result = _mapper.Map<List<AirplaneGetDto>>(airplanes);
    //        return Ok(new { data = result, total = totalCount });
    //    }

    //    [HttpGet("GetAirplaneById/{id:int}")]
    //    public async Task<IActionResult> GetById(int id)
    //    {
    //        var airplane = await _unitOfWork.Repository<Airplane>().GetByIdAsync(id);
    //        if (airplane == null)
    //            return NotFound(new { message = "Airplane not found." });

    //        return Ok(_mapper.Map<AirplaneGetDto>(airplane));
    //    }

    //    [HttpPost("NewAirplane")]
    //    public async Task<IActionResult> NewAirplane([FromBody] AirplanePostDto dto)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var airplane = _mapper.Map<Airplane>(dto);

    //        try
    //        {
    //            await _unitOfWork.Repository<Airplane>().AddAsync(airplane);
    //            await _unitOfWork.CompleteAsync();

    //            await _airplaneService.UpdateAirplaneFeaturesAsync(airplane, dto.FeatureNames);
    //            await _unitOfWork.CompleteAsync(); 

    //            await _airplaneService.CreateAirplaneSeatsAsync(airplane, dto.EconomySeats, dto.BusinessSeats, dto.FirstClassSeats);
    //            await _unitOfWork.CompleteAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(new
    //            {
    //                Message = "Error occurred while adding airplane",
    //                PossibleErrors = new List<string> { "Airline does not exist", ex.Message }
    //            });
    //        }
    //        var savedAirplane = (await _unitOfWork.Repository<Airplane>()
    //            .GetListAsync(a => a.Id == airplane.Id, q => q.Include(a => a.Feature).Include(a => a.Airline)))
    //            .FirstOrDefault();

    //        return CreatedAtAction(nameof(GetById), new { id = airplane.Id }, _mapper.Map<AirplaneGetDto>(savedAirplane));
    //    }

    //    [HttpPut("updateAirplane/{id:int}")]
    //    public async Task<IActionResult> UpdateAirplane(int id, [FromBody] AirplanePutDto dto)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        if (id != dto.Id)
    //            return BadRequest(new { message = "Route ID and Airplane ID do not match." });

    //        var airplaneRepo = _unitOfWork.Repository<Airplane>();
    //        var airplane = await airplaneRepo.GetByIdAsync(id);

    //        if (airplane == null)
    //            return NotFound(new { message = "Airplane not found." });

    //        _mapper.Map(dto, airplane);

    //        if (dto.FeatureNames != null)
    //            await _airplaneService.UpdateAirplaneFeaturesAsync(airplane, dto.FeatureNames);

    //        try
    //        {
    //            await airplaneRepo.UpdateAsync(airplane);
    //            await _unitOfWork.CompleteAsync();
    //        }
    //        catch
    //        {
    //            return BadRequest(new { message = "Error occurred while updating airplane." });
    //        }

    //        return Ok(_mapper.Map<AirplaneGetDto>(airplane));
    //    }

    //    [HttpDelete("deleteAirplane/{id:int}")]
    //    public async Task<IActionResult> DeleteAirplane(int id)
    //    {
    //        var airplaneRepo = _unitOfWork.Repository<Airplane>();
    //        var airplane = await airplaneRepo.GetByIdAsync(id);

    //        if (airplane == null)
    //            return NotFound(new { message = "Airplane not found." });

    //        if (airplane.Flights.Any())
    //            return BadRequest(new { message = "There are flights associated with this airplane. It can't be deleted." });

    //        await airplaneRepo.DeleteAsync(airplane);
    //        await _unitOfWork.CompleteAsync();

    //        return NoContent();
    //    }

    //    [HttpGet("{id:int}/flights")]
    //    public async Task<IActionResult> GetAirplaneFlights(int id)
    //    {
    //        var airplane = await _unitOfWork.Repository<Airplane>().GetByIdAsync(id);
    //        if (airplane == null)
    //            return NotFound(new { message = "Airplane not found." });

    //        var flights = airplane.Flights.Select(f => new
    //        {
    //            f.Id,
    //            DepartureTime = f.DepartureTime.ToString("hh:mm tt"),
    //            ArrivalTime = f.ArrivalTime.ToString("hh:mm tt"),
    //            DepartureAirport = f.DepartureTerminal.Airport.Name,
    //            ArrivalAirport = f.ArrivalTerminal.Airport.Name,
    //            From = f.DepartureTerminal.Airport.Location.Country,
    //            To = f.ArrivalTerminal.Airport.Location.Country
    //        });

    //        return Ok(flights);
    //    }
    //}
}
