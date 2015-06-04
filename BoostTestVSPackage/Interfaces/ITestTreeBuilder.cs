using BoostTestVSPackage.Model;

namespace BoostTestVSPackage.Interfaces
{
    public interface ITestTreeBuilder
    {
        TestTree ParseText(string text);
    }
}