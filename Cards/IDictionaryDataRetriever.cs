using System.Collections.Generic;

namespace Cards
{
    public interface IDictionaryDataRetriever
    {
        (string word, string definition, List<string> examples, string fileName, byte[] fileData) GetDictionaryData(IWord word);
    }
}
