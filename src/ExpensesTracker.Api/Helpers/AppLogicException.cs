using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Helpers
{
    public class AppLogicException : Exception
    {
        public AppLogicException(string message) : base(message)
        {
        }
    }
}
