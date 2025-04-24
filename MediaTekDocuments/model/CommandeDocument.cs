namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une ligne de commande pour un document (Livre ou DVD).
    /// </summary>
    public class CommandeDocument
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la ligne de commande.
        /// </summary>
        public string IdCommande { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre d'exemplaires commandés.
        /// </summary>
        public int NbExemplaire { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du livre ou DVD commandé.
        /// </summary>
        public string IdLivreDvd { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'étape de suivi.
        /// </summary>
        public int IdSuivi { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CommandeDocument"/>.
        /// </summary>
        /// <param name="idCommande">L'identifiant de la commande.</param>
        /// <param name="nbExemplaire">Le nombre d'exemplaires.</param>
        /// <param name="idLivreDvd">L'identifiant du livre ou DVD.</param>
        /// <param name="idSuivi">L'identifiant de l'étape de suivi.</param>
        public CommandeDocument(string idCommande, int nbExemplaire, string idLivreDvd, int idSuivi)
        {
            this.IdCommande = idCommande;
            this.NbExemplaire = nbExemplaire;
            this.IdLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
        }
    }
} 