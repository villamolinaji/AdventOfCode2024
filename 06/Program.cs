string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int rows = lines.Length;
int cols = lines[0].Length;

char[,] map = new char[rows, cols];
(int x, int y) guardStartPosition = (0, 0);
(int dx, int dy) guardStartDirection = (0, -1);

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		map[r, c] = lines[r][c];
		if ("^>v<".Contains(map[r, c]))
		{
			guardStartPosition = (r, c);
			guardStartDirection = map[r, c] switch
			{
				'^' => (-1, 0),
				'>' => (0, 1),
				'v' => (1, 0),
				'<' => (0, -1),
				_ => (0, 0)
			};
			map[r, c] = '.';
		}
	}
}

(int x, int y) currentPosition = guardStartPosition;
(int dx, int dy)[] directions =
{
	(-1, 0),
	(0, 1),
	(1, 0),
	(0, -1)
};
(int dx, int dy) guardDirection = guardStartDirection;
int directionIndex = Array.IndexOf(directions, guardDirection);

HashSet<(int, int)> visited = new HashSet<(int, int)>();
visited.Add(guardStartPosition);

while (true)
{
	(int x, int y) nextPosition = (currentPosition.x + guardDirection.dx, currentPosition.y + guardDirection.dy);

	if (nextPosition.x < 0 ||
		nextPosition.x >= rows ||
		nextPosition.y < 0 ||
		nextPosition.y >= cols)
	{
		break;
	}

	if (map[nextPosition.x, nextPosition.y] == '#')
	{
		directionIndex = (directionIndex + 1) % 4;
		guardDirection = directions[directionIndex];
	}
	else
	{
		currentPosition = nextPosition;
		visited.Add(currentPosition);
	}
}

Console.WriteLine(visited.Count);

// Part 2

int countObstruction = 0;

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (map[r, c] == '.' &&
			(r, c) != guardStartPosition)
		{
			map[r, c] = '#';

			if (CausesLoop())
			{
				countObstruction++;
			}

			map[r, c] = '.';
		}
	}
}

Console.WriteLine(countObstruction);


bool CausesLoop()
{
	(int x, int y) guardPosition = guardStartPosition;
	(int dx, int dy) direction = guardStartDirection;

	HashSet<((int x, int y), (int dx, int dy))> visitedPositionDirection = new();

	while (true)
	{
		var positionDirection = (guardPosition, direction);

		if (visitedPositionDirection.Contains(positionDirection))
		{
			return true;
		}

		visitedPositionDirection.Add(positionDirection);

		(int x, int y) nextPosition = (guardPosition.x + direction.dx, guardPosition.y + direction.dy);

		if (nextPosition.x < 0 ||
			nextPosition.x >= rows ||
			nextPosition.y < 0 ||
			nextPosition.y >= cols)
		{
			return false;
		}

		if (map[nextPosition.x, nextPosition.y] == '#')
		{
			int currentDirectionIndex = Array.IndexOf(directions, direction);
			direction = directions[(currentDirectionIndex + 1) % 4];
		}
		else
		{
			guardPosition = nextPosition;
		}
	}
}