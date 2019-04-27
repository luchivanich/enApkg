using System;
using System.Collections.Generic;
using System.Linq;

namespace Cards
{
    public class CardDbService : ICardsDbService
    {
        public Card GetCard(string word)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCards()
        {
            throw new NotImplementedException();
        }

        public void SaveCard(CardsDbContext db, Card card)
        {
            var dbCard = db.Cards.FirstOrDefault(i => i.Word == card.Word && i.LexicalCategory == card.LexicalCategory);
            if (dbCard == null)
            {
                db.Cards.Add(card);
            }
            else
            {
                dbCard.Word = card.Word;
                dbCard.LexicalCategory = card.LexicalCategory;
                dbCard.Definition = card.Definition;
                dbCard.Examples = card.Examples;
            }
        }
    }
}
