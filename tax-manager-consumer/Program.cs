using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.Json;
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

            Console.WriteLine("CONSUMER APP\n");
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                Console.WriteLine($"\n------------ 1. Load data from file ------------------------------");
                response = await client.GetAsync("load-file");
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<List<Municipality>>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 2. Manually insert new municipality -----------------");
                response = await client.PostAsJsonAsync("", new CreateMunicipalityRequest("Aarhus", new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>()));
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<Municipality>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 3. Schedule yearly tax ------------------------------");
                response = await client.PutAsJsonAsync("2/schedule-tax", new ScheduleTaxRequest('y', 0.2f, DateTime.Parse("2020-01-01"), DateTime.Parse("2020-12-31")));
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<Municipality>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 4. Manually insert new municipality with taxes ------");
                List<Tax> taxList = new List<Tax>();
                taxList.Add(new Tax(0.1f, DateTime.Parse("2020-10-19"), DateTime.Parse("2020-10-25")));
                response = await client.PostAsJsonAsync("", new CreateMunicipalityRequest("Odense", new List<Tax>(), new List<Tax>(), taxList, new List<Tax>()));
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<Municipality>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 5. Get tax for a given date and municipality --------");
                response = await client.GetAsync("Copenhagen/2016-07-10");
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<float>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 6. Get all municipality taxes -----------------------");
                response = await client.GetAsync("");
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<List<Municipality>>().Result, Formatting.Indented));

                Console.WriteLine($"\n------------ 7. Update municipality ------------------------------");
                response = await client.PutAsJsonAsync("1", new UpdateMunicipalityRequest("København", null, null, taxList, new List<Tax>()));
                Console.WriteLine($"Status: {response.StatusCode}\nResponse:");
                Console.WriteLine(JsonConvert.SerializeObject(response.Content.ReadAsAsync<Municipality>().Result, Formatting.Indented));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
