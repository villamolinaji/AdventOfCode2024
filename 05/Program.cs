string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var rules = new List<string>();
var updates = new List<string>();

foreach (var line in lines)
{
	if (line.Contains('|'))
	{
		rules.Add(line);
	}
	else if (line.Contains(','))
	{
		updates.Add(line);
	}
}

var orderingRules = new HashSet<(int Before, int After)>();
foreach (var rule in rules)
{
	var rulePages = rule
		.Split('|')
		.Select(int.Parse)
		.ToArray();

	orderingRules.Add((rulePages[0], rulePages[1]));
}

int total = 0;
int total2 = 0;

foreach (var update in updates)
{
	var updatePages = update
		.Split(',')
		.Select(int.Parse)
		.ToList();

	if (IsOrdered(updatePages))
	{
		int middlePage = updatePages[updatePages.Count / 2];

		total += middlePage;
	}
	else
	{
		var sortUpdatePages = Sort(updatePages);

		int middlePage = sortUpdatePages[sortUpdatePages.Count / 2];

		total2 += middlePage;
	}
}

Console.WriteLine(total);
Console.WriteLine(total2);


bool IsOrdered(List<int> pages)
{
	var pageDictionary = pages
		.Select((page, index) => (page, index))
		.ToDictionary(x => x.page, x => x.index);

	foreach (var (before, after) in orderingRules)
	{
		if (pageDictionary.ContainsKey(before) &&
			pageDictionary.ContainsKey(after) &&
			pageDictionary[before] >= pageDictionary[after])
		{
			return false;
		}
	}

	return true;
}

List<int> Sort(List<int> pages)
{
	var neighbors = new Dictionary<int, List<int>>();
	var inDegree = new Dictionary<int, int>();

	foreach (var page in pages)
	{
		neighbors[page] = new List<int>();
		inDegree[page] = 0;
	}

	foreach (var (before, after) in orderingRules)
	{
		if (neighbors.ContainsKey(before) &&
			neighbors.ContainsKey(after))
		{
			neighbors[before].Add(after);
			inDegree[after]++;
		}
	}

	var queue = new Queue<int>(pages.Where(page => inDegree[page] == 0));
	var sortedPages = new List<int>();

	while (queue.Count > 0)
	{
		var currentPage = queue.Dequeue();
		sortedPages.Add(currentPage);

		foreach (var neighbor in neighbors[currentPage])
		{
			inDegree[neighbor]--;

			if (inDegree[neighbor] == 0)
			{
				queue.Enqueue(neighbor);
			}
		}
	}

	return sortedPages;
}