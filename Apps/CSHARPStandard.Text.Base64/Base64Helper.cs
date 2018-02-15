/********************************************************************************
 * CSHARP Text Library - General utility to manipulate text strings
 * 
 * NOTE: Adapted from Clinch.Text
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to 
 *          chris.williams@readwatchcreate.com
 ********************************************************************************/

using System;
using System.Text;

namespace CSHARPStandard.Text
{
    /// <summary>
    /// Assists in encoding and decoding to and from base 64
    /// </summary>
    /// <remarks>Converted to Non-Static in .NET Standard Library to support greater compatability with other tools like Powershell</remarks>
    public class Base64Helper
	{
		/// <summary>
		/// Base 64 encoder
		/// </summary>
		/// <param name="toEncode">String to encode in to Base64</param>
		/// <returns>Encoded String</returns>
        /// <remarks>V2.0.0.2 Case Corrected in method name
        /// Encoding.ASCII introduced in .NET Standard 1.3
        /// </remarks>
		public string Base64Encode(string toEncode)
		{
			var toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
			return Convert.ToBase64String(toEncodeAsBytes);
		}
		/// <summary>
		/// Base 64 decoder
		/// </summary>
		/// <param name="data">Base64 data to decode</param>
		/// <returns>Decoded String</returns>
		public string Base64Decode(string data)
		{
			string result2;
			try
			{
				var encoder = new UTF8Encoding();
				var utf8Decode = encoder.GetDecoder();
				var todecodeByte = Convert.FromBase64String(data);
				var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
				var decodedChar = new char[charCount];
				utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
				var result = new string(decodedChar);
				result2 = result;
			}
			catch (Exception e)
			{
				throw new Exception("Error in base64Decode" + e.Message);
			}
			return result2;
		}
	}
}
