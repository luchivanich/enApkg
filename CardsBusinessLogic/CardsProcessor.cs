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

        private List<Card> _cardsToAdd = new List<Card>();

        public CardsProcessor(ICardBuilder cardBuilder, IAnkiPackageBuilder ankiPackageBuilder)
        {
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
            var cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(pathToWordsFile));
            foreach (var card in cards)
            {
                _cardBuilder.BuildCard(card);
                if (!string.IsNullOrWhiteSpace(card.Definition))
                {
                    _cardsToAdd.Add(card);
                }
            }
        }

        private void CreateApkgFile(string outputPath)
        {
            _ankiPackageBuilder.BuildApkgPackage(outputPath, Path.GetFileName(outputPath), _cardsToAdd);
        }
    }
}
