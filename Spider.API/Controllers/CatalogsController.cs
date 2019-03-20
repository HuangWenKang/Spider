using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Spider.Api.Models;
using Spider.API.Entities;
using Spider.API.Repositories;
using System;
using System.Threading.Tasks;

namespace Spider.API.Controllers
{
    [Route("api/msdn/v1/catalogs")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogsRepository _catalogsRepository;
        
        public CatalogsController(ICatalogsRepository catalogsRepository)
        {
            _catalogsRepository = catalogsRepository 
                ?? throw new ArgumentNullException(nameof(catalogsRepository));            
        }
       
        [HttpGet]
        public async Task<IActionResult> GetCatalogs([FromQuery(Name = "keyword" )]string keyword)
        {
            var catalogEntities = await _catalogsRepository.GetCatalogsAsync(keyword);
            return Ok(catalogEntities);
        }

        [HttpGet]        
        [Route("{id}", Name = "GetCatalog")]
        public async Task<IActionResult> GetCatalog(string id)
        {
            var catalogEntity = await _catalogsRepository.GetCatalogAsync(id);
            if (catalogEntity == null)
            {
                return NotFound();
            }            
            return Ok((catalogEntity));            
        }      
    }
}
