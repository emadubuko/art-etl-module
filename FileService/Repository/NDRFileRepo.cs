using Common.DBFasade;
using Common.Utility;
using FileService.Model;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FileService.Entities
{
    public class NDRFileRepo : BaseDAO<NDRFile, int>
    {
        readonly ISessionFactory sessionFactory;
        private DatabaseHelper handler;
        readonly IDbConnection connection;
        public NDRFileRepo()
        {
            sessionFactory = BuildSession().SessionFactory;
            connection = ((ISessionFactoryImplementor)sessionFactory).ConnectionProvider.GetConnection();
            handler = new DatabaseHelper(connection);
        }

        public async Task UpdateFileStatusAsync(FileUpdateModel model)
        {
            if (model.Status != FileProcessingStatus.Processed && model.Status != FileProcessingStatus.Failed)
            {
                throw new ApplicationException("transaction status not supported");
            }
            string sql = string.Format("Update ndrfile set Status= '{0}' where Id in ({1})", model.Status.ToString(), string.Join(",", model.Ids));


            if (!string.IsNullOrEmpty(sql))
                await handler.ExecuteStoredProcedure_NoResultAsync(sql, null);
        }

        public async Task<bool> MarkBatchAsCompletedAsync(string batchNumber)
        {
            //check if batch has completed
            string sql = "";
            long status = 0;
            sql = string.Format("Select count(*) from (Select Status,BatchNumber, count(*) 'count' from FileUpload where BatchNumber = '{0}' and Status='Pending' group by BatchNumber,Status) as mydata", batchNumber);
            status = await handler.ExecuteStoredProcedure_ScalarAsync(sql, null);

            //return true if completed, false otherwise
            if (status == 0)
            {
                sql = string.Format("Update FileBatch set Status = 'Completed' where BatchNumber = '{0}'", batchNumber);
                await handler.ExecuteStoredProcedure_NoResultAsync(sql, null);
            }
            return true;
        }

        /*
        public List<NDRFile> GetUnprocessedFilesInBatch()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "[dbo].[sp_getunprocessedfileQuery]";
            cmd.CommandType = CommandType.StoredProcedure;

            var data = RetrieveAsDataTable(cmd);

            List<NDRFile> files = new List<NDRFile>();

            foreach (DataRow dr in data.Rows)
            {
                files.Add(new NDRFile
                {
                    Id = Convert.ToInt32(dr[0]),
                    DateUploaded = Convert.ToDateTime(dr[1]),
                    Status = (FileProcessingStatus)Enum.Parse(typeof(FileProcessingStatus), Convert.ToString(dr[2])),
                    BatchNumber = Convert.ToString(dr[3]),
                    UploadedBy = Convert.ToInt32(dr[4]),
                    FileName = Convert.ToString(dr[5]),
                    ParentFileName = Convert.ToString(dr[6]),
                    FileBatchId = Convert.ToInt32(dr[7])
                });
            }
            return files;
        }
       


        public DataTable GetUploadReport(string organization = "")
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "[dbo].[sp_get_uploads]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@organization", organization);
            cmd.CommandTimeout = 80;
            return RetrieveAsDataTable(cmd);
        }
        */

        public List<FileUploadViewModel> RetrieveUsingPagingAsync(ZipFileSearchModel search, out int totalCount)
        {
             IStatelessSession session = BuildSession().SessionFactory.OpenStatelessSession();
            var result = (from item in session.Query<FileZipUpload>().Skip(search.StartIndex ).Take(search.MaxRows ?? 20)
                          select new FileUploadViewModel
                          {
                              Id = item.Id,
                              DT_RowId = item.Id,
                              BatchNumber = item.BatchNumber,
                              DateUploaded = item.DateUploaded,
                              FileName = item.UploadedFileName,
                              Status = item.Status.ToString(), 
                              UploadedBy = item.UploadedBy.FullName,
                              IP = item.UploadedBy.IP.ShortName,
                          }).ToList();


            foreach (var item in result)
            {
                string sql = string.Format("SELECT status, count(*) FROM  ndrfile where filebatchid = {0} group by status", item.Id);
                IList<TempFileStatus> statusList = handler.ExecuteSQLScriptAsync<TempFileStatus>(sql);
                if(statusList != null)
                {
                    item.TotalFile = statusList.Sum(x => x.Count);
                    item.TotalFileProcessed = statusList.Where(x => x.Status == "Processed").Sum(x => x.Count);
                   // item.TotalFacilities = item.TotalFileProcessed;
                }                 
            }

            totalCount = session.Query<FileZipUpload>().Count();

            return result;
        }


        /*  public void BulkSaveLog(List<NDRFile> data)
          {

              int pageSize = 5000;


              string batchnumber = data.FirstOrDefault().BatchNumber;
              string tableName = "FileUpload";
              for (int currentPage = 0; currentPage < data.Count();)
              {
                  var stopwatch = new Stopwatch();
                  stopwatch.Start();

                  var _dt = data.Skip(currentPage).Take(pageSize).ToList();
                  var dt = new DataTable(tableName);

                  dt.Columns.Add(new DataColumn("FileName", typeof(string)));
                  dt.Columns.Add(new DataColumn("Status", typeof(string)));
                  dt.Columns.Add(new DataColumn("BatchNumber", typeof(string)));
                  //dt.Columns.Add(new DataColumn("File", typeof(string)));
                  dt.Columns.Add(new DataColumn("DateUploaded", typeof(DateTime)));
                  dt.Columns.Add(new DataColumn("UploadedBy", typeof(int)));
                  dt.Columns.Add(new DataColumn("ParentFileName", typeof(string)));
                  dt.Columns.Add(new DataColumn("FileBatchId", typeof(int)));
                  try
                  {
                      foreach (var tx in _dt)
                      {
                          var row = dt.NewRow();
                          row["Status"] = GetDBValue(tx.Status);
                          row["BatchNumber"] = GetDBValue(tx.BatchNumber);
                           row["DateUploaded"] = GetDBValue(tx.DateUploaded);
                          row["UploadedBy"] = GetDBValue(tx.UploadedBy);
                          row["ParentFileName"] = GetDBValue(tx.ParentFileName);
                          row["FileName"] = GetDBValue(tx.FileName);
                          row["FileBatchId"] = GetDBValue(tx.FileBatchId);

                          dt.Rows.Add(row);
                      }
                      DirectDBPost(dt, tableName);

                      stopwatch.Stop();
                      var time = stopwatch.Elapsed;
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
                  currentPage += pageSize;
              }
          }


          */



    }

    public class TempFileStatus
    {
        public string Status { get; set; }
        public Int64 Count { get; set; }
    }
}
