using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class HouseConfig:EntityTypeConfiguration<HouseEntity>
    {
        public HouseConfig()
        {
            ToTable("T_Houses");
            HasRequired(p => p.Community).WithMany().HasForeignKey(p => p.CommunityId).WillCascadeOnDelete(false);
            HasRequired(p => p.RoomType).WithMany().HasForeignKey(p => p.RoomTypeId).WillCascadeOnDelete(false);
            HasRequired(p => p.Status).WithMany().HasForeignKey(p => p.StatusId).WillCascadeOnDelete(false);
            HasRequired(p => p.DecorateStatus).WithMany().HasForeignKey(p => p.DecorateStatusId).WillCascadeOnDelete(false);
            HasRequired(p => p.Type).WithMany().HasForeignKey(p => p.TypeId).WillCascadeOnDelete(false);
            Property(h => h.Address).IsRequired().HasMaxLength(128);
            Property(h => h.Description).IsOptional();
            Property(h => h.Direction).IsRequired().HasMaxLength(20);
            Property(h => h.OwnerName).IsRequired().HasMaxLength(20);
            Property(h => h.OwnerPhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
        }
    }
}
