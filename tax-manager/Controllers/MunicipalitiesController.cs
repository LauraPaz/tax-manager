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

        // GET: api/municipalities/test
        [HttpGet("test")]
        public ActionResult<Municipality> GetTest()
        {
            return Ok(_repo.GetTest());
        }

        // GET: api/municipalities
        [HttpGet]
        public ActionResult<List<Municipality>> GetMunicipalities()
        {
            return Ok(_repo.GetMunicipalities());
        }

        // GET: api/municipalities/5
        [HttpGet("{id}")]
        public ActionResult<Municipality> GetMunicipality(long id)
        {
            var municipality = _repo.GetMunicipality(id);

            return Ok(municipality);
        }

        // PUT: api/municipalities/5
        [HttpPut("{id}")]
        public ActionResult<Municipality> PutMunicipality(long id, Municipality municipality)
        {
            var updatedMunicipality = _repo.PutMunicipality(id, municipality);

            return Ok(updatedMunicipality);
        }

        // PUT: api/municipalities/5/schedule
        [HttpPut("{id}/schedule")]
        public ActionResult<Municipality> ScheduleTax(long id, ScheduleTaxRequest scheduleTaxRequest)
        {
            var updatedMunicipality = _repo.ScheduleTaxMunicipality(id, scheduleTaxRequest);

            return Ok(updatedMunicipality);
        }

        // POST: api/municipalities
        [HttpPost]
        public ActionResult<Municipality> PostMunicipality(Municipality municipality)
        {
            var createdMunicipality = _repo.PostMunicipality(municipality);
            return CreatedAtAction("PostMunicipality", new { id = createdMunicipality.Id }, createdMunicipality);
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
