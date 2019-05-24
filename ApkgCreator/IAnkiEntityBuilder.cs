using ApkgCreator.DataModels;
using Cards;

namespace ApkgCreator
{
    public interface IAnkiEntityBuilder
    {
        AnkiCol BuildAnkiCol();

        AnkiCard BuildAnkiCard(Card card, AnkiCol ankiCol);
    }
}
