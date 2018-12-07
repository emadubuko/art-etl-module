using Common.CommonEntities;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NDR.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string username { get; set; }

        public string Code { get; set; }
    }

    public class RegisterViewModel
    {       
        public virtual string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public virtual string ConfirmPassword { get; set; }

        [Required]
        public virtual string FullName { get; set; }

        [Phone]
        public virtual string ContactPhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public virtual string ContactEmailAddress { get; set; }

        [Required]
        public virtual string RoleName { get; set; }

        [XmlIgnore]
        public virtual DateTime CreationDate { get; set; }
       
        //[XmlIgnore]
        //public virtual ImplementingPartners IP { get; set; }

        [XmlIgnore]
        [Required]
        public virtual int OrganizationId { get; set; }
        public IList<ImplementingPartners> Organization { get; set; }
        public Dictionary<string, string> Roles { get; set; }
    }

    public class ProfileViewModel : AutomaticViewModel<UserProfile>
    {
        public IList<ImplementingPartners> Organization { get; set; }
        public Dictionary<string, string> Roles { get; set; }
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
                        try
                        {
                            _fieldCollection.Add(Utils.PasCaseConversion(info.Name), info.PropertyType.Name);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    //foreach (var info in typeof(T).GetProperties().Where(x => x.CanWrite && !Attribute.IsDefined(x, typeof(XmlIgnoreAttribute))).ToArray())
                    //{
                    //    _fieldCollection.Add(Utils.PasCaseConversion(info.Name), info.PropertyType.Name);
                    //}
                    return _fieldCollection;
                }
            }
        }
    }
}
