using FluentNHibernate.Mapping;

namespace Common.CommonEntities
{
    public class IPFacility
    {
        public virtual int Id { get; set; }
        public virtual ImplementingPartners IP { get; set; }
        public virtual OnboardedFacility Facility { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class IPFacilityMap : ClassMap<IPFacility>
    {
        public IPFacilityMap()
        {
            Table("admin_ipfacility");

            Id(x => x.Id);
            References(x => x.Facility).Column("FacilityId").Not.Nullable();
            References(x => x.IP).Column("IP").Not.Nullable();
            Map(x => x.IsActive);
        }
    }
}
