using System;
using System.Collections.Generic;
using System.Text;
using Common;
using FluentNHibernate.Mapping;


namespace FileService.Entities
{
   public class FileZipUpload : BaseT
    { 
        public virtual string FilePath { get; set; }
        public virtual DateTime DateUploaded { get; set; }
        public virtual FileBatchStatus Status { get; set; }
        public virtual string BatchNumber { get; set; }
        public virtual int UploadedById { get; set; }
        public virtual string UploadedFileName { get; set; }
        public virtual string ErrorMessage { get; set; }
    }

    public enum FileBatchStatus
    {
        Pending = 0, Processing = 1, Completed = 2, Failed = 3
    }

    public class FileZipUploadMap : ClassMap<FileZipUpload>
    {
        public FileZipUploadMap()
        {
            Table("fileZipUpload");

            Id(x => x.Id);
            Map(x => x.FilePath).Length(int.MaxValue);
            Map(x => x.DateUploaded);
            Map(x => x.Status);
            Map(x => x.BatchNumber);
            Map(x => x.UploadedById);
            Map(x => x.UploadedFileName).Length(int.MaxValue);
            Map(x => x.ErrorMessage).Length(int.MaxValue);
        }
    }
}
