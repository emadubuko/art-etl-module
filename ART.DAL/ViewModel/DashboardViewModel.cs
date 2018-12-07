using Common.CommonEntities;
using System;
using System.Collections.Generic;

namespace ART.DAL.ViewModels
{
    public class DashBoardFilter
    {
        public List<IPLGAFacility> IPLocation { get; set; }
        public bool AllowCriteria { get; set; }
        public List<int> Year { get; set; }
        public List<string> Months { get; set; }
    }

    public class IPLGAFacility
    {
        public string IP { get; set; }
        public string FacilityName { get; set; }
        public string DatimFacilityCode { get; set; }
        public LGA LGA { get; set; }
    }

    public class DataSearchModel
    {
        public List<string> IPs { get; set; }
        public List<string> lga_codes { get; set; }
        public List<string> state_codes { get; set; }
        public List<string> facilities { get; set; }

    }

    public class TX_Curr_By_Month_Model
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string State_Code { get; set; }
        public string LGA { get; set; }
        public string Lga_code { get; set; }
        public string Facility { get; set; }
        public int FacilityId { get; set; }
        public int Month11 { get; set; }
        public int Month10 { get; set; }
        public int Month9 { get; set; }
        public int Month8 { get; set; }
        public int Month7 { get; set; }
        public int Month6 { get; set; }
        public int Month5 { get; set; }
        public int Month4 { get; set; }
        public int Month3 { get; set; }
        public int Month2 { get; set; }
        public int Month1 { get; set; }
        public int Month0 { get; set; }
        public DateTime CachedDate { get; set; }
        public bool? GSMFacility { get; set; }
    }

    public class Twelve_Month_Cohort_Retention_by_month_Model
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string State_Code { get; set; }
        public string LGA { get; set; }
        public string Lga_code { get; set; }
        public string Facility { get; set; }
        public int FacilityId { get; set; }
        public string MonthCohort { get; set; }
        public int NumberEnroled { get; set; }
        public int NumberRetained { get; set; }
        public DateTime CachedDate { get; set; }
        public bool? GSMFacility { get; set; }
    }

    public class Twelve_Month_Cohort_Retention_Trend_Model
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string State_Code { get; set; }
        public string LGA { get; set; }
        public string Lga_code { get; set; }
        public string Facility { get; set; }
        public bool? GSMFacility { get; set; }
        public int FacilityId { get; set; }
        public string MonthCohort { get; set; }
        public int NumberEnroled { get; set; }
        public int Month24_after_Start_date { get; set; }
        public int Month23_after_Start_date { get; set; }
        public int Month22_after_Start_date { get; set; }
        public int Month21_after_Start_date { get; set; }
        public int Month20_after_Start_date { get; set; }
        public int Month19_after_Start_date { get; set; }
        public int Month18_after_Start_date { get; set; }
        public int Month17_after_Start_date { get; set; }
        public int Month16_after_Start_date { get; set; }
        public int Month15_after_Start_date { get; set; }
        public int Month14_after_Start_date { get; set; }
        public int Month13_after_Start_date { get; set; }
        public int Month12_after_Start_date { get; set; }
        public int Month11_after_Start_date { get; set; }
        public int Month10_after_Start_date { get; set; }
        public int Month9_after_Start_date { get; set; }
        public int Month8_after_Start_date { get; set; }
        public int Month7_after_Start_date { get; set; }
        public int Month6_after_Start_date { get; set; }
        public int Month5_after_Start_date { get; set; }
        public int Month4_after_Start_date { get; set; }
        public int Month3_after_Start_date { get; set; }
        public int Month2_after_Start_date { get; set; }
        public int Month1_after_Start_date { get; set; }
        public DateTime CachedDate { get; set; }
        
    }

    public class PVLS_Eligibility_by_Months_Model
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string State_Code { get; set; }
        public string LGA { get; set; }
        public string Lga_code { get; set; }
        public string Facility { get; set; }
        public int FacilityId { get; set; }
        public string MonthCohort { get; set; }
        public int NumberElligible { get; set; }
        public int NumberTested { get; set; }
        public int NumberSuppressed { get; set; }
        public DateTime CachedDate { get; set; }
        public bool? GSMFacility { get; set; }
    }


    public class DataTriangulationPage
    {
        public List<IPLGAFacility> IPLocation { get; set; }
        public List<NDR_DATIM_DHIS2_Model> TraingulatedData { get; set; }
    }

    public class NDR_DATIM_DHIS2_Model
    {
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }
        public string IP { get; set; }
        public int DATIM_TX_CURR { get; set; }
        public int DATIM_TX_NEW { get; set; }
        public int NDR_TX_CURR { get; set; }
        public int NDR_TX_NEW { get; set; }
        public int DHIS_TX_CURR { get; set; }
        public int DHIS_TX_NEW { get; set; }

        public bool? GSM { get; set; }
        public int Tx_New_difference
        {
            get
            {
                return DATIM_TX_NEW - NDR_TX_NEW;
            }
        }

        public double Tx_New_Concurrence
        {
            get
            {
                if (NDR_TX_NEW == 0 && DATIM_TX_NEW == 0)
                    return 100;
                else if (DATIM_TX_NEW == 0)
                    return 0;
                else
                    return 100 * (1.0 * NDR_TX_NEW) / DATIM_TX_NEW;
            }
        }

        //this is percentage difference
        public double Tx_New_percentage_difference
        {
            get
            {
                if (NDR_TX_NEW == 0) return 100;
                return 100 * (1.0 * NDR_TX_NEW - DATIM_TX_NEW) / NDR_TX_NEW;
            }
        }

        public double Tx_Curr_difference
        {
            get
            {
                return (DATIM_TX_CURR - NDR_TX_CURR);
            }
        }

        public double Tx_Curr_Concurrence
        {
            get
            {
                if (NDR_TX_CURR == 0 && DATIM_TX_CURR == 0)
                    return 100;
                else if (DATIM_TX_CURR == 0)
                    return 0;
                else
                    return 100 * (1.0 * NDR_TX_CURR) / DATIM_TX_CURR;
            }
        }

        //this is actually percentage difference
        public double Tx_Curr_percentage_difference
        {
            get
            {
                if (NDR_TX_CURR == 0) return 100;
                return 100 * (1.0 * NDR_TX_CURR - DATIM_TX_CURR) / NDR_TX_CURR;
            }
        }
    }

    public class FacilityCompletenessModel
    {
        public int FacilityId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string PatientIdentifier { get; set; }
        public bool? GSMFacility { get; set; }
    }

    //public class PivotTableModel
    //{
    //    public virtual int Id { get; set; }
    //    public virtual string FacilityName { get; set; }
    //    public virtual string State { get; set; }
    //    public virtual string Lga { get; set; }
    //    public virtual Entities.LGA TheLGA { get; set; }
    //    public virtual string IP { get; set; }
    //    public virtual string FacilityCode { get; set; }

    //    public virtual int TB_ART { get; set; }
    //    public virtual int TX_CURR { get; set; }
    //    public virtual int PMTCT_ART { get; set; }
    //    public virtual int? HTS_TST { get; set; }
    //    public virtual int? HTC_Only { get; set; }
    //    public virtual int? HTC_Only_POS { get; set; }
    //    public virtual int? PMTCT_STAT { get; set; }
    //    public virtual int? PMTCT_STAT_NEW { get; set; }
    //    public virtual int? PMTCT_STAT_PREV { get; set; }
    //    public virtual int? PMTCT_EID { get; set; }
    //    public virtual int? TX_NEW { get; set; }
    //    public virtual int OVC_Total { get; set; }
    //    public virtual int? PMTCT_FO { get; set; }
    //    public virtual int? TX_RET { get; set; }
    //    public virtual int? TX_PVLS { get; set; }
    //    public virtual int? TB_STAT { get; set; }
    //    public virtual int? TX_TB { get; set; }

    //    public virtual int? PMTCT_HEI_POS { get; set; }

    //    public virtual string ReportingPeriod { get; set; }
    //}


    //public class PivotTableModelMap : ClassMap<PivotTableModel>
    //{
    //    public PivotTableModelMap()
    //    {
    //        Table("RecentPivotTable");

    //        Id(x => x.Id);
    //        Map(x => x.IP);
    //        Map(x => x.State);
    //        Map(x => x.Lga);
    //        Map(x => x.FacilityName);
    //        Map(x => x.FacilityCode);

    //        Map(x => x.TX_CURR);
    //        Map(x => x.TB_ART);
    //        Map(x => x.HTS_TST);
    //        Map(x => x.HTC_Only);
    //        Map(x => x.HTC_Only_POS);

    //        Map(x => x.PMTCT_ART);
    //        Map(x => x.PMTCT_STAT);
    //        Map(x => x.PMTCT_STAT_NEW);
    //        Map(x => x.PMTCT_STAT_PREV);
    //        Map(x => x.PMTCT_EID);
    //        Map(x => x.PMTCT_HEI_POS);
    //        Map(x => x.PMTCT_FO);

    //        Map(x => x.TX_NEW);
    //        Map(x => x.OVC_Total);

    //        Map(x => x.TX_RET);
    //        Map(x => x.TX_PVLS);
    //        Map(x => x.TB_STAT);
    //        Map(x => x.TX_TB);
    //        Map(x => x.ReportingPeriod);
    //    }
    //}


}
