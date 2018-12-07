using System.Xml.Serialization;

namespace Common
{
    public abstract class BaseT : IEntity
    {
        [XmlIgnore]
        //[JsonIgnore]
        //[JsonProperty(Required = Required.Default)]
        public virtual int Id { get; set; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }
}
