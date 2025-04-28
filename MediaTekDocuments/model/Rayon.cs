﻿
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Rayon (rayon de classement du document) hérite de Categorie
    /// </summary>
    public class Rayon : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Rayon
        /// </summary>
        /// <param name="id">Id du rayon</param>
        /// <param name="libelle">Libellé du rayon</param>
        public Rayon(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
