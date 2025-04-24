using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
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
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
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
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
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
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
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
            // informations sur la revue
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
            // affiche la liste des exemplaires de la revue
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
                // positionnement à la racine du disque où se trouve le dossier actuel
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

        #region Onglet Commande Livres
        private readonly BindingSource bdgCmdLivreListe = new BindingSource();
        private List<CommandeDocumentLivre> lesCommandesLivre = new List<CommandeDocumentLivre>();
        private Livre livreCommandeSelectionne = null;
        private List<Suivi> lesEtapesSuivi = new List<Suivi>();
        private readonly string ETAPE_SUIVI_EN_COURS = "En cours";
        private CommandeDocumentLivre commandeEnCoursEdition = null;
        private int rowIndexEnCoursEdition = -1;

        /// <summary>
        /// Nom de la colonne pour la date de commande dans les DGV commandes livre/DVD.
        /// </summary>
        private const string NomColonneDateCommande = "DateCommande";

        /// <summary>
        /// Nom de la colonne pour le montant dans les DGV commandes livre/DVD.
        /// </summary>
        private const string NomColonneMontant = "Montant";

        /// <summary>
        /// Nom de la colonne pour le nombre d'exemplaires dans les DGV commandes livre/DVD.
        /// </summary>
        private const string NomColonneNbExemplaire = "NbExemplaire";

        /// <summary>
        /// Nom de la colonne pour le libellé de suivi dans les DGV commandes livre/DVD.
        /// </summary>
        private const string NomColonneLibelleSuivi = "LibelleSuivi";

        /// <summary>
        /// Nom de la propriété 'Libelle' utilisée pour l'affichage dans les ComboBox.
        /// </summary>
        private const string NomProprieteLibelle = "Libelle";

        /// <summary>
        /// Titre pour les MessageBox de succès.
        /// </summary>
        private const string MessageBoxTitreSucces = "Succès";

        /// <summary>
        /// Ouverture de l'onglet Commande Livres :
        /// Récupération des étapes de suivi et initialisation des champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivres_Enter(object sender, EventArgs e)
        {
            if (lesEtapesSuivi.Count == 0)
            {
                lesEtapesSuivi = controller.GetAllSuivi();
            }
            VideCmdLivreZones();
            AccesCmdLivreGroupBox(false);
            txbCmdLivreNumRecherche.Text = "";
            btnCmdLivreSaveSuivi.Enabled = false;
        }

        /// <summary>
        /// Recherche d'un livre par son numéro et affichage de ses informations et commandes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmdLivreNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCmdLivreNumRecherche.Text.Equals(""))
            {
                livreCommandeSelectionne = controller.GetLivreById(txbCmdLivreNumRecherche.Text);
                if (livreCommandeSelectionne != null)
                {
                    AfficheCmdLivreInfos(livreCommandeSelectionne);
                    AfficheCmdLivreCommandes();
                    AccesCmdLivreGroupBox(true);
                }
                else
                {
                    MessageBox.Show("Numéro de livre introuvable.", "Erreur");
                    VideCmdLivreZones();
                    AccesCmdLivreGroupBox(false);
                    livreCommandeSelectionne = null;
                }
            }
            else
            {
                VideCmdLivreZones();
                AccesCmdLivreGroupBox(false);
                livreCommandeSelectionne = null;
            }
        }


        /// <summary>
        /// Affichage des informations du livre sélectionné dans la section commande.
        /// </summary>
        /// <param name="livre">Le livre à afficher.</param>
        private void AfficheCmdLivreInfos(Livre livre)
        {
            txbCmdLivreTitre.Text = livre.Titre;
            txbCmdLivreAuteur.Text = livre.Auteur;
            txbCmdLivreCollection.Text = livre.Collection;
            txbCmdLivreISBN.Text = livre.Isbn;
            txbCmdLivreGenre.Text = livre.Genre;
            txbCmdLivrePublic.Text = livre.Public;
            txbCmdLivreRayon.Text = livre.Rayon;
            txbCmdLivreImage.Text = livre.Image;
            txbCmdLivreNumero.Text = livre.Id;

            string imagePath = livre.Image;
            try
            {
                pcbCmdLivreImage.Image = string.IsNullOrEmpty(imagePath) ? null : Image.FromFile(imagePath);
            }
            catch
            {
                pcbCmdLivreImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre et la liste des commandes.
        /// </summary>
        private void VideCmdLivreInfos()
        {
            txbCmdLivreTitre.Text = "";
            txbCmdLivreAuteur.Text = "";
            txbCmdLivreCollection.Text = "";
            txbCmdLivreISBN.Text = "";
            txbCmdLivreGenre.Text = "";
            txbCmdLivrePublic.Text = "";
            txbCmdLivreRayon.Text = "";
            txbCmdLivreImage.Text = "";
            pcbCmdLivreImage.Image = null;

            lesCommandesLivre = new List<CommandeDocumentLivre>();
            RemplirCmdLivreListe(lesCommandesLivre);
            dtpCmdLivreDate.Value = DateTime.Now;
            nudCmdLivreNbEx.Value = 1;
            nudCmdLivreMontant.Value = 0;
        }

        /// <summary>
        /// Vide la zone de recherche par numéro.
        /// </summary>
        private void VideCmdLivreZones()
        {
            txbCmdLivreNumRecherche.Text = "";
            VideCmdLivreInfos();
        }

        /// <summary>
        /// Récupère et affiche les commandes associées au livre actuellement sélectionné.
        /// </summary>
        private void AfficheCmdLivreCommandes()
        {
            if (livreCommandeSelectionne != null)
            {
                string idLivre = livreCommandeSelectionne.Id;
                lesCommandesLivre = controller.GetCommandesLivre(idLivre);
                lesCommandesLivre = lesCommandesLivre.OrderByDescending(c => c.DateCommande).ToList();
                RemplirCmdLivreListe(lesCommandesLivre);
            }
            else
            {
                RemplirCmdLivreListe(new List<CommandeDocumentLivre>());
            }
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste des commandes fournie.
        /// </summary>
        /// <param name="commandes">Liste des commandes (ViewModel).</param>
        private void RemplirCmdLivreListe(List<CommandeDocumentLivre> commandes)
        {
            dgvCmdLivreListe.AutoGenerateColumns = false;
            dgvCmdLivreListe.Columns.Clear();
            dgvCmdLivreListe.DataSource = null;

            // Colonne Date Commande (ReadOnly)
            DataGridViewTextBoxColumn colDate = new DataGridViewTextBoxColumn();
            colDate.Name = NomColonneDateCommande;
            colDate.HeaderText = "Date Commande";
            colDate.DataPropertyName = NomColonneDateCommande;
            colDate.ReadOnly = true;
            colDate.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvCmdLivreListe.Columns.Add(colDate);

            // Colonne Montant (ReadOnly)
            DataGridViewTextBoxColumn colMontant = new DataGridViewTextBoxColumn();
            colMontant.Name = NomColonneMontant;
            colMontant.HeaderText = "Montant";
            colMontant.DataPropertyName = NomColonneMontant;
            colMontant.ReadOnly = true;
            colMontant.DefaultCellStyle.Format = "C2"; // Format monétaire
            dgvCmdLivreListe.Columns.Add(colMontant);

            // Colonne Nb Exemplaires (ReadOnly)
            DataGridViewTextBoxColumn colNbEx = new DataGridViewTextBoxColumn();
            colNbEx.Name = NomColonneNbExemplaire;
            colNbEx.HeaderText = "Nb Exemplaires";
            colNbEx.DataPropertyName = NomColonneNbExemplaire;
            colNbEx.ReadOnly = true;
            dgvCmdLivreListe.Columns.Add(colNbEx);

            // Colonne Étape Suivi (ComboBox, Éditable)
            DataGridViewComboBoxColumn colSuivi = new DataGridViewComboBoxColumn();
            colSuivi.Name = NomColonneLibelleSuivi;
            colSuivi.HeaderText = "Étape Suivi";
            colSuivi.DataPropertyName = NomColonneLibelleSuivi;
            colSuivi.DataSource = lesEtapesSuivi;
            colSuivi.DisplayMember = NomProprieteLibelle;
            colSuivi.ValueMember = NomProprieteLibelle;
            colSuivi.ReadOnly = false;
            colSuivi.FlatStyle = FlatStyle.Flat;
            dgvCmdLivreListe.Columns.Add(colSuivi);

            // Redimensionnement automatique
            dgvCmdLivreListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            bdgCmdLivreListe.DataSource = commandes;
            dgvCmdLivreListe.DataSource = bdgCmdLivreListe;

            commandeEnCoursEdition = null;
            rowIndexEnCoursEdition = -1;
            btnCmdLivreSaveSuivi.Enabled = false;
        }


        /// <summary>
        /// Gère l'activation/désactivation des zones d'information du livre et de création de commande.
        /// </summary>
        /// <param name="acces">True pour activer, False pour désactiver.</param>
        private void AccesCmdLivreGroupBox(bool acces)
        {
            grpCmdLivreInfos.Enabled = acces;
            grpCmdLivreNouvelleCmd.Enabled = acces;
            dgvCmdLivreListe.Enabled = acces;
        }

        /// <summary>
        /// Validation et enregistrement d'une nouvelle commande de livre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmdLivreValiderCmd_Click(object sender, EventArgs e)
        {
            if (livreCommandeSelectionne == null)
            {
                MessageBox.Show("Veuillez d'abord rechercher et sélectionner un livre.", "Information");
                return;
            }

            // Validation simple
            if (nudCmdLivreNbEx.Value <= 0)
            {
                MessageBox.Show("Le nombre d'exemplaires doit être supérieur à zéro.", "Information");
                nudCmdLivreNbEx.Focus();
                return;
            }
            if (nudCmdLivreMontant.Value <= 0)
            {
                MessageBox.Show("Le montant doit être supérieur à zéro.", "Information");
                nudCmdLivreMontant.Focus();
                return;
            }

            // Récupération de l'ID de l'étape "En cours"
            Suivi etapeEnCours = lesEtapesSuivi.FirstOrDefault(s => s.Libelle.Equals(ETAPE_SUIVI_EN_COURS, StringComparison.OrdinalIgnoreCase));
            if (etapeEnCours == null)
            {
                MessageBox.Show($"L'étape de suivi '{ETAPE_SUIVI_EN_COURS}' n'a pas été trouvée. Impossible de créer la commande.", "Erreur");
                return;
            }
            int idSuiviEnCours = etapeEnCours.Id;

            // Création de l'objet Commande
            Commande nouvelleCommande = new Commande(
               "0", // L'ID sera généré par l'API/BDD
               dtpCmdLivreDate.Value,
               (double)nudCmdLivreMontant.Value
           );

            // Création de l'objet CommandeDocument
            CommandeDocument nouvelleCommandeDoc = new CommandeDocument(
                 "0", // Sera lié à l'ID commande généré
                (int)nudCmdLivreNbEx.Value,
                livreCommandeSelectionne.Id,
                idSuiviEnCours
            );


            // Appel au contrôleur pour créer la commande
            if (controller.CreerCommandeDocument(nouvelleCommande, nouvelleCommandeDoc))
            {
                MessageBox.Show("Nouvelle commande enregistrée avec succès.", MessageBoxTitreSucces);
                AfficheCmdLivreCommandes();

                dtpCmdLivreDate.Value = DateTime.Now;
                nudCmdLivreNbEx.Value = 1;
                nudCmdLivreMontant.Value = 0;
            }
            else
            {
                MessageBox.Show("Erreur lors de l'enregistrement de la commande.", "Erreur");
            }
        }


        /// <summary>
        /// Tri sur les colonnes du DataGridView des commandes de livres.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdLivreListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (lesCommandesLivre == null || lesCommandesLivre.Count == 0) return;

            string titreColonne = dgvCmdLivreListe.Columns[e.ColumnIndex].DataPropertyName;
            List<CommandeDocumentLivre> sortedList;

            SortOrder currentOrder = dgvCmdLivreListe.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
            bool ascending = currentOrder == SortOrder.Descending || currentOrder == SortOrder.None;

            // Appliquer le tri
            switch (titreColonne)
            {
                case NomColonneDateCommande:
                    sortedList = ascending ? lesCommandesLivre.OrderBy(c => c.DateCommande).ToList() : lesCommandesLivre.OrderByDescending(c => c.DateCommande).ToList();
                    break;
                case NomColonneMontant:
                    sortedList = ascending ? lesCommandesLivre.OrderBy(c => c.Montant).ToList() : lesCommandesLivre.OrderByDescending(c => c.Montant).ToList();
                    break;
                case NomColonneNbExemplaire:
                    sortedList = ascending ? lesCommandesLivre.OrderBy(c => c.NbExemplaire).ToList() : lesCommandesLivre.OrderByDescending(c => c.NbExemplaire).ToList();
                    break;
                case NomColonneLibelleSuivi:
                    sortedList = ascending ? lesCommandesLivre.OrderBy(c => c.LibelleSuivi).ToList() : lesCommandesLivre.OrderByDescending(c => c.LibelleSuivi).ToList();
                    break;
                default:
                    return;
            }

            // Mettre à jour le glyphe de tri
            foreach (DataGridViewColumn column in dgvCmdLivreListe.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dgvCmdLivreListe.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ascending ? SortOrder.Ascending : SortOrder.Descending;


            RemplirCmdLivreListe(sortedList);
            lesCommandesLivre = sortedList;
        }

        /// <summary>
        /// Se déclenche lors de l'affichage du contrôle d'édition pour une cellule.
        /// Permet de configurer la ComboBox pour la colonne 'LibelleSuivi'.
        /// </summary>
        private void dgvCmdLivreListe_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvCmdLivreListe.CurrentCell.ColumnIndex == dgvCmdLivreListe.Columns[NomColonneLibelleSuivi].Index)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    // Récupérer la commande actuelle pour obtenir l'étape courante
                    CommandeDocumentLivre commandeCourante = (CommandeDocumentLivre)dgvCmdLivreListe.CurrentRow.DataBoundItem;
                    string etapeActuelle = commandeCourante.LibelleSuivi;

                    // Obtenir les étapes valides
                    List<Suivi> etapesValides = GetValidNextSuiviSteps(etapeActuelle);

                    // Configurer la ComboBox
                    cb.DataSource = etapesValides;
                    cb.DisplayMember = NomProprieteLibelle;
                    cb.ValueMember = "Id";
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;

                    // Retirer puis rajouter le handler pour éviter les appels multiples
                    cb.SelectedIndexChanged -= new EventHandler(ComboBoxSuivi_SelectedIndexChanged);
                    cb.SelectedIndexChanged += new EventHandler(ComboBoxSuivi_SelectedIndexChanged);
                }
            }
        }

        /// <summary>
        /// Se déclenche quand la sélection dans la ComboBox d'édition change.
        /// Informe le DataGridView que la cellule est modifiée.
        /// </summary>
        private void ComboBoxSuivi_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvCmdLivreListe.NotifyCurrentCellDirty(true);
        }


        /// <summary>
        /// Se déclenche après qu'une valeur de cellule a été modifiée et validée.
        /// </summary>
        private void dgvCmdLivreListe_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCmdLivreListe.Columns[NomColonneLibelleSuivi].Index && e.RowIndex >= 0)
            {
                commandeEnCoursEdition = (CommandeDocumentLivre)dgvCmdLivreListe.Rows[e.RowIndex].DataBoundItem;
                rowIndexEnCoursEdition = e.RowIndex;

                btnCmdLivreSaveSuivi.Enabled = true;
            }
        }

        /// <summary>
        /// Détermine les étapes de suivi valides suivantes en fonction de l'étape actuelle et des règles métier.
        /// </summary>
        /// <param name="etapeActuelleLibelle">Le libellé de l'étape actuelle.</param>
        /// <returns>Une liste d'objets Suivi valides.</returns>
        private List<Suivi> GetValidNextSuiviSteps(string etapeActuelleLibelle)
        {
            List<Suivi> etapesPossibles = new List<Suivi>();
            Suivi etapeActuelle = lesEtapesSuivi.FirstOrDefault(s => s.Libelle.Equals(etapeActuelleLibelle, StringComparison.OrdinalIgnoreCase));

            if (etapeActuelle == null) return etapesPossibles;

            // IDs des étapes
            const int ID_EN_COURS = 1;
            const int ID_LIVREE = 2;
            const int ID_REGLEE = 3;
            const int ID_RELANCEE = 4;

            etapesPossibles.Add(etapeActuelle);

            switch (etapeActuelle.Id)
            {
                case ID_EN_COURS:
                    // Peut passer à livrée ou relancée
                    etapesPossibles.AddRange(lesEtapesSuivi.Where(s => s.Id == ID_LIVREE || s.Id == ID_RELANCEE));
                    break;
                case ID_LIVREE:
                    // Peut passer à réglée
                    etapesPossibles.AddRange(lesEtapesSuivi.Where(s => s.Id == ID_REGLEE));
                    break;
                case ID_REGLEE:
                    // Ne peut pas changer
                    break;
                case ID_RELANCEE:
                    // Peut passer à en cours ou livrée
                    etapesPossibles.AddRange(lesEtapesSuivi.Where(s => s.Id == ID_EN_COURS || s.Id == ID_LIVREE));
                    break;
            }

            return etapesPossibles.Distinct().OrderBy(s => s.Id).ToList();
        }

        /// <summary>
        /// Gère le clic sur le bouton d'enregistrement des modifications de suivi.
        /// </summary>
        private void btnCmdLivreSaveSuivi_Click(object sender, EventArgs e)
        {
            if (commandeEnCoursEdition != null && rowIndexEnCoursEdition != -1)
            {
                try
                {
                    string nouveauLibelleSuivi = dgvCmdLivreListe.Rows[rowIndexEnCoursEdition].Cells[NomColonneLibelleSuivi].Value.ToString();
                    Suivi nouvelleEtape = lesEtapesSuivi.FirstOrDefault(s => s.Libelle.Equals(nouveauLibelleSuivi, StringComparison.OrdinalIgnoreCase));

                    if (nouvelleEtape != null)
                    {
                        // Appel au contrôleur pour modifier le suivi
                        string idCommande = commandeEnCoursEdition.IdCommande;
                        int nouvelIdSuivi = nouvelleEtape.Id;

                        if (controller.ModifierSuiviCommande(idCommande, nouvelIdSuivi))
                        {
                            MessageBox.Show("Étape de suivi mise à jour avec succès.", MessageBoxTitreSucces);
                            AfficheCmdLivreCommandes();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la mise à jour de l'étape de suivi.", "Erreur");
                            AfficheCmdLivreCommandes();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur : La nouvelle étape sélectionnée est invalide.", "Erreur");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur");
                }
                finally
                {
                    commandeEnCoursEdition = null;
                    rowIndexEnCoursEdition = -1;
                    btnCmdLivreSaveSuivi.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Gère la sélection d'une ligne dans le DataGridView des commandes de livres.
        /// Active ou désactive le bouton de suppression en fonction de l'étape de suivi.
        /// </summary>
        private void dgvCmdLivreListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdLivreListe.CurrentRow != null && dgvCmdLivreListe.CurrentRow.DataBoundItem is CommandeDocumentLivre selectedCommande)
            {
                // Libellés des étapes où la suppression est interdite
                const string ETAPE_LIVREE = "livrée";
                const string ETAPE_REGLEE = "réglée";

                bool canDelete = !selectedCommande.LibelleSuivi.Equals(ETAPE_LIVREE, StringComparison.OrdinalIgnoreCase) &&
                                 !selectedCommande.LibelleSuivi.Equals(ETAPE_REGLEE, StringComparison.OrdinalIgnoreCase);

                btnCmdLivreSupprimer.Enabled = canDelete;
            }
            else
            {
                // Désactiver le bouton si aucune ligne sélectionnée
                btnCmdLivreSupprimer.Enabled = false;
            }
        }


        /// <summary>
        /// Gère le clic sur le bouton de suppression d'une commande de livre.
        /// </summary>
        private void btnCmdLivreSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCmdLivreListe.CurrentRow != null && dgvCmdLivreListe.CurrentRow.DataBoundItem is CommandeDocumentLivre selectedCommande)
            {
                DialogResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la commande N° {selectedCommande.IdCommande} du {selectedCommande.DateCommande:dd/MM/yyyy} ?",
                                                     "Confirmation de suppression",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Appel au contrôleur pour supprimer la commande
                    if (controller.SupprimerCommande(selectedCommande.IdCommande))
                    {
                        MessageBox.Show("Commande supprimée avec succès.", MessageBoxTitreSucces);
                        AfficheCmdLivreCommandes();
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

        #endregion

        #region Onglet Commande Dvd
        private readonly BindingSource bdgCmdDvdListe = new BindingSource();
        private List<CommandeDocumentLivre> lesCommandesDvd = new List<CommandeDocumentLivre>();
        private Dvd dvdCommandeSelectionne = null;
        private CommandeDocumentLivre commandeDvdEnCoursEdition = null;
        private int rowIndexDvdEnCoursEdition = -1;

        /// <summary>
        /// Ouverture de l'onglet Commande Dvd :
        /// Récupération des étapes de suivi et initialisation des champs.
        /// </summary>
        private void tabCommandeDvd_Enter(object sender, EventArgs e)
        {
            if (lesEtapesSuivi.Count == 0)
            {
                lesEtapesSuivi = controller.GetAllSuivi();
            }

            VideCmdDvdZones();
            AccesCmdDvdGroupBox(false);
            txbCmdDvdNumRecherche.Text = "";
            btnCmdDvdSaveSuivi.Enabled = false;
        }

        /// <summary>
        /// Recherche d'un DVD par son numéro et affichage de ses informations et commandes.
        /// </summary>
        private void btnCmdDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCmdDvdNumRecherche.Text.Equals(""))
            {
                dvdCommandeSelectionne = controller.GetDvdById(txbCmdDvdNumRecherche.Text);
                if (dvdCommandeSelectionne != null)
                {
                    AfficheCmdDvdInfos(dvdCommandeSelectionne);
                    AfficheCmdDvdCommandes();
                    AccesCmdDvdGroupBox(true);
                }
                else
                {
                    MessageBox.Show("Numéro de DVD introuvable.", "Erreur");
                    VideCmdDvdZones();
                    AccesCmdDvdGroupBox(false);
                    dvdCommandeSelectionne = null;
                }
            }
            else
            {
                VideCmdDvdZones();
                AccesCmdDvdGroupBox(false);
                dvdCommandeSelectionne = null;
            }
        }

        /// <summary>
        /// Affichage des informations du DVD sélectionné dans la section commande.
        /// </summary>
        /// <param name="dvd">Le DVD à afficher.</param>
        private void AfficheCmdDvdInfos(Dvd dvd)
        {
            txbCmdDvdTitre.Text = dvd.Titre;
            txbCmdDvdRealisateur.Text = dvd.Realisateur;
            txbCmdDvdDuree.Text = dvd.Duree.ToString();
            txbCmdDvdSynopsis.Text = dvd.Synopsis;
            txbCmdDvdGenre.Text = dvd.Genre;
            txbCmdDvdPublic.Text = dvd.Public;
            txbCmdDvdRayon.Text = dvd.Rayon;
            txbCmdDvdImage.Text = dvd.Image;
            txbCmdDvdNumero.Text = dvd.Id;

            string imagePath = dvd.Image;
            try
            {
                pcbCmdDvdImage.Image = string.IsNullOrEmpty(imagePath) ? null : Image.FromFile(imagePath);
            }
            catch
            {
                pcbCmdDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du DVD et la liste des commandes.
        /// </summary>
        private void VideCmdDvdInfos()
        {
            txbCmdDvdTitre.Text = "";
            txbCmdDvdRealisateur.Text = "";
            txbCmdDvdDuree.Text = "";
            txbCmdDvdSynopsis.Text = "";
            txbCmdDvdGenre.Text = "";
            txbCmdDvdPublic.Text = "";
            txbCmdDvdRayon.Text = "";
            txbCmdDvdImage.Text = "";
            pcbCmdDvdImage.Image = null;
            txbCmdDvdNumero.Text = "";

            lesCommandesDvd = new List<CommandeDocumentLivre>();
            RemplirCmdDvdListe(lesCommandesDvd);

            dtpCmdDvdDate.Value = DateTime.Now;
            nudCmdDvdNbEx.Value = 1;
            nudCmdDvdMontant.Value = 0;
        }

        /// <summary>
        /// Vide la zone de recherche par numéro pour les DVD.
        /// </summary>
        private void VideCmdDvdZones()
        {
            txbCmdDvdNumRecherche.Text = "";
            VideCmdDvdInfos();
        }

        /// <summary>
        /// Récupère et affiche les commandes associées au DVD actuellement sélectionné.
        /// </summary>
        private void AfficheCmdDvdCommandes()
        {
            if (dvdCommandeSelectionne != null)
            {
                string idDvd = dvdCommandeSelectionne.Id;
                lesCommandesDvd = controller.GetCommandesLivre(idDvd);
                lesCommandesDvd = lesCommandesDvd.OrderByDescending(c => c.DateCommande).ToList();
                RemplirCmdDvdListe(lesCommandesDvd);
            }
            else
            {
                RemplirCmdDvdListe(new List<CommandeDocumentLivre>());
            }
        }

        /// <summary>
        /// Remplit le DataGridView des commandes DVD avec la liste fournie.
        /// </summary>
        /// <param name="commandes">Liste des commandes (ViewModel).</param>
        private void RemplirCmdDvdListe(List<CommandeDocumentLivre> commandes)
        {
            dgvCmdDvdListe.AutoGenerateColumns = false;
            dgvCmdDvdListe.Columns.Clear();
            dgvCmdDvdListe.DataSource = null;

            DataGridViewTextBoxColumn colDate = new DataGridViewTextBoxColumn { Name = NomColonneDateCommande, HeaderText = "Date Commande", DataPropertyName = NomColonneDateCommande, ReadOnly = true, DefaultCellStyle = { Format = "dd/MM/yyyy" } };
            DataGridViewTextBoxColumn colMontant = new DataGridViewTextBoxColumn { Name = NomColonneMontant, HeaderText = "Montant", DataPropertyName = NomColonneMontant, ReadOnly = true, DefaultCellStyle = { Format = "C2" } };
            DataGridViewTextBoxColumn colNbEx = new DataGridViewTextBoxColumn { Name = NomColonneNbExemplaire, HeaderText = "Nb Exemplaires", DataPropertyName = NomColonneNbExemplaire, ReadOnly = true };
            DataGridViewComboBoxColumn colSuivi = new DataGridViewComboBoxColumn { Name = NomColonneLibelleSuivi, HeaderText = "Étape Suivi", DataPropertyName = NomColonneLibelleSuivi, DataSource = lesEtapesSuivi, DisplayMember = NomProprieteLibelle, ValueMember = NomProprieteLibelle, ReadOnly = false, FlatStyle = FlatStyle.Flat };

            dgvCmdDvdListe.Columns.AddRange(new DataGridViewColumn[] { colDate, colMontant, colNbEx, colSuivi });
            dgvCmdDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            bdgCmdDvdListe.DataSource = commandes;
            dgvCmdDvdListe.DataSource = bdgCmdDvdListe;

            commandeDvdEnCoursEdition = null;
            rowIndexDvdEnCoursEdition = -1;
            btnCmdDvdSaveSuivi.Enabled = false;
        }

        /// <summary>
        /// Gère l'activation/désactivation des zones d'information du DVD et de création de commande.
        /// </summary>
        /// <param name="acces">True pour activer, False pour désactiver.</param>
        private void AccesCmdDvdGroupBox(bool acces)
        {
            grpCmdDvdInfos.Enabled = acces;
            grpCmdDvdNouvelleCmd.Enabled = acces;
            dgvCmdDvdListe.Enabled = acces;
        }

        /// <summary>
        /// Validation et enregistrement d'une nouvelle commande de DVD.
        /// </summary>
        private void btnCmdDvdValiderCmd_Click(object sender, EventArgs e)
        {
            if (dvdCommandeSelectionne == null)
            {
                MessageBox.Show("Veuillez d'abord rechercher et sélectionner un DVD.", "Information");
                return;
            }

            if (nudCmdDvdNbEx.Value <= 0)
            {
                MessageBox.Show("Le nombre d'exemplaires doit être supérieur à zéro.", "Information");
                nudCmdDvdNbEx.Focus();
                return;
            }
            if (nudCmdDvdMontant.Value <= 0)
            {
                MessageBox.Show("Le montant doit être supérieur à zéro.", "Information");
                nudCmdDvdMontant.Focus();
                return;
            }

            Suivi etapeEnCours = lesEtapesSuivi.FirstOrDefault(s => s.Libelle.Equals(ETAPE_SUIVI_EN_COURS, StringComparison.OrdinalIgnoreCase));
            if (etapeEnCours == null)
            {
                MessageBox.Show($"L'étape de suivi '{ETAPE_SUIVI_EN_COURS}' n'a pas été trouvée. Impossible de créer la commande.", "Erreur");
                return;
            }
            int idSuiviEnCours = etapeEnCours.Id;

            Commande nouvelleCommande = new Commande("0", dtpCmdDvdDate.Value, (double)nudCmdDvdMontant.Value);
            CommandeDocument nouvelleCommandeDoc = new CommandeDocument("0", (int)nudCmdDvdNbEx.Value, dvdCommandeSelectionne.Id, idSuiviEnCours);

            if (controller.CreerCommandeDocument(nouvelleCommande, nouvelleCommandeDoc))
            {
                MessageBox.Show("Nouvelle commande enregistrée avec succès.", MessageBoxTitreSucces);
                AfficheCmdDvdCommandes();
                dtpCmdDvdDate.Value = DateTime.Now;
                nudCmdDvdNbEx.Value = 1;
                nudCmdDvdMontant.Value = 0;
            }
        }

        /// <summary>
        /// Tri sur les colonnes du DataGridView des commandes de DVD.
        /// </summary>
        private void dgvCmdDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (lesCommandesDvd == null || lesCommandesDvd.Count == 0) return;

            string titreColonne = dgvCmdDvdListe.Columns[e.ColumnIndex].DataPropertyName;
            List<CommandeDocumentLivre> sortedList;
            SortOrder currentOrder = dgvCmdDvdListe.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
            bool ascending = currentOrder == SortOrder.Descending || currentOrder == SortOrder.None;

            switch (titreColonne)
            {
                case NomColonneDateCommande: sortedList = ascending ? lesCommandesDvd.OrderBy(c => c.DateCommande).ToList() : lesCommandesDvd.OrderByDescending(c => c.DateCommande).ToList(); break;
                case NomColonneMontant: sortedList = ascending ? lesCommandesDvd.OrderBy(c => c.Montant).ToList() : lesCommandesDvd.OrderByDescending(c => c.Montant).ToList(); break;
                case NomColonneNbExemplaire: sortedList = ascending ? lesCommandesDvd.OrderBy(c => c.NbExemplaire).ToList() : lesCommandesDvd.OrderByDescending(c => c.NbExemplaire).ToList(); break;
                case NomColonneLibelleSuivi: sortedList = ascending ? lesCommandesDvd.OrderBy(c => c.LibelleSuivi).ToList() : lesCommandesDvd.OrderByDescending(c => c.LibelleSuivi).ToList(); break;
                default: return;
            }

            foreach (DataGridViewColumn column in dgvCmdDvdListe.Columns) { column.HeaderCell.SortGlyphDirection = SortOrder.None; }
            dgvCmdDvdListe.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ascending ? SortOrder.Ascending : SortOrder.Descending;

            RemplirCmdDvdListe(sortedList);
            lesCommandesDvd = sortedList;
        }

        /// <summary>
        /// Configuration de la ComboBox d'édition pour l'étape de suivi des commandes DVD.
        /// </summary>
        private void dgvCmdDvdListe_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvCmdDvdListe.CurrentCell.ColumnIndex == dgvCmdDvdListe.Columns[NomColonneLibelleSuivi].Index && e.Control is ComboBox cb)
            {
                CommandeDocumentLivre commandeCourante = (CommandeDocumentLivre)dgvCmdDvdListe.CurrentRow.DataBoundItem;
                string etapeActuelle = commandeCourante.LibelleSuivi;
                List<Suivi> etapesValides = GetValidNextSuiviSteps(etapeActuelle);

                cb.DataSource = null;
                cb.DataSource = etapesValides;
                cb.DisplayMember = NomProprieteLibelle;
                cb.ValueMember = "Id";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                // S'assurer que la valeur actuelle est sélectionnée si elle est valide
                cb.SelectedValue = etapesValides.FirstOrDefault(s => s.Libelle.Equals(etapeActuelle, StringComparison.OrdinalIgnoreCase))?.Id ?? -1;

                cb.SelectedIndexChanged -= ComboBoxSuiviDvd_SelectedIndexChanged;
                cb.SelectedIndexChanged += ComboBoxSuiviDvd_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// Informe le DGV DVD que la cellule de suivi a changé.
        /// </summary>
        private void ComboBoxSuiviDvd_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvCmdDvdListe.NotifyCurrentCellDirty(true);
        }

        /// <summary>
        /// Se déclenche après la modification d'une cellule dans le DGV DVD (pour activer Save).
        /// </summary>
        private void dgvCmdDvdListe_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCmdDvdListe.Columns[NomColonneLibelleSuivi].Index && e.RowIndex >= 0)
            {
                commandeDvdEnCoursEdition = (CommandeDocumentLivre)dgvCmdDvdListe.Rows[e.RowIndex].DataBoundItem;
                rowIndexDvdEnCoursEdition = e.RowIndex;
                btnCmdDvdSaveSuivi.Enabled = true;
            }
        }

        /// <summary>
        /// Gère le clic sur le bouton d'enregistrement des modifications de suivi pour les DVD.
        /// </summary>
        private void btnCmdDvdSaveSuivi_Click(object sender, EventArgs e)
        {
            if (commandeDvdEnCoursEdition != null && rowIndexDvdEnCoursEdition != -1)
            {
                try
                {
                    string nouveauLibelleSuivi = dgvCmdDvdListe.Rows[rowIndexDvdEnCoursEdition].Cells[NomColonneLibelleSuivi].Value.ToString();
                    Suivi nouvelleEtape = lesEtapesSuivi.FirstOrDefault(s => s.Libelle.Equals(nouveauLibelleSuivi, StringComparison.OrdinalIgnoreCase));

                    if (nouvelleEtape != null)
                    {
                        string idCommande = commandeDvdEnCoursEdition.IdCommande;
                        int nouvelIdSuivi = nouvelleEtape.Id;

                        if (controller.ModifierSuiviCommande(idCommande, nouvelIdSuivi))
                        {
                            MessageBox.Show("Étape de suivi mise à jour avec succès.", MessageBoxTitreSucces);
                            AfficheCmdDvdCommandes();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la mise à jour de l'étape de suivi.", "Erreur");
                            AfficheCmdDvdCommandes();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur : La nouvelle étape sélectionnée est invalide.", "Erreur");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur");
                }
                finally
                {
                    commandeDvdEnCoursEdition = null;
                    rowIndexDvdEnCoursEdition = -1;
                    btnCmdDvdSaveSuivi.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Gère la sélection d'une ligne dans le DGV des commandes DVD (pour activer/désactiver Supprimer).
        /// </summary>
        private void dgvCmdDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdDvdListe.CurrentRow != null && dgvCmdDvdListe.CurrentRow.DataBoundItem is CommandeDocumentLivre selectedCommande)
            {
                const string ETAPE_LIVREE = "livrée";
                const string ETAPE_REGLEE = "réglée";
                bool canDelete = !selectedCommande.LibelleSuivi.Equals(ETAPE_LIVREE, StringComparison.OrdinalIgnoreCase) &&
                                 !selectedCommande.LibelleSuivi.Equals(ETAPE_REGLEE, StringComparison.OrdinalIgnoreCase);
                btnCmdDvdSupprimer.Enabled = canDelete;
            }
            else
            {
                btnCmdDvdSupprimer.Enabled = false;
            }
        }

        /// <summary>
        /// Gère le clic sur le bouton de suppression d'une commande de DVD.
        /// </summary>
        private void btnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCmdDvdListe.CurrentRow != null && dgvCmdDvdListe.CurrentRow.DataBoundItem is CommandeDocumentLivre selectedCommande)
            {
                DialogResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la commande N° {selectedCommande.IdCommande} du {selectedCommande.DateCommande:dd/MM/yyyy} ?",
                                                     "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (controller.SupprimerCommande(selectedCommande.IdCommande))
                    {
                        MessageBox.Show("Commande supprimée avec succès.", MessageBoxTitreSucces);
                        AfficheCmdDvdCommandes();
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

        #endregion

    }
}