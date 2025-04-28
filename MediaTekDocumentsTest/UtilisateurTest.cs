using MediaTekDocuments.model;

namespace MediaTekDocumentsTest
{
    [TestClass]
    public class UtilisateurTest
    {
        private string login = "moriarty";
        private string password = "mot2passe";
        private int idService = 1;
        private Utilisateur utilisateur;
        private Utilisateur utilisateurAvecService;
        private Utilisateur utilisateurVide;

        [TestInitialize]
        public void Setup()
        {
            utilisateur = new Utilisateur(login, password);
            utilisateurAvecService = new Utilisateur(login, idService);
            utilisateurVide = new Utilisateur();
        }

        [TestMethod]
        public void Utilisateur_ConstructeurTest1()
        {
            Assert.AreEqual(login, utilisateur.Login);
            Assert.AreEqual(password, utilisateur.Password);
            Assert.AreEqual(0, utilisateur.IdService);
        }

        [TestMethod]
        public void Utilisateur_ConstructeurTest2()
        {
            Assert.AreEqual(login, utilisateurAvecService.Login);
            Assert.IsNull(utilisateurAvecService.Password);
            Assert.AreEqual(idService, utilisateurAvecService.IdService);
        }

        [TestMethod]
        public void Utilisateur_ConstructeurTest3()
        {
            Assert.IsNull(utilisateurVide.Login);
            Assert.IsNull(utilisateurVide.Password);
            Assert.AreEqual(0, utilisateurVide.IdService);
        }

        [TestMethod]
        public void Login_TestRetour()
        {
            string newLogin = "sherlock";
            utilisateur.Login = newLogin;
            Assert.AreEqual(newLogin, utilisateur.Login);
        }

        [TestMethod]
        public void Password_TestRetour()
        {
            string newPassword = "motdepasse";
            utilisateur.Password = newPassword;
            Assert.AreEqual(newPassword, utilisateur.Password);
        }

        [TestMethod]
        public void IdService_TestRetour()
        {
            int newIdService = 2;
            utilisateur.IdService = newIdService;
            Assert.AreEqual(newIdService, utilisateur.IdService);
        }
    }
} 