using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class DvdTest
    {
        private string id = "98743";
        private string titre = "Film Test";
        private string image = "image.jpg";
        private int duree = 213;
        private string realisateur = "Réalisateur Test";
        private string synopsis = "Synopsis Test";
        private string idGenre = "84572";
        private string genre = "Humour";
        private string idPublic = "65413";
        private string lePublic = "Adultes";
        private string idRayon = "98462";
        private string rayon = "DVD films";
        private Dvd dvd;

        [TestInitialize]
        public void Setup()
        {
            dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        }

        [TestMethod]
        public void Dvd_Constructeur()
        {
            Assert.AreEqual(id, dvd.Id);
            Assert.AreEqual(titre, dvd.Titre);
            Assert.AreEqual(image, dvd.Image);
            Assert.AreEqual(duree, dvd.Duree);
            Assert.AreEqual(realisateur, dvd.Realisateur);
            Assert.AreEqual(synopsis, dvd.Synopsis);
            Assert.AreEqual(idGenre, dvd.IdGenre);
            Assert.AreEqual(genre, dvd.Genre);
            Assert.AreEqual(idPublic, dvd.IdPublic);
            Assert.AreEqual(lePublic, dvd.Public);
            Assert.AreEqual(idRayon, dvd.IdRayon);
            Assert.AreEqual(rayon, dvd.Rayon);
        }
    }
} 