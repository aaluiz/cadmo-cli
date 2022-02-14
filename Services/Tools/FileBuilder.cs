using System.Collections.Immutable;
using Contracts.Interfaces;
using Models;

namespace Services.Tools
{
    public class FileBuilder : IFileBuilder
    {
        public bool WriteFile(string? contents, string path, string? name)
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

        public bool WriteFile(FileCode filecode, string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(string.Format("{0}{1}", path, filecode.FileName), filecode.Code);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool WriteFiles(ImmutableList<FileCode> contents, string path)
        {
            try
            {
                contents.ForEach((x) => WriteFile(x.Code, path, x.FileName));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}