using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Transform;

namespace Common.DBFasade
{
    public class BaseDAO<T, idT> where T : class
    {
        public virtual void Update(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
                session.Clear();
                session.Update(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public async Task UpdateAsync(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
                session.Clear();
                await session.UpdateAsync(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public void SaveStatelessSession(List<T> dataList)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var d in dataList)
                {
                    session.Insert(d);
                }
                tx.Commit();
            }
        }


        public virtual void Save(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
                session.Save(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public async Task SaveAsync(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
               await session.SaveAsync(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }


        public void BulkSaveLog(IList<T> data)
        {
            using (var session = BuildSession().SessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                foreach (var dt in data)
                {
                    session.Insert(dt);
                }

                tx.Commit();
            }
        }

        public void Delete(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
                session.Delete(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public async Task DeleteAsync(T obj)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
               await session.DeleteAsync(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }


        public void Delete(idT id)
        {
            ISession session = BuildSession();
            ITransaction tran = BuildTransaction(session);
            try
            {
                var queryString = string.Format("delete {0} where id = :id",
                                       typeof(T));
                session.CreateQuery(queryString)
               .SetParameter("id", id)
               .ExecuteUpdate();

                //session.Delete(obj);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public List<T> RetrieveUsingPaging(ICriteria theCriteria, int startIndex, int maxRows, out int totalCount)
        {
            ISession session = BuildSession();
            ICriteria countCriteria = CriteriaTransformer.Clone(theCriteria).SetProjection(Projections.RowCount());
            countCriteria.ClearOrders();

            ICriteria listCriteria = CriteriaTransformer.Clone(theCriteria).SetFirstResult(startIndex).SetMaxResults(maxRows);
            IList allResults = session.CreateMultiCriteria().Add<T>(listCriteria).Add(countCriteria).List();
            totalCount = Convert.ToInt32(((IList)allResults[1])[0]);
            return allResults[0] as List<T>;
        }


        public T Retrieve(idT id)
        {
            T result = default(T);
            ISession session = BuildSession();
            try
            {
                result = session.Get<T>(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<T> RetrieveAsync(idT id)
        {
            T result = default(T);
            ISession session = BuildSession();
            try
            {
                result =await session.GetAsync<T>(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        //public IList<T> RetrieveAll()
        //{
        //    IList<T> results = null;
        //    ISession session = BuildSession();
        //    try
        //    {
        //        results = session.CreateCriteria<T>().List<T>();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return results;
        //}

        //public async Task<IList<T>> RetrieveAllAsync()
        //{
        //    IList<T> results = null;
        //    ISession session = BuildSession();
        //    try
        //    {
        //        results = await session.CreateCriteria<T>().ListAsync<T>();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return results;
        //}

        public IQueryable<T> RetrieveAllLazily()
        {
            IQueryable<T> results = null;
            ISession session = BuildSession();
            try
            {
                results = session.Query<T>();
            }
            catch
            {
                throw;
            }
            return results;
        }


        public static ISession BuildSession()
        {
            return NhibernateSessionManager.Instance.GetSession();
        }


        protected static ITransaction BuildTransaction(ISession session)
        {
            if (session.Transaction == null || !session.Transaction.IsActive)
            {
                return session.BeginTransaction();
            }
            return session.Transaction;
        }
        
        public void CommitChanges()
        {
            ISession session = BuildSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
        }

        public async Task CommitChangesAsync()
        {
            ISession session = BuildSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                await session.Transaction.CommitAsync();
            }
        }

        public void RollbackChanges()
        {
            ISession session = BuildSession();
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
            try
            {
                session.Clear();
                session.Flush();
            }
            catch { }

        }

        public void DirectDBPost(DataTable dt, string tableName)
        {
            ISessionFactory sessionFactory = BuildSession().SessionFactory;

            using (var connection = ((ISessionFactoryImplementor)sessionFactory).ConnectionProvider.GetConnection())
            {
                var s = (SqlConnection)connection;
                var copy = new SqlBulkCopy(s, SqlBulkCopyOptions.UseInternalTransaction, null);
                copy.BulkCopyTimeout = 10000;
                copy.DestinationTableName = tableName;
                foreach (DataColumn column in dt.Columns)
                {
                    copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }
                copy.WriteToServer(dt);
            }
        }

        public int RunSQL(string commandText)
        {
            int i = 0;
            if (!string.IsNullOrEmpty(commandText))
            {
                IDbConnection cn = ((ISessionFactoryImplementor)BuildSession().SessionFactory).ConnectionProvider.GetConnection();
                IDbCommand cmd = new SqlCommand(commandText);
                cmd.Connection = (SqlConnection)cn;
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                try
                {
                    cmd.CommandTimeout = 60 * 60;
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                    cmd.Dispose();
                }
            }
            return i;
        }

        public async Task<long> RunScalarSQLAsync(string sql)
        {
            IDbConnection connection = ((ISessionFactoryImplementor)BuildSession().SessionFactory).ConnectionProvider.GetConnection();
            var handler = new DatabaseHelper(connection);
            int i = await handler.ExecuteStoredProcedure_ScalarAsync(sql, null);
            return i;
        }

        public static object GetDBValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public IList<TOut> ExecuteStoredProcedure<TOut>(string procedureName, IList<SqlParameter> parameters)
        {
            IList<TOut> result = null;
            var session = BuildSession();
            try
            {
                var query = session.GetNamedQuery(procedureName);
                AddStoredProcedureParameters(query, parameters);
                result = query.List<TOut>();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
            }             
            return result;
        }

        public TOut ExecuteScalarStoredProcedure<TOut>(string procedureName, IList<SqlParameter> parameters)
        {
            TOut result;
            var session = BuildSession();
            var query = session.GetNamedQuery(procedureName);
            AddStoredProcedureParameters(query, parameters);
            result = query.SetResultTransformer(Transformers.AliasToBean(typeof(TOut))).UniqueResult<TOut>();

            return result;
        }

        private static IQuery AddStoredProcedureParameters(IQuery query, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                query.SetParameter(parameter.ParameterName, parameter.Value);
            }
            return query;
        }


        public DataTable RetrieveAsDataTable(IDbCommand cmd)
        {
            DataTable ds = new DataTable();
            ISessionFactory sessionFactory = NhibernateSessionManager.Instance.GetSession().SessionFactory;
            try
            {
                using (IDbConnection conn = ((ISessionFactoryImplementor)sessionFactory).ConnectionProvider.GetConnection())
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.Connection = conn;
                    da.SelectCommand = (SqlCommand)cmd;

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    da.Fill(ds);
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }


        public DataTable RetrieveAsDataTable(SqlCommand cmd, string connectionString)
        {
            DataTable ds = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.Connection = conn;
                    da.SelectCommand = cmd;

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    da.Fill(ds);
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
    }
}
