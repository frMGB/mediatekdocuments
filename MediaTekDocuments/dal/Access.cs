using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = ConfigurationManager.AppSettings["UriApi"];
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// Logger pour enregistrer les messages
        /// </summary>
        private static readonly Logger log = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day,
                              outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = ConfigurationManager.AppSettings["ApiAuthenticationString"];
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                log.Fatal(e, "Erreur lors de l'initialisation de l'accès API : {Message}", e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les étapes de suivi à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Categorie> GetAllSuivis()
        {
            IEnumerable<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return new List<Categorie>(lesSuivis);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans CreerExemplaire : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Retourne les commandes d'un document (livre ou DVD)
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandes = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesCommandes;
        }

        /// <summary>
        /// Création d'une commande de document (livre ou DVD)
        /// </summary>
        /// <param name="commande">CommandeDocument à créer</param>
        /// <returns>true si la création a réussi</returns>
        public bool CreerCommandeDocument(CommandeDocument commande)
        {
            string parametres = $"id={commande.Id}" +
                              $"&dateCommande={commande.DateCommande.ToString("yyyy-MM-dd")}" +
                              $"&montant={commande.Montant.ToString().Replace(",", ".")}" +
                              $"&nbExemplaire={commande.NbExemplaire}" +
                              $"&idLivreDvd={commande.IdLivreDvd}" +
                              $"&idSuivi={commande.IdSuivi}";

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument", parametres);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans CreerCommandeDocument : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification de l'étape de suivi d'une commande de document
        /// </summary>
        /// <param name="idCommande">id de la commande à modifier</param>
        /// <param name="idSuivi">nouvel id de l'étape de suivi</param>
        /// <returns>true si la modification a réussi</returns>
        public bool ModifierEtapeSuivi(string idCommande, string idSuivi)
        {
            string url = "commandedocument/" + idCommande;
            string parametres = "idSuivi=" + idSuivi;

            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, url, parametres);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans ModifierEtapeSuivi : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une commande de document (livre ou DVD) et de la commande associée
        /// </summary>
        /// <param name="idCommande">id de la commande à supprimer</param>
        /// <returns>true si la suppression a réussi</returns>
        public bool SupprimerCommandeDocument(string idCommande)
        {
            String jsonIdCommande = convertToJson("id", idCommande);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandedocument/" + jsonIdCommande, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans SupprimerCommandeDocument : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            String jsonIdRevue = convertToJson("id", idRevue);
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "commanderevue/" + jsonIdRevue, null);
            return lesAbonnements;
        }

        /// <summary>
        /// Création d'un abonnement
        /// </summary>
        /// <param name="abonnement">Abonnement à créer</param>
        /// <returns>true si la création a réussi</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            string parametres = $"id={abonnement.Id}" +
                              $"&dateCommande={abonnement.DateCommande.ToString("yyyy-MM-dd")}" +
                              $"&montant={abonnement.Montant.ToString().Replace(",", ".")}" +
                              $"&dateFinAbonnement={abonnement.DateFinAbonnement.ToString("yyyy-MM-dd")}" +
                              $"&idRevue={abonnement.IdRevue}";

            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "commanderevue", parametres);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans CreerAbonnement : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un abonnement et de la commande associée
        /// </summary>
        /// <param name="idAbonnement">id de l'abonnement à supprimer</param>
        /// <returns>true si la suppression a réussi</returns>
        public bool SupprimerAbonnement(string idAbonnement)
        {
            String jsonIdAbonnement = convertToJson("id", idAbonnement);
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "commanderevue/" + jsonIdAbonnement, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Erreur dans SupprimerAbonnement : {Message}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Vérifie si une parution se trouve dans la période d'abonnement
        /// </summary>
        /// <param name="dateCommande">Date de début d'abonnement</param>
        /// <param name="dateFinAbonnement">Date de fin d'abonnement</param>
        /// <param name="dateParution">Date de parution à vérifier</param>
        /// <returns>True si la parution est comprise dans l'abonnement</returns>
        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (dateParution >= dateCommande && dateParution <= dateFinAbonnement);
        }

        /// <summary>
        /// Récupère toutes les revues dont l'abonnement se termine dans moins de 30 jours
        /// </summary>
        /// <returns>Liste de tuples contenant les infos des revues et dates de fin d'abonnement</returns>
        public List<Tuple<Revue, DateTime>> GetAbonnementsFinProche()
        {
            List<Abonnement> abonnementsEnCours = TraitementRecup<Abonnement>(GET, "abofinproche", null);
            List<Tuple<Revue, DateTime>> revuesFin = new List<Tuple<Revue, DateTime>>();
            
            foreach (Abonnement abonnement in abonnementsEnCours)
            {
                Revue revue = GetRevue(abonnement.IdRevue);
                if (revue != null)
                {
                    revuesFin.Add(new Tuple<Revue, DateTime>(revue, abonnement.DateFinAbonnement));
                }
            }
            
            // Trier par date de fin d'abonnement
            return revuesFin.OrderBy(r => r.Item2).ToList();
        }

        /// <summary>
        /// Tente d'authentifier un utilisateur via l'API REST.
        /// </summary>
        /// <param name="login">Login de l'utilisateur.</param>
        /// <param name="password">Mot de passe de l'utilisateur.</param>
        /// <returns>Un objet Utilisateur si l'authentification réussit, sinon null.</returns>
        public Utilisateur AuthenticateUser(string login, string password)
        {
            string parametres = $"login={Uri.EscapeDataString(login)}&password={Uri.EscapeDataString(password)}";
            try
            {
                JObject retour = api.RecupDistant(POST, "auth", parametres);
                
                if (retour != null && retour["code"] != null && (int?)retour["code"] == 200)
                {
                    if (retour["result"] != null && retour["result"].Type == JTokenType.Object)
                    {
                        Utilisateur utilisateur = retour["result"].ToObject<Utilisateur>();
                        if (utilisateur != null && !string.IsNullOrEmpty(utilisateur.Login) && utilisateur.IdService > 0) 
                        {
                            return utilisateur; 
                        }
                        else
                        {
                            log.Warning("Erreur AuthenticateUser : Données utilisateur invalides reçues de l'API.");
                            return null;
                        }
                    }
                    else
                    {
                        log.Warning("Erreur AuthenticateUser : La clé 'result' est manquante ou n'est pas un objet JSON dans la réponse API.");
                        return null;
                    }
                }
                else if (retour != null && retour["code"] != null)
                {
                    log.Warning("Erreur AuthenticateUser (API) : Code {ApiCode} - Message: {ApiMessage}", retour["code"], retour["message"]);
                    return null;
                }
                else
                {
                    log.Warning("Erreur AuthenticateUser : Réponse invalide ou nulle de l'API.");
                    return null;
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Exception dans AuthenticateUser : {Message}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Récupère une revue par son id
        /// </summary>
        /// <param name="idRevue">id de la revue</param>
        /// <returns>La revue correspondante ou null</returns>
        private Revue GetRevue(string idRevue)
        {
            String jsonIdRevue = convertToJson("id", idRevue);
            List<Revue> revues = TraitementRecup<Revue>(GET, "revue/" + jsonIdRevue, null);
            if (revues.Count > 0)
            {
                return revues[0];
            }
            return null;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    log.Warning("Erreur API dans TraitementRecup : code erreur = {ApiCode} message = {ApiMessage}", code, (String)retour["message"]);
                }
            }catch(Exception e)
            {
                log.Fatal(e, "Erreur fatale lors de l'accès à l'API dans TraitementRecup : {Message}", e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private static String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
