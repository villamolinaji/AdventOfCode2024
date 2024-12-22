string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

List<int> initialSecrets = new List<int>();
long sum = 0;

foreach (var line in lines)
{
	int secret = int.Parse(line);

	initialSecrets.Add(secret);

	sum += GetSecret(secret);
}

Console.WriteLine(sum);


// Part 2
int bananas = 0;
int maxBananas = 0;
var secretsDictionary = new List<Dictionary<(int?, int?, int?, int?), int>>();

for (int i = 0; i < initialSecrets.Count; i++)
{
	secretsDictionary.Add(new Dictionary<(int?, int?, int?, int?), int>());
}

for (int i = 0; i < initialSecrets.Count; i++)
{
	int secret = initialSecrets[i];
	(int?, int?, int?, int?) changes = (null, null, null, null);

	for (int l = 0; l < 2000; l++)
	{
		int currentSecret = secret;

		secret = GenerateNextSecret(secret);

		changes = (changes.Item2, changes.Item3, changes.Item4, (secret % 10) - (currentSecret % 10));

		if (!secretsDictionary[i].ContainsKey(changes))
		{
			secretsDictionary[i][changes] = secret % 10;
		}
	}
}

for (int x = -10; x < 10; x++)
{
	for (int x2 = -10; x2 < 10; x2++)
	{
		for (int x3 = -10; x3 < 10; x3++)
		{
			for (int x4 = -10; x4 < 10; x4++)
			{
				bananas = secretsDictionary
					.Where(sd => sd.ContainsKey((x, x2, x3, x4)))
					.Sum(sd => sd[(x, x2, x3, x4)]);

				maxBananas = Math.Max(maxBananas, bananas);
			}
		}
	}
}


Console.WriteLine(maxBananas);


int GetSecret(int initialSecret)
{
	int secret = initialSecret;

	for (int i = 0; i < 2000; i++)
	{
		secret = GenerateNextSecret(secret);
	}

	return secret;
}

int GenerateNextSecret(int secret)
{
	secret = MixAndPrune(secret, (long)secret * 64);

	secret = MixAndPrune(secret, secret / 32);

	secret = MixAndPrune(secret, (long)secret * 2048);

	return secret;
}

int MixAndPrune(int secret, long value)
{
	return (int)((secret ^ value) % 16777216);
}
