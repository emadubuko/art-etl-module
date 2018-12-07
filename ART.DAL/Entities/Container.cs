using Common;
using Common.CommonEntities;
using Common.Entities;
using System;
using System.Collections.Generic;

namespace ART.DAL.Entities
{
    public class dtoContainer : BaseT
    {
        public virtual string PatientIdentifier { get; set; }
        public virtual dtoMessageHeader MessageHeader { get; set; }
        public virtual dtoPatientDemographics PatientDemographics { get; set; }
        public virtual ICollection<dtoCondition> Condition { get; set; }
        public virtual string BatchNumber { get; set; }
        public virtual UserProfile UploadedBy { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
         
        //unmapped fields
        public virtual string FileName { get; set; }
        public virtual int FileId { get; set; }
        public virtual List<ErrorDetails> Errors { get; set; }
        public virtual bool CriticalError { get; set; }
    }
          
    public class dtoMessageHeader : BaseT
    {         
        public virtual MessageHeaderTypeMessageStatusCode MessageStatusCode { get; set; }         
        public virtual DateTime? MessageCreationDateTime { get; set; }
        public virtual decimal MessageSchemaVersion { get; set; }         
        public virtual string MessageUniqueID { get; set; }         
        public virtual ImplementingPartners IP { get; set; }
        public virtual dtoContainer Container { get; set; }
    }
      
    public class dtoPatientDemographics : BaseT
    {        
        public virtual string PatientIdentifier { get; set; }         
        public virtual OnboardedFacility TreatmentFacility { get; set; } 
        public virtual dtoContainer Container { get; set; }
        public virtual ICollection<dtoIdentifier> Identifier { get; set; } 
        public virtual DateTime? PatientDateOfBirth { get; set; } 
        public virtual bool PatientDateOfBirthSpecified { get; set; } 
        public virtual PatientDemographicsTypePatientSexCode? PatientSexCode { get; set; } 
        public virtual bool PatientSexCodeSpecified { get; set; } 
        public virtual bool PatientDeceasedIndicator { get; set; } 
        public virtual bool PatientDeceasedIndicatorSpecified { get; set; } 
        public virtual DateTime? PatientDeceasedDate { get; set; } 
        public virtual bool PatientDeceasedDateSpecified { get; set; } 
        public virtual string PatientPrimaryLanguageCode { get; set; } 
        public virtual PatientDemographicsTypePatientEducationLevelCode? PatientEducationLevelCode { get; set; } 
        public virtual bool PatientEducationLevelCodeSpecified { get; set; } 
        public virtual PatientDemographicsTypePatientOccupationCode? PatientOccupationCode { get; set; } 
        public virtual bool PatientOccupationCodeSpecified { get; set; } 
        public virtual PatientDemographicsTypePatientMaritalStatusCode? PatientMaritalStatusCode { get; set; } 
        public virtual bool PatientMaritalStatusCodeSpecified { get; set; } 
        public virtual string StateOfNigeriaOriginCode { get; set; }
        public virtual string PatientNotes{ get; set; } 
    }

    [Serializable()]
    public class dtoIdentifier :BaseT
    { 
        public virtual dtoPatientDemographics PatientDemographics { get; set; }
        public virtual string IDNumber { get; set; }
        public virtual string IDTypeCode { get; set; }
    }


    public class dtoCondition : BaseT
    { 
        public virtual string ConditionCode { get; set; }
        public virtual string ProgramArea { get; set; }
        public virtual dtoAddress PatientAddress { get; set; }
        public virtual dtoCommonQuestions CommonQuestions { get; set; }
        public virtual dtoHIVQuestions HIVQuestions { get; set; }
        
        //public virtual dtoConditionSpecificQuestions ConditionSpecificQuestions { get; set; }
        public virtual ICollection<dtoHIVEncounter> Encounters { get; set; }    
        public virtual ICollection<dtoLaboratoryReport> LaboratoryReport { get; set; }   
        public virtual ICollection<dtoRegimen> Regimen { get; set; }    
        public virtual ICollection<dtoImmunization> Immunization { get; set; }   

        public virtual dtoContainer Container { get; set; }
    }

    public class dtoAddress : BaseT
    {
        public virtual string AddressTypeCode { get; set; }
        public virtual string WardVillage { get; set; }
        public virtual string Town { get; set; }
        public virtual string LGACode { get; set; }
        public virtual string StateCode { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string OtherAddressInformation { get; set; }
        public virtual dtoCondition Condition { get; set; }
    }

    public class dtoCommonQuestions : BaseT
    {
        public virtual OnboardedFacility DiagnosisFacility { get; set; }
        public virtual string HospitalNumber { get; set; }
        public virtual DateTime? DateOfFirstReport { get; set; }
        public virtual bool? DateOfFirstReportSpecified { get; set; }
        public virtual DateTime? DateOfLastReport { get; set; }
        public virtual bool? DateOfLastReportSpecified { get; set; }
        public virtual DateTime? DiagnosisDate { get; set; }
        public virtual bool? DiagnosisDateSpecified { get; set; }
        public virtual bool? PatientDieFromThisIllness { get; set; }
        public virtual bool? PatientDieFromThisIllnessSpecified { get; set; }
        public virtual CommonQuestionsTypePatientPregnancyStatusCode? PatientPregnancyStatusCode { get; set; }
        public virtual bool? PatientPregnancyStatusCodeSpecified { get; set; }
        public virtual DateTime? EstimatedDeliveryDate { get; set; }
        public virtual bool? EstimatedDeliveryDateSpecified { get; set; }
        public virtual int? PatientAge { get; set; }
        public virtual bool? PatientAgeSpecified { get; set; }
        public virtual dtoCondition Condition { get; set; }
    }

    //public class dtoConditionSpecificQuestions : BaseT
    //{
    //    public virtual dtoHIVQuestions HIVQuestions { get; set; }
    //    public virtual dtoCondition Condition { get; set; }
    //}

    public class dtoHIVEncounter : BaseT
    {
        public virtual string ARVDrugRegimenCode { get; set; }
        public virtual string ARVDrugRegimenCodeDesc { get; set; }

        public virtual string CotrimoxazoleDoseCode { get; set; }
        public virtual string CotrimoxazoleDoseCodeDesc { get; set; }

        public virtual string INHDoseCode { get; set; }
        public virtual string INHDoseCodeDesc { get; set; } 

        public virtual dtoCondition Condition { get; set; }
        public virtual string VisitID { get; set; }
        public virtual DateTime? VisitDate { get; set; }
        public virtual double? DurationOnArt { get; set; }
        public virtual bool? DurationOnArtSpecified { get; set; }
        public virtual int? Weight { get; set; }
        public virtual bool? WeightSpecified { get; set; }
        public virtual int? ChildHeight { get; set; }
        public virtual bool? ChildHeightSpecified { get; set; }
        public virtual string BloodPressure { get; set; }
        public virtual HIVEncounterTypeEDDandPMTCTLink? EDDandPMTCTLink { get; set; }
        public virtual bool? EDDandPMTCTLinkSpecified { get; set; }
        public virtual HIVEncounterTypePatientFamilyPlanningCode? PatientFamilyPlanningCode { get; set; }
        public virtual bool? PatientFamilyPlanningCodeSpecified { get; set; }
        public virtual string PatientFamilyPlanningMethodCode { get; set; }
        public virtual HIVEncounterTypeFunctionalStatus? FunctionalStatus { get; set; }
        public virtual bool? FunctionalStatusSpecified { get; set; }
        public virtual HIVEncounterTypeWHOClinicalStage? WHOClinicalStage { get; set; }
        public virtual bool? WHOClinicalStageSpecified { get; set; }
        public virtual HIVEncounterTypeTBStatus? TBStatus { get; set; }
        public virtual bool? TBStatusSpecified { get; set; }
        public virtual string OtherOIOtherProblems { get; set; }
        public virtual string NotedSideEffects { get; set; }
        public virtual HIVEncounterTypeARVDrugAdherence? ARVDrugAdherence { get; set; }
        public virtual bool? ARVDrugAdherenceSpecified { get; set; }
        public virtual HIVEncounterTypeWhyPoorFairARVDrugAdherence? WhyPoorFairARVDrugAdherence { get; set; }
        public virtual bool? WhyPoorFairARVDrugAdherenceSpecified { get; set; }
        public virtual HIVEncounterTypeCotrimoxazoleAdherence? CotrimoxazoleAdherence { get; set; }
        public virtual bool? CotrimoxazoleAdherenceSpecified { get; set; }
        public virtual HIVEncounterTypeWhyPoorFairCotrimoxazoleDrugAdherence? WhyPoorFairCotrimoxazoleDrugAdherence { get; set; }
        public virtual bool? WhyPoorFairCotrimoxazoleDrugAdherenceSpecified { get; set; }
        public virtual HIVEncounterTypeINHAdherence? INHAdherence { get; set; }
        public virtual bool? INHAdherenceSpecified { get; set; }
        public virtual HIVEncounterTypeWhyPoorFairINHDrugAdherence? WhyPoorFairINHDrugAdherence { get; set; }
        public virtual bool? WhyPoorFairINHDrugAdherenceSpecified { get; set; }
        public virtual double? CD4 { get; set; }
        public virtual bool? CD4Specified { get; set; }
        public virtual DateTime? CD4TestDate { get; set; }
        public virtual bool? CD4TestDateSpecified { get; set; }
        public virtual DateTime? NextAppointmentDate { get; set; }
        public virtual bool? NextAppointmentDateSpecified { get; set; }
    }

    public class dtoLaboratoryReport : BaseT
    {
        public virtual dtoCondition Condition { get; set; }
        public virtual string VisitID { get; set; }
        public virtual DateTime? VisitDate { get; set; }
        public virtual string LaboratoryTestIdentifier { get; set; }
        public virtual DateTime? CollectionDate { get; set; }
        public virtual bool? CollectionDateSpecified { get; set; }
        public virtual LaboratoryReportTypeBaselineRepeatCode? BaselineRepeatCode { get; set; }
        public virtual bool? BaselineRepeatCodeSpecified { get; set; }
        public virtual string ARTStatusCode { get; set; }
        public virtual string Clinician { get; set; }
        public virtual string ReportedBy { get; set; }
        public virtual string CheckedBy { get; set; }
        public virtual ICollection<dtoLaboratoryOrderAndResult> LaboratoryOrderAndResult { get; set; }
    }

    public class dtoLaboratoryOrderAndResult : BaseT
    { 
        public virtual string LaboratoryOrderedTestCode { get; set; }
        public virtual string LaboratoryOrderedTestCodeDesc { get; set; }

        public virtual string LaboratoryResultedTestCode { get; set; }
        public virtual string LaboratoryResultedTestDesc { get; set; }

        //lab result

        //numeric result
        public virtual string LaboratoryResultValue { get; set; }

        //AnswerCode
        public virtual string LaboratoryResult_AnswerCode { get; set; }
        public virtual string LaboratoryResult_AnswerCodeDescTxt { get; set; }
        public virtual string LaboratoryResult_AnswerCodeSystemCode { get; set; }

        //Answer Date
        public virtual DateTime? LaboratoryResult_AnswerDate { get; set; }

        //Answer Text
        public virtual string LaboratoryResult_AnswerText { get; set; }
        



        public virtual dtoLaboratoryReport LaboratoryReport { get; set; }
        public virtual string LaboratoryTestTypeCode { get; set; }
        public virtual DateTime? OrderedTestDate { get; set; }
        public virtual bool? OrderedTestDateSpecified { get; set; }
        public virtual DateTime? ResultedTestDate { get; set; }
        public virtual bool? ResultedTestDateSpecified { get; set; }
        public virtual string OtherLaboratoryInformation { get; set; }
    }

    public class dtoImmunization : BaseT
    {
        //public virtual CodedSimple CodedSimple { get; set; }
        public virtual string ImmunizationCode { get; set; }
        public virtual string ImmunizationCodeDesc { get; set; }
        public virtual dtoCondition Condition { get; set; }
        public virtual string VisitID { get; set; }
        public virtual DateTime? VisitDate { get; set; }
        public virtual string ImmunizationIdentifier { get; set; }
        public virtual DateTime? ImmunizationDate { get; set; }
        public virtual string LotNumber { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }
        public virtual bool? ExpirationDateSpecified { get; set; }
        public virtual string ManufacturerCode { get; set; }
        public virtual string SiteCode { get; set; }
        public virtual string RouteCode { get; set; }
        public virtual string Dose { get; set; }
        public virtual bool? SelfReported { get; set; }
        public virtual bool? SelfReportedSpecified { get; set; }
        public virtual string Clinician { get; set; }
        public virtual string PerformedBy { get; set; }
        public virtual string CheckedBy { get; set; }
    }
      
    public class dtoRegimen : BaseT
    {
        public virtual string PrescribedRegimenCode { get; set; } 
        public virtual string PrescribedRegimenCodeCodeDesc { get; set; }
        public virtual dtoCondition Condition { get; set; }
        public virtual string VisitID { get; set; }
        public virtual DateTime? VisitDate { get; set; }
        public virtual string ReasonForRegimenSwitchSubs { get; set; }
        public virtual string PrescribedRegimenTypeCode { get; set; }
        public virtual string PrescribedRegimenLineCode { get; set; }
        public virtual string PrescribedRegimenDuration { get; set; }
        public virtual DateTime? PrescribedRegimenDispensedDate { get; set; }
        public virtual bool? PrescribedRegimenDispensedDateSpecified { get; set; }
        public virtual DateTime? DateRegimenStarted { get; set; }
        public virtual DateTime? DateRegimenEnded { get; set; }
        public virtual bool? DateRegimenEndedSpecified { get; set; }
        public virtual bool? PrescribedRegimenInitialIndicator { get; set; }
        public virtual bool? PrescribedRegimenInitialIndicatorSpecified { get; set; }
        public virtual bool? PrescribedRegimenCurrentIndicator { get; set; }
        public virtual bool? PrescribedRegimenCurrentIndicatorSpecified { get; set; }
        public virtual string TypeOfPreviousExposureCode { get; set; }
        public virtual bool? PoorAdherenceIndicator { get; set; }
        public virtual bool? PoorAdherenceIndicatorSpecified { get; set; }
        public virtual string ReasonForPoorAdherence { get; set; }
        public virtual string ReasonRegimenEndedCode { get; set; }
        public virtual bool? SubstitutionIndicator { get; set; }
        public virtual bool? SubstitutionIndicatorSpecified { get; set; }
        public virtual bool? SwitchIndicator { get; set; }
        public virtual bool? SwitchIndicatorSpecified { get; set; }
    }
        
    public class dtoHIVQuestions : BaseT
    { 
        public virtual OnboardedFacility TransferredInFrom { get; set; }
        public virtual string FirstARTRegimenCode { get; set; }
        public virtual string FirstARTRegimenCodeDesc { get; set; }
        public virtual OnboardedFacility FacilityReferredTo { get; set; }
        public virtual HIVQuestionsTypeCareEntryPoint? CareEntryPoint { get; set; }
        public virtual bool? CareEntryPointSpecified { get; set; }
        public virtual DateTime? FirstConfirmedHIVTestDate { get; set; }
        public virtual bool? FirstConfirmedHIVTestDateSpecified { get; set; }
        public virtual HIVQuestionsTypeFirstHIVTestMode? FirstHIVTestMode { get; set; }
        public virtual bool? FirstHIVTestModeSpecified { get; set; }
        public virtual string WhereFirstHIVTest { get; set; }
        public virtual string PriorArt { get; set; }
        public virtual DateTime? MedicallyEligibleDate { get; set; }
        public virtual bool? MedicallyEligibleDateSpecified { get; set; }
        public virtual HIVQuestionsTypeReasonMedicallyEligible? ReasonMedicallyEligible { get; set; }
        public virtual bool? ReasonMedicallyEligibleSpecified { get; set; }
        public virtual DateTime? InitialAdherenceCounselingCompletedDate { get; set; }
        public virtual bool? InitialAdherenceCounselingCompletedDateSpecified { get; set; }
        public virtual DateTime? TransferredInDate { get; set; }
        public virtual bool? TransferredInDateSpecified { get; set; }
        public virtual string TransferredInFromPatId { get; set; }
        public virtual DateTime? ARTStartDate { get; set; }
        public virtual bool? ARTStartDateSpecified { get; set; }
        public virtual HIVQuestionsTypeWHOClinicalStageARTStart? WHOClinicalStageARTStart { get; set; }
        public virtual bool? WHOClinicalStageARTStartSpecified { get; set; }
        public virtual int? WeightAtARTStart { get; set; }
        public virtual bool? WeightAtARTStartSpecified { get; set; }
        public virtual int? ChildHeightAtARTStart { get; set; }
        public virtual bool? ChildHeightAtARTStartSpecified { get; set; }
        public virtual HIVQuestionsTypeFunctionalStatusStartART? FunctionalStatusStartART { get; set; }
        public virtual bool? FunctionalStatusStartARTSpecified { get; set; }
        public virtual string CD4AtStartOfART { get; set; }
        public virtual bool? PatientTransferredOut { get; set; }
        public virtual bool? PatientTransferredOutSpecified { get; set; }
        public virtual HIVQuestionsTypeTransferredOutStatus? TransferredOutStatus { get; set; }
        public virtual bool? TransferredOutStatusSpecified { get; set; }
        public virtual DateTime? TransferredOutDate { get; set; }
        public virtual bool? TransferredOutDateSpecified { get; set; }
        public virtual bool? PatientHasDied { get; set; }
        public virtual bool? PatientHasDiedSpecified { get; set; }
        public virtual HIVQuestionsTypeStatusAtDeath? StatusAtDeath { get; set; }
        public virtual bool? StatusAtDeathSpecified { get; set; }
        public virtual DateTime? DeathDate { get; set; }
        public virtual bool? DeathDateSpecified { get; set; }
        public virtual string SourceOfDeathInformation { get; set; }
        public virtual HIVQuestionsTypeCauseOfDeathHIVRelated? CauseOfDeathHIVRelated { get; set; }
        public virtual bool? CauseOfDeathHIVRelatedSpecified { get; set; }
        public virtual string DrugAllergies { get; set; }
        public virtual DateTime? EnrolledInHIVCareDate { get; set; }
        public virtual bool? EnrolledInHIVCareDateSpecified { get; set; }
        public virtual HIVQuestionsTypeInitialTBStatus? InitialTBStatus { get; set; }
        public virtual bool? InitialTBStatusSpecified { get; set; }

        public virtual dtoCondition Condition { get; set; }
        
        //public virtual ICollection<dtoConditionSpecificQuestions> ConditionSpecificQuestions { get; set; } 
    }

    public class PartnerNotificationServices : BaseT
    {
        public virtual string PartnerName { get; set; }

        public virtual PartnerNotificationServicesTypePartnerGender PartnerGender { get; set; } 
    }

}


