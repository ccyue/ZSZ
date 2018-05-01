using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class RoleConfig:EntityTypeConfiguration<RoleEntity>
    {
        public RoleConfig()
        {
            ToTable("T_Roles");
            HasMany(p => p.Permissions).WithMany(p=>p.Roles)
                .Map(p => p.ToTable("T_RolePermissions").MapLeftKey("RoleId").MapRightKey("PermissionId"));
            Property(p => p.Name).IsRequired().HasMaxLength(50);

        }
    }
}
