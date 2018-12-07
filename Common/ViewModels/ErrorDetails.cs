using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class ErrorDetails : BaseT
    {
        public virtual string FileName { get; set; }
        public virtual string DataElement { get; set; }
        public virtual string ErrorMessage { get; set; }
        public virtual string PatientIdentifier { get; set; }
        public virtual bool CrticalError { get; set; }
    }     
}
