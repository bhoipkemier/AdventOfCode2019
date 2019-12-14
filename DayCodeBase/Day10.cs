using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day10: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData(1);
			var points = GetPoints(data);
			var pointAngles = points.Select(p => new { point = p, angles = GetAngles(points, p) });
			var maxCount = pointAngles.Max(pa => pa.angles.Count);
			var location = pointAngles.First(pa => pa.angles.Count == maxCount);
			return location.angles.Count.ToString();
		}

		public override string Problem2()
		{
			var data = GetData(1);
			var points = GetPoints(data);
			var pointAngles = points.Select(p => new { point = p, angles = GetAngles(points, p) });
			var maxCount = pointAngles.Max(pa => pa.angles.Count);
			var location = pointAngles.First(pa => pa.angles.Count == maxCount);
			var eliminatedLocation = GetLocation(location.point, location.angles, 200);
			return eliminatedLocation.X*100+eliminatedLocation.Y.ToString();
		}

		private Point GetLocation(Point point, Dictionary<Point, List<Point>> angles, int elimCount)
		{
			var radialOrder = angles.Keys.OrderBy(a => Math.Atan2(a.Y, a.X)).ToList();
			var startAt = radialOrder.IndexOf(radialOrder.First(a => Math.Atan2(a.Y, a.X) == Math.Atan2(-1, 0)));
			for (var i = startAt; ; i = (i + 1) % radialOrder.Count)
			{
				if (angles[radialOrder[i]].Any())
				{
					var item = angles[radialOrder[i]].First();
					angles[radialOrder[i]] = angles[radialOrder[i]].Skip(1).ToList();
					elimCount--;
					if (elimCount == 0) return item;
				}
			}
		}

		private Dictionary<Point, List<Point>> GetAngles(List<Point> points, Point origin)
		{
			var toReturn = new Dictionary<Point, List<Point>>();
			foreach (var point in points.Where(p => p != origin))
			{
				var angle = GetAngle(origin, point);
				if(!toReturn.ContainsKey(angle)) toReturn.Add(angle, new List<Point>());
				toReturn[angle].Add(point);
			}

			foreach (var returnKey in toReturn.Keys.ToList())
			{
				toReturn[returnKey] = toReturn[returnKey].OrderBy(point => GetDistance(origin, point)).ToList();
			}
			return toReturn;
		}

		private int GetDistance(Point origin, Point point)
		{
			return Math.Abs(origin.X - point.X) + Math.Abs(origin.Y - point.Y);
		}

		private Point GetAngle(Point origin, Point point)
		{
			var x = point.X - origin.X;
			var y = point.Y - origin.Y;
			var gcd = GCD((ulong)Math.Abs(x), (ulong)Math.Abs(y));
			return new Point(x/(int)gcd, y/(int)gcd);
		}

		private static ulong GCD(ulong a, ulong b)
		{
			while (a != 0 && b != 0)
			{
				if (a > b)
					a %= b;
				else
					b %= a;
			}

			return a == 0 ? b : a;
		}

		private List<Point> GetPoints(string[] data)
		{
			var toReturn = new List<Point>();
			for (var y = 0; y < data.Length; ++y)
			{
				var line = data[y];
				for (var x = 0; x < line.Length; ++x)
				{
					if(line[x] == '#') toReturn.Add(new Point(x,y));
				}
			}

			return toReturn;
		}
	}
}
