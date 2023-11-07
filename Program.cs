using System;
namespace FileSystemVisitor;


class Program
{
    static void Main()
    {
        string startingDirectory = @"D:\"; //Point the directory
        FileSystemVisitor fileSystemVisitor = new(startingDirectory,
            (path) => !path.Contains("exclude"));

        fileSystemVisitor.Started += (s, args) => Log(args.Message);
        fileSystemVisitor.FileFound += (s, args) => Log(args.Message);
        fileSystemVisitor.FilteredFileFound += (s, args) => Log(args.Message);
        fileSystemVisitor.DirectoryFound += (s, args) => Log(args.Message);
        fileSystemVisitor.FilteredDirectoryFound += (s, args) => Log(args.Message);
        fileSystemVisitor.Finished += (s, args) => Log(args.Message);

        foreach (string item in fileSystemVisitor)
        {
            Console.WriteLine(item);
        }
    }


    static void Log(string message)
    {
        Console.WriteLine(message);
    }
}

