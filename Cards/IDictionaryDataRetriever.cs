using System.Collections.Generic;

namespace Cards
{
    public interface IDictionaryDataRetriever
    {
        string GetDefinition(IWord word);

        List<string> GetExamples(IWord word);

        (string fileName, byte[] fileData) GetAudioFile(IWord word);
    }
}
