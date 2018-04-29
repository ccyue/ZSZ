using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class AdminUserConfig: EntityTypeConfiguration<AdminUserEntity>
    {
        public AdminUserConfig()
        {
            ToTable("T_AdminUsers");

            HasOptional(p => p.City).WithMany().HasForeignKey(p => p.CityId).WillCascadeOnDelete(false);

            HasMany(p => p.Roles).WithMany(p => p.AdminUsers).Map(p => p.ToTable("T_AdminUserRoles")
                .MapLeftKey("AdminUserId")
                .MapRightKey("RoleId"));

            Property(p => p.Name).HasMaxLength(50).IsRequired();
            Property(p => p.Email).HasMaxLength(30).IsRequired().IsUnicode(false);
            Property(p => p.PhoneNum).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(p => p.PasswordSalt).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(p => p.PasswordHash).HasMaxLength(250).IsRequired().IsUnicode(false);

        }
    }
}
