using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day3: DayCodeBase
	{
		public override string Problem1()
		{
			var input = GetData();
			var (wire1, d1) = GetPath(input[0]);
			var (wire2, d2) = GetPath(input[1]);
			var intersections = wire1.Intersect(wire2);
			return intersections.Select(p => Math.Abs(p.X) + Math.Abs(p.Y)).Min().ToString();
		}


		public override string Problem2()
		{
			var input = GetData();
			var (wire1, d1) = GetPath(input[0]);
			var (wire2, d2) = GetPath(input[1]);
			var intersections = wire1.Intersect(wire2);
			return intersections.Select(p => d1[p] + d2[p]).Min().ToString();
		}

		private (HashSet<Point> path, Dictionary<Point, int> distance) GetPath(string path)
		{
			var toReturnPath = new HashSet<Point>();
			var toReturnDistance = new Dictionary<Point, int>();
			var instructions = path.Split(',', StringSplitOptions.RemoveEmptyEntries);
			var curLocation = new Point(0,0);
			var totalDistance = 1;
			foreach (var instruction in instructions)
			{
				var (x, y) = GetDirectionOffset(instruction[0]);
				var steps = int.Parse(instruction.Substring(1));
				for (var curStep = 0; curStep < steps; ++curStep, ++totalDistance)
				{
					curLocation = new Point(curLocation.X + x, curLocation.Y + y);
					toReturnPath.Add(curLocation);
					if (!toReturnDistance.ContainsKey(curLocation)) toReturnDistance.Add(curLocation, totalDistance);
				}
			}
			return (toReturnPath, toReturnDistance);
		}

		private (int x, int y) GetDirectionOffset(char c)
		{
			switch (c)
			{
				case 'U': return (0, -1);
				case 'D': return (0, 1);
				case 'L': return (-1, 0);
				case 'R': return (1, 0);
				default: throw new Exception();
			}
		}
	}
}
