using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApkgCreator;
using Cards;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CardsBusinessLogic
{
    public class CardsProcessor : ICardsProcessor
    {
        private ICardsDbContext _cardDbContext;
        private ICardBuilder _cardBuilder;
        private IAnkiPackageBuilder _ankiPackageBuilder;

        private List<Card> _cardsToAdd = new List<Card>();

        public CardsProcessor(ICardsDbContext cardDbContext, ICardBuilder cardBuilder, IAnkiPackageBuilder ankiPackageBuilder)
        {
            _cardDbContext = cardDbContext;
            _cardBuilder = cardBuilder;
            _ankiPackageBuilder = ankiPackageBuilder;
        }

        public void ProcessRequest(CardsProcessorOptions options)
        {
            SaveWordsIntoCardsDb(options.PathToWordsFile);
            CreateApkgFile(options.OutputPath);
        }

        private void SaveWordsIntoCardsDb(string pathToWordsFile)
        {
            var words = JsonConvert.DeserializeObject<List<LongmanWord>>(File.ReadAllText(pathToWordsFile));
            foreach (var word in words)
            {
                var card = _cardDbContext.Cards.Include(c => c.Examples).FirstOrDefault(c => c.Word == word.Word && (word.LexicalCategory == null || word.LexicalCategory == c.LexicalCategory));
                if (card == null)
                {
                    card = _cardBuilder.BuildNewCard(word);
                    _cardDbContext.Cards.Add(card);
                }
                else
                {
                    _cardBuilder.BuildCard(card, word);
                }
                _cardsToAdd.Add(card);
            }
            var count = _cardDbContext.SaveChanges();

        }

        private void CreateApkgFile(string outputPath)
        {
            _ankiPackageBuilder.BuildApkgPackage(outputPath, Path.GetFileName(outputPath), _cardsToAdd);
        }
    }
}
