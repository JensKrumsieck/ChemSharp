using BenchmarkDotNet.Running;
using ChemSharp.Benchmarks;

var summary = BenchmarkRunner.Run(typeof(Mol2Benchmarks));
