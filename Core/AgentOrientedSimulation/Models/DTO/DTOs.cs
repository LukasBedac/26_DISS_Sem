using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AgentOrientedSimulation.Models.DTO
{
    public class DoctorDTO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string PatientAssigned { get; set; }
    }

    public class NurseDTO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string PatientAssigned { get; set; }
    }

    public class AmbulanceADTO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string PatientAssigned { get; set; }
    }

    public class AmbulanceBDTO
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string PatientAssigned { get; set; }
    }
}
