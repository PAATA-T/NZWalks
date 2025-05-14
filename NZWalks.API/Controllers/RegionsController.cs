using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZwalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZwalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS
        // GET https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            //Get Data From DataBase - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            
            //return DTOs
            return Ok(regionsDto);

        }

        //Get Single REgion By ID
        //Get https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null) 
            { 
                return NotFound();
            
            }

            // Map/Convert Region DOmain model to Region DTO
            
            // return DTO back to client
            
            return Ok(mapper.Map<RegionDto>(regionDomain));
        
        
        }


        //post to create new REgion
        //POST to: https
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
            // Map or Convert DTO to DOMAIN model
            var regioDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model to create Region

            regioDomainModel = await regionRepository.CreateAsync(regioDomainModel);

            //Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDto>(regioDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

            

        }

        // update region
        //PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            //Map Dto to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }



            //Convert Domain model to DTO

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);

            
            

        }

        //Delete Region
        //Delete https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
           var regionDomainModel = await regionRepository.DeleteAsync(id);

             if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted region back
            //map domain model to Dto

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        
    }
}
