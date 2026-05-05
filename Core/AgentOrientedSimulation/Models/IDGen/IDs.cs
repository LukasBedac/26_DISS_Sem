using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AgentOrientedSimulation.Models.IDGen
{
    public static class IDs
    {
        private static int _doctorID = 0;
        private static int _patientID = 0;
        private static int _nurseID = 0;
        private static int _ambulanceAID = 0;
        private static int _ambulanceBID = 0;
        public static int GetDoctorID()
        {
            return ++_doctorID;
        }
        public static int GetPatientID()
        {
            return ++_patientID;
        }
        public static int GetNurseID()
        {
            return ++_nurseID;
        }
        public static int GetAmbulanceAID()
        {
            return ++_ambulanceAID;
        }
        public static int GetAmbulanceBID()
        {
            return ++_ambulanceBID;
        }
        public static void ResetIDs()
        {
            _doctorID = 0;
            _patientID = 0;
            _nurseID = 0;
            _ambulanceAID = 0;
            _ambulanceBID = 0;  
        }
    }
}
