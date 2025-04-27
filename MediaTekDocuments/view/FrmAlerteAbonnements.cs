using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre d'alerte pour les abonnements qui se terminent bientôt
    /// </summary>
    public partial class FrmAlerteAbonnements : Form
    {
        /// <summary>
        /// Constructeur : initialise les composants
        /// </summary>
        /// <param name="abonnementsFinProche">Liste de tuples (revue, date fin) à afficher</param>
        public FrmAlerteAbonnements(List<Tuple<Revue, DateTime>> abonnementsFinProche)
        {
            InitializeComponent();
            RemplirListeAbonnements(abonnementsFinProche);
        }

        /// <summary>
        /// Remplit la liste des abonnements qui se terminent bientôt
        /// </summary>
        /// <param name="abonnementsFinProche">Liste de tuples (revue, date fin) à afficher</param>
        private void RemplirListeAbonnements(List<Tuple<Revue, DateTime>> abonnementsFinProche)
        {
            if (abonnementsFinProche.Count == 0)
            {
                Close();
                return;
            }

            lvAbonnements.Clear();
            lvAbonnements.Columns.Add("Id");
            lvAbonnements.Columns.Add("Titre");
            lvAbonnements.Columns.Add("Date fin abonnement");
            lvAbonnements.Columns.Add("Périodicité");
            lvAbonnements.Columns.Add("Délai mise à dispo");

            foreach (Tuple<Revue, DateTime> tuple in abonnementsFinProche)
            {
                Revue revue = tuple.Item1;
                DateTime dateFinAbonnement = tuple.Item2;

                ListViewItem item = new ListViewItem(revue.Id);
                item.SubItems.Add(revue.Titre);
                item.SubItems.Add(dateFinAbonnement.ToShortDateString());
                item.SubItems.Add(revue.Periodicite);
                item.SubItems.Add(revue.DelaiMiseADispo.ToString());

                lvAbonnements.Items.Add(item);
            }

            lvAbonnements.Columns[0].Width = 50;
            lvAbonnements.Columns[1].Width = 200;
            lvAbonnements.Columns[2].Width = 100;
            lvAbonnements.Columns[3].Width = 80;
            lvAbonnements.Columns[4].Width = 100;
        }

        /// <summary>
        /// Ferme la fenêtre lorsqu'on clique sur le bouton OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
} 