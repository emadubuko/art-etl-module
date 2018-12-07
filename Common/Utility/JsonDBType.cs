using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Data.Common;

namespace Common.Utility
{
    [Serializable]
    class JsonDBType<T> : IUserType where T : class
    {     
        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xdocX = JsonConvert.SerializeObject(x);
            var xdocY = JsonConvert.SerializeObject(y);

            return xdocY == xdocX;
        }

        public int GetHashCode(object x)
        {
            return x == null ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            if (names.Length != 1)
                throw new InvalidOperationException("Only expecting one column...");

            var val = rs[names[0]] as string;

            if (val != null && !string.IsNullOrWhiteSpace(val))
                return JsonConvert.DeserializeObject<T>(val);

            return null;
        }         

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (DbParameter)cmd.Parameters[index];

            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = JsonConvert.SerializeObject(value);
        }

        public object DeepCopy(object value)
        {
            if (value == null)
                return null;

            var serialized = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            var str = cached as string;
            return string.IsNullOrWhiteSpace(str) ? null : JsonConvert.DeserializeObject<T>(str);
        }

        public object Disassemble(object value)
        {
            return value == null ? null : JsonConvert.SerializeObject(value);
        }


        public SqlType[] SqlTypes
        {
            //we must write extended SqlType and return it here
            get
            {
                return new SqlType[] { new StringSqlType() };
            }
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }

    /*
    public class NpgsqlDriverExtended : NpgsqlDriver
    {
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            if (sqlType is NpgsqlExtendedSqlType && dbParam is NpgsqlParameter)
                this.InitializeParameter(dbParam as NpgsqlParameter, name, sqlType as NpgsqlExtendedSqlType);
            else
                base.InitializeParameter(dbParam, name, sqlType);
        }

        protected virtual void InitializeParameter(NpgsqlParameter dbParam, string name, NpgsqlExtendedSqlType sqlType)
        {
            if (sqlType == null)
                throw new QueryException(String.Format("No type assigned to parameter '{0}'", name));

            dbParam.ParameterName = FormatNameForParameter(name);
            dbParam.DbType = sqlType.DbType;
            dbParam.NpgsqlDbType = sqlType.NpgDbType;
        }
    }

    public class NpgsqlExtendedSqlType : SqlType
    {
        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType)
            : base(dbType)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
            : base(dbType, length)
        {
            this.npgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
            : base(dbType, precision, scale)
        {
            this.npgDbType = npgDbType;
        }

        private readonly NpgsqlDbType npgDbType;
        public NpgsqlDbType NpgDbType
        {
            get
            {
                return this.npgDbType;
            }
        }
    }

    */


}
