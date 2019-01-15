#region Header: Copyright © 2019, N3XeS LLC.

// --------------------------------------------------------------------------------------------------------------------
//	<copyright file="DictionaryVsHashTableVsListTestHarness.cs" company="N3XeS LLC">
//		C# Dictionary vs Hashtable vs ListDictionary Collection Performance Benchmarks
//		Copyright © 2019, N3XeS LLC ("COMPANY")
//
//		This program is free software: you can redistribute it and/or modify 
//		it under the terms of the GNU Lesser General Public License as published by 
//		the Free Software Foundation, either version 3 of the License, or 
//		(at your option) any later version.
//
//		This program is distributed in the hope that it will be useful, 
//		but WITHOUT ANY WARRANTY; without even the implied warranty of 
//		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the 
//		GNU Lesser General Public License for more details.
//
//		You should have received a copy of the GNU Lesser General Public License 
//		along with this program. If not, see<https://www.gnu.org/licenses/>.
//
//	</copyright>
////-------------------------------------------------------------------------------------------------------------------

#endregion

namespace N3XeS.Dev.CSharp.Benchmarks.Performance.Collections
{
	#region Directives

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Linq;

	using JetBrains.Annotations;

	using BenchmarkDotNet.Attributes;
	using BenchmarkDotNet.Configs;
	using BenchmarkDotNet.Exporters.Csv;

	#endregion

	#region Delegates

	#endregion

	#region Enums

	#endregion

	/// <summary>
	///		The dictionary vs hash table vs list dictionary test harness.
	/// </summary>
	/// <author>
	///		<AuthorName>John Caruthers, Jr.</AuthorName>
	///		<CreationDate>Friday, January 11, 2019  (01/11/2019)</CreationDate>
	///		<CreationTime>10:43:23 AM</CreationTime>
	/// </author>
	/// <history>
	///		<Modification>
	///			<ModifierName></ModifierName>
	///			<ModificationDate></ModificationDate>
	///			<ModificationTime></ModificationTime>
	///			<ModificationDescription></ModificationDescription>
	///		</Modification>
	/// </history>
	[UsedImplicitly]
	public static class DictionaryVsHashTableVsListTestHarness
	{
		#region Constants

		#endregion

		#region Instance/Member/Field Variables

		/// <summary>
		///		The letters use to generate a collection's keys state.
		/// </summary>
		private static readonly Char[] _LettersKey =
			{
				'A',
				'B',
				'C',
				'D',
				'E',
				'F',
				'G',
				'H',
				'I',
				'J',
				'K',
				'L',
				'M',
				'N',
				'O',
				'P',
				'Q',
				'R',
				'S',
				'T',
				'U',
				'V',
				'W',
				'X',
				'Y',
				'Z'
			};

		/// <summary>
		///		The test harnesses' configuration.
		/// </summary>
		private static ManualConfig _configuration;

		/// <summary>
		///		The test <see cref="T:System.Collections.Generic.Dictionary`2"/> state.
		/// </summary>
		private static Dictionary<Object, Object> _dictionary;

		/// <summary>
		///		The test <see cref="T:System.Collections.Specialized.ListDictionary"/> state.
		/// </summary>
		private static ListDictionary _dictionaryList;

		/// <summary>
		///		The test <see cref="T:System.Collections.Hashtable"/> state.
		/// </summary>
		private static Hashtable _hashtable;

		/// <summary>
		///		The enumeration lookup benchmark random <see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> key value state.
		/// </summary>
		private static String _valueKeyLookupRandomEnumerated;

		/// <summary>
		///		The indexed lookup lookup benchmark random <see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> key value state.
		/// </summary>
		private static String _valueKeyLookupRandomIndexed;

		/// <summary>
		///		The add manipulation benchmark random new <see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> key value state.
		/// </summary>
		private static String _valueKeyManipulationRandomAdd;

		/// <summary>
		///		The change manipulation benchmark random existing
		///		<see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> key value state.
		/// </summary>
		private static String _valueKeyManipulationRandomChange;

		/// <summary>
		///		The remove manipulation benchmark random existing
		///		<see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> key value state.
		/// </summary>
		private static String _valueKeyManipulationRandomRemove;

		/// <summary>
		///		The add manipulation benchmark random new <see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> value state.
		/// </summary>
		private static String _valueManipulationRandomAdd;

		/// <summary>
		///		The change manipulation benchmark random existing
		///		<see cref="T:System.Collections.Generic.Dictionary`2"/> and
		///		<see cref="T:System.Collections.Hashtable"/> value state.
		/// </summary>
		private static String _valueManipulationRandomChange;

		#endregion

		#region Constructors

		#endregion

		#region Destructor

		#endregion

		#region Events

		#endregion

		#region Properties/Accessors/Mutators

		/// <summary>
		///		Gets the test harnesses' configuration.
		/// </summary>
		/// <returns>
		///		The test harnesses' configuration.
		/// </returns>
		public static ManualConfig Configuration
		{
			[DebuggerStepThrough]
			get
			{
				if (_configuration != null)
				{
					return _configuration;
				}

				_configuration = ManualConfig.Create(DefaultConfig.Instance);

				_configuration.Add(new CsvExporter(CsvSeparator.CurrentCulture,
												   new BenchmarkDotNet.Reports.SummaryStyle
												   {
													   PrintUnitsInContent = false,
													   PrintUnitsInHeader = true,
													   SizeUnit = BenchmarkDotNet.Columns.SizeUnit.KB,
													   TimeUnit = BenchmarkDotNet.Horology.TimeUnit.Nanosecond
												   }));
				_configuration.Add(new CsvMeasurementsExporter(CsvSeparator.CurrentCulture,
															   new BenchmarkDotNet.Reports.SummaryStyle
															   {
																   PrintUnitsInContent = false,
																   PrintUnitsInHeader = true,
																   SizeUnit = BenchmarkDotNet.Columns.SizeUnit.KB,
																   TimeUnit = BenchmarkDotNet.Horology.TimeUnit.Nanosecond
															   }));

				return _configuration;
			}
		}

		/// <summary>
		///		Gets or sets the collection length parameters.
		/// </summary>
		/// <returns>
		///		The collection length parameters.
		/// </returns>
		public static Int32 ParametersLengthGlobal { get; set; }

		#endregion

		#region Indexers

		#endregion

		#region Event Handlers

		#endregion

		#region Methods/Functions

		/// <summary>
		///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
		/// </summary>
		public static void CleanupIterationGlobal()
		{
			_dictionary = null;
			_dictionaryList = null;
			_hashtable = null;

			_valueKeyLookupRandomEnumerated = null;
			_valueKeyLookupRandomIndexed = null;
			_valueKeyManipulationRandomAdd = null;
			_valueKeyManipulationRandomChange = null;
			_valueKeyManipulationRandomRemove = null;
			_valueManipulationRandomAdd = null;
			_valueManipulationRandomChange = null;
		}

		/// <summary>
		///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
		/// </summary>
		public static void SetupIterationGlobal()
		{
			Int32 numberRecordsToCreate = ParametersLengthGlobal;

			Random rng = new Random();

			Int32[] numbersExcluded = new Int32[3];

			Int32 indexRecordRandomToChange = 
				rng.Next(0, numberRecordsToCreate - 1);

			numbersExcluded[0] = indexRecordRandomToChange;

			String numberRecordRandomToChange =
				(indexRecordRandomToChange + 1).ToString();

			_valueKeyManipulationRandomChange = 
				_LettersKey[rng.Next(0, 25)] + "_Key" + numberRecordRandomToChange;
			_valueManipulationRandomChange = 
				"ChangeValue" + numberRecordRandomToChange;

			Int32 indexRecordRandomToEnumerate = 
				GenerateRandomUniqueNumber(rng, 
										   0, 
										   numberRecordsToCreate - 1, 
										   numbersExcluded);

			numbersExcluded[1] = indexRecordRandomToEnumerate;
			_valueKeyLookupRandomEnumerated = 
				_LettersKey[rng.Next(0, 25)] + "_Key" + (indexRecordRandomToEnumerate + 1);
			Int32 indexRecordRandomToIndex = 
				GenerateRandomUniqueNumber(rng, 
										   0, 
										   numberRecordsToCreate - 1, 
										   numbersExcluded);

			numbersExcluded[2] = indexRecordRandomToIndex;
			_valueKeyLookupRandomIndexed = 
				_LettersKey[rng.Next(0, 25)] + "_Key" + (indexRecordRandomToIndex + 1);
			Int32 indexRecordRandomToRemove = 
				GenerateRandomUniqueNumber(rng, 
										   0, 
										   numberRecordsToCreate - 1, 
										   numbersExcluded);

			_valueKeyManipulationRandomRemove = 
				_LettersKey[rng.Next(0, 25)] + "_Key" + (indexRecordRandomToRemove + 1);

			_dictionary = new Dictionary<Object, Object>();

			for (Int32 indexDictionary = 0; 
				 indexDictionary < numberRecordsToCreate; 
				 indexDictionary++)
			{
				String valueNumberRecordDictionary = (indexDictionary + 1).ToString();
				String valueRecordDictionary = "Value" + valueNumberRecordDictionary;

				if (indexDictionary == indexRecordRandomToChange)
				{
					_dictionary.Add(_valueKeyManipulationRandomChange, valueRecordDictionary);
				}
				else if (indexDictionary == indexRecordRandomToRemove)
				{
					_dictionary.Add(_valueKeyManipulationRandomRemove, valueRecordDictionary);
				}
				else if (indexDictionary == indexRecordRandomToEnumerate)
				{
					_dictionary.Add(_valueKeyLookupRandomEnumerated, valueRecordDictionary);
				}
				else if (indexDictionary == indexRecordRandomToIndex)
				{
					_dictionary.Add(_valueKeyLookupRandomIndexed, valueRecordDictionary);
				}
				else
				{
					_dictionary.Add(_LettersKey[rng.Next(0, 25)] + "_Key" + valueNumberRecordDictionary, 
									valueRecordDictionary);
				}
			}

			_dictionaryList = new ListDictionary();

			for (Int32 indexDictionaryList = 0; 
				 indexDictionaryList < numberRecordsToCreate; 
				 indexDictionaryList++)
			{
				String valueNumberRecordDictionaryList = (indexDictionaryList + 1).ToString();
				String valueRecordDictionaryList = "Value" + valueNumberRecordDictionaryList;

				if (indexDictionaryList == indexRecordRandomToChange)
				{
					_dictionaryList.Add(_valueKeyManipulationRandomChange, valueRecordDictionaryList);
				}
				else if (indexDictionaryList == indexRecordRandomToRemove)
				{
					_dictionaryList.Add(_valueKeyManipulationRandomRemove, valueRecordDictionaryList);
				}
				else if (indexDictionaryList == indexRecordRandomToEnumerate)
				{
					_dictionaryList.Add(_valueKeyLookupRandomEnumerated, valueRecordDictionaryList);
				}
				else if (indexDictionaryList == indexRecordRandomToIndex)
				{
					_dictionaryList.Add(_valueKeyLookupRandomIndexed, valueRecordDictionaryList);
				}
				else
				{
					_dictionaryList.Add(_LettersKey[rng.Next(0, 25)] + "_Key" + valueNumberRecordDictionaryList, 
										valueRecordDictionaryList);
				}
			}

			_hashtable = new Hashtable();

			for (Int32 indexHashtable = 0; indexHashtable < numberRecordsToCreate; indexHashtable++)
			{
				String valueNumberRecordHashtable = (indexHashtable + 1).ToString();
				String valueRecordHashtable = "Value" + valueNumberRecordHashtable;

				if (indexHashtable == indexRecordRandomToChange)
				{
					_hashtable.Add(_valueKeyManipulationRandomChange, valueRecordHashtable);
				}
				else if (indexHashtable == indexRecordRandomToRemove)
				{
					_hashtable.Add(_valueKeyManipulationRandomRemove, valueRecordHashtable);
				}
				else if (indexHashtable == indexRecordRandomToEnumerate)
				{
					_hashtable.Add(_valueKeyLookupRandomEnumerated, valueRecordHashtable);
				}
				else if (indexHashtable == indexRecordRandomToIndex)
				{
					_hashtable.Add(_valueKeyLookupRandomIndexed, valueRecordHashtable);
				}
				else
				{
					_hashtable.Add(_LettersKey[rng.Next(0, 25)] + "_Key" + valueNumberRecordHashtable, 
								   valueRecordHashtable);
				}
			}

			String valueNumberRecordNew = (numberRecordsToCreate + 1).ToString();

			_valueKeyManipulationRandomAdd = 
				_LettersKey[rng.Next(0, 25)] + "_Key" + valueNumberRecordNew;
			_valueManipulationRandomAdd = "Value" + valueNumberRecordNew;
		}

		/// <summary>
		///		Uses <paramref name="rng"/> to generate a random number between the <paramref name="numberMin"/> and
		///		<paramref name="numberMax"/> that does not exist in the <paramref name="numbersExcluded"/>.
		/// </summary>
		/// <param name="rng">
		///		The <see cref="T:System.Random"/> random number generator that is use to generate the random number
		///		between <paramref name="numberMin"/> and <paramref name="numberMax"/> that does not exist in the
		///		<paramref name="numbersExcluded"/>.
		/// </param>
		/// <param name="numberMin">
		///		The <see cref="T:System.Int32"/> number that represents the minimum number that the
		///		<paramref name="rng"/> can generate.
		/// </param>
		/// <param name="numberMax">
		///		The <see cref="T:System.Int32"/> number that represents the maximum number that the
		///		<paramref name="rng"/> can generate.
		/// </param>
		/// <param name="numbersExcluded">
		///		The <see cref="T:System.Int32"/> array that represents the numbers that will be excluded from the
		///		numbers the <paramref name="rng"/> can generate.
		/// </param>
		/// <returns>
		///		A unique random number between the <paramref name="numberMin"/> and <paramref name="numberMax"/> that
		///		does not exist in the <paramref name="numbersExcluded"/>.
		/// </returns>
		private static Int32 GenerateRandomUniqueNumber(Random rng, Int32 numberMin, Int32 numberMax, Int32[] numbersExcluded)
		{
			Int32 result;

			List<Int32> listNumbersExcluded = numbersExcluded.ToList();

			do
			{
				result = rng.Next(numberMin, numberMax);
			}
			while (listNumbersExcluded.Contains(result));

			return result;
		}

		#endregion

		#region Operators

		#endregion

		#region Nested Types

		/// <summary>
		///		The dictionary vs hash table vs list dictionary add benchmarks.
		/// </summary>
		[EvaluateOverhead,
		 GcForce,
		 StopOnFirstError,
		 UsedImplicitly]
		public class Add
		{
			/// <summary>
			///		Gets or sets the collection length parameters.
			/// </summary>
			/// <returns>
			///		The collection length parameters.
			/// </returns>
			[Params(5, 6, 7, 8, 9, 10, 50, 500, 5000, 10000, 50000, 100000),
			 UsedImplicitly]
			public Int32 ParametersLength
			{
				get { return ParametersLengthGlobal; }
				set { ParametersLengthGlobal = value; }
			}

			/// <summary>
			///		The add to list <see cref="T:System.Collections.Specialized.ListDictionary"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkDictionaryListManipulationAdd() => 
				_dictionaryList.Add(_valueKeyManipulationRandomAdd, 
									_valueManipulationRandomAdd);

			/// <summary>
			///		The add to <see cref="T:System.Collections.Generic.Dictionary`2"/> manipulation benchmark.
			/// </summary>
			[Benchmark(Baseline = true)]
			public void BenchmarkDictionaryManipulationAdd() => 
				_dictionary.Add(_valueKeyManipulationRandomAdd, 
								_valueManipulationRandomAdd);

			/// <summary>
			///		The add to <see cref="T:System.Collections.Hashtable"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkHashTableManipulationAdd() => 
				_hashtable.Add(_valueKeyManipulationRandomAdd, 
							   _valueManipulationRandomAdd);

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
			/// </summary>
			[IterationCleanup]
			public void CleanupIteration()
			{
				CleanupIterationGlobal();
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
			/// </summary>
			[IterationSetup]
			public void SetupIteration()
			{
				SetupIterationGlobal();
			}
		}

		/// <summary>
		///		The dictionary vs hash table vs list dictionary change benchmarks.
		/// </summary>
		[EvaluateOverhead,
		 GcForce,
		 StopOnFirstError,
		 UsedImplicitly]
		public class Change
		{
			/// <summary>
			///		Gets or sets the collection length parameters.
			/// </summary>
			/// <returns>
			///		The collection length parameters.
			/// </returns>
			[Params(5, 6, 7, 8, 9, 10, 50, 500, 5000, 10000, 50000, 100000),
			 UsedImplicitly]
			public Int32 ParametersLength
			{
				get { return ParametersLengthGlobal; }
				set { ParametersLengthGlobal = value; }
			}

			/// <summary>
			///		The change list <see cref="T:System.Collections.Specialized.ListDictionary"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkDictionaryListManipulationChange() => 
				_dictionaryList[_valueKeyManipulationRandomChange] = 
					_valueManipulationRandomChange;

			/// <summary>
			///		The change <see cref="T:System.Collections.Generic.Dictionary`2"/> manipulation benchmark.
			/// </summary>
			[Benchmark(Baseline = true)]
			public void BenchmarkDictionaryManipulationChange() => 
				_dictionary[_valueKeyManipulationRandomChange] = 
					_valueManipulationRandomChange;

			/// <summary>
			///		The change <see cref="T:System.Collections.Hashtable"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkHashTableManipulationChange() => 
				_hashtable[_valueKeyManipulationRandomChange] = 
					_valueManipulationRandomChange;

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
			/// </summary>
			[IterationCleanup]
			public void CleanupIteration()
			{
				CleanupIterationGlobal();
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
			/// </summary>
			[IterationSetup]
			public void SetupIteration()
			{
				SetupIterationGlobal();
			}
		}

		/// <summary>
		///		The dictionary vs hash table vs list dictionary enumerate benchmarks.
		/// </summary>
		[EvaluateOverhead,
		 GcForce,
		 StopOnFirstError,
		 UsedImplicitly]
		public class Enumerate
		{
			/// <summary>
			///		Gets or sets the collection length parameters.
			/// </summary>
			/// <returns>
			///		The collection length parameters.
			/// </returns>
			[Params(5, 6, 7, 8, 9, 10, 50, 500, 5000, 10000, 50000, 100000),
			 UsedImplicitly]
			public Int32 ParametersLength
			{
				get { return ParametersLengthGlobal; }
				set { ParametersLengthGlobal = value; }
			}

			/// <summary>
			///		The enumerate <see cref="T:System.Collections.Specialized.ListDictionary"/> lookup benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkDictionaryListLookupEnumerate()
			{
				foreach (DictionaryEntry entryDictionary in _dictionaryList)
				{
					if ((String)entryDictionary.Key == _valueKeyLookupRandomEnumerated)
					{
						break;
					}
				}
			}

			/// <summary>
			///		The enumerate <see cref="T:System.Collections.Generic.Dictionary`2"/> lookup benchmark.
			/// </summary>
			[Benchmark(Baseline = true)]
			public void BenchmarkDictionaryLookupEnumerate()
			{
				foreach (KeyValuePair<Object, Object> keyValuePairDictionary in _dictionary)
				{
					if ((String)keyValuePairDictionary.Key == _valueKeyLookupRandomEnumerated)
					{
						break;
					}
				}
			}

			/// <summary>
			///		The enumerate <see cref="T:System.Collections.Hashtable"/> lookup benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkHashTableLookupEnumerate()
			{
				foreach (DictionaryEntry entryDictionary in _hashtable)
				{
					if ((String)entryDictionary.Key == _valueKeyLookupRandomEnumerated)
					{
						break;
					}
				}
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
			/// </summary>
			[IterationCleanup]
			public void CleanupIteration()
			{
				CleanupIterationGlobal();
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
			/// </summary>
			[IterationSetup]
			public void SetupIteration()
			{
				SetupIterationGlobal();
			}
		}

		/// <summary>
		///		The dictionary vs hash table vs list dictionary index benchmarks.
		/// </summary>
		[EvaluateOverhead,
		 GcForce,
		 StopOnFirstError,
		 UsedImplicitly]
		public class Index
		{
			/// <summary>
			///		Gets or sets the collection length parameters.
			/// </summary>
			/// <returns>
			///		The collection length parameters.
			/// </returns>
			[Params(5, 6, 7, 8, 9, 10, 50, 500, 5000, 10000, 50000, 100000),
			 UsedImplicitly]
			public Int32 ParametersLength
			{
				get { return ParametersLengthGlobal; }
				set { ParametersLengthGlobal = value; }
			}

			/// <summary>
			///		The indexed <see cref="T:System.Collections.Specialized.ListDictionary"/> lookup benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkDictionaryListLookupIndexed()
			{
				// ReSharper disable once UnusedVariable
				String test = (String)_dictionaryList[_valueKeyLookupRandomIndexed];
			}

			/// <summary>
			///		The indexed <see cref="T:System.Collections.Generic.Dictionary`2"/> lookup benchmark.
			/// </summary>
			[Benchmark(Baseline = true)]
			public void BenchmarkDictionaryLookupIndexed()
			{
				// ReSharper disable once UnusedVariable
				String test = (String)_dictionary[_valueKeyLookupRandomIndexed];
			}

			/// <summary>
			///		The indexed <see cref="T:System.Collections.Hashtable"/> lookup benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkHashTableLookupIndexed()
			{
				// ReSharper disable once UnusedVariable
				String test = (String)_hashtable[_valueKeyLookupRandomIndexed];
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
			/// </summary>
			[IterationCleanup]
			public void CleanupIteration()
			{
				CleanupIterationGlobal();
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
			/// </summary>
			[IterationSetup]
			public void SetupIteration()
			{
				SetupIterationGlobal();
			}
		}

		/// <summary>
		///		The dictionary vs hash table vs list dictionary remove benchmarks.
		/// </summary>
		[EvaluateOverhead,
		 GcForce,
		 StopOnFirstError,
		 UsedImplicitly]
		public class Remove
		{
			/// <summary>
			///		Gets or sets the collection length parameters.
			/// </summary>
			/// <returns>
			///		The collection length parameters.
			/// </returns>
			[Params(5, 6, 7, 8, 9, 10, 50, 500, 5000, 10000, 50000, 100000),
			 UsedImplicitly]
			public Int32 ParametersLength
			{
				get { return ParametersLengthGlobal; }
				set { ParametersLengthGlobal = value; }
			}

			/// <summary>
			///		The remove from list <see cref="T:System.Collections.Specialized.ListDictionary"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkDictionaryListManipulationRemove() => 
				_dictionaryList.Remove(_valueKeyManipulationRandomRemove);

			/// <summary>
			///		The remove from <see cref="T:System.Collections.Generic.Dictionary`2"/> manipulation benchmark.
			/// </summary>
			[Benchmark(Baseline = true)]
			public void BenchmarkDictionaryManipulationRemove() => 
				_dictionary.Remove(_valueKeyManipulationRandomRemove);

			/// <summary>
			///		The remove from <see cref="T:System.Collections.Hashtable"/> manipulation benchmark.
			/// </summary>
			[Benchmark]
			public void BenchmarkHashTableManipulationRemove() => 
				_hashtable.Remove(_valueKeyManipulationRandomRemove);

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness cleanup.
			/// </summary>
			[IterationCleanup]
			public void CleanupIteration()
			{
				CleanupIterationGlobal();
			}

			/// <summary>
			///		The dictionary vs hash table vs list dictionary benchmark test harness setup.
			/// </summary>
			[IterationSetup]
			public void SetupIteration()
			{
				SetupIterationGlobal();
			}
		}

		#endregion
	}
}
