using FluentNHibernate.Mapping;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Common.CommonEntities
{
    public class ImplementingPartners : BaseT
    { 
        [XmlElement("FacilityTypeCode")]
        [JsonProperty("FacilityTypeCode")]
        public virtual FacilityTypeFacilityTypeCode FacilityTypeCode { get; set; }

        [XmlElement("FacilityName")]
        [JsonProperty("FacilityName")]
        public virtual string Name { get; set; }
        [XmlElement("FacilityID")]
        [JsonProperty("FacilityID")]
        public virtual string ShortName { get; set; }

        public virtual string Address { get; set; }
        public virtual string MissionPartner { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual byte[] Logo { get; set; }
        public virtual string WebSite { get; set; }
        public virtual string Fax { get; set; } 
    }

    public class ImplementingPartnersMap : ClassMap<ImplementingPartners>
    {
        public ImplementingPartnersMap()
        {
            Table("admin_implementingPartners");

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.ShortName);
            Map(x => x.Address);
            Map(x => x.MissionPartner);
            Map(x => x.Logo).Length(int.MaxValue);
            Map(x => x.WebSite);
            Map(x => x.Fax);
            Map(x => x.PhoneNumber); 
        }
    }
}