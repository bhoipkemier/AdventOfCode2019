using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.DayCodeBase
{
	public class Day8: DayCodeBase
	{
		public override string Problem1()
		{
			var image = GetData().First();
			var layers = GetLayers(image, 25, 6);
			var layerGroups = layers.Select(l => new
			{
				layer = l,
				zeros = l.ToCharArray().Count(c => c == '0'),
				ones = l.ToCharArray().Count(c => c == '1'),
				twos = l.ToCharArray().Count(c => c == '2'),
			});
			var minZeroCount = layerGroups.Min(g => g.zeros);
			var min0Layer = layerGroups.First(l => l.zeros == minZeroCount);

			return (min0Layer.ones * min0Layer.twos).ToString();
		}
		public override string Problem2()
		{
			var result = new string('2', 25 * 6);
			var image = GetData(0).First();
			var layers = GetLayers(image, 25, 6);
			foreach (var layer in layers)
			{
				result = MergeLayers(result, layer);
			}

			result = result.Replace("0", " ").Replace("1", "█");

			var lines = GetLayers(result, 25, 1);
			return $"\r\n{string.Join("\r\n", lines)}";
		}

		private string MergeLayers(string result, string layer)
		{
			return new string(
				result.ToCharArray()
					.Zip(layer.ToCharArray(), (c1, c2) => c1 == '2' ? c2 : c1)
					.ToArray());
		}


		private List<string> GetLayers(string image, int width, int height)
		{
			var toReturn = new List<string>();
			var imageSize = width * height;
			for(var startPos = 0; startPos < image.Length-1; startPos += imageSize)
				toReturn.Add(image.Substring(startPos, imageSize));
			return toReturn;
		}
	}
}
