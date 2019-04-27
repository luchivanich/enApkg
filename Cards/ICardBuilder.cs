namespace Cards
{
    public interface ICardBuilder
    {
        Card BuildNewCard(LongmanWord word);
        void BuildCard(Card card, LongmanWord word);
    }
}
