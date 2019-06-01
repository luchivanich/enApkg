using CardsBusinessLogic;

namespace consoleApp
{
    public class Application : IApplication
    {
        ICardsProcessor _cardsProcessor;

        public Application(ICardsProcessor cardsProcessor)
        {
            _cardsProcessor = cardsProcessor;
        }

        public void Run(InputOptions inputOptions)
        {
            var cardsProcessorOptions = new CardsProcessorOptions
            {
                OutputPath = inputOptions.OutputPath,
                PathToWordsFile = inputOptions.PathToWordsFile,
                NumberOfCardsToProcess = inputOptions.NumberOfCardsToProcess
            };

            _cardsProcessor.ProcessRequest(cardsProcessorOptions);
        }
    }
}
