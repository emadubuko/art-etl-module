using FluentNHibernate.Mapping;

namespace ART.DAL.Entities
{
    public class PatientdemographicsMap : ClassMap<dtoPatientDemographics>
    {
        public class ContainerMap : ClassMap<dtoContainer>
        { 
            public ContainerMap()
            {
                Table("Container");
                LazyLoad();
                Id(x => x.Id).GeneratedBy.Identity().Column("Id");
                //References(x => x.MessageHeader).Column("MessageHeaderId").Cascade.Delete();
                //References(x => x.PatientDemographics).Column("PatientDemographicId");
                HasMany(x => x.Condition).KeyColumn("ContainerId").Inverse().Cascade.Delete();
                Map(x => x.PatientIdentifier);
                Map(m => m.LastUpdatedDate);
                Map(m => m.BatchNumber);
                References(m => m.UploadedBy).Column("UploadedBy"); 
            }
        }

        public class MessageheaderMap : ClassMap<dtoMessageHeader>
        { 
            public MessageheaderMap()
            {
                Table("MessageHeader");
                LazyLoad();
                Id(x => x.Id).GeneratedBy.Identity().Column("Id");
                References(x => x.IP).Column("IP");
                Map(x => x.MessageStatusCode).Column("MessageStatusCode").Length(255);
                Map(x => x.MessageCreationDateTime).Column("MessageCreationDateTime");
                Map(x => x.MessageSchemaVersion).Column("MessageSchemaVersion").Precision(19).Scale(5);
                Map(x => x.MessageUniqueID).Column("MessageUniqueID").Length(255);
                References(x => x.Container).Column("ContainerId").Cascade.Delete(); 
            }
        }


        public PatientdemographicsMap()
        {
            Table("PatientDemographics");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.TreatmentFacility).Column("TreatmentFacilityId").Cascade.None();
            References(x => x.Container).Cascade.Delete().Column("ContainerId");
            Map(x => x.PatientIdentifier).Column("PatientIdentifier").Length(255);
            Map(x => x.PatientDateOfBirth).Column("PatientDateOfBirth");
            Map(x => x.PatientDateOfBirthSpecified).Column("PatientDateOfBirthSpecified");
            Map(x => x.PatientSexCode).Column("PatientSexCode").Length(255);
            Map(x => x.PatientSexCodeSpecified).Column("PatientSexCodeSpecified");
            Map(x => x.PatientDeceasedIndicator).Column("PatientDeceasedIndicator");
            Map(x => x.PatientDeceasedIndicatorSpecified).Column("PatientDeceasedIndicatorSpecified");
            Map(x => x.PatientDeceasedDate).Column("PatientDeceasedDate");
            Map(x => x.PatientDeceasedDateSpecified).Column("PatientDeceasedDateSpecified");
            Map(x => x.PatientPrimaryLanguageCode).Column("PatientPrimaryLanguageCode").Length(255);
            Map(x => x.PatientEducationLevelCode).Column("PatientEducationLevelCode").Length(255);
            Map(x => x.PatientEducationLevelCodeSpecified).Column("PatientEducationLevelCodeSpecified");
            Map(x => x.PatientOccupationCode).Column("PatientOccupationCode").Length(255);
            Map(x => x.PatientOccupationCodeSpecified).Column("PatientOccupationCodeSpecified");
            Map(x => x.PatientMaritalStatusCode).Column("PatientMaritalStatusCode").Length(255);
            Map(x => x.PatientMaritalStatusCodeSpecified).Column("PatientMaritalStatusCodeSpecified");
            Map(x => x.StateOfNigeriaOriginCode).Column("StateOfNigeriaOriginCode").Length(255);
            HasMany(x => x.Identifier).Inverse()
                .KeyColumn("PatientDemographicsId")
                .Cascade.Delete();
            Map(x => x.PatientNotes);
        }
    }

    public class ConditionMap : ClassMap<dtoCondition>
    {
        public ConditionMap()
        {
            Table("Condition");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ProgramArea).Column("ProgramArea");
            References(x => x.PatientAddress).Column("PatientAddressId").Cascade.Delete();//.Cascade.All();
            References(x => x.CommonQuestions).Column("CommonQuestionsId").Cascade.Delete();//.Cascade.All();
            References(x => x.HIVQuestions).Column("HIVQuestionsId"); 
            Map(x => x.ConditionCode).Column("ConditionCode").Length(255);
            HasMany(x => x.Encounters).Inverse().KeyColumn("ConditionId").Cascade.Delete();
            HasMany(x => x.Immunization).Inverse().KeyColumn("ConditionId").Cascade.Delete();
            HasMany(x => x.LaboratoryReport).Inverse().KeyColumn("ConditionId").Cascade.Delete();
            HasMany(x => x.Regimen).Inverse().KeyColumn("ConditionId").Cascade.Delete();
            References(x => x.Container).Column("ContainerId").Cascade.Delete();
        }
    }

    public class AddressMap : ClassMap<dtoAddress>
    {
        public AddressMap()
        {
            Table("Address");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.AddressTypeCode).Column("AddressTypeCode").Length(255);
            Map(x => x.WardVillage).Column("WardVillage").Length(255);
            Map(x => x.Town).Column("Town").Length(255);
            Map(x => x.LGACode).Column("LGACode").Length(255);
            Map(x => x.StateCode).Column("StateCode").Length(255);
            Map(x => x.CountryCode).Column("CountryCode").Length(255);
            Map(x => x.PostalCode).Column("PostalCode").Length(255);
            Map(x => x.OtherAddressInformation).Column("OtherAddressInformation").Length(255);
            References(x => x.Condition).Column("Condition");
        }
    }

    public class CommonquestionsMap : ClassMap<dtoCommonQuestions>
    {
        public CommonquestionsMap()
        {
            Table("CommonQuestions");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.DiagnosisFacility).Column("DiagnosisFacilityId");
            Map(x => x.HospitalNumber).Column("HospitalNumber").Length(255);
            Map(x => x.DateOfFirstReport).Column("DateOfFirstReport");
            Map(x => x.DateOfFirstReportSpecified).Column("DateOfFirstReportSpecified");
            Map(x => x.DateOfLastReport).Column("DateOfLastReport");
            Map(x => x.DateOfLastReportSpecified).Column("DateOfLastReportSpecified");
            Map(x => x.DiagnosisDate).Column("DiagnosisDate");
            Map(x => x.DiagnosisDateSpecified).Column("DiagnosisDateSpecified");
            Map(x => x.PatientDieFromThisIllness).Column("PatientDieFromThisIllness");
            Map(x => x.PatientDieFromThisIllnessSpecified).Column("PatientDieFromThisIllnessSpecified");
            Map(x => x.PatientPregnancyStatusCode).Column("PatientPregnancyStatusCode").Length(255);
            Map(x => x.PatientPregnancyStatusCodeSpecified).Column("PatientPregnancyStatusCodeSpecified");
            Map(x => x.EstimatedDeliveryDate).Column("EstimatedDeliveryDate");
            Map(x => x.EstimatedDeliveryDateSpecified).Column("EstimatedDeliveryDateSpecified");
            Map(x => x.PatientAge).Column("PatientAge");
            Map(x => x.PatientAgeSpecified).Column("PatientAgeSpecified");
            References(x => x.Condition).Column("ConditionId");//.Cascade.Delete();
        }
    }

    public class HivencounterMap : ClassMap<dtoHIVEncounter>
    {

        public HivencounterMap()
        {
            Table("HIVEncounter");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ARVDrugRegimenCode);
            Map(x => x.ARVDrugRegimenCodeDesc);
            Map(x => x.CotrimoxazoleDoseCode);
            Map(x => x.CotrimoxazoleDoseCodeDesc);
            Map(x => x.INHDoseCode);
            Map(x => x.INHDoseCodeDesc);

            References(x => x.Condition).Column("ConditionId");
            Map(x => x.VisitID).Column("VisitID").Length(255);
            Map(x => x.VisitDate).Column("VisitDate");
            Map(x => x.DurationOnArt).Column("DurationOnArt").Precision(10);
            Map(x => x.DurationOnArtSpecified).Column("DurationOnArtSpecified");
            Map(x => x.Weight).Column("Weight").Precision(10);
            Map(x => x.WeightSpecified).Column("WeightSpecified");
            Map(x => x.ChildHeight).Column("ChildHeight").Precision(10);
            Map(x => x.ChildHeightSpecified).Column("ChildHeightSpecified");
            Map(x => x.BloodPressure).Column("BloodPressure").Length(255);
            Map(x => x.EDDandPMTCTLink).Column("EDDandPMTCTLink").Length(255);
            Map(x => x.EDDandPMTCTLinkSpecified).Column("EDDandPMTCTLinkSpecified");
            Map(x => x.PatientFamilyPlanningCode).Column("PatientFamilyPlanningCode").Length(255);
            Map(x => x.PatientFamilyPlanningCodeSpecified).Column("PatientFamilyPlanningCodeSpecified");
            Map(x => x.PatientFamilyPlanningMethodCode).Column("PatientFamilyPlanningMethodCode").Length(255);
            Map(x => x.FunctionalStatus).Column("FunctionalStatus").Length(255);
            Map(x => x.FunctionalStatusSpecified).Column("FunctionalStatusSpecified");
            Map(x => x.WHOClinicalStage).Column("WHOClinicalStage").Length(255);
            Map(x => x.WHOClinicalStageSpecified).Column("WHOClinicalStageSpecified");
            Map(x => x.TBStatus).Column("TBStatus").Length(255);
            Map(x => x.TBStatusSpecified).Column("TBStatusSpecified");
            Map(x => x.OtherOIOtherProblems).Column("OtherOIOtherProblems").Length(255);
            Map(x => x.NotedSideEffects).Column("NotedSideEffects").Length(255);
            Map(x => x.ARVDrugAdherence).Column("ARVDrugAdherence").Length(255);
            Map(x => x.ARVDrugAdherenceSpecified).Column("ARVDrugAdherenceSpecified");
            Map(x => x.WhyPoorFairARVDrugAdherence).Column("WhyPoorFairARVDrugAdherence").Length(255);
            Map(x => x.WhyPoorFairARVDrugAdherenceSpecified).Column("WhyPoorFairARVDrugAdherenceSpecified");
            Map(x => x.CotrimoxazoleAdherence).Column("CotrimoxazoleAdherence").Length(255);
            Map(x => x.CotrimoxazoleAdherenceSpecified).Column("CotrimoxazoleAdherenceSpecified");
            Map(x => x.WhyPoorFairCotrimoxazoleDrugAdherence).Column("WhyPoorFairCotrimoxazoleDrugAdherence").Length(255);
            Map(x => x.WhyPoorFairCotrimoxazoleDrugAdherenceSpecified).Column("WhyPoorFairCotrimoxazoleDrugAdherenceSpecified");
            Map(x => x.INHAdherence).Column("INHAdherence").Length(255);
            Map(x => x.INHAdherenceSpecified).Column("INHAdherenceSpecified");
            Map(x => x.WhyPoorFairINHDrugAdherence).Column("WhyPoorFairINHDrugAdherence").Length(255);
            Map(x => x.WhyPoorFairINHDrugAdherenceSpecified).Column("WhyPoorFairINHDrugAdherenceSpecified");
            Map(x => x.CD4).Column("CD4").Precision(10);
            Map(x => x.CD4Specified).Column("CD4Specified");
            Map(x => x.CD4TestDate).Column("CD4TestDate");
            Map(x => x.CD4TestDateSpecified).Column("CD4TestDateSpecified");
            Map(x => x.NextAppointmentDate).Column("NextAppointmentDate");
            Map(x => x.NextAppointmentDateSpecified).Column("NextAppointmentDateSpecified");
        }
    }

    public class IdentifierMap : ClassMap<dtoIdentifier>
    {

        public IdentifierMap()
        {
            Table("Identifier");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.PatientDemographics).Column("PatientDemographicsId");
            Map(x => x.IDNumber).Column("IDNumber").Length(255);
            Map(x => x.IDTypeCode).Column("IDTypeCode").Length(255);
        }
    }

    public class LaboratoryreportMap : ClassMap<dtoLaboratoryReport>
    { 
        public LaboratoryreportMap()
        {
            Table("LaboratoryReport");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.Condition).Column("ConditionId");
            Map(x => x.VisitID).Column("VisitID").Length(255);
            Map(x => x.VisitDate).Column("VisitDate");
            Map(x => x.LaboratoryTestIdentifier).Column("LaboratoryTestIdentifier").Length(255);
            Map(x => x.CollectionDate).Column("CollectionDate");
            Map(x => x.CollectionDateSpecified).Column("CollectionDateSpecified");
            Map(x => x.BaselineRepeatCode).Column("BaselineRepeatCode").Length(255);
            Map(x => x.BaselineRepeatCodeSpecified).Column("BaselineRepeatCodeSpecified");
            Map(x => x.ARTStatusCode).Column("ARTStatusCode").Length(255);
            Map(x => x.Clinician).Column("Clinician").Length(255);
            Map(x => x.ReportedBy).Column("ReportedBy").Length(255);
            Map(x => x.CheckedBy).Column("CheckedBy").Length(255);
            HasMany(x => x.LaboratoryOrderAndResult).Inverse().KeyColumn("LaboratoryReportId").Cascade.Delete();
        }
    }

    public class LaboratoryorderandresultMap : ClassMap<dtoLaboratoryOrderAndResult>
    { 
        public LaboratoryorderandresultMap()
        {
            Table("LaboratoryOrderAndResult");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.LaboratoryOrderedTestCode);
            Map(x => x.LaboratoryResultedTestCode);
            Map(x => x.LaboratoryResultedTestDesc);
            Map(x => x.LaboratoryOrderedTestCodeDesc);
            Map(x => x.LaboratoryResultValue).Column("LaboratoryResultNumericValue");
            Map(x => x.LaboratoryResult_AnswerCode);
            Map(x => x.LaboratoryResult_AnswerCodeDescTxt);
            Map(x => x.LaboratoryResult_AnswerCodeSystemCode);
            Map(x => x.LaboratoryResult_AnswerDate);
            Map(x => x.LaboratoryResult_AnswerText);

            References(x => x.LaboratoryReport).Column("LaboratoryReportId");//.Cascade.All();
            Map(x => x.LaboratoryTestTypeCode).Column("LaboratoryTestTypeCode").Length(255);
            Map(x => x.OrderedTestDate).Column("OrderedTestDate");
            Map(x => x.OrderedTestDateSpecified).Column("OrderedTestDateSpecified");
            Map(x => x.ResultedTestDate).Column("ResultedTestDate");
            Map(x => x.ResultedTestDateSpecified).Column("ResultedTestDateSpecified");
            Map(x => x.OtherLaboratoryInformation).Column("OtherLaboratoryInformation").Length(255);
        }
    }


    public class ImmunizationMap : ClassMap<dtoImmunization>
    {
        public ImmunizationMap()
        {
            Table("Immunization");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ImmunizationCode).Column("ImmunizationCode");
            Map(x => x.ImmunizationCodeDesc).Column("CodeDesc");
            References(x => x.Condition).Column("ConditionId");
            Map(x => x.VisitID).Column("VisitID").Length(255);
            Map(x => x.VisitDate).Column("VisitDate");
            Map(x => x.ImmunizationIdentifier).Column("ImmunizationIdentifier").Length(255);
            Map(x => x.ImmunizationDate).Column("ImmunizationDate");
            Map(x => x.LotNumber).Column("LotNumber").Length(255);
            Map(x => x.ExpirationDate).Column("ExpirationDate");
            Map(x => x.ExpirationDateSpecified).Column("ExpirationDateSpecified");
            Map(x => x.ManufacturerCode).Column("ManufacturerCode").Length(255);
            Map(x => x.SiteCode).Column("SiteCode").Length(255);
            Map(x => x.RouteCode).Column("RouteCode").Length(255);
            Map(x => x.Dose).Column("Dose").Length(255);
            Map(x => x.SelfReported).Column("SelfReported");
            Map(x => x.SelfReportedSpecified).Column("SelfReportedSpecified");
            Map(x => x.Clinician).Column("Clinician").Length(255);
            Map(x => x.PerformedBy).Column("PerformedBy").Length(255);
            Map(x => x.CheckedBy).Column("CheckedBy").Length(255);
        }
    }

    public class RegimenMap : ClassMap<dtoRegimen>
    {

        public RegimenMap()
        {
            Table("Regimen");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.PrescribedRegimenCode).Column("PrescribedRegimenCode");
            Map(x => x.PrescribedRegimenCodeCodeDesc).Column("CodeDesc");
            References(x => x.Condition).Column("ConditionId");
            Map(x => x.VisitID).Column("VisitID").Length(255);
            Map(x => x.VisitDate).Column("VisitDate");
            Map(x => x.ReasonForRegimenSwitchSubs).Column("ReasonForRegimenSwitchSubs").Length(255);
            Map(x => x.PrescribedRegimenTypeCode).Column("PrescribedRegimenTypeCode").Length(255);
            Map(x => x.PrescribedRegimenLineCode).Column("PrescribedRegimenLineCode").Length(255);
            Map(x => x.PrescribedRegimenDuration).Column("PrescribedRegimenDuration").Length(255);
            Map(x => x.PrescribedRegimenDispensedDate).Column("PrescribedRegimenDispensedDate");
            Map(x => x.PrescribedRegimenDispensedDateSpecified).Column("PrescribedRegimenDispensedDateSpecified");
            Map(x => x.DateRegimenStarted).Column("DateRegimenStarted");
            Map(x => x.DateRegimenEnded).Column("DateRegimenEnded");
            Map(x => x.DateRegimenEndedSpecified).Column("DateRegimenEndedSpecified");
            Map(x => x.PrescribedRegimenInitialIndicator).Column("PrescribedRegimenInitialIndicator");
            Map(x => x.PrescribedRegimenInitialIndicatorSpecified).Column("PrescribedRegimenInitialIndicatorSpecified");
            Map(x => x.PrescribedRegimenCurrentIndicator).Column("PrescribedRegimenCurrentIndicator");
            Map(x => x.PrescribedRegimenCurrentIndicatorSpecified).Column("PrescribedRegimenCurrentIndicatorSpecified");
            Map(x => x.TypeOfPreviousExposureCode).Column("TypeOfPreviousExposureCode").Length(255);
            Map(x => x.PoorAdherenceIndicator).Column("PoorAdherenceIndicator");
            Map(x => x.PoorAdherenceIndicatorSpecified).Column("PoorAdherenceIndicatorSpecified");
            Map(x => x.ReasonForPoorAdherence).Column("ReasonForPoorAdherence").Length(255);
            Map(x => x.ReasonRegimenEndedCode).Column("ReasonRegimenEndedCode").Length(255);
            Map(x => x.SubstitutionIndicator).Column("SubstitutionIndicator");
            Map(x => x.SubstitutionIndicatorSpecified).Column("SubstitutionIndicatorSpecified");
            Map(x => x.SwitchIndicator).Column("SwitchIndicator");
            Map(x => x.SwitchIndicatorSpecified).Column("SwitchIndicatorSpecified");
        }
    }

    public class HivquestionsMap : ClassMap<dtoHIVQuestions>
    {
        public HivquestionsMap()
        {
            Table("HIVQuestions");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.TransferredInFrom).Column("TransferredInFromId");
            Map(x => x.FirstARTRegimenCode).Column("FirstARTRegimenCode");
            Map(x => x.FirstARTRegimenCodeDesc).Column("FirstARTRegimenDesc");
            References(x => x.FacilityReferredTo).Column("FacilityReferredToId");
            Map(x => x.CareEntryPoint).Column("CareEntryPoint").Length(255);
            Map(x => x.CareEntryPointSpecified).Column("CareEntryPointSpecified");
            Map(x => x.FirstConfirmedHIVTestDate).Column("FirstConfirmedHIVTestDate");
            Map(x => x.FirstConfirmedHIVTestDateSpecified).Column("FirstConfirmedHIVTestDateSpecified");
            Map(x => x.FirstHIVTestMode).Column("FirstHIVTestMode").Length(255);
            Map(x => x.FirstHIVTestModeSpecified).Column("FirstHIVTestModeSpecified");
            Map(x => x.WhereFirstHIVTest).Column("WhereFirstHIVTest").Length(255);
            Map(x => x.PriorArt).Column("PriorArt").Length(255);
            Map(x => x.MedicallyEligibleDate).Column("MedicallyEligibleDate");
            Map(x => x.MedicallyEligibleDateSpecified).Column("MedicallyEligibleDateSpecified");
            Map(x => x.ReasonMedicallyEligible).Column("ReasonMedicallyEligible").Length(255);
            Map(x => x.ReasonMedicallyEligibleSpecified).Column("ReasonMedicallyEligibleSpecified");
            Map(x => x.InitialAdherenceCounselingCompletedDate).Column("InitialAdherenceCounselingCompletedDate");
            Map(x => x.InitialAdherenceCounselingCompletedDateSpecified).Column("InitialAdherenceCounselingCompletedDateSpecified");
            Map(x => x.TransferredInDate).Column("TransferredInDate");
            Map(x => x.TransferredInDateSpecified).Column("TransferredInDateSpecified");
            Map(x => x.TransferredInFromPatId).Column("TransferredInFromPatId").Length(255);
            Map(x => x.ARTStartDate).Column("ARTStartDate");
            Map(x => x.ARTStartDateSpecified).Column("ARTStartDateSpecified");
            Map(x => x.WHOClinicalStageARTStart).Column("WHOClinicalStageARTStart").Length(255);
            Map(x => x.WHOClinicalStageARTStartSpecified).Column("WHOClinicalStageARTStartSpecified");
            Map(x => x.WeightAtARTStart).Column("WeightAtARTStart").Precision(10);
            Map(x => x.WeightAtARTStartSpecified).Column("WeightAtARTStartSpecified");
            Map(x => x.ChildHeightAtARTStart).Column("ChildHeightAtARTStart").Precision(10);
            Map(x => x.ChildHeightAtARTStartSpecified).Column("ChildHeightAtARTStartSpecified");
            Map(x => x.FunctionalStatusStartART).Column("FunctionalStatusStartART").Length(255);
            Map(x => x.FunctionalStatusStartARTSpecified).Column("FunctionalStatusStartARTSpecified");
            Map(x => x.CD4AtStartOfART).Column("CD4AtStartOfART").Length(255);
            Map(x => x.PatientTransferredOut).Column("PatientTransferredOut");
            Map(x => x.PatientTransferredOutSpecified).Column("PatientTransferredOutSpecified");
            Map(x => x.TransferredOutStatus).Column("TransferredOutStatus").Length(255);
            Map(x => x.TransferredOutStatusSpecified).Column("TransferredOutStatusSpecified");
            Map(x => x.TransferredOutDate).Column("TransferredOutDate");
            Map(x => x.TransferredOutDateSpecified).Column("TransferredOutDateSpecified");
            Map(x => x.PatientHasDied).Column("PatientHasDied");
            Map(x => x.PatientHasDiedSpecified).Column("PatientHasDiedSpecified");
            Map(x => x.StatusAtDeath).Column("StatusAtDeath").Length(255);
            Map(x => x.StatusAtDeathSpecified).Column("StatusAtDeathSpecified");
            Map(x => x.DeathDate).Column("DeathDate");
            Map(x => x.DeathDateSpecified).Column("DeathDateSpecified");
            Map(x => x.SourceOfDeathInformation).Column("SourceOfDeathInformation").Length(255);
            Map(x => x.CauseOfDeathHIVRelated).Column("CauseOfDeathHIVRelated").Length(255);
            Map(x => x.CauseOfDeathHIVRelatedSpecified).Column("CauseOfDeathHIVRelatedSpecified");
            Map(x => x.DrugAllergies).Column("DrugAllergies").Length(255);
            Map(x => x.EnrolledInHIVCareDate).Column("EnrolledInHIVCareDate");
            Map(x => x.EnrolledInHIVCareDateSpecified).Column("EnrolledInHIVCareDateSpecified");
            Map(x => x.InitialTBStatus).Column("InitialTBStatus").Length(255);
            Map(x => x.InitialTBStatusSpecified).Column("InitialTBStatusSpecified");
            References(x => x.Condition).Column("ConditionId");
        }
    }

    public class PartnerNotificationServicesMapping : ClassMap<PartnerNotificationServices>
    {
        public PartnerNotificationServicesMapping()
        {
            Table("PartnerNotificationServices");
            LazyLoad();
            Id(x => x.Id);
            Map(x => x.PartnerName);
            Map(x => x.PartnerGender);
        }
    }




}