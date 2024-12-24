using System.Collections.Concurrent;

string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

Dictionary<(int x, int y), char> numericPad = new()
{
	[(0, 0)] = '7',
	[(1, 0)] = '8',
	[(2, 0)] = '9',
	[(0, -1)] = '4',
	[(1, -1)] = '5',
	[(2, -1)] = '6',
	[(0, -2)] = '1',
	[(1, -2)] = '2',
	[(2, -2)] = '3',
	[(0, -3)] = ' ',
	[(1, -3)] = '0',
	[(2, -3)] = 'A'
};

Dictionary<(int x, int y), char> directionalPad = new()
{
	[(0, 0)] = ' ',
	[(1, 0)] = '^',
	[(2, 0)] = 'A',
	[(0, -1)] = '<',
	[(1, -1)] = 'v',
	[(2, -1)] = '>'
};

ConcurrentDictionary<(char currentKey, char nextKey, int depth), long> cache = new ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>();

var pads = Enumerable.Repeat(directionalPad, 2).Prepend(numericPad).ToArray();

var total = 0L;

foreach (var line in lines)
{
	var number = int.Parse(line[..^1]);

	total += number * EncodeKeys(line, pads);
}

Console.WriteLine(total);


// Part 2
cache = new ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>();

pads = Enumerable.Repeat(directionalPad, 25).Prepend(numericPad).ToArray();

total = 0L;

foreach (var line in lines)
{
	var number = int.Parse(line[..^1]);

	total += number * EncodeKeys(line, pads);
}

Console.WriteLine(total);


long EncodeKeys(string keys, Dictionary<(int x, int y), char>[] pads)
{
	if (pads.Length == 0)
	{
		return keys.Length;
	}
	else
	{
		var currentKey = 'A';
		var length = 0L;

		foreach (var nextKey in keys)
		{
			length += EncodeKey(currentKey, nextKey, pads, cache);
			currentKey = nextKey;
		}

		return length;
	}
}

long EncodeKey(char currentKey, char nextKey, Dictionary<(int x, int y), char>[] keypads, ConcurrentDictionary<(char currentKey, char nextKey, int depth), long> cache) =>
	cache.GetOrAdd((currentKey, nextKey, keypads.Length), _ =>
	{
		var keypad = keypads[0];

		var currentPos = keypad.Single(kvp => kvp.Value == currentKey).Key;
		var nextPos = keypad.Single(kvp => kvp.Value == nextKey).Key;

		var dy = nextPos.y - currentPos.y;
		var vert = new string(dy < 0 ? 'v' : '^', Math.Abs(dy));

		var dx = nextPos.x - currentPos.x;
		var horiz = new string(dx < 0 ? '<' : '>', Math.Abs(dx));

		var cost = long.MaxValue;

		if (keypad[(currentPos.x, nextPos.y)] != ' ')
		{
			cost = Math.Min(cost, EncodeKeys($"{vert}{horiz}A", keypads[1..]));
		}

		if (keypad[(nextPos.x, currentPos.y)] != ' ')
		{
			cost = Math.Min(cost, EncodeKeys($"{horiz}{vert}A", keypads[1..]));
		}

		return cost;
	});