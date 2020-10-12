using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager.Repositories
{
    public interface IMunicipalityRespository
    {
        public Municipality GetTest();
        public List<Municipality> GetMunicipalities();
        public Municipality GetMunicipality(long id);
        public Municipality PutMunicipality(long id, Municipality municipality);
        public Municipality ScheduleTaxMunicipality(long id, ScheduleTaxRequest scheduleTaxRequest);
        public Municipality PostMunicipality(Municipality municipality);
        public void DeleteMunicipality(long id);
    }
}
