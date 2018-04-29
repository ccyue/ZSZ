using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    public class AdminLogConfig: EntityTypeConfiguration<AdminLogEntity>
    {
        public AdminLogConfig()
        {
            ToTable("T_AdminLogs");
            HasRequired(p => p.AdminUser).WithMany()
                .HasForeignKey(p => p.AdminUserId).WillCascadeOnDelete(false);
            Property(p => p.Message).IsRequired();
        }
    }
}
