using System.Collections.Generic;

namespace BoostTestVSPackage.Model
{
    public class TestSuite : TestItem
    {
        public bool IsRoot
        {
            get { return ParentSuite == null; }
        }

        public List<TestSuite> Suites { get; private set; }
        public IList<TestCase> TestCases { get; private set; }

        public TestSuite(string name, int lineNumber, TestSuite parentSuite)
        {
            Name = name;
            LineNumber = lineNumber;
            ParentSuite = parentSuite;

            TestCases = new List<TestCase>();
            Suites = new List<TestSuite>();
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, LineNumber: {1}, ParentSuite: {2}, Suites: {3}, TestCases: {4}",
                                 Name, LineNumber, ParentSuite.Name, Suites.Count, TestCases.Count);
        }
    }
}