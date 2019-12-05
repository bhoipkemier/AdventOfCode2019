using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day5: DayCodeBase
	{
		public override string Problem1()
		{
			var codes = GetData(0, ",").Select(int.Parse).ToList();
			var output = RunProgram(codes, 1);
			return output.Last().ToString();
		}

		public override string Problem2()
		{
			var codes = GetData(0, ",").Select(int.Parse).ToList();
			var output = RunProgram(codes, 5);
			return output.Last().ToString();
		}

		private List<int> RunProgram(List<int> codes, int input)
		{
			var output = new List<int>();
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
						codes[codes[ip + 1]] = input;
						break;
					case 4:
						instructionSize = 2;
						output.Add(codes[codes[ip + 1]]);
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
						return output;
					default:
						throw new NotImplementedException(codes[ip].ToString());
				}
			}

			return output;
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
