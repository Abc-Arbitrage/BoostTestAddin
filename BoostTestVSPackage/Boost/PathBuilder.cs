using BoostTestVSPackage.Model;

namespace BoostTestVSPackage.Boost
{
    public static class PathBuilder
    {
        private const char _separator = '/';

        public static string GetPath(TestSuite testItem)
        {
            if (testItem.ParentSuite != null && !testItem.ParentSuite.IsRoot)
            {
                var path = GetPath(testItem.ParentSuite);
                return path + _separator + testItem.Name;
            }

            return testItem.Name;
        }

        public static string GetPath(TestCase testCase)
        {
            string result;

            if (testCase.ParentSuite != null && !testCase.ParentSuite.IsRoot)
            {
                var path = GetPath(testCase.ParentSuite);
                result = path + _separator + testCase.Name;
            }
            else 
                result = testCase.Name;

            if (testCase.IsTemplate)
                result += '*';

            return result;
        }
    }
}