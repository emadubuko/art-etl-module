using ART.DAL.Entities;
using Common.CommonEntities;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ART.DAL.ViewModels
{
    public class ProfileViewModel : AutomaticViewModel<UserProfile>
    {
        public IList<ImplementingPartners> Organization { get; set; }
        public Dictionary<string,string> Roles { get; set; }
    }

    public class AutomaticViewModel<T> where T : class
    {
        private Dictionary<string, object> _fieldCollection;
        public Dictionary<string, object> FieldCollection
        {
            get
            {
                if (_fieldCollection != null && _fieldCollection.Count() != 0)
                    return _fieldCollection;

                else
                {
                    _fieldCollection = new Dictionary<string, object>();
                    foreach (var info in typeof(T).GetProperties().Where(x => x.CanWrite && !Attribute.IsDefined(x, typeof(XmlIgnoreAttribute))).ToArray())
                    {
                        _fieldCollection.Add(Utils.PasCaseConversion(info.Name), info.PropertyType.Name);
                    }
                    return _fieldCollection;
                }
            }
        }
    }
}
