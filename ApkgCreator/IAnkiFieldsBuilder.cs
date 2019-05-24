using System.Collections.Generic;
using ApkgCreator.AdditionalModels;
using Cards;

namespace ApkgCreator
{
    public interface IAnkiFieldsBuilder
    {
        string BuildFieldsString(Card card, List<Fld> fields);
    }
}
