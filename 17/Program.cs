string[] lines = File.ReadAllLinesAsync("Input.txt").Result;

var registers = new List<long>();
var instructions = new List<long>();

foreach (var line in lines)
{
	if (line.StartsWith("Register"))
	{
		registers.Add(long.Parse(line.Split(": ")[1]));
	}
	else if (!string.IsNullOrEmpty(line))
	{
		instructions = new List<long>(Array.ConvertAll(line.Split(": ")[1].Split(","), long.Parse));
	}
}

var result = Run(registers);

Console.WriteLine(string.Join(",", result));


// Part 2
List<long> validsA = new List<long> { 0 };

for (int i = 0; i < instructions.Count; i++)
{
	var currentValidsA = validsA.Select(a => 8 * a).ToList();

	List<long> possA = new List<long>();

	foreach (var currentA in currentValidsA)
	{
		long a = currentA;

		for (int b = 0; b < 8; b++)
		{
			var newRegisters = new List<long>();
			newRegisters.Add(a);
			newRegisters.Add(0);
			newRegisters.Add(0);
			var outputs = Run(newRegisters);

			if (outputs.AsEnumerable().Reverse().Take(i + 1).SequenceEqual(instructions.AsEnumerable().Reverse().Take(i + 1)))
			{
				possA.Add(a);
			}

			a += 1;
		}
	}

	validsA = possA;
}

Console.WriteLine(validsA.Min());


List<long> Run(List<long> originalRegisters)
{
	var copyRegisters = originalRegisters.ToList();
	List<long> result = new List<long>();
	int instructionIndex = 0;

	while (instructionIndex < instructions.Count)
	{
		long code = instructions[instructionIndex];
		long operation = instructions[instructionIndex + 1];
		long? operationValue = GetOperationValue(operation, copyRegisters);
		bool jumps = false;

		switch (code)
		{
			case 0:
				if (operationValue.HasValue)
				{
					copyRegisters[0] /= (long)Math.Pow(2, operationValue.Value);
				}

				break;
			case 1:
				copyRegisters[1] ^= operation;

				break;
			case 2:
				if (operationValue.HasValue)
				{
					copyRegisters[1] = operationValue.Value % 8;
				}

				break;
			case 3:
				if (copyRegisters[0] != 0)
				{
					instructionIndex = (int)operation;
					jumps = true;
				}

				break;
			case 4:
				copyRegisters[1] ^= copyRegisters[2];

				break;
			case 5:
				if (operationValue.HasValue)
				{
					result.Add(operationValue.Value % 8);
				}

				break;
			case 6:
				if (operationValue.HasValue)
				{
					copyRegisters[1] = copyRegisters[0] / (long)Math.Pow(2, operationValue.Value);
				}

				break;
			case 7:
				if (operationValue.HasValue)
				{
					copyRegisters[2] = copyRegisters[0] / (long)Math.Pow(2, operationValue.Value);
				}

				break;
		}

		if (!jumps)
		{
			instructionIndex += 2;
		}
	}

	return result;
}

long? GetOperationValue(long opValue, List<long> registers)
{
	if (opValue < 4)
	{
		return opValue;
	}

	return opValue < 7
		? registers[(int)opValue - 4]
		: (long?)null;
}