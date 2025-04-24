using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une Commande.
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la commande.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime DateCommande { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la commande.
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Commande"/>.
        /// </summary>
        /// <param name="id">L'identifiant de la commande.</param>
        /// <param name="dateCommande">La date de la commande.</param>
        /// <param name="montant">Le montant de la commande.</param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
} 