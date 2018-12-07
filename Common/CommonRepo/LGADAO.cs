using Common.CommonEntities;
using Common.DBFasade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.CommonRepo
{
    public class LGADAO : BaseDAO<LGA, string>
    {
        public static LGA FindLGA(IList<LGA> lgas, string lgaName, string state)
        {
            LGA lga = null;
            string _lga_name = lgaName.ToLower().Replace(" local government area", "").Trim();
            string _state_name = state.ToLower().Replace(" state", "").Trim();

            lga = lgas.FirstOrDefault(x => x.lga_name.ToLower() == _lga_name && x.State.state_name.ToLower() == _state_name);
            if (lga == null)
            {
                lga = lgas.FirstOrDefault(x => !string.IsNullOrEmpty(x.alternative_name)
                && x.alternative_name.ToLower() == _lga_name && x.State.state_name.ToLower() == _state_name);
            }
            return lga;
        }

    }
}
