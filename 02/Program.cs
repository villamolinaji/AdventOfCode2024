string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int safe = 0;
int safe2 = 0;

foreach (var line in lines)
{
	int[] levels = line.Split(' ').Select(int.Parse).ToArray();

	if (IsSafe(levels))
	{
		safe++;
		safe2++;
	}
	else if (IsSafe2(levels))
	{
		safe2++;
	}
}

Console.WriteLine(safe);
Console.WriteLine(safe2);


bool IsSafe(int[] levels)
{
	bool isIncreasing = true;
	bool isDecreasing = true;

	for (int i = 1; i < levels.Length; i++)
	{
		int difference = levels[i] - levels[i - 1];

		if (Math.Abs(difference) < 1 ||
			Math.Abs(difference) > 3)
		{
			return false;
		}

		if (difference < 0)
		{
			isIncreasing = false;
		}
		if (difference > 0)
		{
			isDecreasing = false;
		}
	}

	return isIncreasing || isDecreasing;
}

bool IsSafe2(int[] levels)
{
	for (int i = 0; i < levels.Length; i++)
	{
		var levelsRemoved = levels.Where((_, index) => index != i).ToArray();

		if (IsSafe(levelsRemoved))
		{
			return true;
		}
	}

	return false;
}