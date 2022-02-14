using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;

namespace Services
{
    [AddService]
    public class FileBuilder : IFileBuilder
    {
        private bool WriteFile(string contents, string path, string name)
        {
            try
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(string.Format("{0}/{1}", path, name), contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteFiles(ImmutableList<FileCode> contents, string path)
        {
            try
            {
                contents.ForEach((x) => WriteFile(x.Code!, path, x.FileName!));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteFile(FileCode filecode, string path)
        {
            throw new NotImplementedException();
        }
    }
}
