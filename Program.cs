using Microsoft.AspNetCore.Builder;
using NLog;

static void DisplayMovieDetails(int movieId, string title, string director, TimeSpan runtime, string genres)
{
    Console.WriteLine($"Id: {movieId}");
    Console.WriteLine($"Title: {title}");
    Console.WriteLine($"Director: {director}");
    Console.WriteLine($"Run time: {runtime:hh\\:mm\\:ss}");
    Console.WriteLine($"Genres: {genres}");
    Console.WriteLine();
}
static void SearchMoviesByTitle(string titleToSearch)
    {
        int matchCount = 0;
        Console.WriteLine("Search Results:");
    using StreamReader sr = new StreamReader("movies.scrubbed.csv");
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        string[] fields = line.Split(',');
        if (fields.Length == 5)
        {
            if (int.TryParse(fields[0], out int movieId))
            {
                string title = fields[1];
                string genres = fields[2];
                string director = fields[3];
                if (TimeSpan.TryParse(fields[4], out TimeSpan runtime) && title.ToLower().Contains(titleToSearch.ToLower()))
                {
                    matchCount++;
                    DisplayMovieDetails(movieId, title, director, runtime, genres);
                }
            }
        }
    }
}
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");
string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

Console.WriteLine("1) View All Movies.");
Console.WriteLine("2) Add Movie.");
Console.WriteLine("3) Find Movie.");
Console.WriteLine("Enter quit.");

string? resp = Console.ReadLine();

if (resp == "1")
{
    using (StreamReader sr = new StreamReader("movies.scrubbed.csv"))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                if (fields.Length == 5)
                {
                    int movieId;
                    if (int.TryParse(fields[0], out movieId))
                    {
                        string title = fields[1];
                        string genres = fields[2];
                        string director = fields[3];
                        TimeSpan runtime;
                        if (TimeSpan.TryParse(fields[4], out runtime))
                        {
                            DisplayMovieDetails(movieId, title, director, runtime, genres);
                        }
                    }
                }
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
     Console.Write("Enter the title to search for: ");
    string titleToSearch = Console.ReadLine();
    SearchMoviesByTitle(titleToSearch);
    }

logger.Info("Program ended");

