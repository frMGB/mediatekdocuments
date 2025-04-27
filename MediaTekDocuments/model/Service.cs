namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Service
    /// Représente un service de la médiathèque (Administratif, Commandes, Culture)
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Identifiant numérique du service
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom du service
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="id">Id du service</param>
        /// <param name="libelle">Libellé du service</param>
        public Service(int id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Retourne le libellé du service pour l'affichage
        /// </summary>
        /// <returns>Libellé du service</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
} 