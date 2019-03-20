using AutoMapper;
using Spider.Api.Models;
using Spider.API.Entities;
using System.Collections.Generic;

namespace Spider.API.Profiles
{
    /// <summary>
    /// AutoMapper profile for working with Movie objects
    /// </summary>
    public class VideosProfile : Profile
    {
        public VideosProfile()
        {            
            CreateMap<VideoForCreation, Video>();            
        }
    }
}
