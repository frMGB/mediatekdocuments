namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une étape de suivi d'une commande
    /// </summary>
    public class Suivi : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Suivi
        /// </summary>
        /// <param name="id">Identifiant de l'étape de suivi</param>
        /// <param name="libelle">Libellé de l'étape de suivi</param>
        public Suivi(string id, string libelle) : base(id, libelle)
        {
        }
    }
} 