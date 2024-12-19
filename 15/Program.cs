using System.Text;

string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var moves = new StringBuilder();
List<char[]> map = new List<char[]>();

foreach (string line in lines)
{
	if (line.StartsWith('#'))
	{
		map.Add(line.Trim().ToCharArray());
	}
	else if (!string.IsNullOrEmpty(line))
	{
		moves.Append(line.Trim());
	}
}

int rows = map.Count;
int cols = map[0].Length;

(int row, int col) currentPosition = (0, 0);
for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (map[r][c] == '@')
		{
			currentPosition = (r, c);
			break;
		}
	}
}

Dictionary<char, (int row, int col)> directions = new Dictionary<char, (int, int)>
{
	{ '>', (0, 1) },
	{ '<', (0, -1) },
	{ '^', (-1, 0) },
	{ 'v', (1, 0) }
};

foreach (var move in moves.ToString())
{
	var direction = directions[move];
	(int row, int col) newPosition = (currentPosition.row + direction.row, currentPosition.col + direction.col);
	var finalPosition = newPosition;

	while (map[finalPosition.row][finalPosition.col] != '.' &&
		map[finalPosition.row][finalPosition.col] != '#')
	{
		finalPosition = (finalPosition.row + direction.row, finalPosition.col + direction.col);
	}

	if (map[finalPosition.row][finalPosition.col] != '#')
	{
		map[finalPosition.row][finalPosition.col] = map[newPosition.row][newPosition.col];
		map[newPosition.row][newPosition.col] = '@';
		map[currentPosition.row][currentPosition.col] = '.';

		currentPosition = newPosition;
	}
}

int sum = 0;

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (map[r][c] == 'O')
		{
			sum += 100 * r + c;
		}
	}
}

Console.WriteLine(sum);


// Part 2
map = new List<char[]>();

foreach (string line in lines)
{
	if (line.StartsWith('#'))
	{
		map.Add(line.Trim().ToCharArray());
	}
}

List<List<char>> newMap = new List<List<char>>();
for (int r = 0; r < rows; r++)
{
	newMap.Add(new List<char>());
	for (int c = 0; c < cols; c++)
	{
		if (map[r][c] == '#')
		{
			newMap[r].Add('#');
			newMap[r].Add('#');
		}
		else if (map[r][c] == 'O')
		{
			newMap[r].Add('[');
			newMap[r].Add(']');
		}
		else if (map[r][c] == '.')
		{
			newMap[r].Add('.');
			newMap[r].Add('.');
		}
		else if (map[r][c] == '@')
		{
			currentPosition = (r, newMap[r].Count);
			newMap[r].Add('@');
			newMap[r].Add('.');
		}
	}
}

rows = newMap.Count;
cols = newMap[0].Count;

foreach (var move in moves.ToString())
{
	var direction = directions[move];
	(int row, int col) newPosition = (currentPosition.row + direction.row, currentPosition.col + direction.col);

	if (direction.row == 0 ||
		newMap[newPosition.row][newPosition.col] == '.')
	{
		var finalPostion = newPosition;

		while (newMap[finalPostion.row][finalPostion.col] != '.' &&
			newMap[finalPostion.row][finalPostion.col] != '#')
		{
			finalPostion = (finalPostion.row + direction.row, finalPostion.col + direction.col);
		}

		if (newMap[finalPostion.row][finalPostion.col] != '#')
		{
			while ((finalPostion.row - direction.row, finalPostion.col - direction.col) != currentPosition)
			{
				newMap[finalPostion.row][finalPostion.col] = newMap[finalPostion.row - direction.row][finalPostion.col - direction.col];
				finalPostion = (finalPostion.row - direction.row, finalPostion.col - direction.col);
			}

			newMap[newPosition.row][newPosition.col] = '@';
			newMap[currentPosition.row][currentPosition.col] = '.';

			currentPosition = newPosition;
		}
	}
	else if (newMap[newPosition.row][newPosition.col] == '#')
	{
		continue;
	}
	else
	{
		List<HashSet<(int row, int col)>> neighbours = new List<HashSet<(int, int)>>
		{
			Neighbour(newPosition.row, newPosition.col)
		};
		bool isBad = false;

		while (neighbours[^1].Count > 0)
		{
			HashSet<(int row, int col)> newNeighbour = new HashSet<(int row, int col)>();
			foreach (var (r, c) in neighbours[^1])
			{
				if (newMap[r + direction.row][c] == '#')
				{
					isBad = true;
					break;
				}

				foreach (var (r2, c2) in Neighbour(r + direction.row, c))
				{
					newNeighbour.Add((r2, c2));
				}
			}

			if (isBad)
			{
				break;
			}

			neighbours.Add(newNeighbour);
		}

		if (!isBad)
		{
			for (int i = neighbours.Count - 1; i >= 0; i--)
			{
				foreach (var (r, c) in neighbours[i])
				{
					newMap[r + direction.row][c] = newMap[r][c];
					newMap[r][c] = '.';
				}
			}

			newMap[newPosition.row][newPosition.col] = '@';
			newMap[currentPosition.row][currentPosition.col] = '.';

			currentPosition = newPosition;
		}
	}
}

int sum2 = 0;

for (int r = 0; r < rows; r++)
{
	for (int c = 0; c < cols; c++)
	{
		if (newMap[r][c] == '[')
		{
			sum2 += 100 * r + c;
		}
	}
}

Console.WriteLine(sum2);


HashSet<(int row, int col)> Neighbour(int row, int col)
{
	if (newMap[row][col] == '.')
	{
		return new HashSet<(int row, int col)>();
	}
	else if (newMap[row][col] == '[')
	{
		return new HashSet<(int row, int col)> { (row, col), (row, col + 1) };
	}
	else if (newMap[row][col] == ']')
	{
		return new HashSet<(int row, int col)> { (row, col), (row, col - 1) };
	}

	return new HashSet<(int row, int col)>();
}