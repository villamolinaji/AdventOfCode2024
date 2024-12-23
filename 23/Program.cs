string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var connections = new Dictionary<string, HashSet<string>>();

foreach (var line in lines)
{
	var computers = line.Split('-');
	var computerA = computers[0];
	var computerB = computers[1];

	if (!connections.ContainsKey(computerA))
	{
		connections[computerA] = new HashSet<string>();
	}

	if (!connections.ContainsKey(computerB))
	{
		connections[computerB] = new HashSet<string>();
	}

	connections[computerA].Add(computerB);
	connections[computerB].Add(computerA);
}

var triangles = new List<(string, string, string)>();

foreach (var node in connections)
{
	var neighbors = node.Value.ToList();

	for (int i = 0; i < neighbors.Count; i++)
	{
		for (int j = i + 1; j < neighbors.Count; j++)
		{
			if (connections[neighbors[i]].Contains(neighbors[j]))
			{
				var triangle = new[]
				{
					node.Key,
					neighbors[i],
					neighbors[j]
				};

				Array.Sort(triangle);

				triangles.Add((triangle[0], triangle[1], triangle[2]));
			}
		}
	}
}

triangles = triangles.Distinct().ToList();

var result = triangles.Count(t => t.Item1.StartsWith('t') || t.Item2.StartsWith('t') || t.Item3.StartsWith('t'));

Console.WriteLine(result);


// Part 2
var largest = new List<string>();

foreach (var node in connections)
{
	var allConnected = new List<string> { node.Key };
	var neighbors = node.Value.ToList();

	allConnected.AddRange(
		from neighbor in neighbors
		where allConnected.All(n => connections[n].Contains(neighbor))
		select neighbor);

	if (allConnected.Count > largest.Count)
	{
		largest = allConnected;
	}
}

largest.Sort();

string password = string.Join(",", largest);

Console.WriteLine(password);