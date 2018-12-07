using Common.CommonEntities;
using Common.DBFasade;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Common.CommonRepo
{
    public class IPRepo : BaseDAO<ImplementingPartners, int>
    {
        public ImplementingPartners SearchByShortName(string shortName)
        {
            var session = BuildSession();
            ICriteria criteria = session.CreateCriteria<ImplementingPartners>()
            .Add(Restrictions.Like("ShortName", shortName, MatchMode.Anywhere));
            var Ip = criteria.UniqueResult<ImplementingPartners>();

            return Ip;
        }



        public OnboardedFacility RetrievebyCodeandIP(string code, int ip)
        {
            ISession session = BuildSession();
            var ipf = session.Query<IPFacility>().Where(x => x.Facility.DatimFacilityCode == code && x.IP.Id == ip).FirstOrDefault();
            return ipf != null ? ipf.Facility : null;
        }



    }
}