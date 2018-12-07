using Common;
using Common.Entities;
using Common.Utility;
using FluentNHibernate.Mapping;
using System.Collections.Generic;

namespace FileService.Entities
{
    public class ValidationSummary : BaseT
    {
        // public virtual long Id { get; set; }
        public virtual string FacilityName { get; set; }
        public virtual int TotalPatients { get; set; }
        public virtual int TotalFiles { get; set; }
        public virtual int ValidFiles { get; set; }
        public virtual int InvalidFiles { get; set; }
        public virtual List<ErrorDetails> ErrorDetails { get; set; }
        public virtual string FileUploadBacthNumber { get; set; }
    }


    public class ValidationSummaryMap : ClassMap<ValidationSummary>
    {
        public ValidationSummaryMap()
        {
            Table("validationSummaryLog");

            Id(x => x.Id);
            Map(x => x.FacilityName);
            Map(x => x.TotalPatients);
            Map(x => x.ValidFiles);
            Map(x => x.InvalidFiles);
            Map(x => x.FileUploadBacthNumber);
            Map(x => x.ErrorDetails).CustomType(typeof(XmlType<List<ErrorDetails>>));
        }
    }

}
