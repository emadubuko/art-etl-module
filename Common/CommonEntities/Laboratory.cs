using FluentNHibernate.Mapping;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Common.CommonEntities
{
    [Serializable()]
    public class Laboratory : BaseT
    {
        [XmlElement("LaboratoryName")]
        [JsonProperty("LaboratoryName")]
        public virtual string LaboratoryName { get; set; }

        public virtual string ServicesOffered { get; set; }  

        [XmlElement("LabUniqueID")]
        [JsonProperty("LabUniqueID")]
        public virtual string LabUniqueID { get; set; }
         
        [XmlIgnore]
        [JsonIgnore]
        public virtual LGA LGA { get; set; }

        [XmlIgnore]
        [JsonIgnore]
       public virtual ImplementingPartners Organization { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual string Funder { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public virtual string Longitude { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public virtual string Latitutde { get; set; }
    }

    public class LaboratoryMap : ClassMap<Laboratory>
    {
        public LaboratoryMap()
        {
            Table("admin_laboratory");

            Id(x => x.Id);
            Map(x => x.LaboratoryName);
            Map(x => x.LabUniqueID).Not.Nullable().Unique();
            References(x => x.LGA).Column("LGA").Not.Nullable();
            References(x => x.Organization).Column("IP").Not.Nullable();
             Map(x => x.Funder); 
            Map(x => x.Longitude);
            Map(x => x.Latitutde);
            Map(x => x.ServicesOffered); 
        }
    }
}
