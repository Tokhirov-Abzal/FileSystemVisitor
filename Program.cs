using System;
using FileSystemVisitor;


class Program
{
    static void Main()
    {
        string startingDirectory = @"D:\"; //Point the directory
        FileSystemVisitor.FileSystemVisitor fileSystemVisitor = new FileSystemVisitor.FileSystemVisitor(startingDirectory,
            (path) => !path.Contains("exclude"));

        foreach (string item in fileSystemVisitor)
        {
            Console.WriteLine(item);
        }
    }
}

