﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/. 
namespace Cake.Incubator.StringExtensions
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using Cake.Incubator.AssertExtensions;
    using Core.IO;

    /// <summary>
    /// Several extension methods when using String.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex TargetframeworkCondition = new Regex("\\s*\\\'\\$\\(TargetFramework\\)\\\'\\s*==\\s*", RegexOptions.Compiled);
        private const string ConfigPlatformCondition = "'$(Configuration)|$(Platform)'==";

        /// <summary>
        /// Case-insensitive String.Equals
        /// </summary>
        /// <param name="source">the source string</param>
        /// <param name="value">the string to compare</param>
        /// <returns>true if strings are the same</returns>
        public static bool EqualsIgnoreCase(this string source, string value)
        {
            return source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Case-insensitive String.StartsWith
        /// </summary>
        /// <param name="source">the source string</param>
        /// <param name="value">the string to compare</param>
        /// <returns>true if string starts with the value to check</returns>
        public static bool StartsWithIgnoreCase(this string source, string value)
        {
            return source.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// Case-insensitive String.EndsWith
        /// </summary>
        /// <param name="source">the source string</param>
        /// <param name="value">the string to compare</param>
        /// <returns>true if string ends with the value to check</returns>
        public static bool EndsWithIgnoreCase(this string source, string value)
        {
            return source.EndsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool HasTargetFrameworkCondition(this string condition)
        {
            return !condition.IsNullOrEmpty() && TargetframeworkCondition.IsMatch(condition);
        }

        internal static bool HasConfigPlatformCondition(this string condition, string config = null, string platform = null)
        {
            return config.IsNullOrEmpty() ? condition.StartsWith(ConfigPlatformCondition) : condition.EqualsIgnoreCase($"{ConfigPlatformCondition}'{config}|{platform}'");
        }

        internal static string GetConditionalConfigPlatform(this string condition)
        {
            return condition.Substring(ConfigPlatformCondition.Length).Trim().TrimStart('\'').TrimEnd('\'');
        }

        internal static string[] SplitIgnoreEmpty(this string value, params char[] separator)
        {
            return value.IsNullOrEmpty() ? new string[0] : value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        internal static string GetConditionTargetFramework(this string condition)
        {
            return TargetframeworkCondition.Replace(condition, string.Empty).Trim().TrimStart('\'').TrimEnd('\'');
        }

        internal static FilePath GetAbsolutePath(this string relativePath, DirectoryPath rootPath)
        {

            var isRelative = new FilePath(relativePath).IsRelative;
            Cake.Core.IO.FilePath absolutePath;
            if (isRelative)
            {
                absolutePath = rootPath.CombineWithFilePath(relativePath);
            }
            else
            {
                absolutePath = relativePath;
                Debug.WriteLine($"An absolute path {absolutePath} was used in a project reference. It is recommended that projects contain only relative paths for references");
            }

            return absolutePath;
        }
    }
}