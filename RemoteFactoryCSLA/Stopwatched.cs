using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFactoryCSLA
{
    public class Stopwatched
    {

        public Stopwatched()
        {
            var benchmarks = new BusinessObjectBenchmarks();
            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            var totalCount = benchmarks.CSLABusinessBase();
            stopwatch.Stop();

            //Debug.Assert(cslaBB.IsValid);
            //cslaBB.ChildA.ChildA.ChildA.Description = null;
            //Debug.Assert(!cslaBB.IsValid);

            Console.WriteLine($"CSLA Total Count: {CSLABusinessBase.TotalCount}");
            Console.WriteLine($"CSLA Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            totalCount = benchmarks.RemoteFactoryCSLABusinessBase();
            stopwatch.Stop();

            Console.WriteLine($"RemoteFactory Total Count: {RemoteFactoryCSLABusinessBase.TotalCount}");
            Console.WriteLine($"RemoteFactory Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            totalCount = benchmarks.NeatooEditBase();
            stopwatch.Stop();

            Console.WriteLine($"NeatooEditBase Total Count: {NeatooEditBase.TotalCount}");
            Console.WriteLine($"NeatooEditBase Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            totalCount = benchmarks.DIOnly();
            stopwatch.Stop();

            Console.WriteLine($"DIOnly Total Count: {totalCount}");
            Console.WriteLine($"DIOnly Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            totalCount = benchmarks.ConstructorOnly();
            stopwatch.Stop();

            Console.WriteLine($"ConstructorOnly Total Count: {totalCount}");
            Console.WriteLine($"ConstructorOnly Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            totalCount = benchmarks.ActivatorCreateInstance();
            stopwatch.Stop();

            Console.WriteLine($"ActivatorCreateInstance Total Count: {totalCount}");
            Console.WriteLine($"ActivatorCreateInstance Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            Console.WriteLine($"Press any key to exit...");
            Console.ReadLine();
        }

    }
}

