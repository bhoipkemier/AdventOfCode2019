using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day9: DayCodeBase
	{
		public override string Problem1()
		{
			var codes = LoadProgram(0);
			var inputStream = new InputOutputStream<long>();
			inputStream.Write(1);
			var outputStream = new InputOutputStream<long>();
			var result = RunProgram(codes, inputStream, outputStream);
			return string.Join(", ", outputStream.GetQueue());
		}
		public override string Problem2()
		{
			var codes = LoadProgram(0);
			var inputStream = new InputOutputStream<long>();
			inputStream.Write(2);
			var outputStream = new InputOutputStream<long>();
			var result = RunProgram(codes, inputStream, outputStream);
			return string.Join(", ", outputStream.GetQueue());
		}

		private Dictionary<long, long> LoadProgram(int file)
		{
			var data = GetData(file, ",");
			var toReturn = new Dictionary<long, long>();
			for (var l = 0l; l < data.Length; ++l)
			{
				toReturn[l] = long.Parse(data[(int) l]);
			}
			return toReturn;
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

			public List<T> GetQueue()
			{
				return _queue.Skip(curRead).Take(indx + 1).ToList();
			}
		}



		private async Task<int> RunProgram(Dictionary<long, long> codes, InputOutputStream<long> input, InputOutputStream<long> output)
		{
			var instructionSize = 4L;
			var relativeBase = 0L;
			for (var ip = 0L; codes[ip] != 99L; ip += instructionSize)
			{
				instructionSize = 4;
				var opcode = codes[ip] % 100;
				switch (opcode)
				{
					case 1:
						var val = GetVal(codes, ip, 1, relativeBase) + GetVal(codes, ip, 2, relativeBase);
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 2:
						val = GetVal(codes, ip, 1, relativeBase) * GetVal(codes, ip, 2, relativeBase);
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 3:
						instructionSize = 2;
						SetVal(codes, ip, 1, relativeBase, await input.Get());
						break;
					case 4:
						instructionSize = 2;
						output.Write(GetVal(codes, ip, 1, relativeBase));
						break;
					case 5:
						var test = GetVal(codes, ip, 1, relativeBase) != 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2, relativeBase);
						}
						break;
					case 6:
						test = GetVal(codes, ip, 1, relativeBase) == 0;
						instructionSize = test ? 0 : 3;
						if (test)
						{
							ip = GetVal(codes, ip, 2, relativeBase);
						}
						break;
					case 7:
						test = GetVal(codes, ip, 1, relativeBase) < GetVal(codes, ip, 2, relativeBase);
						val = test ? 1 : 0;
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 8:
						test = GetVal(codes, ip, 1, relativeBase) == GetVal(codes, ip, 2, relativeBase);
						val = test ? 1 : 0;
						SetVal(codes, ip, 3, relativeBase, val);
						break;
					case 9:
						instructionSize = 2;
						relativeBase += GetVal(codes, ip, 1, relativeBase);
						break;
					case 99:
						return 1;
					default:
						throw new NotImplementedException(codes[ip].ToString());
				}
			}

			return 0;
		}

		private void SetVal(Dictionary<long, long> codes, long ip, int ipOffset, long relativeBase, long val)
		{
			var modeOpCode = codes[ip];
			var paramMode = ipOffset == 1 ? (modeOpCode / 100) % 10 :
				ipOffset == 2 ? (modeOpCode / 1000) % 10 :
				ipOffset == 3 ? (modeOpCode / 10000) % 10 :
				ipOffset == 4 ? (modeOpCode / 100000) % 10 : int.MinValue;
			long addr;
			switch (paramMode)
			{
				case 0L:
					addr = codes[ip + ipOffset];
					break;
				case 1L:
					throw new Exception();
				case 2L:
					addr = codes[ip + ipOffset] + relativeBase;
					break;
				default:
					throw new Exception();
			}
			codes[addr] = val;
		}

		private long GetVal(Dictionary<long, long> codes, long ip, long ipOffset, long relativeBase)
		{
			var modeOpCode = codes[ip];
			var paramMode = ipOffset == 1 ? (modeOpCode / 100) % 10 :
				ipOffset == 2 ? (modeOpCode / 1000) % 10 :
				ipOffset == 3 ? (modeOpCode / 10000) % 10 :
				ipOffset == 4 ? (modeOpCode / 100000) % 10 : int.MinValue;
			long addr;
			switch (paramMode)
			{
				case 0L:
					addr = codes[ip + ipOffset];
					break;
				case 1L:
					addr = ip + ipOffset;
					break;
				case 2L:
					addr = codes[ip + ipOffset] + relativeBase;
					break;
				default:
					throw new Exception();
			}

			return codes.ContainsKey(addr) ? codes[addr] : 0;
		}
	}
}
