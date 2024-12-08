string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int rows = lines.Length;
int cols = lines[0].Length;
var antennas = new List<(int x, int y, char frequency)>();

for (int y = 0; y < rows; y++)
{
	for (int x = 0; x < cols; x++)
	{
		if (char.IsLetterOrDigit(lines[y][x]))
		{
			antennas.Add((x, y, lines[y][x]));
		}
	}
}

var antinodes = new HashSet<(int x, int y)>();

for (int i = 0; i < antennas.Count; i++)
{
	for (int j = 0; j < antennas.Count; j++)
	{
		if (i == j)
		{
			continue;
		}

		var (x1, y1, frequency1) = antennas[i];
		var (x2, y2, frequency2) = antennas[j];

		if (frequency1 == frequency2)
		{
			int dx = x2 - x1;
			int dy = y2 - y1;

			int newX = x1 - dx;
			int newY = y1 - dy;

			if (IsInside(newX, newY))
			{
				antinodes.Add((newX, newY));
			}

			newX = x2 + dx;
			newY = y2 + dy;

			if (IsInside(newX, newY))
			{
				antinodes.Add((newX, newY));
			}
		}
	}
}

Console.WriteLine(antinodes.Count);


// Part 2
antinodes = new HashSet<(int x, int y)>();

for (int i = 0; i < antennas.Count; i++)
{
	for (int j = 0; j < antennas.Count; j++)
	{
		if (i == j)
		{
			continue;
		}

		var (x1, y1, frequency1) = antennas[i];
		var (x2, y2, frequency2) = antennas[j];

		if (frequency1 == frequency2)
		{
			antinodes.Add((x1, y1));
			antinodes.Add((x2, y2));

			int index = 1;
			while (true)
			{
				var doNotBreak = false;

				int dx = (x2 - x1) * index;
				int dy = (y2 - y1) * index;

				int newX = x1 - dx;
				int newY = y1 - dy;

				if (IsInside(newX, newY))
				{
					antinodes.Add((newX, newY));
					doNotBreak = true;
				}

				newX = x2 + dx;
				newY = y2 + dy;

				if (IsInside(newX, newY))
				{
					antinodes.Add((newX, newY));
					doNotBreak = true;
				}

				if (!doNotBreak)
				{
					break;
				}

				index++;
			}
		}
	}
}

Console.WriteLine(antinodes.Count);


bool IsInside(int newX, int newY)
{
	return newX >= 0 &&
		newX < cols &&
		newY >= 0 &&
		newY < rows;
}