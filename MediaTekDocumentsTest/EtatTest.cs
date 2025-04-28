using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class EtatTest
    {
        private string id = "46452";
        private string libelle = "Neuf";
        private Etat etat;

        [TestInitialize]
        public void Setup()
        {
            etat = new Etat(id, libelle);
        }

        [TestMethod]
        public void Etat_Constructeur()
        {
            Assert.AreEqual(id, etat.Id);
            Assert.AreEqual(libelle, etat.Libelle);
        }

        [TestMethod]
        public void Id_TestRetour()
        {
            string newId = "48625";
            etat.Id = newId;
            Assert.AreEqual(newId, etat.Id);
        }

        [TestMethod]
        public void Libelle_TestRetour()
        {
            string newLibelle = "Usagé";
            etat.Libelle = newLibelle;
            Assert.AreEqual(newLibelle, etat.Libelle);
        }
    }
} 