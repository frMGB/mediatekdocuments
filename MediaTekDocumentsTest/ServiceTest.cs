using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class ServiceTest
    {
        private int id = 1;
        private string libelle = "Commandes";
        private Service service;

        [TestInitialize]
        public void Setup()
        {
            service = new Service(id, libelle);
        }

        [TestMethod]
        public void Service_Constructeur()
        {
            Assert.AreEqual(id, service.Id);
            Assert.AreEqual(libelle, service.Libelle);
        }

        [TestMethod]
        public void Id_TestRetour()
        {
            int newId = 2;
            service.Id = newId;
            Assert.AreEqual(newId, service.Id);
        }

        [TestMethod]
        public void Libelle_TestRetour()
        {
            string newLibelle = "Culture";
            service.Libelle = newLibelle;
            Assert.AreEqual(newLibelle, service.Libelle);
        }

        [TestMethod]
        public void Service_ToString()
        {
            string result = service.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 