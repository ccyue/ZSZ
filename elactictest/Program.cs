using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elactictest
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticConnection client = new ElasticConnection("127.0.0.1", 9200);
            SearchCommand scmd = new SearchCommand("verycd", "verycds");
            var query = new QueryBuilder<VerycdItem>().Query(
                p => p.Bool(
                    q => q.Must(
                        t => t.QueryString(
                            t1 => t1.DefaultField("title").Query("爱")
                            )
                    )
                )
            ).Build();
            var sresult = client.Post(scmd, query);
            var seri = new JsonNetSerializer();
            var list = seri.ToSearchResult<VerycdItem>(sresult);
            foreach (var pp in list.Documents)
            {
                Console.WriteLine(pp.title);
            }
            Console.ReadLine();
        }


        static void Main3(string[] args)
        {
            ElasticConnection client = new ElasticConnection("127.0.0.1", 9200);
            using (SQLiteConnection conn =new SQLiteConnection(@"Data Source = D:\RuPeng课件\07. 掌上租课件\电驴数据库（全部电影）\verycd.sqlite3.db"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from verycd";
                    using(var reader = cmd.ExecuteReader())
                    {
                        VerycdItem verycd = new VerycdItem();
                        while (reader.Read())
                        {
                            verycd.verycdid = reader.GetInt64(reader.GetOrdinal("verycdid"));
                            verycd.title = reader.GetString(reader.GetOrdinal("title"));
                            verycd.status = reader.GetString(reader.GetOrdinal("status"));
                            verycd.brief = reader.GetString(reader.GetOrdinal("brief"));
                            verycd.pubtime = reader.GetString(reader.GetOrdinal("pubtime"));
                            verycd.updtime = reader.GetString(reader.GetOrdinal("updtime"));
                            verycd.category1 = reader.GetString(reader.GetOrdinal("category1"));
                            verycd.category2 = reader.GetString(reader.GetOrdinal("category2"));
                            verycd.ed2k = reader.GetString(reader.GetOrdinal("ed2k"));
                            verycd.content = reader.GetString(reader.GetOrdinal("content"));
                            verycd.related = reader.GetString(reader.GetOrdinal("related"));
                            var serializer = new JsonNetSerializer();
                            IndexCommand indexcmd = new IndexCommand("verycd", "verycds", verycd.verycdid.ToString());
                            OperationResult result = client.Put(indexcmd, serializer.Serialize(verycd));
                            var indexresult = serializer.ToIndexResult(result.Result);
                            if (indexresult.created)
                            {
                                Console.WriteLine(verycd.verycdid.ToString() + ":创建了");
                            }
                            else
                            {
                                Console.WriteLine("没创建" + indexresult.error);
                            }
                        }
                                             
                    }

                }
            }

            Console.ReadKey();
        }

        static void Main2(string[] args)
        {
            //Person p1 = new Person() { Id = 4, Name = "crd", Age = 22 };

            //ElasticConnection client = new ElasticConnection("127.0.0.1", 9200);
            //var serializer = new JsonNetSerializer();
            //IndexCommand cmd = new IndexCommand("zsz", "person", p1.Id.ToString());
            //OperationResult result = client.Put(cmd, serializer.Serialize(p1));
            //var indexresult = serializer.ToIndexResult(result.Result);
            //if (indexresult.created)
            //{
            //    Console.WriteLine("创建了");
            //}
            //else
            //{
            //    Console.WriteLine("没创建" + indexresult.error);
            //}
            //Console.ReadKey();

            ElasticConnection client = new ElasticConnection("127.0.0.1", 9200);
            SearchCommand scmd = new SearchCommand("zsz", "person");
            var query = new QueryBuilder<Person>().Query(
                p => p.Bool(
                    q => q.Must(
                        t => t.QueryString(
                            t1 => t1.DefaultField("Name").Query("crd")
                            )
                    )
                )
            )
            ////.From(0).Size(10)
            ////.Sort(c => c.Field("Age", SortDirection.desc))
            ////.Highlight(h => h.PreTags("<b>").PostTags("</b>")
            ////.Fields(
            ////    f => f.FieldName("Name").Order(HighlightOrder.score)
            ////    )
            ////)
            .Build();
            var sresult = client.Post(scmd, query);
            var seri = new JsonNetSerializer();
            var list = seri.ToSearchResult<Person>(sresult);
            foreach (var pp in list.Documents)
            {
                Console.WriteLine(pp.ToString());
            }

            Console.ReadKey();

        }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("Id:{0},Name:{1},Age:{2}\r\n", Id, Name, Age);
        }
    }
}
