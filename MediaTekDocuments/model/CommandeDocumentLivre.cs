using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// ViewModel pour l'affichage des informations combinées d'une commande de livre/DVD et son suivi.
    /// Hérite de CommandeDocument pour réutiliser les propriétés communes.
    /// </summary>
    public class CommandeDocumentLivre : CommandeDocument
    {
        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la commande.
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'étape de suivi.
        /// </summary>
        public string LibelleSuivi { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CommandeDocumentLivre"/>.
        /// </summary>
        /// <param name="idCommande">L'identifiant de la commande.</param>
        /// <param name="dateCommande">La date de la commande.</param>
        /// <param name="montant">Le montant de la commande.</param>
        /// <param name="nbExemplaire">Le nombre d'exemplaires.</param>
        /// <param name="idLivreDvd">L'identifiant du livre ou DVD.</param>
        /// <param name="idSuivi">L'identifiant de l'étape de suivi.</param>
        /// <param name="libelleSuivi">Le libellé de l'étape de suivi.</param>
        public CommandeDocumentLivre(string idCommande, DateTime dateCommande, double montant,
                                     int nbExemplaire, string idLivreDvd, int idSuivi, string libelleSuivi)
            : base(idCommande, nbExemplaire, idLivreDvd, idSuivi)
        {
            this.DateCommande = dateCommande;
            this.Montant = montant;
            this.LibelleSuivi = libelleSuivi;
        }
    }
} 