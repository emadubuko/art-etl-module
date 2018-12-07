using Common.CommonEntities;
using FluentNHibernate.Mapping;

namespace ART.DAL.Entities
{
    public class PivotTable
    {
        public virtual int Id { get; set; }
        public virtual string FacilityName { get; set; }
        public virtual string State { get; set; }
        public virtual string Lga { get; set; }
        public virtual LGA TheLGA { get; set; }
        public virtual string IP { get; set; }
        public virtual string FacilityCode { get; set; }

        public virtual int TB_ART { get; set; }
        public virtual int TX_CURR { get; set; }
        public virtual int PMTCT_ART { get; set; }
        public virtual int? HTS_TST { get; set; }
        public virtual int? HTC_Only { get; set; }
        public virtual int? HTC_Only_POS { get; set; }
        public virtual int? PMTCT_STAT { get; set; }
        public virtual int? PMTCT_STAT_NEW { get; set; }
        public virtual int? PMTCT_STAT_PREV { get; set; }
        public virtual int? PMTCT_EID { get; set; }
        public virtual int? TX_NEW { get; set; }
        public virtual int OVC_Total { get; set; }
        public virtual int? PMTCT_FO { get; set; }
        public virtual int? TX_RET { get; set; }
        public virtual int? TX_PVLS { get; set; }
        public virtual int? TB_STAT { get; set; }
        public virtual int? TX_TB { get; set; }

        public virtual int? PMTCT_HEI_POS { get; set; }

        public virtual string ReportingPeriod { get; set; }
    }


    public class PivotTableMap : ClassMap<PivotTable>
    {
        public PivotTableMap()
        {
            Table("RecentPivotTable");

            Id(x => x.Id);
            Map(x => x.IP);
            Map(x => x.State);
            Map(x => x.Lga);
            Map(x => x.FacilityName);
            Map(x => x.FacilityCode);

            Map(x => x.TX_CURR);
            Map(x => x.TB_ART);
            Map(x => x.HTS_TST);
            Map(x => x.HTC_Only);
            Map(x => x.HTC_Only_POS);

            Map(x => x.PMTCT_ART);
            Map(x => x.PMTCT_STAT);
            Map(x => x.PMTCT_STAT_NEW);
            Map(x => x.PMTCT_STAT_PREV);
            Map(x => x.PMTCT_EID);
            Map(x => x.PMTCT_HEI_POS);
            Map(x => x.PMTCT_FO);

            Map(x => x.TX_NEW);
            Map(x => x.OVC_Total);

            Map(x => x.TX_RET);
            Map(x => x.TX_PVLS);
            Map(x => x.TB_STAT);
            Map(x => x.TX_TB);
            Map(x => x.ReportingPeriod);
        }
    }
}
