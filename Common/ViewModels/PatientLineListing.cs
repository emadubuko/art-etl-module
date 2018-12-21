using System;

namespace Common.ViewModels
{
    public class PatientLineListing
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string State_Code { get; set; }
        public string LGA { get; set; }
        public string Lga_code { get; set; }
        public string Facility { get; set; }
        public int FacilityId { get; set; }
        public bool? GSMFacility { get; set; }

        public string PatientIdentifier { get; set; }

        public string HospitalNo { get; set; }
        public string Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int? AgeAtARTInitiation { get; set; }

        public int? CurrentAge { get; set; }

        public DateTime? ARTStartDate { get; set; }

        public DateTime? LastDrugPickupDate { get; set; }
        public DateTime? LastClinicVisitDate { get; set; }
        public int? DaysOfARVRefill { get; set; }
        public string PregnancyStatus { get; set; }
        public string CurrentViralLoad { get; set; }
        public DateTime? DateOfCurrentViralLoad { get; set; }
        public string CurrentStatus { get; set; }
        public string _12monthStatus { get; set; }
        public string _24monthStatus { get; set; }
        public string _36monthStatus { get; set; }
        public string DatimCode { get;  set; }
    }
}


