using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;

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
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
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
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Récupère toutes les étapes de suivi depuis la BDD.
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivi()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }

        /// <summary>
        /// Récupère un livre spécifique par son ID.
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre.</param>
        /// <returns>L'objet Livre trouvé ou null si non trouvé.</returns>
        public Livre GetLivreById(string idLivre)
        {
            String jsonIdLivre = convertToJson("id", idLivre);
            List<Livre> result = TraitementRecup<Livre>(GET, "livre/" + jsonIdLivre, null);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// Récupère les commandes (avec détails et suivi) pour un livre donné.
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre.</param>
        /// <returns>Liste d'objets CommandeDocumentLivre.</returns>
        public List<CommandeDocumentLivre> GetCommandesLivre(string idLivre)
        {
            String jsonIdLivre = convertToJson("idLivreDvd", idLivre);
            List<CommandeDocumentLivre> lesCommandes = TraitementRecup<CommandeDocumentLivre>(GET, "commandedocument/" + jsonIdLivre, null);
            return lesCommandes;
        }

        /// <summary>
        /// Crée une nouvelle commande et sa ligne de document associée via l'API.
        /// </summary>
        /// <param name="commande">L'objet Commande à créer.</param>
        /// <param name="commandeDocument">L'objet CommandeDocument associé.</param>
        /// <returns>True si la création a réussi (code 200), sinon False.</returns>
        public bool CreerCommandeDocument(Commande commande, CommandeDocument commandeDocument)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);

            String parametres = $"commande={jsonCommande}&commandeDoc={jsonCommandeDocument}";

            try
            {
                JObject retour = api.RecupDistant(POST, "commandedocument", parametres);
                return retour != null && (String)retour["code"] == "200";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de la commande via l'API : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande document via l'API.
        /// </summary>
        /// <param name="idCommande">L'ID de la commande document à modifier.</param>
        /// <param name="idSuivi">Le nouvel ID de l'étape de suivi.</param>
        /// <returns>True si la modification a réussi (code 200), sinon False.</returns>
        public bool ModifierSuiviCommande(string idCommande, int idSuivi)
        {
            String jsonIdSuivi = convertToJson("idSuivi", idSuivi);
            String parametres = "champs=" + jsonIdSuivi;

            try
            {
                JObject retour = api.RecupDistant(PUT, "commandedocument/" + idCommande, parametres);
                return retour != null && (String)retour["code"] == "200";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la modification du suivi (ID: {idCommande}) via l'API : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Supprime une commande via l'API.
        /// </summary>
        /// <param name="idCommande">L'ID de la commande à supprimer.</param>
        /// <returns>True si la suppression a réussi (code 200), sinon False.</returns>
        public bool SupprimerCommande(string idCommande)
        {
            try
            {
                JObject retour = api.RecupDistant(DELETE, "commande/" + idCommande, null);
                bool success = retour != null && (String)retour["code"] == "200";
                if (!success)
                {
                    Console.WriteLine($"Erreur retournée par l'API lors de la suppression (ID: {idCommande}): Code={(String)retour?["code"]} Message={(String)retour?["message"]}");
                }
                 return success;
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Erreur lors de la suppression de la commande (ID: {idCommande}) via l'API : " + ex.Message);
                 return false;
            }
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
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
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
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
        private String convertToJson(Object nom, Object valeur)
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

        /// <summary>
        /// Récupère un DVD spécifique par son ID.
        /// </summary>
        /// <param name="idDvd">L'identifiant du DVD.</param>
        /// <returns>L'objet Dvd trouvé ou null si non trouvé.</returns>
        public Dvd GetDvdById(string idDvd)
        {
            String jsonIdDvd = convertToJson("id", idDvd);
            List<Dvd> result = TraitementRecup<Dvd>(GET, "dvd/" + jsonIdDvd, null);
            return result?.FirstOrDefault();
        }
    }
}
