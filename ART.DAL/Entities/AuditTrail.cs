using Common.CommonEntities;
using FluentNHibernate.Mapping;
using System;

namespace ART.DAL.Entities
{
    public class AuditTrail  
    { 
        public virtual long Id { get; set; }
        public virtual OnboardedFacility Facility { get; set; }
        public virtual string PatientIdentifier { get; set; }
        public virtual string AuditType { get; set; }
        public virtual string FileName { get; set; }
        public virtual DateTime DateUploaded { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Source { get; set; }
        public virtual string BatchFileUri { get; set; }

        public virtual UserProfile UserProfileId { get; set; }
     }

    public class Trackable
    {
        public string FileAddress { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UserId { get; set; }
        public string Source { get; set; }
    }

    public class AuditTrailMap : ClassMap<AuditTrail>
    {
        public AuditTrailMap()
        {
            Table("admin_audit_trail");

            Id(x => x.Id);
            References(x => x.Facility).Column("FacilityId");
            Map(x => x.PatientIdentifier);
            Map(x => x.AuditType);
            Map(x => x.FileName);
            Map(x => x.DateUploaded);
            Map(x => x.UserId);
            Map(x => x.Source);
            Map(x => x.BatchFileUri);
            References(x => x.UserProfileId).Column("UserprofileId");
        }
    }
}
