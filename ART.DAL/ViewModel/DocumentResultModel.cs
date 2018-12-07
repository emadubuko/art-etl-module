using System;
using System.Collections.Generic;

namespace ART.DAL.ViewModels
{
    public class DocumentResultModel
    {
        public int Id { get; set; }
        public string MessageUniqueID { get; set; }
        public string PatientIdentifier { get; set; }
        public string MessageSender { get; set; }
        public string TreatmentFacility { get; set; }
        public string DatimFacilityCode { get; set; } 
        public int DT_RowId { get; set; }
        public string BatchNumber { get; set; }
        public string FirstColumn { get; set; }
        public string LastColumn { get; set; } 
        public int FacilityId { get; set; }
        public DateTime? MessageCreationDateTime { get; set; }
    }

    public class DocumentSearchModel
    {
        public IEnumerable<string> MessageSenders { get; set; }
        public IEnumerable<string> FacilityDatimCodes { get; set; }
        public string BatchNumber { get; set; }
        public IEnumerable<string> PatientIds { get; set; }
        public IEnumerable<int> FacilityIds { get; set; }
        public bool ReturnAll { get; set; }
        public bool ContainerIdOnly { get; set; }

    }
}
