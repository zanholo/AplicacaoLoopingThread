// See https://aka.ms/new-console-template for more information
using AplicacaoLoopingThread.Entidade;
using System.Runtime.CompilerServices;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Preenchendo dados Aleatorios");

            //
            List<Cliente> listCliente = new List<Cliente>();

            Cliente cliente = new Cliente();

            for (int i = 0; i < 30000; i++)
            {
                cliente = new Cliente();
                Random rand = new Random();
                cliente.id = rand.Next();
                cliente.Nome = rand.NextDouble().ToString();

                listCliente.Add(cliente);
            }

            List<Task> tasks = new List<Task>();
            Task t;
            int threadCount = 0;
            foreach (var cli in listCliente)
            {

                tasks = new List<Task>();
                threadCount++;
                tasks.Add(Task.Run(() =>
                {                    
                        Consumir();                    
                }));


                if(threadCount == 25)
                {
                    // Wait for all to finish
                    t = Task.WhenAll(tasks);
                    try
                    {
                        t.Wait();
                    }
                    catch { }
                    threadCount = 0;
                    Console.WriteLine("Executou 25 requisições");
                }
            }
            if(threadCount > 0)
            {
                t = Task.WhenAll(tasks);
                try
                {
                    t.Wait();
                }
                catch { }
            }            
            Console.WriteLine("Finalizou a aplicação");
            Console.ReadKey();

        }

        private static void Consumir()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7291/api/Conta/ListaConta");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));

            System.Net.Http.HttpResponseMessage response = client.GetAsync("").Result;
        }


    }
}