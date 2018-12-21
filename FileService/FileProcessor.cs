using Common.Utility;
using FileService.Entities;
using FileService.Repository;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    public class FileProcessor
    {
        NDRFileRepo FileRepo;
        FileZipUploadRepo batchRepo;
        public FileProcessor()
        {
            FileRepo = new NDRFileRepo();
            batchRepo = new FileZipUploadRepo();
        }

        public async Task<string> UnZipFilesAsync(string destinationFile)
        {
            IList<NDRFile> theFiles = FileRepo.RetrieveAllLazily().Where(x => x.Status == FileProcessingStatus.Pending).ToList();

            if (theFiles == null || theFiles.Count == 0)
            {

                FileZipUpload batch = batchRepo.RetrieveByStatus(FileBatchStatus.Pending);
                if (batch != null)
                {
                    EmptyFile(destinationFile);

                    string filePath = batch.FilePath;

                    try
                    {
                        FileStream reader = File.OpenRead(filePath);
                        theFiles = UnZip(reader, destinationFile, batch);

                        FileRepo.BulkSaveLog(theFiles);
                        batch.Status = FileBatchStatus.Processing;
                        batchRepo.Update(batch);
                        batchRepo.CommitChanges();

                    }
                    catch (Exception ex)
                    {
                        string msg = Logger.LogError(ex);
                        batch.ErrorMessage = msg;
                        batch.Status = FileBatchStatus.Failed;
                        batchRepo.Update(batch);
                        batchRepo.CommitChanges();
                    }
                }
            }
            //either push the files to the server 

            string batchNo = await SendFilesToMediatorAsync(theFiles);

            FileRepo.CloseSession();
            return batchNo;
        }

        private async Task<string> SendFilesToMediatorAsync(IList<NDRFile> theFiles)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            string fileBatchNo = "";

            HttpClient client = new HttpClient(httpClientHandler);
            string username_password = Utils.GetAppConfigItem("mediator_username_password");
            var byteArray = Encoding.ASCII.GetBytes(username_password); //Encoding.ASCII.GetBytes("ndr:xds");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            HttpResponseMessage response;
            HttpContent content;

            string url = Utils.GetAppConfigItem("mediatorUrl"); //string.Format("https://10.10.8.178:5000/encounters/");

            foreach (var item in theFiles)
            {
                fileBatchNo = item.BatchNumber;

                string fileContent = await File.ReadAllTextAsync(item.FileName);
                var postcontent = new StringContent(item.Id + "@||@" + item.BatchNumber + "@||@" + fileContent, Encoding.UTF8, "application/json");

                response = await client.PostAsync(url, postcontent);
                content = response.Content;
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.Created ||
                    response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    item.Status = FileProcessingStatus.Processing;
                    FileRepo.Save(item);
                }
                else
                {
                    string errorMsg = await content.ReadAsStringAsync();
                    Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);
                    Console.WriteLine("response message" + errorMsg);
                    Logger.LogInfo("posting to OpenHIM", errorMsg);
                }
                FileRepo.CommitChanges();
            }

            client.Dispose();
            return fileBatchNo;
        }



        void EmptyFile(string destinationFile)
        {
            DirectoryInfo di = new DirectoryInfo(destinationFile);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private List<NDRFile> UnZip(Stream stream, string destination, FileZipUpload batch)
        {
            List<NDRFile> allFiles = new List<NDRFile>();
            try
            {
                using (ZipFile zipFile = new ZipFile(stream))
                {
                    for (int i = 0; i < zipFile.Count; i++)
                    {
                        ZipEntry zipEntry = zipFile[i];
                        string fileName = ZipEntry.CleanName(zipEntry.Name);

                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }
                        Stream zipStream = zipFile.GetInputStream(zipEntry);
                        
                        if (fileName.EndsWith(".zip"))
                        {
                            if (!zipStream.CanSeek)
                            {
                                continue;
                            }
                            allFiles.AddRange(UnZip(zipStream, destination, batch));                           
                        }
                        else
                        {
                            string filename = destination + i + "_" + Path.GetFileName(fileName);
                            if (File.Exists(filename))
                            {
                                filename = destination + i + "_" + 1 + Path.GetFileName(fileName);
                            }

                            using (FileStream streamWriter = File.Create(filename))
                            {
                                byte[] buffer = new byte[8 * 1024];
                                StreamUtils.Copy(zipStream, streamWriter, buffer);
                            }
                             

                            allFiles.Add(
                                 new NDRFile
                                 {
                                     DateUploaded = batch.DateUploaded,
                                     FileName = filename,
                                     //ParentFileName = batch.UploadedFileName,
                                     Status = FileProcessingStatus.Pending,
                                     UploadedBy = batch.UploadedBy.Id,
                                     BatchNumber = batch.BatchNumber,
                                     FileBatch = batch
                                 });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }

            return allFiles;
        }




    }
}
