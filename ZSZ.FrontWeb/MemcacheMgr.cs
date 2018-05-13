using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class MemcacheMgr
    {
        private MemcachedClient client;
        //单例模式
        public static MemcacheMgr Instance { get; private set; } = new MemcacheMgr();

        private MemcacheMgr()
        {
            var settingService = DependencyResolver.Current.GetService<ISettingService>();
            var servers = settingService.GetValue("MemcacheServers").Split(';');

            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            foreach(var s in servers)
            {
                config.Servers.Add(new IPEndPoint(IPAddress.Parse(s), 11211));
            }
            
            config.Protocol = MemcachedProtocol.Binary;
            client = new MemcachedClient(config);
        }
        public void SetValue(string key, object value,TimeSpan span)
        {
            if(!value.GetType().IsSerializable)
            {
                throw new ArgumentException("value必须是可序列化的");
            }
            client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value, span);
        }
        public T GetValue<T>(string key)
        {
            return client.Get<T>(key);
        }
    }
}