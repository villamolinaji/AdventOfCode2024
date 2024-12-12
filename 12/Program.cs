string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int rows = lines.Length;
int cols = lines[0].Length;

int[] dR = { -1, 1, 0, 0 };
int[] dC = { 0, 0, -1, 1 };

var visited = new HashSet<(int, int)>();

var perimeterDictionary = new Dictionary<(int, int), HashSet<(int, int)>>();

int total = 0;
int total2 = 0;

for (int row = 0; row < rows; row++)
{
	for (int col = 0; col < cols; col++)
	{
		if (visited.Contains((row, col)))
		{
			continue;
		}

		perimeterDictionary = new Dictionary<(int, int), HashSet<(int, int)>>();

		(int area, int perimeter) = GetAreaAndPerimeter(row, col);

		int sides = GetSides();

		total += area * perimeter;

		total2 += area * sides;
	}
}

Console.WriteLine(total);
Console.WriteLine(total2);


(int area, int perimeter) GetAreaAndPerimeter(int startRow, int startCol)
{
	int area = 0;
	int perimeter = 0;

	var queue = new Queue<(int, int)>();
	queue.Enqueue((startRow, startCol));

	while (queue.Count > 0)
	{
		(int row, int col) = queue.Dequeue();

		if (visited.Contains((row, col)))
		{
			continue;
		}

		visited.Add((row, col));

		area++;

		for (int i = 0; i < dR.Length; i++)
		{
			int newRow = row + dR[i];
			int newCol = col + dC[i];

			if (newRow >= 0 &&
				newRow < rows &&
				newCol >= 0 &&
				newCol < cols &&
				lines[newRow][newCol] == lines[row][col])
			{
				queue.Enqueue((newRow, newCol));
			}
			else
			{
				perimeter++;

				var dir = (dR[i], dC[i]);

				if (!perimeterDictionary.ContainsKey(dir))
				{
					perimeterDictionary[dir] = new HashSet<(int, int)>();
				}

				perimeterDictionary[dir].Add((newRow, newCol));
			}
		}
	}

	return (area, perimeter);
}

int GetSides()
{
	int sides = 0;

	foreach (var perimeterValue in perimeterDictionary.Select(p => p.Value))
	{
		HashSet<(int, int)> visitedPerimeter = new HashSet<(int, int)>();

		foreach (var cell in perimeterValue)
		{
			if (visitedPerimeter.Contains(cell))
			{
				continue;
			}

			sides++;

			var queue = new Queue<(int, int)>();
			queue.Enqueue(cell);

			while (queue.Count > 0)
			{
				(int row, int col) = queue.Dequeue();

				if (visitedPerimeter.Contains((row, col)))
				{
					continue;
				}

				visitedPerimeter.Add((row, col));

				for (int i = 0; i < dR.Length; i++)
				{
					int newRow = row + dR[i];
					int newCol = col + dC[i];

					if (perimeterValue.Contains((newRow, newCol)))
					{
						queue.Enqueue((newRow, newCol));
					}
				}
			}
		}
	}

	return sides;
}