string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

int rows = lines.Length;
int cols = lines[0].Length;
string xmas = "XMAS";
int wordLength = xmas.Length;
int count = 0;
int count2 = 0;

int[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
int[] dy = { 1, 1, 0, -1, -1, -1, 0, 1 };

for (int row = 0; row < rows; row++)
{
	for (int col = 0; col < cols; col++)
	{
		for (int dir = 0; dir < 8; dir++)
		{
			if (Find(row, col, dir))
			{
				count++;
			}
		}

		if (Find2(row, col))
		{
			count2++;
		}
	}
}

Console.WriteLine(count);

Console.WriteLine(count2);


bool Find(int row, int col, int dir)
{
	for (int i = 0; i < wordLength; i++)
	{
		int newRow = row + i * dx[dir];
		int newCol = col + i * dy[dir];

		if (newRow < 0 ||
			newRow >= rows ||
			newCol < 0 ||
			newCol >= cols ||
			lines[newRow][newCol] != xmas[i])
		{
			return false;
		}
	}

	return true;
}


bool Find2(int row, int col)
{
	if (lines[row][col] != 'A')
	{
		return false;
	}

	if (row == 0 ||
		row == rows - 1 ||
		col == 0 ||
		col == cols - 1)
	{
		return false;
	}

	bool isMas = lines[row - 1][col - 1] == 'M' &&
		lines[row + 1][col + 1] == 'S';

	bool isSam = lines[row - 1][col - 1] == 'S' &&
		lines[row + 1][col + 1] == 'M';

	bool isMas2 = lines[row + 1][col - 1] == 'M' &&
		lines[row - 1][col + 1] == 'S';

	bool isSam2 = lines[row + 1][col - 1] == 'S' &&
		lines[row - 1][col + 1] == 'M';

	return (isMas || isSam) &&
		(isMas2 || isSam2);
}
