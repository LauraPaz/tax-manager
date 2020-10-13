using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager.Repositories
{
    public interface IMunicipalityRespository
    {
        public List<Municipality> GetMunicipalities();
        public Municipality GetMunicipality(long id);
        public float GetTaxInfo(string name, DateTime date);
        public Municipality UpdateMunicipality(long id, UpdateMunicipalityRequest request);
        public Municipality ScheduleTaxMunicipality(long id, ScheduleTaxRequest request);
        public Municipality CreateMunicipality(Municipality municipality);
        public void DeleteMunicipality(long id);
        public void LoadFromFile(string fileName);
    }
}
