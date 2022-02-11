using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Services
{
    [AddService]
	public class TypeProcessor : ITypeProcessor
	{
		public string MainDataTable { get => mainDataTable!; set => mainDataTable = value; }
		private string RefOrOut { get; set; } = "";

		private string DotNetTypeToAlias(string x, bool offDT)
		{
			string key = x.Contains("&") ? x.Replace("&", "") : x;
			var dictionary = new Dictionary<string, string>
			{
				{"System.Byte", "byte" },
				{"System.SByte", "sbyte" },
				{"System.Int32", "int" },
				{"System.UInt32", "uint"},
				{"System.Int16", "short"},
				{"System.UInt16", "ushort"},
				{"System.Int64", "long"},
				{"System.UInt64", "ulong"},
				{"System.Single", "float"},
				{"System.Double", "double"},
				{"System.Char", "char"},
				{"System.Boolean", "bool"},
				{"System.Object", "object"},
				{"System.String", "string"},
				{"System.Decimal", "decimal"},
				{"System.DateTime", "DateTime"},
				{"System.Void", "void"},
				{"System.Collections.IEnumerable", "IEnumerable" }
			};
			string value = "";
			if (dictionary.TryGetValue(key, out value!))
			{
				if (x.Contains("&"))
				{
					return string.Format("{0} {1}", RefOrOut, value);
				}
				return value;
			}
			if (((ITypeProcessor)this).MainDataTable == x)
			{
				if (x.Contains("DT") || x.Contains("Dt")) return "DataTable";
			}

			if (x.Contains("+")) return x.Replace("+", ".");

			//DT with ref
			if (x.Contains("&"))
			{
				return string.Format("{1} {0}", x.Replace("&", ""), RefOrOut);
			}

			return x;
		}

		private bool IsList(string fullName)
		{
			string typeName = DotNetTypeToAlias(fullName, true);

			if (typeName.Length < 4) return false;

			string startTypeName = typeName.Substring(0, 4);

			if (typeName.Contains("System.Collections.Generic.List")) return true;

			bool result = startTypeName == "List";
			return result;
		}

		private bool IsGeneric(Type x)
		{
			if (x.FullName == null) return false;
			if (x.FullName.Contains("Dictionary")) return true;
			if (IsList(x.FullName)) return true;
			return x.IsGenericType;
		}

		private readonly Func<Type, bool> IsNullable = (x =>
		{
			return x.FullName!.Contains("Nullable");
		});
		private string? mainDataTable;

		private string GetOnlyLetters(string name)
		{
			Match m = Regex.Match(name, "[A-Za-z]*", RegexOptions.IgnoreCase);
			if (m.Success)
			{
				return m.Value;
			}
			return name;
		}

		public string GetFullNameFromNullableRef(Type type, bool offDT = false)
		{
			return
				   type.FullName!.Contains("&")
				   ? DotNetTypeToAlias(string.Format("{0}&", type!.GetGenericArguments()!.FirstOrDefault()!.FullName), offDT) + "?"
				   : DotNetTypeToAlias(type.GetGenericArguments().FirstOrDefault()!.FullName!, offDT) + "?";
		}


		private bool IsIDictionary(Type type)
		{
			return type.FullName!.Contains("IDictionary");
		}

		private string DictionaryProcess(Type type, bool offDT = false)
		{
			//Type
			Regex rx = new Regex(@"[0-9]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			var match = rx.Match(type.Name);
			int quantityOfTypes = (match.Success) ? Convert.ToInt32(match.Value) : 0;

			Regex regex = new Regex(@"\[[A-Za-z0-9]*\.[A-Za-z0-9]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			MatchCollection matches = regex.Matches(type.FullName!);
			string outIndicator = RefOrOut;

			if (quantityOfTypes >= 2)
			{
				List<string> values = new List<string>();
				foreach (Match m in matches)
				{
					values.Add(DotNetTypeToAlias(m.Value.Replace("[", ""), offDT));
				}

				string IDictionary = (IsIDictionary(type)) ? "I" : "";

				string resultMulti = string.Format("{0} {2}Dictionary<{1}>",
					outIndicator,
					string.Join(", ", values),
					IDictionary);

				return resultMulti;
			}
			// Inner types
			string result = string.Format("{0} Dictionary<{1}>", outIndicator, DotNetTypeToAlias(matches[0].Value, offDT));
			return result;
		}

		private bool IsArrayInGenericType(Type type)
		{
			Regex regex = new Regex(@"\[[A-Za-z0-9]*\.[A-Za-z0-9]*\[]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Match match = regex.Match(type.FullName!);

			return match.Success;
		}

		private string GetNullableType(Type type, bool offDT)
		{
			string result = "";

			bool isArray = (IsArrayInGenericType(type));

			string pattern = @"\[[A-Za-z0-9]*\.[A-Za-z0-9]*";
			Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			MatchCollection matches = regex.Matches(type.FullName!);

			string value = "";
			string nullChecker = "";
			foreach (Match m in matches)
			{
				if (!m.Value.Contains("Nullable"))
				{
					value = m.Value;
				}
				else nullChecker = "?";
			}

			result = (isArray)
				? string.Format("List<{0}{1}[]>", DotNetTypeToAlias(value.Replace("[", ""), offDT), nullChecker)
				: string.Format("List<{0}{1}>", DotNetTypeToAlias(value.Replace("[", ""), offDT), nullChecker);
			return result;
		}

		private MatchCollection GetValueFromPattern(string fullName, string pattern)
		{
			Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return regex.Matches(fullName);
		}

		private string DataManagerType(Type type, bool offDT)
		{
			string result = "";

			MatchCollection matches =
				(GetValueFromPattern(type.FullName!, @"\[[A-Za-z0-9]*\.[A-Za-z0-9]*\.[A-Za-z0-9]*\.[A-Za-z0-9]*").Count != 0)
				? GetValueFromPattern(type.FullName!, @"\[[A-Za-z0-9]*\.[A-Za-z0-9]*\.[A-Za-z0-9]*\.[A-Za-z0-9]*")
				: GetValueFromPattern(type.FullName!, @"\[[A-Za-z0-9]*\.[A-Za-z0-9]*\.[A-Za-z0-9]*");

			if (matches.Count == 0) return "";

			string value = "";
			string nullChecker = "";
			foreach (Match m in matches)
			{
				if (!m.Value.Contains("Nullable"))
				{
					value = m.Value;
				}
				else nullChecker = "?";
			}
			var values = value.Split('.');
			result = string.Format("List<{0}{1}>", DotNetTypeToAlias(values[values.Length - 1], offDT), nullChecker);
			return result;
		}

		private string ListProcess(Type type, bool offDT = false)
		{
			//Type
			Regex rx = new Regex(@"[0-9]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			var match = rx.Match(type.Name);
			int quantityOfTypes = (match.Success) ? Convert.ToInt32(match.Value) : 0;

			Regex regex = new Regex(@"\[[A-Za-z0-9]*\,", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			MatchCollection matches = regex.Matches(type.FullName!);

			if (matches.Count == 0)
			{
				string dataManagerType = DataManagerType(type, offDT);
				if (!string.IsNullOrEmpty(dataManagerType)) return dataManagerType;

				return GetNullableType(type, offDT);
			}

			string outIndicator = RefOrOut;
			if (quantityOfTypes >= 2)
			{
				List<string> values = new List<string>();
				foreach (Match m in matches)
				{
					values.Add(DotNetTypeToAlias(m.Value.Replace("[", ""), offDT));
				}
				string resultMulti = string.Format("{0}List<{1}>",
					outIndicator,
					string.Join(", ", values));
				return resultMulti;
			}

			// Inner types
			string result = string.Format("{0} List<{1}>", outIndicator, DotNetTypeToAlias(matches[0].Value.Replace("[", "").Replace(",", ""), offDT));
			return result;
		}


		private string GetFullName(Type type, bool offDT = false)
		{
			int argumentsInside = type.GenericTypeArguments.First().GetGenericArguments().Count();
			string? result = (argumentsInside > 0)
				? (argumentsInside == 1)
				   ? type.GenericTypeArguments.FirstOrDefault()!.GetGenericArguments().FirstOrDefault()!.FullName
				   : string.Join(" ,",
						 type.GenericTypeArguments.FirstOrDefault()!.GetGenericArguments()
						 .Select(x => DotNetTypeToAlias(x.FullName!, offDT)))
				: type.GenericTypeArguments.FirstOrDefault()!.FullName;

			return result!;
		}

		private string ProcessGenericDataBasse(bool returnType, Type type, bool offDT)
		{
			if (returnType && IsDataBase(type))
			{
				string dataTable = "DataTable";
				string currentDataTable = DotNetTypeToAlias(GetFullName(type), offDT);
				if (((ITypeProcessor)this).MainDataTable != currentDataTable)
				{
					dataTable = currentDataTable;
				}
				return string.Format("{0}<{1}>", GetOnlyLetters(type.Name), dataTable);
			}
			return "";
		}


		private string ProcesssGeneric(Type type, bool returnType, bool offDT = false)
		{
			var generic = type.Name;
			if (generic.Contains("Dictionary")) return DictionaryProcess(type);

			string listDataBase = ProcessGenericDataBasse(returnType, type, offDT);
			if (!string.IsNullOrEmpty(listDataBase)) return listDataBase;

			if (generic.Contains("List")) return ListProcess(type, offDT);

			var innerType = GetFullName(type);
			var typeProcessed =
				(type.GenericTypeArguments.Count() > 1)
				? innerType
				: DotNetTypeToAlias(innerType, offDT);

			var nullMark = (IsNullable(type)) ? "?" : "";

			string preResult = string.Format("{0}<{1}{2}>", GetOnlyLetters(generic), typeProcessed, nullMark);

			string result = preResult.Contains("Nullable")
				? preResult.Replace("Nullable", "").Replace("<", "").Replace(">", "")
				: preResult;

			return result;
		}

		private bool IsDataBase(Type type)
		{
			if (type.FullName == null) return false;

			if (type.FullName.Contains("DT"))
			{
				return true;
			}

			return false;
		}

		private string ProcessType(Type type, bool offDataTableChecker = false)
		{
			if (!offDataTableChecker) if (IsDataBase(type)) return "DataTable";

			if (type.Name == "T&") return "out T";

			if (IsNullable(type)) return GetFullNameFromNullableRef(type);

			return DotNetTypeToAlias(type.FullName!, offDataTableChecker);
		}

		private bool ReleaseDataTableCheck(Type type)
		{
			if (type.Name.Contains("DT") || type.Name.Contains("Dt"))
			{
				if (!string.IsNullOrEmpty(((ITypeProcessor)this).MainDataTable))
					if (((ITypeProcessor)this).MainDataTable != type.Name) return true;
			}
			return false;
		}

		public string ReflectionToCode(Type type, bool returnType = false)
		{
			string result = (IsGeneric(type))
				? ProcesssGeneric(type, returnType, ReleaseDataTableCheck(type))
				: ProcessType(type, ReleaseDataTableCheck(type));
			return result;
		}
		public string ReflectionToCode(Type type, bool offDataTableChecker, bool returnType = false)
		{
			string result = (IsGeneric(type)) ? ProcesssGeneric(type, returnType, offDataTableChecker) : ProcessType(type, offDataTableChecker);
			return result;
		}
		public string ReflectionToCode(Type type, bool offDataTableChecker, bool returnType, string refOrOut)
		{
			RefOrOut = refOrOut;
			string result = (IsGeneric(type)) ? ProcesssGeneric(type, returnType, offDataTableChecker) : ProcessType(type, offDataTableChecker);
			return result;
		}
	}
}
