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
        private static int _crateID = 0;
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
        public static int GetCrateID()
        {
            return _crateID++;
        }
    }
}
