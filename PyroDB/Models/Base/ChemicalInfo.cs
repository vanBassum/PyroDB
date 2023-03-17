using PyroDB.Models.Database;

namespace PyroDB.Models.Base
{
    public class ChemicalInfo
    {
        public Chemical DbChemical { get; set; }
        public bool Owned { get; set; } = false;


        public ChemicalInfo(Chemical dbChemical, bool owned)
        {
            DbChemical = dbChemical;
            Owned = owned;
        }


    }
}
