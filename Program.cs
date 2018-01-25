using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace ClientHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestNumber = long.Parse(args[1]);
            var maxParallel = int.Parse(args[2]);
            var stats = new ConcurrentBag<long>();

            var sw = new Stopwatch();
            sw.Start();
            Parallel.For(0, requestNumber, new ParallelOptions() { MaxDegreeOfParallelism = maxParallel },
                (i) => {SendRequest(args[0], stats);});
            sw.Stop();

            Console.WriteLine($"Total time: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Nb request/s: {(double)requestNumber/sw.ElapsedMilliseconds*1000.0}");
            Console.WriteLine($"Avg Latency: {stats.Average()}");
            Console.WriteLine($"Min Latency: {stats.Min()}");
            Console.WriteLine($"Max Latency: {stats.Max()}");
        }

        private static void SendRequest(string url, ConcurrentBag<long> stats)
        {
            var sw = new Stopwatch();
            sw.Start();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var stringTask = client.GetStringAsync(url);

            stringTask.Wait();
            sw.Stop();
            stats.Add(sw.ElapsedMilliseconds);
        }
    }
}
