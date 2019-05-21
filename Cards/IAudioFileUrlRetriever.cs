using System;

namespace Cards
{
    public interface IAudioFileUrlRetriever
    {
        Uri GetAudioFileUri(LongmanWord word);
    }
}
