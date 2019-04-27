using System.Collections.Generic;

namespace Cards
{
    public interface IExamplesRetriever
    {
        List<string> GetExamples(LongmanWord word);
    }
}
