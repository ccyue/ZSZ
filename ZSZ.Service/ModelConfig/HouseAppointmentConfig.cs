using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class HouseAppointmentConfig:EntityTypeConfiguration<HouseAppointmentEntity>
    {
        public HouseAppointmentConfig()
        {
            ToTable("T_HouseAppointments");
            HasOptional(p => p.User).WithMany().HasForeignKey(p => p.UserId).WillCascadeOnDelete(false);
            HasRequired(p => p.House).WithMany().HasForeignKey(p => p.HouseId).WillCascadeOnDelete(false);
            HasOptional(p => p.FollowAdminUser).WithMany().HasForeignKey(p => p.FollowAdminUserId).WillCascadeOnDelete(false);
            Property(h => h.Name).IsRequired().HasMaxLength(20);
            Property(h => h.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
            Property(h => h.Status).IsRequired().HasMaxLength(20);
            Property(h => h.RowVersion).IsRequired().IsRowVersion();  //乐观锁
        }
    }
}
