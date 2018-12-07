using System;
using System.Collections.Generic;
using System.Text;

namespace ART.DAL.ViewModel
{
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
