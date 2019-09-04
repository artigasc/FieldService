using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Web.Models
{
    public class Enumerations
    {
        public enum UserStatus
        {
            Available = 0,
            Assigned = 1,
            OnWork = 2
        }

        public enum UserType
        {
            Technician = 0,
            Supervisor = 1
        }

        public enum  ISLEAD
        {
            Technician = 0,
            Leader = 2,
            Supervisor = 1 
        }

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
            None = 99,
            PreparingTrip = 0,
            Traveling = 1,
            Driving = 2,
            FieldService = 3,
            StandByClient = 4,
            FieldReport = 5,
            StandByFesa = 6
        }

        public enum ExpenseCategory
        {
            Gas = 0,
            Food = 1,
            Supplies = 2,
            Other = 3,
        }

        public enum DocumentType
        {
            TechnicalReport = 0,
            TripExpense = 1,
            Agreement = 2,
            Sims = 3,
            SecurityDocument = 4,
            PartsGuideSigned = 5,
            VanChecking = 6
        }


       
        public enum AssignmentType
        {
            FieldService = 0,
            Scheduled = 1
        }

        public enum ContactType
        {
            Technical = 0,
            FerreyrosTeam = 1
        }

        public enum ServiceDataType
        {
            ServiceData = 0,
            MachineData = 1,
            TechnicalContact = 2,
            FesaTeam = 3
        }

        public enum ActivityState
        {
            New = -1,
            Active = 0,
            Completed = 1
        }
    }
}