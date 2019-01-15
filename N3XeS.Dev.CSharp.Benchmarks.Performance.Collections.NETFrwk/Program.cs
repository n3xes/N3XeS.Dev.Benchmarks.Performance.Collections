using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace N3XeS.Dev.CSharp.Benchmarks.Performance.Collections.NETFrwk
{
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules",
					 "SA1402:FileMayOnlyContainASingleClass",
					 Justification = "Reviewed. Suppression is OK here."),
	 UsedImplicitly]
	internal class Program
	{
		private static void Main()
		{
			ManualConfig configuration = DictionaryVsHashTableVsListTestHarness.Configuration;

			configuration.Add(new Job
							  {
								  Environment = { Runtime = Runtime.Clr }
							  });

			BenchmarkRunner.Run<DictionaryVsHashTableVsListTestHarness.Add>(configuration);
			BenchmarkRunner.Run<DictionaryVsHashTableVsListTestHarness.Change>(configuration);
			BenchmarkRunner.Run<DictionaryVsHashTableVsListTestHarness.Enumerate>(configuration);
			BenchmarkRunner.Run<DictionaryVsHashTableVsListTestHarness.Index>(configuration);
			BenchmarkRunner.Run<DictionaryVsHashTableVsListTestHarness.Remove>(configuration);
		}
	}
}
