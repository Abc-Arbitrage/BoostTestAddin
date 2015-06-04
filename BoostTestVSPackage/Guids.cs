// Guids.cs
// MUST match guids.h

using System;

namespace BoostTestVSPackage
{
    static class GuidList
    {
        public const string guidBoostTestVSPackagePkgString = "0a66f06a-ea9f-449b-bb85-02e20553b898";
        public const string guidBoostTestVSPackageCmdSetString = "b1828bb5-b0ac-44dc-8a9d-6cab93baa19c";

        public static readonly Guid guidBoostTestVSPackageCmdSet = new Guid(guidBoostTestVSPackageCmdSetString);
    };
}