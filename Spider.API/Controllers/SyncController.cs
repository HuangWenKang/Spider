using Books.Api.Models;
using HtmlSpider.Services;
using Microsoft.AspNetCore.Mvc;
using Spider.API.Entities;
using Spider.API.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoSpider.Services;

namespace Spider.API.Controllers
{
    [Route("api/msdn/v1/sync")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private IVideosRepository _videosRepository;
        private IVideoSpiderService _videoSpiderService;
        private ICatalogService _catalogService;
        private ICatalogsRepository _catalogsRepository;

        public SyncController(IVideosRepository videosRepository, IVideoSpiderService videoSpiderService, ICatalogService catalogService, ICatalogsRepository catalogsRepository)
        {
            _videosRepository = videosRepository ?? 
                throw new ArgumentNullException(nameof(videosRepository));

            _catalogsRepository = catalogsRepository ??
                throw new ArgumentNullException(nameof(catalogsRepository));

            _videoSpiderService = videoSpiderService ??
               throw new ArgumentNullException(nameof(videoSpiderService));

            _catalogService = catalogService ??
               throw new ArgumentNullException(nameof(catalogService));
        }

        [HttpPost("videos")]
        public async Task<IActionResult> SyncVideos([FromBody] VideoForSync videoForSync)
        {
            var videoSpiderResult = await _videoSpiderService.SearchAsync(videoForSync.Keyword, videoForSync.PageNumber,videoForSync.PageSize);
            var videoEntities = videoSpiderResult.Data.Select(d => new Video
            {
                Id = Guid.Parse(d.id),
                Title = d.title,
                Summary = d.summary,
                SearchScore = d.searchScore,
                TotalViews = d.views.total,
                MediaDuration = d.mediaDuration,
                Url = "https://channel9.msdn.com" + d.url
            }).ToList();

            foreach (var video in videoEntities)
            {
                _videosRepository.AddOrUpdateVideo(video);
            }
            bool isSucceed = await _videosRepository.SaveChangesAsync();
            return Ok(isSucceed);
        }

        [HttpPost("catalogs")]
        public async Task<IActionResult> SyncCatalogs()
        {
            var catalogEntities = new List<Catalog>();
            var catalogs = await _catalogService.SearchAsync("https://msdn.microsoft.com/en-us/library/ms123401.aspx");
            foreach (var catalog in catalogs)
            {
                foreach (var item in catalog.Items)
                {
                    catalogEntities.Add(new Catalog
                    {
                        Id = item.Id,
                        Link = item.Link,
                        Name = item.Name,
                        Group = catalog.Name
                    });
                }
            }
           
            foreach (var catalog in catalogEntities)
            {
                _catalogsRepository.AddCatalog(catalog);
            }
            bool isSucceed = await _catalogsRepository.SaveChangesAsync();
            return Ok(isSucceed);
        }
    }
}
