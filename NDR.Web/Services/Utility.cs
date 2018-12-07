using Common.CommonEntities;
using Common.CommonRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NDR.Web.Services
{
    public class Utility
    {
        public static string DisplayBadge
        {
            get
            {
                UserProfile profile = null;
                //if (HttpContext.Current.Session[".:LoggedInProfile:."] != null)
                //{
                //    profile = HttpContext.Current.Session[".:LoggedInProfile:."] as UserProfile;
                //}
                //else
                //{
                //    UserRepo dao = new UserRepo();
                //    var user = HttpContext.Current.User;
                //    if (user != null && user.Identity != null && !string.IsNullOrEmpty(user.Identity.Name))
                //    {
                //        profile = dao.GetUser(user.Identity.Name);
                //        HttpContext.Current.Session[".:LoggedInProfile:."] = profile;
                //    }
                //}
                //if (profile != null)
                //{
                //    return string.Format("{0} - ({1})", profile.FullName, profile.IP.ShortName);
                //}
                //else
                    return null;
            }
        }

    }
}
