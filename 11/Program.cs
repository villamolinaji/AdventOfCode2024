var line = File.ReadAllTextAsync("Input.txt").Result;

var stones = new List<long>(Array.ConvertAll(line.Split(' '), long.Parse));

int iterations = 25;

var solved = new Dictionary<(long, int), long>();

long result = 0;

foreach (long stone in stones)
{
	result += Solve(stone, iterations);
}

Console.WriteLine(result);

// Part2
iterations = 75;
result = 0;

foreach (long stone in stones)
{
	result += Solve(stone, iterations);
}

Console.WriteLine(result);


long Solve(long stone, int iterations)
{
	if (solved.ContainsKey((stone, iterations)))
	{
		return solved[(stone, iterations)];
	}

	long value = 0;

	if (iterations == 0)
	{
		value = 1;
	}
	else if (stone == 0)
	{
		value = Solve(1, iterations - 1);
	}
	else if (stone.ToString().Length % 2 == 0)
	{
		string stoneString = stone.ToString();
		string leftString = stoneString.Substring(0, stoneString.Length / 2);
		string rightString = stoneString.Substring(stoneString.Length / 2);

		int left = int.Parse(leftString);
		int right = int.Parse(rightString);

		value = Solve(left, iterations - 1) + Solve(right, iterations - 1);
	}
	else
	{
		value = Solve(stone * 2024, iterations - 1);
	}

	solved[(stone, iterations)] = value;

	return value;
}
