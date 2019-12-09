using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day7: DayCodeBase
	{
		public override string Problem1()
		{
			var permutations = GetIntPermutations("01234");
			var results = permutations.Select(p => new { phasePattern = p, outputVal = RunProgram(p) })
				.ToList();
			var max = results.Max(r => r.outputVal);
			var result = results.First(r => r.outputVal == max);
			return string.Join("", result.outputVal);
		}

		public override string Problem2()
		{
			var permutations = GetIntPermutations("56789");
			var results = permutations.Select(p => new { phasePattern = p, outputVal = RunProgram(p) })
				.ToList();
			var max = results.Max(r => r.outputVal);
			var result = results.First(r => r.outputVal == max);
			return string.Join("", result.outputVal);
		}

		private int RunProgram(List<int> phases)
		{
			var streams = phases.Select(p =>
			{
				var toReturn = new InputOutputStream<int>();
				toReturn.Write(p);
				return toReturn;
			}).ToList();
			streams[0].Write(0);
			var tasks = new List<Task<int>>();
			for (var i = 0; i < phases.Count; ++i)
			{
				var codes = GetData(0, ",").Select(int.Parse).ToList();
				tasks.Add(RunProgram(codes, streams[i == 0 ? phases.Count - 1 : i - 1], streams[i]));
			}

			var result = tasks.Last().Result;
			return streams[0].Get().Result;
		}

		private static List<List<int>> GetIntPermutations(string options)
		{
			return GetPermutations(options)
				.Select(s => s.ToCharArray().Select(c => int.Parse(c.ToString())).ToList())
				.ToList();
		}

		private static IEnumerable<string> GetPermutations(string s)
		{
			if (s.Length > 1)
				return from ch in s
					from permutation in GetPermutations(s.Remove(s.IndexOf(ch), 1))
					select $"{ch}{permutation}";
			else
				return new [] { s };
		}

		private class InputOutputStream<T>
		{
			private int indx = -1;
			private int curRead = 0;
			private T[] _queue = new T[1000];
			
			public void Write(T obj)
			{
				_queue[++indx] = obj;
			}

			public async Task<T> Get()
			{
				while (indx < curRead)
				{
					await Task.Delay(5);
				}

				return _queue[curRead++];
			}
		}



		private async Task<int> RunProgram(List<int> codes, InputOutputStream<int> input, InputOutputStream<int> output)
		{
			var instructionSize = 4;
			for (var ip = 0; codes[ip] != 99; ip += instructionSize)
			{
				instructionSize = 4;
				var opcode = codes[ip] % 100;
				switch (opcode)
				{
					case 1:
						codes[codes[ip + 3]] = GetVal(codes, ip, 1) + GetVal(codes, ip, 2);
						break;
					case 2:
						codes[codes[ip + 3]] = GetVal(codes, ip, 1) * GetVal(codes, ip, 2);
						break;
					case 3:
						instructionSize = 2;
						codes[codes[ip + 1]] = await input.Get();
						break;
					case 4:
						instructionSize = 2;
						output.Write(GetVal(codes, ip, 1));
						break;
					case 5:
						var test = GetVal(codes, ip, 1) != 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2);
						}
						break;
					case 6:
						test = GetVal(codes, ip, 1) == 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2);
						}
						break;
					case 7:
						test = GetVal(codes, ip, 1) < GetVal(codes, ip, 2);
						codes[codes[ip + 3]] = test ? 1 : 0;
						break;
					case 8:
						test = GetVal(codes, ip, 1) == GetVal(codes, ip, 2);
						codes[codes[ip + 3]] = test ? 1 : 0;
						break;
					case 99:
						return 1;
					default:
						throw new NotImplementedException(codes[ip].ToString());
				}
			}

			return 0;
		}

		private int GetVal(List<int> codes, int ip, int ipOffset)
		{
			var modeOpCode = codes[ip];
			var paramMode = ipOffset == 1 ? (modeOpCode / 100) % 10 :
				ipOffset == 2 ? (modeOpCode / 1000) % 10 :
				ipOffset == 3 ? (modeOpCode / 10000) % 10 :
				ipOffset == 4 ? (modeOpCode / 100000) % 10 : int.MinValue;
			switch (paramMode)
			{
				case 0:
					return codes[codes[ip + ipOffset]];
				case 1:
					return codes[ip + ipOffset];
				default:
					throw new Exception();
			}
		}
	}
}
