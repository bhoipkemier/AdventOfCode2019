using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day14 : DayCodeBase
	{
		public override string Problem1()
		{
			return Solve(1L).ToString();
		}
		
		public override string Problem2()
		{
			for (var answer = 2595000; true; ++answer)
			{
				var result = Solve(answer);
				if (result > 1000000000000L) return (answer - 1).ToString();
			}
			throw new NotImplementedException();
		}

		public long Solve(long fuelAmount)
		{
			var reactions = GetData().Select(d => new Reaction(d)).ToDictionary(d => d.Output, d => d);
			var ore = reactions["ORE"] = new Reaction(" => 1 ORE");
			reactions["FUEL"].Needed = fuelAmount;
			PopulateUseIn(reactions);
			Calculate(reactions);
			return (long)ore.Needed;
		}

		private void Calculate(Dictionary<string, Reaction> reactions)
		{
			var ore = reactions["ORE"];
			while (ore.Needed == null)
			{
				foreach (var reaction in reactions.Values.Where(r => r.CanCalculate()))
				{
					reaction.Calculate();
				}
			}
		}

		private void PopulateUseIn(Dictionary<string, Reaction> reactions)
		{
			foreach (var reaction in reactions.Values)
			{
				reaction.PopulateUsedIn(reactions);
			}
		}

		public class Reaction
		{
			public Dictionary<string, long> Inputs { get; set; }
			public string Output { get; set; }
			public long OutputAmount { get; set; }
			public long? Needed { get; set; }
			public HashSet<Reaction> UsedIn { get; set; }
			public Reaction(string data)
			{
				var io = data.Split(" => ");
				var inputs = io[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
				Inputs = inputs.ToDictionary(input => input.Split(" ")[1].Trim(),
					input => long.Parse(input.Split(" ")[0].Trim()));
				Output = io[1].Trim().Split(" ")[1];
				OutputAmount = long.Parse(io[1].Trim().Split(" ")[0]);
			}

			public void PopulateUsedIn(Dictionary<string, Reaction> reactions)
			{
				UsedIn = new HashSet<Reaction>(reactions.Where(kv => kv.Value.Inputs.ContainsKey(Output)).Select(kv => kv.Value));
			}

			public bool CanCalculate()
			{
				return UsedIn.All(r => r.Needed != null) && Needed == null;
			}

			public void Calculate()
			{
				Needed = UsedIn.Sum(r => r.Inputs[Output] * (long)Math.Ceiling((decimal)r.Needed/(decimal)r.OutputAmount));
			}
		}
	}
}
