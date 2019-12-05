using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day4: DayCodeBase
	{
		public override string Problem1()
		{
			var toReturn = 0;
			for (var curNum = 273025; curNum <= 767253; ++curNum)
			{
				if (ValidPassword(curNum)) ++toReturn;
			}
			return toReturn.ToString();
		}
		public override string Problem2()
		{
			var toReturn = 0;
			for (var curNum = 273025; curNum <= 767253; ++curNum)
			{
				if (ValidPassword(curNum) && ValidOccurTest(curNum)) ++toReturn;
			}
			return toReturn.ToString();
		}

		private bool ValidPassword(int curNum)
		{
			var str = curNum.ToString();
			var hasSequential = false;
			var increase = true;
			for (var i = 1; i<str.Length; ++i)
				if (str[i] == str[i - 1])
					hasSequential = true;

			for (var i = 1; i < str.Length; ++i)
				if (str[i] < str[i - 1])
					increase = false;
			return increase && hasSequential;
		}

		private bool ValidOccurTest(int curNum)
		{
			return curNum
				.ToString()
				.ToCharArray()
				.GroupBy(c => c)
				.Any(g => g.Count() == 2);
		}
	}
}
