using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class SuiviTest
    {
        private string id = "56546";
        private string libelle = "En cours";
        private Suivi suivi;

        [TestInitialize]
        public void Setup()
        {
            suivi = new Suivi(id, libelle);
        }

        [TestMethod]
        public void Suivi_Constructeur()
        {
            Assert.AreEqual(id, suivi.Id);
            Assert.AreEqual(libelle, suivi.Libelle);
        }

        [TestMethod]
        public void Suivi_ToString()
        {
            string result = suivi.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 