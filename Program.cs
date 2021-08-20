using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RedisConsolePOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("40.65.216.220");
            var sub = redis.GetSubscriber();

            var db = redis.GetDatabase();


            sub.Subscribe("perguntas").OnMessage(
            c =>
            {
                if (c.Message.ToString().Contains(":"))
                {
                    var dados = c.Message.ToString().Split(':');
                    var chavePergunta = dados[0];
                    var pergunta = dados[1];

                    var value = Convert.ToInt32(chavePergunta.Substring(1));
                    var resp = value * 2;
                    Console.WriteLine(c.Message);
                    Console.WriteLine(resp);
                    db.HashSet(chavePergunta, "Grupo01", resp);
                }
            });

            Console.ReadLine();
        }
    }
}
