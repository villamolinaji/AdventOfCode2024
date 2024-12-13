string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var machines = new List<Machine>();

for (int i = 0; i < lines.Length; i += 4)
{
	if (i + 2 >= lines.Length)
	{
		break;
	}

	var lineA = lines[i].Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
	var lineB = lines[i + 1].Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
	var linePrize = lines[i + 2].Split(new[] { ' ', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

	var machine = new Machine
	{
		Ax = long.Parse(lineA[2].Replace("X+", "")),
		Ay = long.Parse(lineA[3].Replace("Y+", "")),
		Bx = long.Parse(lineB[2].Replace("X+", "")),
		By = long.Parse(lineB[3].Replace("Y+", "")),
		Px = long.Parse(linePrize[2]),
		Py = long.Parse(linePrize[4])
	};

	machines.Add(machine);
}

long result = 0;

foreach (var machine in machines)
{
	(bool canWin, long cost) = SolveMachine(machine);
	if (canWin)
	{
		result += cost;
	}
}

Console.WriteLine(result);

// Part 2
long result2 = 0;

foreach (var machine in machines)
{
	machine.Px += 10000000000000;
	machine.Py += 10000000000000;

	long cost = SolveMachine2(machine);

	result2 += cost;
}

Console.WriteLine(result2);


(bool, long) SolveMachine(Machine machine)
{
	long minCost = long.MaxValue;
	bool canWin = false;

	for (int i = 0; i <= 100; i++)
	{
		for (int j = 0; j <= 100; j++)
		{
			long x = i * machine.Ax + j * machine.Bx;
			long y = i * machine.Ay + j * machine.By;

			if (x == machine.Px && y == machine.Py)
			{
				int cost = i * 3 + j;

				if (cost < minCost)
				{
					minCost = cost;
					canWin = true;
				}
			}
		}
	}

	return (canWin, minCost);
}

long SolveMachine2(Machine machine)
{
	var det = machine.Ax * machine.By - machine.Ay * machine.Bx;

	if (det == 0)
	{
		return 0;
	}

	var pressA = (double)(machine.Px * machine.By - machine.Py * machine.Bx) / det;
	var pressB = (double)(machine.Ax * machine.Py - machine.Ay * machine.Px) / det;

	if (pressA >= 0.0 &&
		pressB >= 0.0 &&
		Math.Abs(pressA - Math.Round(pressA)) < 1e-10 &&
		Math.Abs(pressB - Math.Round(pressB)) < 1e-10)
	{
		long a = (long)Math.Round(pressA);
		long b = (long)Math.Round(pressB);

		if (a * machine.Ax + b * machine.Bx == machine.Px &&
			a * machine.Ay + b * machine.By == machine.Py)
		{
			return a * 3 + b;
		}
	}

	return 0;
}

class Machine
{
	public long Ax, Ay, Bx, By, Px, Py;
}
