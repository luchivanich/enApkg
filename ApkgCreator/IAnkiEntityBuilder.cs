using Cards;

namespace ApkgCreator
{
    public interface IAnkiEntityBuilder
    {
        AnkiCol BuildAnkiCol(long deckId, long modelId);

        AnkiCard BuildAnkiCard(Card card, long deckId, long modelId);
    }
}
