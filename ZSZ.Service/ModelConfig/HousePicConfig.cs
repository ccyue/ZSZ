using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class HousePicConfig:EntityTypeConfiguration<HousePicEntity>
    {
        public HousePicConfig()
        {
            ToTable("T_HousePics");
            HasRequired(p => p.House).WithMany().HasForeignKey(p => p.HouseId).WillCascadeOnDelete(false);
            Property(p => p.ThumbUrl).IsRequired().HasMaxLength(1024);
            Property(p => p.Url).IsRequired().HasMaxLength(1024);
        }
    }
}
