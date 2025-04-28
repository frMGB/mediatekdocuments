using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class CommandeTest
    {
        private string id = "94215";
        private DateTime dateCommande = new DateTime(2038, 4, 4);
        private double montant = 1869.0;
        private Commande commande;

        [TestInitialize]
        public void Setup()
        {
            commande = new Commande(id, dateCommande, montant);
        }

        [TestMethod]
        public void Commande_Constructeur()
        {
            Assert.AreEqual(id, commande.Id);
            Assert.AreEqual(dateCommande, commande.DateCommande);
            Assert.AreEqual(montant, commande.Montant);
        }

        [TestMethod]
        public void Id_TestRetour()
        {
            string newId = "59626";
            commande.Id = newId;
            Assert.AreEqual(newId, commande.Id);
        }

        [TestMethod]
        public void DateCommande_TestRetour()
        {
            DateTime newDate = new DateTime(2025, 10, 20);
            commande.DateCommande = newDate;
            Assert.AreEqual(newDate, commande.DateCommande);
        }

        [TestMethod]
        public void Montant_TestRetour()
        {
            double newMontant = 27.0;
            commande.Montant = newMontant;
            Assert.AreEqual(newMontant, commande.Montant);
        }
    }
} 