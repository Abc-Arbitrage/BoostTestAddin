using System.Diagnostics;

namespace BoostTestVSPackage.Utils
{
    public static class Logger
    {
        private const string _cateogory = "BoostTestAddin";

        public static void Write(string message)
        {
            Trace.WriteLine(message, _cateogory);
        }
    }
}