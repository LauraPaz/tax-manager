using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tax_manager;
using tax_manager.Exceptions;
using tax_manager.model;
using tax_manager.Repositories;

namespace tax_manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MunicipalitiesController : ControllerBase
    {
        private readonly IMunicipalityRespository _repo;

        public MunicipalitiesController(IMunicipalityRespository repo)
        {
            _repo = repo;
        }

        // GET: municipalities/load-file
        [HttpGet("load-file")]
        public ActionResult<List<Municipality>> LoadFromFile()
        {
            try
            {
                _repo.LoadFromFile("Data.csv");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return GetMunicipalities();
        }

        // GET: municipalities
        [HttpGet]
        public ActionResult<List<Municipality>> GetMunicipalities()
        {
            try
            {
                return Ok(_repo.GetMunicipalities());
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        // GET: municipalities/1
        [HttpGet("{id}")]
        public ActionResult<Municipality> GetMunicipality(long id)
        {
            try
            {
                var municipality = _repo.GetMunicipality(id);
                return Ok(municipality);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: municipalities/copenhagen/2020-10-13
        [HttpGet("{name}/{date}")]
        public ActionResult<float> GetTaxInfo(string name, DateTime date)
        {
            try
            {
                var result = _repo.GetTaxInfo(name, date);
                return Ok(result);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: municipalities/1/update
        [HttpPut("{id}/update")]
        public ActionResult<Municipality> UpdateMunicipality(long id, UpdateMunicipalityRequest request)
        {
            try 
            { 
                var updatedMunicipality = _repo.UpdateMunicipality(id, request);
                return Ok(updatedMunicipality);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: municipalities
        [HttpPut("{id}")]
        public ActionResult<Municipality> ScheduleTaxMunicipality(long id, ScheduleTaxRequest request)
        {
            try
            {
                var updatedMunicipality = _repo.ScheduleTaxMunicipality(id, request);
                return Ok(updatedMunicipality);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: municipalities
        [HttpPost]
        public ActionResult<Municipality> CreateMunicipality(CreateMunicipalityRequest municipality)
        {
            try 
            {
                var createdMunicipality = _repo.CreateMunicipality(municipality);
                return CreatedAtAction("CreateMunicipality", new { id = createdMunicipality.Id }, createdMunicipality);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // DELETE: municipalities/1
        [HttpDelete("{id}")]
        public ActionResult DeleteMunicipality(long id)
        {
            try { 
                _repo.DeleteMunicipality(id);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
