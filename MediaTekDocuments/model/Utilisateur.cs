namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur (minimaliste pour l'authentification et la gestion des droits)
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant de connexion
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Mot de passe (utilisé pour l'envoi à l'API lors de la tentative de connexion)
        /// Ce champ ne devrait idéalement pas être stocké après l'authentification réussie.
        /// L'API renverra l'objet Utilisateur sans le mot de passe.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Identifiant du service de l'utilisateur pour la gestion des droits
        /// </summary>
        public int IdService { get; set; }

        /// <summary>
        /// Constructeur pour la tentative de connexion (avant validation par l'API)
        /// </summary>
        /// <param name="login">Login saisi</param>
        /// <param name="password">Password saisi</param>
        public Utilisateur(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
        
        /// <summary>
        /// Constructeur utilisé lors de la désérialisation de la réponse API (sans le mot de passe)
        /// </summary>
        /// <param name="login">Login récupéré</param>
        /// <param name="idService">IdService récupéré</param>
        public Utilisateur(string login, int idService) 
        {
            this.Login = login;
            this.Password = null;
            this.IdService = idService;
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Utilisateur() { }
    }
} 