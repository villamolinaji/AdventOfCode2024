string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var patterns = new List<string>();
var designs = new List<string>();

foreach (var line in lines)
{
	if (line.Contains(","))
	{
		patterns.AddRange(line.Split(", ").ToList());
	}
	else if (!string.IsNullOrEmpty(line))
	{
		designs.Add(line);
	}
}

int count = 0;
long count2 = 0;

foreach (var design in designs)
{
	if (CanDesign(design))
	{
		count++;
	}

	count2 += CountWays(design, new Dictionary<string, long>());
}

Console.WriteLine(count);

Console.WriteLine(count2);


bool CanDesign(string design)
{
	int designLength = design.Length;
	bool[] dp = new bool[designLength + 1];
	dp[0] = true;

	for (int i = 1; i <= designLength; i++)
	{
		dp[i] = patterns
			.Any(pattern => i >= pattern.Length &&
				design.Substring(i - pattern.Length, pattern.Length) == pattern &&
				dp[i - pattern.Length]);
	}

	return dp[designLength];
}

long CountWays(string design, Dictionary<string, long> memo)
{
	if (memo.ContainsKey(design))
	{
		return memo[design];
	}

	if (string.IsNullOrEmpty(design))
	{
		return 1;
	}

	long ways = patterns
		.Where(pattern => design.StartsWith(pattern))
		.Sum(pattern => CountWays(design.Substring(pattern.Length), memo));

	memo[design] = ways;

	return ways;
}
