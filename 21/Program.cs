string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var numericPad = new string[][]
{
	new string[] { "7", "8", "9" },
	new string[] { "4", "5", "6" },
	new string[] { "1", "2", "3" },
	new string[] { string.Empty, "0", "A" }
};

var directionalPad = new string[][]
{
	new string[] { string.Empty, "^", "A" },
	new string[] { "<", "v", ">" }
};

var cache = new Dictionary<(string, string, int), long>();

long result = 0;

foreach (string line in lines)
{
	int number = int.Parse(line.Substring(0, 3));
	long total = 0;

	for (int i = 0; i < line.Length; i++)
	{
		string startPosition = i == 0
			? "A"
			: line[i - 1].ToString();
		string endPosition = line[i].ToString();

		total += Shortest(GetPos(numericPad, startPosition), GetPos(numericPad, endPosition), 3);
	}

	result += number * total;
}

Console.WriteLine(result);

// Part 2
result = 0;

foreach (string line in lines)
{
	int number = int.Parse(line.Substring(0, 3));
	long total = 0;

	for (int i = 0; i < line.Length; i++)
	{
		string startPosition = i == 0
			? "A"
			: line[i - 1].ToString();
		string endPosition = line[i].ToString();

		total += Shortest(GetPos(numericPad, startPosition), GetPos(numericPad, endPosition), 26);
	}

	result += number * total;
}

Console.WriteLine(result);


(int, int)? GetPos(string[][] pad, string code)
{
	for (int i = 0; i < pad.Length; i++)
	{
		var row = pad[i];
		var index = Array.IndexOf(row, code);

		if (index != -1)
		{
			return (i, index);
		}
	}

	return null;
}

long Shortest(object? start, object? end, int layers)
{
	if (start is string sStart &&
		end is string sEnd
		&& sStart == "<"
		&& sEnd == ">")
	{
		return 0;
	}

	if (start is string startStr)
	{
		start = GetPos(directionalPad, startStr);
	}

	if (end is string endStr)
	{
		end = GetPos(directionalPad, endStr);
	}

	if (!(start is (int, int)) ||
		!(end is (int, int)))
	{
		throw new InvalidOperationException("Invalid start or end position.");
	}

	var startPosition = ((int, int))start;
	var endPosition = ((int, int))end;

	var key = (startPosition.ToString(), endPosition.ToString(), layers);
	if (cache.ContainsKey(key))
	{
		return cache[key];
	}

	if (layers == 0)
	{
		return 1;
	}

	string verticalMove = string.Empty;
	string horizontalMove = string.Empty;

	if (endPosition.Item1 < startPosition.Item1)
	{
		verticalMove = "^";
	}
	else if (endPosition.Item1 > startPosition.Item1)
	{
		verticalMove = "v";
	}

	if (endPosition.Item2 < startPosition.Item2)
	{
		horizontalMove = "<";
	}
	else if (endPosition.Item2 > startPosition.Item2)
	{
		horizontalMove = ">";
	}

	long result;
	if (horizontalMove == null && verticalMove == null)
	{
		result = Shortest("A", "A", layers - 1);
	}
	else if (horizontalMove == null)
	{
		result = Shortest("A", verticalMove, layers - 1) +
			(Math.Abs(endPosition.Item1 - startPosition.Item1) - 1) * Shortest(verticalMove, verticalMove, layers - 1) + Shortest(verticalMove, "A", layers - 1);
	}
	else if (verticalMove == null)
	{
		result = Shortest("A", horizontalMove, layers - 1) +
			(Math.Abs(endPosition.Item2 - startPosition.Item2) - 1) * Shortest(horizontalMove, horizontalMove, layers - 1) + Shortest(horizontalMove, "A", layers - 1);
	}
	else
	{
		result = Math.Min(
			Shortest("A", horizontalMove, layers - 1) + (Math.Abs(endPosition.Item2 - startPosition.Item2) - 1) * Shortest(horizontalMove, horizontalMove, layers - 1) + Shortest(horizontalMove, verticalMove, layers - 1) + (Math.Abs(endPosition.Item1 - startPosition.Item1) - 1) * Shortest(verticalMove, verticalMove, layers - 1) + Shortest(verticalMove, "A", layers - 1),
			Shortest("A", verticalMove, layers - 1) + (Math.Abs(endPosition.Item1 - startPosition.Item1) - 1) * Shortest(verticalMove, verticalMove, layers - 1) + Shortest(verticalMove, horizontalMove, layers - 1) + (Math.Abs(endPosition.Item2 - startPosition.Item2) - 1) * Shortest(horizontalMove, horizontalMove, layers - 1) + Shortest(horizontalMove, "A", layers - 1)
		);
	}

	cache[key] = result;

	return result;
}
