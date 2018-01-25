using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace ClientHttp
{
    class Program
    {
        static void Main(string[] args)
        {
                        
        }

        private async void SendRequest(string url, ConcurrentBag<long> stats)
        {
            var sw = new Stopwatch();
            sw.Start();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var stringTask = client.GetStringAsync(url);

            var msg = await stringTask;
            sw.Stop();
            stats.Add(sw.ElapsedMilliseconds);
        }
    }
}
