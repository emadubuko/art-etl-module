using Common;
using FluentNHibernate.Mapping;
using System;

namespace FileService.Entities
{
    public class NDRFile : BaseT
    {
        // public virtual Int64 Id { get; set; }
        public virtual string File { get; set; }
        public virtual string FileName { get; set; }
        public virtual DateTime DateUploaded { get; set; }
        public virtual FileProcessingStatus Status { get; set; }
        public virtual string BatchNumber { get; set; }
        public virtual int UploadedBy { get; set; }
        public virtual FileZipUpload FileBatch { get; set; }
        // public virtual int FileBatchId { get; set; }
        //public virtual string ParentFileName { get; set; }
    }

    public enum FileProcessingStatus
    {
        Pending = 0, Processed = 1, Failed = 2, Processing = 3, Completed = 4
    }

    public class NDRFileMap : ClassMap<NDRFile>
    {
        public NDRFileMap()
        {
            Table("ndrFile");

            Id(i => i.Id);
            Map(x => x.BatchNumber);
            Map(x => x.DateUploaded);
            References(x => x.FileBatch).Column("FileBatchId");
            //Map(x => x.FileBatchId);
            Map(x => x.FileName);
           // Map(x => x.ParentFileName);
            Map(x => x.Status);
            Map(x => x.UploadedBy);
        }
    }
}
