using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class RevueTest
    {
        private string id = "54665";
        private string titre = "Revue Test";
        private string image = "image.jpg";
        private string idGenre = "55544";
        private string genre = "Essai";
        private string idPublic = "88888";
        private string lePublic = "Ados";
        private string idRayon = "22222";
        private string rayon = "Beaux Livres";
        private string periodicite = "Mensuel";
        private int delaiMiseADispo = 30;
        private Revue revue;

        [TestInitialize]
        public void Setup()
        {
            revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
        }

        [TestMethod]
        public void Revue_Constructeur()
        {
            Assert.AreEqual(id, revue.Id);
            Assert.AreEqual(titre, revue.Titre);
            Assert.AreEqual(image, revue.Image);
            Assert.AreEqual(idGenre, revue.IdGenre);
            Assert.AreEqual(genre, revue.Genre);
            Assert.AreEqual(idPublic, revue.IdPublic);
            Assert.AreEqual(lePublic, revue.Public);
            Assert.AreEqual(idRayon, revue.IdRayon);
            Assert.AreEqual(rayon, revue.Rayon);
            Assert.AreEqual(periodicite, revue.Periodicite);
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo);
        }

        [TestMethod]
        public void Periodicite_TestRetour()
        {
            string newPeriodicite = "Hebdomadaire";
            revue.Periodicite = newPeriodicite;
            Assert.AreEqual(newPeriodicite, revue.Periodicite);
        }

        [TestMethod]
        public void DelaiMiseADispo_TestRetour()
        {
            int newDelai = 7;
            revue.DelaiMiseADispo = newDelai;
            Assert.AreEqual(newDelai, revue.DelaiMiseADispo);
        }
    }
} 