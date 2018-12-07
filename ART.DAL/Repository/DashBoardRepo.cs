using ART.DAL.ViewModels;
using Common.DBFasade;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ART.DAL.Repository
{

    /*
    public class DashBoardRepo
    {
        public List<MacroReport> GenerateMacroReportForDisplay(string IPShortName)
        {
            List<Models.MacroReport> report = new List<Models.MacroReport>();

            var cmd = new SqlCommand();
            string sql = "select * from [dbo].[temp_DashBoardSummaryReport]";
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }

            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            foreach (DataRow dr in table.Rows)
            {
                var m = new Models.MacroReport
                {
                    State = dr.Field<string>("State"),
                    LGA = dr.Field<string>("LGA"),
                    FacilityName = dr.Field<string>("Facility"),
                    LastNDRUpdatedDate = dr.Field<DateTime?>("LastNDRUpdatedDate"),
                    Patients = dr.Field<string>("Patients"),
                    ActivePatient = dr.Field<string>("ActivePatient"),
                    FacilityId = dr.Field<int>("FacilityId"),
                    GSMFacility = dr.Field<bool?>("GSMFacility"),
                    IPShortName = dr.Field<string>("IP"),
                    Regimen = dr.Field<string>("Regimens"),
                    LabReports = dr.Field<string>("Lab_Reports"),
                    HIVEncounters = dr.Field<string>("HIVEncounter"),
                    LastEMRUpdatedDate = dr.Field<DateTime?>("LastEMRUpdatedDate"),
                };
                report.Add(m);
            }
            return report;
        }

        public List<TX_Curr_By_Month_Model> Get_TX_CURR_By_Month(string IPShortName)
        {
            var cmd = new SqlCommand();
            string sql = "select * from [dbo].[temp_TX_CURR_by_month]";
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }
            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<TX_Curr_By_Month_Model> model = new List<TX_Curr_By_Month_Model>();
            foreach (DataRow row in table.Rows)
            {

                model.Add(new TX_Curr_By_Month_Model
                {
                    Facility = row.Field<string>("Facility"),
                    IP = row.Field<string>("IP"),
                    FacilityId = row.Field<int>("FacilityId"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    State = row.Field<string>("State"),
                    State_Code = row.Field<string>("State_Code"),
                    LGA = row.Field<string>("LGA"),
                    Lga_code = row.Field<string>("Lga_code"),
                    Month0 = row.Field<int>("0"),
                    Month1 = row.Field<int>("1"),
                    Month2 = row.Field<int>("2"),
                    Month3 = row.Field<int>("3"),
                    Month4 = row.Field<int>("4"),
                    Month5 = row.Field<int>("5"),
                    Month6 = row.Field<int>("6"),
                    Month7 = row.Field<int>("7"),
                    Month8 = row.Field<int>("8"),
                    Month9 = row.Field<int>("9"),
                    Month10 = row.Field<int>("10"),
                    Month11 = row.Field<int>("11"),
                    CachedDate = row.Field<DateTime>("CachedDate")
                });
            }
            return model;
        }

        public List<FacilityCompletenessModel> GetCompletenessReportFacility(string IPShortName)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "Select * from [dbo].[temp_FacilityReportingCompleteness]";
            //cmd.CommandType = CommandType.StoredProcedure;

            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<FacilityCompletenessModel> model = new List<FacilityCompletenessModel>();
            foreach (DataRow row in table.Rows)
            {

                model.Add(new FacilityCompletenessModel
                {
                    FacilityId = row.Field<int>("FacilityId"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    DateUploaded = row.Field<DateTime>("LastUploadDate"),
                });
            }
            return model;
        }

        public List<FacilityCompletenessModel> GetCompletenessReportPatient(string IPShortName)
        {
            var cmd = new SqlCommand("Select * from [dbo].[temp_PatientReportingCompleteness]");
            //cmd.CommandText = "[dbo].[sp_get_PatientReportingCompleteness]";
            //cmd.CommandType = CommandType.StoredProcedure;

            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<FacilityCompletenessModel> model = new List<FacilityCompletenessModel>();
            foreach (DataRow row in table.Rows)
            {
                model.Add(new FacilityCompletenessModel
                {
                    FacilityId = row.Field<int>("FacilityId"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    PatientIdentifier = row.Field<string>("PatientIdentifier"),
                    DateUploaded = row.Field<DateTime>("LastUploadDate"),
                });
            }
            return model;
        }

        public List<Twelve_Month_Cohort_Retention_by_month_Model> Get_12_Month_Cohort_Retention_by_month(string IPShortName)
        {
            var cmd = new SqlCommand();
            string sql = "select * from [dbo].[temp_12_Month_Cohort_Retention_by_month]";
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }
            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<Twelve_Month_Cohort_Retention_by_month_Model> model = new List<Twelve_Month_Cohort_Retention_by_month_Model>();
            foreach (DataRow row in table.Rows)
            {

                model.Add(new Twelve_Month_Cohort_Retention_by_month_Model
                {
                    Facility = row.Field<string>("Facility"),
                    IP = row.Field<string>("IP"),
                    FacilityId = row.Field<int>("FacilityId"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    State = row.Field<string>("State"),
                    State_Code = row.Field<string>("State_Code"),
                    LGA = row.Field<string>("LGA"),
                    Lga_code = row.Field<string>("Lga_code"),
                    MonthCohort = row.Field<string>("MonthCohort"),
                    NumberEnroled = row.Field<int>("NumberEnroled"),
                    NumberRetained = row.Field<int>("NumberRetained"),
                    CachedDate = row.Field<DateTime>("CachedDate")
                });
            }
            return model;
        }

        public List<PVLS_Eligibility_by_Months_Model> Get_PVLS_Eligibility_by_Months(string IPShortName, bool Returning)
        {
            string tablename = Returning ? "[dbo].[temp_PVLS_Returning_Eligible_by_Months]" : "[dbo].[temp_PVLS_Newly_Eligible_by_Months]";
            var cmd = new SqlCommand();
            string sql = "select * from " + tablename;
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }
            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<PVLS_Eligibility_by_Months_Model> model = new List<PVLS_Eligibility_by_Months_Model>();
            foreach (DataRow row in table.Rows)
            {
                model.Add(new PVLS_Eligibility_by_Months_Model
                {
                    Facility = row.Field<string>("Facility"),
                    IP = row.Field<string>("IP"),
                    FacilityId = row.Field<int>("FacilityId"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    State = row.Field<string>("State"),
                    State_Code = row.Field<string>("State_Code"),
                    LGA = row.Field<string>("LGA"),
                    Lga_code = row.Field<string>("Lga_code"),
                    MonthCohort = row.Field<string>("MonthCohort"),
                    NumberElligible = row.Field<int>("NumberElligible"),
                    NumberTested = row.Field<int>("NumberTested"),
                    NumberSuppressed = row.Field<int>("NumberSuppressed"),
                    CachedDate = row.Field<DateTime>("CachedDate")
                });
            }
            return model;
        }

        public List<Twelve_Month_Cohort_Retention_Trend_Model> Get_12_Month_Cohort_Retention_Trend(string IPShortName)
        {
            var cmd = new SqlCommand();
            string sql = "select * from [dbo].[temp_12_Month_Cohort_Retention_Trend]";
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }

            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<Twelve_Month_Cohort_Retention_Trend_Model> model = new List<Twelve_Month_Cohort_Retention_Trend_Model>();
            foreach (DataRow row in table.Rows)
            {

                model.Add(new Twelve_Month_Cohort_Retention_Trend_Model
                {
                    Facility = row.Field<string>("Facility"),
                    IP = row.Field<string>("IP"),
                    GSMFacility = row.Field<bool?>("GSMFacility"),
                    FacilityId = row.Field<int>("FacilityId"),
                    State = row.Field<string>("State"),
                    State_Code = row.Field<string>("State_Code"),
                    LGA = row.Field<string>("LGA"),
                    Lga_code = row.Field<string>("Lga_code"),
                    MonthCohort = row.Field<string>("MonthCohort"),
                    NumberEnroled = row.Field<int>("NumberEnroled"),
                    Month24_after_Start_date = row.Field<int>("mth24_after_Start_date"),
                    Month23_after_Start_date = row.Field<int>("mth23_after_Start_date"),
                    Month22_after_Start_date = row.Field<int>("mth22_after_Start_date"),
                    Month21_after_Start_date = row.Field<int>("mth21_after_Start_date"),
                    Month20_after_Start_date = row.Field<int>("mth20_after_Start_date"),
                    Month19_after_Start_date = row.Field<int>("mth19_after_Start_date"),
                    Month18_after_Start_date = row.Field<int>("mth18_after_Start_date"),
                    Month17_after_Start_date = row.Field<int>("mth17_after_Start_date"),
                    Month16_after_Start_date = row.Field<int>("mth16_after_Start_date"),
                    Month15_after_Start_date = row.Field<int>("mth15_after_Start_date"),
                    Month14_after_Start_date = row.Field<int>("mth14_after_Start_date"),
                    Month13_after_Start_date = row.Field<int>("mth13_after_Start_date"),
                    Month12_after_Start_date = row.Field<int>("mth12_after_Start_date"),
                    Month11_after_Start_date = row.Field<int>("mth11_after_Start_date"),
                    Month10_after_Start_date = row.Field<int>("mth10_after_Start_date"),
                    Month9_after_Start_date = row.Field<int>("mth9_after_Start_date"),
                    Month8_after_Start_date = row.Field<int>("mth8_after_Start_date"),
                    Month7_after_Start_date = row.Field<int>("mth7_after_Start_date"),
                    Month6_after_Start_date = row.Field<int>("mth6_after_Start_date"),
                    Month5_after_Start_date = row.Field<int>("mth5_after_Start_date"),
                    Month4_after_Start_date = row.Field<int>("mth4_after_Start_date"),
                    Month3_after_Start_date = row.Field<int>("mth3_after_Start_date"),
                    Month2_after_Start_date = row.Field<int>("mth2_after_Start_date"),
                    Month1_after_Start_date = row.Field<int>("mth1_after_Start_date"),
                    CachedDate = row.Field<DateTime>("CachedDate")
                });
            }
            return model;
        }

        public Dictionary<string, List<PatientLineListing>> GetPatientLineListing(string IPShortName)
        {
            Dictionary<string, List<PatientLineListing>> patientlineDictionary = new Dictionary<string, List<PatientLineListing>>();
            var cmd = new SqlCommand();

            string sql = "select * from [dbo].[temp_patientlinelisting]";
            if (!string.IsNullOrEmpty(IPShortName))
            {
                sql += " where IP= @ip";
                cmd.CommandText = sql;
                var param = new SqlParameter("ip", SqlDbType.VarChar);
                param.Value = IPShortName;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.CommandText = sql;
            }


            var data = new Utils().RetrieveAsDataTable(cmd);
            if (data.Rows == null || data.Rows.Count == 0)
                return null;

            foreach (DataRow row in data.Rows)
            {
                string IP = row.Field<string>("IP");// Convert.ToString(row[0]);
                string State = row.Field<string>("State");// Convert.ToString(row[1]);
                string State_Code = row.Field<string>("State_Code");// Convert.ToString(row[2]);
                string LGA = row.Field<string>("LGA");// Convert.ToString(row[3]);
                string Lga_code = row.Field<string>("Lga_Code");//  Convert.ToString(row[4]);
                string Facility = row.Field<string>("Facility");//  Convert.ToString(row[5]);
                int FacilityId = row.Field<int>("FacilityId");// Convert.ToInt32(row[6]);
                bool? GSMFacility = row.Field<bool?>("GSMFacility");
                string DatimCode = row.Field<string>("DatimCode");//  Convert.ToString(row[7]);
                string PatientIdentifier = row.Field<string>("PatientIdentifier");// Convert.ToString(row[8]);
                string HospitalNo = row.Field<string>("Hospital No");//Convert.ToString(row[9]);
                string Sex = row.Field<string>("Sex");//Convert.ToString(row[10]);
                DateTime? DateOfBirth = row.Field<DateTime?>("Date Of Birth"); // row[10] != null ? Convert.ToDateTime(row[10]) : (DateTime?)DBNull.Value,

                int? AgeAtARTInitiation = row.Field<int?>("Age at ART Initiation"); // row[12] is DBNull ? (int?)null : Convert.ToInt32(row[12]);
                int? CurrentAge = row.Field<int?>("Current Age"); //row[13] is DBNull ? (int?)null : Convert.ToInt32(row[13]);

                DateTime? ARTStartDate = row.Field<DateTime?>("ART Start Date"); // row[13] != null ? Convert.ToDateTime(row[13]) : (DateTime?)DBNull,
                DateTime? LastDrugPickupDate = row.Field<DateTime?>("Last Drug Pickup date"); // row[14] != null ? Convert.ToDateTime(row[14]) : (DateTime?)DBNull,
                DateTime? LastClinicVisitDate = row.Field<DateTime?>("Last clinic visit date"); // row[15] != null ? Convert.ToDateTime(row[15]) : (DateTime?)DBNull,

                int? DaysOfARVRefill = row.Field<int?>("Days Of ARV Refill"); // row[17] is DBNull ? (int?)null: Convert.ToInt32(row[17]);

                string PregnancyStatus = row.Field<string>("Pregnancy Status"); //Convert.ToString(row[18]);
                string CurrentViralLoad = row.Field<string>("Current Viral Load"); //Convert.ToString(row[19]);
                DateTime? DateOfCurrentViralLoad = row.Field<DateTime?>("Date Of Current Viral Load"); // row[19] != null ? Convert.ToDateTime(row[19]) : (DateTime?)DBNull,
                string CurrentStatus = row.Field<string>("Current Status"); //Convert.ToString(row[21]);
                string _12monthStatus = row.Field<string>("12 month Status"); //Convert.ToString(row[22]);
                string _24monthStatus = row.Field<string>("24 month Status"); //Convert.ToString(row[23]);
                string _36monthStatus = row.Field<string>("36 month Status"); //Convert.ToString(row[24]);

                try
                {
                    var pData = new PatientLineListing
                    {
                        IP = IP,
                        State = State,
                        State_Code = State_Code,
                        LGA = LGA,
                        Lga_code = Lga_code,
                        Facility = Facility,
                        DatimCode = DatimCode,
                        FacilityId = FacilityId,
                        GSMFacility = GSMFacility,
                        PatientIdentifier = PatientIdentifier,
                        HospitalNo = HospitalNo,
                        Sex = Sex,
                        DateOfBirth = DateOfBirth,
                        AgeAtARTInitiation = AgeAtARTInitiation,
                        CurrentAge = CurrentAge,
                        ARTStartDate = ARTStartDate,

                        LastDrugPickupDate = LastDrugPickupDate,
                        LastClinicVisitDate = LastClinicVisitDate,
                        DaysOfARVRefill = DaysOfARVRefill,
                        PregnancyStatus = PregnancyStatus,
                        CurrentViralLoad = CurrentViralLoad,
                        DateOfCurrentViralLoad = DateOfCurrentViralLoad,
                        CurrentStatus = CurrentStatus,
                        _12monthStatus = _12monthStatus,
                        _24monthStatus = _24monthStatus,
                        _36monthStatus = _36monthStatus,
                    };

                    if (patientlineDictionary.Keys.Contains(DatimCode))
                    {
                        patientlineDictionary[DatimCode].Add(pData);
                    }
                    else
                    {
                        patientlineDictionary.Add(DatimCode, new List<PatientLineListing> { pData });
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("", ex.ToString());
                    Logger.LogError(ex);
                }
            }

            return patientlineDictionary; // patientlinedata;
        }


        /// <summary>
        /// this is for external consumption like shield
        /// tx_new is for the current month
        /// </summary>
        /// <returns></returns>
        public string Tx_Curr_Tx_New()
        {
            var cmd = new SqlCommand("sp_tx_curr_tx_new");
            cmd.CommandType = CommandType.StoredProcedure;
            var data = new Utils().RetrieveAsDataTable(cmd);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in data.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in data.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
            //List<NDR_DATIM_DHIS2_Model> ndr_data = new List<NDR_DATIM_DHIS2_Model>();
            //foreach (DataRow row in data.Rows)
            //{
            //    ndr_data.Add(new NDR_DATIM_DHIS2_Model
            //    {
            //        IP = row.Field<string>("IP"),
            //        FacilityName = row.Field<string>("Facility"),
            //        FacilityCode = row.Field<string>("FacilityCode"),
            //        NDR_TX_CURR = row.Field<int>("NDR_TX_CURR"),
            //        NDR_TX_NEW = row.Field<int>("NDR_TX_NEW"),
            //    });
            //}
            //return ndr_data;
        }


        public async Task<List<PivotTable>> GetPivotTable(string reportPeriod, List<string> IPs = null)
        {
            var counts = new BaseDAO<PivotTable, int>().RetrieveAllLazily()
                .Count(x => x.ReportingPeriod == reportPeriod);

            List<PivotTable> pivotTables = new List<PivotTable>();
            if (counts == 0)
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["pivotTableUrl"];
                string queryString = $"?Quater={reportPeriod}&&IPstring={Newtonsoft.Json.JsonConvert.SerializeObject(IPs)}";

                url += queryString;

                Logger.LogInfo("", "about to call " + url);

                pivotTables = await new Utils().GetDateListRemotely<PivotTable>(url);
                InsertPivotData(pivotTables, reportPeriod);
            }
            else
            {
                pivotTables = new BaseDAO<PivotTable, int>().RetrieveAllLazily().Where(x => x.ReportingPeriod == reportPeriod).ToList();
            }
            return pivotTables;
        }

        void InsertPivotData(List<PivotTable> data, string reportPeriod)
        {
            BaseDAO<PivotTable, int> baseDAO = new BaseDAO<PivotTable, int>();

            string sql = "Truncate table RecentPivotTable";
            baseDAO.RunSQL(sql);

            using (var session = NhibernateSessionManager.Instance.GetSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var d in data)
                {
                    d.ReportingPeriod = reportPeriod;
                    session.Insert(d);
                }
                tx.Commit();
            }
        }
    }

    */
}
