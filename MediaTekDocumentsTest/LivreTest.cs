using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class LivreTest
    {
        private string id = "64651";
        private string titre = "Livre Test";
        private string image = "image.jpg";
        private string isbn = "9781234567897";
        private string auteur = "Auteur Test";
        private string collection = "Collection Test";
        private string idGenre = "98512";
        private string genre = "Roman";
        private string idPublic = "65488";
        private string lePublic = "Adultes";
        private string idRayon = "88888";
        private string rayon = "Magazines";
        private Livre livre;

        [TestInitialize]
        public void Setup()
        {
            livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        }

        [TestMethod]
        public void Livre_Constructeur()
        {
            Assert.AreEqual(id, livre.Id);
            Assert.AreEqual(titre, livre.Titre);
            Assert.AreEqual(image, livre.Image);
            Assert.AreEqual(isbn, livre.Isbn);
            Assert.AreEqual(auteur, livre.Auteur);
            Assert.AreEqual(collection, livre.Collection);
            Assert.AreEqual(idGenre, livre.IdGenre);
            Assert.AreEqual(genre, livre.Genre);
            Assert.AreEqual(idPublic, livre.IdPublic);
            Assert.AreEqual(lePublic, livre.Public);
            Assert.AreEqual(idRayon, livre.IdRayon);
            Assert.AreEqual(rayon, livre.Rayon);
        }
    }
} 