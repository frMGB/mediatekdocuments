using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class DocumentTest
    {
        private string id = "65412";
        private string titre = "Titre Test";
        private string image = "image.jpg";
        private string idGenre = "65416";
        private string genre = "Science Fiction";
        private string idPublic = "65444";
        private string lePublic = "Adultes";
        private string idRayon = "85214";
        private string rayon = "Maison";
        private Document document;

        [TestInitialize]
        public void Setup()
        {
            document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        }

        [TestMethod]
        public void Document_Constructeur()
        {
            Assert.AreEqual(id, document.Id);
            Assert.AreEqual(titre, document.Titre);
            Assert.AreEqual(image, document.Image);
            Assert.AreEqual(idGenre, document.IdGenre);
            Assert.AreEqual(genre, document.Genre);
            Assert.AreEqual(idPublic, document.IdPublic);
            Assert.AreEqual(lePublic, document.Public);
            Assert.AreEqual(idRayon, document.IdRayon);
            Assert.AreEqual(rayon, document.Rayon);
        }
    }
} 