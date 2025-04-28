using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class GenreTest
    {
        private string id = "94658";
        private string libelle = "Science Fiction";
        private Genre genre;

        [TestInitialize]
        public void Setup()
        {
            genre = new Genre(id, libelle);
        }

        [TestMethod]
        public void Genre_Constructeur()
        {
            Assert.AreEqual(id, genre.Id);
            Assert.AreEqual(libelle, genre.Libelle);
        }

        [TestMethod]
        public void Genre_ToString()
        {
            string result = genre.ToString();
            Assert.AreEqual(libelle, result);
        }
    }
} 