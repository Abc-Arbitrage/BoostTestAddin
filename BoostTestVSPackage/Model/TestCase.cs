namespace BoostTestVSPackage.Model
{
    public class TestCase : TestItem
    {
        public bool IsTemplate { get; private set; }

        public TestCase(string name, int lineNumber, TestSuite parentSuite, bool isTemplate)
        {
            IsTemplate = isTemplate;
            LineNumber = lineNumber;
            Name = name;
            ParentSuite = parentSuite;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, LineNumber: {1}, ParentSuite: {2}, IsTemplate: {3}", 
                Name, LineNumber, ParentSuite.Name, IsTemplate);
        }
    }
}