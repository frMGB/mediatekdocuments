using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class RayonTest
    {
        private string id = "55555";
        private string libelle = "Presse sportive";
        private Rayon rayon;

        [TestInitialize]
        public void Setup()
        {
            rayon = new Rayon(id, libelle);
        }

        [TestMethod]
        public void Rayon_Constructeur()
        {
            Assert.AreEqual(id, rayon.Id);
            Assert.AreEqual(libelle, rayon.Libelle);
        }

        [TestMethod]
        public void Rayon_ToString()
        {
            string result = rayon.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 