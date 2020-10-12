using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tax_manager;
using tax_manager.model;
using tax_manager.Repositories;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.Mvc.Formatters;
using tax_manager.Exceptions;

namespace tax_manager.Controllers
{
    public class MunicipalityRepository : IMunicipalityRespository
    {
        private readonly TaxManagerContext _context;

        public MunicipalityRepository(TaxManagerContext context)
        {
            _context = context;
        }

        public Municipality GetTest()
        {
            return new Municipality("name", new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>());
        }

        public List<Municipality> GetMunicipalities()
        {
            return _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .ToList();
        }

        public Municipality GetMunicipality(long id)
        {
            var municipality = _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefault(m => m.Id == id);

            if (municipality == null)
            {
                throw new NotFoundException("Municipality with Id: " + id + " has not been found");
            }

            return municipality;
        }

        public Municipality PutMunicipality(long id, Municipality municipality)
        {
            if (id != municipality.Id)
            {
                throw new BadRequestException("The given Id does not match the municipality's Id");
            }

            _context.Entry(municipality).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MunicipalityExists(id))
                {
                    throw new NotFoundException("Municipality with Id: "+id+" has not been found");
                }
                else
                {
                    throw;
                }
            }

            return municipality;
        }

        public Municipality ScheduleTaxMunicipality(long id, ScheduleTaxRequest request)
        {
            var municipality = _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefault(m => m.Id == id);

            if (municipality == null)
            {
                throw new NotFoundException("Municipality with Id: " + id + " has not been found");
            }

            switch (request.Type)
            {
                case 'Y':
                    municipality.YearlyTaxes = 
                        insertTaxIntoList(municipality.YearlyTaxes, new Tax(request.Value, request.InitialDate, request.FinalDate));
                    break;
                case 'M':
                    municipality.MonthlyTaxes =
                        insertTaxIntoList(municipality.MonthlyTaxes, new Tax(request.Value, request.InitialDate, request.FinalDate));
                    break;
                case 'W':
                    municipality.WeeklyTaxes =
                        insertTaxIntoList(municipality.WeeklyTaxes, new Tax(request.Value, request.InitialDate, request.FinalDate));
                    break;
                case 'D':
                    municipality.DailyTaxes =
                        insertTaxIntoList(municipality.DailyTaxes, new Tax(request.Value, request.InitialDate, request.FinalDate));
                    break;
                default:
                    throw new BadRequestException("Invalid tax type provided.");
            }

            _context.Entry(municipality).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MunicipalityExists(id))
                {
                    throw new NotFoundException("Municipality with Id: " + id + " has not been found");
                }
                else
                {
                    throw;
                }
            }

            return municipality;
        }

        public Municipality PostMunicipality(Municipality municipality)
        {
            _context.Municipalities.Add(municipality);
            _context.SaveChanges();
            return municipality;
        }

        // DELETE: api/Municipalities/5
        [HttpDelete("{id}")]
        public void DeleteMunicipality(long id)
        {
            var municipality = _context.Municipalities.Find(id);
            if (municipality == null)
            {
                throw new NotFoundException("Municipality with Id: " + id + " has not been found");
            }

            _context.Municipalities.Remove(municipality);
            _context.SaveChanges();

            return;
        }

        private bool MunicipalityExists(long id)
        {
            return _context.Municipalities.Any(e => e.Id == id);
        }

        private List<Tax> insertTaxIntoList(List<Tax> list, Tax tax) 
        {
            List<Tax> listToReturn = list != null ? list : new List<Tax>();
            listToReturn.Add(tax);
            return listToReturn;
        }
    }
}
