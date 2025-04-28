using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class AbonnementTest
    {
        private string id = "94546";
        private DateTime dateCommande = new DateTime(2016, 3, 3);
        private double montant = 54.0;
        private DateTime dateFinAbonnement = new DateTime(2018, 6, 6);
        private string idRevue = "48966";
        private Abonnement abonnement;

        [TestInitialize]
        public void Setup()
        {
            abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
        }

        [TestMethod]
        public void Abonnement_Constructeur()
        {
            Assert.AreEqual(id, abonnement.Id);
            Assert.AreEqual(dateCommande, abonnement.DateCommande);
            Assert.AreEqual(montant, abonnement.Montant);
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement);
            Assert.AreEqual(idRevue, abonnement.IdRevue);
        }

        [TestMethod]
        public void DateFinAbonnement_TestRetour()
        {
            DateTime newDate = new DateTime(2030, 3, 30);
            abonnement.DateFinAbonnement = newDate;
            Assert.AreEqual(newDate, abonnement.DateFinAbonnement);
        }

        [TestMethod]
        public void IdRevue_TestRetour()
        {
            string newIdRevue = "65485";
            abonnement.IdRevue = newIdRevue;
            Assert.AreEqual(newIdRevue, abonnement.IdRevue);
        }
    }
}