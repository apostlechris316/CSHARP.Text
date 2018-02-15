/********************************************************************************
 * CSHARP Text Library - General utility to manipulate text strings
 * 
 * NOTE: Adapted from Clinch.Text
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to 
 *          chris.williams@readwatchcreate.com
 ********************************************************************************/

using System;
using System.Text.RegularExpressions;

namespace CSHARPStandard.Text
{
    /// <summary>
    /// Functions to assist in validating strings
    /// </summary>
    /// <remarks>Converted to Non-Static in .NET Standard Library to support greater compatability with other tools like Powershell</remarks>
    public class StringValidation
    {
        /// <summary>
        /// Types of data that is storeable in a string
        /// </summary>
        public enum ValidStringTypes {
            /// <summary>
            /// All types of data that is storeable in a string
            /// </summary>
            All,
            /// <summary>
            /// Letters can be storeable in a string
            /// </summary>
            Alpha,
            /// <summary>
            /// Letters and numbers can be storeable in a string
            /// </summary>
            AlphaNumeric,
            /// <summary>
            /// Email addresses can be storeable in a string
            /// </summary>
            Email,
            /// <summary>
            /// Integers can be storeable in a string
            /// </summary>
            Integer,
            /// <summary>
            /// Natural numbers can be storeable in a string
            /// </summary>
            NaturalNumber,
            /// <summary>
            /// Numbers can be storeable in a string
            /// </summary>
            Number,
            /// <summary>
            /// Phone numbers can be storeable in a string
            /// </summary>
            Phone,
            /// <summary>
            /// Positive numbers can be storeable in a string
            /// </summary>
            PositiveNumber,
            /// <summary>
            /// Postal Codes can be storeable in a string
            /// </summary>
            PostalCode,
            /// <summary>
            /// Urls can be storeable in a string
            /// </summary>
            Url,
            /// <summary>
            /// Whole numbers can be storeable in a string
            /// </summary>
            WholeNumber,
            /// <summary>
            /// Zip Codes can be storeable in a string
            /// </summary>
            ZipCode
        }

        #region Validation Regex 

        /// <summary>
        /// Regular Expression used to validate string is not an integer
        /// </summary>
        public const string NotIntegerRegex = "[^0-9-]";

        /// <summary>
        /// Regular Expression used to validate string is not a natural number
        /// </summary>
        public const string NotNaturalNumberRegex = "[^0-9]";

        /// <summary>
        /// Regular Expression used to validate string is not a whole number
        /// </summary>
        public const string NotWholeNumberRegex = "[^0-9]";

        /// <summary>
        /// Regular Expression used to validate string is not an positive number
        /// </summary>
        public const string NotPositiveNumberRegex = "[^0-9.]";

        /// <summary>
        /// Regular Expression used to validate string is not a number
        /// </summary>
        public const string NotNumberRegex = "^[0-9.-]";

        /// <summary>
        /// Regular Expression used to validate strings with alphabet characters only
        /// </summary>
        public const string AlphaRegex = "^[a-zA-Z]*$";

        /// <summary>
        /// Regular Expression used to validate strings with alphabet characters only
        /// </summary>
        /// <remarks>NEW in v.2.0.0.12</remarks>
        public const string AlphaRegexAllowSpace = @"^[a-zA-Z\s]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numeric characters
        /// </summary>
        public const string AlphaNumericRegex = "^[a-zA-Z0-9]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numeric characters
        /// </summary>
        /// <remarks>NEW in v.2.0.0.12</remarks>
        public const string AlphaNumericRegexAllowSpace = @"^[a-zA-Z0-9\s]*$";

        /// <summary>
        /// 
        /// </summary>
        public const string AlphaNumericOrUnderscoreRegex = "^[a-zA-Z0-9_]*$";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>NEW in v.2.0.0.12</remarks>
        public const string AlphaNumericOrUnderscoreRegexAllowSpace = @"[^a-zA-Z0-9_\s]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numeric and dash
        /// </summary>
        public const string AlphaNumericOrDashRegex = @"[^a-zA-Z0-9\-]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numberc and dash and space
        /// </summary>
        public const string AlphaNumericOrDashRegexAllowSpace = @"[^a-zA-Z0-9_\-\s]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numberc and dot
        /// </summary>
        public const string AlphaNumericOrDotRegex = @"[^a-zA-Z0-9.]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numberc and dot and space
        /// </summary>
        public const string AlphaNumericOrDotRegexAllowSpace = @"[^a-zA-Z0-9_.\-\s]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numeric and dot and dash
        /// </summary>
        public const string AlphaNumericOrDotOrDashRegex = @"[^a-zA-Z0-9.]*$";

        /// <summary>
        /// Regular Expression used to validate string with alphabet and numeric and dot and dash and space
        /// </summary>
        public const string AlphaNumericOrDotOrDashRegexAllowSpace = @"[^a-zA-Z0-9_.\-\s]*$";

        /// <summary>
        /// Alpha Numeric or Underscore or Dot or comma
        /// </summary>
        public const string AlphaNumericOrUnderscoreOrDotOrDashOrCommaRegex = @"^[a-zA-Z0-9_.,\-]*$";

        /// <summary>
        /// Alpha Numeric or Underscore or Dot or comma
        /// </summary>
        /// <remarks>NEW in v.2.0.0.12</remarks>
        public const string AlphaNumericOrUnderscoreOrDotOrDashOrCommaAllowSpaceRegex = @"^[a-zA-Z0-9_.,\-\s]*$";

        /// <summary>
        /// Regular Expression used to validate email addresses
        /// </summary>
        public const string EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// Regular Expression used to validate integers
        /// </summary>
        public const string IntegerRegex = "^([-]|[0-9])[0-9]*$";

        /// <summary>
        /// Regular Expression used to validate natural numbers
        /// </summary>
        public const string NaturalNumberRegex = "0*[1-9][0-9]*";

        /// <summary>
        /// Regular Expression used to validate real numbers
        /// </summary>
        public const string RealNumberRegex = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";

        /// <summary>
        /// Regular Expression used to validate phone numbers
        /// </summary>
        public const string PhoneRegex = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";

        /// <summary>
        /// Regular Expression used to validate positive numbers
        /// </summary>
        public const string PositiveNumberRegex = "^[.][0-9]+$|[0-9]*[.]*[0-9]+$";

        /// <summary>
        /// Regular Expression used to validate postal codes
        /// </summary>
        public const string PostalCodeRegex = "[A-Z]\\d[A-Z] \\d[A-Z]\\d";
        /// <summary>
        /// 
        /// </summary>
        public const string TwoDotPositiveNumberRegex = "[0-9]*[.][0-9]*[.][0-9]*";
        /// <summary>
        /// 
        /// </summary>
        public const string TwoMinusNumberRegex = "[0-9]*[-][0-9]*[-][0-9]*";

        /// <summary>
        /// Regular Expression used to validate Urls
        /// </summary>
        public const string UrlRegex = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
        
        /// <summary>
        /// Regular Expression used to validate Zip Codes
        /// </summary>
        public const string ZipCodeRegex = "\\d{5}(-\\d{4})?";

        #endregion

        /// <summary>
        /// Removes any possible injection code
        /// </summary>
        /// <param name="text">Text to clean</param>
        /// <returns></returns>
        public string SAFE_TEXT(string text)
        {
            return String.IsNullOrEmpty(text) ? text : text.Replace("<script", " ").Replace("javascript", " ");
        }

        #region Validator Functions (Is...)

        /// <summary>
        /// Returns true if string matches valid data type passed in
        /// </summary>
        /// <param name="validStringType">Type of data expected in text</param>
        /// <param name="text">string to validate</param>
        public bool IsValid(ValidStringTypes validStringType, string text)
        {
            switch (validStringType)
            {
                case ValidStringTypes.Alpha: return IsAlpha(text); 
                case ValidStringTypes.AlphaNumeric: return IsAlphaNumeric(text); 
                case ValidStringTypes.Email: return IsEmail(text); 
                case ValidStringTypes.Integer: return IsInteger(text); 
                case ValidStringTypes.NaturalNumber: return IsNaturalNumber(text); 
                case ValidStringTypes.Number: return IsNumber(text); 
                case ValidStringTypes.Phone: return IsPhoneNumber(text); 
                case ValidStringTypes.PositiveNumber: return IsPositiveNumber(text); 
                case ValidStringTypes.PostalCode: return IsPostalCode(text); 
                case ValidStringTypes.Url: return IsUrl(text); 
                case ValidStringTypes.WholeNumber: return IsWholeNumber(text); 
                case ValidStringTypes.ZipCode: return IsZipCode(text);
                default: return true;
            }
        }

        /// <summary>
        /// Checks if the string passed in is a valid email address
        /// </summary>
        /// <param name="text">string to validate</param>
        /// <returns></returns>
        public bool IsEmail(string text)
        {
            return (SAFE_TEXT(text) == text) && Regex.IsMatch(text.Trim(), EmailRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if the string passed in is a valid phone number
        /// </summary>
        /// <param name="text">string to validate</param>
        /// <returns></returns>
        public bool IsPhoneNumber(string text)
        {
            return (SAFE_TEXT(text) == text) && Regex.IsMatch(text.Trim(), PhoneRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if the string passed in is a valid postal code
        /// </summary>
        /// <param name="text">string to validate</param>
        /// <returns></returns>
        public bool IsPostalCode(string text)
        {
            return (SAFE_TEXT(text) == text) && Regex.IsMatch(text.Trim(), PostalCodeRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if the string passed in is a valid url
        /// </summary>
        /// <param name="text">string to validate</param>
        /// <returns></returns>
        public bool IsUrl(string text)
        {
            return (SAFE_TEXT(text) == text) && Regex.IsMatch(text.Trim(), UrlRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if the string passed in is a valid zip code
        /// </summary>
        /// <param name="text">string to validate</param>
        /// <returns></returns>
        public bool IsZipCode(string text)
        {
            return (SAFE_TEXT(text) == text) && Regex.IsMatch(text.Trim(), ZipCodeRegex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Checks if string passed in is a valid Positive Integer
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsNaturalNumber(String text)
        {
            var objNotNaturalPattern = new Regex(NotNaturalNumberRegex);
            var objNaturalPattern = new Regex(NaturalNumberRegex);
            return (SAFE_TEXT(text) == text) && (!objNotNaturalPattern.IsMatch(text) && objNaturalPattern.IsMatch(text));
        }

        /// <summary>
        /// Checks if string passed in is a valid Positive Integers with zero inclusive 
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsWholeNumber(String text)
        {
            var objNotWholePattern = new Regex(NotWholeNumberRegex);
            return (SAFE_TEXT(text) == text) && !objNotWholePattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid Integers both Positive and Negative 
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsInteger(String text)
        {
            var objNotIntPattern = new Regex(NotIntegerRegex);
            var objIntPattern = new Regex(IntegerRegex);
            return (SAFE_TEXT(text) == text) && (!objNotIntPattern.IsMatch(text) && objIntPattern.IsMatch(text));
        }

        /// <summary>
        /// Checks if string passed in is a valid Positive Number both Integer and Real 
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsPositiveNumber(String text)
        {
            var objNotPositivePattern = new Regex(NotPositiveNumberRegex);
            var objPositivePattern = new Regex(PositiveNumberRegex);
            var objTwoDotPattern = new Regex(TwoDotPositiveNumberRegex);
            return (SAFE_TEXT(text) == text) && (objNotPositivePattern.IsMatch(text) && objPositivePattern.IsMatch(text) && !objTwoDotPattern.IsMatch(text));
        }

        /// <summary>
        /// Checks if string passed in is a valid number or not
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsNumber(string text)
        {
            var objNotNumberPattern = new Regex(NotNaturalNumberRegex);
            var objTwoDotPattern = new Regex(TwoDotPositiveNumberRegex);
            var objTwoMinusPattern = new Regex(TwoMinusNumberRegex);
            var objNumberPattern = new Regex("(" + RealNumberRegex + ")|(" + IntegerRegex + ")");
            return (SAFE_TEXT(text) == text) && (objNotNumberPattern.IsMatch(text) &&
                                                 !objTwoDotPattern.IsMatch(text) &&
                                                 !objTwoMinusPattern.IsMatch(text) &&
                                                 objNumberPattern.IsMatch(text));
        }

        /// <summary>
        /// Checks if string passed in is a valid Alphabets
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsAlpha(String text)
        {
            return IsAlpha(text, false);
        }

        /// <summary>
        /// Checks if string passed in is a valid Alphabets
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <param name="allowSpace"></param>
        /// <returns></returns>
        /// <remarks>v.2.0.0.12 now supports allowing spaces</remarks>
        public bool IsAlpha(String text, bool allowSpace)
        {
            var objAlphaPattern = new Regex(allowSpace == true ? AlphaRegexAllowSpace : AlphaRegex);
            return (SAFE_TEXT(text) == text) && objAlphaPattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid AlphaNumeric
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsAlphaNumeric(String text)
        {
            return IsAlphaNumeric(text, false);
        }

        /// <summary>
        /// Checks if string passed in is a valid AlphaNumeric
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowSpace"></param>
        /// <returns></returns>
        /// <remarks>v.2.0.0.12 now supports allowing spaces</remarks>
        public bool IsAlphaNumeric(String text, bool allowSpace)
        { 
            var objAlphaNumericPattern = new Regex(allowSpace == true ? AlphaNumericRegexAllowSpace : AlphaNumericRegex);
            return (SAFE_TEXT(text) == text) && objAlphaNumericPattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid alphanumeric or a dash
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowSpace"></param>
        /// <returns></returns>
        public bool IsAlphaNumericOrDash(String text, bool allowSpace)
        {
            var objAlphaNumericPattern = new Regex(allowSpace == true ? AlphaNumericOrDashRegexAllowSpace : AlphaNumericOrDashRegex);
            return (SAFE_TEXT(text) == text) && objAlphaNumericPattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid Alphanumeric (may contain period or space)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowSpace"></param>
        /// <returns></returns>
        public bool IsAlphaNumericOrDot(String text, bool allowSpace)
        {
            var objAlphaNumericPattern = new Regex(allowSpace == true ? AlphaNumericOrDotRegexAllowSpace  : AlphaNumericOrDotRegex);
            return (SAFE_TEXT(text) == text) && objAlphaNumericPattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid AlphaNumeric (may contain underscore)
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        public bool IsAlphaNumericOrUnderscore(String text)
        {
            var objAlphaNumericPattern = new Regex(AlphaNumericOrUnderscoreRegex);
            return (SAFE_TEXT(text) == text) && objAlphaNumericPattern.IsMatch(text);
        }

        /// <summary>
        /// Checks if string passed in is a valid AlphaNumeric (may contain underscore, period or comma)
        /// </summary>
        /// <param name="text">string containing number to validate</param>
        /// <returns></returns>
        /// <remarks>NEW in v2.0.0.11</remarks>
        public bool IsAlphaNumericOrUnderscoreOrDotOrDashOrComma(String text)
        {
            return IsAlphaNumericOrUnderscoreOrDotOrDashOrComma(text, false);
        }

        /// <summary>
        /// Checks if string passed in is a valid AlphaNumeric (may contain underscore, period or comma)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowSpace"></param>
        /// <returns></returns>
        /// <remarks>v.2.0.0.12 now supports allowing spaces</remarks>
        public bool IsAlphaNumericOrUnderscoreOrDotOrDashOrComma(String text, bool allowSpace)
        { 
            var objAlphaNumericPattern = new Regex(allowSpace == true ? AlphaNumericOrUnderscoreOrDotOrDashOrCommaAllowSpaceRegex : AlphaNumericOrUnderscoreOrDotOrDashOrCommaRegex);
            return (SAFE_TEXT(text) == text) && objAlphaNumericPattern.IsMatch(text);
        }

        #endregion
    }
}
