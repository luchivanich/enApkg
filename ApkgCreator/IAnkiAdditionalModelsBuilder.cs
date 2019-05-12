namespace ApkgCreator
{
    public interface IAnkiAdditionalModelsBuilder
    {
        string BuildAnkiCol();
        string BuildAnkiDeckConfig(long configId);
        string BuildAnkiDeckInfo(long deckId, string deckName);
    }
}
