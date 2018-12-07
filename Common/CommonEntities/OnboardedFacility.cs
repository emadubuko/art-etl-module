using FluentNHibernate.Mapping;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Common.CommonEntities
{
    [Serializable()]
    public class OnboardedFacility : BaseT
    {
        [XmlElement("FacilityName")]
        [JsonProperty("FacilityName")]
        public virtual string FacilityName { get; set; }
         
        public virtual string ServicesOffered { get; set; }
        public virtual string NationalID { get; set; }
        public virtual string DatimFacilityCode { get; set; }


        [XmlElement("FacilityID")]
        [JsonProperty("FacilityID")]
        public virtual string FacilityID { get; set; }

        [XmlElement("FacilityTypeCode")]
        [JsonProperty("FacilityTypeCode")]
        public virtual FacilityTypeFacilityTypeCode FacilityTypeCode { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual LGA LGA { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual string EMRSystem { get; set; }
        public virtual bool DoesEMRSupportPMTCT { get; set; }
        public virtual bool DoesEMRSupportHTS { get; set; }
        public virtual bool OptionBAvailable { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual string Funder { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public virtual string Longitude { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public virtual string Latitutde { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual bool GranularSite { get; set; }
    }

    public class OnboardedFacilityMap : ClassMap<OnboardedFacility>
    {
        public OnboardedFacilityMap()
        {
            Table("admin_onboardedfacility");
            Id(x => x.Id);
            Map(x => x.FacilityName);
            Map(x => x.FacilityID).Not.Nullable();
            References(x => x.LGA).Column("LGA").Not.Nullable();
            Map(x => x.FacilityTypeCode); 
            Map(x => x.Funder);
            Map(x => x.EMRSystem);
            Map(x => x.Longitude);
            Map(x => x.Latitutde);
            Map(x => x.ServicesOffered);
            Map(x => x.NationalID);
            Map(x => x.DatimFacilityCode).Unique().Not.Nullable(); 
            Map(x => x.DoesEMRSupportPMTCT);
            Map(x => x.DoesEMRSupportHTS);
            Map(x => x.OptionBAvailable);
            Map(x => x.GranularSite);
        }
    }
}
