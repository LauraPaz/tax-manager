using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }
        public NotFoundException(string msg) : base(msg)
        {
        }
    }
}
