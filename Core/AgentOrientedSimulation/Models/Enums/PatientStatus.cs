using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AgentOrientedSimulation.Models.Enums
{
    public enum PatientStatus
    {
        Arrived,
        WaitingForInitialExamination,
        InitialExamination,
        WaitingForExamination,
        Examination,
        TransferBetweenAmbulances,
        Leaving
    }
}
