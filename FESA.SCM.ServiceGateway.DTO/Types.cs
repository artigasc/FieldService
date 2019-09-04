namespace FESA.SCM.ServiceGateway.DTO
{
    public enum AssignmentStatus
    {
        New = 0,
        Hold = 1,
        Active = 2,
        Complete = 3,
        Declined = 9999,
    }
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

	public enum AssignmentType
    {
        FieldService = 0,
        Scheduled = 1
    }

    public enum ActivityState
    {
        Active = 0,
        Completed = 1
    }

    public enum UserStatus
    {
        Available = 0,
        Assigned = 1,
        OnWork = 2
    }

    public enum UserType
    {
        Technician = 0,
        Supervisor = 1,
        Lead = 2
    }
}