using ART.DAL.Services;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.DBFasade;
using Common.Entities;
using Common.Utility;
using FileService;
using FileService.Entities;
using FileService.Repository;
using QueueSubscriber;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //DocumentValidator("");

            Console.WriteLine("Hello World!");
            string destinationFolder = @"C:\logs\ndrerror\unzipped\";
            //var documentProcessor = new DocumentProcessor();
            var fileProcessor = new FileProcessor();
            var filerepo = new NDRFileRepo();

            var batchNo = fileProcessor.UnZipFilesAsync(destinationFolder).Result;
            //filerepo.MarkBatchAsCompletedAsync(batchNo).Wait();


            //Thread ths = new Thread( async () =>
            //{ 
            //   var batchNo =  await new FileProcessor().UnZipFilesAsync(destinationFolder);
            //    await filerepo.MarkBatchAsCompletedAsync(batchNo);
            //    //await FileServiceTestAsync();
            //});
             
            //var new_thread = new Thread(() =>
            //{               
            //    new MessageListener("10.10.8.178", documentProcessor.ProcessDocument);
            //});

            //ths.Start();
            //new_thread.Start();

            Console.WriteLine("Waiting...");
            Console.ReadLine();
        }
         

        static bool DocumentValidator(string fileName)
        {
            ValidationSummaryRepo summaryRepo = new ValidationSummaryRepo();
            List<ValidationSummary> validationSummaries = new List<ValidationSummary>
            {
                new ValidationSummary
                {
                     ErrorDetails = new List<ErrorDetails>
                     {
                         new ErrorDetails
                         {
                              CrticalError = true,
                               DataElement = "name",
                                ErrorMessage = "system error",
                                 FileName= "something.xml",
                                  PatientIdentifier="123",
                         }
                     },
                      FacilityName = "PHC",
                       FileUploadBacthNumber= "12345",
                        InvalidFiles = 12,
                            TotalFiles = 20,
                            TotalPatients = 20,
                             ValidFiles = 15,
                }
            };
            summaryRepo.SaveValidationResult(validationSummaries);

            Console.ReadLine();

            //// string fileName = @"C:\logs\ndrerror\unzipped\UPDATED_20181123073447.xml";
            //List<ErrorDetails> errors = new List<ErrorDetails>();


            //DocumentValidator validator = new DocumentValidator(@"C:\MGIC\OneDrive - University of Maryland School of Medicine\Document\NDR\documentation\NDR 1.2.xsd");
            //List<string> errorMessage = validator.ValidateXMLMessage(fileName, out string patientId);

            //errorMessage.ForEach(x =>
            //{
            //    errors.Add(new ErrorDetails
            //    {
            //        FileName = Path.GetFileName(fileName),
            //        ErrorMessage = x.Split("|")[1],
            //        DataElement = x.Split("|")[0],
            //        PatientIdentifier = patientId,
            //        CrticalError = true
            //    });
            //});

            //Console.WriteLine("Total errors " + errors.Count);
            return false; //errors.Count == 0;
        }

        //static async System.Threading.Tasks.Task FileServiceTestAsync()
        //{

        //    string destinationFolder = @"C:\logs\ndrerror\unzipped\"; ;// = System.Configuration.ConfigurationManager.AppSettings["unzipLocation"];

        //    bool t = await new FileProcessor().UnZipFilesAsync(destinationFolder);

        //}


        static void MigrateData()
        {
            //string connectionString = "data source=.\\sqlexpress;user id=sa;password=P@ssw0rd;MultipleActiveResultSets=true;initial catalog=ndr_db;";
            //var dao = new BaseDAO<State, int>();
            //SqlCommand cmd = new SqlCommand();

            //cmd.CommandText = "Select [state_code] ,[state_name],[geo_polictical_region],[map_code] FROM [dbo].[states]";
            //var st_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            //var states = Utils.ConvertToList<State>(st_dt);
            //dao.BulkSaveLog(states);

            //var states_dict = dao.RetrieveAll().ToDictionary(x => x.state_code);
            //cmd.CommandText = "Select [lga_code] ,[lga_name],[lga_hm_longcode],[alternative_name],[state_code] FROM [dbo].[lga]";
            //var lg_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            //var lgas = Utils.ConvertToList<LGA>(lg_dt);
            //lgas.ForEach(x =>
            //{
            //    states_dict.TryGetValue(x.state_code, out State state);
            //    x.State = state;
            //});
            //new LGADAO().BulkSaveLog(lgas);

            //cmd.CommandText = "select [Name],[ShortName] from [ImplementingPartners]";
            //var ip_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            //var ips = Utils.ConvertToList<ImplementingPartners>(ip_dt);
            //new IPRepo().BulkSaveLog(ips);

            //cmd.CommandText = "Select [FacilityName],[FacilityID],[FacilityTypeCode], [DatimFacilityCode], [LGA], [GranularSite] from OnboardedFacility";
            //var fac_dt = dao.RetrieveAsDataTable(cmd, connectionString);
            //var facilities = Utils.ConvertToList<OnboardedFacility>(fac_dt);
            //new FacilityRepo().BulkSaveLog(facilities);


            //using (var file = new FileStream(@"C:\MGIC\OneDrive - University of Maryland School of Medicine\Document\NDR\Harmonized_Facility Listing_PEPFAR_Partners.xlsx", FileMode.Open))
            //{
            //    new FacilityRepo().OnBoardFacility(file);
            //}




        }


        //FileZipUploadRepo repo = new FileZipUploadRepo();
        //repo.Save(new FileZipUpload
        //{
        //    BatchNumber = "CCFN_Nov_2017_45",
        //    DateUploaded = DateTime.Now,
        //    FilePath = @"C:\Users\Emeka C. Madubuko\Downloads\NDR_CCFN_RAW.zip",
        //    Status = FileBatchStatus.Pending,
        //    UploadedFileName = "NDR_CCFN_RAW.zip"
        //});
        //repo.CommitChanges();

    }


}
