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


}
