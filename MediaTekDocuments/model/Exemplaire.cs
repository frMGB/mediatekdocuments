﻿using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// Numéro de l'exemplaire
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Photo de l'exemplaire
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Date d'achat de l'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// Id de l'état de l'exemplaire
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// Id du document
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Constructeur de la classe Exemplaire
        /// </summary>
        /// <param name="numero">Numéro de l'exemplaire</param>
        /// <param name="dateAchat">Date d'achat de l'exemplaire</param>
        /// <param name="photo">Photo de l'exemplaire</param>
        /// <param name="idEtat">Id de l'état de l'exemplaire</param>
        /// <param name="idDocument">Id du document</param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Id = idDocument;
        }

    }
}
