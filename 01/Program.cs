string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var left = new List<int>();
var right = new List<int>();

foreach (var line in lines)
{
	var splitLine = line.Split("   ");

	left.Add(int.Parse(splitLine[0]));
	right.Add(int.Parse(splitLine[1]));
}

left.Sort();
right.Sort();

var distance = 0;

for (int i = 0; i < left.Count; i++)
{
	distance += Math.Abs(left[i] - right[i]);
}

Console.WriteLine(distance);


// Part 2

var result = 0;

foreach (var l in left)
{
	var count = right.Count(r => r == l);
	result += l * count;
}

Console.WriteLine(result);