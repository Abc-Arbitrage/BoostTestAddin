// PkgCmdID.cs
// MUST match PkgCmdID.h

namespace BoostTestVSPackage
{
    static class PkgCmdIdList
    {
        public const uint cmdidDebugCurrentTest = 0x100;
        public const uint cmdidRunCurrentTest = 0x101;
        public const uint cmdidRunCurrentSuite = 0x103;
        public const uint cmdidRunCurrentProject = 0x104;
        
        public const uint cmdidDetectMemoryLeak = 0x106;
        public const uint cmdidOptionShowProgress = 0x107;
        public const uint cmdidOptionRunInRandomOrder = 0x108;
    };
}