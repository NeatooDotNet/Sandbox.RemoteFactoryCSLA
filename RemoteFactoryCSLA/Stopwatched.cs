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
            benchmarks.CSLABusinessBase();
            var totalCount = CSLABusinessBase.TotalCount;
            stopwatch.Stop();

            //Debug.Assert(cslaBB.IsValid);
            //cslaBB.ChildA.ChildA.ChildA.Description = null;
            //Debug.Assert(!cslaBB.IsValid);

            Console.WriteLine($"CSLA Total Count: {CSLABusinessBase.TotalCount}");
            Console.WriteLine($"CSLA Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            benchmarks.NeatooEditBase();
            totalCount = NeatooEditBase.TotalCount;
            stopwatch.Stop();

            Console.WriteLine($"NeatooEditBase Total Count: {NeatooEditBase.TotalCount}");
            Console.WriteLine($"NeatooEditBase Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            benchmarks.DIOnly();
            totalCount = DIOnly.TotalCount;
            stopwatch.Stop();

            Console.WriteLine($"DIOnly Total Count: {totalCount}");
            Console.WriteLine($"DIOnly Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            benchmarks.ConstructorOnly();
            totalCount = Constructor.TotalCount;
            stopwatch.Stop();

            Console.WriteLine($"ConstructorOnly Total Count: {totalCount}");
            Console.WriteLine($"ConstructorOnly Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            benchmarks.ActivatorCreateInstance();
            totalCount = ActivatorCreateInstance.TotalCount;
            stopwatch.Stop();

            Console.WriteLine($"ActivatorCreateInstance Total Count: {totalCount}");
            Console.WriteLine($"ActivatorCreateInstance Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            Console.WriteLine($"Press any key to exit...");
            Console.ReadLine();
        }

    }
}

