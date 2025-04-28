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

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre de connexion à l'application
    /// </summary>
    public partial class FrmAuth : Form
    {
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Constructeur de la fenêtre de connexion
        /// </summary>
        public FrmAuth()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton de connexion
        /// </summary>
        private void btnConnection_Click(object sender, EventArgs e)
        {
            string login = txbLogin.Text;
            string password = txbPassword.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez saisir votre login et votre mot de passe.", "Champs requis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Utilisateur utilisateur = this.controller.AuthenticateUser(login, password);

            if (utilisateur != null)
            {
                this.Hide();

                FrmMediatek frmMediatek = new FrmMediatek(utilisateur.IdService);
                frmMediatek.FormClosed += (s, args) => this.Close();
                frmMediatek.Show();
            }
            else
            {
                MessageBox.Show("Login ou mot de passe incorrect.", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPassword.Text = "";
                txbPassword.Focus();
            }
        }
    }
}
