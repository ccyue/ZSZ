using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class CommunityConfig:EntityTypeConfiguration<CommunityEntity>
    {
        public CommunityConfig()
        {
            ToTable("T_Communities");
            HasRequired(p => p.Region).WithMany().HasForeignKey(p => p.RegionId).WillCascadeOnDelete(false);
            Property(p => p.Name).HasMaxLength(20).IsRequired();
            Property(p => p.Location).HasMaxLength(1024).IsRequired();
        }
    }
}
