using System;
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
        /// getter sur les étapes de suivi
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Categorie> GetAllSuivis()
        {
            return access.GetAllSuivis();
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
        /// Récupère les commandes d'un livre
        /// </summary>
        /// <param name="idDocument">Id du livre concerné</param>
        /// <returns>Liste des commandes de ce livre</returns>
        public List<CommandeDocument> GetCommandesLivre(string idDocument)
        {
            return access.GetCommandesLivre(idDocument);
        }

        /// <summary>
        /// Récupère les commandes d'un DVD
        /// </summary>
        /// <param name="idDocument">Id du DVD concerné</param>
        /// <returns>Liste des commandes de ce DVD</returns>
        public List<CommandeDocument> GetCommandesDvd(string idDocument)
        {
            return access.GetCommandesDvd(idDocument);
        }

        /// <summary>
        /// Crée une commande de document (livre ou DVD)
        /// </summary>
        /// <param name="commande">La commande à créer</param>
        /// <returns>True si la création a réussi</returns>
        public bool CreerCommandeDocument(CommandeDocument commande)
        {
            return access.CreerCommandeDocument(commande);
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande
        /// </summary>
        /// <param name="idCommande">Id de la commande</param>
        /// <param name="idSuivi">Nouvel id de l'étape de suivi</param>
        /// <returns>True si la modification a réussi</returns>
        public bool ModifierEtapeSuivi(string idCommande, string idSuivi)
        {
            return access.ModifierEtapeSuivi(idCommande, idSuivi);
        }

        /// <summary>
        /// Supprime une commande de document
        /// </summary>
        /// <param name="idCommande">Id de la commande à supprimer</param>
        /// <returns>True si la suppression a réussi</returns>
        public bool SupprimerCommandeDocument(string idCommande)
        {
            return access.SupprimerCommandeDocument(idCommande);
        }

        /// <summary>
        /// Récupère les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">Id de la revue concernée</param>
        /// <returns>Liste des abonnements de cette revue</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            return access.GetAbonnementsRevue(idRevue);
        }

        /// <summary>
        /// Crée un abonnement (commande de revue)
        /// </summary>
        /// <param name="abonnement">L'abonnement à créer</param>
        /// <returns>True si la création a réussi</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Supprime un abonnement
        /// </summary>
        /// <param name="idAbonnement">Id de l'abonnement à supprimer</param>
        /// <returns>True si la suppression a réussi</returns>
        public bool SupprimerAbonnement(string idAbonnement)
        {
            return access.SupprimerAbonnement(idAbonnement);
        }

        /// <summary>
        /// Vérifie si une parution se trouve dans la période d'un abonnement
        /// </summary>
        /// <param name="dateCommande">Date de début d'abonnement</param>
        /// <param name="dateFinAbonnement">Date de fin d'abonnement</param>
        /// <param name="dateParution">Date de parution à vérifier</param>
        /// <returns>True si la parution est dans la période d'abonnement</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return access.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);
        }

        /// <summary>
        /// Récupère les revues dont l'abonnement se termine dans moins de 30 jours
        /// </summary>
        /// <returns>Liste de tuples (revue, date de fin)</returns>
        public List<Tuple<Revue, DateTime>> GetAbonnementsFinProche()
        {
            return access.GetAbonnementsFinProche();
        }

        /// <summary>
        /// Tente d'authentifier un utilisateur.
        /// </summary>
        /// <param name="login">Login de l'utilisateur.</param>
        /// <param name="password">Mot de passe de l'utilisateur.</param>
        /// <returns>L'objet Utilisateur si l'authentification réussit, sinon null.</returns>
        public Utilisateur AuthenticateUser(string login, string password)
        {
            return access.AuthenticateUser(login, password);
        }
    }
}
