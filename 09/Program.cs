string input = File.ReadAllTextAsync("Input.txt").Result;

var diskMap = new List<(int id, int blocks)>();
bool isBlockFile = true;
int currentId = 0;
int? freeIndex = null;

foreach (char ch in input)
{
	int x = int.Parse(ch.ToString());

	for (int i = 0; i < x; i++)
	{
		if (isBlockFile)
		{
			diskMap.Add((currentId, x));
		}
		else
		{
			if (freeIndex == null)
			{
				freeIndex = diskMap.Count;
			}

			diskMap.Add((-1, x));
		}
	}

	if (isBlockFile)
	{
		currentId++;
	}

	isBlockFile = !isBlockFile;
}

while (freeIndex < diskMap.Count)
{
	while (diskMap[diskMap.Count - 1].id == -1)
	{
		diskMap.RemoveAt(diskMap.Count - 1);
	}

	if (freeIndex >= diskMap.Count)
	{
		break;
	}

	diskMap[(int)freeIndex] = diskMap[diskMap.Count - 1];
	diskMap.RemoveAt(diskMap.Count - 1);

	while (freeIndex < diskMap.Count &&
		diskMap[(int)freeIndex].id != -1)
	{
		freeIndex++;
	}
}

long result = 0;
for (int i = 0; i < diskMap.Count; i++)
{
	if (diskMap[i].id != -1)
	{
		result += i * diskMap[i].id;
	}
}

Console.WriteLine(result);


// Part 2
diskMap = new List<(int, int)>();
currentId = 0;
isBlockFile = true;
int? firstGap = null;

foreach (char ch in input)
{
	int x = int.Parse(ch.ToString());
	if (isBlockFile)
	{
		diskMap.Add((currentId, x));
	}
	else
	{
		if (firstGap == null)
		{
			firstGap = diskMap.Count;
		}
		diskMap.Add((-1, x));
	}

	if (isBlockFile)
	{
		currentId++;
	}

	isBlockFile = !isBlockFile;
}

int idToMove = currentId - 1;
while (idToMove >= 0)
{
	int topIndex = diskMap.Count - 1;
	while (diskMap[topIndex].id != idToMove)
	{
		topIndex--;
	}

	for (int i = 0; i < topIndex; i++)
	{
		if (diskMap[i].id == -1 &&
			diskMap[i].blocks >= diskMap[topIndex].blocks)
		{
			int spaceLeft = diskMap[i].blocks - diskMap[topIndex].blocks;
			var newDiskMap = new List<(int, int)>();

			newDiskMap.AddRange(diskMap.GetRange(0, i));
			newDiskMap.Add(diskMap[topIndex]);
			newDiskMap.Add((-1, spaceLeft));
			newDiskMap.AddRange(diskMap.GetRange(i + 1, topIndex - i - 1));
			newDiskMap.Add((-1, diskMap[topIndex].Item2));
			newDiskMap.AddRange(diskMap.GetRange(topIndex + 1, diskMap.Count - topIndex - 1));

			diskMap = newDiskMap;

			break;
		}
	}

	idToMove--;

	var updatedDiskMap = new List<(int, int)>();
	int curSpace = 0;

	foreach (var (id, blocks) in diskMap)
	{
		if (id != -1)
		{
			if (curSpace > 0)
			{
				updatedDiskMap.Add((-1, curSpace));
			}

			curSpace = 0;
			updatedDiskMap.Add((id, blocks));
		}
		else
		{
			curSpace += blocks;
		}
	}

	diskMap = updatedDiskMap;
}

long result2 = 0;
int loc = 0;

foreach (var (id, blocks) in diskMap)
{
	if (id != -1)
	{
		for (int x = 0; x < blocks; x++)
		{
			result2 += loc * id;
			loc++;
		}
	}
	else
	{
		loc += blocks;
	}
}

Console.WriteLine(result2);