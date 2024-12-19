string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

List<string> map = new List<string>();

foreach (var line in lines)
{
	if (!string.IsNullOrWhiteSpace(line))
	{
		map.Add(line.Trim());
	}
}

int rows = map.Count;
int cols = map[0].Length;

int[] directions = { -1, 0, 0, 1, 1, 0, 0, -1 };

int startRow = 0, startCol = 0, endRow = 0, endCol = 0;

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (map[r][c] == 'S')
		{
			startRow = r;
			startCol = c;
		}
		if (map[r][c] == 'E')
		{
			endRow = r;
			endCol = c;
		}
	}
}

var result1 = GetPart1();

Console.WriteLine(result1.best);

var result2 = CalculateOptimalPaths(result1);

Console.WriteLine(result2);


(int best, Dictionary<(int, int, int), int> distances) GetPart1()
{
	var queue = new PriorityQueue<(int score, int r, int c, int direction), int>();
	var visited = new HashSet<(int, int, int)>();
	var distances = new Dictionary<(int, int, int), int>();

	queue.Enqueue((0, startRow, startCol, 1), 0);

	int? best = null;

	while (queue.Count > 0)
	{
		var (score, r, c, direction) = queue.Dequeue();

		if (!distances.ContainsKey((r, c, direction)))
		{
			distances[(r, c, direction)] = score;
		}

		if (r == endRow &&
			c == endCol &&
			best == null)
		{
			best = score;
		}

		if (visited.Contains((r, c, direction)))
		{
			continue;
		}

		visited.Add((r, c, direction));

		int dr = directions[direction * 2];
		int dc = directions[direction * 2 + 1];
		int newR = r + dr;
		int newC = c + dc;

		if (newR >= 0 &&
			newR < rows &&
			newC >= 0 &&
			newC < cols &&
			map[newR][newC] != '#')
		{
			queue.Enqueue((score + 1, newR, newC, direction), score + 1);
		}

		queue.Enqueue((score + 1000, r, c, (direction + 1) % 4), score + 1000);
		queue.Enqueue((score + 1000, r, c, (direction + 3) % 4), score + 1000);
	}

	return (best ?? -1, distances);
}

int CalculateOptimalPaths((int best, Dictionary<(int, int, int), int> distances) part1)
{
	var queue = new PriorityQueue<(int score, int r, int c, int direction), int>();
	var visited = new HashSet<(int, int, int)>();
	var distances = new Dictionary<(int, int, int), int>();

	for (int i = 0; i < 4; i++)
	{
		queue.Enqueue((0, endRow, endCol, i), 0);
	}

	while (queue.Count > 0)
	{
		var (score, r, c, direction) = queue.Dequeue();

		if (!distances.ContainsKey((r, c, direction)))
		{
			distances[(r, c, direction)] = score;
		}

		if (visited.Contains((r, c, direction)))
		{
			continue;
		}

		visited.Add((r, c, direction));

		int dr = directions[((direction + 2) % 4) * 2];
		int dc = directions[((direction + 2) % 4) * 2 + 1];
		int rr = r + dr;
		int cc = c + dc;

		if (0 <= rr && rr < rows && 0 <= cc && cc < cols && map[rr][cc] != '#')
		{
			queue.Enqueue((score + 1, rr, cc, direction), score + 1);
		}

		queue.Enqueue((score + 1000, r, c, (direction + 1) % 4), score + 1000);
		queue.Enqueue((score + 1000, r, c, (direction + 3) % 4), score + 1000);
	}

	var validPaths = new HashSet<(int, int)>();

	for (int r = 0; r < rows; r++)
	{
		for (int c = 0; c < cols; c++)
		{
			for (int i = 0; i < 4; i++)
			{
				if (part1.distances.ContainsKey((r, c, i)) &&
					distances.ContainsKey((r, c, i)) &&
					part1.distances[(r, c, i)] + distances[(r, c, i)] == part1.best)
				{
					validPaths.Add((r, c));
				}
			}
		}
	}

	return validPaths.Count;
}