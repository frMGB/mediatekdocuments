
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Revue hérite de Document : contient des propriétés spécifiques aux revues
    /// </summary>
    public class Revue : Document
    {
        /// <summary>
        /// Période de publication de la revue
        /// </summary>
        public string Periodicite { get; set; }

        /// <summary>
        /// Délai de mise à disposition de la revue
        /// </summary>
        public int DelaiMiseADispo { get; set; }

        /// <summary>
        /// Constructeur de la classe Revue
        /// </summary>
        /// <param name="id">Id de la revue</param>
        /// <param name="titre">Titre de la revue</param>
        /// <param name="image">Image de la revue</param>
        /// <param name="idGenre">Id du genre de la revue</param>
        /// <param name="genre">Genre de la revue</param>
        /// <param name="idPublic">Id du public de la revue</param>
        /// <param name="lePublic">Public de la revue</param>
        /// <param name="idRayon">Id du rayon de la revue</param>
        /// <param name="rayon">Rayon de la revue</param>
        /// <param name="periodicite">Période de publication de la revue</param>
        /// <param name="delaiMiseADispo">Délai de mise à disposition de la revue</param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon,
            string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            DelaiMiseADispo = delaiMiseADispo;
        }

    }
}
