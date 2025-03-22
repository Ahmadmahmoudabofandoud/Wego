using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Wego.Core;
using Wego.Core.Models;
using Wego.API.Helpers;
using API.Errors;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.Core.Models.Flights;
using Wego.Core.Specifications;
using Wego.Core.Specifications.AirportSpecification;

namespace Wego.API.Controllers
{
    public class AirportsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<AirportDto>>> GetAllAirports([FromQuery] AppSpecParams specParams)
        {
            var spec = new AirportWithLocationSpecification(specParams);
            var airports = await _unitOfWork.Repository<Airport>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Airport>().GetCountWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Airport>, IReadOnlyList<AirportDto>>(airports);
            return Ok(new Pagination<AirportDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AirportDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<AirportDto>> GetAirportById(int id)
        {
            var spec = new AirportWithLocationSpecification(id);
            var airport = await _unitOfWork.Repository<Airport>().GetEntityWithSpecAsync(spec);
            if (airport == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<AirportDto>(airport));
        }

        [HttpPost]
        public async Task<ActionResult<AirportDto>> AddAirport([FromBody] AirportPostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var airport = _mapper.Map<Airport>(dto);
            await _unitOfWork.Repository<Airport>().Add(airport);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAirportById), new { id = airport.Id }, _mapper.Map<AirportDto>(airport));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AirportDto>> UpdateAirport(int id, [FromBody] AirportPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var airport = await _unitOfWork.Repository<Airport>().GetByIdAsync(dto.Id);
            if (airport == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, airport);
            _unitOfWork.Repository<Airport>().Update(airport);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<AirportDto>(airport));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAirport(int id)
        {
            var airport = await _unitOfWork.Repository<Airport>().GetByIdAsync(id);
            if (airport == null) return NotFound(new ApiResponse(404));

            if (airport.FlightArrivalAirports.Any() || airport.FlightDepartureAirports.Any())
                return BadRequest("Airport has associated flights.");

            _unitOfWork.Repository<Airport>().Delete(airport);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Airport '{airport.Name}' has been deleted successfully." });
        }
    }
}
