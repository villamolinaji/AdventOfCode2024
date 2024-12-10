string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var map = lines
	.Select(row => row.Select(ch => int.Parse(ch.ToString())).ToArray())
	.ToArray();

var rows = map.Length;
var cols = map[0].Length;

int[] dRow = { -1, 1, 0, 0 };
int[] dCol = { 0, 0, -1, 1 };

int total = 0;

for (int row = 0; row < rows; row++)
{
	for (int col = 0; col < cols; col++)
	{
		if (map[row][col] == 0)
		{
			total += CalculateScore(row, col);
		}
	}
}

Console.WriteLine(total);


// Part 2
var dp = new int[rows, cols];

for (int height = 9; height >= 0; height--)
{
	for (int row = 0; row < rows; row++)
	{
		for (int col = 0; col < cols; col++)
		{
			if (map[row][col] == height)
			{
				if (height == 9)
				{
					dp[row, col] = 1;
				}
				else
				{
					for (int i = 0; i < dRow.Length; i++)
					{
						int newRow = row + dRow[i];
						int newCol = col + dCol[i];

						if (newRow >= 0 &&
							newRow < rows &&
							newCol >= 0 &&
							newCol < cols &&
							map[newRow][newCol] == height + 1)
						{
							dp[row, col] += dp[newRow, newCol];
						}
					}
				}
			}
		}
	}
}

int total2 = 0;
for (int row = 0; row < rows; row++)
{
	for (int col = 0; col < cols; col++)
	{
		if (map[row][col] == 0)
		{
			total2 += dp[row, col];
		}
	}
}

Console.WriteLine(total2);


int CalculateScore(int startRow, int startCol)
{
	var isVisited = new bool[rows, cols];
	var queue = new Queue<(int row, int col, int height)>();

	queue.Enqueue((startRow, startCol, 0));
	isVisited[startRow, startCol] = true;

	var visitedTop = new HashSet<(int, int)>();

	while (queue.Count > 0)
	{
		var (currentRow, currentCol, currentHeight) = queue.Dequeue();

		if (map[currentRow][currentCol] == 9)
		{
			visitedTop.Add((currentRow, currentCol));

			continue;
		}

		for (int i = 0; i < dRow.Length; i++)
		{
			int newRow = currentRow + dRow[i];
			int newCol = currentCol + dCol[i];

			if (newRow >= 0 &&
				newRow < rows &&
				newCol >= 0 &&
				newCol < cols &&
				!isVisited[newRow, newCol] &&
				map[newRow][newCol] == currentHeight + 1)
			{
				queue.Enqueue((newRow, newCol, map[newRow][newCol]));

				isVisited[newRow, newCol] = true;
			}
		}
	}

	return visitedTop.Count;
}