namespace BoostTestVSPackage.Model
{
    public class TestItem
    {
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public TestSuite ParentSuite { get; set; }
    }
}