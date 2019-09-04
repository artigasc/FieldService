namespace FESA.SCM.FieldService.BE.ActivityBE
{
	public enum ActivityType {
		None = 99,
		//PRE
		Delay = 10,
		PreparingTrip = 0,
		Traveling = 1,
		Driving = 2,
		//DURANTE
		FieldService = 3,
		StandByClient = 4,
		FieldReport = 5,
		StandByFesa = 6,
		//POST
		InformeFinal = 7,
		DrivingReturn = 8,
		TravelingReturn = 9,
	}
}