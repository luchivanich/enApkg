using System;
using System.Collections.Generic;
using System.IO;
using ApkgCreator;
using Cards;
using Newtonsoft.Json;

namespace CardsBusinessLogic
{
    public class CardsProcessor : ICardsProcessor
    {
        private ICardBuilder _cardBuilder;
        private IAnkiPackageBuilder _ankiPackageBuilder;

        private CardsProcessorOptions _options;

        private List<Card> _cardsToAdd = new List<Card>();

        public CardsProcessor(ICardBuilder cardBuilder, IAnkiPackageBuilder ankiPackageBuilder)
        {
            _cardBuilder = cardBuilder;
            _ankiPackageBuilder = ankiPackageBuilder;
        }

        public void ProcessRequest(CardsProcessorOptions options)
        {
            _options = options;
            SaveWordsIntoCardsDb();
            CreateApkgFile();
        }

        private void SaveWordsIntoCardsDb()
        {
            var cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(_options.PathToWordsFile));
            for (var i = 0; i < Math.Min(cards.Count, _options.NumberOfCardsToProcess ?? int.MaxValue); i++)
            {
                var card = cards[i];
                _cardBuilder.BuildCard(card);
                if (!string.IsNullOrWhiteSpace(card.Definition))
                {
                    _cardsToAdd.Add(card);
                }
                else
                {
                    Console.WriteLine($"Unable to get word definition: {card.Word}"); // TODO: Rework to event
                }
            }
        }

        private void CreateApkgFile()
        {
            _ankiPackageBuilder.BuildApkgPackage(_options.OutputPath, Path.GetFileName(_options.OutputPath), _cardsToAdd);
        }
    }
}
