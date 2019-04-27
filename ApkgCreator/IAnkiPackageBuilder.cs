using Cards;
using System.Collections.Generic;

namespace ApkgCreator
{
    public interface IAnkiPackageBuilder
    {
        void BuildApkgPackage(string targetPath, string targetName, List<Card> cards);
    }
}
