namespace CSHARPStandard.Text.Csv
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Assists in working with CSV
    /// </summary>
    public class CsvStringHelper
    {
        /// <summary>
        /// Converts a list of objects to CSV
        /// </summary>
        /// <param name="collection">Collection of objects to generate CSV from</param>
        /// <param name="type">type of object being generated</param>
        /// <returns>csv string</returns>
        /// <remarks>V2.0.0.2 Case Corrected in method name</remarks>
        public string ConvertToCsv(IEnumerable collection, Type type)
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
    }
}
