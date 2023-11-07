using System;


namespace FileSystemVisitor
{
    // allows us to provide any arguments for subscribers
    public class FileVisitorEventArgs : EventArgs
    {
        public FileVisitorEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    public class FileSystemVisitor : IEnumerable<string>
    {
        private readonly string rootPath;
        private readonly Func<string, bool> filter;

        public event EventHandler<FileVisitorEventArgs> Started;
        public event EventHandler<FileVisitorEventArgs> Finished;
        public event EventHandler<FileVisitorEventArgs> FileFound;
        public event EventHandler<FileVisitorEventArgs> DirectoryFound;
        public event EventHandler<FileVisitorEventArgs> FilteredFileFound;
        public event EventHandler<FileVisitorEventArgs> FilteredDirectoryFound;

        public FileSystemVisitor(string rootPath, Func<string, bool> filterAlgorithm = null)
        {
            this.rootPath = rootPath;
            this.filter = filterAlgorithm ?? ((path) => true);
        }

        public IEnumerator<string> GetEnumerator()
        {
            Started(this, new FileVisitorEventArgs("Proccess has been started"));
            foreach (string item in GetFileSystemEntries(rootPath))
            {
                yield return item;
            }
            Finished(this, new FileVisitorEventArgs("Proccess has been finished"));
        }

        private IEnumerable<string> GetFileSystemEntries(string directory)
        {
            foreach (string file in Directory.GetFiles(directory).Where(filter))
            {
                FileFound(this, new FileVisitorEventArgs($"File was found: {file}"));
                if (filter(file))
                {
                    FilteredFileFound(this, new FileVisitorEventArgs($"Filtered file was found: ${file}"));
                    yield return file;
                }
            }

            foreach (string subDirectory in Directory.GetDirectories(directory).Where(filter))
            {
                DirectoryFound(this, new FileVisitorEventArgs($"Directory was found: {subDirectory}"));

                if (filter(subDirectory))
                {
                    FilteredDirectoryFound(this, new FileVisitorEventArgs($"Filtered directory was found: {subDirectory}"));
                    yield return subDirectory;
                }

                foreach (string item in GetFileSystemEntries(subDirectory))
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }

}
