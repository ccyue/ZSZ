using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class AttachmentConfig:EntityTypeConfiguration<AttachmentEntity>
    {
        public AttachmentConfig()
        {
            ToTable("T_Attachments");
            HasMany(p => p.Houses).WithMany(p => p.Attachments).Map(p => p.ToTable("T_HouseAttachments")
              .MapLeftKey("AttachmentId").MapRightKey("HouseId"));
            Property(p => p.Name).HasMaxLength(50).IsRequired();
            Property(p => p.IconName).HasMaxLength(50).IsRequired();
        }
    }
}
