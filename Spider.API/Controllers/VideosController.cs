using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Spider.Api.Models;
using Spider.API.Entities;
using Spider.API.Repositories;
using System;
using System.Threading.Tasks;

namespace Spider.API.Controllers
{
    [Route("api/msdn/v1/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private IVideosRepository _videosRepository;
        private readonly IMapper _mapper;

        public VideosController(IVideosRepository videosRepository)
        {
            _videosRepository = videosRepository 
                ?? throw new ArgumentNullException(nameof(videosRepository));            
        }

        [HttpGet]        
        public async Task<IActionResult> GetVideos()
        {
            var videoEntities = await _videosRepository.GetVideosAsync();
            return Ok(videoEntities);
        }

        [HttpGet]        
        [Route("{id}", Name = "GetVideo")]
        public async Task<IActionResult> GetVideo(Guid id)
        {
            var videoEntity = await _videosRepository.GetVideoAsync(id);
            if (videoEntity == null)
            {
                return NotFound();
            }            
            return Ok((videoEntity));            
        }

        [HttpPost]        
        public async Task<IActionResult> CreateVideo([FromBody] VideoForCreation video)
        {
            var videoEntity = _mapper.Map<Video>(video);
            _videosRepository.AddOrUpdateVideo(videoEntity);

            await _videosRepository.SaveChangesAsync();
            
            await _videosRepository.GetVideoAsync(videoEntity.Id);

            return CreatedAtRoute("GetVideo",
                new { id = videoEntity.Id },
                videoEntity);
        }
    }
}
