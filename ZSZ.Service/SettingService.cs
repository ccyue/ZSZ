using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class SettingService : ISettingService
    {
        public SettingDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                return cs.GetAll().Select(p => new SettingDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Value = p.Value,
                    CreateDateTime = p.CreateDateTime
                }).ToArray();
            }
        }
        public string GetValue(string name)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(p => p.Name == name);
                if (setting == null)
                {
                    throw new ArgumentException("Name is not exist.");
                }
                else
                {
                    return setting.Value;
                }
            }
        }
        public void SetValue(string name, string value)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(p => p.Name == name);
                if (setting == null)
                {
                    throw new ArgumentException("Name is not exist.");
                }
                else
                {
                    setting.Value = value;
                    dbc.SaveChanges();
                }
            }
        }
        public bool? GetBoolValue(string name)
        {
            var value = GetValue(name);
            if (value == null)
            {
                throw new ArgumentException("Name is not exist.");
            }
            else
            {
                return Convert.ToBoolean(value);
            }
        }

        public int? GetIntValue(string name)
        {
            var value = GetValue(name);
            if (value == null)
            {
                throw new ArgumentException("Name is not exist.");
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }

        public void SetBoolValue(string name, bool value)
        {
            SetValue(name, value.ToString());
        }

        public void setIntValue(string name, int value)
        {
            SetValue(name, value.ToString());
        }
    }
}
