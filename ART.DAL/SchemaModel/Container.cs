using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ART.DAL.SchemaModel
{
    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Container
    {
        [XmlElement("MessageHeader")]
        public virtual MessageHeader MessageHeader { get; set; }

        [XmlElement("IndividualReport", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public virtual IndividualReport IndividualReport { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string FileName { get; set; }

        public Container ReadXMLDocument(string xmlContent, string filename, out string err, bool repeat = false)
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
                container = JsonConvert.DeserializeObject<Container>(jsonText, dateTimeConverter);
                container.FileName = filename;
            }
            catch (Exception ex)
            {
                //err = Logger.LogError(ex);
            }
            return container;
        }
    }


    [Serializable()]
    public class MessageHeader
    {
        [XmlElement("MessageStatusCode")]
        [JsonProperty("MessageStatusCode")]
        public virtual MessageHeaderTypeMessageStatusCode? MessageStatusCode { get; set; }

        [XmlElement("MessageCreationDateTime")]
        [JsonProperty("MessageCreationDateTime")]
        public virtual DateTime? MessageCreationDateTime { get; set; }

        [XmlElement("MessageSchemaVersion")]
        [JsonProperty("MessageSchemaVersion")]
        public virtual decimal MessageSchemaVersion { get; set; }

        [XmlElement("MessageUniqueID")]
        [JsonProperty("MessageUniqueID")]
        public virtual string MessageUniqueID { get; set; }

        [XmlElement("MessageSendingOrganization")]
        public virtual Facility MessageSendingOrganization { get; set; }
    }


    [Serializable()]
    public class Facility
    {

        public virtual string FacilityName { get; set; }


        public virtual string FacilityID { get; set; }


        public virtual FacilityTypeFacilityTypeCode? FacilityTypeCode { get; set; }
    }


    [Serializable()]
    public class Immunization
    {

        private string visitIDField;

        private DateTime? visitDateField;

        private string immunizationIdentifierField;

        private DateTime? immunizationDateField;

        private string lotNumberField;

        private DateTime? expirationDateField;

        private bool expirationDateFieldSpecified;

        private string manufacturerCodeField;

        private CodedSimple immunizationType1Field;

        private string siteCodeField;

        private string routeCodeField;

        private string doseField;

        private bool selfReportedField;

        private bool selfReportedFieldSpecified;

        private string clinicianField;

        private string performedByField;

        private string checkedByField;



        public virtual string VisitID
        {
            get
            {
                return this.visitIDField;
            }
            set
            {
                this.visitIDField = value;
            }
        }



        public virtual DateTime? VisitDate
        {
            get
            {
                return this.visitDateField;
            }
            set
            {
                this.visitDateField = value;
            }
        }



        public virtual string ImmunizationIdentifier
        {
            get
            {
                return this.immunizationIdentifierField;
            }
            set
            {
                this.immunizationIdentifierField = value;
            }
        }



        public virtual DateTime? ImmunizationDate
        {
            get
            {
                return this.immunizationDateField;
            }
            set
            {
                this.immunizationDateField = value;
            }
        }



        public virtual string LotNumber
        {
            get
            {
                return this.lotNumberField;
            }
            set
            {
                this.lotNumberField = value;
            }
        }



        public virtual DateTime? ExpirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ExpirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }



        public virtual string ManufacturerCode
        {
            get
            {
                return this.manufacturerCodeField;
            }
            set
            {
                this.manufacturerCodeField = value;
            }
        }


        [XmlElement("ImmunizationType", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public virtual CodedSimple ImmunizationType1
        {
            get
            {
                return this.immunizationType1Field;
            }
            set
            {
                this.immunizationType1Field = value;
            }
        }



        public virtual string SiteCode
        {
            get
            {
                return this.siteCodeField;
            }
            set
            {
                this.siteCodeField = value;
            }
        }



        public virtual string RouteCode
        {
            get
            {
                return this.routeCodeField;
            }
            set
            {
                this.routeCodeField = value;
            }
        }



        public virtual string Dose
        {
            get
            {
                return this.doseField;
            }
            set
            {
                this.doseField = value;
            }
        }



        public virtual bool SelfReported
        {
            get
            {
                return this.selfReportedField;
            }
            set
            {
                this.selfReportedField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool SelfReportedSpecified
        {
            get
            {
                return this.selfReportedFieldSpecified;
            }
            set
            {
                this.selfReportedFieldSpecified = value;
            }
        }



        public virtual string Clinician
        {
            get
            {
                return this.clinicianField;
            }
            set
            {
                this.clinicianField = value;
            }
        }



        public virtual string PerformedBy
        {
            get
            {
                return this.performedByField;
            }
            set
            {
                this.performedByField = value;
            }
        }



        public virtual string CheckedBy
        {
            get
            {
                return this.checkedByField;
            }
            set
            {
                this.checkedByField = value;
            }
        }
    }



    [Serializable()]
    public class CodedSimple
    {

        public virtual string Code { get; set; }



        public virtual string CodeDescTxt { get; set; }
    }



    [Serializable()]
    public class Regimen
    {

        private string visitIDField;

        private DateTime? visitDateField;

        private string reasonForRegimenSwitchSubsField;

        private CodedSimple prescribedRegimenField;

        private string prescribedRegimenTypeCodeField;

        private string prescribedRegimenLineCodeField;

        private string prescribedRegimenDurationField;

        private DateTime? prescribedRegimenDispensedDateField;

        private bool prescribedRegimenDispensedDateFieldSpecified;

        private DateTime? dateRegimenStartedField;

        private string dateRegimenStartedDDField;

        private string dateRegimenStartedMMField;

        private string dateRegimenStartedYYYYField;

        private DateTime? dateRegimenEndedField;

        private bool dateRegimenEndedFieldSpecified;

        private string dateRegimenEndedDDField;

        private string dateRegimenEndedMMField;

        private string dateRegimenEndedYYYYField;

        private bool prescribedRegimenInitialIndicatorField;

        private bool prescribedRegimenInitialIndicatorFieldSpecified;

        private bool prescribedRegimenCurrentIndicatorField;

        private bool prescribedRegimenCurrentIndicatorFieldSpecified;

        private string typeOfPreviousExposureCodeField;

        private bool poorAdherenceIndicatorField;

        private bool poorAdherenceIndicatorFieldSpecified;

        private string reasonForPoorAdherenceField;

        private string reasonRegimenEndedCodeField;

        private bool substitutionIndicatorField;

        private bool substitutionIndicatorFieldSpecified;

        private bool switchIndicatorField;

        private bool switchIndicatorFieldSpecified;



        public virtual string VisitID
        {
            get
            {
                return this.visitIDField;
            }
            set
            {
                this.visitIDField = value;
            }
        }



        public virtual DateTime? VisitDate
        {
            get
            {
                return this.visitDateField;
            }
            set
            {
                this.visitDateField = value;
            }
        }



        public virtual string ReasonForRegimenSwitchSubs
        {
            get
            {
                return this.reasonForRegimenSwitchSubsField;
            }
            set
            {
                this.reasonForRegimenSwitchSubsField = value;
            }
        }



        public virtual CodedSimple PrescribedRegimen
        {
            get
            {
                return this.prescribedRegimenField;
            }
            set
            {
                this.prescribedRegimenField = value;
            }
        }



        public virtual string PrescribedRegimenTypeCode
        {
            get
            {
                return this.prescribedRegimenTypeCodeField;
            }
            set
            {
                this.prescribedRegimenTypeCodeField = value;
            }
        }



        public virtual string PrescribedRegimenLineCode
        {
            get
            {
                return this.prescribedRegimenLineCodeField;
            }
            set
            {
                this.prescribedRegimenLineCodeField = value;
            }
        }



        public virtual string PrescribedRegimenDuration
        {
            get
            {
                return this.prescribedRegimenDurationField;
            }
            set
            {
                this.prescribedRegimenDurationField = value;
            }
        }



        public virtual DateTime? PrescribedRegimenDispensedDate
        {
            get
            {
                return this.prescribedRegimenDispensedDateField;
            }
            set
            {
                this.prescribedRegimenDispensedDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PrescribedRegimenDispensedDateSpecified
        {
            get
            {
                return this.prescribedRegimenDispensedDateFieldSpecified;
            }
            set
            {
                this.prescribedRegimenDispensedDateFieldSpecified = value;
            }
        }



        public virtual DateTime? DateRegimenStarted
        {
            get
            {
                return this.dateRegimenStartedField;
            }
            set
            {
                this.dateRegimenStartedField = value;
            }
        }



        public virtual string DateRegimenStartedDD
        {
            get
            {
                return this.dateRegimenStartedDDField;
            }
            set
            {
                this.dateRegimenStartedDDField = value;
            }
        }



        public virtual string DateRegimenStartedMM
        {
            get
            {
                return this.dateRegimenStartedMMField;
            }
            set
            {
                this.dateRegimenStartedMMField = value;
            }
        }



        public virtual string DateRegimenStartedYYYY
        {
            get
            {
                return this.dateRegimenStartedYYYYField;
            }
            set
            {
                this.dateRegimenStartedYYYYField = value;
            }
        }



        public virtual DateTime? DateRegimenEnded
        {
            get
            {
                return this.dateRegimenEndedField;
            }
            set
            {
                this.dateRegimenEndedField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DateRegimenEndedSpecified
        {
            get
            {
                return this.dateRegimenEndedFieldSpecified;
            }
            set
            {
                this.dateRegimenEndedFieldSpecified = value;
            }
        }



        public virtual string DateRegimenEndedDD
        {
            get
            {
                return this.dateRegimenEndedDDField;
            }
            set
            {
                this.dateRegimenEndedDDField = value;
            }
        }



        public virtual string DateRegimenEndedMM
        {
            get
            {
                return this.dateRegimenEndedMMField;
            }
            set
            {
                this.dateRegimenEndedMMField = value;
            }
        }



        public virtual string DateRegimenEndedYYYY
        {
            get
            {
                return this.dateRegimenEndedYYYYField;
            }
            set
            {
                this.dateRegimenEndedYYYYField = value;
            }
        }



        public virtual bool PrescribedRegimenInitialIndicator
        {
            get
            {
                return this.prescribedRegimenInitialIndicatorField;
            }
            set
            {
                this.prescribedRegimenInitialIndicatorField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PrescribedRegimenInitialIndicatorSpecified
        {
            get
            {
                return this.prescribedRegimenInitialIndicatorFieldSpecified;
            }
            set
            {
                this.prescribedRegimenInitialIndicatorFieldSpecified = value;
            }
        }



        public virtual bool PrescribedRegimenCurrentIndicator
        {
            get
            {
                return this.prescribedRegimenCurrentIndicatorField;
            }
            set
            {
                this.prescribedRegimenCurrentIndicatorField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PrescribedRegimenCurrentIndicatorSpecified
        {
            get
            {
                return this.prescribedRegimenCurrentIndicatorFieldSpecified;
            }
            set
            {
                this.prescribedRegimenCurrentIndicatorFieldSpecified = value;
            }
        }



        public virtual string TypeOfPreviousExposureCode
        {
            get
            {
                return this.typeOfPreviousExposureCodeField;
            }
            set
            {
                this.typeOfPreviousExposureCodeField = value;
            }
        }



        public virtual bool PoorAdherenceIndicator
        {
            get
            {
                return this.poorAdherenceIndicatorField;
            }
            set
            {
                this.poorAdherenceIndicatorField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PoorAdherenceIndicatorSpecified
        {
            get
            {
                return this.poorAdherenceIndicatorFieldSpecified;
            }
            set
            {
                this.poorAdherenceIndicatorFieldSpecified = value;
            }
        }



        public virtual string ReasonForPoorAdherence
        {
            get
            {
                return this.reasonForPoorAdherenceField;
            }
            set
            {
                this.reasonForPoorAdherenceField = value;
            }
        }



        public virtual string ReasonRegimenEndedCode
        {
            get
            {
                return this.reasonRegimenEndedCodeField;
            }
            set
            {
                this.reasonRegimenEndedCodeField = value;
            }
        }



        public virtual bool SubstitutionIndicator
        {
            get
            {
                return this.substitutionIndicatorField;
            }
            set
            {
                this.substitutionIndicatorField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool SubstitutionIndicatorSpecified
        {
            get
            {
                return this.substitutionIndicatorFieldSpecified;
            }
            set
            {
                this.substitutionIndicatorFieldSpecified = value;
            }
        }



        public virtual bool SwitchIndicator
        {
            get
            {
                return this.switchIndicatorField;
            }
            set
            {
                this.switchIndicatorField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool SwitchIndicatorSpecified
        {
            get
            {
                return this.switchIndicatorFieldSpecified;
            }
            set
            {
                this.switchIndicatorFieldSpecified = value;
            }
        }
    }


    [Serializable()]
    public class Numeric
    {

        public virtual string ComparatorCode { get; set; }



        public virtual float Value1 { get; set; }


        public virtual string SeperatorCode { get; set; }


        public virtual float Value2 { get; set; }


        [XmlIgnore()]
        public virtual bool Value2Specified { get; set; }



        public virtual Coded Unit { get; set; }
    }



    [Serializable()]
    public class Coded
    {


        public virtual string Code { get; set; }



        public virtual string CodeDescTxt { get; set; }



        public virtual string CodeSystemCode { get; set; }



        public virtual string Text { get; set; }
    }


    [Serializable()]
    public class LaboratoryOrderAndResult
    {


        public virtual string LaboratoryTestTypeCode { get; set; }

        [XmlElement(ElementName = "OrderedTestDate")]
        [JsonProperty("OrderedTestDate")]
        public virtual DateTime? OrderedTestDate { get; set; }


        [XmlIgnore()]
        public virtual bool OrderedTestDateSpecified { get; set; }


        public virtual CodedSimple LaboratoryOrderedTest { get; set; }



        public virtual CodedSimple LaboratoryResultedTest { get; set; }


        //
        //public virtual AnswerType LaboratoryResult { get; set; }

        [XmlElement(ElementName = "LaboratoryResult")]
        public virtual LaboratoryResult LaboratoryResult { get; set; }


        public virtual DateTime? ResultedTestDate { get; set; }


        [XmlIgnore()]
        public virtual bool ResultedTestDateSpecified { get; set; }


        public virtual string OtherLaboratoryInformation { get; set; }
    }

    [XmlRoot(ElementName = "AnswerNumeric")]
    public class AnswerNumeric
    {
        [XmlElement(ElementName = "Value1")]
        public virtual string Value1 { get; set; }
    }

    [XmlRoot(ElementName = "AnswerCode")]
    public class AnswerCode
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "CodeDescTxt")]
        public string CodeDescTxt { get; set; }
        [XmlElement(ElementName = "CodeSystemCode")]
        public string CodeSystemCode { get; set; }
    }

    //[XmlRoot(ElementName = "LaboratoryResult")]
    //public class LaboratoryResult 
    //{
    //    [XmlElement(ElementName = "AnswerNumeric")]
    //    public virtual AnswerNumeric AnswerNumeric { get; set; }
    //}

    [XmlRoot(ElementName = "LaboratoryResult")]
    public class LaboratoryResult
    {
        [XmlElement(ElementName = "AnswerNumeric")]
        public AnswerNumeric AnswerNumeric { get; set; }

        [XmlElement(ElementName = "AnswerCode")]
        public AnswerCode AnswerCode { get; set; }

        [XmlElement(ElementName = "AnswerDate")]
        public DateTime? AnswerDate { get; set; }

        [XmlElement(ElementName = "AnswerDateTime")]
        public DateTime? AnswerDateTime { get; set; }

        [XmlElement(ElementName = "AnswerText")]
        public virtual string AnswerText { get; set; }
    }

    [Serializable()]
    public class LaboratoryReport
    {

        private string visitIDField;

        private DateTime? visitDateField;

        private string laboratoryTestIdentifierField;

        private DateTime? collectionDateField;

        private bool collectionDateFieldSpecified;

        private LaboratoryReportTypeBaselineRepeatCode? baselineRepeatCodeField;

        private bool baselineRepeatCodeFieldSpecified;

        private string aRTStatusCodeField;

        private string clinicianField;

        private string reportedByField;

        private string checkedByField;



        public virtual string VisitID
        {
            get
            {
                return this.visitIDField;
            }
            set
            {
                this.visitIDField = value;
            }
        }



        public virtual DateTime? VisitDate
        {
            get
            {
                return this.visitDateField;
            }
            set
            {
                this.visitDateField = value;
            }
        }



        public virtual string LaboratoryTestIdentifier
        {
            get
            {
                return this.laboratoryTestIdentifierField;
            }
            set
            {
                this.laboratoryTestIdentifierField = value;
            }
        }



        public virtual DateTime? CollectionDate
        {
            get
            {
                return this.collectionDateField;
            }
            set
            {
                this.collectionDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CollectionDateSpecified
        {
            get
            {
                return this.collectionDateFieldSpecified;
            }
            set
            {
                this.collectionDateFieldSpecified = value;
            }
        }



        public virtual LaboratoryReportTypeBaselineRepeatCode? BaselineRepeatCode
        {
            get
            {
                return this.baselineRepeatCodeField;
            }
            set
            {
                this.baselineRepeatCodeField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool BaselineRepeatCodeSpecified
        {
            get
            {
                return this.baselineRepeatCodeFieldSpecified;
            }
            set
            {
                this.baselineRepeatCodeFieldSpecified = value;
            }
        }



        public virtual string ARTStatusCode
        {
            get
            {
                return this.aRTStatusCodeField;
            }
            set
            {
                this.aRTStatusCodeField = value;
            }
        }

        private List<LaboratoryOrderAndResult> laboratoryOrderAndResult;

        [XmlElement("LaboratoryOrderAndResult")]
        [JsonProperty("LaboratoryOrderAndResult")]
        [JsonConverter(typeof(SingleOrArrayConverter<LaboratoryOrderAndResult>))]
        public virtual List<LaboratoryOrderAndResult> LaboratoryOrderAndResult_xml
        {
            get
            {
                return laboratoryOrderAndResult;
            }
            set
            {
                laboratoryOrderAndResult = value;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<LaboratoryOrderAndResult> LaboratoryOrderAndResult
        {
            get
            {
                return laboratoryOrderAndResult;
            }
            set
            {
                laboratoryOrderAndResult = value as List<LaboratoryOrderAndResult>;
            }
        }


        public virtual string Clinician
        {
            get
            {
                return this.clinicianField;
            }
            set
            {
                this.clinicianField = value;
            }
        }



        public virtual string ReportedBy
        {
            get
            {
                return this.reportedByField;
            }
            set
            {
                this.reportedByField = value;
            }
        }



        public virtual string CheckedBy
        {
            get
            {
                return this.checkedByField;
            }
            set
            {
                this.checkedByField = value;
            }
        }
    }

    public class Encounters
    {
        [XmlElement(ElementName = "HIVEncounter")]
        [JsonProperty("HIVEncounter")]
        [JsonConverter(typeof(SingleOrArrayConverter<HIVEncounter>))]
        public virtual IList<HIVEncounter> HIVEncounter { get; set; }
    }


    [Serializable()]
    public class HIVEncounter
    {

        private string visitIDField;

        private DateTime? visitDateField;

        private double durationOnArtField;

        private bool durationOnArtFieldSpecified;

        private int weightField;

        private bool weightFieldSpecified;

        private int childHeightField;

        private bool childHeightFieldSpecified;

        private string bloodPressureField;

        private HIVEncounterTypeEDDandPMTCTLink? eDDandPMTCTLinkField;

        private bool eDDandPMTCTLinkFieldSpecified;

        private HIVEncounterTypePatientFamilyPlanningCode? patientFamilyPlanningCodeField;

        private bool patientFamilyPlanningCodeFieldSpecified;

        private string patientFamilyPlanningMethodCodeField;

        private HIVEncounterTypeFunctionalStatus? functionalStatusField;

        private bool functionalStatusFieldSpecified;

        private HIVEncounterTypeWHOClinicalStage? wHOClinicalStageField;

        private bool wHOClinicalStageFieldSpecified;

        private HIVEncounterTypeTBStatus? tBStatusField;

        private bool tBStatusFieldSpecified;

        private string otherOIOtherProblemsField;

        private string notedSideEffectsField;

        private CodedSimple aRVDrugRegimenField;

        private HIVEncounterTypeARVDrugAdherence? aRVDrugAdherenceField;

        private bool aRVDrugAdherenceFieldSpecified;

        private HIVEncounterTypeWhyPoorFairARVDrugAdherence? whyPoorFairARVDrugAdherenceField;

        private bool whyPoorFairARVDrugAdherenceFieldSpecified;

        private CodedSimple cotrimoxazoleDoseField;

        private HIVEncounterTypeCotrimoxazoleAdherence? cotrimoxazoleAdherenceField;

        private bool cotrimoxazoleAdherenceFieldSpecified;

        private HIVEncounterTypeWhyPoorFairCotrimoxazoleDrugAdherence? whyPoorFairCotrimoxazoleDrugAdherenceField;

        private bool whyPoorFairCotrimoxazoleDrugAdherenceFieldSpecified;

        private CodedSimple iNHDoseField;

        private HIVEncounterTypeINHAdherence? iNHAdherenceField;

        private bool iNHAdherenceFieldSpecified;

        private HIVEncounterTypeWhyPoorFairINHDrugAdherence? whyPoorFairINHDrugAdherenceField;

        private bool whyPoorFairINHDrugAdherenceFieldSpecified;

        private double? cD4Field;

        private bool cD4FieldSpecified;

        private DateTime? cD4TestDateField;

        private bool cD4TestDateFieldSpecified;

        private DateTime? nextAppointmentDateField;

        private bool nextAppointmentDateFieldSpecified;



        public virtual string VisitID
        {
            get
            {
                return this.visitIDField;
            }
            set
            {
                this.visitIDField = value;
            }
        }
         

        public virtual DateTime? VisitDate
        {
            get
            {
                return this.visitDateField;
            }
            set
            {
                this.visitDateField = value;
            }
        }

 
        public virtual double DurationOnArt
        {
            get
            {
                return this.durationOnArtField;
            }
            set
            {
                this.durationOnArtField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DurationOnArtSpecified
        {
            get
            {
                return this.durationOnArtFieldSpecified;
            }
            set
            {
                this.durationOnArtFieldSpecified = value;
            }
        }



        public virtual int Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WeightSpecified
        {
            get
            {
                return this.weightFieldSpecified;
            }
            set
            {
                this.weightFieldSpecified = value;
            }
        }



        public virtual int ChildHeight
        {
            get
            {
                return this.childHeightField;
            }
            set
            {
                this.childHeightField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ChildHeightSpecified
        {
            get
            {
                return this.childHeightFieldSpecified;
            }
            set
            {
                this.childHeightFieldSpecified = value;
            }
        }



        public virtual string BloodPressure
        {
            get
            {
                return this.bloodPressureField;
            }
            set
            {
                this.bloodPressureField = value;
            }
        }



        public virtual HIVEncounterTypeEDDandPMTCTLink? EDDandPMTCTLink
        {
            get
            {
                return this.eDDandPMTCTLinkField;
            }
            set
            {
                this.eDDandPMTCTLinkField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool EDDandPMTCTLinkSpecified
        {
            get
            {
                return this.eDDandPMTCTLinkFieldSpecified;
            }
            set
            {
                this.eDDandPMTCTLinkFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypePatientFamilyPlanningCode? PatientFamilyPlanningCode
        {
            get
            {
                return this.patientFamilyPlanningCodeField;
            }
            set
            {
                this.patientFamilyPlanningCodeField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientFamilyPlanningCodeSpecified
        {
            get
            {
                return this.patientFamilyPlanningCodeFieldSpecified;
            }
            set
            {
                this.patientFamilyPlanningCodeFieldSpecified = value;
            }
        }



        public virtual string PatientFamilyPlanningMethodCode
        {
            get
            {
                return this.patientFamilyPlanningMethodCodeField;
            }
            set
            {
                this.patientFamilyPlanningMethodCodeField = value;
            }
        }



        public virtual HIVEncounterTypeFunctionalStatus? FunctionalStatus
        {
            get
            {
                return this.functionalStatusField;
            }
            set
            {
                this.functionalStatusField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool FunctionalStatusSpecified
        {
            get
            {
                return this.functionalStatusFieldSpecified;
            }
            set
            {
                this.functionalStatusFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypeWHOClinicalStage? WHOClinicalStage
        {
            get
            {
                return this.wHOClinicalStageField;
            }
            set
            {
                this.wHOClinicalStageField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WHOClinicalStageSpecified
        {
            get
            {
                return this.wHOClinicalStageFieldSpecified;
            }
            set
            {
                this.wHOClinicalStageFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypeTBStatus? TBStatus
        {
            get
            {
                return this.tBStatusField;
            }
            set
            {
                this.tBStatusField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool TBStatusSpecified
        {
            get
            {
                return this.tBStatusFieldSpecified;
            }
            set
            {
                this.tBStatusFieldSpecified = value;
            }
        }



        public virtual string OtherOIOtherProblems
        {
            get
            {
                return this.otherOIOtherProblemsField;
            }
            set
            {
                this.otherOIOtherProblemsField = value;
            }
        }



        public virtual string NotedSideEffects
        {
            get
            {
                return this.notedSideEffectsField;
            }
            set
            {
                this.notedSideEffectsField = value;
            }
        }



        public virtual CodedSimple ARVDrugRegimen
        {
            get
            {
                return this.aRVDrugRegimenField;
            }
            set
            {
                this.aRVDrugRegimenField = value;
            }
        }



        public virtual HIVEncounterTypeARVDrugAdherence? ARVDrugAdherence
        {
            get
            {
                return this.aRVDrugAdherenceField;
            }
            set
            {
                this.aRVDrugAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ARVDrugAdherenceSpecified
        {
            get
            {
                return this.aRVDrugAdherenceFieldSpecified;
            }
            set
            {
                this.aRVDrugAdherenceFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypeWhyPoorFairARVDrugAdherence? WhyPoorFairARVDrugAdherence
        {
            get
            {
                return this.whyPoorFairARVDrugAdherenceField;
            }
            set
            {
                this.whyPoorFairARVDrugAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WhyPoorFairARVDrugAdherenceSpecified
        {
            get
            {
                return this.whyPoorFairARVDrugAdherenceFieldSpecified;
            }
            set
            {
                this.whyPoorFairARVDrugAdherenceFieldSpecified = value;
            }
        }



        public virtual CodedSimple CotrimoxazoleDose
        {
            get
            {
                return this.cotrimoxazoleDoseField;
            }
            set
            {
                this.cotrimoxazoleDoseField = value;
            }
        }



        public virtual HIVEncounterTypeCotrimoxazoleAdherence? CotrimoxazoleAdherence
        {
            get
            {
                return this.cotrimoxazoleAdherenceField;
            }
            set
            {
                this.cotrimoxazoleAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CotrimoxazoleAdherenceSpecified
        {
            get
            {
                return this.cotrimoxazoleAdherenceFieldSpecified;
            }
            set
            {
                this.cotrimoxazoleAdherenceFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypeWhyPoorFairCotrimoxazoleDrugAdherence? WhyPoorFairCotrimoxazoleDrugAdherence
        {
            get
            {
                return this.whyPoorFairCotrimoxazoleDrugAdherenceField;
            }
            set
            {
                this.whyPoorFairCotrimoxazoleDrugAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WhyPoorFairCotrimoxazoleDrugAdherenceSpecified
        {
            get
            {
                return this.whyPoorFairCotrimoxazoleDrugAdherenceFieldSpecified;
            }
            set
            {
                this.whyPoorFairCotrimoxazoleDrugAdherenceFieldSpecified = value;
            }
        }



        public virtual CodedSimple INHDose
        {
            get
            {
                return this.iNHDoseField;
            }
            set
            {
                this.iNHDoseField = value;
            }
        }



        public virtual HIVEncounterTypeINHAdherence? INHAdherence
        {
            get
            {
                return this.iNHAdherenceField;
            }
            set
            {
                this.iNHAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool INHAdherenceSpecified
        {
            get
            {
                return this.iNHAdherenceFieldSpecified;
            }
            set
            {
                this.iNHAdherenceFieldSpecified = value;
            }
        }



        public virtual HIVEncounterTypeWhyPoorFairINHDrugAdherence? WhyPoorFairINHDrugAdherence
        {
            get
            {
                return this.whyPoorFairINHDrugAdherenceField;
            }
            set
            {
                this.whyPoorFairINHDrugAdherenceField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WhyPoorFairINHDrugAdherenceSpecified
        {
            get
            {
                return this.whyPoorFairINHDrugAdherenceFieldSpecified;
            }
            set
            {
                this.whyPoorFairINHDrugAdherenceFieldSpecified = value;
            }
        }



        //[JsonIgnore]
        //[XmlIgnore]
        public virtual double? CD4
        {
            get
            {
                return this.cD4Field;
            }
            set
            {
                this.cD4Field = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CD4Specified
        {
            get
            {
                return this.cD4FieldSpecified;
            }
            set
            {
                this.cD4FieldSpecified = value;
            }
        }



        public virtual DateTime? CD4TestDate
        {
            get
            {
                return this.cD4TestDateField;
            }
            set
            {
                this.cD4TestDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CD4TestDateSpecified
        {
            get
            {
                return this.cD4TestDateFieldSpecified;
            }
            set
            {
                this.cD4TestDateFieldSpecified = value;
            }
        }



        public virtual DateTime? NextAppointmentDate
        {
            get
            {
                return this.nextAppointmentDateField;
            }
            set
            {
                this.nextAppointmentDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool NextAppointmentDateSpecified
        {
            get
            {
                return this.nextAppointmentDateFieldSpecified;
            }
            set
            {
                this.nextAppointmentDateFieldSpecified = value;
            }
        }
    }



    [Serializable()]
    public class HIVQuestions
    {

        private HIVQuestionsTypeCareEntryPoint? careEntryPointField;

        private bool careEntryPointFieldSpecified;

        private DateTime? firstConfirmedHIVTestDateField;

        private bool firstConfirmedHIVTestDateFieldSpecified;

        private HIVQuestionsTypeFirstHIVTestMode? firstHIVTestModeField;

        private bool firstHIVTestModeFieldSpecified;

        private string whereFirstHIVTestField;

        private string priorArtField;

        private DateTime? medicallyEligibleDateField;

        private bool medicallyEligibleDateFieldSpecified;

        private HIVQuestionsTypeReasonMedicallyEligible? reasonMedicallyEligibleField;

        private bool reasonMedicallyEligibleFieldSpecified;

        private DateTime? initialAdherenceCounselingCompletedDateField;

        private bool initialAdherenceCounselingCompletedDateFieldSpecified;

        private DateTime? transferredInDateField;

        private bool transferredInDateFieldSpecified;

        private Facility transferredInFromField;

        private string transferredInFromPatIdField;

        private CodedSimple firstARTRegimenField;

        private DateTime? aRTStartDateField;

        private bool aRTStartDateFieldSpecified;

        private HIVQuestionsTypeWHOClinicalStageARTStart? wHOClinicalStageARTStartField;

        private bool wHOClinicalStageARTStartFieldSpecified;

        private int weightAtARTStartField;

        private bool weightAtARTStartFieldSpecified;

        private int childHeightAtARTStartField;

        private bool childHeightAtARTStartFieldSpecified;

        private HIVQuestionsTypeFunctionalStatusStartART? functionalStatusStartARTField;

        private bool functionalStatusStartARTFieldSpecified;

        private string cD4AtStartOfARTField;

        private bool patientTransferredOutField;

        private bool patientTransferredOutFieldSpecified;

        private HIVQuestionsTypeTransferredOutStatus? transferredOutStatusField;

        private bool transferredOutStatusFieldSpecified;

        private DateTime? transferredOutDateField;

        private bool transferredOutDateFieldSpecified;

        private Facility facilityReferredToField;

        private bool patientHasDiedField;

        private bool patientHasDiedFieldSpecified;

        private HIVQuestionsTypeStatusAtDeath? statusAtDeathField;

        private bool statusAtDeathFieldSpecified;

        private DateTime? deathDateField;

        private bool deathDateFieldSpecified;

        private string sourceOfDeathInformationField;

        private HIVQuestionsTypeCauseOfDeathHIVRelated? causeOfDeathHIVRelatedField;

        private bool causeOfDeathHIVRelatedFieldSpecified;

        private string drugAllergiesField;

        private DateTime? enrolledInHIVCareDateField;

        private bool enrolledInHIVCareDateFieldSpecified;

        private HIVQuestionsTypeInitialTBStatus? initialTBStatusField;

        private bool initialTBStatusFieldSpecified;



        public virtual HIVQuestionsTypeCareEntryPoint? CareEntryPoint
        {
            get
            {
                return this.careEntryPointField;
            }
            set
            {
                this.careEntryPointField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CareEntryPointSpecified
        {
            get
            {
                return this.careEntryPointFieldSpecified;
            }
            set
            {
                this.careEntryPointFieldSpecified = value;
            }
        }



        public virtual DateTime? FirstConfirmedHIVTestDate
        {
            get
            {
                return this.firstConfirmedHIVTestDateField;
            }
            set
            {
                this.firstConfirmedHIVTestDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool FirstConfirmedHIVTestDateSpecified
        {
            get
            {
                return this.firstConfirmedHIVTestDateFieldSpecified;
            }
            set
            {
                this.firstConfirmedHIVTestDateFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeFirstHIVTestMode? FirstHIVTestMode
        {
            get
            {
                return this.firstHIVTestModeField;
            }
            set
            {
                this.firstHIVTestModeField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool FirstHIVTestModeSpecified
        {
            get
            {
                return this.firstHIVTestModeFieldSpecified;
            }
            set
            {
                this.firstHIVTestModeFieldSpecified = value;
            }
        }



        public virtual string WhereFirstHIVTest
        {
            get
            {
                return this.whereFirstHIVTestField;
            }
            set
            {
                this.whereFirstHIVTestField = value;
            }
        }



        public virtual string PriorArt
        {
            get
            {
                return this.priorArtField;
            }
            set
            {
                this.priorArtField = value;
            }
        }



        public virtual DateTime? MedicallyEligibleDate
        {
            get
            {
                return this.medicallyEligibleDateField;
            }
            set
            {
                this.medicallyEligibleDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool MedicallyEligibleDateSpecified
        {
            get
            {
                return this.medicallyEligibleDateFieldSpecified;
            }
            set
            {
                this.medicallyEligibleDateFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeReasonMedicallyEligible? ReasonMedicallyEligible
        {
            get
            {
                return this.reasonMedicallyEligibleField;
            }
            set
            {
                this.reasonMedicallyEligibleField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ReasonMedicallyEligibleSpecified
        {
            get
            {
                return this.reasonMedicallyEligibleFieldSpecified;
            }
            set
            {
                this.reasonMedicallyEligibleFieldSpecified = value;
            }
        }



        public virtual DateTime? InitialAdherenceCounselingCompletedDate
        {
            get
            {
                return this.initialAdherenceCounselingCompletedDateField;
            }
            set
            {
                this.initialAdherenceCounselingCompletedDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool InitialAdherenceCounselingCompletedDateSpecified
        {
            get
            {
                return this.initialAdherenceCounselingCompletedDateFieldSpecified;
            }
            set
            {
                this.initialAdherenceCounselingCompletedDateFieldSpecified = value;
            }
        }



        public virtual DateTime? TransferredInDate
        {
            get
            {
                return this.transferredInDateField;
            }
            set
            {
                this.transferredInDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool TransferredInDateSpecified
        {
            get
            {
                return this.transferredInDateFieldSpecified;
            }
            set
            {
                this.transferredInDateFieldSpecified = value;
            }
        }



        public virtual Facility TransferredInFrom
        {
            get
            {
                return this.transferredInFromField;
            }
            set
            {
                this.transferredInFromField = value;
            }
        }



        public virtual string TransferredInFromPatId
        {
            get
            {
                return this.transferredInFromPatIdField;
            }
            set
            {
                this.transferredInFromPatIdField = value;
            }
        }



        public virtual CodedSimple FirstARTRegimen
        {
            get
            {
                return this.firstARTRegimenField;
            }
            set
            {
                this.firstARTRegimenField = value;
            }
        }



        public virtual DateTime? ARTStartDate
        {
            get
            {
                return this.aRTStartDateField;
            }
            set
            {
                this.aRTStartDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ARTStartDateSpecified
        {
            get
            {
                return this.aRTStartDateFieldSpecified;
            }
            set
            {
                this.aRTStartDateFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeWHOClinicalStageARTStart? WHOClinicalStageARTStart
        {
            get
            {
                return this.wHOClinicalStageARTStartField;
            }
            set
            {
                this.wHOClinicalStageARTStartField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WHOClinicalStageARTStartSpecified
        {
            get
            {
                return this.wHOClinicalStageARTStartFieldSpecified;
            }
            set
            {
                this.wHOClinicalStageARTStartFieldSpecified = value;
            }
        }



        public virtual int WeightAtARTStart
        {
            get
            {
                return this.weightAtARTStartField;
            }
            set
            {
                this.weightAtARTStartField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool WeightAtARTStartSpecified
        {
            get
            {
                return this.weightAtARTStartFieldSpecified;
            }
            set
            {
                this.weightAtARTStartFieldSpecified = value;
            }
        }



        public virtual int ChildHeightAtARTStart
        {
            get
            {
                return this.childHeightAtARTStartField;
            }
            set
            {
                this.childHeightAtARTStartField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool ChildHeightAtARTStartSpecified
        {
            get
            {
                return this.childHeightAtARTStartFieldSpecified;
            }
            set
            {
                this.childHeightAtARTStartFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeFunctionalStatusStartART? FunctionalStatusStartART
        {
            get
            {
                return this.functionalStatusStartARTField;
            }
            set
            {
                this.functionalStatusStartARTField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool FunctionalStatusStartARTSpecified
        {
            get
            {
                return this.functionalStatusStartARTFieldSpecified;
            }
            set
            {
                this.functionalStatusStartARTFieldSpecified = value;
            }
        }



        public virtual string CD4AtStartOfART
        {
            get
            {
                return this.cD4AtStartOfARTField;
            }
            set
            {
                this.cD4AtStartOfARTField = value;
            }
        }



        public virtual bool PatientTransferredOut
        {
            get
            {
                return this.patientTransferredOutField;
            }
            set
            {
                this.patientTransferredOutField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientTransferredOutSpecified
        {
            get
            {
                return this.patientTransferredOutFieldSpecified;
            }
            set
            {
                this.patientTransferredOutFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeTransferredOutStatus? TransferredOutStatus
        {
            get
            {
                return this.transferredOutStatusField;
            }
            set
            {
                this.transferredOutStatusField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool TransferredOutStatusSpecified
        {
            get
            {
                return this.transferredOutStatusFieldSpecified;
            }
            set
            {
                this.transferredOutStatusFieldSpecified = value;
            }
        }



        public virtual DateTime? TransferredOutDate
        {
            get
            {
                return this.transferredOutDateField;
            }
            set
            {
                this.transferredOutDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool TransferredOutDateSpecified
        {
            get
            {
                return this.transferredOutDateFieldSpecified;
            }
            set
            {
                this.transferredOutDateFieldSpecified = value;
            }
        }



        public virtual Facility FacilityReferredTo
        {
            get
            {
                return this.facilityReferredToField;
            }
            set
            {
                this.facilityReferredToField = value;
            }
        }



        public virtual bool PatientHasDied
        {
            get
            {
                return this.patientHasDiedField;
            }
            set
            {
                this.patientHasDiedField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientHasDiedSpecified
        {
            get
            {
                return this.patientHasDiedFieldSpecified;
            }
            set
            {
                this.patientHasDiedFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeStatusAtDeath? StatusAtDeath
        {
            get
            {
                return this.statusAtDeathField;
            }
            set
            {
                this.statusAtDeathField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool StatusAtDeathSpecified
        {
            get
            {
                return this.statusAtDeathFieldSpecified;
            }
            set
            {
                this.statusAtDeathFieldSpecified = value;
            }
        }



        public virtual DateTime? DeathDate
        {
            get
            {
                return this.deathDateField;
            }
            set
            {
                this.deathDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DeathDateSpecified
        {
            get
            {
                return this.deathDateFieldSpecified;
            }
            set
            {
                this.deathDateFieldSpecified = value;
            }
        }



        public virtual string SourceOfDeathInformation
        {
            get
            {
                return this.sourceOfDeathInformationField;
            }
            set
            {
                this.sourceOfDeathInformationField = value;
            }
        }



        public virtual HIVQuestionsTypeCauseOfDeathHIVRelated? CauseOfDeathHIVRelated
        {
            get
            {
                return this.causeOfDeathHIVRelatedField;
            }
            set
            {
                this.causeOfDeathHIVRelatedField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool CauseOfDeathHIVRelatedSpecified
        {
            get
            {
                return this.causeOfDeathHIVRelatedFieldSpecified;
            }
            set
            {
                this.causeOfDeathHIVRelatedFieldSpecified = value;
            }
        }



        public virtual string DrugAllergies
        {
            get
            {
                return this.drugAllergiesField;
            }
            set
            {
                this.drugAllergiesField = value;
            }
        }



        public virtual DateTime? EnrolledInHIVCareDate
        {
            get
            {
                return this.enrolledInHIVCareDateField;
            }
            set
            {
                this.enrolledInHIVCareDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool EnrolledInHIVCareDateSpecified
        {
            get
            {
                return this.enrolledInHIVCareDateFieldSpecified;
            }
            set
            {
                this.enrolledInHIVCareDateFieldSpecified = value;
            }
        }



        public virtual HIVQuestionsTypeInitialTBStatus? InitialTBStatus
        {
            get
            {
                return this.initialTBStatusField;
            }
            set
            {
                this.initialTBStatusField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool InitialTBStatusSpecified
        {
            get
            {
                return this.initialTBStatusFieldSpecified;
            }
            set
            {
                this.initialTBStatusFieldSpecified = value;
            }
        }
    }


    [Serializable()]
    public class ConditionSpecificQuestions
    {

        private HIVQuestions itemField;


        [XmlElement("HIVQuestions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public virtual HIVQuestions HIVQuestions
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }



    [Serializable()]
    public class CommonQuestions
    {

        private string hospitalNumberField;

        private Facility diagnosisFacilityField;

        private DateTime? dateOfFirstReportField;

        private bool dateOfFirstReportFieldSpecified;

        private DateTime? dateOfLastReportField;

        private bool dateOfLastReportFieldSpecified;

        private DateTime? diagnosisDateField;

        private bool diagnosisDateFieldSpecified;

        private bool patientDieFromThisIllnessField;

        private bool patientDieFromThisIllnessFieldSpecified;

        private CommonQuestionsTypePatientPregnancyStatusCode? patientPregnancyStatusCodeField;

        private bool patientPregnancyStatusCodeFieldSpecified;

        private DateTime? estimatedDeliveryDateField;

        private bool estimatedDeliveryDateFieldSpecified;

        private int patientAgeField;

        private bool patientAgeFieldSpecified;



        public virtual string HospitalNumber
        {
            get
            {
                return this.hospitalNumberField;
            }
            set
            {
                this.hospitalNumberField = value;
            }
        }



        public virtual Facility DiagnosisFacility
        {
            get
            {
                return this.diagnosisFacilityField;
            }
            set
            {
                this.diagnosisFacilityField = value;
            }
        }



        public virtual DateTime? DateOfFirstReport
        {
            get
            {
                return this.dateOfFirstReportField;
            }
            set
            {
                this.dateOfFirstReportField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DateOfFirstReportSpecified
        {
            get
            {
                return this.dateOfFirstReportFieldSpecified;
            }
            set
            {
                this.dateOfFirstReportFieldSpecified = value;
            }
        }



        public virtual DateTime? DateOfLastReport
        {
            get
            {
                return this.dateOfLastReportField;
            }
            set
            {
                this.dateOfLastReportField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DateOfLastReportSpecified
        {
            get
            {
                return this.dateOfLastReportFieldSpecified;
            }
            set
            {
                this.dateOfLastReportFieldSpecified = value;
            }
        }



        public virtual DateTime? DiagnosisDate
        {
            get
            {
                return this.diagnosisDateField;
            }
            set
            {
                this.diagnosisDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool DiagnosisDateSpecified
        {
            get
            {
                return this.diagnosisDateFieldSpecified;
            }
            set
            {
                this.diagnosisDateFieldSpecified = value;
            }
        }



        public virtual bool PatientDieFromThisIllness
        {
            get
            {
                return this.patientDieFromThisIllnessField;
            }
            set
            {
                this.patientDieFromThisIllnessField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientDieFromThisIllnessSpecified
        {
            get
            {
                return this.patientDieFromThisIllnessFieldSpecified;
            }
            set
            {
                this.patientDieFromThisIllnessFieldSpecified = value;
            }
        }



        public virtual CommonQuestionsTypePatientPregnancyStatusCode? PatientPregnancyStatusCode
        {
            get
            {
                return this.patientPregnancyStatusCodeField;
            }
            set
            {
                this.patientPregnancyStatusCodeField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientPregnancyStatusCodeSpecified
        {
            get
            {
                return this.patientPregnancyStatusCodeFieldSpecified;
            }
            set
            {
                this.patientPregnancyStatusCodeFieldSpecified = value;
            }
        }



        public virtual DateTime? EstimatedDeliveryDate
        {
            get
            {
                return this.estimatedDeliveryDateField;
            }
            set
            {
                this.estimatedDeliveryDateField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool EstimatedDeliveryDateSpecified
        {
            get
            {
                return this.estimatedDeliveryDateFieldSpecified;
            }
            set
            {
                this.estimatedDeliveryDateFieldSpecified = value;
            }
        }



        public virtual int PatientAge
        {
            get
            {
                return this.patientAgeField;
            }
            set
            {
                this.patientAgeField = value;
            }
        }


        [XmlIgnore()]
        public virtual bool PatientAgeSpecified
        {
            get
            {
                return this.patientAgeFieldSpecified;
            }
            set
            {
                this.patientAgeFieldSpecified = value;
            }
        }
    }


    [Serializable()]
    public class Address
    {

        private string addressTypeCodeField;

        private string wardVillageField;

        private string townField;

        private string lGACodeField;

        private string stateCodeField;

        private string countryCodeField;

        private string postalCodeField;

        private string otherAddressInformationField;



        public virtual string AddressTypeCode
        {
            get
            {
                return this.addressTypeCodeField;
            }
            set
            {
                this.addressTypeCodeField = value;
            }
        }



        public virtual string WardVillage
        {
            get
            {
                return this.wardVillageField;
            }
            set
            {
                this.wardVillageField = value;
            }
        }



        public virtual string Town
        {
            get
            {
                return this.townField;
            }
            set
            {
                this.townField = value;
            }
        }



        public virtual string LGACode
        {
            get
            {
                return this.lGACodeField;
            }
            set
            {
                this.lGACodeField = value;
            }
        }



        public virtual string StateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }



        public virtual string CountryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }



        public virtual string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }



        public virtual string OtherAddressInformation
        {
            get
            {
                return this.otherAddressInformationField;
            }
            set
            {
                this.otherAddressInformationField = value;
            }
        }
    }



    [Serializable()]
    public class ProgramArea
    {

        private string programAreaCodeField;



        public virtual string ProgramAreaCode
        {
            get
            {
                return this.programAreaCodeField;
            }
            set
            {
                this.programAreaCodeField = value;
            }
        }
    }


    [Serializable()]
    public class Condition
    {


        public virtual string ConditionCode { get; set; }


        public virtual ProgramArea ProgramArea { get; set; }


        public virtual Address PatientAddress { get; set; }


        public virtual CommonQuestions CommonQuestions { get; set; }


        public virtual ConditionSpecificQuestions ConditionSpecificQuestions { get; set; }

        [XmlElement(ElementName = "Encounters")]
        [JsonProperty("Encounters")]
        public virtual Encounters Encounters { get; set; }

        private List<LaboratoryReport> laboratoryReport;
        [XmlElement("LaboratoryReport")]
        [JsonProperty("LaboratoryReport")]
        [JsonConverter(typeof(SingleOrArrayConverter<LaboratoryReport>))]
        public virtual List<LaboratoryReport> LaboratoryReport_xml
        {
            get
            {
                return laboratoryReport;
            }
            set
            {
                laboratoryReport = value;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<LaboratoryReport> LaboratoryReport
        {
            get
            {
                return laboratoryReport;
            }
            set
            {
                laboratoryReport = value as List<LaboratoryReport>;
            }
        }

        private List<Regimen> regimen;
        [XmlElement("Regimen")]
        [JsonProperty("Regimen")]
        [JsonConverter(typeof(SingleOrArrayConverter<Regimen>))]
        public virtual List<Regimen> Regimen_xml
        {
            get
            {
                return regimen;
            }
            set
            {
                regimen = value;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<Regimen> Regimen
        {
            get
            {
                return regimen;
            }
            set
            {
                regimen = value as List<Regimen>;
            }
        }

        private List<Immunization> immunization;
        [XmlElement("Immunization")]
        [JsonProperty("Immunization")]
        [JsonConverter(typeof(SingleOrArrayConverter<Immunization>))]
        public virtual List<Immunization> Immunization_xml
        {
            get
            {
                return immunization;
            }
            set
            {
                immunization = value;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public virtual IList<Immunization> Immunization
        {
            get
            {
                return immunization;
            }
            set
            {
                immunization = value as List<Immunization>;
            }
        }

        [XmlElement("PregnancyRecord")]
        [JsonProperty("PregnancyRecord")]
        [JsonConverter(typeof(SingleOrArrayConverter<PregnancyEncounterType>))]
        public List<PregnancyEncounterType> PregnancyRecord { get; set; }

        [XmlElement("HIVTestRecords")]
        [JsonProperty("HIVTestRecords")]
        [JsonConverter(typeof(SingleOrArrayConverter<HIVTestingEncounterType>))]
        public List<HIVTestingEncounterType> HIVTestRecords { get; set; }
    }


    [Serializable()]
    public class NoteType
    {

        private string noteField;



        public virtual string Note
        {
            get
            {
                return this.noteField;
            }
            set
            {
                this.noteField = value;
            }
        }
    }

    [XmlRoot(ElementName = "OtherPatientIdentifiers")]
    public class OtherPatientIdentifiers
    {
        [XmlElement(ElementName = "Identifier")]
        [JsonProperty("Identifier")]
        [JsonConverter(typeof(SingleOrArrayConverter<Identifier>))]
        public List<Identifier> Identifier { get; set; }
    }


    [Serializable()]
    [XmlRoot(ElementName = "Identifier")]
    public class Identifier
    {
        [XmlElement(ElementName = "IDNumber")]
        [JsonProperty("IDNumber")]
        public string IDNumber { get; set; }

        [XmlElement(ElementName = "IDTypeCode")]
        [JsonProperty("IDTypeCode")]
        public string IDTypeCode { get; set; }
    }

    [Serializable()]
    public class PatientDemographics
    {

        public virtual string PatientIdentifier { get; set; }


        public virtual Facility TreatmentFacility { get; set; }

        private List<OtherPatientIdentifiers> otherPatientIdentifiers;
        [XmlElement(ElementName = "OtherPatientIdentifiers")]
        [JsonProperty("OtherPatientIdentifiers")]
        [JsonConverter(typeof(SingleOrArrayConverter<OtherPatientIdentifiers>))]
        public virtual List<OtherPatientIdentifiers> OtherPatientIdentifiers
        {
            get
            {
                return otherPatientIdentifiers;
            }
            set
            {
                otherPatientIdentifiers = value as List<OtherPatientIdentifiers>;
            }
        }


        public virtual DateTime? PatientDateOfBirth { get; set; }

        [XmlIgnore()]
        public virtual bool PatientDateOfBirthSpecified { get; set; }


        public virtual PatientDemographicsTypePatientSexCode? PatientSexCode { get; set; }

        [XmlIgnore()]
        public virtual bool PatientSexCodeSpecified { get; set; }


        public virtual bool PatientDeceasedIndicator { get; set; }

        [XmlIgnore()]
        public virtual bool PatientDeceasedIndicatorSpecified { get; set; }


        public virtual DateTime? PatientDeceasedDate { get; set; }

        [XmlIgnore()]
        public virtual bool PatientDeceasedDateSpecified { get; set; }


        public virtual string PatientPrimaryLanguageCode { get; set; }


        public virtual PatientDemographicsTypePatientEducationLevelCode? PatientEducationLevelCode { get; set; }

        [XmlIgnore()]
        public virtual bool PatientEducationLevelCodeSpecified { get; set; }


        public virtual PatientDemographicsTypePatientOccupationCode? PatientOccupationCode { get; set; }


        [XmlIgnore()]
        public virtual bool PatientOccupationCodeSpecified { get; set; }


        public virtual PatientDemographicsTypePatientMaritalStatusCode? PatientMaritalStatusCode { get; set; }

        [XmlIgnore()]
        public virtual bool PatientMaritalStatusCodeSpecified { get; set; }



        public virtual string StateOfNigeriaOriginCode { get; set; }


        public virtual NoteType PatientNotes { get; set; }
    }

    [Serializable()]
    public class IndividualReport
    {

        public virtual PatientDemographics PatientDemographics { get; set; }

        [XmlElement("Condition", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [JsonProperty("Condition")]
        [JsonConverter(typeof(SingleOrArrayConverter<Condition>))]
        public virtual List<Condition> Condition { get; set; }

    }

    [Serializable()]
    public class AntenatalRegistrationType
    {
        public DateTime DateOfVisit { get; set; }
        public DateTime? LastMenstralPeriod { get; set; }
        public int? GestationalAgeAtANCRegistration { get; set; }
        public int? Gravida { get; set; }
        public int? Parity { get; set; }
        public string SourceOfReferral { get; set; }
        public DateTime? ExpectedDateOfDelivery { get; set; }
        public AntenatalRegistrationTypeTestedForSyphilis? TestedForSyphilis { get; set; }
        public AntenatalRegistrationTypeSyphilisTestResult? SyphilisTestResult { get; set; }
        public AntenatalRegistrationTypeTreatedForSyphilis? TreatedForSyphilis { get; set; }
        public AntenatalRegistrationTypeRefferedSyphilisPositiveClient RefferedSyphilisPositiveClient { get; set; }
    }

    [Serializable()]
    public class PartnerDetailType
    {
        public int? PartnerAge { get; set; }
        public PartnerDetailTypePartnerPreTestCounseled? PartnerPreTestCounseled { get; set; }
        public PartnerDetailTypePartnerAcceptsHIVTest? PartnerAcceptsHIVTest { get; set; }
        public DateTime? DateOfTest { get; set; }
        public PartnerDetailTypePartnerHIVTestResult? PartnerHIVTestResult { get; set; }
        public PartnerDetailTypePartnerPostTestCounseled? PartnerPostTestCounseled { get; set; }
        public PartnerDetailTypePartnerHBVStatus? PartnerHBVStatus { get; set; }
        public PartnerDetailTypePartnerHCVStatus? PartnerHCVStatus { get; set; }
        public PartnerDetailTypePartnerSyphilisStatus? PartnerSyphilisStatus { get; set; }
        public PartnerDetailTypePartnerReferredTo? PartnerReferredTo { get; set; }
    }

    [Serializable()]
    public class DeliveryEncounterType
    {
        public DateTime DateOfDelivery { get; set; }
        public DeliveryEncounterTypeTimeOfHIVDiagnosis? TimeOfHIVDiagnosis { get; set; }
        public int? GestationalAgeAtDelivery { get; set; }
        public DeliveryEncounterTypeHBVStatus? HBVStatus { get; set; }
        public DeliveryEncounterTypeHCVStatus? HCVStatus { get; set; }
        public DeliveryEncounterTypeWomanOnART? WomanOnART { get; set; }
        public DeliveryEncounterTypeARTStartedinLDWard? ARTStartedinLDWard { get; set; }
        public DeliveryEncounterTypeROMDeliveryInterval? ROMDeliveryInterval { get; set; }
        public DeliveryEncounterTypeModeOfDelivery? ModeOfDelivery { get; set; }
        public DeliveryEncounterTypeEpisiotomy? Episiotomy { get; set; }
        public DeliveryEncounterTypeVaginalTear? VaginalTear { get; set; }
        public DeliveryEncounterTypeFeedingDecision? FeedingDecision { get; set; }
        public DeliveryEncounterTypeMaternalOutcome? MaternalOutcome { get; set; }
    }

    [Serializable()]
    public class ChildType
    {
        public ChildBirthDetailsType ChildBirthDetails { get; set; }
        public ChildFollowUpType ChildFollowUp { get; set; }
    }

    [Serializable()]
    public class ChildFollowUpType
    {
        public ChildFollowUpTypeInfantARVType? InfantARVType { get; set; }
        public ChildFollowUpTypeTimingofARVProphylaxis? TimingofARVProphylaxis { get; set; }
        public int? AgeAtCTXInitiation { get; set; }
        public ChildFollowUpTypeInfantOutcomeAt18Months? InfantOutcomeAt18Months { get; set; }
        public System.DateTime? DateLinkedtoARTClinic { get; set; }
        public string ARTEnrollmentNumber { get; set; }

        [XmlElement("HealthFacilityVisits", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [JsonProperty("HealthFacilityVisits")]
        [JsonConverter(typeof(SingleOrArrayConverter<HealthFacilityVisitType>))]
        public List<HealthFacilityVisitType> HealthFacilityVisits { get; set; }

        [XmlElement("PCRTests", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [JsonProperty("PCRTests")]
        [JsonConverter(typeof(SingleOrArrayConverter<InfantPCRTestingType>))]
        public List<InfantPCRTestingType> PCRTests { get; set; }

        [XmlElement("RapidTests", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [JsonProperty("RapidTests")]
        [JsonConverter(typeof(SingleOrArrayConverter<InfantRapidTestingType>))]
        public List<InfantRapidTestingType> RapidTests { get; set; }
    }

    [Serializable()]
    public class InfantRapidTestingType
    {
        public int AgeAtTest { get; set; }
        public DateTime? DateOfTest { get; set; }
        public InfantRapidTestingTypeRapidTestResult RapidTestResult { get; set; }
    }

    [Serializable()]
    public class InfantPCRTestingType
    {
        public int? AgeAtTest { get; set; }
        public DateTime? DateSampleCollected { get; set; }
        public DateTime? DateSampleSent { get; set; }
        public DateTime? DateResultRecievedAtFacility { get; set; }
        public System.DateTime? DateCaregiverGivenResult { get; set; }
        public InfantPCRTestingTypePCRTestResult? PCRTestResult { get; set; }
    }

    [Serializable()]
    public class HealthFacilityVisitType
    {
        public DateTime VisitDate { get; set; }
        public HealthFacilityVisitTypeVisitStatus? VisitStatus { get; set; }
        public HealthFacilityVisitTypeCotrimoxazole? Cotrimoxazole { get; set; }
        public int? Weight { get; set; }
        public HealthFacilityVisitTypeBreastFeeding? BreastFeeding { get; set; }
    }

    [Serializable()]
    public class ChildBirthDetailsType
    {
        public string ChildHospitalNumber { get; set; }
        public string ChildEIDNumber { get; set; }
        public System.DateTime ChildDateOfBirth { get; set; }
        public ChildBirthDetailsTypeChildSexCode ChildSexCode { get; set; }
        public ChildBirthDetailsTypeChildStatus? ChildStatus { get; set; }
        public ChildBirthDetailsTypeChildGivenNVPWithin72hrs? ChildGivenNVPWithin72hrs { get; set; }
        public ChildBirthDetailsTypeHBVExposedInfantGivenHepBIg? HBVExposedInfantGivenHepBIg { get; set; }
        public ChildBirthDetailsTypeNonHBVExposedInfantGivenHBV? NonHBVExposedInfantGivenHBV { get; set; }
        public int? APGARScore { get; set; }
        public int? BirthMUAC { get; set; }
        public int? BirthLenght { get; set; }
        public int? BirthWeight { get; set; }
        public int? HeadCircumferenceAtBirth { get; set; }
    }

    [Serializable()]
    public class PregnancyEncounterType
    {
        public AntenatalRegistrationType AntenatalRegistration { get; set; }

        public PartnerDetailType PartnerDetail { get; set; }
        public DeliveryEncounterType DeliveryEncounter { get; set; }

        [XmlElement("Child")]
        [JsonProperty("Child")]
        [JsonConverter(typeof(SingleOrArrayConverter<ChildType>))]
        public List<ChildType> Child { get; set; }
    }

    [Serializable()]
    public class HIVTestDetailsType
    {
        public string ClientCode { get; set; }
        public DateTime DateOfVisit { get; set; }
        public HIVTestDetailsTypeSetting? Setting { get; set; }
        public HIVTestDetailsTypeReferredFrom? ReferredFrom { get; set; }
        public HIVTestDetailsTypeFirstTimeVisit? FirstTimeVisit { get; set; }
        public HIVTestDetailsTypeTypeOfSession? TypeOfSession { get; set; }
        public HIVTestDetailsTypeMaritalStatus? MaritalStatus { get; set; }
        public int? NumberOfChildrenLessThanFive { get; set; }
        public int? NumberOfWivesOrCowives { get; set; }
        public HIVTestDetailsTypeClientIdentiedFromIndex? ClientIdentiedFromIndex { get; set; }
        public HIVTestDetailsTypeIndexType? IndexType { get; set; }
        public string IndexClientID { get; set; }
    }

    [Serializable()]
    public class KnowledgeAssessmentType
    {
        public bool? PreviouslyTestedHIVNegative { get; set; }
        public bool? ClientPregnant { get; set; }
        public bool? ClientInformedAboutHIVTransmissionRoutes { get; set; }
        public bool? ClientInformedAboutRiskFactors { get; set; }
        public bool? ClientInformedAboutPreventingHIV { get; set; }
        public bool? ClientInformedAboutPossibleTestResults { get; set; }
        public bool? InformedConsentForHIVTestingGiven { get; set; }
    }

    [Serializable()]
    public class HIVRiskAssessmentType
    {
        public bool? EverHadSexualIntercourse { get; set; }
        public bool? BloodTransfussionInLast3Months { get; set; }
        public bool? UnprotectedSexWithCasualPartnerinLast3Months { get; set; }
        public bool? UnprotectedSexWithRegularPartnerInLast3Months { get; set; }
        public bool? STIInLast3Months { get; set; }
        public bool? MoreThan1SexPartnerDuringLast3Months { get; set; }
    }

    [Serializable()]
    public class ClinicalTBScreeningType
    {
        public bool? CurrentCough { get; set; }
        public bool? WeigthLoss { get; set; }
        public bool? Fever { get; set; }
        public bool? NightSweats { get; set; }
    }

    [Serializable()]
    public class SyndromicSTIScreeningType
    {
        public bool? VaginalDischargeOrBurningWhenUrinating { get; set; }
        public bool? LowerAbdominalPainsWithOrWithoutVaginalDischarge { get; set; }
        public bool? UrethralDischargeOrBurningWhenUrinating { get; set; }
        public bool? ScrotalSwellingAndPain { get; set; }
        public bool? GenitalSoreOrSwollenInguinalLymphNodes { get; set; }
    }

    [Serializable()]
    public class HIVPreTestCounsellingType
    {
        public KnowledgeAssessmentType KnowledgeAssessment { get; set; }

        public HIVRiskAssessmentType HIVRiskAssessment { get; set; }

        public ClinicalTBScreeningType ClinicalTBScreening { get; set; }

        public SyndromicSTIScreeningType SyndromicSTIScreening { get; set; }
    }

    [Serializable()]
    public class PostTestCounsellingType
    {
        public PostTestCounsellingTypeTestedForHIVBeforeWithinThisYear? TestedForHIVBeforeWithinThisYear { get; set; }
        public bool? HIVRequestAndResultFormSignedByTester { get; set; }
        public bool? HIVRequestAndResultFormFilledWithCTIForm { get; set; }
        public bool? ClientRecievedHIVTestResult { get; set; }
        public bool? PostTestCounsellingDone { get; set; }
        public bool? RiskReductionPlanDeveloped { get; set; }
        public bool? PostTestDisclosurePlanDeveloped { get; set; }
        public bool? WillBringPartnerForHIVTesting { get; set; }
        public bool? WillBringOwnChildrenForHIVTesting { get; set; }
        public bool? ProvidedWithInformationOnFPandDualContraception { get; set; }
        public bool? ClientOrPartnerUseFPMethodsOtherThanCondoms { get; set; }
        public bool? ClientOrPartnerUseCondomsAsOneFPMethods { get; set; }
        public bool? CorrectCondomUseDemonstrated { get; set; }
        public bool? CondomsProvidedToClient { get; set; }
        public bool? ClientReferredToOtherServices { get; set; }
    }

    [Serializable()]
    public class HIVTestResultsType
    {
        public DateTime? ScreeningTestDate { get; set; }
        public HIVTestResultsTypeScreeningTestResult? ScreeningTestResult { get; set; }
        public DateTime? ConfirmatoryTestDate { get; set; }
        public HIVTestResultsTypeConfirmatoryTestResult? ConfirmatoryTestResult { get; set; }
        public DateTime? TieBreakerTestDate { get; set; }
        public HIVTestResultsTypeTieBreakerTestResult? TieBreakerTestResult { get; set; }
        public HIVTestResultsTypeFinalHIVTestResult? FinalHIVTestResult { get; set; }
    }

    [Serializable()]
    public class HIVRecencyTestResultType
    {
        public string RecencyTestName { get; set; }
        public DateTime? RecencyTestDate { get; set; }
        public HIVRecencyTestResultTypeRapidRecencyAssay? RapidRecencyAssay { get; set; }
        public DateTime? ViralLoadTestDate { get; set; }
        public string ViralLoadTestResult { get; set; }
        public HIVRecencyTestResultTypeFinalHIVRecentTestinResult? FinalHIVRecentTestinResult { get; set; }
    }

    [Serializable()]
    public class OtherTestingServicesType
    {
        public OtherTestingServicesTypeSyphilisTestResult? SyphilisTestResult { get; set; }
        public OtherTestingServicesTypeHepBTestResult? HepBTestResult { get; set; }
        public OtherTestingServicesTypeHepCTestResult? HepCTestResult { get; set; }
    }

    [Serializable()]
    public class PartnerNotificationServicesType
    {
        public string PartnerName { get; set; }
        public PartnerNotificationServicesTypePartnerGender? PartnerGender { get; set; }
    }

    [Serializable()]
    public class HIVTestingEncounterType
    {
        public HIVTestDetailsType TestDetail { get; set; }

        public HIVPreTestCounsellingType PreTestCounselling { get; set; }

        public PostTestCounsellingType PostTestCounselling { get; set; }

        public HIVTestResultsType HIVTestResult { get; set; }

        public HIVRecencyTestResultType HIVRecencyTestResult { get; set; }

        public OtherTestingServicesType OtherTestingServices { get; set; }

        [XmlElement("PartnerNotificationServices")]
        [JsonProperty("PartnerNotificationServices")]
        [JsonConverter(typeof(SingleOrArrayConverter<PartnerNotificationServicesType>))]
        public List<PartnerNotificationServicesType> PartnerNotificationServices { get; set; }

    }
}
