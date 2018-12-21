using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ViewModels
{
    public class MacroReport
    {
        public virtual string State { get; set; }
        public virtual string LGA { get; set; }
        public virtual string Facility { get; set; }
        public virtual int FacilityId { get; set; }
        public virtual string IP { get; set; }

        public virtual DateTime? LastEMRUpdatedDate { get; set; }
        public virtual DateTime? LastNDRUpdatedDate { get; set; }
        public virtual string Patients { get; set; }
        public virtual string ActivePatient { get; set; }


        public virtual string Last30days { get; set; }
        public virtual string patEnc30 { get; set; }
        public virtual string patEnc90 { get; set; }
        public virtual string HIVEncounter { get; set; }
        public virtual string Lab_Reports { get; set; }
        public virtual string ViralLoad { get; set; }
        public virtual string Regimens { get; set; }
        public virtual string Enrolled { get; set; }
        public virtual string enc90 { get; set; }
        public virtual string AVGLab { get; set; }
        public virtual string AVGReg { get; set; }
        public virtual string AVGEnc { get; set; }

        public virtual string NewOnART { get; set; }
        public bool? GSMFacility { get; set; }
    }
}
