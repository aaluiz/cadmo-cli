using Contracts.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Models;
using System.Collections.Immutable;
using System.Text;

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
				contents.ForEach((x) => WriteFile(FormatCSharpFileIdentation(x.Code!), path, x.FileName!));
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
				WriteFile(FormatCSharpFileIdentation(Identation(filecode.Code!)), path, filecode.FileName!);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		private string FormatCSharpFileIdentation(string csCode)
		{
			var tree = CSharpSyntaxTree.ParseText(csCode);
			var root = tree.GetRoot().NormalizeWhitespace();
			var ret = root.ToFullString();
			return ret;
		}

		private string Identation(string code)
		{
			var result = new StringBuilder();
			var temp = code.Split("\n");

			int indentCount = 0;
			bool shouldIdent = false;

			foreach (string line in temp)
			{
				if (shouldIdent) indentCount++;

				if (line.Contains("}")) indentCount--;

				if (indentCount == 0)
				{
					result.Append("\r\n" + line);
					shouldIdent = line.Contains("{");
					continue;
				}

				string blankSpace = string.Empty;
				for (int i = 0; i < indentCount; i++)
				{
					blankSpace += "    ";
				}

				result.Append("\r\n" + blankSpace + line);
				shouldIdent = line.Contains("{");
			}
			return result.ToString();
		}

	}
}
