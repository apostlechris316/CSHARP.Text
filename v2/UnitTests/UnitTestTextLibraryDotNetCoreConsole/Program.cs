namespace UnitTestTextLibraryDotNetCoreConsole
{
    using CSHARPStandard.Text;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            #region Negative Testing - Things we expect to fail  

            //try { var expectedFailure = NEGATIVE_TEST_InsertTestHere(null, string.Empty, string.Empty, null);}
            //catch (Exception exception)
            //{
            //    Console.WriteLine("******* STEP 1.1.1 - PASSED : EXPECTED FAILURE with no drive service passed: ");
            //    Console.WriteLine(exception.ToString());
            //    Console.WriteLine("*******");
            //    Console.ReadKey();
            //}

            #endregion

            #region Positive Testing Things we expect to work

            #region Test public static string ConvertToAlphaNumeric(string toConvert, bool removeWhiteSpace) 

            // Test with an empty string
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false", string.Empty, string.Empty, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true", string.Empty, string.Empty, true) == false)
                return;

            // Test with whitespace characters
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - '\t12 34\t', RemoveWhiteSpace = false", "\t12 34\t", "\t12 34\t", false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - '\t12 34\t', RemoveWhiteSpace = true", "1234", "\t12 34\t", false) == false)
                return;

            #endregion

            #region Test public static string ConvertToAlphaNumeric(string toConvert, bool removeWhiteSpace, bool removeUnderScore) 

            // Test with an empty string
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = false", string.Empty, string.Empty, false, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = true", string.Empty, string.Empty, false, true) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = false", string.Empty, string.Empty, true, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = true", string.Empty, string.Empty, false, true) == false)
                return;

            // Testd with whitespace characters but no underscores
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = false", "\t12 34\t", "\t12 34\t", false, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = true", "\t12 34\t", "\t12 34\t", false, true) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = false", "\t12 34\t", "1234", true, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = true", "\t12 34\t", "1234", true, true) == false)
                return;

            // Test with whitespace and underscore characters
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = false", "\t12_ _34\t", "\t12_ _34\t", false, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = false, removeUnderScore = true", "\t12_ _34\t", "\t12 34\t", false, true) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = false", "\t12_ _34\t", "12__34", true, false) == false)
                return;
            if (POSITIVE_TEST_ConvertToAlplaNumeric("ConvertToAlphaNumeric - string.Empty, RemoveWhiteSpace = true, removeUnderScore = true", "\t12_ _34\t", "1234", true, true) == false)
                return;

            #endregion

            #endregion

            Console.WriteLine("Unit Testing Text .NET Standard LIbrary Successful");
            Console.ReadKey();
        }

        /// <summary>
        /// Runs a positive test on ConvertToAlphaNumeric
        /// </summary>
        /// <param name="failMessage">message to display on failure</param>
        /// <param name="expectedResult">result we expect</param>
        /// <param name="toConvert">string to convert</param>
        /// <param name="removeWhiteSpace">If true, removes tabs and white spaces</param>
        /// <returns></returns>
        public static bool POSITIVE_TEST_ConvertToAlplaNumeric(string failMessage, string expectedResult, string toConvert, bool removeWhiteSpace)
        {
            // Declare helper classes
            var stringHelper = new StringHelper();

            var result = stringHelper.ConvertToAlphaNumeric(toConvert, false);
            if (result != expectedResult)
            {
                Console.WriteLine(string.Format("******* ERROR: {0} - Test Failed converting({1}) expected({2}). Result was({3}) *******", failMessage, toConvert, expectedResult, result));
                Console.ReadKey();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Runs a positive test on ConvertToAlphaNumeric
        /// </summary>
        /// <param name="failMessage">message to display on failure</param>
        /// <param name="expectedResult">result we expect</param>
        /// <param name="toConvert">string to convert</param>
        /// <param name="removeWhiteSpace">If true, removes tabs and white spaces</param>
        /// <param name="removeUnderScore">If true, removes _ as well</param>
        /// <returns></returns>
        public static bool POSITIVE_TEST_ConvertToAlplaNumeric(string failMessage, string expectedResult, string toConvert, bool removeWhiteSpace, bool removeUnderScore)
        {
            // Declare helper classes
            var stringHelper = new StringHelper();

            var result = stringHelper.ConvertToAlphaNumeric(toConvert, removeWhiteSpace, removeUnderScore);
            if (result != expectedResult)
            {
                Console.WriteLine(string.Format("******* ERROR: {0} - Test Failed converting({1}) expected({2}). Result was({3}) *******", failMessage, toConvert, expectedResult, result));
                Console.ReadKey();
                return false;
            }

            return true;
        }
    }
}
