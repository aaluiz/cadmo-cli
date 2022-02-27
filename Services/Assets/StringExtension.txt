using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Extension.String
{
    public static class StringExtension
    {
        public static List<string>? SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
            return csvList.TrimEnd(',').Split(',').AsEnumerable<string>().Select(s => s.Trim()).ToList();
        }

        public static bool IsNullOrWhitespace(this string? s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
    }
}