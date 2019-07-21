/********************************************************************************
 * CSHARP Text Library - General utility to manipulate text strings
 * 
 * NOTE: Adapted from Clinch.Text
 * 
 * LICENSE: Free to use provided details on fixes and/or extensions emailed to 
 *          chris.williams@readwatchcreate.com
 ********************************************************************************/

using System.Text;
using System.IO;

namespace CSHARP.Text
{
    /// <summary>
    /// String Writer that encodes in UTF-8
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        /// <summary>
        /// Writes string with UTF8 Encoding
        /// </summary>
        /// <param name="stringBuilder">Builder containing content to write</param>
        public Utf8StringWriter(StringBuilder stringBuilder)
            : base(stringBuilder)
        {
        }

        /// <summary>
        /// Sets the encoding
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
