namespace FileSystemVisitor;

class Program
{
    static void Log(string message)
    {
        Console.WriteLine(message);
    }

    static void Main()
    {
        string startingDirectory = @"D:\"; //Point the directory

        FileSystemVisitor fileSystemVisitor = new(startingDirectory,
            (path) => !path.Contains("exclude"));

        // subscribe to events
        fileSystemVisitor.Started += (s, args) => Log(args.Message);
        fileSystemVisitor.Finished += (s, args) => Log(args.Message);

        foreach (string item in fileSystemVisitor)
        {
            Console.WriteLine(item);
        }
    }
}

