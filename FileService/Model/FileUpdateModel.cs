using FileService.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileService.Model
{
    public class FileUpdateModel
    {
        public List<int> Ids { get; set; }
        public FileProcessingStatus Status { get; set; }
    }

    public class UploadReportModel
    {
        public string BatchNumber { get; set; }
        public string Status { get; set; }
        public string TotalFile { get; set; }
        public int TotalFileProcessed { get; set; }
        public string TotalFacilities { get; set; }
        public string File { get; set; }
        public string ViewErrorbutton { get; set; }
        public DateTime DateUploaded { get; set; }
        public string _dateUploaded { get; set; }
        public string UploadedBy { get; set; }
        public string IP { get; set; }
    }

    public class FileUploadViewModel
    {
        public int Id { get; set; }
        public int DT_RowId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UploadedBy { get; set; }
        public string IP { get; set; }
        public string Status { get; set; }
        public string StatusString { get; set; }
        public long TotalFile { get; set; }
        public long TotalFacilities { get; set; }
        public long TotalFileProcessed { get; set; }
        public string FileName { get; set; }
        public string ViewErrorbutton { get; set; }
    }

    public class ZipFileSearchModel
    {
        public string IP { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string BatchNumber { get; set; }
        public string UploadedBy { get; set; }
        public int StartIndex { get; set; }
        public int? MaxRows { get; set; }
    }

}
