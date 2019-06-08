using System;

namespace ApkgCreator
{
    public interface IResourceManager
    {
        string LoadFromResource(Type type, string path);
    }
}
