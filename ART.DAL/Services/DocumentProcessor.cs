using ART.DAL.SchemaModel;
using Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using ART.DAL.Entities;
using System.Collections.Generic;
using Common.CommonEntities;
using Common.Entities;
using System.Linq;
using Common.CommonRepo;
using Common.DBFasade;
using ART.DAL.Repository;

namespace ART.DAL.Services
{
    public class DocumentProcessor
    {
         Dictionary<string, ImplementingPartners> IPs;
         IEnumerable<IGrouping<ImplementingPartners, IPFacility>> IPFacilities;
        Dictionary<string, Dictionary<string, OnboardedFacility>> facilities;
               
        public DocumentProcessor()
        {
            IPs = new IPRepo().RetrieveAllLazily().ToDictionary(x => x.ShortName);
            IPFacilities = new BaseDAO<IPFacility, int>().RetrieveAllLazily().GroupBy(x => x.IP);
            facilities = new Dictionary<string, Dictionary<string, OnboardedFacility>>();
            foreach (var ipf in IPFacilities)
            {
                facilities.Add(ipf.Key.ShortName, ipf.ToList().Select(x => x.Facility).ToDictionary(x => x.DatimFacilityCode.ToLower().Trim()));
            }
        }

        public bool ProcessDocument(string xmlContent)
        {
             string err = "";
            if (!string.IsNullOrEmpty(err))
            {
                return false;
            }

            xmlContent = xmlContent.Replace(" & ", " &amp; ");
            xmlContent = xmlContent.Replace("'", "&apos;");
            xmlContent = CleanInvalidXmlChars(xmlContent);
            var xdoc = XDocument.Parse(xmlContent);
            if (xdoc == null)
            {
                return false;
            }

            Container msg = ConvertToXMLModel(xmlContent, out err);

            if (msg != null)
            {
                var container = ExtractAndTransformMessage(msg);
                return SaveData(new List<dtoContainer> { container });
            }
            else
            {
                return false;
            }
        }

        private bool SaveData(List<dtoContainer> containers)
        {
            ThirdPartyProcessor _3PartyProcessor = new ThirdPartyProcessor();
            try
            {
                var containerRepo = new ContainerRepo();
                var time = containerRepo.BulkSave(containers);

                //update the file uploads entries
                var fileIds = containers.Where(x => !x.CriticalError).Select(x => x.FileId);
                if(fileIds != null && fileIds.Count() > 0)
                {
                    _3PartyProcessor.UpdateFileStatusAsync(
                                        JsonConvert.SerializeObject(
                                            new
                                            {
                                                Ids = fileIds,
                                                Status = "Processed"
                                            })
                                        );
                }
                
                var failedFileId = containers.Where(x => x.CriticalError).Select(x => x.FileId);
                if(failedFileId != null || failedFileId.Count() > 0)
                {
                    _3PartyProcessor.UpdateFileStatusAsync(
                       JsonConvert.SerializeObject(
                           new
                           {
                               Ids = failedFileId,
                               Status = "Failed"
                           })
                       );
                }


                Logger.LogInfo("", "about to save validation summary");

                List<dynamic> validationSummary = new List<dynamic>();
                if (containers.Where(x => x.PatientDemographics != null && x.PatientDemographics.TreatmentFacility != null).Count() > 0)
                {
                    var patientGrouping = containers.Where(x => x.PatientDemographics != null && x.PatientDemographics.TreatmentFacility != null)
                        .GroupBy(x => x.PatientDemographics.TreatmentFacility);
                                       
                    foreach (var grp in patientGrouping)
                    {
                        var summary = new
                        {
                            FacilityName = grp.Key != null ? grp.Key.FacilityName : "Unknown",
                            TotalPatients = grp.DistinctBy(x => x.PatientIdentifier).Count(),
                            ValidFiles = grp.Count(x => x.Errors.Count == 0),
                            InvalidFiles = grp.Count(x => x.Errors.Count != 0),
                            ErrorDetails = grp.ToList().SelectMany(x => x.Errors).ToList(),
                            FileUploadBacthNumber = grp.FirstOrDefault().BatchNumber,
                        };                         
                        validationSummary.Add(summary);
                    }
                }
                 
                var othererrors = containers.Where(x => x.PatientDemographics == null 
                                    || x.PatientDemographics.TreatmentFacility == null)
                                    .SelectMany(x => x.Errors);

                if (othererrors != null && othererrors.Count() > 0)
                {
                    validationSummary.Add(new
                    {
                        FacilityName = "Others",
                        ErrorDetails = othererrors.ToList(),
                        InvalidFiles = othererrors.Count(),
                        Status = NotificationStatus.Logged,
                        FileUploadBacthNumber = containers.FirstOrDefault().BatchNumber,
                    });
                }
                _3PartyProcessor.PublishValidationSummaryAsync(JsonConvert.SerializeObject(validationSummary));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
         }
                
        public dtoContainer ExtractAndTransformMessage(Container msg)
        {
            dtoContainer cnt = new dtoContainer();
            List<ErrorDetails> error = new List<ErrorDetails>();
            var allFaciltyDictionary = facilities.Values.SelectMany(x => x).Select(x => x.Value).ToDictionary(x => x.DatimFacilityCode.ToLower().Trim());


            string patientIdentifier = msg.IndividualReport != null && msg.IndividualReport.PatientDemographics != null && !string.IsNullOrEmpty(msg.IndividualReport.PatientDemographics.PatientIdentifier) ? msg.IndividualReport.PatientDemographics.PatientIdentifier : "";

            if (msg.MessageHeader == null)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Message header was not supplied",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "MessageHeader",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
                return cnt;
            }

            if (msg.IndividualReport == null)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Individual Report was not supplied",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "IndividualReport",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
                return cnt;
            }

            if (msg.IndividualReport.PatientDemographics == null)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Patient Demographics was not supplied",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "IndividualReport/PatientDemographics",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
                return cnt;
            }


            if (msg.IndividualReport.Condition == null)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Condition was not supplied",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "IndividualReport/Condition",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
                return cnt;
            }


            IPs.TryGetValue(msg.MessageHeader.MessageSendingOrganization.FacilityID, out ImplementingPartners Ip);

            if (Ip == null)
            {
                IPs.TryGetValue(msg.MessageHeader.MessageSendingOrganization.FacilityName.Trim(), out Ip); //sometimes the ip shortname is in facilityname tag
                if (Ip == null)
                {
                    Ip = IPs.Values.FirstOrDefault(x => msg.MessageHeader.MessageSendingOrganization.FacilityID.Contains(x.ShortName));
                    if (Ip == null)
                    {
                        error.Add(new ErrorDetails
                        {
                            ErrorMessage = "Unknown IP",
                            FileName = msg.FileName,
                            PatientIdentifier = patientIdentifier,
                            DataElement = "MessageHeader/MessageSendingOrganization",
                            CrticalError = true
                        });
                        cnt.Errors = error;
                        cnt.CriticalError = true;
                        return cnt;
                    }
                }
            }

            Dictionary<string, OnboardedFacility> ipFacilities = new Dictionary<string, OnboardedFacility>();
            if (facilities.TryGetValue(Ip.ShortName, out ipFacilities) == false)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Unknown IP | " + Ip.ShortName,
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "MessageHeader/MessageSendingOrganization",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
                return cnt;
            }



            var header = new dtoMessageHeader();
            msg.MessageHeader.CopyPropertiesTo(header);
            header.IP = Ip;
            header.Container = cnt;

            OnboardedFacility fac = ConvertToEntityFacility(msg.IndividualReport.PatientDemographics.TreatmentFacility, patientIdentifier, msg.FileName, "PatientDemographics/TreatmentFacility", ipFacilities, ref error);
            if (fac == null)
            {

                if (msg.IndividualReport.PatientDemographics.TreatmentFacility == null || string.IsNullOrEmpty(msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityID))
                {
                    error.Add(new ErrorDetails
                    {
                        ErrorMessage = string.Format("Treatment facility not supplied "),
                        FileName = msg.FileName,
                        PatientIdentifier = patientIdentifier,
                        DataElement = "PatientDemographics/Treatment Facility",
                        CrticalError = true
                    });
                }
                else if (allFaciltyDictionary.ContainsKey(msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityID))
                {
                    error.Add(new ErrorDetails
                    {
                        ErrorMessage = string.Format("Wrong facility, IP mapping|{0}|{1}{2} ", msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityID, msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityName, Ip.ShortName),
                        FileName = msg.FileName,
                        PatientIdentifier = patientIdentifier,
                        DataElement = "PatientDemographics/Treatment Facility",
                        CrticalError = true,
                    });
                }
                else
                {
                    error.Add(new ErrorDetails
                    {
                        ErrorMessage = string.Format("invalid treatment facility Id supplied |{0}|{1} ", msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityID, msg.IndividualReport.PatientDemographics.TreatmentFacility.FacilityName),
                        FileName = msg.FileName,
                        PatientIdentifier = patientIdentifier,
                        DataElement = "PatientDemographics/Treatment Facility",
                        CrticalError = true,
                    });
                }
                cnt.Errors = error;
                cnt.CriticalError = true;
            }

            if (string.IsNullOrEmpty(msg.IndividualReport.PatientDemographics.PatientIdentifier))
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Patient identifier is empty",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "PatientDemographics/PatientIdentifier",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
            }

            if (!msg.IndividualReport.PatientDemographics.PatientSexCode.HasValue)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Patient Sex is missing",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "PatientDemographics/PatientSexCode",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
            }

            if (!msg.IndividualReport.PatientDemographics.PatientDateOfBirth.HasValue)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Patient date of birth is missing",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "PatientDemographics/PatientDateOfBirth",
                    CrticalError = true
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
            }
            else if (msg.IndividualReport.PatientDemographics.PatientDateOfBirth.Value > DateTime.Now)
            {
                error.Add(new ErrorDetails
                {
                    ErrorMessage = "Patient date of birth cannot be a future date",
                    FileName = msg.FileName,
                    PatientIdentifier = patientIdentifier,
                    DataElement = "PatientDemographics/PatientDateOfBirth"
                });
                cnt.Errors = error;
                cnt.CriticalError = true;
            }

            //patientdemography
            var identifier = new List<dtoIdentifier>();
            var PatientDemographics = new dtoPatientDemographics();

            msg.IndividualReport.PatientDemographics.CopyPropertiesTo(PatientDemographics);
            PatientDemographics.TreatmentFacility = fac;
            //PatientDemographics.PatientNotes = msg.IndividualReport.PatientDemographics.PatientNotes != null ? msg.IndividualReport.PatientDemographics.PatientNotes.Note : "";

            //add the other patient identifiers
            if (msg.IndividualReport.PatientDemographics.OtherPatientIdentifiers != null)
            {
                foreach (var pid in msg.IndividualReport.PatientDemographics.OtherPatientIdentifiers)
                {
                    var dtid = new dtoIdentifier();
                    if (pid.Identifier != null)
                    {
                        foreach (var d in pid.Identifier)
                        {
                            dtid.IDNumber = d.IDNumber;
                            dtid.IDTypeCode = d.IDTypeCode;
                            dtid.PatientDemographics = PatientDemographics;
                            identifier.Add(dtid);
                        }
                        //dtid.IDNumber = pid.Identifier.IDNumber;
                        //dtid.IDTypeCode = pid.Identifier.IDTypeCode;
                    }
                }
            }
            PatientDemographics.Identifier = identifier;

            //add patient note
            if (msg.IndividualReport.PatientDemographics.PatientNotes != null)
            {
                PatientDemographics.PatientNotes = msg.IndividualReport.PatientDemographics.PatientNotes.Note;
            }
            PatientDemographics.Container = cnt;

            //condition
            List<dtoCondition> conditions = new List<dtoCondition>();
            if (msg.IndividualReport.Condition != null)
            {
                foreach (var cdt in msg.IndividualReport.Condition)
                {
                    var condition = new dtoCondition();

                    if (string.IsNullOrEmpty(cdt.ConditionCode))
                    {
                        error.Add(new ErrorDetails
                        {
                            ErrorMessage = "ConditionCode is empty",
                            FileName = msg.FileName,
                            PatientIdentifier = patientIdentifier,
                            DataElement = "Condition/ConditionCode"
                        });
                    }

                    if (cdt.ProgramArea == null || string.IsNullOrEmpty(cdt.ProgramArea.ProgramAreaCode))
                    {
                        error.Add(new ErrorDetails
                        {
                            ErrorMessage = "Program Area Code is empty",
                            FileName = msg.FileName,
                            PatientIdentifier = patientIdentifier,
                            DataElement = "Condition/ProgramAreaCode"
                        });
                    }
                    if (cdt.PatientAddress == null || string.IsNullOrEmpty(cdt.PatientAddress.AddressTypeCode))
                    {
                        error.Add(new ErrorDetails
                        {
                            ErrorMessage = "Address Type Code is empty",
                            FileName = msg.FileName,
                            PatientIdentifier = patientIdentifier,
                            DataElement = "Address/AddressTypeCode"
                        });
                    }

                    //regimen
                    var Regimen = new List<dtoRegimen>();
                    if (cdt.Regimen != null)
                    {
                        foreach (var reg in cdt.Regimen)
                        {
                            bool failedValidation = false;
                            if (!reg.VisitDate.HasValue)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/VisitDate",
                                    CrticalError = true
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            else if (reg.VisitDate.Value > DateTime.Now)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date cannot be in the future | " + reg.VisitDate.Value.ToString("dd-MMM-yyyy"),
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/VisitDate",
                                    CrticalError = true
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }

                            if (string.IsNullOrEmpty(reg.VisitID))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit ID is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/VisitID",
                                });
                            }

                            if (reg.PrescribedRegimen == null || string.IsNullOrEmpty(reg.PrescribedRegimen.Code))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Prescribed Regimen is missing",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/PrescribedRegimen"
                                });
                                // cnt.CriticalError = true;
                                //failedValidation = true;
                            }

                            if (!double.TryParse(reg.PrescribedRegimenDuration, out double regDuration))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Prescribed Regimen Duration is not a number | " + reg.PrescribedRegimenDuration,
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/PrescribedRegimenDuration",
                                    CrticalError = true
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }

                            if (string.IsNullOrEmpty(reg.PrescribedRegimenTypeCode))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Invalid Prescribed Regimen Type code specified ",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/PrescribedRegimenTypeCode",
                                    CrticalError = true
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }

                            if (regDuration < 1 || regDuration > 180)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Prescribed Regimen Duration falls out of normal range of 1 to 180 days | " + reg.PrescribedRegimenDuration,
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Regimen/PrescribedRegimenDuration",
                                    CrticalError = true
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            else
                            {
                                reg.PrescribedRegimenDuration = regDuration.ToString();
                            }

                            if (failedValidation) //move over to the next regimen
                            {
                                continue;
                            }

                            var dtr = new dtoRegimen();
                            reg.CopyPropertiesTo(dtr);
                            if (reg.PrescribedRegimen != null)
                            {
                                dtr.PrescribedRegimenCode = reg.PrescribedRegimen.Code;
                                dtr.PrescribedRegimenCodeCodeDesc = reg.PrescribedRegimen.CodeDescTxt;
                            }
                            dtr.Condition = condition;
                            Regimen.Add(dtr);
                        }
                    }


                    //immunization
                    var Immunization = new List<dtoImmunization>();
                    if (cdt.Immunization != null)
                    {
                        foreach (var imm in cdt.Immunization)
                        {
                            if (!imm.VisitDate.HasValue)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "VisitDate is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Immunization/VisitDate"
                                });
                                cnt.CriticalError = true;
                            }
                            else if (imm.VisitDate.Value > DateTime.Now)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "VisitDate must not be a future date |" + imm.VisitDate.Value.ToString("dd-MMM-yyyy"),
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Immunization/VisitDate"
                                });
                                cnt.CriticalError = true;
                            }

                            if (string.IsNullOrEmpty(imm.VisitID))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "VisitID is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Immunization/VisitID"
                                });
                            }
                            if (imm.ImmunizationType1 == null || string.IsNullOrEmpty(imm.ImmunizationType1.Code))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "ImmunizationType is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Immunization/ImmunizationType"
                                });
                            }
                            var dti = new dtoImmunization();
                            imm.CopyPropertiesTo(dti);
                            if (imm.ImmunizationType1 != null)
                            {
                                dti.ImmunizationCode = imm.ImmunizationType1.Code;
                                dti.ImmunizationCodeDesc = imm.ImmunizationType1.CodeDescTxt;
                            }
                            Immunization.Add(dti);
                        }
                    }


                    //patient address
                    var PatientAddress = new dtoAddress();
                    if (cdt.PatientAddress != null)
                    {
                        cdt.PatientAddress.CopyPropertiesTo(PatientAddress);
                    }

                    //laboratory report
                    var LaboratoryReport = new List<dtoLaboratoryReport>();
                    if (cdt.LaboratoryReport != null)
                    {
                        foreach (var lab in cdt.LaboratoryReport)
                        {
                            bool failedValidation = false;
                            if (!lab.VisitDate.HasValue)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "LaboratoryReport/VisitDate"
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            else if (lab.VisitDate.Value > DateTime.Now)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date must not be a future date |" + lab.VisitDate.Value.ToString("dd-MMM-yyyy"),
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "LaboratoryReport/VisitDate"
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            if (string.IsNullOrEmpty(lab.VisitID))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit ID is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "LaboratoryReport/VisitID"
                                });
                            }

                            if (failedValidation) //move over to the next lab report
                            {
                                continue;
                            }


                            var dtlab = new dtoLaboratoryReport();
                            lab.CopyPropertiesTo(dtlab);
                            dtlab.LaboratoryOrderAndResult = new List<dtoLaboratoryOrderAndResult>();

                            if (lab.LaboratoryOrderAndResult != null)
                            {
                                foreach (var laborder in lab.LaboratoryOrderAndResult)
                                {
                                    bool laborder_failed = false;
                                    if (laborder.OrderedTestDate.HasValue && laborder.OrderedTestDate.Value > DateTime.Now)
                                    {
                                        error.Add(new ErrorDetails
                                        {
                                            ErrorMessage = "when Laboratory Ordered Test Date is supplied. it must not be a future date |" + laborder.OrderedTestDate.Value.ToString("dd-MMM-yyyy"),
                                            FileName = msg.FileName,
                                            PatientIdentifier = patientIdentifier,
                                            DataElement = "LaboratoryResult/OrderedTestDate"
                                        });
                                        //cnt.CriticalError = true;
                                        laborder_failed = true;
                                    }

                                    var LaboratoryOrderAndResult = new dtoLaboratoryOrderAndResult();
                                    laborder.CopyPropertiesTo(LaboratoryOrderAndResult);

                                    if (laborder.LaboratoryResult != null)
                                    {
                                        if (laborder.LaboratoryResult.AnswerNumeric != null && !string.IsNullOrEmpty(laborder.LaboratoryResult.AnswerNumeric.Value1))
                                        {
                                            LaboratoryOrderAndResult.LaboratoryResultValue = laborder.LaboratoryResult.AnswerNumeric.Value1;
                                        }

                                        if (laborder.LaboratoryResult.AnswerCode != null)
                                        {
                                            LaboratoryOrderAndResult.LaboratoryResult_AnswerCode = laborder.LaboratoryResult.AnswerCode.Code;
                                            LaboratoryOrderAndResult.LaboratoryResult_AnswerCodeDescTxt = laborder.LaboratoryResult.AnswerCode.CodeDescTxt;
                                            LaboratoryOrderAndResult.LaboratoryResult_AnswerCodeSystemCode = laborder.LaboratoryResult.AnswerCode.CodeSystemCode;
                                        }
                                        LaboratoryOrderAndResult.LaboratoryResult_AnswerDate = laborder.LaboratoryResult.AnswerDate ?? laborder.LaboratoryResult.AnswerDateTime;
                                    }


                                    if (laborder.LaboratoryResultedTest == null || (string.IsNullOrEmpty(laborder.LaboratoryResultedTest.Code) && string.IsNullOrEmpty(laborder.LaboratoryResultedTest.CodeDescTxt)))
                                    {
                                        error.Add(new ErrorDetails
                                        {
                                            ErrorMessage = "Laboratory Resulted Test Code and Desc is required",
                                            FileName = msg.FileName,
                                            PatientIdentifier = patientIdentifier,
                                            DataElement = "LaboratoryResult/LaboratoryResultedTest"
                                        });
                                        //cnt.CriticalError = true;
                                        laborder_failed = true;
                                    }
                                    else
                                    {
                                        LaboratoryOrderAndResult.LaboratoryResultedTestCode = laborder.LaboratoryResultedTest.Code;
                                        LaboratoryOrderAndResult.LaboratoryResultedTestDesc = laborder.LaboratoryResultedTest.CodeDescTxt;
                                    }
                                    if (laborder.LaboratoryOrderedTest != null)
                                    {
                                        LaboratoryOrderAndResult.LaboratoryOrderedTestCode = laborder.LaboratoryOrderedTest.Code;
                                        LaboratoryOrderAndResult.LaboratoryOrderedTestCodeDesc = laborder.LaboratoryOrderedTest.CodeDescTxt;
                                    }

                                    if (laborder_failed) //move over to the next lab order result
                                    {
                                        continue;
                                    }

                                    LaboratoryOrderAndResult.LaboratoryReport = dtlab;
                                    dtlab.LaboratoryOrderAndResult.Add(LaboratoryOrderAndResult);
                                }
                            }

                            dtlab.Condition = condition;
                            LaboratoryReport.Add(dtlab);
                        }
                    }



                    //encounters
                    var Encounters = new List<dtoHIVEncounter>();
                    if (cdt.Encounters != null && cdt.Encounters.HIVEncounter != null)
                    {
                        foreach (var enc in cdt.Encounters.HIVEncounter)
                        {
                            bool failedValidation = false;
                            if (!enc.VisitDate.HasValue)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "HIVEncounter/VisitDate"
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            else if (enc.VisitDate.Value > DateTime.Now)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit Date must not be a future date | " + enc.VisitDate.Value.ToString("dd-MMM_yyyy"),
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/VisitDate"
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }
                            if (enc.CD4TestDate.HasValue && enc.CD4TestDate.Value > DateTime.Now)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "CD4 Test Date must not be a future date | " + enc.CD4TestDate.Value.ToString("dd-MMM_yyyy"),
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/CD4TestDate"
                                });
                                //cnt.CriticalError = true;
                                failedValidation = true;
                            }

                            if (string.IsNullOrEmpty(enc.VisitID))
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Visit ID is empty",
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/VisitID"
                                });
                            }

                            if (enc.Weight != 0 && enc.Weight > 200)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Weight is outside of normal range of 200. Supplied value is  | " + enc.Weight,
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/Weight"
                                });
                                failedValidation = true;
                            }
                            if (enc.ChildHeight != 0 && enc.ChildHeight > 200)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Child Height is outside of normal range of 200. Supplied value is  | " + enc.ChildHeight,
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/ChildHeight"
                                });
                                failedValidation = true;
                            }
                            if (enc.DurationOnArt != 0 && enc.DurationOnArt > Int32.MaxValue)
                            {
                                error.Add(new ErrorDetails
                                {
                                    ErrorMessage = "Duration On Art is excessive. Supplied value is |" + enc.DurationOnArt,
                                    FileName = msg.FileName,
                                    PatientIdentifier = patientIdentifier,
                                    DataElement = "Encounters/CD4TestDate"
                                });
                                failedValidation = true;
                            }

                            if (failedValidation)
                            {
                                continue;
                            }

                            var dtenc = new dtoHIVEncounter();
                            enc.CopyPropertiesTo(dtenc);
                            if (enc.ARVDrugRegimen != null)
                            {
                                dtenc.ARVDrugRegimenCode = enc.ARVDrugRegimen.Code;
                                dtenc.ARVDrugRegimenCodeDesc = enc.ARVDrugRegimen.CodeDescTxt;
                            }
                            if (enc.CotrimoxazoleDose != null)
                            {
                                dtenc.CotrimoxazoleDoseCode = enc.CotrimoxazoleDose.Code;
                                dtenc.CotrimoxazoleDoseCodeDesc = enc.CotrimoxazoleDose.CodeDescTxt;
                            }
                            if (enc.INHDose != null)
                            {
                                dtenc.INHDoseCode = enc.INHDose.Code;
                                dtenc.INHDoseCodeDesc = enc.INHDose.CodeDescTxt;
                            }

                            dtenc.Condition = condition;
                            Encounters.Add(dtenc);
                        }
                    }


                    //hiv questions
                    var HIVQuestions = new dtoHIVQuestions();
                    if (cdt.ConditionSpecificQuestions != null && cdt.ConditionSpecificQuestions.HIVQuestions != null)
                    {
                        if (!cdt.ConditionSpecificQuestions.HIVQuestions.ARTStartDate.HasValue)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "ART Start Date is missing",
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/ARTStartDate"
                            });
                            // cnt.CriticalError = true;
                        }
                        else if (cdt.ConditionSpecificQuestions.HIVQuestions.ARTStartDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "ART Start Date must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.ARTStartDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/ARTStartDate"
                            });
                            cnt.CriticalError = true;
                        }
                        if (!cdt.ConditionSpecificQuestions.HIVQuestions.EnrolledInHIVCareDate.HasValue)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "Enrolled In HIV Care Date is required field",
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/EnrolledInHIVCareDate"
                            });
                            //cnt.CriticalError = true;
                        }
                        else if (cdt.ConditionSpecificQuestions.HIVQuestions.EnrolledInHIVCareDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "Enrolled In HIV Care Date must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.EnrolledInHIVCareDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/EnrolledInHIVCareDate"
                            });
                            cnt.CriticalError = true;
                        }
                        if (cdt.ConditionSpecificQuestions.HIVQuestions.DeathDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.DeathDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Death Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.DeathDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/DeathDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.ConditionSpecificQuestions.HIVQuestions.FirstConfirmedHIVTestDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.FirstConfirmedHIVTestDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when First Confirmed HIV Test Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.FirstConfirmedHIVTestDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/FirstConfirmedHIVTestDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.ConditionSpecificQuestions.HIVQuestions.InitialAdherenceCounselingCompletedDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.InitialAdherenceCounselingCompletedDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Initial Adherence Counseling Completed Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.InitialAdherenceCounselingCompletedDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/InitialAdherenceCounselingCompletedDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.ConditionSpecificQuestions.HIVQuestions.MedicallyEligibleDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.MedicallyEligibleDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Medically Eligible Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.MedicallyEligibleDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/MedicallyEligibleDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Transferred In Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/TransferredInDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.ConditionSpecificQuestions.HIVQuestions.TransferredOutDate.HasValue && cdt.ConditionSpecificQuestions.HIVQuestions.TransferredOutDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Transferred Out Date is supplied, it must not be a future date |" + cdt.ConditionSpecificQuestions.HIVQuestions.TransferredOutDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "HIVQuestions/TransferredOutDate"
                            });
                            cnt.CriticalError = true;
                        }


                        cdt.ConditionSpecificQuestions.HIVQuestions.CopyPropertiesTo(HIVQuestions);
                        if (cdt.ConditionSpecificQuestions.HIVQuestions.FacilityReferredTo != null && !string.IsNullOrEmpty(cdt.ConditionSpecificQuestions.HIVQuestions.FacilityReferredTo.FacilityID))
                        {


                            OnboardedFacility FacilityReferredTo = ConvertToEntityFacility(cdt.ConditionSpecificQuestions.HIVQuestions.FacilityReferredTo, patientIdentifier, msg.FileName, "HIVQuestions/FacilityReferredTo", allFaciltyDictionary, ref error);
                            HIVQuestions.FacilityReferredTo = FacilityReferredTo;
                        }
                        if (cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInFrom != null && !string.IsNullOrEmpty(cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInFrom.FacilityID))
                        {
                            OnboardedFacility TransferredInFrom = ConvertToEntityFacility(cdt.ConditionSpecificQuestions.HIVQuestions.TransferredInFrom, patientIdentifier, msg.FileName, "HIVQuestions/TransferredInFrom", allFaciltyDictionary, ref error);
                            HIVQuestions.TransferredInFrom = TransferredInFrom;
                        }
                    }

                    //common question
                    var CommonQuestions = new dtoCommonQuestions();
                    cdt.CommonQuestions.CopyPropertiesTo(CommonQuestions);
                    if (cdt.CommonQuestions != null)
                    {
                        if (cdt.CommonQuestions.DateOfFirstReport.HasValue && cdt.CommonQuestions.DateOfFirstReport.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Date Of First Report is supplied, it must not be a future date |" + cdt.CommonQuestions.DateOfFirstReport.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "CommonQuestions/DateOfFirstReport"
                            });
                            cnt.CriticalError = true;
                        }
                        if (cdt.CommonQuestions.DateOfLastReport.HasValue && cdt.CommonQuestions.DateOfLastReport.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Date Of Last Report is supplied, it must not be a future date |" + cdt.CommonQuestions.DateOfLastReport.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "CommonQuestions/DateOfLastReport"
                            });
                            cnt.CriticalError = true;
                        }
                        if (cdt.CommonQuestions.DiagnosisDate.HasValue && cdt.CommonQuestions.DiagnosisDate.Value > DateTime.Now)
                        {
                            error.Add(new ErrorDetails
                            {
                                ErrorMessage = "when Diagnosis Date is supplied, it must not be a future date |" + cdt.CommonQuestions.DiagnosisDate.Value.ToString("dd-MMM-yyyy"),
                                FileName = msg.FileName,
                                PatientIdentifier = patientIdentifier,
                                DataElement = "CommonQuestions/DiagnosisDate"
                            });
                            cnt.CriticalError = true;
                        }

                        if (cdt.CommonQuestions.DiagnosisFacility != null && !string.IsNullOrEmpty(cdt.CommonQuestions.DiagnosisFacility.FacilityID))
                        {
                            CommonQuestions.DiagnosisFacility = ConvertToEntityFacility(cdt.CommonQuestions.DiagnosisFacility, patientIdentifier, msg.FileName, "CommonQuestions/DiagnosisFacility", allFaciltyDictionary, ref error);
                        }
                    }

                    HIVQuestions.Condition = condition;
                    condition.HIVQuestions = HIVQuestions;
                    condition.ConditionCode = string.IsNullOrEmpty(cdt.ConditionCode) ? "86406008" : cdt.ConditionCode;
                    condition.ProgramArea = cdt.ProgramArea.ProgramAreaCode;
                    condition.CommonQuestions = CommonQuestions;
                    //condition.ConditionSpecificQuestions = ConditionSpecificQuestions;
                    condition.Encounters = Encounters;
                    condition.Immunization = Immunization;
                    condition.LaboratoryReport = LaboratoryReport;
                    condition.PatientAddress = PatientAddress;
                    condition.Regimen = Regimen;
                    condition.Container = cnt;
                    //ConditionSpecificQuestions.Condition = condition;
                    conditions.Add(condition);
                }
            }

            cnt.MessageHeader = header;
            cnt.PatientDemographics = PatientDemographics;
            cnt.Condition = conditions;
            cnt.PatientIdentifier = PatientDemographics.PatientIdentifier;
            cnt.FileName = msg.FileName;

            cnt.Errors = error;
            return cnt;
        }

        OnboardedFacility ConvertToEntityFacility(Facility modelfacility, string patientId, string filename, string dataElement, Dictionary<string, OnboardedFacility> Facilities, ref List<ErrorDetails> error)
        {
            OnboardedFacility fac = null;

            if (modelfacility == null)
            {
                return null;
            }
            else
            {
                string facilityId = modelfacility.FacilityID;

                if (string.IsNullOrEmpty(facilityId) || string.IsNullOrEmpty(facilityId.Trim()))
                {
                    error.Add(new ErrorDetails
                    {
                        ErrorMessage = "empty Facility Id suppplied",
                        FileName = filename,
                        PatientIdentifier = patientId,
                        DataElement = dataElement,
                    });
                    return fac;
                }

                Facilities.TryGetValue(facilityId.ToLower().Trim(), out fac);
            }
            return fac;
        }

        private Container ConvertToXMLModel(string xmlContent, out string err)
        {
            err = "";
            Container container = null;
            XmlDocument xdoc = new XmlDocument();
            string jsonText = "";
            var format = "yyyy-M-d";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            try
            {
                xmlContent = XDocument.Parse(xmlContent).ToString();
                xdoc.LoadXml(xmlContent);
                if (xdoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    xdoc.RemoveChild(xdoc.FirstChild);
                }
                jsonText = JsonConvert.SerializeXmlNode(xdoc, Newtonsoft.Json.Formatting.None, true);

                // var _container = JsonConvert.DeserializeObject<List<Container>>(jsonText, dateTimeConverter);

                container = JsonConvert.DeserializeObject<Container>(jsonText, dateTimeConverter);

            }
            catch (Exception ex)
            {
                err = Logger.LogError(ex);
            }
            return container;
        }

        private string ReadFileStream(string filepath, out string err)
        {
            err = "";
            string xmlContent = "";
            try
            {
                using (StreamReader reader = new StreamReader(filepath, Encoding.UTF8))
                {
                    xmlContent = reader.ReadToEnd();
                }
                xmlContent = xmlContent.Replace(" & ", " &amp; ");  //SecurityElement.Escape(xmlContent);
                xmlContent = xmlContent.Replace("'", "&apos;");

                xmlContent = CleanInvalidXmlChars(xmlContent);

                var xdoc = XDocument.Parse(xmlContent);
                if (xdoc != null)
                {
                    return xmlContent;
                }
                else
                {
                    err = "empty xml file";
                    return null;
                }
            }
            catch (Exception ex)
            {
                err = Logger.LogError(ex);
                if (!string.IsNullOrEmpty(err))
                {
                    err = err.Replace("System.Nullable`1[DAL.Utility.", "");
                }
                return null;
            }
        }

        private static string CleanInvalidXmlChars(string text)
        {
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return Regex.Replace(text, re, "");
        }

    }
}
