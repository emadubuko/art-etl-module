using ART.DAL.Entities;
using Common.Utility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ART.DAL.Services
{
    public class ThirdPartyProcessor
    {
        readonly string clientRegistryURL;
        readonly string fileServiceURL;
        readonly string validationSummaryURL;
        public ThirdPartyProcessor()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            clientRegistryURL = configuration.GetSection("clientRegistryURL").Value;
            fileServiceURL  = configuration.GetSection("fileServiceURL").Value;
            validationSummaryURL = configuration.GetSection("validationSummaryURL").Value;
        }

        public async void DispatchToClientRegistryAsync(dtoPatientDemographics patientDemography)
        {
            Utils _3rdPartyConnector = new Utils();
            patientDemography.Container = null;

            string jsonData = JsonConvert.SerializeObject(
                patientDemography,
                Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new NHibernateContractResolver()
                });

            await _3rdPartyConnector.PostDateRemotelyAsync(url: clientRegistryURL, jsondata: jsonData);
        }


        public async void UpdateFileStatusAsync(string jsonData)
        {
            Utils _3rdPartyConnector = new Utils();
            await _3rdPartyConnector.PostDateRemotelyAsync(url: fileServiceURL, jsondata: jsonData);
        }

        public async void PublishValidationSummaryAsync(string jsondata)
        {
            Utils _3rdPartyConnector = new Utils();
            await _3rdPartyConnector.PostDateRemotelyAsync(url: validationSummaryURL, jsondata: jsondata);
        }

    }
}
