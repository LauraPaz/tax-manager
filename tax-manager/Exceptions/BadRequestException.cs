using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager.Exceptions
{
    public class BadRequestException : Exception
    {

        public BadRequestException() : base() 
        { 
        }
        public BadRequestException(string msg) : base(msg)
        {
        }
    }
}
