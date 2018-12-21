using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ViewModels
{
    public class RADETNDRAPRModel
    {
        public string IP { get; set; }
        public string State { get; set; }
        public string state_code { get; set; }
        public string LGA { get; set; }
        public string lga_code { get; set; }

        public string Facility { get; set; }
        public string DatimCode { get; set; }
        public string AgeGroup { get; set; }
        public string Sex { get; set; }
        public int Tx_CURR { get; set; }
        public int Tx_New { get; set; }
        public int TX_PVLS_Num { get; set; }
        public int TX_PVLS_Den { get; set; }

    }
}
