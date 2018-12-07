using Common.DBFasade;
using Common.Utility;
using FileService.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FileService.Repository
{
    
    public class ValidationSummaryRepo : BaseDAO<ValidationSummary, int>
    {
        public IQueryable<ValidationSummary> SearchWithStatelessSession()
        {
            var session = BuildSession().SessionFactory.OpenStatelessSession();
            var result = session.Query<ValidationSummary>();
            return result;
        }

        public void SaveValidationResult(IList<ValidationSummary> validationSummary)
        {
            BulkSaveLog(validationSummary);
        }
/*
        public IList<ValidationSummary> GetValidationFilesInBatch(string batchnumber)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = string.Format("SELECT [Id], [FacilityName], [TotalPatients], [ValidFiles], [InvalidFiles], [FileUploadBacthNumber], [ErrorDetails], [Status] FROM [ValidationSummaryLog]  where FileUploadBacthNumber = '{0}'", batchnumber);
            var data = RetrieveAsDataTable(cmd);

            List<ValidationSummary> files = new List<ValidationSummary>();

            foreach (DataRow dr in data.Rows)
            {
                string err = Convert.ToString(dr[6]);
                List<ErrorDetails> errorDetails = XMLUtil.FromXml<List<ErrorDetails>>(err);
                files.Add(new ValidationSummary
                {
                    Id = Convert.ToInt32(dr[0]),
                    FacilityName = Convert.ToString(dr[1]),
                    TotalPatients = Convert.ToInt32(dr[2]),
                    ValidFiles = Convert.ToInt32(dr[3]),
                    InvalidFiles = Convert.ToInt32(dr[4]),
                    FileUploadBacthNumber = Convert.ToString(dr[5]),
                    ErrorDetails = errorDetails,

                });
            }
            return files;
        }

        public void BulkSaveLog(List<ValidationSummary> data, string FileUploadBacthNumber)
        {
            string tableName = "validationSummaryLog";
            var dt = new DataTable(tableName);

            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("FileUploadBacthNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("ErrorDetails", typeof(string)));
            dt.Columns.Add(new DataColumn("FacilityName", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPatients", typeof(string)));
            dt.Columns.Add(new DataColumn("ValidFiles", typeof(string)));
            dt.Columns.Add(new DataColumn("InvalidFiles", typeof(string)));

            try
            {
                foreach (var tx in data)
                {
                    var row = dt.NewRow();

                    row["FileUploadBacthNumber"] = GetDBValue(FileUploadBacthNumber);
                    row["ErrorDetails"] = XMLUtil.ConvertToXml(tx.ErrorDetails);
                    row["FacilityName"] = GetDBValue(tx.FacilityName);
                    row["TotalPatients"] = GetDBValue(tx.TotalPatients);
                    row["ValidFiles"] = GetDBValue(tx.ValidFiles);
                    row["InvalidFiles"] = GetDBValue(tx.InvalidFiles);

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
