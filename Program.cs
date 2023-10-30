using Microsoft.AspNetCore.Mvc;
using NLog;


string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

List<Movie> movies = movieFile.Movies;

while (true)
    {
        Console.WriteLine("1) View All Movies.");
        Console.WriteLine("2) Add Movie.");
        Console.WriteLine("3) Find Movie.");
        Console.WriteLine("Enter to quit.");

        string? resp = Console.ReadLine();

            if (resp == "1")
            {
                foreach (var movie in movies)
                {
                    DisplayMovieDetails(movie);
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
        // Search for movies by title using LINQ
        Console.Write("Enter the title to search for: ");
        string titleToSearch = Console.ReadLine();
        SearchMoviesByTitle(movies, titleToSearch);
    }
    else if (resp == "")
    {
        break;
    }
    else
    {
        Console.WriteLine("Invalid option. Please choose a valid option.");
    }
}

logger.Info("Program ended");

 static void SearchMoviesByTitle(List<Movie> movies, string titleToSearch)
    {
        var matchingMovies = movies.Where(movie => movie.title.Contains(titleToSearch, StringComparison.OrdinalIgnoreCase)).ToList();

        Console.WriteLine("Search Results:");
        foreach (var movie in matchingMovies)
        {
            DisplayMovieDetails(movie);
        }
        Console.WriteLine($"Total matches found: {matchingMovies.Count}");
    }

static void DisplayMovieDetails(Movie movie)
    {
        Console.WriteLine($"Id: {movie.mediaId}");
        Console.WriteLine($"Title: {movie.title}");
        Console.WriteLine($"Director: {movie.director}");
        Console.WriteLine($"Run time: {movie.runningTime:hh\\:mm\\:ss}");
        Console.WriteLine($"Genres: {movie.genres}");
        Console.WriteLine();
    }