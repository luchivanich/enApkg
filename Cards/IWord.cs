namespace Cards
{
    public interface IWord
    {
        string Word { get; set; }

        LexicalCategory? LexicalCategory { get; set; }
    }
}