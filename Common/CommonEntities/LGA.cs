using FluentNHibernate.Mapping;
using System.Collections.Generic;

namespace Common.CommonEntities
{
    public class LGA
    {
        public virtual string lga_code { get; set; }

        public virtual string lga_name { get; set; }

        public virtual State State { get; set; }

        public virtual string state_code { get; set; }

        public virtual string lga_hm_longcode { get; set; }
        public virtual string alternative_name { get; set; }
         

        public virtual string DisplayName
        {
            get
            {
                if (this == null) return "";
                return string.Format("{0} ({1})", !string.IsNullOrEmpty(alternative_name) ? alternative_name : lga_name, State.state_name);
            }
        }
    }

    public class State
    {
        public virtual string state_name { get; set; }
        public virtual string state_code { get; set; }
        public virtual string geo_polictical_region { get; set; }

        public virtual List<LGA> LgaList { get; set; }
    }

    public class StateMap : ClassMap<State>
    {
        public StateMap()
        {
            Table("admin_states");
            Id(x => x.state_code);
            Map(x => x.state_name);
            Map(x => x.geo_polictical_region); 
        }
    }

    public class LGAMap : ClassMap<LGA>
    {
        public LGAMap()
        {
            Table("admin_lga");
            Id(x => x.lga_code);
            Map(x => x.lga_name);
            Map(x => x.lga_hm_longcode);
            Map(x => x.alternative_name);
            
            References(x => x.State).Column("state_code").Not.LazyLoad();
        }
    }
}