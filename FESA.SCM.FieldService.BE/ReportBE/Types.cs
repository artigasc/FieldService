namespace FESA.SCM.FieldService.BE.ReportBE
{
    public enum AssignmentStatus
    {
        New = 0,
        Hold = 1,
        Active = 2,
        Complete = 3,
        Declined = 9999,
    }
    public enum ActivityType
    {
        PreparingTrip = 0,
        Traveling = 1,
        Driving = 2,
        FieldService = 3,
        StandByClient = 4,
        FieldReport = 5,
        StandByFesa = 6
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
