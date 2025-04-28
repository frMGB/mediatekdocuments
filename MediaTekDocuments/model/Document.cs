
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Id du document
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Titre du document
        /// </summary>
        public string Titre { get; }

        /// <summary>
        /// Image du document
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// Id du genre du document
        /// </summary>
        public string IdGenre { get; }

        /// <summary>
        /// Genre du document
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Id du public du document
        /// </summary>
        public string IdPublic { get; }

        /// <summary>
        /// Public du document
        /// </summary>
        public string Public { get; }

        /// <summary>
        /// Id du rayon du document
        /// </summary>
        public string IdRayon { get; }

        /// <summary>
        /// Rayon du document
        /// </summary>
        public string Rayon { get; }

        /// <summary>
        /// Constructeur de la classe Document
        /// </summary>
        /// <param name="id">Id du document</param>
        /// <param name="titre">Titre du document</param>
        /// <param name="image">Image du document</param>
        /// <param name="idGenre">Id du genre du document</param>
        /// <param name="genre">Genre du document</param>
        /// <param name="idPublic">Id du public du document</param>
        /// <param name="lePublic">Public du document</param>
        /// <param name="idRayon">Id du rayon du document</param>
        /// <param name="rayon">Rayon du document</param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
