using OSPABA;

namespace Simulation
{
	public class Mc : OSPABA.IdList
	{
		//meta! userInfo="Generated code: do not modify", tag="begin"
		public const int FreeDoctor = 1019;
		public const int FreeNurse = 1020;
		public const int FreeAmbulance = 1021;
		public const int RequestResource = 1026;
		public const int PatientArrival = 1001;
		public const int PatientDeparture = 1002;
		public const int PatientExamination = 1003;
		public const int FreeResource = 1027;
		public const int RequestAmbulanceAllocationA = 1004;
		public const int RequestAmbulanceAllocationB = 1005;
		public const int RequestNurseAllocation = 1007;
		public const int RequestDoctorAllocation = 1008;
		public const int InitalExamination = 1013;
		public const int Examination = 1014;
		//meta! tag="end"

		// 1..1000 range reserved for user
	}
}