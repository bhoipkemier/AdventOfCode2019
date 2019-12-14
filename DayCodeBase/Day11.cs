using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day11: DayCodeBase
	{
		public override string Problem1()
		{
			var codes = GetProgram();
			var camera = new Camera();
			var computer = new Computer();
			var result = computer.RunProgram(codes, camera, camera).Result;
			return camera.GetPaintedCount().ToString();
		}
		public override string Problem2()
		{
			var codes = GetProgram();
			var camera = new Camera(true);
			var computer = new Computer();
			var result = computer.RunProgram(codes, camera, camera).Result;
			return camera.Print();
		}

		public class Camera : IInputOutputStream<long>
		{
			private HashSet<Point> _visited = new HashSet<Point>();
			private HashSet<Point> _white = new HashSet<Point>();
			private bool inPaintMode = true;
			private Point _curLocation = new Point(0,0);
			private int heading = 0;

			public Camera(bool startOnWhite = false)
			{
				if (startOnWhite) _white.Add(_curLocation);
			}


			public void Write(long output)
			{
				if (inPaintMode)
				{
					if (output == 1) _white.Add(_curLocation);
					else if (output == 0) _white.Remove(_curLocation);
					else throw new NotImplementedException();
					_visited.Add(_curLocation);
				}
				else
				{
					if (output == 0) heading = new[] { 3, 0, 1, 2 }[heading];
					else if (output == 1) heading = new[] { 1, 2, 3, 0 }[heading];
					else throw new NotImplementedException();
					_curLocation = heading == 0 ? new Point(_curLocation.X, _curLocation.Y - 1) :
						heading == 1 ? new Point(_curLocation.X + 1, _curLocation.Y) :
						heading == 2 ? new Point(_curLocation.X, _curLocation.Y + 1) :
						heading == 3 ? new Point(_curLocation.X - 1, _curLocation.Y) : throw new Exception();
				}
				inPaintMode = !inPaintMode;
			}

			public Task<long> Get()
			{
				return Task.FromResult(_white.Contains(_curLocation) ? 1L : 0L);
			}

			public int GetPaintedCount()
			{
				return _visited.Count;
			}

			public string Print()
			{
				var sb = new StringBuilder("\r\n");
				for (var y = Math.Min(_visited.Min(v => v.Y), _curLocation.Y); y <= Math.Max(_visited.Max(v => v.Y), _curLocation.Y); ++y)
				{
					for (var x = Math.Min(_visited.Min(v => v.X), _curLocation.X); x <= Math.Max(_visited.Max(v => v.X), _curLocation.X); ++x)
					{
						var loc = new Point(x,y);
						sb.Append(_white.Contains(loc) ? "█" : " ");
					}
					sb.Append("\r\n");
				}

				return sb.ToString();
			}
		}
	}
}
