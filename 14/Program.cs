string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int width = 101;
int height = 103;

List<Robot> robots = lines.Select(line =>
{
	var parts = line.Split(new[] { ' ', '=', ',', 'v', 'p' }, StringSplitOptions.RemoveEmptyEntries);

	int px = int.Parse(parts[0]);
	int py = int.Parse(parts[1]);
	int vx = int.Parse(parts[2]);
	int vy = int.Parse(parts[3]);

	return new Robot()
	{
		Px = px,
		Py = py,
		Vx = vx,
		Vy = vy
	};
}).ToList();

for (int t = 0; t < 100; t++)
{
	for (int i = 0; i < robots.Count; i++)
	{
		var robot = robots[i];

		int newX = (robot.Px + robot.Vx) % width;
		int newY = (robot.Py + robot.Vy) % height;

		if (newX < 0)
		{
			newX += width;
		}

		if (newY < 0)
		{
			newY += height;
		}

		robots[i].Px = newX;
		robots[i].Py = newY;
	}
}

int midX = width / 2;
int midY = height / 2;

int q1 = 0;
int q2 = 0;
int q3 = 0;
int q4 = 0;

foreach (var robot in robots)
{
	if (robot.Px == midX ||
		robot.Py == midY)
	{
		continue;
	}

	if (robot.Px > midX && robot.Py < midY)
	{
		q1++;
	}
	else if (robot.Px < midX && robot.Py < midY)
	{
		q2++;
	}
	else if (robot.Px < midX && robot.Py > midY)
	{
		q3++;
	}
	else if (robot.Px > midX && robot.Py > midY)
	{
		q4++;
	}
}

int safetyFactor = q1 * q2 * q3 * q4;

Console.WriteLine(safetyFactor);


//Part 2
int seconds = 0;

robots = lines.Select(line =>
{
	var parts = line.Split(new[] { ' ', '=', ',', 'v', 'p' }, StringSplitOptions.RemoveEmptyEntries);

	int px = int.Parse(parts[0]);
	int py = int.Parse(parts[1]);
	int vx = int.Parse(parts[2]);
	int vy = int.Parse(parts[3]);

	return new Robot()
	{
		Px = px,
		Py = py,
		Vx = vx,
		Vy = vy
	};
}).ToList();

while (true)
{
	seconds++;

	for (int i = 0; i < robots.Count; i++)
	{
		var robot = robots[i];

		int newX = (robot.Px + robot.Vx) % width;
		int newY = (robot.Py + robot.Vy) % height;

		if (newX < 0)
		{
			newX += width;
		}

		if (newY < 0)
		{
			newY += height;
		}

		robots[i].Px = newX;
		robots[i].Py = newY;
	}

	char[][] map = new char[103][];

	for (int i = 0; i < 103; i++)
	{
		map[i] = new char[101];

		for (int j = 0; j < 101; j++)
		{
			map[i][j] = '.';
		}
	}

	foreach (var robot in robots)
	{
		map[robot.Py][robot.Px] = '#';
	}

	string mapString = string.Join("\n", Array.ConvertAll(map, row => new string(row)));

	if (mapString.Contains(new string('#', 10)))
	{
		break;
	}
}

Console.WriteLine(seconds);


class Robot
{
	public int Px, Py, Vx, Vy;
}