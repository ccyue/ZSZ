using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface ISettingService:IServiceSupport
    {
        void SetValue(string name, string value);
        string GetValue(string name);
        void setIntValue(string name, int value);
        int? GetIntValue(string name);
        void SetBoolValue(string name, bool value);
        bool? GetBoolValue(string name);
        SettingDTO[] GetAll();
    }
}
