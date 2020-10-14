using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using tax_manager.model;
using tax_manager.Repositories;
using tax_manager.Exceptions;
using System.IO;
using System.Text;
using System.Globalization;

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
                throw new NotFoundException("Municipality with Id: " + id + " has not been found.");

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
                throw new NotFoundException("Municipality with name: " + name + " has not been found.");

            Tax taxToApply; //More specific -> higher priority

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
            var municipality = _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefault(m => m.Id == id);

            if (municipality == null)
                throw new NotFoundException("Municipality with Id: " + id + " has not been found.");

            if (!String.IsNullOrEmpty(request.Name)) municipality.Name = request.Name;
            if (request.YearlyTaxes != null) municipality.YearlyTaxes = request.YearlyTaxes;
            if (request.MonthlyTaxes != null) municipality.MonthlyTaxes = request.MonthlyTaxes;
            if (request.WeeklyTaxes != null) municipality.WeeklyTaxes = request.WeeklyTaxes;
            if (request.DailyTaxes != null) municipality.DailyTaxes = request.DailyTaxes;

            _context.Entry(municipality).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!MunicipalityExists(id))
                    throw new NotFoundException("Municipality with Id: " + id + " has not been found.");
                throw;
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
                throw new NotFoundException("Municipality with Id: " + id + " has not been found.");

            switch (request.Type)
            {
                case 'Y' :
                case 'y' :
                    municipality.YearlyTaxes = 
                        InsertTaxIntoList(municipality.YearlyTaxes, 
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'M':
                case 'm':
                    municipality.MonthlyTaxes =
                        InsertTaxIntoList(municipality.MonthlyTaxes,
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'W':
                case 'w':
                    municipality.WeeklyTaxes =
                        InsertTaxIntoList(municipality.WeeklyTaxes,
                            new Tax(request.Value.Value, request.InitialDate.Value, request.FinalDate.Value));
                    break;
                case 'D':
                case 'd':
                    municipality.DailyTaxes =
                        InsertTaxIntoList(municipality.DailyTaxes,
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
            catch (DbUpdateException)
            {
                if (!MunicipalityExists(id))
                    throw new NotFoundException("Municipality with Id: " + id + " has not been found");
                throw;
            }

            return municipality;
        }

        public Municipality CreateMunicipality(CreateMunicipalityRequest request)
        {
            if (_context.Municipalities.Any(m => m.Name.ToLower().Equals(request.Name.ToLower())))
                throw new BadRequestException("A municipality with name: " + request.Name + " already exists.");

            Municipality municipality = new Municipality(request.Name, new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>());
            foreach (var item in request.YearlyTaxes)
                municipality.YearlyTaxes = InsertTaxIntoList(municipality.YearlyTaxes, item);

            foreach (var item in request.MonthlyTaxes)
                municipality.MonthlyTaxes = InsertTaxIntoList(municipality.MonthlyTaxes, item);

            foreach (var item in request.WeeklyTaxes)
                municipality.WeeklyTaxes = InsertTaxIntoList(municipality.WeeklyTaxes, item);

            foreach (var item in request.DailyTaxes)
                municipality.DailyTaxes = InsertTaxIntoList(municipality.DailyTaxes, item);

            _context.Municipalities.Add(municipality);
            _context.SaveChanges();
            return municipality;
        }

        public void DeleteMunicipality(long id)
        {
            var municipality = _context.Municipalities.Find(id);
            if (municipality == null)
                throw new NotFoundException("Municipality with Id: " + id + " has not been found");

            _context.Municipalities.Remove(municipality);
            _context.SaveChanges();

            return;
        }

        public void LoadFromFile(string fileName)
        {
            const Int32 BufferSize = 512;
            try
            {
                var fileStream = File.OpenRead(fileName);
                var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize);
                string line;
                String[] values;
                Municipality municipality;
                string header = streamReader.ReadLine();

                while ((line = streamReader.ReadLine()) != null)
                {
                    values = line.Split(';');
                    if (values.Length != 5)
                        throw new BadRequestException("Unexpected file format");
                    
                    municipality = _context.Municipalities.FirstOrDefault(m => m.Name.ToLower().Equals(values[0].ToLower()));
                    if (municipality == null)
                        municipality = CreateMunicipality(new CreateMunicipalityRequest(values[0], new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>()));

                    ScheduleTaxMunicipality(municipality.Id, new ScheduleTaxRequest(char.Parse(values[1]), float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat), DateTime.Parse(values[3]), DateTime.Parse(values[4])));
                }
            }
            catch (FileNotFoundException e) 
            {
                throw new NotFoundException(e.Message);
            }

            return;
        }

        private bool MunicipalityExists(long id)
        {
            return _context.Municipalities.Any(e => e.Id == id);
        }

        private List<Tax> InsertTaxIntoList(List<Tax> list, Tax tax) 
        {
            List<Tax> listToReturn = list ?? new List<Tax>();

            if (!listToReturn.Any(t => t.Equals(tax)))
                listToReturn.Add(tax);

            return listToReturn;
        }
    }
}
