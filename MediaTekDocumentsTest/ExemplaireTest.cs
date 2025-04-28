using System;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class ExemplaireTest
    {
        private int numero = 1;
        private DateTime dateAchat = new DateTime(2014, 1, 5);
        private string photo = "photo.jpg";
        private string idEtat = "45616";
        private string idDocument = "65449";
        private Exemplaire exemplaire;

        [TestInitialize]
        public void Setup()
        {
            exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
        }

        [TestMethod]
        public void Exemplaire_Constructeur()
        {
            Assert.AreEqual(numero, exemplaire.Numero);
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
            Assert.AreEqual(photo, exemplaire.Photo);
            Assert.AreEqual(idEtat, exemplaire.IdEtat);
            Assert.AreEqual(idDocument, exemplaire.Id);
        }

        [TestMethod]
        public void Numero_TestRetour()
        {
            int newNumero = 2;
            exemplaire.Numero = newNumero;
            Assert.AreEqual(newNumero, exemplaire.Numero);
        }

        [TestMethod]
        public void Photo_TestRetour()
        {
            string newPhoto = "nouvelle_photo.jpg";
            exemplaire.Photo = newPhoto;
            Assert.AreEqual(newPhoto, exemplaire.Photo);
        }

        [TestMethod]
        public void DateAchat_TestRetour()
        {
            DateTime newDateAchat = new DateTime(2054, 2, 4);
            exemplaire.DateAchat = newDateAchat;
            Assert.AreEqual(newDateAchat, exemplaire.DateAchat);
        }

        [TestMethod]
        public void IdEtat_TestRetour()
        {
            string newIdEtat = "98456";
            exemplaire.IdEtat = newIdEtat;
            Assert.AreEqual(newIdEtat, exemplaire.IdEtat);
        }

        [TestMethod]
        public void Id_TestRetour()
        {
            string newId = "65425";
            exemplaire.Id = newId;
            Assert.AreEqual(newId, exemplaire.Id);
        }
    }
} 