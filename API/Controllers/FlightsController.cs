using API.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.Core;
using Wego.Core.Models.Flights;
using Wego.Core.Repositories.Contract;
using Wego.Core.Specifications.AirlineSpecification;
using Wego.Core.Specifications.HotelSpecification;

namespace Wego.API.Controllers
{
    //public class FlightsController : BaseApiController
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public FlightsController(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<FlightTicketDTO>>> GetFlights([FromQuery] FlightSpecParams specParams)
    //    {
    //        var spec = new FlightsWithDetailsSpecification(specParams);
    //        var flights = await _unitOfWork.Repository<Flight>().GetAllWithSpecAsync(spec);
    //        return Ok(_mapper.Map<IEnumerable<FlightTicketDTO>>(flights));
    //    }
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<FlightTicketDTO>> GetFlight(int id)
    //    {
    //        var spec = new FlightsWithDetailsSpecification(id);
    //        var flight = await _unitOfWork.Repository<Flight>().GetEntityWithSpecAsync(spec);

    //        if (flight == null)
    //            return NotFound(new ApiResponse(404, "Flight not found"));

    //        return Ok(_mapper.Map<FlightTicketDTO>(flight));
    //    }


    //    [HttpPost]
    //    public async Task<ActionResult<FlightDTO>> CreateFlight(FlightDTO flightDto)
    //    {
    //        var flight = _mapper.Map<Flight>(flightDto);
    //        await _unitOfWork.Repository<Flight>().Add(flight);
    //        await _unitOfWork.CompleteAsync();

    //        return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, _mapper.Map<FlightTicketDTO>(flight));
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateFlight(int id, FlightDTO flightDto)
    //    {
    //        var spec = new FlightsWithDetailsSpecification(id);
    //        var flight = await _unitOfWork.Repository<Flight>().GetEntityWithSpecAsync(spec);
    //        if (flight == null) return NotFound(new ApiResponse(404, "Flight not found"));

    //        _mapper.Map(flightDto, flight);
    //        _unitOfWork.Repository<Flight>().Update(flight);
    //        await _unitOfWork.CompleteAsync();

    //        return NoContent();
    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteFlight(int id)
    //    {
    //        var spec = new FlightsWithDetailsSpecification(id);
    //        var flight = await _unitOfWork.Repository<Flight>().GetEntityWithSpecAsync(spec);
    //        if (flight == null) return NotFound(new ApiResponse(404, "Flight not found"));

    //        _unitOfWork.Repository<Flight>().Delete(flight);
    //        await _unitOfWork.CompleteAsync();

    //        return NoContent();
    //    }
    //}
}
