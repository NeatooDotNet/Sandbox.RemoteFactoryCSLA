using BenchmarkDotNet.Running;
using RemoteFactoryCSLA;
using System.Diagnostics;
// See https://aka.ms/new-console-template for more information

var summary = BenchmarkRunner.Run<BusinessObjectBenchmarks>();

//var stopwatched = new Stopwatched();