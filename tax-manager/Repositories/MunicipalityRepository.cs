using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tax_manager.model;
using tax_manager.Repositories;
using tax_manager.Exceptions;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Collections;

namespace tax_manager.Controllers
{
    public class MunicipalityRepository : IMunicipalityRespository
    {
        private readonly TaxManagerContext _context;

        public MunicipalityRepository(TaxManagerContext context)
        {
            _context = context;
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

        public float GetTaxInfo(string name, DateTime date)
        {
            var municipality = _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefault(m => m.Name.ToLower().Equals(name.ToLower()));

            if (municipality == null)
            {
                throw new NotFoundException("Municipality with name: " + name + " has not been found.");
            }

            Tax taxToApply;
            taxToApply = municipality.DailyTaxes
                .FirstOrDefault(t => t.InitialDate.CompareTo(date) <= 0 && t.FinalDate.CompareTo(date) >= 0);
            if (taxToApply != null) return taxToApply.Value;

            taxToApply = municipality.WeeklyTaxes
                .FirstOrDefault(t => t.InitialDate.CompareTo(date) <= 0 && t.FinalDate.CompareTo(date) >= 0);
            if (taxToApply != null) return taxToApply.Value;

            taxToApply = municipality.MonthlyTaxes
                .FirstOrDefault(t => t.InitialDate.CompareTo(date) <= 0 && t.FinalDate.CompareTo(date) >= 0);
            if (taxToApply != null) return taxToApply.Value;

            taxToApply = municipality.YearlyTaxes
                .FirstOrDefault(t => t.InitialDate.CompareTo(date) <= 0 && t.FinalDate.CompareTo(date) >= 0);
            if (taxToApply != null) return taxToApply.Value;

            throw new NotFoundException("Municipality with name: " + name + " has not defined a tax to be applied during the given date.");
        }

        public Municipality UpdateMunicipality(long id, UpdateMunicipalityRequest request)
        {
            var Municipality = _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefault(m => m.Id == id);

            if (!String.IsNullOrEmpty(request.Name)) Municipality.Name = request.Name;
            if (request.YearlyTaxes != null) Municipality.YearlyTaxes = request.YearlyTaxes;
            if (request.MonthlyTaxes != null) Municipality.MonthlyTaxes = request.MonthlyTaxes;
            if (request.WeeklyTaxes != null) Municipality.WeeklyTaxes = request.WeeklyTaxes;
            if (request.DailyTaxes != null) Municipality.DailyTaxes = request.DailyTaxes;

            _context.Entry(Municipality).State = EntityState.Modified;

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

            return Municipality;
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
                throw new NotFoundException("Municipality with Id: " + id + " has not been found.");
            }

            switch (request.Type)
            {
                case 'Y':
                    municipality.YearlyTaxes = 
                        insertTaxIntoList(municipality.YearlyTaxes, 
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'M':
                    municipality.MonthlyTaxes =
                        insertTaxIntoList(municipality.MonthlyTaxes,
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'W':
                    municipality.WeeklyTaxes =
                        insertTaxIntoList(municipality.WeeklyTaxes,
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'D':
                    municipality.DailyTaxes =
                        insertTaxIntoList(municipality.DailyTaxes,
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
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

        public Municipality CreateMunicipality(Municipality municipality)
        {
            _context.Municipalities.Add(municipality);
            _context.SaveChanges();
            return municipality;
        }

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

        public void LoadFromFile(string fileName)
        {
            const Int32 BufferSize = 512;
            var fileStream = File.OpenRead(fileName);
            var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
            string line;
            String[] values;
            Municipality municipality;
            string header = streamReader.ReadLine();
            while ((line = streamReader.ReadLine()) != null) 
            {
                values = line.Split(";");
                if (values.Length != 5)
                    throw new BadRequestException("Unexpected file format");
                municipality = _context.Municipalities.FirstOrDefault(m => m.Name.ToLower().Equals(values[0].ToLower()));
                if (municipality == null)
                    municipality = CreateMunicipality(new Municipality(values[0], new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>()));
                switch (char.Parse(values[1]))
                {
                    case 'Y':
                        municipality.YearlyTaxes =
                            insertTaxIntoList(municipality.YearlyTaxes,
                                new Tax(float.Parse(values[2]), DateTime.Parse(values[3]), DateTime.Parse(values[4])));
                        break;
                    case 'M':
                        municipality.MonthlyTaxes =
                            insertTaxIntoList(municipality.MonthlyTaxes,
                                new Tax(float.Parse(values[2]), DateTime.Parse(values[3]), DateTime.Parse(values[4])));
                        break;
                    case 'W':
                        municipality.WeeklyTaxes =
                            insertTaxIntoList(municipality.WeeklyTaxes,
                                new Tax(float.Parse(values[2]), DateTime.Parse(values[3]), DateTime.Parse(values[4])));
                        break;
                    case 'D':
                        municipality.DailyTaxes =
                            insertTaxIntoList(municipality.DailyTaxes,
                                new Tax(float.Parse(values[2]), DateTime.Parse(values[3]), DateTime.Parse(values[4])));
                        break;
                    default:
                        throw new BadRequestException("Invalid tax type provided.");
                }
            }

            return;
        }

        private bool MunicipalityExists(long id)
        {
            return _context.Municipalities.Any(e => e.Id == id);
        }

        private List<Tax> insertTaxIntoList(List<Tax> list, Tax tax) 
        {
            List<Tax> listToReturn = list != null ? list : new List<Tax>();
            if (!listToReturn.Any(t => t.Equals(tax)))
            {
                listToReturn.Add(tax);
            }
            return listToReturn;
        }
    }
}
