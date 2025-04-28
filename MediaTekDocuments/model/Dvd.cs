
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Dvd hérite de LivreDvd : contient des propriétés spécifiques aux dvd
    /// </summary>
    public class Dvd : LivreDvd
    {
        /// <summary>
        /// Duree du dvd
        /// </summary>
        public int Duree { get; }

        /// <summary>
        /// Realisateur du dvd
        /// </summary>
        public string Realisateur { get; }

        /// <summary>
        /// Synopsis du dvd
        /// </summary>
        public string Synopsis { get; }

        /// <summary>
        /// Constructeur de la classe Dvd
        /// </summary>
        /// <param name="id">Id du dvd</param>
        /// <param name="titre">Titre du dvd</param>
        /// <param name="image">Image du dvd</param>
        /// <param name="duree">Duree du dvd</param>
        /// <param name="realisateur">Realisateur du dvd</param>
        /// <param name="synopsis">Synopsis du dvd</param>
        /// <param name="idGenre">Id du genre du dvd</param>
        /// <param name="genre">Genre du dvd</param>
        /// <param name="idPublic">Id du public du dvd</param>
        /// <param name="lePublic">Public du dvd</param>
        /// <param name="idRayon">Id du rayon du dvd</param>
        /// <param name="rayon">Rayon du dvd</param>
        public Dvd(string id, string titre, string image, int duree, string realisateur, string synopsis,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.Duree = duree;
            this.Realisateur = realisateur;
            this.Synopsis = synopsis;
        }

    }
}
