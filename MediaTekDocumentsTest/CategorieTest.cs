using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CategorieTest
    {
        private string id = "1654";
        private string libelle = "Drame";
        private Categorie categorie;

        [TestInitialize]
        public void Setup()
        {
            categorie = new Categorie(id, libelle);
        }

        [TestMethod]
        public void Categorie_Constructeur()
        {
            Assert.AreEqual(id, categorie.Id);
            Assert.AreEqual(libelle, categorie.Libelle);
        }

        [TestMethod]
        public void ToString_TestRetour()
        {
            string result = categorie.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 