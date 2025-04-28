using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.IO;
using System.Xml.Linq;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Variables globales
        /// <summary>
        /// Contrôleur de la fenêtre principale
        /// </summary>
        private readonly FrmMediatekController controller;
        /// <summary>
        /// BindingSource pour les genres
        /// </summary>
        private readonly BindingSource bdgGenres = new BindingSource();
        /// <summary>
        /// BindingSource pour les publics
        /// </summary>
        private readonly BindingSource bdgPublics = new BindingSource();
        /// <summary>
        /// BindingSource pour les rayons
        /// </summary>
        private readonly BindingSource bdgRayons = new BindingSource();
        /// <summary>
        /// BindingSource pour les étapes de suivi
        /// </summary>
        private readonly BindingSource bdgSuivis = new BindingSource();
        /// <summary>
        /// Liste des étapes de suivi
        /// </summary>
        private List<Categorie> lesEtapesSuivi;
        /// <summary>
        /// ID du service de l'utilisateur connecté
        /// </summary>
        private readonly int idServiceUtilisateurConnecte;

        #endregion

        #region Commun

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire et application des droits
        /// </summary>
        /// <param name="idService">ID du service de l'utilisateur connecté</param>
        internal FrmMediatek(int idService)
        {
            InitializeComponent();
            this.idServiceUtilisateurConnecte = idService;
            this.controller = new FrmMediatekController();

            try 
            {
                this.lesEtapesSuivi = controller.GetAllSuivis();
                 if (this.lesEtapesSuivi == null) {
                    MessageBox.Show("Impossible de charger les étapes de suivi depuis la base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.lesEtapesSuivi = new List<Categorie>(); 
                 }
                 else
                 {
                    bdgSuivis.DataSource = this.lesEtapesSuivi; 
                 }
            }
            catch (Exception ex)
            {
                 MessageBox.Show($"Erreur lors du chargement initial des étapes de suivi : {ex.Message} .", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 this.lesEtapesSuivi = new List<Categorie>(); 
                 
            }


            ApplyAccessRights(); 

        }

        /// <summary>
        /// Applique les restrictions d'accès en fonction du service de l'utilisateur.
        /// </summary>
        private void ApplyAccessRights()
        {


            switch (this.idServiceUtilisateurConnecte)
            {
                case 1:
                    tabOngletsApplication.TabPages.Remove(tabCommandesLivres);
                    tabOngletsApplication.TabPages.Remove(tabCommandesDvd);
                    tabOngletsApplication.TabPages.Remove(tabCommandesRevues);

                    if(grpReceptionExemplaire != null) grpReceptionExemplaire.Enabled = false;
                    if(cbxModifierSuiviLivre != null) cbxModifierSuiviLivre.Visible = false;
                    if(btnModifierSuiviLivre != null) btnModifierSuiviLivre.Visible = false;
                    if(cbxModifierSuiviDvd != null) cbxModifierSuiviDvd.Visible = false;
                    if(btnModifierSuiviDvd != null) btnModifierSuiviDvd.Visible = false;
                    break;

                case 2:
                    break;

                case 3:
                    MessageBox.Show("Vos droits d'accès ne vous permettent pas d'utiliser cette application.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Shown += (s, args) => this.Close(); 
                    break;

                default:
                    MessageBox.Show("Votre service utilisateur n'est pas reconnu. L'accès est refusé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     this.Shown += (s, args) => this.Close();
                    break;
            }
        }
        
        /// <summary>
        /// Se déclenche juste avant que le formulaire ne soit affiché pour la première fois.
        /// Idéal pour afficher les alertes modales.
        /// </summary>
        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            if (this.idServiceUtilisateurConnecte == 2) 
            {
                try
                {
                    List<Tuple<Revue, DateTime>> abonnementsFinProche = controller.GetAbonnementsFinProche();
                    if (abonnementsFinProche != null && abonnementsFinProche.Count > 0)
                    {
                        using (FrmAlerteAbonnements frmAlerte = new FrmAlerteAbonnements(abonnementsFinProche))
                        {
                           frmAlerte.ShowDialog();
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la récupération ou l'affichage de l'alerte de fin d'abonnement : " + ex.Message);
                    MessageBox.Show("Une erreur s'est produite lors de la vérification des fins d'abonnement.", "Erreur Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public static void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commandes Livres

        /// <summary>
        /// Objets et variables liés à l'onglet de commandes de livres
        /// </summary>
        private readonly BindingSource bdgCommandesLivres = new BindingSource();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();
        private List<Livre> lesLivresCommandes = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Commandes de livres : 
        /// récupère les livres et charge les étapes de suivi dans les combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            if (lesLivresCommandes.Count == 0)
            {
                lesLivresCommandes = controller.GetAllLivres();
            }
            AccesCommandeLivresGroupBox(false);
            txbCommandesLivresNumRecherche.Text = "";
            
            
            
            ViderCommandesLivreInfos();
            ViderCommandesLivresListe();
        }

        /// <summary>
        /// Remplit le datagrid des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">Liste des commandes</param>
        private void RemplirCommandesLivresListe(List<CommandeDocument> commandes)
        {
            if (commandes != null)
            {
                bdgCommandesLivres.DataSource = commandes;
                dgvCommandesLivres.DataSource = bdgCommandesLivres;
                dgvCommandesLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvCommandesLivres.Columns["DateCommande"].HeaderText = "Date";
                dgvCommandesLivres.Columns["DateCommande"].DefaultCellStyle.Format = "d";
                dgvCommandesLivres.Columns["Montant"].HeaderText = "Montant";
                dgvCommandesLivres.Columns["NbExemplaire"].HeaderText = "Nb exemplaires";
                dgvCommandesLivres.Columns["LibelleSuivi"].HeaderText = "Étape";
                dgvCommandesLivres.Columns["Id"].Visible = false;
                dgvCommandesLivres.Columns["IdLivreDvd"].Visible = false;
                dgvCommandesLivres.Columns["IdSuivi"].Visible = false;

                dgvCommandesLivres.Columns["DateCommande"].DisplayIndex = 0;
                dgvCommandesLivres.Columns["Montant"].DisplayIndex = 1;
                dgvCommandesLivres.Columns["NbExemplaire"].DisplayIndex = 2;
                dgvCommandesLivres.Columns["LibelleSuivi"].DisplayIndex = 3;
            }
            else
            {
                dgvCommandesLivres.DataSource = null;
            }
        }

        /// <summary>
        /// Vide la liste des commandes de livres
        /// </summary>
        private void ViderCommandesLivresListe()
        {
            dgvCommandesLivres.DataSource = null;
        }

        /// <summary>
        /// Recherche et affiche les informations du livre dont on a saisi le numéro.
        /// Affiche également la liste de ses commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandesLivresNumRecherche.Text.Equals(""))
            {
                Livre livre = lesLivresCommandes.Find(x => x.Id.Equals(txbCommandesLivresNumRecherche.Text));
                if (livre != null)
                {
                    AfficheCommandesLivresInfos(livre);
                    lesCommandesLivres = controller.GetCommandesDocument(livre.Id);
                    RemplirCommandesLivresListe(lesCommandesLivres);
                    AccesCommandeLivresGroupBox(true);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    txbCommandesLivresNumRecherche.Text = "";
                    txbCommandesLivresNumRecherche.Focus();
                    ViderCommandesLivreInfos();
                    ViderCommandesLivresListe();
                    AccesCommandeLivresGroupBox(false);
                }
            }
            else
            {
                ViderCommandesLivreInfos();
                ViderCommandesLivresListe();
                AccesCommandeLivresGroupBox(false);
            }
        }

        /// <summary>
        /// Si le numéro de livre est modifié, la zone de recherche est vidée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbCommandesLivresNumRecherche_TextChanged(object sender, EventArgs e)
        {
            ViderCommandesLivresListe();
            ViderCommandesLivreInfos();
            AccesCommandeLivresGroupBox(false);
        }

        /// <summary>
        /// Affiche les informations du livre sélectionné
        /// </summary>
        /// <param name="livre">Le livre à afficher</param>
        private void AfficheCommandesLivresInfos(Livre livre)
        {
            txbCommandesLivresNumero.Text = livre.Id;
            txbCommandesLivresTitre.Text = livre.Titre;
            txbCommandesLivresAuteur.Text = livre.Auteur;
            txbCommandesLivresIsbn.Text = livre.Isbn;
            txbCommandesLivresCollection.Text = livre.Collection;
            txbCommandesLivresGenre.Text = livre.Genre;
            txbCommandesLivresPublic.Text = livre.Public;
            txbCommandesLivresRayon.Text = livre.Rayon;
            txbCommandesLivresImage.Text = livre.Image;
            string image = livre.Image;
            try
            {
                pcbCommandesLivresImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCommandesLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void ViderCommandesLivreInfos()
        {
            txbCommandesLivresNumero.Text = "";
            txbCommandesLivresTitre.Text = "";
            txbCommandesLivresAuteur.Text = "";
            txbCommandesLivresIsbn.Text = "";
            txbCommandesLivresCollection.Text = "";
            txbCommandesLivresGenre.Text = "";
            txbCommandesLivresPublic.Text = "";
            txbCommandesLivresRayon.Text = "";
            txbCommandesLivresImage.Text = "";
            pcbCommandesLivresImage.Image = null;
        }

        /// <summary>
        /// Active ou désactive l'accès à la gestion des commandes de livres
        /// </summary>
        /// <param name="acces">true pour activer, false pour désactiver</param>
        private void AccesCommandeLivresGroupBox(bool acces)
        {
            grpCommandesLivresAjout.Enabled = acces;
            grpCommandesLivresRecherche.Enabled = true;
        }

        /// <summary>
        /// Tri sur les colonnes du DataGridView dgvCommandesLivres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesLivres.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date":
                    sortedList = lesCommandesLivres.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Étape":
                    sortedList = lesCommandesLivres.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCommandesLivresListe(sortedList);
        }

        /// <summary>
        /// Sur le clic du bouton d'ajout d'une commande livre, 
        /// vérifie les données puis ajoute la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesLivresValider_Click(object sender, EventArgs e)
        {
            if (txbCommandesLivresMontant.Text.Equals("") || txbCommandesLivresNbExemplaires.Text.Equals(""))
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
                return;
            }

            int nbExemplaires;
            if (!int.TryParse(txbCommandesLivresNbExemplaires.Text, out nbExemplaires) || nbExemplaires <= 0)
            {
                MessageBox.Show("Le nombre d'exemplaires doit être un entier positif.", "Information");
                txbCommandesLivresNbExemplaires.Text = "";
                txbCommandesLivresNbExemplaires.Focus();
                return;
            }

            double montant;
            if (!double.TryParse(txbCommandesLivresMontant.Text, out montant) || montant <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre positif.", "Information");
                txbCommandesLivresMontant.Text = "";
                txbCommandesLivresMontant.Focus();
                return;
            }

            string idDocument = txbCommandesLivresNumRecherche.Text;
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Vous devez sélectionner un livre.", "Information");
                return;
            }

            Random random = new Random();
            StringBuilder id = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int choice = random.Next(3);
                switch (choice)
                {
                    case 0:
                        id.Append((char)random.Next(48, 58));
                        break;
                    case 1:
                        id.Append((char)random.Next(65, 91));
                        break;
                    case 2:
                        id.Append((char)random.Next(97, 123));
                        break;
                }
            }

            string idSuivi = "1"; 
            string libelleSuivi = "en cours"; 

            CommandeDocument commande = new CommandeDocument(
                id.ToString(),
                DateTime.Now,
                montant,
                nbExemplaires,
                idDocument,
                idSuivi,
                libelleSuivi
            );

            if (controller.CreerCommandeDocument(commande))
            {
                lesCommandesLivres = controller.GetCommandesDocument(idDocument);
                RemplirCommandesLivresListe(lesCommandesLivres);
                txbCommandesLivresMontant.Text = "";
                txbCommandesLivresNbExemplaires.Text = "";
            }
            else
            {
                MessageBox.Show("Erreur, la commande n'a pas été créée.", "Erreur");
            }
        }

        /// <summary>
        /// Suppression d'une commande de livre si elle n'est pas encore livrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesLivresSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesLivres.CurrentCell != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
                if (commandeDocument.LibelleSuivi == "livrée" || commandeDocument.LibelleSuivi == "réglée")
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas être supprimée.", "Information");
                    return;
                }

                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation de suppression", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.SupprimerCommandeDocument(commandeDocument.Id))
                    {
                        lesCommandesLivres.Remove(commandeDocument);
                        bdgCommandesLivres.ResetBindings(false); 
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information");
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour la sélection d'une ligne dans le DataGridView des commandes de livres.
        /// Met à jour la ComboBox externe avec les étapes de suivi possibles.
        /// </summary>
        private void dgvCommandesLivres_SelectionChanged(object sender, EventArgs e)
        {
            bool modificationPossible = false;
            CommandeDocument commandeSelectionnee = null;
            List<Categorie> etapesPossibles = new List<Categorie>();

            if (dgvCommandesLivres.CurrentRow != null && dgvCommandesLivres.CurrentRow.DataBoundItem is CommandeDocument)
            {
                commandeSelectionnee = (CommandeDocument)dgvCommandesLivres.CurrentRow.DataBoundItem;
                string etatActuel = commandeSelectionnee.LibelleSuivi?.Trim().ToLowerInvariant();

                if (idServiceUtilisateurConnecte == 2 && (etatActuel == "en cours" || etatActuel == "livrée"))
                {
                    modificationPossible = true;
                    etapesPossibles = lesEtapesSuivi.Where(etape => etape.Id == "2" || etape.Id == "3").ToList();
                }
            }

            RemplirComboEtapesSuivi(cbxModifierSuiviLivre, etapesPossibles);
            cbxModifierSuiviLivre.Enabled = modificationPossible;
            btnModifierSuiviLivre.Enabled = modificationPossible;

            if (modificationPossible && commandeSelectionnee != null && etapesPossibles.Any(etape => etape.Id == commandeSelectionnee.IdSuivi))
            {
                 cbxModifierSuiviLivre.SelectedValue = commandeSelectionnee.IdSuivi;
            }
            else if (modificationPossible)
            {
                 cbxModifierSuiviLivre.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton de modification de l'étape de suivi d'une commande de livre.
        /// </summary>
        private void btnModifierSuiviLivre_Click(object sender, EventArgs e)
        {
            if (dgvCommandesLivres.CurrentRow != null && dgvCommandesLivres.CurrentRow.DataBoundItem is CommandeDocument commandeSelectionnee &&
                cbxModifierSuiviLivre.SelectedItem != null && cbxModifierSuiviLivre.SelectedValue != null)
            {
                string etatActuel = commandeSelectionnee.LibelleSuivi?.Trim().ToLowerInvariant();
                if (idServiceUtilisateurConnecte != 2 || (etatActuel != "en cours" && etatActuel != "livrée"))
                {
                    MessageBox.Show("La modification de l'étape de suivi n'est pas autorisée pour cette commande ou cet utilisateur.", "Information");
                    return;
                }

                string nouvelIdSuivi = cbxModifierSuiviLivre.SelectedValue.ToString();

                 if (nouvelIdSuivi == commandeSelectionnee.IdSuivi)
                 {
                     MessageBox.Show("Veuillez sélectionner une étape différente de l'étape actuelle.", "Information");
                     return;
                 }

                string idCommande = commandeSelectionnee.Id;

                if (controller.ModifierEtapeSuivi(idCommande, nouvelIdSuivi))
                {
                    MessageBox.Show("L'étape de suivi a été modifiée avec succès.", "Succès");

                    Categorie nouvelleEtape = lesEtapesSuivi.FirstOrDefault(s => s.Id == nouvelIdSuivi);
                    if (nouvelleEtape != null)
                    {
                        commandeSelectionnee.IdSuivi = nouvelIdSuivi;
                        commandeSelectionnee.LibelleSuivi = nouvelleEtape.Libelle;
                    }

                    bdgCommandesLivres.ResetBindings(false);

                    dgvCommandesLivres_SelectionChanged(dgvCommandesLivres, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de l'étape de suivi.", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande ('en cours' ou 'livrée') et une nouvelle étape de suivi valide.", "Information");
            }
        }

        #endregion

        #region Onglet Commandes DVD

        /// <summary>
        /// Objets et variables liés à l'onglet de commandes de DVD
        /// </summary>
        private readonly BindingSource bdgCommandesDvd = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private List<Dvd> lesDvdCommandes = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Commandes de DVD : 
        /// récupère les DVD et charge les étapes de suivi dans les combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandesDvd_Enter(object sender, EventArgs e)
        {
            if (lesDvdCommandes.Count == 0)
            {
                lesDvdCommandes = controller.GetAllDvd();
            }
            AccesCommandeDvdGroupBox(false);
            txbCommandesDvdNumRecherche.Text = "";
            
            
            
            ViderCommandesDvdInfos();
            ViderCommandesDvdListe();
        }

        /// <summary>
        /// Remplit le datagrid des commandes avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">Liste des commandes</param>
        private void RemplirCommandesDvdListe(List<CommandeDocument> commandes)
        {
            if (commandes != null)
            {
                bdgCommandesDvd.DataSource = commandes;
                dgvCommandesDvd.DataSource = bdgCommandesDvd;
                dgvCommandesDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvCommandesDvd.Columns["DateCommande"].HeaderText = "Date";
                dgvCommandesDvd.Columns["DateCommande"].DefaultCellStyle.Format = "d";
                dgvCommandesDvd.Columns["Montant"].HeaderText = "Montant";
                dgvCommandesDvd.Columns["NbExemplaire"].HeaderText = "Nb exemplaires";
                dgvCommandesDvd.Columns["LibelleSuivi"].HeaderText = "Étape";
                dgvCommandesDvd.Columns["Id"].Visible = false;
                dgvCommandesDvd.Columns["IdLivreDvd"].Visible = false;
                dgvCommandesDvd.Columns["IdSuivi"].Visible = false;

                // Définir l'ordre des colonnes visibles
                dgvCommandesDvd.Columns["DateCommande"].DisplayIndex = 0;
                dgvCommandesDvd.Columns["Montant"].DisplayIndex = 1;
                dgvCommandesDvd.Columns["NbExemplaire"].DisplayIndex = 2;
                dgvCommandesDvd.Columns["LibelleSuivi"].DisplayIndex = 3;
            }
            else
            {
                dgvCommandesDvd.DataSource = null;
            }
        }

        /// <summary>
        /// Vide la liste des commandes de DVD
        /// </summary>
        private void ViderCommandesDvdListe()
        {
            dgvCommandesDvd.DataSource = null;
        }

        /// <summary>
        /// Recherche et affiche les informations du DVD dont on a saisi le numéro.
        /// Affiche également la liste de ses commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandesDvdNumRecherche.Text.Equals(""))
            {
                Dvd dvd = lesDvdCommandes.Find(x => x.Id.Equals(txbCommandesDvdNumRecherche.Text));
                if (dvd != null)
                {
                    AfficheCommandesDvdInfos(dvd);
                    lesCommandesDvd = controller.GetCommandesDocument(dvd.Id);
                    RemplirCommandesDvdListe(lesCommandesDvd);
                    AccesCommandeDvdGroupBox(true);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    txbCommandesDvdNumRecherche.Text = "";
                    txbCommandesDvdNumRecherche.Focus();
                    ViderCommandesDvdInfos();
                    ViderCommandesDvdListe();
                    AccesCommandeDvdGroupBox(false);
                }
            }
            else
            {
                ViderCommandesDvdInfos();
                ViderCommandesDvdListe();
                AccesCommandeDvdGroupBox(false);
            }
        }

        /// <summary>
        /// Si le numéro de DVD est modifié, la zone de recherche est vidée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbCommandesDvdNumRecherche_TextChanged(object sender, EventArgs e)
        {
            ViderCommandesDvdListe();
            ViderCommandesDvdInfos();
            AccesCommandeDvdGroupBox(false);
        }

        /// <summary>
        /// Affiche les informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">Le dvd à afficher</param>
        private void AfficheCommandesDvdInfos(Dvd dvd)
        {
            txbCommandesDvdNumero.Text = dvd.Id;
            txbCommandesDvdDuree.Text = dvd.Duree.ToString();
            txbCommandesDvdTitre.Text = dvd.Titre;
            txbCommandesDvdRealisateur.Text = dvd.Realisateur;
            txbCommandesDvdSynopsis.Text = dvd.Synopsis;
            txbCommandesDvdGenre.Text = dvd.Genre;
            txbCommandesDvdPublic.Text = dvd.Public;
            txbCommandesDvdRayon.Text = dvd.Rayon;
            txbCommandesDvdImage.Text = dvd.Image;
            string image = dvd.Image;
            try
            {
                pcbCommandesDvdImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCommandesDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du DVD
        /// </summary>
        private void ViderCommandesDvdInfos()
        {
            txbCommandesDvdNumero.Text = "";
            txbCommandesDvdDuree.Text = "";
            txbCommandesDvdTitre.Text = "";
            txbCommandesDvdRealisateur.Text = "";
            txbCommandesDvdSynopsis.Text = "";
            txbCommandesDvdGenre.Text = "";
            txbCommandesDvdPublic.Text = "";
            txbCommandesDvdRayon.Text = "";
            txbCommandesDvdImage.Text = "";
            pcbCommandesDvdImage.Image = null;
        }

        /// <summary>
        /// Active ou désactive l'accès à la gestion des commandes de DVD
        /// </summary>
        /// <param name="acces">true pour activer, false pour désactiver</param>
        private void AccesCommandeDvdGroupBox(bool acces)
        {
            grpCommandesDvdAjout.Enabled = acces;
            grpCommandesDvdRecherche.Enabled = true;
        }

        /// <summary>
        /// Tri sur les colonnes du DataGridView dgvCommandesDvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date":
                    sortedList = lesCommandesDvd.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nb exemplaires":
                    sortedList = lesCommandesDvd.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Étape":
                    sortedList = lesCommandesDvd.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }
            RemplirCommandesDvdListe(sortedList);
        }

        /// <summary>
        /// Sur le clic du bouton d'ajout d'une commande DVD, 
        /// vérifie les données puis ajoute la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesDvdValider_Click(object sender, EventArgs e)
        {
            if (txbCommandesDvdMontant.Text.Equals("") || txbCommandesDvdNbExemplaires.Text.Equals(""))
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
                return;
            }

            int nbExemplaires;
            if (!int.TryParse(txbCommandesDvdNbExemplaires.Text, out nbExemplaires) || nbExemplaires <= 0)
            {
                MessageBox.Show("Le nombre d'exemplaires doit être un entier positif.", "Information");
                txbCommandesDvdNbExemplaires.Text = "";
                txbCommandesDvdNbExemplaires.Focus();
                return;
            }

            double montant;
            if (!double.TryParse(txbCommandesDvdMontant.Text, out montant) || montant <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre positif.", "Information");
                txbCommandesDvdMontant.Text = "";
                txbCommandesDvdMontant.Focus();
                return;
            }

            string idDocument = txbCommandesDvdNumRecherche.Text;
            if (string.IsNullOrEmpty(idDocument))
            {
                MessageBox.Show("Vous devez sélectionner un DVD.", "Information");
                return;
            }

            Random random = new Random();
            StringBuilder id = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int choice = random.Next(3);
                switch (choice)
                {
                    case 0:
                        id.Append((char)random.Next(48, 58));
                        break;
                    case 1:
                        id.Append((char)random.Next(65, 91));
                        break;
                    case 2:
                        id.Append((char)random.Next(97, 123));
                        break;
                }
            }

            string idSuivi = "1"; 
            string libelleSuivi = "en cours"; 

            CommandeDocument commande = new CommandeDocument(
                id.ToString(),
                DateTime.Now,
                montant,
                nbExemplaires,
                idDocument,
                idSuivi,
                libelleSuivi
            );

            if (controller.CreerCommandeDocument(commande))
            {
                lesCommandesDvd = controller.GetCommandesDocument(idDocument);
                RemplirCommandesDvdListe(lesCommandesDvd);
                txbCommandesDvdMontant.Text = "";
                txbCommandesDvdNbExemplaires.Text = "";
            }
            else
            {
                MessageBox.Show("Erreur, la commande n'a pas été créée.", "Erreur");
            }
        }

        /// <summary>
        /// Suppression d'une commande de DVD si elle n'est pas encore livrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesDvd.CurrentCell != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
                if (commandeDocument.LibelleSuivi == "livrée" || commandeDocument.LibelleSuivi == "réglée")
                {
                    MessageBox.Show("Une commande livrée ou réglée ne peut pas être supprimée.", "Information");
                    return;
                }

                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette commande ?", "Confirmation de suppression", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.SupprimerCommandeDocument(commandeDocument.Id))
                    {
                        lesCommandesDvd.Remove(commandeDocument);
                        bdgCommandesDvd.ResetBindings(false); 
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information");
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour la sélection d'une ligne dans le DataGridView des commandes de DVD.
        /// Met à jour la ComboBox externe avec les étapes de suivi possibles.
        /// </summary>
        private void dgvCommandesDvd_SelectionChanged(object sender, EventArgs e)
        {
            bool modificationPossible = false;
            CommandeDocument commandeSelectionnee = null;
            List<Categorie> etapesPossibles = new List<Categorie>();

            if (dgvCommandesDvd.CurrentRow != null && dgvCommandesDvd.CurrentRow.DataBoundItem is CommandeDocument)
            {
                commandeSelectionnee = (CommandeDocument)dgvCommandesDvd.CurrentRow.DataBoundItem;
                string etatActuel = commandeSelectionnee.LibelleSuivi?.Trim().ToLowerInvariant();

                if (idServiceUtilisateurConnecte == 2 && (etatActuel == "en cours" || etatActuel == "livrée"))
                {
                    modificationPossible = true;
                    etapesPossibles = lesEtapesSuivi.Where(etape => etape.Id == "2" || etape.Id == "3").ToList();
                }
            }

            RemplirComboEtapesSuivi(cbxModifierSuiviDvd, etapesPossibles);
            cbxModifierSuiviDvd.Enabled = modificationPossible;
            btnModifierSuiviDvd.Enabled = modificationPossible;

            if (modificationPossible && commandeSelectionnee != null && etapesPossibles.Any(etape => etape.Id == commandeSelectionnee.IdSuivi))
            {
                 cbxModifierSuiviDvd.SelectedValue = commandeSelectionnee.IdSuivi;
            }
            else if (modificationPossible)
            {
                 cbxModifierSuiviDvd.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton de modification de l'étape de suivi d'une commande de DVD.
        /// </summary>
        private void btnModifierSuiviDvd_Click(object sender, EventArgs e)
        {
            if (dgvCommandesDvd.CurrentRow != null && dgvCommandesDvd.CurrentRow.DataBoundItem is CommandeDocument commandeSelectionnee &&
                cbxModifierSuiviDvd.SelectedItem != null && cbxModifierSuiviDvd.SelectedValue != null)
            {
                string etatActuel = commandeSelectionnee.LibelleSuivi?.Trim().ToLowerInvariant();
                if (idServiceUtilisateurConnecte != 2 || (etatActuel != "en cours" && etatActuel != "livrée"))
                {
                    MessageBox.Show("La modification de l'étape de suivi n'est pas autorisée pour cette commande ou cet utilisateur.", "Information");
                    return;
                }

                string nouvelIdSuivi = cbxModifierSuiviDvd.SelectedValue.ToString();

                 if (nouvelIdSuivi == commandeSelectionnee.IdSuivi)
                 {
                     MessageBox.Show("Veuillez sélectionner une étape différente de l'étape actuelle.", "Information");
                     return;
                 }

                string idCommande = commandeSelectionnee.Id;

                if (controller.ModifierEtapeSuivi(idCommande, nouvelIdSuivi))
                {
                    MessageBox.Show("L'étape de suivi a été modifiée avec succès.", "Succès");

                    Categorie nouvelleEtape = lesEtapesSuivi.FirstOrDefault(s => s.Id == nouvelIdSuivi);
                    if (nouvelleEtape != null)
                    {
                        commandeSelectionnee.IdSuivi = nouvelIdSuivi;
                        commandeSelectionnee.LibelleSuivi = nouvelleEtape.Libelle;
                    }

                    bdgCommandesDvd.ResetBindings(false);

                    dgvCommandesDvd_SelectionChanged(dgvCommandesDvd, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de l'étape de suivi.", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande ('en cours' ou 'livrée') et une nouvelle étape de suivi valide.", "Information");
            }
        }

        #endregion

        #region Onglet Commandes Revues

        /// <summary>
        /// Objets et variables liés à l'onglet de commandes de revues
        /// </summary>
        private readonly BindingSource bdgAbonnementsRevue = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private List<Revue> lesRevuesCommandes = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Commandes de revues : 
        /// récupère les revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandesRevues_Enter(object sender, EventArgs e)
        {
            if (lesRevuesCommandes.Count == 0)
            {
                lesRevuesCommandes = controller.GetAllRevues();
            }
            AccesCommandeRevuesGroupBox(false);
            txbCommandesRevuesNumRecherche.Text = "";
            
            ViderCommandesRevuesInfos();
            ViderCommandesRevuesListe();
        }

        /// <summary>
        /// Remplit le datagrid des abonnements avec la liste reçue en paramètre
        /// </summary>
        /// <param name="abonnements">Liste des abonnements</param>
        private void RemplirCommandesRevuesListe(List<Abonnement> abonnements)
        {
            if (abonnements != null)
            {
                bdgAbonnementsRevue.DataSource = abonnements;
                dgvCommandesRevues.DataSource = bdgAbonnementsRevue;
                dgvCommandesRevues.Columns["Id"].Visible = false; // Masquer l'ID
                dgvCommandesRevues.Columns["Id"].HeaderText = "ID";
                dgvCommandesRevues.Columns["DateCommande"].HeaderText = "Date commande";
                dgvCommandesRevues.Columns["Montant"].HeaderText = "Montant";
                dgvCommandesRevues.Columns["DateFinAbonnement"].HeaderText = "Date fin abonnement";
                dgvCommandesRevues.Columns["IdRevue"].Visible = false;
                dgvCommandesRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandesRevues.Columns["DateCommande"].DefaultCellStyle.Format = "d";
                dgvCommandesRevues.Columns["DateFinAbonnement"].DefaultCellStyle.Format = "d";

                // Définir l'ordre des colonnes visibles
                dgvCommandesRevues.Columns["DateCommande"].DisplayIndex = 0;
                dgvCommandesRevues.Columns["Montant"].DisplayIndex = 1;
                dgvCommandesRevues.Columns["DateFinAbonnement"].DisplayIndex = 2;
            }
            else
            {
                dgvCommandesRevues.DataSource = null;
            }
        }

        /// <summary>
        /// Vide la liste des abonnements
        /// </summary>
        private void ViderCommandesRevuesListe()
        {
            dgvCommandesRevues.DataSource = null;
        }

        /// <summary>
        /// Recherche et affiche les informations de la revue dont on a saisi le numéro.
        /// Affiche également la liste de ses abonnements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandesRevuesNumRecherche.Text.Equals(""))
            {
                Revue revue = lesRevuesCommandes.Find(x => x.Id.Equals(txbCommandesRevuesNumRecherche.Text));
                if (revue != null)
                {
                    AfficheCommandesRevuesInfos(revue);
                    lesAbonnements = controller.GetAbonnementsRevue(revue.Id);
                    RemplirCommandesRevuesListe(lesAbonnements);
                    AccesCommandeRevuesGroupBox(true);
                }
                else
                {
                    MessageBox.Show("Numéro introuvable");
                    txbCommandesRevuesNumRecherche.Text = "";
                    txbCommandesRevuesNumRecherche.Focus();
                    ViderCommandesRevuesInfos();
                    ViderCommandesRevuesListe();
                    AccesCommandeRevuesGroupBox(false);
                }
            }
            else
            {
                ViderCommandesRevuesInfos();
                ViderCommandesRevuesListe();
                AccesCommandeRevuesGroupBox(false);
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de recherche est vidée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbCommandesRevuesNumRecherche_TextChanged(object sender, EventArgs e)
        {
            ViderCommandesRevuesListe();
            ViderCommandesRevuesInfos();
            AccesCommandeRevuesGroupBox(false);
        }

        /// <summary>
        /// Affiche les informations de la revue sélectionnée
        /// </summary>
        /// <param name="revue">La revue à afficher</param>
        private void AfficheCommandesRevuesInfos(Revue revue)
        {
            txbCommandesRevuesNum.Text = revue.Id;
            txbCommandesRevuesTitre.Text = revue.Titre;
            txbCommandesRevuesPeriodicite.Text = revue.Periodicite;
            txbCommandesRevuesDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbCommandesRevuesGenre.Text = revue.Genre;
            txbCommandesRevuesPublic.Text = revue.Public;
            txbCommandesRevuesRayon.Text = revue.Rayon;
            txbCommandesRevuesImage.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pcbCommandesRevuesImage.Image = System.Drawing.Image.FromFile(image);
            }
            catch
            {
                pcbCommandesRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la revue
        /// </summary>
        private void ViderCommandesRevuesInfos()
        {
            txbCommandesRevuesNum.Text = "";
            txbCommandesRevuesTitre.Text = "";
            txbCommandesRevuesPeriodicite.Text = "";
            txbCommandesRevuesDelaiMiseADispo.Text = "";
            txbCommandesRevuesGenre.Text = "";
            txbCommandesRevuesPublic.Text = "";
            txbCommandesRevuesRayon.Text = "";
            txbCommandesRevuesImage.Text = "";
            pcbCommandesRevuesImage.Image = null;
            dtpCommandesRevuesDateFinAbonnement.Value = DateTime.Now.AddYears(1);
        }

        /// <summary>
        /// Active ou désactive l'accès à la gestion des commandes de revues
        /// </summary>
        /// <param name="acces">true pour activer, false pour désactiver</param>
        private void AccesCommandeRevuesGroupBox(bool acces)
        {
            grpCommandesRevuesAjout.Enabled = acces;
            grpCommandesRevuesRecherche.Enabled = true;
        }

        /// <summary>
        /// Tri sur les colonnes du DataGridView dgvCommandesRevues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesRevues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesRevues.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Date commande":
                    sortedList = lesAbonnements.OrderByDescending(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnements.OrderBy(o => o.Montant).ToList();
                    break;
                case "Date fin abonnement":
                    sortedList = lesAbonnements.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
            }
            RemplirCommandesRevuesListe(sortedList);
        }

        /// <summary>
        /// Sur le clic du bouton d'ajout d'un abonnement, 
        /// vérifie les données puis ajoute l'abonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesRevuesValider_Click(object sender, EventArgs e)
        {
            if (txbCommandesRevuesMontant.Text.Equals(""))
            {
                MessageBox.Show("Le montant est obligatoire.", "Information");
                return;
            }

            double montant;
            if (!double.TryParse(txbCommandesRevuesMontant.Text, out montant) || montant <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre positif.", "Information");
                txbCommandesRevuesMontant.Text = "";
                txbCommandesRevuesMontant.Focus();
                return;
            }

            string idRevue = txbCommandesRevuesNumRecherche.Text;
            if (string.IsNullOrEmpty(idRevue))
            {
                MessageBox.Show("Vous devez sélectionner une revue.", "Information");
                return;
            }

            DateTime dateFinAbonnement = dtpCommandesRevuesDateFinAbonnement.Value;
            if (dateFinAbonnement <= DateTime.Now)
            {
                MessageBox.Show("La date de fin d'abonnement doit être postérieure à aujourd'hui.", "Information");
                return;
            }

            Random random = new Random();
            StringBuilder id = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int choice = random.Next(3);
                switch (choice)
                {
                    case 0:
                        id.Append((char)random.Next(48, 58));
                        break;
                    case 1:
                        id.Append((char)random.Next(65, 91));
                        break;
                    case 2:
                        id.Append((char)random.Next(97, 123));
                        break;
                }
            }

            Abonnement abonnement = new Abonnement(
                id.ToString(),
                DateTime.Now,
                montant,
                dateFinAbonnement,
                idRevue
            );

            if (controller.CreerAbonnement(abonnement))
            {
                lesAbonnements = controller.GetAbonnementsRevue(idRevue);
                RemplirCommandesRevuesListe(lesAbonnements);
                txbCommandesRevuesMontant.Text = "";
                dtpCommandesRevuesDateFinAbonnement.Value = DateTime.Now.AddYears(1);
            }
            else
            {
                MessageBox.Show("Erreur, l'abonnement n'a pas été créé.", "Erreur");
            }
        }

        /// <summary>
        /// Suppression d'un abonnement si aucun exemplaire n'est rattaché à celui-ci
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandesRevuesSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesRevues.CurrentCell != null)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnementsRevue.List[bdgAbonnementsRevue.Position];
                string idRevue = abonnement.IdRevue;

                List<Exemplaire> exemplaires = controller.GetExemplairesRevue(idRevue);
                bool exemplaireDansAbonnement = false;

                if (exemplaires.Any(exemplaire => FrmMediatekController.ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaire.DateAchat)))
                {
                    exemplaireDansAbonnement = true;
                }

                if (exemplaireDansAbonnement)
                {
                    MessageBox.Show("Des exemplaires sont rattachés à cet abonnement, impossible de le supprimer.", "Information");
                    return;
                }

                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet abonnement ?", "Confirmation de suppression", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.SupprimerAbonnement(abonnement.Id))
                    {
                        lesAbonnements.Remove(abonnement);
                        bdgAbonnementsRevue.ResetBindings(false);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de l'abonnement.", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un abonnement à supprimer.", "Information");
            }
        }

        #endregion

        #region Méthodes modification étape de suivi

        /// <summary>
        /// Remplit une ComboBox avec une liste d'étapes de suivi.
        /// </summary>
        /// <param name="cbx">La ComboBox à remplir.</param>
        /// <param name="etapes">La liste des étapes (objets Categorie/Suivi).</param>
        private static void RemplirComboEtapesSuivi(ComboBox cbx, List<Categorie> etapes)
        {
            object selectedValueAvant = cbx.SelectedValue;

            cbx.DataSource = null;
            cbx.Items.Clear();

            if (etapes != null && etapes.Count > 0)
            {
                cbx.DisplayMember = "Libelle";
                cbx.ValueMember = "Id";
                cbx.DataSource = new List<Categorie>(etapes);

                if (selectedValueAvant != null && etapes.Any(etape => etape.Id == selectedValueAvant.ToString()))
                {
                    cbx.SelectedValue = selectedValueAvant;
                }
                else
                {
                   // Si la valeur précédente n'est plus valide ou n'existait pas, laisser la sélection par défaut (souvent le premier élément) ou mettre à -1 si aucun ne doit être sélectionné.
                }
            }
            else
            {
                 // Aucune étape à afficher, la ComboBox reste vide (déjà fait par DataSource = null)
            }
        }

        #endregion
    }
}
