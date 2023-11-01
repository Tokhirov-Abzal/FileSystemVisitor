


namespace FileSystemVisitor
{
    public class FileSystemVisitor : IEnumerable<string>
    {
        private readonly string rootPath;
        private readonly Func<string, bool> filter;

        public event EventHandler? Started;
        public event EventHandler? Finished;
        public event EventHandler<string>? FileFound;
        public event EventHandler<string>? DirectoryFound;
        public event EventHandler<string>? FilteredFileFound;
        public event EventHandler<string>? FilteredDirectoryFound;

        public FileSystemVisitor(string rootPath, Func<string, bool> filterAlgorithm = null)
        {
            this.rootPath = rootPath;
            this.filter = filterAlgorithm ?? ((path) => true);
        }

        public IEnumerator<string> GetEnumerator()
        {
            OnStarted();
            foreach (string item in GetFileSystemEntries(rootPath))
            {
                yield return item;
            }
            OnFinished();
        }

        private IEnumerable<string> GetFileSystemEntries(string directory)
        {
            foreach (string file in Directory.GetFiles(directory).Where(filter))
            {
                OnFileFound(file);
                if (filter(file))
                {
                    OnFilteredFileFound(file);
                    yield return file;
                }
            }

            foreach (string subDirectory in Directory.GetDirectories(directory).Where(filter))
            {
                OnDirectoryFound(subDirectory);

                if (filter(subDirectory))
                {
                    OnFilteredDirectoryFound(subDirectory);
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

        protected virtual void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFileFound(string filePath)
        {
            FileFound?.Invoke(this, filePath);
        }

        protected virtual void OnDirectoryFound(string directoryPath)
        {
            DirectoryFound?.Invoke(this, directoryPath);
        }

        protected virtual void OnFilteredFileFound(string filePath)
        {
            FilteredFileFound?.Invoke(this, filePath);
        }

        protected virtual void OnFilteredDirectoryFound(string directoryPath)
        {
            FilteredDirectoryFound?.Invoke(this, directoryPath);
        }
    }

}
