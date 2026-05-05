using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AgentOrientedSimulation.Models.DTO
{
    public static class DTOManager
    {
        public static DoctorDTO MapToDoctorDTO(Doctor doctor)
        {
            return new DoctorDTO
            {
                ID = doctor.ID,
                Status = !doctor.IsBusy ? "Free" : "Busy",
                PatientAssigned = doctor.AssignedPatient != null ? $"Patient {doctor.AssignedPatient.ID}" : "-"
            };
        }

        public static NurseDTO MapToNurseDTO(Nurse nurse)
        {
            return new NurseDTO
            {
                ID = nurse.ID,
                Status = !nurse.IsBusy ? "Free" : "Busy",
                PatientAssigned = nurse.AssignedPatient != null ? $"Patient {nurse.AssignedPatient.ID}" : "-"
            };
        }

        public static AmbulanceADTO MapToAmbulanceADTO(AmbulanceA ambulanceA)
        {
            return new AmbulanceADTO
            {
                ID = ambulanceA.ID,
                Status = !ambulanceA.IsBusy ? "Free" : "Busy",
                PatientAssigned = ambulanceA.AssignedPatient != null ? $"Patient {ambulanceA.AssignedPatient.ID}" : "-"
            };
        }

        public static AmbulanceBDTO MapToAmbulanceBDTO(AmbulanceB ambulanceB)
        {
            return new AmbulanceBDTO
            {
                ID = ambulanceB.ID,
                Status = !ambulanceB.IsBusy ? "Free" : "Busy",
                PatientAssigned = ambulanceB.AssignedPatient != null ? $"Patient {ambulanceB.AssignedPatient.ID}" : "-"
            };
        }
    }
}
