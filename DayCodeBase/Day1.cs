using System;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day1: DayCodeBase
	{
		public override string Problem1()
		{
			return GetData()
				.Select(int.Parse)
				.Select(i => ((int)(i/3))-2)
				.Sum()
				.ToString();
		}

		public override string Problem2()
		{
			return GetData()
				.Select(int.Parse)
				.Select(FuelCalculation)
				.Sum()
				.ToString();
		}

		private int FuelCalculation(int moduleWeight)
		{
			var toReturn = 0;
			while(moduleWeight > 0)
			{
				var toAdd = Math.Max(0, ((int)(moduleWeight / 3)) - 2);
				toReturn += toAdd;
				moduleWeight = toAdd;
			}
			return toReturn;
		}
	}
}
