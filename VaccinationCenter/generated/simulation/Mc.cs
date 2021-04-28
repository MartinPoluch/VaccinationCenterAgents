using OSPABA;
namespace simulation
{
	public class Mc : IdList
	{
		//meta! userInfo="Generated code: do not modify", tag="begin"
		public const int EndOfMove = 1038;
		public const int PatientArrival = 1001;
		public const int PatientEnterCenter = 1002;
		public const int Refill = 1039;
		public const int Registration = 1003;
		public const int Examination = 1004;
		public const int Vaccination = 1005;
		public const int Waiting = 1006;
		public const int AdminStartBreak = 1007;
		public const int EndOfNurseMove = 1040;
		public const int DoctorStartBreak = 1008;
		public const int NurseStartBreak = 1009;
		public const int LunchBreak = 1010;
		public const int EndOfRefill = 1041;
		public const int NewArrival = 1011;
		public const int EndOfTravel = 1012;
		public const int EndOfLunch = 1014;
		public const int PatientLeftCenter = 1015;
		public const int PatientExit = 1016;
		public const int AdminEndBreak = 1017;
		public const int DoctorEndBreak = 1018;
		public const int NurseEndBreak = 1020;
		public const int RegistrationProcessEnd = 1021;
		public const int AdminLunchBreak = 1022;
		public const int ExaminationProcessEnd = 1023;
		public const int DoctorLunchBreak = 1024;
		public const int NurseLunchBreak = 1025;
		public const int VaccinationProcessEnd = 1026;
		public const int MoveToAnotherRoom = 1029;
		public const int EndOfWaiting = 1031;
		//meta! tag="end"

		// 1..1000 range reserved for user
		public const int Initialization = 1;
		public const int OneRefillDone = 2; // nurse refilled one vaccine
	}
}