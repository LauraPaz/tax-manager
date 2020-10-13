using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tax_manager;
using tax_manager.model;
using tax_manager.Repositories;

namespace tax_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalitiesController : ControllerBase
    {
        private readonly IMunicipalityRespository _repo;

        public MunicipalitiesController(IMunicipalityRespository repo)
        {
            _repo = repo;
        }

        // GET: api/municipalities/load-file
        [HttpGet("load-file")]
        public ActionResult<List<Municipality>> LoadFromFile()
        {
            _repo.LoadFromFile("Data.csv");
            return GetMunicipalities();
        }

        // GET: api/municipalities
        [HttpGet]
        public ActionResult<List<Municipality>> GetMunicipalities()
        {
            return Ok(_repo.GetMunicipalities());
        }

        // GET: api/municipalities/1
        [HttpGet("{id}")]
        public ActionResult<Municipality> GetMunicipality(long id)
        {
            var municipality = _repo.GetMunicipality(id);

            return Ok(municipality);
        }

        // GET: api/municipalities/copenhagen/2020-10-13
        [HttpGet("{name}/{date}")]
        public ActionResult<float> GetTaxInfo(string name, DateTime date)
        {
            var result = _repo.GetTaxInfo(name, date);

            return Ok(result);
        }

        // PUT: api/municipalities/1/update
        [HttpPut("{id}/update")]
        public ActionResult<Municipality> UpdateMunicipality(long id, UpdateMunicipalityRequest request)
        {
            var updatedMunicipality = _repo.UpdateMunicipality(id, request);

            return Ok(updatedMunicipality);
        }

        // PUT: api/municipalities
        [HttpPut("{id}")]
        public ActionResult<Municipality> ScheduleTaxMunicipality(long id, ScheduleTaxRequest request)
        {
            var updatedMunicipality = _repo.ScheduleTaxMunicipality(id, request);
            return Ok(updatedMunicipality);
        }

        // POST: api/municipalities
        [HttpPost]
        public ActionResult<Municipality> CreateMunicipality(Municipality municipality)
        {
            var createdMunicipality = _repo.CreateMunicipality(municipality);
            return CreatedAtAction("CreateMunicipality", new { id = createdMunicipality.Id }, createdMunicipality);
        }

        // DELETE: api/municipalities/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMunicipality(long id)
        {
            _repo.DeleteMunicipality(id);
            return NoContent();
        }
    }
}
