string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var wireValues = new Dictionary<string, int>();
var instructions = new List<string>();
var rules = new List<Rule>();

foreach (var line in lines)
{
	if (line.Contains(":"))
	{
		var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
		wireValues[parts[0]] = int.Parse(parts[1]);
	}
	else if (!string.IsNullOrEmpty(line))
	{
		instructions.Add(line);

		var parts = line.Split(" ");
		rules.Add(new Rule(parts[0], parts[2], parts[1], parts[4]));
	}
}

bool updated;

do
{
	updated = false;

	foreach (var instr in instructions.ToList())
	{
		var parts = instr.Split(new[] { " -> " }, StringSplitOptions.None);
		var operation = parts[0];
		var output = parts[1];

		var tokens = operation.Split(' ');

		int GetValue(string wire) => wireValues.ContainsKey(wire) ? wireValues[wire] : -1;

		int gate1 = GetValue(tokens[0]);
		int gate2 = tokens.Length == 3
			? GetValue(tokens[2])
			: -1;

		if (gate1 == -1
			|| (tokens.Length == 3 && gate2 == -1))
		{
			continue;
		}

		int result = tokens[1] switch
		{
			"AND" => gate1 & gate2,
			"OR" => gate1 | gate2,
			"XOR" => gate1 ^ gate2,
			_ => 0
		};

		wireValues[output] = result;

		instructions.Remove(instr);

		updated = true;
	}
} while (updated);


var zWires = wireValues
	.Where(kv => kv.Key.StartsWith('z'))
	.OrderBy(kv => kv.Key)
	.Select(kv => kv.Value);

var binaryString = string.Concat(zWires.Reverse().Select(b => b.ToString()));

var decimalValue = Convert.ToInt64(binaryString, 2);

Console.WriteLine(decimalValue);


// Part 2
var swaps = GetSwaps(rules);

Console.WriteLine(string.Join(",", swaps.OrderBy(x => x)));


IEnumerable<string> GetSwaps(List<Rule> rules)
{
	var cin = Output(rules, "x00", "AND", "y00");

	for (var i = 1; i < 45; i++)
	{
		var x = $"x{i:D2}";
		var y = $"y{i:D2}";
		var z = $"z{i:D2}";

		var xor1 = Output(rules, x, "XOR", y);
		var and1 = Output(rules, x, "AND", y);

		var and2 = Output(rules, cin, "AND", xor1);
		var xor2 = Output(rules, cin, "XOR", xor1);

		if (xor2 == null &&
			and2 == null)
		{
			return Swap(rules, xor1, and1);
		}

		var carry = Output(rules, and1, "OR", and2);

		if (xor2 != z)
		{
			return Swap(rules, z, xor2);
		}
		else
		{
			cin = carry;
		}
	}

	return Array.Empty<string>();
}

string Output(IEnumerable<Rule> rules, string x, string gate, string y)
{
	return rules
		.SingleOrDefault(rule =>
			(rule.In1 == x && rule.Kind == gate && rule.In2 == y) ||
			(rule.In1 == y && rule.Kind == gate && rule.In2 == x))
		.Output;
}

IEnumerable<string> Swap(List<Rule> rules, string out1, string out2)
{
	rules = rules
		.Select(rule =>
			rule.Output == out1
				? rule with { Output = out2 }
				: rule.Output == out2
					? rule with { Output = out1 }
					: rule)
		.ToList();

	return GetSwaps(rules).Concat(new[] { out1, out2 });
}

record struct Rule(string In1, string In2, string Kind, string Output);
