using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum FacilityTypeFacilityTypeCode
    {


        IP,


        FAC,


        LGA,


        SGA,


        FGA,


        OTH,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum CommonQuestionsTypePatientPregnancyStatusCode
    {


        P,


        PMTCT,


        NP,


        NK,
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum MessageHeaderTypeMessageStatusCode
    {


        INITIAL,


        UPDATED,


        REDACTED,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum PatientDemographicsTypePatientSexCode
    {


        F,


        M,


        A,


        N,


        O,


        U,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum PatientDemographicsTypePatientEducationLevelCode
    {
        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,


        [XmlEnum("6")]
        Item6,


        [XmlEnum("7")]
        Item7,

        [XmlEnum("NA")]
        NA,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum PatientDemographicsTypePatientOccupationCode
    {


        UNE,


        EMP,


        RET,


        STU,


        NA,


        UNK,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum PatientDemographicsTypePatientMaritalStatusCode
    {


        N,


        C,


        D,


        P,


        I,


        E,


        G,


        M,


        O,


        R,


        A,


        S,


        U,


        B,


        T,

        W,
        NA
    }

    [Serializable()]
    [XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemChoiceType
    {


        AnswerCode,


        AnswerDate,


        AnswerDateTime,


        AnswerNumeric,


        AnswerText,
    }


    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum LaboratoryReportTypeBaselineRepeatCode
    {


        B,


        R,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeCareEntryPoint
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,


        [XmlEnum("6")]
        Item6,


        [XmlEnum("7")]
        Item7,


        [XmlEnum("8")]
        Item8,


        [XmlEnum("9")]
        Item9,


        [XmlEnum("10")]
        Item10,


        [XmlEnum("11")]
        Item11,


        [XmlEnum("12")]
        Item12,


        [XmlEnum("13")]
        Item13,


        [XmlEnum("14")]
        Item14,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeFirstHIVTestMode
    {


        HIVAb,


        HIVPCR,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeReasonMedicallyEligible
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeWHOClinicalStageARTStart
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeFunctionalStatusStartART
    {


        W,


        A,


        B,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeTransferredOutStatus
    {


        A,


        P,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeStatusAtDeath
    {


        A,


        P,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeCauseOfDeathHIVRelated
    {


        Y,


        N,


        U,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVQuestionsTypeInitialTBStatus
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeEDDandPMTCTLink
    {


        P,


        PMTCT,


        NP,


        NK,
    }

    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypePatientFamilyPlanningCode
    {


        FP,


        NOFP,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeFunctionalStatus
    {


        W,


        A,


        B,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeWHOClinicalStage
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeTBStatus
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeARVDrugAdherence
    {


        G,


        F,


        P,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeWhyPoorFairARVDrugAdherence
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,


        [XmlEnum("6")]
        Item6,


        [XmlEnum("7")]
        Item7,


        [XmlEnum("8")]
        Item8,


        [XmlEnum("9")]
        Item9,


        [XmlEnum("10")]
        Item10,


        [XmlEnum("11")]
        Item11,


        [XmlEnum("12")]
        Item12,


        [XmlEnum("13")]
        Item13,


        [XmlEnum("14")]
        Item14,


        [XmlEnum("15")]
        Item15,


        [XmlEnum("16")]
        Item16,


        [XmlEnum("17")]
        Item17,


        [XmlEnum("18")]
        Item18,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeCotrimoxazoleAdherence
    {


        G,


        F,


        P,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeWhyPoorFairCotrimoxazoleDrugAdherence
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,


        [XmlEnum("6")]
        Item6,


        [XmlEnum("7")]
        Item7,


        [XmlEnum("8")]
        Item8,


        [XmlEnum("9")]
        Item9,


        [XmlEnum("10")]
        Item10,


        [XmlEnum("11")]
        Item11,


        [XmlEnum("12")]
        Item12,


        [XmlEnum("13")]
        Item13,


        [XmlEnum("14")]
        Item14,


        [XmlEnum("15")]
        Item15,


        [XmlEnum("16")]
        Item16,


        [XmlEnum("17")]
        Item17,


        [XmlEnum("18")]
        Item18,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeINHAdherence
    {


        G,


        F,


        P,
    }



    [Serializable()]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum HIVEncounterTypeWhyPoorFairINHDrugAdherence
    {


        [XmlEnum("1")]
        Item1,


        [XmlEnum("2")]
        Item2,


        [XmlEnum("3")]
        Item3,


        [XmlEnum("4")]
        Item4,


        [XmlEnum("5")]
        Item5,


        [XmlEnum("6")]
        Item6,


        [XmlEnum("7")]
        Item7,


        [XmlEnum("8")]
        Item8,


        [XmlEnum("9")]
        Item9,


        [XmlEnum("10")]
        Item10,


        [XmlEnum("11")]
        Item11,


        [XmlEnum("12")]
        Item12,


        [XmlEnum("13")]
        Item13,


        [XmlEnum("14")]
        Item14,


        [XmlEnum("15")]
        Item15,


        [XmlEnum("16")]
        Item16,


        [XmlEnum("17")]
        Item17,


        [XmlEnum("18")]
        Item18,
    }

    ////PMTCT

    public enum AntenatalRegistrationTypeTestedForSyphilis
    {

        Y,
        N,
        U,
    }

    public enum AntenatalRegistrationTypeSyphilisTestResult
    {
        Pos,
        Neg,
    }

    public enum AntenatalRegistrationTypeTreatedForSyphilis
    {
        Y,
        N,
        U,
    }

    public enum AntenatalRegistrationTypeRefferedSyphilisPositiveClient
    {
        Y,
        N,
        U,
    }

    public enum PartnerDetailTypePartnerPreTestCounseled
    {
        Y,
        N,
        U,
    }

    public enum PartnerDetailTypePartnerAcceptsHIVTest
    {
        Y,
        N,
        U,
    }

    public enum PartnerDetailTypePartnerHIVTestResult
    {
        Pos,
        Neg,
    }

    public enum PartnerDetailTypePartnerPostTestCounseled
    {
        Y,
        N,
        U,
    }

    public enum PartnerDetailTypePartnerHBVStatus
    {
        Pos,
        Neg,
    }

    public enum PartnerDetailTypePartnerHCVStatus
    {
        Pos,
        Neg,
    }

    public enum PartnerDetailTypePartnerSyphilisStatus
    {
        R,
        NR,
    }

    public enum PartnerDetailTypePartnerReferredTo
    {
        FP,
        ART,
        Other,
    }

    public enum DeliveryEncounterTypeTimeOfHIVDiagnosis
    {
        [XmlEnum("1")] Item1,
        [XmlEnum("2")] Item2,
        [XmlEnum("3")] Item3,
        [XmlEnum("4")] Item4,
    }

    public enum DeliveryEncounterTypeHBVStatus
    {
        Pos, Neg,
    }

    public enum DeliveryEncounterTypeHCVStatus
    {
        Pos, Neg,
    }

    public enum DeliveryEncounterTypeWomanOnART
    {
        Y, N, U,
    }

    public enum DeliveryEncounterTypeARTStartedinLDWard
    {
        Y, N, U,
    }

    public enum DeliveryEncounterTypeROMDeliveryInterval
    {
        [XmlEnum("1")] Item1, [XmlEnum("2")] Item2,
    }

    public enum DeliveryEncounterTypeModeOfDelivery
    {
        [XmlEnum("1")] Item1,
        [XmlEnum("2")] Item2,
        [XmlEnum("3")] Item3,
        [XmlEnum("4")] Item4,
    }

    public enum DeliveryEncounterTypeEpisiotomy
    {
        Y, N, U,
    }

    public enum DeliveryEncounterTypeVaginalTear
    {
        Y, N, U,
    }

    public enum DeliveryEncounterTypeFeedingDecision
    {
        EBF, ERF, Mixed,
    }

    public enum DeliveryEncounterTypeMaternalOutcome
    {
        Alive, Dead,
    }

    public enum ChildBirthDetailsTypeChildSexCode
    {
        F, M, A, N, O, U,
    }

    public enum ChildBirthDetailsTypeChildStatus
    {
        Alive, SB, NND,
    }

    public enum ChildBirthDetailsTypeChildGivenNVPWithin72hrs
    {
        Y, N, U,
    }

    public enum ChildBirthDetailsTypeHBVExposedInfantGivenHepBIg
    {
        Y, N, U,
    }

    public enum ChildBirthDetailsTypeNonHBVExposedInfantGivenHBV
    {
        Y, N, U,
    }
    public enum ChildFollowUpTypeInfantARVType
    {

        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,
    }

    public enum ChildFollowUpTypeTimingofARVProphylaxis
    {

        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,
    }

    public enum ChildFollowUpTypeInfantOutcomeAt18Months
    {

        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,

        [XmlEnum("5")]
        Item5,

        [XmlEnum("6")]
        Item6,

        [XmlEnum("7")]
        Item7,
    }
    public enum HealthFacilityVisitTypeVisitStatus
    {
        A, TI, TO, L, DC, X, LTFU, D,
    }

    public enum HealthFacilityVisitTypeCotrimoxazole
    {
        Y, N, U,
    }

    public enum HealthFacilityVisitTypeBreastFeeding
    {
        Y, N, U,
    }
    public enum InfantPCRTestingTypePCRTestResult
    {
        Pos,
        Neg,
        NP,
    }
    public enum InfantRapidTestingTypeRapidTestResult
    {
        Pos,
        Neg,
        Indet,
    }
    public enum HIVTestDetailsTypeSetting
    {
        CT, PMTCT, TB, STI, FP, OPD, Ward, Outreach, Standalone, Others,
    }

    public enum HIVTestDetailsTypeReferredFrom
    {
        Self, TB, STI, FP, OPD, Ward, BB, Others,
    }

    public enum HIVTestDetailsTypeFirstTimeVisit
    {
        Y, N, U,
    }

    public enum HIVTestDetailsTypeTypeOfSession
    {
        Individual, Couple, Previous,
    }

    public enum HIVTestDetailsTypeMaritalStatus
    {
        N, C, D, P, I, E, G, M, O, R, A, S, U, B, T, W,
    }

    public enum HIVTestDetailsTypeClientIdentiedFromIndex
    {
        Y, N, U,
    }

    public enum HIVTestDetailsTypeIndexType
    {
        Biological, Sexual, Social,
    }

    public enum PostTestCounsellingTypeTestedForHIVBeforeWithinThisYear
    {
        [XmlEnum("1")]
        Item1,
        [XmlEnum("2")]
        Item2,
        [XmlEnum("3")]
        Item3,
        [XmlEnum("4")]
        Item4,
    }
    public enum HIVTestResultsTypeScreeningTestResult
    {
        R, NR,
    }

    public enum HIVTestResultsTypeConfirmatoryTestResult
    {
        R, NR,
    }

    public enum HIVTestResultsTypeTieBreakerTestResult
    {
        R, NR,
    }

    public enum HIVTestResultsTypeFinalHIVTestResult
    {
        Pos, Neg,
    }
    public enum HIVRecencyTestResultTypeRapidRecencyAssay
    {
        Recent,
        //[XmlEnum("Long term")]
        Longterm,
    }

    public enum HIVRecencyTestResultTypeFinalHIVRecentTestinResult
    {
        Recent,
        //[XmlEnum("Long term")]
        Longterm,
    }
    public enum OtherTestingServicesTypeSyphilisTestResult
    {
        R, NR,
    }

    public enum OtherTestingServicesTypeHepBTestResult
    {
        Pos, Neg,
    }

    public enum OtherTestingServicesTypeHepCTestResult
    {
        Pos, Neg,
    }
    public enum PartnerNotificationServicesTypePartnerGender
    {
        F,
        M,
        A,
        N,
        O,
        U,
    }
}
