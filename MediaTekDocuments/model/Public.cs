﻿
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Public (public concerné par le document) hérite de Categorie
    /// </summary>
    public class Public : Categorie
    {
        /// <summary>
        /// Constructeur de la classe Public
        /// </summary>
        /// <param name="id">Id du public</param>
        /// <param name="libelle">Libellé du public</param>
        public Public(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
