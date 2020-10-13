using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager.Exceptions
{
    public class InternalErrorException : Exception
    {
        public InternalErrorException() : base()
        {
        }
        public InternalErrorException(string msg) : base(msg)
        {
        }
    }
}
