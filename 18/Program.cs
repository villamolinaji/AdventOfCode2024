string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

const int xyMax = 71;
bool[,] grid = new bool[xyMax, xyMax];
var points = new List<(int x, int y)>();
int soFar = 0;

foreach (var line in lines)
{
	string[] parts = line.Split(',');
	int x = int.Parse(parts[0]);
	int y = int.Parse(parts[1]);

	points.Add((x, y));

	soFar++;
	if (soFar <= 1024)
	{
		grid[x, y] = true;
	}
}

var directions = new (int, int)[]
{
	(0, 1),
	(0, -1),
	(1, 0),
	(-1, 0) };

var result = 0;

Dictionary<(int, int), int> distances = new Dictionary<(int, int), int>();
var queue = new Queue<(int, int)>();

(int x, int y) start = (0, 0);
queue.Enqueue(start);
distances[start] = 0;

while (queue.Count > 0)
{
	var (x, y) = queue.Dequeue();

	if (x == xyMax - 1 &&
		y == xyMax - 1)
	{
		result = distances[(x, y)];

		break;
	}

	foreach (var (dx, dy) in directions)
	{
		int newX = x + dx;
		int newY = y + dy;

		if (newX >= 0 &&
			newX < xyMax &&
			newY >= 0 &&
			newY < xyMax &&
			!grid[newX, newY] &&
			!distances.ContainsKey((newX, newY)))
		{
			distances[(newX, newY)] = distances[(x, y)] + 1;
			queue.Enqueue((newX, newY));
		}
	}
}

Console.WriteLine(result);

// Part 2
int limit = 0;

while (true)
{
	grid[points[limit].x, points[limit].y] = true;
	limit++;

	distances = new Dictionary<(int, int), int>();
	queue = new Queue<(int, int)>();

	start = (0, 0);
	queue.Enqueue(start);
	distances[start] = 0;

	bool isGood = false;

	while (queue.Count > 0)
	{
		var (x, y) = queue.Dequeue();

		if (x == xyMax - 1 &&
			y == xyMax - 1)
		{
			isGood = true;
			break;
		}

		foreach (var (dx, dy) in directions)
		{
			int newX = x + dx;
			int newY = y + dy;

			if (newX >= 0 &&
				newX < xyMax &&
				newY >= 0 &&
				newY < xyMax &&
				!grid[newX, newY] &&
				!distances.ContainsKey((newX, newY)))
			{
				distances[(newX, newY)] = distances[(x, y)] + 1;
				queue.Enqueue((newX, newY));
			}
		}
	}

	if (!isGood)
	{
		Console.WriteLine($"{points[limit - 1].x},{points[limit - 1].y}");
		break;
	}
}
