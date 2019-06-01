using System.Collections.Generic;
using ApkgCreator.AdditionalModels;
using ApkgCreator.DataModels;
using Cards;

namespace ApkgCreator
{
    public interface IAnkiFieldsBuilder
    {
        string BuildFieldsString(Card card, List<Fld> fields);

        Dictionary<string, string> BuildFieldsPairs(AnkiNote note, AnkiCol ankiCol);
    }
}
