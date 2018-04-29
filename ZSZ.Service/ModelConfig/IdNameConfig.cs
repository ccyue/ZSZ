using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class IdNameConfig:EntityTypeConfiguration<IdNameEntity>
    {
        public IdNameConfig()
        {
            ToTable("T_IdNames");
            Property(p => p.Name).HasMaxLength(1024).IsRequired();
            Property(p => p.TypeName).HasMaxLength(1024).IsRequired();
        }
    }
}
