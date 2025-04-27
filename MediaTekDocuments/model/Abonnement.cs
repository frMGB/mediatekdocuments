using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un abonnement à une revue
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// Date de fin d'abonnement
        /// </summary>
        public DateTime DateFinAbonnement { get; set; }
        
        /// <summary>
        /// Identifiant de la revue concernée
        /// </summary>
        public string IdRevue { get; set; }
        
        /// <summary>
        /// Constructeur de la classe Abonnement
        /// </summary>
        /// <param name="id">Identifiant de l'abonnement</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de l'abonnement</param>
        /// <param name="dateFinAbonnement">Date de fin d'abonnement</param>
        /// <param name="idRevue">Identifiant de la revue</param>
        public Abonnement(string id, DateTime dateCommande, double montant, 
            DateTime dateFinAbonnement, string idRevue) 
            : base(id, dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
        }
    }
} 