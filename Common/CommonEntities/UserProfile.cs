using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.CommonEntities
{
    public class UserProfile : IdentityUser<int>
    {
        [XmlIgnore]
        public virtual int Id { get; set; }

        [Required]
        public virtual string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Required]
        public virtual string FullName { get; set; }
               
        [Phone]
        public virtual string ContactPhoneNumber { get; set; }
        //[Required]
        //[EmailAddress]
        //public virtual string ContactEmailAddress { get; set; }

        [Required]
        public virtual string RoleName { get; set; }

        [XmlIgnore]
        public virtual DateTime CreationDate { get; set; }

        [XmlIgnore]
        public virtual ProfileStatus Status { get; set; }
         
        [XmlIgnore]
        public virtual ImplementingPartners IP { get; set; }

        [XmlIgnore]
        [Required]
        public virtual int OrganizationId { get; set; }

        [XmlIgnore]
        public virtual Role Role { get;  set; } 

    }

    public class Role : IdentityRole<int>
    {        
        public new virtual string Name { get; set; }
        public new virtual string NormalizedName { get; set; }
        public virtual IList<UserProfile> Users { get; set; }
         
    }


    public enum ProfileStatus
    {
        Disabled, Enabled
    }
        
    public class UserProfileMap : ClassMap<UserProfile>
    {
        public UserProfileMap()
        {
            Table("admin_userprofile");
            Id(x => x.Id);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.FullName);
            Map(x => x.ContactPhoneNumber);
            Map(x => x.Email);
            Map(x => x.RoleName);
            Map(x => x.CreationDate);
            Map(x => x.Status);
            References(x => x.IP).Column("IP");
            References(x => x.Role).Column("RoleId");
        }
    }

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("admin_role");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.NormalizedName);
            HasMany(x => x.Users).Inverse().Cascade.None()
               .KeyColumns.Add("RoleId", mapping => mapping.Name("RoleId"));

           // HasMany(x => x.Users).Inverse().KeyColumn("Role");
        }
    }
}