using Newtonsoft.Json; // Ajout pour JsonProperty

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant une étape de suivi de commande.
    /// </summary>
    public class Suivi
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'étape de suivi.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'étape de suivi.
        /// Le nom "etape" correspond à la colonne dans la BDD.
        /// </summary>
        [JsonProperty("etape")]
        public string Libelle { get; set; }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Suivi"/>.
        /// </summary>
        /// <param name="id">L'identifiant de l'étape.</param>
        /// <param name="libelle">Le libellé de l'étape.</param>
        public Suivi(int id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// Retourne le libellé de l'étape pour l'affichage dans les listes déroulantes ou grilles.
        /// </summary>
        /// <returns>Le libellé de l'étape.</returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
} 