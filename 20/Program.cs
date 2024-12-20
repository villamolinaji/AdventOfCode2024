string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var grid = new List<string>();

foreach (var line in lines)
{
	if (!string.IsNullOrWhiteSpace(line))
	{
		grid.Add(line.Trim());
	}
}

int rows = grid.Count;
int cols = grid[0].Length;

(int r, int c) start = (0, 0);
(int r, int c) end = (0, 0);

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (grid[r][c] == 'S')
		{
			start = (r, c);
		}
		else if (grid[r][c] == 'E')
		{
			end = (r, c);
		}
	}
}

var directions = new (int, int)[]
{
	(0, 1),
	(0, -1),
	(1, 0),
	(-1, 0)
};

int solveTime = Solve(-1, -1, -1, -1);

int goodCheats = 0;

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (grid[r][c] == '#')
		{
			foreach (var (dx, dy) in directions)
			{
				int newRow = r + dx;
				int newCol = c + dy;

				if (newRow >= 0 &&
					newRow < rows &&
					newCol >= 0 &&
					newCol < cols &&
					(grid[newRow][newCol] == '.' || grid[newRow][newCol] == 'E'))
				{
					int newSolveTime = Solve(r, c, newRow, newCol);
					if (newSolveTime <= solveTime - 100)
					{
						goodCheats++;
					}
				}
			}
		}
	}
}

Console.WriteLine(goodCheats);


// Part 2
var solutions = Solve2();

goodCheats = 0;

for (int i = 0; i < solutions.Count; i++)
{
	for (int j = i + 1; j < solutions.Count; j++)
	{
		int cheatDistance = Math.Abs(solutions[i].row - solutions[j].row) + Math.Abs(solutions[i].col - solutions[j].col);

		if (cheatDistance > 20)
		{
			continue;
		}

		int newPath = i + cheatDistance + solutions.Count - j - 1;

		if (newPath <= solutions.Count - 1 - 100)
		{
			goodCheats++;
		}
	}
}

Console.WriteLine(goodCheats);


int Solve(int startRow, int startCol, int endRow, int endCol)
{
	var visited = new HashSet<(int, int, bool)>();
	var queue = new Queue<(int, int, bool, int)>();

	void AddNode(int r, int c, bool isUsed, int distance)
	{
		if (visited.Contains((r, c, isUsed)))
		{
			return;
		}

		visited.Add((r, c, isUsed));
		queue.Enqueue((r, c, isUsed, distance));
	}

	AddNode(start.r, start.c, false, 0);

	while (queue.Count > 0)
	{
		var (currentRow, currentCol, isUsed, distance) = queue.Dequeue();

		if (currentRow == end.r &&
			currentCol == end.c)
		{
			return distance;
		}

		if (currentRow == startRow &&
			currentCol == startCol)
		{
			AddNode(endRow, endCol, isUsed, distance + 1);
		}
		else
		{
			foreach (var (dx, dy) in directions)
			{
				int newRow = currentRow + dx;
				int newCol = currentCol + dy;

				if (newRow >= 0 &&
					newRow < rows &&
					newCol >= 0 &&
					newCol < cols &&
					(grid[newRow][newCol] == '.' || grid[newRow][newCol] == 'E' || (newRow == startRow && newCol == startCol && !isUsed)))
				{
					bool newIsUsed = newRow == startRow &&
						newCol == startCol
						&& !isUsed;

					AddNode(newRow, newCol, newIsUsed, distance + 1);
				}
			}
		}
	}

	return int.MaxValue;
}

List<(int row, int col)> Solve2()
{
	var visited = new HashSet<(int, int)>();
	var queue = new Queue<(int, int, int)>();
	var prev = new Dictionary<(int, int), (int, int)>();

	void AddNode(int row, int col, int distance, int? prevRow, int? prevCol)
	{
		if (visited.Contains((row, col)))
		{
			return;
		}

		if (prevRow.HasValue && prevCol.HasValue)
		{
			prev[(row, col)] = (prevRow.Value, prevCol.Value);
		}

		visited.Add((row, col));
		queue.Enqueue((row, col, distance));
	}

	AddNode(start.r, start.c, 0, null, null);

	while (queue.Count > 0)
	{
		var (currentRow, currentCol, distance) = queue.Dequeue();

		if (currentRow == end.r &&
			currentCol == end.c)
		{
			var path = new List<(int, int)> { (currentRow, currentCol) };

			while (prev.ContainsKey(path[^1]))
			{
				path.Add(prev[path[^1]]);
			}

			return path;
		}

		foreach (var (dx, dy) in directions)
		{
			int newRow = currentRow + dx;
			int newCol = currentCol + dy;

			if (newRow >= 0 &&
				newRow < rows &&
				newCol >= 0 &&
				newCol < cols &&
				(grid[newRow][newCol] == '.' || grid[newRow][newCol] == 'E'))
			{
				AddNode(newRow, newCol, distance + 1, currentRow, currentCol);
			}
		}
	}

	return new List<(int, int)>();
}