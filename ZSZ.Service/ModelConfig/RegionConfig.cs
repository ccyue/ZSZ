using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class RegionConfig:EntityTypeConfiguration<RegionEntity>
    {
        public RegionConfig()
        {
            ToTable("T_Regions");
            HasRequired(p => p.City).WithMany().HasForeignKey(p => p.CityId).WillCascadeOnDelete(false);
            Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
