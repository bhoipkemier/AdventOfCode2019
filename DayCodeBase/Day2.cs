using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day2: DayCodeBase
	{
		public override string Problem1()
		{
			return LoadAndRun(12, 02).ToString();
		}

		public override string Problem2()
		{
			for (var noun = 0; noun <= 99; ++noun)
			{
				for (var verb = 0; verb <= 99; ++verb)
				{
					if (LoadAndRun(noun, verb) == 19690720) return (noun * 100 + verb).ToString();
				}
			}
			throw new NotImplementedException();
		}

		private int LoadAndRun(int noun, int verb)
		{
			var codes = GetData(0, ",").Select(int.Parse).ToList();
			codes[1] = noun;
			codes[2] = verb;
			RunProgram(codes);
			return codes[0];
		}

		private void RunProgram(List<int> codes)
		{
			for (var instructionPointer = 0; codes[instructionPointer] != 99; instructionPointer += 4)
			{
				if (codes[instructionPointer] == 1)
				{
					codes[codes[instructionPointer + 3]] = codes[codes[instructionPointer + 1]] + codes[codes[instructionPointer + 2]];
				}
				else if(codes[instructionPointer] == 2)
				{
					codes[codes[instructionPointer + 3]] = codes[codes[instructionPointer + 1]] * codes[codes[instructionPointer + 2]];
				}
				else
				{
					throw new NotImplementedException(codes[instructionPointer].ToString());
				}
			}

		}
	}
}
