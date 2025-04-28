
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier LivreDvd hérite de Document
    /// </summary>
    public abstract class LivreDvd : Document
    {
        /// <summary>
        /// Constructeur de la classe LivreDvd
        /// </summary>
        /// <param name="id">Id du livre ou dvd</param>
        /// <param name="titre">Titre du livre ou dvd</param>
        /// <param name="image">Image du livre ou dvd</param>
        /// <param name="idGenre">Id du genre du livre ou dvd</param>
        /// <param name="genre">Genre du livre ou dvd</param>
        /// <param name="idPublic">Id du public du livre ou dvd</param>
        /// <param name="lePublic">Public du livre ou dvd</param>
        /// <param name="idRayon">Id du rayon du livre ou dvd</param>
        /// <param name="rayon">Rayon du livre ou dvd</param>
        protected LivreDvd(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
        }

    }
}
