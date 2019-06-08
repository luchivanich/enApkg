using System;
using System.IO;

namespace ApkgCreator
{
    public class ResourceManager : IResourceManager
    {
        public string LoadFromResource(Type type, string path)
        {
            var assembly = type.Assembly;
            var resource = assembly.GetManifestResourceStream(path);
            using (var reader = new StreamReader(resource))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
