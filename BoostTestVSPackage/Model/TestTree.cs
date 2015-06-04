namespace BoostTestVSPackage.Model
{
    public class TestTree
    {
        public TestSuite Root { get; private set; }

        public TestTree()
        {
            Root = new TestSuite(string.Empty, 0, null);
        }

        public override string ToString()
        {
            return Root.ToString();
        }
    }
}