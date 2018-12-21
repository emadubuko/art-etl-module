using ART.DAL.Entities;
using Common.DBFasade;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ART.DAL.Repository
{
   public class PivotTableRepo
    {
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
}
