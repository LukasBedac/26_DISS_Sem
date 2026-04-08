using Core.Distributions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class IDGenerator
    {
        private static int _beltID = 0;
        private static int _employeeID = 0;
        private static int _customerID = 0;
        private static int _luggageID = 0;
        public static int GetBeltID()
        {
            return _beltID++;
        }
        public static int GetEmployeeID()
        {
            return _employeeID++;
        }
        public static int GetCustomerID()
        {
            return _customerID++;
        }
        public static int GetLuggageID()
        {
            return _luggageID++;
        }
        public static void ResetIDs()
        {
            _beltID = 0;
            _employeeID = 0;
            _customerID = 0;
            _luggageID = 0;
        }
    }
}
