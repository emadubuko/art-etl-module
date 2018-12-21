using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class FileUploadViewModel
    {
        public string BatchNumber { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UploadedBy { get; set; }
        public string IP { get; set; }
        //public Status Status { get; set; }
        public string StatusString { get; set; }
        public int TotalFile { get; set; }
        public string TotalFacilities { get; set; }
        public string FileName { get; set; }
        public string ViewErrorbutton { get; set; }
    }

    public class IPSummary
    {
        public string IP { get; set; }
        public int? Active { get; set; }
        public int? Inactive { get; set; }
        public int DATIM_TX_CURR { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public double? Concurrence
        {
            get
            {
                return 100 * (1.0 * Active / DATIM_TX_CURR);
            }
        }
    }
}
