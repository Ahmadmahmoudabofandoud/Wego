using API.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.API.Controllers;
using Wego.API.Helpers;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.Core.Models.Flights;
using Wego.Core.Specifications;
using Wego.Core;
using Wego.Core.Specifications.AirplaneSpecification;

namespace Wego.API.Controllers
{
    //public class AirplanesController : BaseApiController
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public AirplanesController(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<Pagination<AirplaneDto>>> GetAllAirplanes([FromQuery] AppSpecParams specParams)
    //    {
    //        var spec = new AirplaneWithDetailsSpecification(specParams);
    //        var airplanes = await _unitOfWork.Repository<Airplane>().GetAllWithSpecAsync(spec);
    //        var totalCount = await _unitOfWork.Repository<Airplane>().GetCountWithSpecAsync(spec);

    //        var data = _mapper.Map<IReadOnlyList<Airplane>, IReadOnlyList<AirplaneDto>>(airplanes);
    //        return Ok(new Pagination<AirplaneDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<AirplaneDto>> GetAirplaneById(int id)
    //    {
    //        var spec = new AirplaneWithDetailsSpecification(id);
    //        var airplane = await _unitOfWork.Repository<Airplane>().GetEntityWithSpecAsync(spec);
    //        if (airplane == null) return NotFound(new ApiResponse(404));
    //        return Ok(_mapper.Map<AirplaneDto>(airplane));
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<AirplaneDto>> AddAirplane([FromBody] AirplanePostDto dto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(ModelState);

    //        var airplane = _mapper.Map<Airplane>(dto);
    //        await _unitOfWork.Repository<Airplane>().Add(airplane);
    //        await _unitOfWork.CompleteAsync();
    //        return CreatedAtAction(nameof(GetAirplaneById), new { id = airplane.Id }, _mapper.Map<AirplaneDto>(airplane));
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<ActionResult<AirplaneDto>> UpdateAirplane(int id, [FromBody] AirplanePutDto dto)
    //    {
    //        if (!ModelState.IsValid) return BadRequest(ModelState);
    //        if (id != dto.Id) return BadRequest("ID mismatch");

    //        var airplane = await _unitOfWork.Repository<Airplane>().GetByIdAsync(dto.Id);
    //        if (airplane == null) return NotFound(new ApiResponse(404));

    //        _mapper.Map(dto, airplane);
    //        _unitOfWork.Repository<Airplane>().Update(airplane);
    //        await _unitOfWork.CompleteAsync();

    //        return Ok(_mapper.Map<AirplaneDto>(airplane));
    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<ActionResult> DeleteAirplane(int id)
    //    {
    //        var airplane = await _unitOfWork.Repository<Airplane>().GetByIdAsync(id);
    //        if (airplane == null) return NotFound(new ApiResponse(404));

    //        _unitOfWork.Repository<Airplane>().Delete(airplane);
    //        await _unitOfWork.CompleteAsync();

    //        return Ok(new { message = "Airplane deleted successfully." });
    //    }
    //}
}