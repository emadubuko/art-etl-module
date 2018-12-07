using ART.DAL.Entities;
using Common.DBFasade;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace ART.DAL.Repository
{
    public class AuditTrailResponse
    {
        public string Action { get; set; }
        public string FullName { get; set; }
        public string Organization { get; set; }
        public DateTime DateUploaded { get; set; }
        public long Id { get; set; }
        public long DT_RowId { get; set; }
    }

    public class AuditDataSearchModel
    {
        public string IP { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
    }

    public class AuditTrailRepo : BaseDAO<AuditTrail, Int64>
    {
        public List<AuditTrailResponse> RetrieveUsingPaging(AuditDataSearchModel search, int startIndex, int maxRows, bool order_Asc, out int totalCount)
        {
            IStatelessSession session = BuildSession().SessionFactory.OpenStatelessSession();
            ICriteria criteria = session.CreateCriteria<AuditTrail>("at")
                 .CreateAlias("at.UserProfileId", "upl")
                  .CreateAlias("upl.IP", "ip")
                .SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("at.AuditType"), "Action")
                    .Add(Projections.Property("ip.ShortName"), "Organization")
                    .Add(Projections.Property("upl.FullName"), "FullName")
                   .Add(Projections.Property("at.DateUploaded"), "DateUploaded")
                    .Add(Projections.Property("at.Id"), "Id")
                   .Add(Projections.Property("at.Id"), "DT_RowId")
                   )
                .SetResultTransformer(Transformers.AliasToBean<AuditTrailResponse>());

            List<string> actions = new List<string>
            {
                "LOGIN_HISTORY_TYPE","LOGIN", "MER_INDICATORS_TYPE","View MER INDICATORS","View QoC INDICATORS",
                "DATA_TRIANGULATION_TYPE","View DATA TRIANGULATION", "PREVIOUS_UPLOAD_TYPE",""
            };
            criteria.Add(Restrictions.IsNotNull("at.UserProfileId"));

            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.Action))
                {
                    criteria.Add(Restrictions.Eq("at.AuditType", search.Action));
                }
                else
                {
                    criteria.Add(Restrictions.In("at.AuditType", actions));
                }
                if (!string.IsNullOrEmpty(search.IP))
                {
                    criteria.Add(Restrictions.Eq("ip.ShortName", search.IP));
                }
                if (!string.IsNullOrEmpty(search.User))
                {
                    criteria.Add(Restrictions.InsensitiveLike("upl.FullName", search.User));
                }
                if (search.DateFrom.HasValue)
                {
                    criteria.Add(Restrictions.Ge("at.DateUploaded", search.DateFrom.Value));
                }
                if (search.DateTo.HasValue)
                {
                    criteria.Add(Restrictions.Le("at.DateUploaded", search.DateTo.Value));
                }
            }
            else
            {
                criteria.Add(Restrictions.In("at.AuditType", actions));
            }

            var countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCount());
            totalCount = Convert.ToInt32(countCriteria.UniqueResult());

            criteria.AddOrder(Order.Desc("at.DateUploaded"));
            criteria.SetFirstResult(startIndex);
            if (maxRows > 0)
            {
                criteria.SetMaxResults(maxRows);
            }
            var result = criteria.List<AuditTrailResponse>() as List<AuditTrailResponse>;
            return result;
        }

        /*
        public void BulkSaveLog(List<AuditTrail> data)
        {
            string tableName = "AuditTrail";
            var dt = new DataTable(tableName);

            dt.Columns.Add(new DataColumn("FacilityId", typeof(int)));
            dt.Columns.Add(new DataColumn("PatientIdentifier", typeof(string)));
            dt.Columns.Add(new DataColumn("AuditType", typeof(string)));
            dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            dt.Columns.Add(new DataColumn("DateUploaded", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("UserId", typeof(string)));
            dt.Columns.Add(new DataColumn("Source", typeof(string)));
            dt.Columns.Add(new DataColumn("BatchFileUri", typeof(string)));
            dt.Columns.Add(new DataColumn("UserprofileId", typeof(string)));

            try
            {
                foreach (var tx in data)
                {
                    var row = dt.NewRow();

                    if (tx.Facility != null) { row["FacilityId"] = GetDBValue(tx.Facility.Id); }
                    if (tx.PatientIdentifier != null) { row["PatientIdentifier"] = GetDBValue(tx.PatientIdentifier); }
                    if (tx.AuditType != null) { row["AuditType"] = GetDBValue(tx.AuditType); }
                    if (tx.FileName != null) { row["FileName"] = GetDBValue(tx.FileName); }
                    if (tx.DateUploaded != null) { row["DateUploaded"] = GetDBValue(tx.DateUploaded); }
                    if (tx.UserId != null) { row["UserId"] = GetDBValue(tx.UserId); }
                    if (tx.Source != null) { row["Source"] = GetDBValue(tx.Source); }
                    if (tx.BatchFileUri != null) { row["BatchFileUri"] = GetDBValue(tx.BatchFileUri); }
                    if (tx.UserProfileId != null) { row["UserprofileId"] = GetDBValue(tx.UserProfileId.Id); }


                    dt.Rows.Add(row);
                }
                DirectDBPost(dt, tableName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        */

    }
}
