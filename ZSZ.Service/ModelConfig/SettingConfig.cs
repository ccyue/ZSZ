using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class SettingConfig:EntityTypeConfiguration<SettingEntity>
    {
        public SettingConfig()
        {
            ToTable("T_Settings");
            Property(p => p.Name).IsRequired().HasMaxLength(1024);
            Property(p => p.Value).IsRequired();
        }
    }
}
