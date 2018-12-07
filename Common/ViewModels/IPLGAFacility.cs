using Common.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ViewModels
{
    public class IPLGAFacility
    {
        public string IP { get; set; }
        public string FacilityName { get; set; }
        public string DatimFacilityCode { get; set; }
        public LGA LGA { get; set; }
    }
}
