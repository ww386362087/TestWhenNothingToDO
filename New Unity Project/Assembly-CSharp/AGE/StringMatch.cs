﻿namespace AGE
{
    using System;
    using System.Text.RegularExpressions;

    public class StringMatch
    {
        public static bool IsMatchString(string[] strArray, string pattern)
        {
            if (strArray != null)
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (IsMatchString(strArray[i], pattern))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsMatchString(string str, string pattern)
        {
            pattern = pattern.Replace("*", ".*");
            pattern = pattern.Replace("?", ".");
            return Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase);
        }
    }
}

