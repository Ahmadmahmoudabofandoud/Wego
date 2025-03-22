using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Wego.Core.Repositories.Contract;
using Wego.Core.Models;
using Wego.Core;
using Wego.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Wego.Core.Models.Hotels;
using System.Linq.Expressions;
using Wego.API.Models.DTOS.Rooms.Dtos;

namespace Wego.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roomRepository = _unitOfWork.Repository<Room>();
        }

        //[HttpGet("GetAllRooms")]
        //public async Task<IActionResult> GetAll(int pageIndex = 1, int pageSize = 10, string search = "")
        //{
        //    var rooms = await _roomRepository.SearchAsync(
        //        r => string.IsNullOrEmpty(search) || r.RoomTitle.ToLower().Contains(search.ToLower()),
        //    "RoomTitle", "asc", pageIndex, pageSize,
        //        "Images", "Hotel");

        //    var totalCount = (await _roomRepository.GetAllAsync()).Count;
        //    var mappedResult = _mapper.Map<List<RoomDto>>(rooms);

        //    foreach (var room in mappedResult)
        //    {
        //        room.Images = room.Images?.Select(img => $"{Request.Scheme}://{Request.Host.Value}{img}").ToList();
        //    }

        //    return Ok(new { message = "Rooms retrieved successfully", data = mappedResult, total = totalCount });
        //}

        //[HttpGet("GetRoomById/{id:int}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var room = await _roomRepository.GetAsync(id, "Images,Hotel");
        //    if (room is null)
        //        return NotFound(new { message = "Room not found" });

        //    var mappedRoom = _mapper.Map<RoomDto>(room);

        //    // التحقق من صحة الـ Request قبل استخدامه
        //    if (!string.IsNullOrEmpty(Request?.Host.Value) && mappedRoom.Images?.Any() == true)
        //    {
        //        string baseUrl = $"{Request.Scheme}://{Request.Host}";

        //        mappedRoom.Images = mappedRoom.Images
        //            .Where(img => !string.IsNullOrEmpty(img))
        //            .Select(img => img.StartsWith("/") ? $"{baseUrl}{img}" : $"{baseUrl}/{img}")
        //            .ToList();
        //    }

        //    return Ok(new { message = "Room retrieved successfully", data = mappedRoom });
        //}



        //[HttpPost("AddNewRoom")]
        //public async Task<IActionResult> AddRoom([FromForm] RoomPostDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Invalid data" });

        //    var room = _mapper.Map<Room>(dto);
        //    room.Images = await ProcessRoomImagesAsync(dto.Images, "roomsImg");

        //    await _roomRepository.AddAsync(room);
        //    await _unitOfWork.CompleteAsync();

        //    var result = _mapper.Map<RoomDto>(room);
        //    return CreatedAtAction(nameof(GetById), new { id = room.Id }, new { message = "Room created successfully", data = result });
        //}


        //[HttpPut("UpdateRoom/{id:int}")]
        //public async Task<IActionResult> UpdateRoom(int id, [FromForm] RoomPutDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { message = "Invalid data" });

        //    if (id != dto.Id)
        //        return BadRequest(new { message = "ID mismatch" });

        //    var room = await _roomRepository.GetAsync(dto.Id);
        //    if (room is null)
        //        return NotFound(new { message = "Room not found" });

        //    _mapper.Map(dto, room);
        //    room.Images = await ProcessRoomImagesAsync(dto.NewImages, "roomsImg", room.Images.ToList(), dto.ImagesToDelete);

        //    _roomRepository.Update(room);
        //    await _unitOfWork.CompleteAsync();

        //    var result = _mapper.Map<RoomDto>(room);
        //    return Ok(new { message = "Room updated successfully", data = result });
        //}

        //[HttpDelete("DeleteRoom/{id:int}")]
        //public async Task<IActionResult> DeleteRoom(int id)
        //{
        //    var room = await _roomRepository.GetAsync(id);
        //    if (room is null)
        //        return NotFound(new { message = "Room not found" });

        //    _roomRepository.Delete(room);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok(new { message = $"Room '{room.RoomTitle}' has been deleted successfully." });
        //}

        //private async Task<List<Image>> ProcessRoomImagesAsync(List<IFormFile>? imageFiles, string folder, List<Image>? existingImages = null, List<int>? imagesToDelete = null)
        //{
        //    existingImages ??= new List<Image>();

        //    if (imagesToDelete != null)
        //    {
        //        existingImages.RemoveAll(img => imagesToDelete.Contains(img.Id));
        //    }

        //    if (imageFiles == null || !imageFiles.Any())
        //        return existingImages;

        //    foreach (var file in imageFiles)
        //    {
        //        string newImageName = $"Room-{Guid.NewGuid()}.jpg";
        //        await ImageHelper.UploadImageAsync(file, folder, newImageName);
        //        existingImages.Add(new Image { Url = $"/imgs/{folder}/{newImageName}" });
        //    }

        //    return existingImages;
        //}
    }
}