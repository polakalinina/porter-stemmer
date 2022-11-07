using PorterStemmer;

while (true)
{
    Console.WriteLine("Введите слово");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Пустое слово нельзя");
        continue;
    }

    try
    {
        var result = Porter.TransformingWord(input);
        Console.WriteLine(result);
    }
    catch (Exception e)
    {
        Console.WriteLine("Что-то сломалось");
    }
}

