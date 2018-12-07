using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Linq;
using System.Diagnostics;
using NHibernate.Persister.Entity;
using Common.DBFasade;
using ART.DAL.Entities;
using ART.DAL.ViewModels;
using Common.Utility;
using System.Data.SqlClient;
using ART.DAL.Services;
using NHibernate.SqlCommand;

namespace ART.DAL.Repository
{
    public class ContainerRepo : BaseDAO<dtoContainer, int>
    {
        readonly ThirdPartyProcessor thirdPartyProcessor = new ThirdPartyProcessor();
        readonly IDbConnection connection;
        private DatabaseHelper handler;
        public ContainerRepo()
        {
            connection = ((ISessionFactoryImplementor)BuildSession().SessionFactory).ConnectionProvider.GetConnection();
            handler = new DatabaseHelper(connection);
        }
        public TimeSpan BulkSave(List<dtoContainer> Containers)
        {
            var stopwatch = new Stopwatch();
            TimeSpan time = new TimeSpan();
            IList<DocumentResultModel> previousUploads = new List<DocumentResultModel>();
            var containers = Containers.Where(x => !x.CriticalError && x.PatientDemographics != null && x.PatientDemographics.TreatmentFacility != null).ToList();
            if (containers.Count() == 0)
            {
                return time;
            }
             
            //if dupliates exist, select the most recent based on message time
            containers = containers.GroupBy(x => new { x.PatientDemographics.PatientIdentifier, x.PatientDemographics.TreatmentFacility.Id })
                .SelectMany(y => y.OrderByDescending(c => c.MessageHeader.MessageCreationDateTime)
                .Take(1)).ToList();

            stopwatch.Start();

            DocumentSearchModel searchModel = new DocumentSearchModel
            {
                FacilityIds = containers.Select(x => x.PatientDemographics.TreatmentFacility.Id),
                PatientIds = containers.Select(x => x.PatientDemographics.PatientIdentifier),
                ContainerIdOnly = true
            };
            previousUploads = GetPreviousRecord(searchModel);

            int IdsToDelete = 0;
            //for records, 
            //Delete from database if message date is older than the current message 
            //remove from the list of message to save if the previous message is newer
            foreach (var previous in previousUploads)
            {
                var _new = containers.FirstOrDefault(x => x.PatientIdentifier == previous.PatientIdentifier && x.PatientDemographics.TreatmentFacility.Id == previous.FacilityId);
                if (_new != null && previous.MessageCreationDateTime.HasValue && previous.MessageCreationDateTime.Value.Year > 1900)
                {
                    if (previous.MessageCreationDateTime > _new.MessageHeader.MessageCreationDateTime)
                    {
                        //remove from current list
                        containers.Remove(_new);
                    }
                    else
                    {
                        //delete from database
                        IdsToDelete += 0;
                        ExecuteDelete(previous.Id);
                        //Delete(previous.Id);
                    }
                }
            }

            stopwatch.Stop();
            time = stopwatch.Elapsed;
            Console.WriteLine("finished running through previous records in " + time);
            Logger.LogInfo("", "finished deleting " + IdsToDelete + ", in =" + time);

            try
            {
                Logger.LogInfo("", "about to save. count = " + containers.Count);
                stopwatch.Restart();

                SaveStatelesslyGroupedBySimilarObj(containers);

                //save the audit trail
                var audit = (from cnt in containers
                             select new AuditTrail
                             {
                                 AuditType = "PatientData",
                                 DateUploaded = DateTime.Now,
                                 Facility = cnt.PatientDemographics.TreatmentFacility,
                                 FileName = cnt.FileName,
                                 PatientIdentifier = cnt.PatientIdentifier,
                                 Source = "NDR_Upload"
                             }).ToList();
                new AuditTrailRepo().BulkSaveLog(audit);

                stopwatch.Stop();
                time = stopwatch.Elapsed;

                Console.WriteLine("finished the batch in " + time);
                Logger.LogInfo("", "finished saving =" + containers.Count + ", in =" + time);

            }
            catch (Exception ex)
            {
                RollbackChanges();
                Logger.LogInfo("failed batch", containers.FirstOrDefault().BatchNumber);
                Logger.LogError(ex);
                throw ex;
            }
            return time;
        }
         

        void SaveStatelesslyGroupedBySimilarObj(List<dtoContainer> containers)
        {

            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    for (int i = 0; i < containers.Count; i++) //save MessageHeader
                    {
                        var cnt = containers[i];
                        cnt.MessageHeader.Container = null;
                        session.Insert(cnt.MessageHeader);
                        cnt.MessageHeader.Container = cnt;
                    }
                    for (int i = 0; i < containers.Count; i++) //save PatientDemographics
                    {
                        var cnt = containers[i];
                        cnt.PatientDemographics.Container = null;
                        session.Insert(cnt.PatientDemographics);
                        thirdPartyProcessor.DispatchToClientRegistryAsync(cnt.PatientDemographics);
                        cnt.PatientDemographics.Container = cnt;


                    }
                    for (int i = 0; i < containers.Count; i++) //save container
                    {
                        var cnt = containers[i];
                        session.Insert(cnt);
                    }
                    for (int i = 0; i < containers.Count; i++) //update
                    {
                        var cnt = containers[i];
                        session.Update(cnt.MessageHeader);
                        session.Update(cnt.PatientDemographics);
                    }
                    Logger.LogInfo("", "done saving flat objects");

                    //save condition obj
                    //save flat objects
                    var conditions = containers.SelectMany(x => x.Condition).ToList();
                    foreach (var cdt in conditions)//save CommonQuestions
                    {
                        cdt.CommonQuestions.Condition = null;
                        session.Insert(cdt.CommonQuestions);
                        cdt.CommonQuestions.Condition = cdt;
                    }
                    foreach (var cdt in conditions)//save HIVQuestions
                    {
                        cdt.HIVQuestions.Condition = null;
                        session.Insert(cdt.HIVQuestions);
                        cdt.HIVQuestions.Condition = cdt;
                    }
                    foreach (var cdt in conditions)//save PatientAddress
                    {
                        cdt.PatientAddress.Condition = null;
                        session.Insert(cdt.PatientAddress);
                        cdt.PatientAddress.Condition = cdt;
                    }
                    foreach (var cdt in conditions)//save condition
                    {
                        session.Insert(cdt);
                    }

                    foreach (var cdt in conditions)//update
                    {
                        session.Update(cdt.PatientAddress);
                        session.Update(cdt.HIVQuestions);
                        session.Update(cdt.CommonQuestions);
                    }

                    /// save the collections
                    var encs = conditions.SelectMany(x => x.Encounters).ToList();
                    foreach (var enc in encs) //save enounter
                        session.Insert(enc);

                    //save regimen
                    var regs = conditions.SelectMany(x => x.Regimen).ToList();
                    foreach (var reg in regs) //save regimen
                        session.Insert(reg);

                    //save immunization
                    var imms = conditions.SelectMany(x => x.Immunization).ToList();
                    foreach (var imm in imms)
                        session.Insert(imm);

                    var labs = conditions.SelectMany(x => x.LaboratoryReport).ToList();
                    foreach (var lab in labs) //save lab
                        session.Insert(lab);


                    //save lab order result
                    var laborders = labs.SelectMany(x => x.LaboratoryOrderAndResult).ToList();
                    foreach (var laborder in laborders)
                        session.Insert(laborder);

                    tx.Commit();
                }
            }

        }

        void SaveStatelesslyAll(List<dtoContainer> containers)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {

                for (int i = 0; i < containers.Count; i++) //save container
                {
                    var cnt = containers[i];
                    cnt.MessageHeader.Container = null;
                    session.Insert(cnt.MessageHeader);

                    cnt.PatientDemographics.Container = null;
                    session.Insert(cnt.PatientDemographics);

                    session.Insert(cnt);
                    cnt.MessageHeader.Container = cnt;
                    cnt.PatientDemographics.Container = cnt;
                    session.Update(cnt.MessageHeader);
                    session.Update(cnt.PatientDemographics);

                    foreach (var cdt in cnt.Condition)
                    {
                        cdt.CommonQuestions.Condition = null;
                        cdt.PatientAddress.Condition = null;
                        cdt.HIVQuestions.Condition = null;

                        session.Insert(cdt.CommonQuestions);//save CommonQuestions
                        session.Insert(cdt.HIVQuestions);//save HIVQuestions 
                        session.Insert(cdt.PatientAddress);//save PatientAddress
                        session.Insert(cdt);//save condition

                        foreach (var enc in cdt.Encounters) //save enounter
                            session.Insert(enc);

                        foreach (var lab in cdt.LaboratoryReport)//save lab
                        {
                            session.Insert(lab);
                            foreach (var laborder in lab.LaboratoryOrderAndResult)//save lab order result
                                session.Insert(laborder);
                        }


                        foreach (var reg in cdt.Regimen) //save regimen
                            session.Insert(reg);

                        foreach (var imm in cdt.Immunization) //save immunization
                            session.Insert(imm);
                    }
                }
                tx.Commit();
            }

        }


        public IList<DocumentResultModel> GetPreviousRecord(DocumentSearchModel model)
        {
            string sql = string.Format("select cnt.id, pd.TreatmentFacilityId as FacilityId, pd.PatientIdentifier, msg.MessageCreationDateTime" +
            " from container cnt inner join patientdemographics pd on cnt.Id = pd.containerid" +
            " inner join messageheader msg on cnt.id = msg.containerid  where pd.TreatmentFacilityId in ({0}) and pd.PatientIdentifier in ('{1}')", string.Join(",", model.FacilityIds), string.Join("','", model.PatientIds));

            IStatelessSession statelessSession = BuildSession().SessionFactory.OpenStatelessSession();
            var result = statelessSession.CreateSQLQuery(sql)
                .SetResultTransformer(Transformers.AliasToBean<DocumentResultModel>()).List<DocumentResultModel>();
            return result;
        }

        public List<DocumentResultModel> RetrieveUsingPaging(DocumentSearchModel search, int startIndex, int maxRows, out int totalCount)
        {
            IStatelessSession session = BuildSession().SessionFactory.OpenStatelessSession();
            ICriteria criteria = session.CreateCriteria<dtoContainer>("cnt")
                 .CreateAlias("cnt.PatientDemographics", "pd")
                .CreateAlias("cnt.MessageHeader", "msgh");

            if (search.ReturnAll)
            {
                criteria.CreateAlias("pd.TreatmentFacility", "tf");
                criteria = criteria.SetProjection(Projections.ProjectionList()
               .Add(Projections.Property("msgh.MessageUniqueID"), "MessageUniqueID")
               .Add(Projections.Property("pd.PatientIdentifier"), "PatientId")
               .Add(Projections.Property("tf.FacilityName"), "TreatmentFacility")
               .Add(Projections.Property("tf.DatimFacilityCode"), "DatimFacilityCode")
               .Add(Projections.Property("tf.Id"), "FacilityId")
               .Add(Projections.Property("cnt.Id"), "Id")
               .Add(Projections.Property("cnt.Id"), "DT_RowId"))
               .SetResultTransformer(Transformers.AliasToBean<DocumentResultModel>());
            }
            else if (search.ContainerIdOnly)
            {
                criteria = criteria.SetProjection(Projections.ProjectionList()
               .Add(Projections.Property("cnt.Id"), "Id")
               .Add(Projections.Property("cnt.Id"), "DT_RowId")
               .Add(Projections.Property("pd.TreatmentFacility.Id"), "FacilityId")
               .Add(Projections.Property("pd.PatientIdentifier"), "PatientId")
               .Add(Projections.Property("msgh.MessageCreationDateTime"), "MessageCreationDateTime"))
               .SetResultTransformer(Transformers.AliasToBean<DocumentResultModel>());
            }

            if (search != null)
            {
                if (search.PatientIds != null && search.PatientIds.Count() > 0)
                {
                    criteria.Add(Restrictions.In("pd.PatientIdentifier", search.PatientIds.ToList()));
                }
                if (search.FacilityDatimCodes != null && search.FacilityDatimCodes.Count() > 0)
                {
                    criteria.Add(Restrictions.In("tf.DatimFacilityCode", search.FacilityDatimCodes.ToList()));
                }
                if (search.FacilityIds != null && search.FacilityIds.Count() > 0)
                {
                    criteria.Add(Restrictions.In("pd.TreatmentFacility.Id", search.FacilityIds.ToList()));
                }
            }

            var countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.CountDistinct("cnt.Id"));

            totalCount = Convert.ToInt32(countCriteria.UniqueResult());

            criteria.AddOrder(Order.Desc("cnt.Id"));
            criteria.SetFirstResult(startIndex);
            if (maxRows > 0)
            {
                criteria.SetMaxResults(maxRows);
            }
            var result = criteria.List<DocumentResultModel>() as List<DocumentResultModel>;

            return result;
        }

        public void BulkDelete(List<int> containerIds)
        {
            if (containerIds == null || containerIds.Count == 0)
                return;

            foreach (var item in containerIds)
            {
                base.Delete(item);
            }

            //DataTable tvp = new DataTable();
            //tvp.Columns.Add(new DataColumn("Id"));
            //foreach (var item in containerIds)
            //{
            //    tvp.Rows.Add(item);
            //}
            //using (var con = ((ISessionFactoryImplementor)BuildSession().SessionFactory).ConnectionProvider.GetConnection())
            //{
            //    SqlCommand cmd = new SqlCommand("dbo.deleteRecordWithListOfContainerIds", (SqlConnection)con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    SqlParameter tvparam = cmd.Parameters.AddWithValue("@Ids", tvp);
            //    tvparam.SqlDbType = SqlDbType.Structured;
            //    cmd.CommandTimeout = 2 * 60 * 60;

            //    if (con.State != ConnectionState.Open)
            //        con.Open();
            //    cmd.ExecuteNonQuery();
            //}
        }

        public void ExecuteDelete(int containerId)
        {
            handler.ExecuteStoredProcedure_NoResultAsync
               (string.Format("Select public.sp_delete_record_by_containerid(p_containerId := {0})", containerId),
               null).Wait();
             
        }

        private IList<DocumentResultModel> GetPreviousRecord(List<dtoContainer> container)//(List<string> patientids, List<int> facilityIds)
        {
            IList<DocumentResultModel> record = new List<DocumentResultModel>();

            foreach (var item in container)
            {
                var result = handler.ExecuteStoredProcedure_OneResultSet<DocumentResultModel>
                 ("public.sp_get_patient_record",
                 new
                 {
                     p_patientid = item.PatientDemographics.PatientIdentifier,
                     p_facilityid = item.PatientDemographics.TreatmentFacility.Id,
                     p_messagecreationtime = item.MessageHeader.MessageCreationDateTime,
                 });
                if (result != null)
                    record.Add(result);
            }


            //base.ExecuteStoredProcedure<DocumentResultModel>
            //("sp_get_previous_patient_record",
            //new[] { new SqlParameter("p_query", patient_tvp) });

            return record;
        }

    }
}