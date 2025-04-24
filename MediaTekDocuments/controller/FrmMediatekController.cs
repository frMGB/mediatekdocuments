using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Récupère toutes les étapes de suivi.
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivi()
        {
            return access.GetAllSuivi();
        }

        /// <summary>
        /// Récupère un livre par son Id.
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre.</param>
        /// <returns>L'objet Livre trouvé ou null.</returns>
        public Livre GetLivreById(string idLivre)
        {
            return access.GetLivreById(idLivre);
        }

        /// <summary>
        /// Récupère un DVD par son Id.
        /// </summary>
        /// <param name="idDvd">L'identifiant du DVD.</param>
        /// <returns>L'objet Dvd trouvé ou null.</returns>
        public Dvd GetDvdById(string idDvd)
        {
            return access.GetDvdById(idDvd);
        }

        /// <summary>
        /// Récupère les commandes associées à un livre.
        /// Utilise le ViewModel CommandeDocumentLivre pour inclure le libellé du suivi.
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre.</param>
        /// <returns>Liste d'objets CommandeDocumentLivre</returns>
        public List<CommandeDocumentLivre> GetCommandesLivre(string idLivre)
        {
            return access.GetCommandesLivre(idLivre);
        }

        /// <summary>
        /// Crée une nouvelle commande et sa ligne de document associée.
        /// </summary>
        /// <param name="commande">L'objet Commande à créer.</param>
        /// <param name="commandeDocument">L'objet CommandeDocument associé.</param>
        /// <returns>True si la création a réussi, sinon False.</returns>
        public bool CreerCommandeDocument(Commande commande, CommandeDocument commandeDocument)
        {
            return access.CreerCommandeDocument(commande, commandeDocument);
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande document.
        /// </summary>
        /// <param name="idCommande">L'ID de la commande document à modifier.</param>
        /// <param name="idSuivi">Le nouvel ID de l'étape de suivi.</param>
        /// <returns>True si la modification a réussi, sinon False.</returns>
        public bool ModifierSuiviCommande(string idCommande, int idSuivi)
        {
            return access.ModifierSuiviCommande(idCommande, idSuivi);
        }

        /// <summary>
        /// Supprime une commande et sa ligne de document associée.
        /// </summary>
        /// <param name="idCommande">L'ID de la commande à supprimer.</param>
        /// <returns>True si la suppression a réussi, sinon False.</returns>
        public bool SupprimerCommande(string idCommande)
        {
            return access.SupprimerCommande(idCommande);
        }
    }
}
