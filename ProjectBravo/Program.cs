﻿﻿public class Program
{
    public static void Main(string[] args)
    {
        if (args[0] == "frequency")
        {
            GitInsights.PrintFrequencies(args[1]);
        }
        if (args[0] == "author")
        {
            GitInsights.PrintAuthors(args[1]);
        }
    }
}

