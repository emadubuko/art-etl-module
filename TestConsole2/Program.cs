using ART.DAL.Services;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.DBFasade;
using Common.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace TestConsole2
{
    class Program
    {
        static ThirdPartyProcessor _3PartyProcessor = new ThirdPartyProcessor();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var error = new
            //{
            //    FileName = "0_UPDATED_20180518105137_235.xml",
            //    ErrorMessage = "new error",
            //    CrticalError = true
            //};
            //var summary = new
            //{
            //    FacilityName = "Others",
            //    ErrorDetails = new List<dynamic> { error },
            //    InvalidFiles = 1,
            //    FileUploadBacthNumber = "some other batch",
            //};
            //_3PartyProcessor.PublishValidationSummaryAsync(
            //    JsonConvert.SerializeObject(new List<dynamic> { summary }));

            string xmlContent = File.ReadAllText(@"C:\logs\ndrerror\unzipped\0_UPDATED_20180518105137_235.xml");
            var documentProcessor = new DocumentProcessor();
            documentProcessor.ProcessDocument(xmlContent);
            // MigrateData();

            Console.WriteLine("done");
            Console.ReadLine();

        }

        static void MigrateData()
        {
            string connectionString = "data source=.\\sqlexpress;user id=sa;password=P@ssw0rd;MultipleActiveResultSets=true;initial catalog=ndr_db;";
            var dao = new BaseDAO<State, int>();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Select [state_code] ,[state_name],[geo_polictical_region],[map_code] FROM [dbo].[states]";
            var st_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            var states = Utils.ConvertToList<State>(st_dt);
            dao.BulkSaveLog(states);

            var states_dict = dao.RetrieveAllLazily().ToDictionary(x => x.state_code);

            cmd.CommandText = "Select [lga_code] ,[lga_name],[lga_hm_longcode],[alternative_name],[state_code] FROM [dbo].[lga]";
            var lg_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            var lgas = Utils.ConvertToList<LGA>(lg_dt);
            lgas.ForEach(x =>
            {
                states_dict.TryGetValue(x.state_code, out State state);
                x.State = state;
            });
            new LGADAO().BulkSaveLog(lgas);

            cmd.CommandText = "select [Name],[ShortName] from [ImplementingPartners]";
            var ip_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            var ips = Utils.ConvertToList<ImplementingPartners>(ip_dt);
            new IPRepo().BulkSaveLog(ips);

            //cmd.CommandText = "Select [FacilityName],[FacilityID],[FacilityTypeCode], [DatimFacilityCode], [LGA], [GranularSite] from OnboardedFacility";
            //var fac_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            //var facilities = Utils.ConvertToList<OnboardedFacility>(fac_dt);
            //new FacilityRepo().BulkSaveLog(facilities);


            using (var file = new FileStream(@"C:\MGIC\OneDrive - University of Maryland School of Medicine\Document\NDR\Harmonized_Facility Listing_PEPFAR_Partners.xlsx", FileMode.Open))
            {
                new FacilityRepo().OnBoardFacility(file);
            }
        }
    }
}
