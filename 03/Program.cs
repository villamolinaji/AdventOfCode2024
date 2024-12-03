using System.Text.RegularExpressions;

string input = File.ReadAllTextAsync("Input.txt").Result;

string pattern = @"mul\(\s*(\d+)\s*,\s*(\d+)\s*\)";
Regex regex = new Regex(pattern);

int result = regex.Matches(input)
	.Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
	.Sum();

Console.WriteLine(result);


// Part 2
string doPattern = @"do\(\)";
string dontPattern = @"don't\(\)";

Regex doRegex = new Regex(doPattern);
Regex dontRegex = new Regex(dontPattern);

bool doMul = true;
result = 0;

int i = 0;
while (i < input.Length)
{
	Match dontMatch = dontRegex.Match(input, i);
	if (dontMatch.Success &&
		dontMatch.Index == i)
	{
		doMul = false;
		i += dontMatch.Length;

		continue;
	}

	Match doMatch = doRegex.Match(input, i);
	if (doMatch.Success &&
		doMatch.Index == i)
	{
		doMul = true;
		i += doMatch.Length;

		continue;
	}

	Match mulMatch = regex.Match(input, i);
	if (mulMatch.Success &&
		mulMatch.Index == i)
	{
		if (doMul)
		{
			int x = int.Parse(mulMatch.Groups[1].Value);
			int y = int.Parse(mulMatch.Groups[2].Value);
			result += x * y;
		}

		i += mulMatch.Length;

		continue;
	}

	i++;
}

Console.WriteLine(result);