using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace memcachedtest
{
    class Program
    {
        static void Main(string[] args)
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.Servers.Add(new IPEndPoint(IPAddress.Loopback, 11211));
            config.Protocol = MemcachedProtocol.Binary;
            MemcachedClient client = new MemcachedClient(config);
            var p = new Person { Id = 3, Name = "yy" };
            client.Store(Enyim.Caching.Memcached.StoreMode.Set, "p", p,DateTime.Now.AddSeconds(3));//还可以指定第四个 参数指定数据的过期时间。 Person p1 = client.Get<Person>("p"); 
            var p1 = client.Get<Person>("p");
            Console.WriteLine(p1.Name);
            Thread.Sleep(2000);
            var p2 = client.Get<Person>("p");
            Console.WriteLine(p2.Name);
            Thread.Sleep(2000);
            var p3 = client.Get<Person>("p");
            Console.WriteLine(p3.Name);
            Console.ReadKey();
        }
    }
}
