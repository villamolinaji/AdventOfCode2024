string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

long total = 0;
long total2 = 0;

foreach (var line in lines)
{
	string[] parts = line.Split(":");
	long target = long.Parse(parts[0].Trim());
	string[] numbersString = parts[1].Trim().Split(" ");
	long[] numbers = Array.ConvertAll(numbersString, long.Parse);

	if (CanTarget(numbers, target, false))
	{
		total += target;
	}

	if (CanTarget(numbers, target, true))
	{
		total2 += target;
	}
}

Console.WriteLine(total);
Console.WriteLine(total2);


bool CanTarget(long[] numbers, long target, bool isPart2)
{
	return CheckTarget(numbers, target, 0, numbers[0], isPart2);
}

bool CheckTarget(long[] numbers, long target, int index, long currentValue, bool isPart2)
{
	if (index == numbers.Length - 1)
	{
		return currentValue == target;
	}

	int nextIndex = index + 1;
	long nextNumber = numbers[nextIndex];
	long nextValue = currentValue + nextNumber;

	if (CheckTarget(numbers, target, nextIndex, nextValue, isPart2))
	{
		return true;
	}

	nextValue = currentValue * nextNumber;
	if (CheckTarget(numbers, target, nextIndex, nextValue, isPart2))
	{
		return true;
	}

	if (isPart2)
	{
		nextValue = long.Parse(currentValue.ToString() + nextNumber.ToString());
		if (CheckTarget(numbers, target, nextIndex, nextValue, isPart2))
		{
			return true;
		}
	}

	return false;
}