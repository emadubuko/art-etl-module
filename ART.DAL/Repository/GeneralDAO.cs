using Common.DBFasade;
using NHibernate;
using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace DAL.Repository
{
    public class GeneralDAO
    {        
        /*
        public List<IPSummary> GetIPSummary()
        {
            var cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[sp_getIPSummary]"
            };
            var table = new Utils().RetrieveAsDataTable(cmd);
            cmd.Dispose();

            List<IPSummary> model = new List<IPSummary>();
            foreach (DataRow row in table.Rows)
            {
                model.Add(new IPSummary
                {
                    IP = row.Field<string>("IP"),
                    Active = row.Field<int?>("Active"),
                    Inactive = row.Field<int?>("Inactive"),
                    LastUpdateDate = row.Field<DateTime>("LastDateUploaded"),
                    DATIM_TX_CURR = row.Field<int>("DATIM_Tx_curr")
                });
            }
            return model;
        }

        */

         

          
    }
}
