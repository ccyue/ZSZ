using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class PermissionConfig:EntityTypeConfiguration<PermissionEntity>
    {
        public PermissionConfig()
        {
            ToTable("T_Permissions");
            Property(p => p.Description).IsOptional().HasMaxLength(1024);
            Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
