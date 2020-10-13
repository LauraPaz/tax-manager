using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace tax_manager_consumer
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:59678/municipalities/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("HOLA");

            try
            {
                HttpResponseMessage response = await client.GetAsync("1");
                Console.WriteLine(response.StatusCode);

                response = await client.GetAsync("load-file");
                Console.WriteLine(response.StatusCode);

                response = await client.GetAsync("1");
                Console.WriteLine(response.StatusCode);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
