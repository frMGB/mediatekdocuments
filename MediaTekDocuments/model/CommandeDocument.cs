namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une commande de document (livre ou DVD)
    /// </summary>
    public class CommandeDocument : Commande
    {
        /// <summary>
        /// Nombre d'exemplaires commandés
        /// </summary>
        public int NbExemplaire { get; set; }
        
        /// <summary>
        /// Identifiant du livre ou DVD
        /// </summary>
        public string IdLivreDvd { get; set; }
        
        /// <summary>
        /// Étape de suivi de la commande
        /// </summary>
        public string IdSuivi { get; set; }
        
        /// <summary>
        /// Libellé de l'étape de suivi
        /// </summary>
        public string LibelleSuivi { get; set; }
        
        /// <summary>
        /// Constructeur de la classe CommandeDocument
        /// </summary>
        /// <param name="id">Identifiant de la commande</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de la commande</param>
        /// <param name="nbExemplaire">Nombre d'exemplaires</param>
        /// <param name="idLivreDvd">Identifiant du livre ou DVD</param>
        /// <param name="idSuivi">Identifiant de l'étape de suivi</param>
        /// <param name="libelleSuivi">Libellé de l'étape de suivi</param>
        public CommandeDocument(string id, System.DateTime dateCommande, double montant, int nbExemplaire, 
            string idLivreDvd, string idSuivi, string libelleSuivi) 
            : base(id, dateCommande, montant)
        {
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
            LibelleSuivi = libelleSuivi;
        }
    }
} 