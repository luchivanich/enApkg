namespace ApkgCreator
{
    public interface IAnkiAdditionalModelsBuilder
    {
        string BuildAnkiCol();
        string BuildAnkiDeckInfo(long deckId, string deckName);
    }
}
