using System;
using Humanizer;
using System.Globalization;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine(2.ToWords(new CultureInfo("fr")));
    }
}
