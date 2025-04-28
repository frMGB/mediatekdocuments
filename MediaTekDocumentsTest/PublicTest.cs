using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class PublicTest
    {
        private string id = "77777";
        private string libelle = "Jeunesse";
        private Public publicTest;

        [TestInitialize]
        public void Setup()
        {
            publicTest = new Public(id, libelle);
        }

        [TestMethod]
        public void Public_Constructeur()
        {
            Assert.AreEqual(id, publicTest.Id);
            Assert.AreEqual(libelle, publicTest.Libelle);
        }

        [TestMethod]
        public void Public_ToString()
        {
            string result = publicTest.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 