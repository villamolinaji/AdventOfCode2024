string[] lines = File.ReadAllLinesAsync("input.txt").Result;

var buffer = new List<string>();
var locks = new List<int[]>();
var keys = new List<int[]>();
int result = 0;

foreach (string line in lines)
{
	if (string.IsNullOrWhiteSpace(line))
	{
		ProcessBuffer(buffer);
		buffer.Clear();

		continue;
	}

	buffer.Add(line.Trim());
}

ProcessBuffer(buffer);

foreach (var key in keys)
{
	foreach (var lockHeights in locks)
	{
		bool isGood = true;

		for (int i = 0; i < key.Length; i++)
		{
			if (key[i] + lockHeights[i] > 5)
			{
				isGood = false;
				break;
			}
		}

		if (isGood)
		{
			result++;
		}
	}
}

Console.WriteLine(result);


void ProcessBuffer(List<string> buffer)
{
	if (buffer.Count > 0)
	{
		int[] heights = new int[buffer[0].Length];

		if (buffer[0] == "#####")
		{
			for (int j = 0; j < buffer[0].Length; j++)
			{
				int height = buffer.Count;

				for (int i = 0; i < buffer.Count; i++)
				{
					if (buffer[i][j] == '.')
					{
						height = i - 1;

						break;
					}
				}

				heights[j] = height;
			}

			locks.Add(heights);
		}
		else
		{
			for (int j = 0; j < buffer[0].Length; j++)
			{
				int height = buffer.Count;

				for (int i = 0; i < buffer.Count; i++)
				{
					if (buffer[buffer.Count - 1 - i][j] == '.')
					{
						height = i - 1;
						break;
					}
				}

				heights[j] = height;
			}

			keys.Add(heights);
		}
	}
}
