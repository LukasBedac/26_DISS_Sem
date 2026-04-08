using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSimulation.Airport.Models.Enums
{
    public enum Status
    {
        [Description("Arrived")]
        ARRIVED,
        [Description("At customer")]
        AT_CUSTOMER,
        [Description("In queue")]
        IN_QUEUE,
        [Description("Storing luggage")]
        LUGGAGE_STORE,
        [Description("In Metal Detector scanning")]
        DETECTOR_SCANNING,
        [Description("Personal check")]
        PERSONAL_CHECK,
        [Description("In Rontgen Detector scanning")]
        RONTGEN_SCANNING,
        [Description("At Output belt")]
        AT_OUTPUT_BELT,
        [Description("At luggage pickup")]
        LUGGAGE_PICKUP,
        [Description("Checked")]
        CHECKED
    }
}
