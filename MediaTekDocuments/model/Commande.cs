using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une commande
    /// </summary>
    public class Commande
    {
        /// <summary>
        /// Identifiant de la commande
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Date de la commande
        /// </summary>
        public DateTime DateCommande { get; set; }
        
        /// <summary>
        /// Montant de la commande
        /// </summary>
        public double Montant { get; set; }
        
        /// <summary>
        /// Constructeur de la classe Commande
        /// </summary>
        /// <param name="id">Identifiant de la commande</param>
        /// <param name="dateCommande">Date de la commande</param>
        /// <param name="montant">Montant de la commande</param>
        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
} 