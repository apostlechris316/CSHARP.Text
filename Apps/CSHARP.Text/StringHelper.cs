/********************************************************************************
 * CSHARP Text Library - General utility to manipulate text strings
 * 
 * NOTE: Adapted from Clinch.Text
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to 
 *          chris.williams@readwatchcreate.com
 ********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSHARP.Text
{
	/// <summary>
	/// General function to help manupulate strings
	/// </summary>
	public static class StringHelper
    {
        /// <summary>
        /// Generates an 8 character random string based on random filename function
        /// </summary>
        /// <returns>8 character random number</returns>
        public static string Get8CharacterRandomString()
        {
            var path = Path.GetRandomFileName();
            path = path.Replace(".", string.Empty); // Remove period.
            return path.Substring(0, 8);  // Return 8 character string
        }

        /// <summary>
        /// Converts an url friendly string by replacing all non-alphanumeric and spaces with an alternate string
        /// </summary>
        /// <param name="toConvert">String to convert</param>
        /// <param name="replaceUnfriendlyWith">string to replace non-friendly characters with</param>
        /// <returns></returns>
        /// <remarks>NEW in V2.0.0.8
        /// 2.0.0.10 - Spaces were being replaced with empty string rather than replaceUnfriendlyWith
        /// </remarks>
	    public static string ConvertToUrlFriendly(string toConvert, string replaceUnfriendlyWith)
	    {
            var result = string.Empty;
            if (string.IsNullOrEmpty(toConvert)) return result;

            // Repalce all non-alphanumeric with space
            const string toIngnore = "!@#$%^&*()-=+\\/?<>|[{}];:`'\".,™“’–”\"";
            var charactersToIgnore = toIngnore.ToCharArray();
            var cleaned = toConvert.Replace("\r", string.Empty).Replace("\n", string.Empty);
            var array = charactersToIgnore;
            cleaned = array.Aggregate(cleaned, (current, ignore) => current.Replace(ignore, ' '));

            // Change all whitespace with string
            cleaned = cleaned.Replace(" ", replaceUnfriendlyWith).Replace("\t", replaceUnfriendlyWith);

            result = cleaned;

            return result;	        
	    }

        #region ConvertToAlphaNumeric 

        /// <summary>
        /// Removes all non-alpha-numeric from string.
        /// </summary>
        /// <param name="toConvert">String to remove alpha-numeric from</param>
        /// <param name="removeWhiteSpace">if true removes spaces and, tabs.</param>
        /// <returns>alpha-numeric string</returns>
        public static string ConvertToAlphaNumeric(string toConvert, bool removeWhiteSpace)
        {
            return ConvertToAlphaNumeric(toConvert, removeWhiteSpace, true);
        }

        /// <summary>
        /// Converts all non-alpha-numeric to spaces in the string.  
        /// Removes White Spaces and Underscores if requested
        /// </summary>
        /// <param name="toConvert">String to remove alpha-numeric from</param>
        /// <param name="removeWhiteSpace">if true removes spaces and, tabs.</param>
        /// <param name="removeUnderScore">if true reomoves underscores</param>
        /// <returns>alpha-numeric string</returns>
        public static string ConvertToAlphaNumeric(string toConvert, bool removeWhiteSpace, bool removeUnderScore)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(toConvert)) return result;
            
            var toIngnore = removeUnderScore ? "!@#$%^&*()-_=+\\/?<>|[{}];:`'\".,™“’\"" : "!@#$%^&*()-=+\\/?<>|[{}];:`'\".,™“’–”\"";

            var charactersToIgnore = toIngnore.ToCharArray();
            var cleaned = toConvert.Replace("\r", string.Empty).Replace("\n", string.Empty);
            var array = charactersToIgnore;
            cleaned = array.Aggregate(cleaned, (current, ignore) => current.Replace(ignore, ' '));

            if (removeWhiteSpace) cleaned = cleaned.Replace(" ", string.Empty).Replace("\t", string.Empty);

            result = cleaned;

            return result;
        }

        #endregion

	    /// <summary>
	    /// Converts a list of objects to CSV
        /// </summary>
	    /// <param name="collection">Collection of objects to generate CSV from</param>
	    /// <param name="type">type of object being generated</param>
	    /// <returns>csv string</returns>
	    /// <remarks>V2.0.0.2 Case Corrected in method name</remarks>
	    public static string ConvertToCsv(IEnumerable collection, Type type)
        {
            var stringBuilder = new StringBuilder();
            var header = new StringBuilder();

            // Gets all  properies of the class
            var properties = type.GetProperties();

            // Create CSV header using the classes properties
            foreach (var propertyForHeader in properties) header.Append(propertyForHeader.Name + ",");

            stringBuilder.AppendLine(header.ToString());

            foreach (var objectToGenerateCsvRowFor in collection)
            {
                var body = new StringBuilder();

                // Create new item
                var t1 = objectToGenerateCsvRowFor;
                foreach (var propertyForBody in properties.Select(p => p.GetValue(t1, null)))
                {
                    if (propertyForBody != null)
                    {
                        // Ensure column values with commas in it are quoted
                        if (propertyForBody.ToString().IndexOf(',') > -1) body.Append("\"" + propertyForBody + "\",");
                        else body.Append(propertyForBody + ",");
                    }
                    else body.Append(",");
                }

                stringBuilder.AppendLine(body.ToString());
            }
            return stringBuilder.ToString();
        }

        #region String Splitters

	    /// <summary>
	    /// Splits a string into its words for manipulation
	    /// </summary>
	    /// <param name="toSplit">String to split into words</param>
	    /// <returns></returns>
	    /// <remarks>Uses default values to split words</remarks>
	    public static List<string> SplitStringIntoWords(string toSplit)
	    {
            return SplitStringIntoWords(toSplit, new char[] {' ', ',', ';', ':', '(',')','{','}','[',']','!','.','?' });
	    }

	    /// <summary>
        /// Splits a string into its words for manipulation
        /// </summary>
	    /// <param name="toSplit">String to split into words</param>
	    /// <param name="endOfWordToken"></param>
	    /// <returns></returns>
	    public static List<string> SplitStringIntoWords(string toSplit, char[] endOfWordToken)
	    {
            var words = new List<string>();
	        var splitBuffer = toSplit;

            while (string.IsNullOrEmpty(splitBuffer) == false)
	        {
	            string foundWord = GetBeforeOneOf(splitBuffer, endOfWordToken, "EXCLUDING");
	            words.Add(foundWord);

	            splitBuffer = (foundWord == splitBuffer)
	                ? string.Empty
	                : GetAfterOneOf(splitBuffer, endOfWordToken, "EXCLUDING");
	        }

	        return words;
	    }

        /// <summary>
		/// Splits a string into lines for manipulation
		/// </summary>
		/// <param name="toSplit">string containing words</param>
		/// <returns></returns>
		public static string [] SplitStringIntoLines(string toSplit)
		{
            const RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                                          | RegexOptions.IgnoreCase);
            var reg = new Regex("(?:^|,)(\\\"(?:[^\\\"]+|\\\"\\\")*\\\"|[^,]*)", options);
            var coll = reg.Matches(toSplit);
            var items = new string[coll.Count];
            var i = 0;
            foreach (Match m in coll) items[i++] = m.Groups[0].Value.Trim('"').Trim(',').Trim('"').Trim();
            return items;
        }

	    /// <summary>
	    /// Splits the string to dictionary given tokens.
	    /// </summary>
	    /// <param name="toSplit">string to split</param>
        /// <param name="keyValueToken">token to split key and value</param>
	    /// <param name="entryToken">token to split each dictionary entry</param>
	    /// <returns></returns>
        /// <remarks>NEW in v2.0.0.3</remarks>
	    public static Dictionary<string, string> SplitStringToDictionary(string toSplit, char keyValueToken, char entryToken)
        {
            var stringList = toSplit.Split(entryToken);

            return stringList.Select(pair => pair.Split(keyValueToken)).ToDictionary(keyValue => keyValue[0], keyValue => keyValue.Length > 1 ? keyValue[1] : string.Empty);
        }

        /// <summary>
        /// Splits the string to dictionary given tokens, only including duplicate keys once.
        /// </summary>
        /// <param name="toSplit">string to split</param>
        /// <param name="keyValueToken">token to split key and value</param>
        /// <param name="entryToken">token to split each dictionary entry</param>
        /// <returns></returns>
        /// <remarks>NEW in v2.0.0.5</remarks>
        public static Dictionary<string, string> SplitStringToDistinctDictionary(string toSplit, char keyValueToken, char entryToken)
        {
            var stringList = toSplit.Split(entryToken);
            var distinctDictionary = new Dictionary<string, string>();

            foreach (var itemPairs in stringList.Select(item => item.Split(keyValueToken)).Where(itemPairs => distinctDictionary.ContainsKey(itemPairs[0]) == false))
            {
                distinctDictionary.Add(itemPairs[0], itemPairs[1]);
            }

            return distinctDictionary;
        }


	    /// <summary>
	    /// Splits the string to dictionary on the end of line marker.
	    /// </summary>
	    /// <param name="input">string to split</param>
	    /// <param name="token">token to split key and value</param>
	    /// <returns></returns>
        /// <remarks>FIX: 2.0.0.2 - Exception on Line with key and no value</remarks>
	    public static Dictionary<string, string> SplitStringToDictionaryOnEndOfLine(string input, char token)
        {   
            var stringList = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
	        return stringList.Select(pair => pair.Split(token)).ToDictionary(keyValue => keyValue[0], keyValue => keyValue.Length > 1 ? keyValue[1] : string.Empty);
        }

        /// <summary>
        /// Splits the string on the end of line marker.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] SplitStringOnEndOfLine(string input)
        {
            return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Splits the string on the end of line marker. Only include distinct strings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>NEW in 2.0.0.6</remarks>
        public static string[] SplitStringOnEndOfLineDistinct(string input)
        {
            var stringList = new List<string>();
            foreach (var item in input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(item => stringList.Contains(item) == false))
            {
                stringList.Add(item);
            }

            return stringList.ToArray();
        }

        #endregion

        #region StringArray To ...

        /// <summary>
        /// Builds a delimited string from a string array. Only include distinct strings
        /// </summary>
        /// <param name="items">array of strings</param>
        /// <param name="token">delimiter</param>
        /// <returns></returns>
        /// <remarks>NEW in 2.0.0.6</remarks>
        public static string StringArrayToDistinctDelimitedString(string[] items, char token)
        {
            var returnValue = new StringBuilder();
            var itemList = new List<string>();
            var firstItem = true;
            foreach (var item in items)
            {
                if (itemList.Contains(item) == false)
                {
                    if (!firstItem) returnValue.Append(token);
                    itemList.Add(item);
                    returnValue.Append(item);
                }
                firstItem = false;
            }
            return returnValue.ToString();
        }

        /// <summary>
		/// Builds a delimited string from a string array
		/// </summary>
		/// <param name="items">array of strings</param>
		/// <param name="token">delimiter</param>
		/// <returns></returns>
		public static string StringArrayToDelimitedString(string[] items, char token)
		{
			var returnValue = new StringBuilder();
			var firstItem = true;
			foreach (var item in items)
			{
			    if (!firstItem) returnValue.Append(token);
			    returnValue.Append(item);
			    firstItem = false;
			}
			return returnValue.ToString();
		}

        #endregion

        #region StringList To ...

        /// <summary>
        /// Builds a delimited string from a list of strings
        /// </summary>
        /// <param name="items">list of strings</param>
        /// <param name="token">delimiter</param>
        /// <returns></returns>
        public static string StringListToDelimitedString(List<string> items, char token)
        {
            var returnValue = new StringBuilder();
            var firstItem = true;
            foreach (var item in items)
            {
                if (!firstItem) returnValue.Append(token);
                returnValue.Append(item);
                firstItem = false;
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Builds a string containing a string per line from a delimited string 
        /// </summary>
        /// <param name="items">list of strings</param>
        /// <returns></returns>
        /// <remarks>NEW in 2.0.0.7</remarks>
        public static string StringListToEndOfLineDelimited(List<string> items)
        {
            var returnValue = new StringBuilder();
            var firstItem = true;
            foreach (var item in items)
            {
                if (!firstItem) returnValue.Append("\r\n");
                returnValue.Append(item);
                firstItem = false;
            }

            return returnValue.ToString();
        }

        #endregion

        #region Delimited String To ...

        /// <summary>
        /// Builds an array of strings from a delimited string 
        /// </summary>
        /// <param name="delimitedString">delimited string</param>
        /// <param name="token">delimiter</param>
        /// <returns></returns>
		/// <remarks>FIXED v2.0.0.10 - Returns null if null passed in</remarks>
        public static List<Guid> DelimitedStringToGuidList(string delimitedString, char token)
        {
            return (string.IsNullOrEmpty(delimitedString)) ? null : delimitedString.Split(token).Select(item => new Guid(item)).ToList();
        }

        /// <summary>
		/// Builds an array of strings from a delimited string 
		/// </summary>
		/// <param name="delimitedString">delimited string</param>
		/// <param name="token">delimiter</param>
		/// <returns></returns>
		/// <remarks>FIXED v2.0.0.10 - Returns null if null passed in</remarks>
		public static string[] DelimitedStringToStringArray(string delimitedString, char token)
		{
            return (string.IsNullOrEmpty(delimitedString)) ? null : delimitedString.Split(token);
		}

        /// <summary>
		/// Builds an array of strings from a delimited string 
		/// </summary>
		/// <param name="delimitedString">delimited string</param>
		/// <param name="token">delimiter</param>
		/// <returns></returns>
		/// <remarks>FIXED v2.0.0.10 - Returns null if null passed in</remarks>
		public static List<string> DelimitedStringToStringList(string delimitedString, char token)
		{
            return (string.IsNullOrEmpty(delimitedString)) ? null : delimitedString.Split(token).ToList();
		}

        /// <summary>
		/// Builds an array of strings from a delimited string 
		/// </summary>
		/// <param name="delimitedString">delimited string</param>
		/// <param name="token">delimiter</param>
		/// <returns></returns>
		/// <remarks>FIXED v2.0.0.10 - Returns null if null passed in</remarks>
		public static string [] DelimitedStringToStringArray(string delimitedString, string token)
		{
            return (string.IsNullOrEmpty(delimitedString)) ? null : Regex.Split(delimitedString, token);
		}

        /// <summary>
        /// Builds a string containing a string per line from a delimited string 
        /// </summary>
        /// <param name="delimitedString">delimited string</param>
        /// <param name="token">delimiter</param>
        /// <returns></returns>
		/// <remarks>FIXED v2.0.0.10 - Returns null if null passed in</remarks>
        public static string DelimitedStringToEndOfLineDelimited(string delimitedString, string token)
        {
            return (string.IsNullOrEmpty(delimitedString)) ? null : delimitedString.Replace(token, "\r\n");
        }

        #endregion

        #region GuidList To ...

        /// <summary>
		/// Builds a delimited string from a list of strings
		/// </summary>
		/// <param name="items">list of strings</param>
		/// <param name="token">delimiter</param>
		/// <returns></returns>
		public static string GuidListToDelimitedString(List<Guid> items, char token)
		{
			var returnValue = new StringBuilder();
			var firstItem = true;
			foreach (var item in items)
			{
				if (!firstItem) returnValue.Append(token);
				returnValue.Append(item);
				firstItem = false;
			}
			return returnValue.ToString();
		}

        #endregion

        #region GetAfter 

        /// <summary>
		/// Removes all text after the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content after</param>
		/// <returns></returns>
		public static string GetAfter(string snippet, string token)
		{
			return GetAfter(snippet, token, "INCLUDING");
		}
        /// <summary>
        /// Removes all text after the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetAfter(string snippet, string token, bool including)
        {
            return GetAfter(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }
        /// <summary>
		/// Removes all text after the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content after</param>
		/// <param name="including">Determines if returned string includes the token</param>
		/// <returns></returns>
		public static string GetAfter(string snippet, string token, string including)
		{
			string returnValue;
			var index = snippet.IndexOf(token, StringComparison.Ordinal);
			if (index > -1)
			{
				returnValue = snippet.Substring(index + token.Length);
				if (including == "INCLUDING") returnValue = token + returnValue;
			}
			else
			{
				returnValue = snippet;
			}
			return returnValue;
		}

	    /// <summary>
	    /// Removes all text after the first occurence of one of the given tokens
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="tokens">array of tokens to look for</param>
	    /// <param name="including">Determines if returned string includes the token</param>
	    /// <returns></returns>
	    /// <remarks>NEW In v2.0.0.9</remarks>
	    public static string GetAfterOneOf(string snippet, char [] tokens, string including)
        {
            string returnValue;
            var index = snippet.Length;
            var token = string.Empty;

            foreach (var foundToken in tokens)
            {
                var foundIndex = snippet.IndexOf(foundToken.ToString(), StringComparison.Ordinal);
                if (foundIndex >= index || foundIndex == -1) continue;
                
                index = foundIndex;
                token = foundToken.ToString();
            }

            if (index > -1)
            {
                returnValue = snippet.Substring(index + token.Length);
                if (including == "INCLUDING") returnValue = token + returnValue;
            }
            else
            {
                returnValue = snippet;
            }

            return returnValue;
        }

        #endregion

        #region GetAfterLast 

        /// <summary>
        /// Removes all text after the last occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetAfterLast(string snippet, string token, bool including)
        {
            return GetAfterLast(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }

        /// <summary>
        /// Removes all text after the last occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetAfterLast(string snippet, string token, string including)
        {
            string returnValue;
            var index = snippet.LastIndexOf(token, StringComparison.Ordinal);
            if (index > -1)
            {
                returnValue = snippet.Substring(index + token.Length);
                if (including == "INCLUDING") returnValue = token + returnValue;
            }
            else
            {
                returnValue = snippet;
            }
            return returnValue;
        }
        
        #endregion

        #region GetAfterPosition

        /// <summary>
        /// Gets text after a given position
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="position">position to get content after</param>
        /// <returns></returns>
        public static string GetAfterPosition(string snippet, int position)
        {
            return position > -1 ? snippet.Substring(position) : snippet;
        }

        #endregion

        #region DeleteAfter

        /// <summary>
		/// Removes all text after the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content after</param>
		/// <returns></returns>
		public static string DeleteAfter(string snippet, string token)
		{
			return DeleteAfter(snippet, token, "INCLUDING");
		}

        /// <summary>
        /// Removes all text after the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string DeleteAfter(string snippet, string token, bool including)
        {
            return DeleteAfter(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }
        /// <summary>
		/// Removes all text after the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content after</param>
		/// <param name="including">Determines if returned string includes the token</param>
		/// <returns></returns>
		public static string DeleteAfter(string snippet, string token, string including)
		{
			string returnValue;
			var index = snippet.IndexOf(token, StringComparison.Ordinal);
			if (index > -1)
			{
                returnValue = snippet.Substring(0, index);
                if (including == "INCLUDING") returnValue = returnValue + token;
            }
			else
			{
				returnValue = snippet;
			}
			return returnValue;
		}

        #endregion

        #region DeleteAfterLast 

        /// <summary>
        /// Removes all text after the last occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string DeleteAfterLast(string snippet, string token, bool including)
        {
            return DeleteAfterLast(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }

        /// <summary>
        /// Removes all text after the last occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content after</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string DeleteAfterLast(string snippet, string token, string including)
        {
            string returnValue;
            var index = snippet.LastIndexOf(token, StringComparison.Ordinal);
            if (index > -1)
            {
                returnValue = snippet.Substring(0, index);
                if (including == "INCLUDING") returnValue = returnValue + token;
            }
            else
            {
                returnValue = snippet;
            }
            return returnValue;
        }

        #endregion

        #region FindOneOf

        /// <summary>
        /// Finds the first occurance of one of the characters in the tokens string in the snippet
        /// </summary>
        /// <param name="snippet">String to search inside</param>
        /// <param name="tokens">character tokens to search for inside snippet</param>
        /// <returns></returns>
	    public static int FindOneOf(string snippet, char[] tokens)
	    {
	        var index = -1;

	        foreach (var foundIndex in tokens.Select(foundToken => snippet.IndexOf(foundToken.ToString(), StringComparison.Ordinal)).Where(foundIndex => foundIndex < index))
	        {
	            index = foundIndex;
	        }

	        return index;
	    }

	    #endregion

        #region GetFirst

        /// <summary>
	    /// Gets the first x characters in a string
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="length">number of characters to get from the beginning for the string</param>
	    /// <returns></returns>
	    public static string GetFirst(string snippet, int length)
	    {
	        return GetFirst(snippet, length, false);
	    }

	    /// <summary>
	    /// Gets the first x characters in a string
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="length">number of characters to get from the beginning for the string</param>
	    /// <param name="addEllipsis">if true, will reduce length by an addition 3 characters and adds ... to the end</param>
	    /// <returns></returns>
	    public static string GetFirst(string snippet, int length, bool addEllipsis)
	    {
            return length < snippet.Length ? (addEllipsis ? snippet.Substring(0, length - 3) + "..." : snippet.Substring(0, length) ) : snippet;
	    }

        #endregion

        #region GetLast

        /// <summary>
        /// Gets the last x characters in a string
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="length">number of characters to get from the end for the string</param>
        /// <returns></returns>
        public static string GetLast(string snippet, int length)
        {
            return snippet.Substring(0, snippet.Length - length);
        }

        #endregion

        #region GetBefore 

        /// <summary>
		/// Removes all text before the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content before</param>
		/// <returns></returns>
		public static string GetBefore(string snippet, string token)
		{
			return GetBefore(snippet, token, "INCLUDING");
		}

        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content before</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetBefore(string snippet, string token, bool including)
        {
            return GetBefore(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }

		/// <summary>
		/// Removes all text before the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content before</param>
		/// <param name="including">Determines if returned string includes the token</param>
		/// <returns></returns>
		public static string GetBefore(string snippet, string token, string including)
		{
			string returnValue;
			int index = snippet.IndexOf(token, StringComparison.Ordinal);
			if (index > -1)
			{
                returnValue = snippet.Substring(0, index);
                if (including == "INCLUDING") returnValue = returnValue + token;
            }
			else
			{
				returnValue = snippet;
            }
			return returnValue;
		}

        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="tokens">array of tokens to look for</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        /// <remarks>NEW in v2.0.0.9</remarks>
        public static string GetBeforeOneOf(string snippet, char [] tokens, string including)
        {
            string returnValue;
            var index = snippet.Length;
            var token = string.Empty;

            foreach (var foundToken in tokens)
            {
                var foundIndex = snippet.IndexOf(foundToken.ToString(), StringComparison.Ordinal);
                if (foundIndex >= index || foundIndex == -1) continue;

                index = foundIndex;
                token = foundToken.ToString();
            }

            if (index > -1)
            {
                returnValue = snippet.Substring(0, index);
                if (including == "INCLUDING") returnValue = returnValue + token;
            }
            else
            {
                returnValue = snippet;
            }
            return returnValue;
        }

        #endregion

        /// <summary>
        /// Removes all text before the last occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content before</param>
        /// <returns></returns>
        public static string GetBeforeLast(string snippet, string token)
        {
            return GetBeforeLast(snippet, token, "INCLUDING");
        }

        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content before</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetBeforeLast(string snippet, string token, bool including)
        {
            return GetBeforeLast(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
        }

        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content before</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string GetBeforeLast(string snippet, string token, string including)
        {
            string returnValue;
            int index = snippet.LastIndexOf(token, StringComparison.Ordinal);
            if (index > -1)
            {
                returnValue = snippet.Substring(0, index);
                if (including == "INCLUDING") returnValue = returnValue + token;
            }
            else
            {
                returnValue = snippet;
            }
            return returnValue;
        }

        /// <summary>
        /// Removes all text after a given position in a string
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="position">position to get content before</param>
        /// <returns></returns>
        public static string GetBeforePosition(string snippet, int position)
        {
            string returnValue = position > -1 ? snippet.Substring(0, position) : snippet;
            return returnValue;
        }

	    /// <summary>
		/// Removes all text before the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content before</param>
		/// <returns></returns>
		public static string DeleteBefore(string snippet, string token)
		{
			return DeleteBefore(snippet, token, "INCLUDING");
		}

        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="token">string to delete content before</param>
        /// <param name="including">Determines if returned string includes the token</param>
        /// <returns></returns>
        public static string DeleteBefore(string snippet, string token, bool including)
	    {
	        return DeleteBefore(snippet, token, (including ? "INCLUDING" : "EXCLUDING"));
	    }

	    /// <summary>
		/// Removes all text before the first occurence of a given token
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="token">string to delete content before</param>
		/// <param name="including">Determines if returned string includes the token</param>
		/// <returns></returns>
		public static string DeleteBefore(string snippet, string token, string including)
		{
			string returnValue;
			var index = snippet.IndexOf(token, StringComparison.Ordinal);
			if (index > -1)
			{
                returnValue = snippet.Substring(index + token.Length);
                if (including == "INCLUDING") returnValue = token + returnValue;
			}
			else
			{
                returnValue = snippet;
			}
			return returnValue;
		}
        /// <summary>
        /// Removes all text before the first occurence of a given token
        /// </summary>
        /// <param name="snippet">String containing text to parse</param>
        /// <param name="position">position to delete content before</param>
        /// <returns></returns>
        public static string DeleteBeforePosition(string snippet, int position)
        {
            return position > -1 ? snippet.Substring(position) : snippet;
        }

	    /// <summary>
	    /// Gets the string content between the beforeToken and afterToken.
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="afterToken"></param>
	    /// <param name="beforeToken"></param>
	    /// <param name="including"></param>
	    /// <returns></returns>
	    public static string GetBetween(string snippet, string afterToken, string beforeToken, bool including)
	    {
            return GetBetween(snippet,afterToken,beforeToken, (including ? "INCLUDING" : "EXCLUDING" ));
	    }

	    /// <summary>
		/// Gets the string content between the beforeToken and afterToken.
		/// </summary>
		/// <param name="snippet">String containing text to parse</param>
		/// <param name="afterToken"></param>
		/// <param name="beforeToken"></param>
		/// <param name="including"></param>
		/// <returns></returns>
		/// <remarks>FIXED v2.0.0.4 - Including was missing before token</remarks>
		public static string GetBetween(string snippet, string afterToken, string beforeToken, string including)
		{
			string returnValue;
			var afterIndex = snippet.IndexOf(afterToken, StringComparison.Ordinal);
			var beforeIndex = snippet.IndexOf(beforeToken, StringComparison.Ordinal);
			if (afterIndex > -1)
			{
				beforeIndex = snippet.Substring(afterIndex + afterToken.Length).IndexOf(beforeToken, StringComparison.Ordinal);
				if (beforeIndex > -1) beforeIndex = afterIndex + beforeIndex + afterToken.Length;
			}
			if (afterIndex < 1 && beforeIndex < 1)
			{
				returnValue = snippet;
			}
			else
			{
				if (afterIndex < 1)
				{
				    returnValue = GetBefore(including == "EXCLUDING" ? snippet.Substring(afterToken.Length) : snippet, beforeToken, including);
				}
				else
				{
					if (beforeIndex < 1)
					{
						returnValue = GetAfter(snippet, afterToken, including);
					}
					else
					{
                        // FIXED v2.0.0.4 - Including was missing before token
                        returnValue = including == "EXCLUDING" ? snippet.Substring(afterIndex + afterToken.Length, beforeIndex - afterIndex - afterToken.Length) : snippet.Substring(afterIndex, beforeIndex - afterIndex + beforeToken.Length);
					}
				}
			}
			return returnValue;
		}

	    /// <summary>
	    /// Gets the string content between two positions.
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="afterPosition"></param>
	    /// <param name="beforePosition"></param>
	    /// <param name="beforeTokenLength"></param>
	    /// <returns></returns>
        /// <remarks>FIXED v2.0.0.4 - Including was missing before token</remarks>
        public static string GetBetweenPositions(string snippet, int afterPosition, int beforePosition, int beforeTokenLength)
        {
            string returnValue;
            if (afterPosition < 1 && beforePosition < 1)
            {
                returnValue = snippet;
            }
            else
            {
                if (afterPosition < 1)
                {
                    returnValue = GetBeforePosition(snippet, beforePosition);
                }
                else
                {
                    // FIXED v2.0.0.4 - Including was missing before token
                    returnValue = beforePosition < 1 ? GetAfterPosition(snippet, afterPosition) : snippet.Substring(afterPosition, beforePosition - afterPosition + beforeTokenLength);
                }
            }
            return returnValue;
        }

	    /// <summary>
	    /// Replaces the last occurence of a given token with the replacement string
	    /// </summary>
	    /// <param name="snippet">String containing text to parse</param>
	    /// <param name="token">string to delete content after</param>
	    /// <param name="replaceWith"></param>
	    /// <returns></returns>
	    public static string ReplaceLast(string snippet, string token, string replaceWith)
        {
            string returnValue;
            var index = snippet.LastIndexOf(token, StringComparison.Ordinal);
            if (index > -1)
            {
                returnValue = snippet.Substring(0, index) + replaceWith + snippet.Substring(index + token.Length);
            }
            else
            {
                returnValue = snippet;
            }
            return returnValue;
        }
    }
}
