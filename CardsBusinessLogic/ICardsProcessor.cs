
namespace CardsBusinessLogic
{
    public interface ICardsProcessor
    {
        ProcessingMessage ProcessRequest(CardsProcessorOptions options);
    }
}
