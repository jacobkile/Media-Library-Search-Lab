﻿using NLog;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");
string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

Console.WriteLine("Enter 1 to see all movies on file.");
Console.WriteLine("Enter 2 to add movies to the file.");
Console.WriteLine("Enter anything else to quit.");

string? resp = Console.ReadLine();

if (resp == "1")
{
    using (StreamReader sr = new StreamReader("movies.scrubbed.csv"))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                foreach (string field in fields)
                {
                    Console.Write(field + "\n");
                }
                Console.WriteLine();
            }
        }
    
}
else if (resp == "2")
{
    int x = 0;
    bool validInput = false;

    while (!validInput)
        {
        Console.Write("Please enter a movie ID (a number): ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out x))
        {
        validInput = true;
        }
        else
        {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    Console.Write("What is the movie Title?: ");
    string title = Console.ReadLine();

    Console.Write("What genres is the movie?: ");
    string genres = Console.ReadLine();
    genres = genres.Replace(" ", "|");

    Console.Write("Enter movie Director:");
    string director = Console.ReadLine();

    Console.Write("Enter running time (h:m:s):");
    var runtime = Console.ReadLine();

    using (StreamWriter sw = new StreamWriter("movies.scrubbed.csv", true))
    {
        sw.WriteLine("{0},{1},{2},{3},{4}", x, title, genres, director, runtime);

        logger.Info("Inserted the movie {title} at {now}",title, DateTime.Now);
    }
}
else if (resp == "3")
{

}

logger.Info("Program ended");

