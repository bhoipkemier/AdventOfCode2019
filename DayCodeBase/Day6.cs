using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day6: DayCodeBase
	{
		public override string Problem1()
		{
			var orbits = GetNodeSet(GetData());
			return orbits.Values.Select(GetOrbitCount).Sum().ToString();
		}


		public override string Problem2()
		{
			var orbits = GetNodeSet(GetData());
			return GetTransfers(GetAncesstors(orbits["YOU"]), GetAncesstors(orbits["SAN"])).ToString();
		}



		private List<string> GetAncesstors(Node node)
		{
			var toReturn = new List<string>();
			while (node.Parent != null)
			{
				toReturn.Add(node.Parent.Name);
				node = node.Parent;
			}
			return toReturn;
		}

		private int GetTransfers(List<string> ancesstors1, List<string> ancesstors2)
		{
			for (var i = 0; i < ancesstors1.Count; ++i)
			{
				if (ancesstors2.Contains(ancesstors1[i]))
				{
					var depth = ancesstors2.IndexOf(ancesstors1[i]);
					return i + depth;
				}
			}
			throw new Exception();
		}

		private int GetOrbitCount(Node node)
		{
			if (node.Parent == null) return 0;
			return GetOrbitCount(node.Parent) + 1;
		}

		private Dictionary<string, Node> GetNodeSet(IEnumerable<string> mappings)
		{
			Dictionary<string, Node> toReturn = new Dictionary<string, Node>();
			foreach (var mapping in mappings)
			{
				var body = mapping.Split(')');
				toReturn[body[0]] = toReturn.ContainsKey(body[0]) ? toReturn[body[0]] : new Node(body[0], null);
				toReturn[body[1]] = toReturn.ContainsKey(body[1]) ? toReturn[body[1]] : new Node(body[1], null);
				toReturn[body[1]].Parent = toReturn[body[0]];
			}

			return toReturn;
		}

		private class Node
		{
			public string Name;
			public Node Parent;

			public Node(string name, Node parent)
			{
				this.Name = name;
				this.Parent = parent;
			}
		}
	}
}
