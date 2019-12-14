using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day12: DayCodeBase
	{
		public override string Problem1()
		{
			var moons = GetData().Select(d => new Moon(d)).ToList();
			for (var i = 0; i < 1000; ++i)
			{
				foreach (var moon in moons)
				{
					moon.CalculateVelocity(moons);
				}

				foreach (var moon in moons)
				{
					moon.ApplyVelocity();
				}
			}

			return moons.Sum(m => m.TotalEnergy).ToString();
		}
		
		public override string Problem2()
		{
			var moons = GetData().Select(d => new Moon(d)).ToList();
			var previousStates = new[] {new HashSet<string>(), new HashSet<string>(), new HashSet<string>()};
			var cycleLen = new int?[] {null, null, null};
			var i = 0;

			for (var states = GetStates(moons); cycleLen.Any(l => l == null); ++i)
			{

				previousStates[0].Add(states[0]);
				previousStates[1].Add(states[1]);
				previousStates[2].Add(states[2]);

				foreach (var moon in moons)
				{
					moon.CalculateVelocity(moons);
				}

				foreach (var moon in moons)
				{
					moon.ApplyVelocity();
				}

				states = GetStates(moons);
				if (cycleLen[0] == null && previousStates[0].Contains(states[0])) cycleLen[0] = i + 1;
				if (cycleLen[1] == null && previousStates[1].Contains(states[1])) cycleLen[1] = i + 1;
				if (cycleLen[2] == null && previousStates[2].Contains(states[2])) cycleLen[2] = i + 1;
			}

			return lcm((int)cycleLen[2], lcm((int)cycleLen[1], (int)cycleLen[0])).ToString();
		}


		private long lcm(long num1, long num2)
		{
			var x = num1;
			var y = num2;
			while (num1 != num2)
			{
				if (num1 > num2) num1 -= num2;
				else num2 -= num1;
			}
			return (x * y) / num1;
		}

		private string[] GetStates(List<Moon> moons)
		{
			var toReturn = new []{ new StringBuilder(), new StringBuilder(), new StringBuilder() };
			for (var i = 0; i < 3; ++i)
			{
				foreach (var moon in moons)
				{
					toReturn[i].AppendLine(moon.Print(i));
				}
			}
			return toReturn.Select(sb => sb.ToString()).ToArray();
		}

		class Moon
		{
			public long[] Pos { get; set; }
			public long[] Vel { get; set; }
			public long PotentialEnergy => Math.Abs(Pos[0]) + Math.Abs(Pos[1]) + Math.Abs(Pos[2]);
			public long KineticEnergy => Math.Abs(Vel[0]) + Math.Abs(Vel[1]) + Math.Abs(Vel[2]);
			public long TotalEnergy => PotentialEnergy * KineticEnergy;

			public Moon(string data)
			{
				Pos = new[] {0L, 0L, 0L};
				Vel = new[] { 0L, 0L, 0L };
				var coords = data.Split(',');
				Pos[0] = int.Parse(coords[0].Split('=')[1]);
				Pos[1] = int.Parse(coords[1].Split('=')[1]);
				Pos[2] = int.Parse(coords[2].Replace(">",string.Empty).Split('=')[1]);
			}

			public void CalculateVelocity(IEnumerable<Moon> moons)
			{
				foreach (var moon in moons.Where(m => m != this))
				{
					Vel[0] += Pos[0] > moon.Pos[0] ? -1 : Pos[0] < moon.Pos[0] ? 1 : 0;
					Vel[1] += Pos[1] > moon.Pos[1] ? -1 : Pos[1] < moon.Pos[1] ? 1 : 0;
					Vel[2] += Pos[2] > moon.Pos[2] ? -1 : Pos[2] < moon.Pos[2] ? 1 : 0;
				}
			}

			public void ApplyVelocity()
			{
				Pos[0] += Vel[0];
				Pos[1] += Vel[1];
				Pos[2] += Vel[2];
			}

			public string Print(int axis)
			{
				return $"{Pos[axis]},{Vel[axis]}";
			}
		}
	}
}
