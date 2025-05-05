# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) et les commandes/abonnements d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.<br>
Voici le lien du dépôt d'origine contenant la présentation de l'application d'origine: https://github.com/CNED-SLAM/MediaTekDocuments
## Fonctionnalités ajoutées
Trois onglets ont été ajoutés permettant respectivement la commande de livres, la commande de dvd et la commande (abonnement) de revues.<br>
Une fenêtre d'alerte pour les abonnements se terminant bientôt a été ajoutée.<br>
Enfin, une fenêtre d'authentification a été ajoutée à l'application.
## Onglets:
### Commandes de livres
Cet onglet permet: la recherche d'un livre par son numéro de document, l'affichage de ses informations détaillées, l'affichage de ses commandes passées, le passage d'une nouvelle commande, la modification de l'étape de suivi d'une commande et la suppression d'une commande.<br>
![img1](https://github.com/frMGB/mediatekdocuments/blob/main/assets/132413.png?raw=true)
#### Recherche
<strong>Par le numéro :</strong> il est possible de saisir le numéro d'un livre et, en cliquant sur "Rechercher", ses informations détaillées et ses commandes apparaissent.
#### Tri
Le fait de cliquer sur le titre d'une des colonnes de la liste des commandes permet de trier la liste par rapport à la colonne choisie.
#### Commande
Une fois un livre sélectionné, il est possible de passer une nouvelle commande en entrant le nombre d'exemplaires et le montant de cette dernière.
#### Modification
Il est possible de modifier l'étape de suivi d'une commande. Pour cela il faut: sélectionner une commande dans la liste, cliquer et choisir une des étapes de suivi possibles dans la comboBox, et enfin valider la modification en cliquant sur le bouton 'Modifier'.
#### Suppression
Il est possible de supprimer une commande. Pour cela il faut sélectionner une commande dans la liste dont l'étape de suivi permet la suppression et cliquer sur le bouton 'Supprimer'.
### Commandes de DVD
Cet onglet permet les mêmes fonctionnalités que l'onglet 'Commandes de livres', adaptées aux DVD.
![img2](https://github.com/frMGB/mediatekdocuments/blob/main/assets/132431.png?raw=true)
### Commandes de revues
Cet onglet permet: la recherche d'une revue par son numéro de document, l'affichage de ses informations détaillées, l'affichage de ses commandes (abonnements) passées, le passage d'une nouvelle commande et la suppression d'une commande.<br>
![img3](https://github.com/frMGB/mediatekdocuments/blob/main/assets/132443.png?raw=true)
Le fonctionnement est quasiment identique aux onglets précédents, les différences résident au niveau:<br>
- du nombre d'exemplaires, remplacé par la date de fin d'abonnement.<br>
- de l'étape de suivi absente et par extension la modification de cette dernière aussi, étant gérée dans l'onglet 'Parutions des revues'.
## Fenêtres:
### Fenêtre d'alerte
Cette fenêtre alerte l'utilisateur quant aux abonnements se terminant dans moins de 30 jours, sous forme d'un tableau comprenant le titre et la date de fin d'abonnement de ces derniers.<br>
![img4](https://github.com/frMGB/mediatekdocuments/blob/main/assets/132323.png?raw=true)
### Fenêtre d'authentification
Cette fenêtre permet, au démarrage de l'application, à un utilisateur de se connecter. Elle gère alors la logique de vérification des informations de connexion fournies par rapport à la base de données, ainsi que les permissions accordées à l'utilisateur se connectant.<br>
![img5](https://github.com/frMGB/mediatekdocuments/blob/main/assets/132300.png?raw=true)
## Installation de l'application
Ce mode opératoire permet d'installer l'application pour pouvoir travailler dessus.<br>
- Installer Visual Studio 2019 entreprise et les extension Specflow et newtonsoft.json (pour ce dernier, voir l'article "Accéder à une API REST à partir d'une application C#" dans le wiki de ce dépôt : consulter juste le début pour la configuration, car la suite permet de comprendre le code existant).<br>
- Télécharger le code et le dézipper puis renommer le dossier en "mediatekdocuments".<br>
- Récupérer et installer l'API REST nécessaire (https://github.com/CNED-SLAM/rest_mediatekdocuments) ainsi que la base de données (les explications sont données dans le readme correspondant).
