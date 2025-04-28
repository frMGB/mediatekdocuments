using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CommandeDocumentTest
    {
        private string id = "41589";
        private DateTime dateCommande = new DateTime(1999, 11, 22);
        private double montant = 142.0;
        private int nbExemplaire = 13;
        private string idLivreDvd = "14873";
        private string idSuivi = "1";
        private string libelleSuivi = "En cours";
        private CommandeDocument commandeDocument;

        [TestInitialize]
        public void Setup()
        {
            commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelleSuivi);
        }

        [TestMethod]
        public void CommandeDocument_Constructeur()
        {
            Assert.AreEqual(id, commandeDocument.Id);
            Assert.AreEqual(dateCommande, commandeDocument.DateCommande);
            Assert.AreEqual(montant, commandeDocument.Montant);
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire);
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd);
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi);
            Assert.AreEqual(libelleSuivi, commandeDocument.LibelleSuivi);
        }

        [TestMethod]
        public void NbExemplaire_TestRetour()
        {
            int newNbExemplaire = 84;
            commandeDocument.NbExemplaire = newNbExemplaire;
            Assert.AreEqual(newNbExemplaire, commandeDocument.NbExemplaire);
        }

        [TestMethod]
        public void IdLivreDvd_TestRetour()
        {
            string newIdLivreDvd = "65168";
            commandeDocument.IdLivreDvd = newIdLivreDvd;
            Assert.AreEqual(newIdLivreDvd, commandeDocument.IdLivreDvd);
        }

        [TestMethod]
        public void IdSuivi_TestRetour()
        {
            string newIdSuivi = "59625";
            commandeDocument.IdSuivi = newIdSuivi;
            Assert.AreEqual(newIdSuivi, commandeDocument.IdSuivi);
        }

        [TestMethod]
        public void LibelleSuivi_TestRetour()
        {
            string newLibelleSuivi = "Livrée";
            commandeDocument.LibelleSuivi = newLibelleSuivi;
            Assert.AreEqual(newLibelleSuivi, commandeDocument.LibelleSuivi);
        }
    }
} 