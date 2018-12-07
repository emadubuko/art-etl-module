using Newtonsoft.Json.Serialization;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace Common.Utility
{
    public class Utils
    {

        public static string PasCaseConversion(string PascalWord)
        {
            return Regex.Replace(PascalWord, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }

        public static string PasCaseConversion(object PascalWord)
        {
            if (PascalWord != null && !string.IsNullOrEmpty(Convert.ToString(PascalWord)))
                return PasCaseConversion(Convert.ToString(PascalWord));
            else
                return "";
        }


        public static string GetMd5Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        public DataTable GenerateDataTable<T>(IEnumerable<T> entities, SingleTableEntityPersister classMapping) where T : BaseT
        {
            var entityTable = new DataTable();
            var propertyNames = classMapping.PropertyNames;


            List<dynamic> peristedProperties = new List<dynamic>();
            foreach (var propertyName in propertyNames)
            {
                var propertyType = classMapping.GetPropertyType(propertyName);
                if (propertyType.IsCollectionType)
                {
                    return null;
                }
                var type = propertyType.ReturnedClass;
                if (propertyType.IsEntityType)
                {
                    type = typeof(int);
                }
                var columnName = classMapping.GetPropertyColumnNames(propertyName).FirstOrDefault();
                if (columnName == null)
                {
                    return null;
                }
                entityTable.Columns.Add(columnName, type);

                peristedProperties.Add(new
                {
                    ColumnName = columnName,
                    PropertyName = propertyName,
                    IsEnum = propertyType.ReturnedClass.IsEnum,
                    Type = propertyType.ReturnedClass
                });
            }


            foreach (var entity in entities)
            {
                var row = entityTable.NewRow();

                foreach (var persistedProperty in peristedProperties)
                {
                    var columnName = persistedProperty.ColumnName;
                    if (columnName != null)
                    {
                        object value = classMapping.GetPropertyValue(entity, persistedProperty.PropertyName);//, EntityMode.Poco);

                        if (value == null)
                        {
                            row[columnName] = DBNull.Value;
                        }
                        else
                        {
                            if (value is BaseT)
                            {
                                row[columnName] = (value as BaseT).Id;
                            }
                            else
                            {
                                row[columnName] = value;
                            }
                        }
                    }
                }
                entityTable.Rows.Add(row);
            }
            return entityTable;
        }


        public async Task<T> GetDateRemotely<T>(string url)
        {

            using (var client = new HttpClient())
            {
                var remote_address = new Uri(url);

                try
                {
                    var result = await client.GetAsync(remote_address);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new ApplicationException(result.Content.ReadAsStringAsync().Result);
                    else
                    {
                        string response = await result.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }
            return default(T);
        }

        public async Task<List<T>> GetDateListRemotely<T>(string url)
        {
            var client = new HttpClient();

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                var remote_address = new Uri(url);
                var result = await client.GetAsync(remote_address);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ApplicationException(await result.Content.ReadAsStringAsync());
                else
                {
                    string response = await result.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            return default(List<T>);
        }


        public async Task<bool> PostDateRemotelyAsync(string url, string jsondata)
        {

            using (var client = new HttpClient())
            {
                var remote_address = new Uri(url);
                try
                {
                    StringContent dataString = new StringContent(jsondata, Encoding.UTF8, "application/json");                       

                    var result = await client.PostAsync(remote_address, dataString);
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        string stringResult = result.Content.ReadAsStringAsync().Result;
                        if (result.Content != null && !string.IsNullOrEmpty(stringResult))
                        {
                            Logger.LogInfo(url, stringResult);
                            Console.WriteLine(stringResult);
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                   string err = Logger.LogError(ex);
                    Console.WriteLine(err);
                }
                return true;
            }
        }
    }

    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(NHibernate.Proxy.INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);
            else
                return base.CreateContract(objectType);
        }
    }

    public static class Extension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}