using System.Collections.Generic;

namespace Cards
{
    public interface ICardDbService
    {
        void SaveCard(CardsDbContext db, Card card);
        Card GetCard(string word);
        List<Card> GetCards();
    }
}
