using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day13: DayCodeBase
	{
		public override string Problem1()
		{
			var codes = GetProgram();
			var computer = new Computer();
			var arcadeScreen = new ArcadeScreen();
			var result = computer.RunProgram(codes, arcadeScreen,arcadeScreen).Result;
			return arcadeScreen.BlockCount().ToString();
		}
		public override string Problem2()
		{
			var codes = GetProgram();
			codes[0L] = 2;
			var computer = new Computer();
			var arcadeScreen = new ArcadeScreen();
			var result = computer.RunProgram(codes, arcadeScreen, arcadeScreen).Result;
			return arcadeScreen.GetScore().ToString();
		}

		public class ArcadeScreen: IInputOutputStream<long>
		{
			private Dictionary<Point, int> _screen = new Dictionary<Point, int>();
			private List<int> _screenInput = new List<int>();
			private Point _lastBallPos = new Point(0,0);
			private Point _ballHitLoc = new Point(0,0);

			public void Write(long obj)
			{
				_screenInput.Add((int)obj);
				if (_screenInput.Count == 3)
				{
					Draw();
					_screenInput.Clear();
				}
			}

			private void Draw()
			{
				_screen[new Point(_screenInput[0], _screenInput[1])] = _screenInput[2];
			}

			public int BlockCount()
			{
				return _screen.Count(t => t.Value == 2);
			}

			public int GetScore()
			{
				return _screen[new Point(-1, 0)];
			}

			public Task<long> Get()
			{
				//PrintScreen();
				var paddle = _screen.FirstOrDefault(kvp => kvp.Value == 3).Key;
				var ball = _screen.FirstOrDefault(kvp => kvp.Value == 4).Key;
				var toReturn = 0;
				if (_lastBallPos != ball && _lastBallPos.Y < ball.Y)
				{
					var right = _lastBallPos.X < ball.X;
					var height = paddle.Y - ball.Y;
					_ballHitLoc = new Point(ball.X + (right ? height : -height), paddle.Y);
				}
				else
				{
					_ballHitLoc = new Point(ball.X,paddle.Y);
				}
				toReturn = paddle.X < _ballHitLoc.X ? 1 : paddle.X > _ballHitLoc.X ? -1 : 0;
				_lastBallPos = ball;
				return Task.FromResult((long)toReturn);
			}

			public void PrintScreen()
			{
				var sb = new StringBuilder("\r\n");
				var width = _screen.Max(kvp => kvp.Key.X);
				var height = _screen.Max(kvp => kvp.Key.Y);
				for (var y = 0; y <= height; ++y)
				{
					for (var x = 0; x <= width; ++x)
					{
						var c = _screen.ContainsKey(new Point(x, y)) ? _screen[new Point(x, y)] : 0;
						sb.Append(" |B_o"[c]);
					}

					sb.Append("\r\n");
				}


				Console.WriteLine(sb);
				Console.ReadLine();
			}
		}
		
	}
}
