using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace mah
{
    class LLC_Bi
    {
        List<Maillon_bi> list;
        private Boolean triee;
        private double coordX;
        private double coordY;
        /***********LES COULEURS*************/
        private SolidColorBrush couleurAlgo = Brushes.White;
        private SolidColorBrush couleurFondMaillon = Brushes.Gainsboro;
        private SolidColorBrush couleurFondCase = Brushes.White;
        private SolidColorBrush couleurBordureMaillon = Brushes.Black;
        /*********** LES CONSTANTES ***********/
        private const double widthOfmaillon = 50;
        private const double heightOfmaillon = 100;
        private const double tailleFleche = 40;
        private const int nbmaillonmax = 20;
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;
        /***************************************/

        public LLC_Bi(Boolean triee, double coordX, double coordY)
        {
            this.triee = triee;// si la liste est triée ou pas
            this.coordX = coordX;//coordonnées de la liste
            this.coordY = coordY;
            list = new List<Maillon_bi>();//instansiation de la liste
        }
        public void chargement_alea(int nbvaleur, Canvas c)//initialisation de la liste avec "nbvaleur" maillons
        {
            Random rndNumber = new Random();// gérération aléatoire des valeurs
            Boolean stop = new Boolean();
            int[] tabValeur = new int[nbvaleur];
            list = new List<Maillon_bi>();
            Maillon_bi maillon;
            Commentaire comChar_Impo = new Commentaire("Chargement impossible le nombre de valeur dépasse le nombre maximum", Brushes.Black, this.coordX + 50, this.coordY - 50, 400, 30, Brushes.PaleGreen, Brushes.White);
            int i;
            if (nbvaleur > nbmaillonmax)//Si le nombre de maillons et supérieur au nombre max
            {
                comChar_Impo.ajouterCanvas(c);
                comChar_Impo.disparaitre(5);
            }
            else
            {
                for (i = 0; i < nbvaleur; i++) //Géneration des nombres aléatoires
                {
                    stop = true;
                    while (stop == true)
                    {
                        tabValeur[i] = rndNumber.Next(0, 40);
                        stop = false;
                        for (int j = i - 1; j >= 0; j--)
                            if (tabValeur[i] == tabValeur[j]) stop = true;
                    }
                }
                if (this.triee) Array.Sort(tabValeur); // tabValuer doit être triée si l'attribut triee est à vrai
                if (nbvaleur == 1)
                {
                    maillon = new Maillon_bi(tabValeur[0], this.coordX + tailleFleche, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 3, 3, tailleFleche);
                    list.Add(maillon);//ajouter le maillon dans la liste
                }
                else
                {
                    maillon = new Maillon_bi(tabValeur[0], this.coordX + tailleFleche, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 3, 1, tailleFleche);
                    list.Add(maillon);
                    for (i = 1; i < nbvaleur - 1; i++)//Création des maillons 
                    {
                        maillon = new Maillon_bi(tabValeur[i], this.coordX + i * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 2, 1, tailleFleche);
                        list.Add(maillon);
                    }
                    //Partie du dernier maillon qui est à Null
                    maillon = new Maillon_bi(tabValeur[nbvaleur - 1], this.coordX + i * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 2, 3, tailleFleche);
                    list.Add(maillon);
                }
            }
        }
        public async Task recherche_seq(int val, int[] tabInfo, Canvas c, Boolean insSup, Commentaire comPrincipal, Canvas Alg)
        {
            //recherche de la valeur val et returne indice qui se trouve dans tabInfo
            int i = 0;
            tabInfo[0] = 0;
            Algo algo = new Algo(8, coordX_Algo, coordY_Algo); //Création d'un algorithme de déroulement
            SolidColorBrush couleurparcours = Brushes.Red;
            SolidColorBrush couleurtrouve = Brushes.Green;
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 5, this.coordY - 35, 50, 50, couleurparcours, couleurparcours);
            comPrincipal.CouleurFond = Brushes.Yellow;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(1);
            comPrincipal.Text = "Recherche en cours ...";
            algo.afficher(Alg); //Affichage de l'algorithme
            await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);//Déroulement de l'algorithme 
            await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);
            await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
            await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
            com.ajouterCanvas(c);//faire aparaitre un commentaire
            com.disparaitre(0);
            com.opacity = 0;
            com.Height = 30;
            com.Width = 160;
            Fleche parcours = new Fleche(1);//instanciation d'une fléche
            parcours.Height = tailleFleche;
            parcours.StrokeThick = 3;
            parcours.Color = couleurparcours;
            parcours.L.Opacity = 0.9;
            parcours.CoordY = this.coordY + widthOfmaillon / 3;
            if (this.triee == false)//Si la liste n'est pas triée
            {
                while (i < this.list.Count & tabInfo[0] == 0)//Si on est pas arriver à la fin de la liste
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    this.list[i].opacity = 0.7;
                    if (this.list[i].Valeur != val)//Si la valeur du maillon est différente de la valeur rechercher
                    {
                        com.CoordX = this.list[i].CoordX;
                        com.CoordY = this.list[i].CoordY - 35;
                        this.list[i].colorMaillon(couleurparcours, couleurBordureMaillon, 2);//On colorie en gris le maillon
                        this.list[i].colorCase(couleurparcours, couleurBordureMaillon, 2);
                        com.Text = "  " + this.list[i].Valeur + " est différent de " + val;
                        com.apparaitre(Temps.time);//On affiche le commantaire que la valeur du maillon est différente de la valeur rechercher
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        parcours.CoordX = list[i].Adr.CoordX;
                        parcours.bout = new Bout(list[i].Adr.bout.coordX, list[i].Adr.bout.coordY, couleurparcours, list[i].Adr.bout.TypeBout, 2.5);
                        await parcours.dessiner(Temps.time,c);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        parcours.retirerCanvas(c);
                        com.disparaitre(0);//faire disparaitre le commentaire
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorCase(Brushes.Gainsboro, couleurBordureMaillon, 2);
                        await algo.colorer(couleurAlgo, 9, Temps.time);// faire apparaitre l'algorithme déroulant
                    }
                    else//Si on a trouver la valeur 
                    {
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        tabInfo[0] = 1;//Indiquer qu'on a trouvé la valeur 
                        comPrincipal.disparaitre(1);//disparition du commentaire
                        comPrincipal.Text = "La valeur " + val + " a été trouvée";
                        comPrincipal.CouleurFond = couleurtrouve;
                        comPrincipal.CouleurBordure = couleurtrouve;
                        this.list[i].opacity = 0.7;
                        comPrincipal.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 6, Temps.time);//apparaition de l'algorithme déroulant
                        if (insSup == false)//si la recherche n'est pas suivie d'une insértion/suppression
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 3);
                            await Task.Delay(TimeSpan.FromSeconds(4));
                        }
                        else
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                    await algo.colorer(couleurAlgo, 10, 0.2 * Temps.time);
                    i++;
                }
                await algo.colorer(couleurAlgo, 11, 0.2 * Temps.time);
            }
            else//Si la liste est triée 
            {
                parcours.CoordX = heightOfmaillon + list[i].CoordX;
                while (i < this.list.Count && tabInfo[0] == 0 && list[i].Valeur <= val)//milieu de la liste
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    this.list[i].opacity = 0.7;
                    if (this.list[i].Valeur != val)
                    {
                        com.CoordX = this.list[i].CoordX;
                        com.CoordY = this.list[i].CoordY - 35;
                        this.list[i].colorMaillon(couleurparcours, couleurBordureMaillon, 2);
                        this.list[i].colorCase(couleurparcours, couleurBordureMaillon, 2);
                        com.Text = "  " + this.list[i].Valeur + " est différent de " + val;
                        com.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        parcours.CoordX = list[i].Adr.CoordX;
                        parcours.bout = new Bout(list[i].Adr.bout.coordX, list[i].Adr.bout.coordY, couleurparcours, list[i].Adr.bout.TypeBout, 2.5);
                        await parcours.dessiner(Temps.time, c);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        com.disparaitre(0);
                        parcours.retirerCanvas(c);
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2);
                    }
                    else//Si on trouver la valeur
                    {
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        tabInfo[0] = 1;
                        comPrincipal.disparaitre(1);
                        comPrincipal.Text = "La valeur " + val + " a été trouvée";
                        comPrincipal.CouleurFond = couleurtrouve;
                        comPrincipal.CouleurBordure = couleurtrouve;
                        this.list[i].opacity = 0.7;
                        comPrincipal.apparaitre(Temps.time);//apparition du commentaire
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        if (insSup == false)//si la recherche n'est pas suivie d'une insértion/suppression
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 3);
                            await Task.Delay(TimeSpan.FromSeconds(4));
                        }
                        else
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
                    i++;
                }
                await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
            }
            if (tabInfo[0] == 0)//si la valeur n'a pas été trouvée
            {
                comPrincipal.disparaitre(2.5);
                comPrincipal.Width = 200;
                comPrincipal.Text = "La valeur " + val + " n'a pas été trouvée";
                comPrincipal.CouleurFond = couleurparcours;
                comPrincipal.CouleurBordure = couleurparcours;
                comPrincipal.apparaitre(Temps.time);
                com.enleverCanvas(c);
                if (!triee) tabInfo[1] = tabInfo[1] - 1;
            }
            comPrincipal.disparaitre(2);//faire disparaitre le commentaire
            await algo.colorer(couleurAlgo, 12, Temps.time);
            algo.disparaitre(Alg);
            foreach (Maillon maillon in list)
            {
                maillon.colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2);
                maillon.colorCase(couleurFondCase, couleurBordureMaillon, 2);
            }
        }
        public async Task recherche_seq_qeue(int val, int[] tabInfo, Canvas c, Boolean insSup, Commentaire comPrincipal, Canvas Alg)
        {
            //recherche de la valeur val et returne indice qui se trouve dans tabInfo
            int i = list.Count - 1;
            tabInfo[0] = 0;
            Algo algo = new Algo(12, coordX_Algo, coordY_Algo); //Création d'un algorithme de déroulement
            SolidColorBrush couleurparcours = Brushes.Red;
            SolidColorBrush couleurtrouve = Brushes.Green;
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 5, this.coordY - 35, 50, 50, couleurparcours, couleurparcours);
            comPrincipal.CouleurFond = Brushes.Yellow;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(1);
            comPrincipal.Text = "Recherche en cours ...";
            algo.afficher(Alg); //Affichage de l'algorithme
            await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);//Déroulement de l'algorithme 
            await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);
            await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
            await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
            com.ajouterCanvas(c);
            com.disparaitre(0);
            com.opacity = 0;
            com.Height = 30;
            com.Width = 160;
            Fleche parcours = new Fleche(1);
            parcours.Height = -tailleFleche;
            parcours.StrokeThick = 3;
            parcours.Color = couleurparcours;
            parcours.L.Opacity = 0.9;
            parcours.CoordY = this.coordY + 2 * widthOfmaillon / 3;
            if (this.triee == false)//Si la liste n'est pas triée
            {
                while (i >= 0 & tabInfo[0] == 0)//Si on est pas arriver à la fin de la liste
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    this.list[i].opacity = 0.7;
                    if (this.list[i].Valeur != val)//Si la valeur du maillon est différente de la valeur rechercher
                    {
                        com.CoordX = this.list[i].CoordX;
                        com.CoordY = this.list[i].CoordY - 35;
                        this.list[i].colorMaillon(couleurparcours, couleurBordureMaillon, 2);//On colorie en gris le maillon
                        this.list[i].colorCase(couleurparcours, couleurBordureMaillon, 2);
                        com.Text = "  " + this.list[i].Valeur + " est différent de " + val;
                        com.apparaitre(Temps.time);//On affiche le commantaire que la valeur du maillon est différente de la valeur rechercher
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        parcours.CoordX = list[i].Prec.CoordX;
                        parcours.bout = new Bout(list[i].Prec.bout.coordX, list[i].Prec.bout.coordY, couleurparcours, list[i].Prec.bout.TypeBout, 2.5);
                        await parcours.dessiner(Temps.time, c);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        parcours.retirerCanvas(c);
                        com.disparaitre(0);
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorCase(Brushes.Gainsboro, couleurBordureMaillon, 2);
                    }
                    else//Si on a trouver la valeur 
                    {
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        tabInfo[0] = 1;
                        comPrincipal.disparaitre(1);
                        comPrincipal.Text = "La valeur " + val + " a été trouvée";
                        comPrincipal.CouleurFond = couleurtrouve;
                        comPrincipal.CouleurBordure = couleurtrouve;
                        this.list[i].opacity = 0.7;
                        comPrincipal.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        if (insSup == false)
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            // this.list[i].vibrate(4, 19);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 3);
                            await Task.Delay(TimeSpan.FromSeconds(4));
                        }
                        else
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                    i--;
                    await algo.colorer(couleurAlgo, 10, 0.2 * Temps.time);
                }
                await algo.colorer(couleurAlgo, 11, 0.2 * Temps.time);
            }
            else
            {
                parcours.CoordX = heightOfmaillon + list[i].CoordX;
                while (i >= 0 && tabInfo[0] == 0 && list[i].Valeur >= val)
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    this.list[i].opacity = 0.7;
                    if (this.list[i].Valeur != val)
                    {
                        com.CoordX = this.list[i].CoordX;
                        com.CoordY = this.list[i].CoordY - 35;
                        this.list[i].colorMaillon(couleurparcours, couleurBordureMaillon, 2);
                        this.list[i].colorCase(couleurparcours, couleurBordureMaillon, 2);
                        com.Text = "  " + this.list[i].Valeur + " est différent de " + val;
                        com.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        parcours.CoordX = list[i].Prec.CoordX;
                        parcours.bout = new Bout(list[i].Prec.bout.coordX, list[i].Prec.bout.coordY, couleurparcours, list[i].Prec.bout.TypeBout, 2.5);
                        await parcours.dessiner(Temps.time,c);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        parcours.retirerCanvas(c);
                        com.disparaitre(0);
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorMaillon(Brushes.Gainsboro, couleurBordureMaillon, 2);
                    }
                    else//Si on trouver la valeur
                    {
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        tabInfo[0] = 1;
                        comPrincipal.disparaitre(1);
                        comPrincipal.Text = "La valeur " + val + " a été trouvée";
                        comPrincipal.CouleurFond = couleurtrouve;
                        comPrincipal.CouleurBordure = couleurtrouve;
                        this.list[i].opacity = 0.7;
                        comPrincipal.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        if (insSup == false)//si la recherche n'est pas suivie d'une insértion/suppression
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 3);
                            await Task.Delay(TimeSpan.FromSeconds(4));
                        }
                        else
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
                    i--;
                }
                await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
            }
            if (tabInfo[0] == 0)
            {
                comPrincipal.disparaitre(2.5);// faire disparaitre le commentaire
                comPrincipal.Text = "La valeur " + val + " n'a pas été trouvée";
                comPrincipal.CouleurFond = couleurparcours;
                comPrincipal.CouleurBordure = couleurparcours;
                comPrincipal.apparaitre(Temps.time);//apparition du commentaire
                com.enleverCanvas(c);
                if (!triee) tabInfo[1] = tabInfo[1] + 1;//si la liste n'est pas triée 
            }
            comPrincipal.disparaitre(2);//faire disparaitre le commentaire
            await algo.colorer(couleurAlgo, 12, Temps.time);
            algo.disparaitre(Alg);//faire disparaitre l'algorithme déroulant
            foreach (Maillon maillon in list)
            {
                maillon.colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2);
                maillon.colorCase(couleurFondCase, couleurBordureMaillon, 2);
            }
        }

        public async Task insert(int val, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {

            int[] tabInfo = new int[2];
            int pos = 0;
            Algo algo = new Algo(13, coordX_Algo, coordY_Algo);
            if (list.Count != 0) await recherche_seq(val, tabInfo, c, true, comPrincipal, Alg);//recherche de la valeur dans la liste
            if (tabInfo[0] == 0)//si la valeur n'existe pas dans la liste
            {
                comPrincipal.Text = "Insertion en cours...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Black;
                comPrincipal.apparaitre(0);// faire apparaitre le commentaire 
                algo.afficher(Alg);// faire apparaitre l'algorithme déroulant
                await algo.colorer(couleurAlgo, 0, Temps.time);
                if (this.triee) pos = tabInfo[1];
                Commentaire com = new Commentaire("Insertion à la tête de la liste", Brushes.Black, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, coordY - widthOfmaillon, 200, 30, Brushes.PaleTurquoise, Brushes.White);
                if (triee && pos != 0) com.Text = "Insertion à la position : " + pos;
                Point[] tabPoint = new Point[1];
                list.Add(new Maillon_bi());//ajouter le maillon dans la liste
                if (list.Count - 1 == 0)//si la liste est vide
                {
                    await algo.colorer(couleurAlgo, 1, Temps.time);
                    await algo.colorer(couleurAlgo, 2, Temps.time);
                }
                else if (pos == 0)//insértion à la première position
                {
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);

                }
                else if (pos == this.list.Count - 1)//insértion à la dernière position
                {
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                }
                else//insértion au milieu de la liste
                {
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);

                }
                for (int i = list.Count - 1; i > pos; i--)//décalage des maillon pour l'insértion
                {
                    tabPoint[0] = new Point(coordX + i * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY);
                    list[i - 1].deplacer(tabPoint, 1);
                    list[i - 1].Adr.decaler(tabPoint[0].X + heightOfmaillon - 5, tabPoint[0].X + heightOfmaillon + tailleFleche - 5);
                    list[i - 1].Prec.decaler(tabPoint[0].X + 5, tabPoint[0].X - tailleFleche + 5);
                    list[i] = list[i - 1];
                }
                if (list.Count - 1 == 0)//liste vide
                {
                    list[0] = new Maillon_bi(val, coordX + tailleFleche, coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 3, 3, tailleFleche);

                }
                else if (pos == 0)//insértion en début de liste
                {
                    list[pos] = new Maillon_bi(val, coordX + (pos) * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 3, 1, tailleFleche);
                    list[1].Prec.retirerCanvas(c);
                    list[1].Prec = new Fleche(list[0].Adr.CoordX + tailleFleche + 10, list[1].Prec.CoordY, list[1].Prec.Color, tailleFleche, 2, 2);

                }
                else if (pos == this.list.Count - 1)//insértion en fin de liste
                {
                    list[pos] = new Maillon_bi(val, coordX + (pos) * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 2, 3, tailleFleche);
                    list[pos - 1].Adr.retirerCanvas(c);
                    list[pos - 1].Adr = new Fleche(list[pos - 1].CoordX + heightOfmaillon - 5, list[pos - 1].Adr.CoordY, list[pos - 1].Adr.Color, tailleFleche, 1, 1);
                    await list[pos - 1].Adr.dessiner(Temps.time, c);
                }
                else//insértion au milieu
                {
                    list[pos] = new Maillon_bi(val, coordX + (pos) * (heightOfmaillon + tailleFleche) + tailleFleche, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 2, 1, 2, 1, tailleFleche);
                }
                list[pos].appear(c);//faire apparaitre le maillon à insérer
                if (list.Count - 1 != 0 && pos == 0) await list[1].Prec.dessiner(Temps.time, c);
                com.ajouterCanvas(c);
                com.apparaitre(0.5);//faire apparaitre le commentaire
                if (list.Count - 1 == 0)//liste vide
                {
                    await algo.colorer(couleurAlgo, 3, Temps.time);
                    await algo.colorer(couleurAlgo, 4, Temps.time);

                }
                else if (pos == 0)//insértion en début de liste
                {
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);

                }
                else if (pos == this.list.Count - 1)//insértion en fin de liste
                {
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
                }
                else//insértion au milieu de la liste
                {
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
                }
                com.disparaitre(2000);//faire disparaitre le commentaire
                await Task.Delay(2000);
                await algo.colorer(couleurAlgo, 11, Temps.time);
                com.enleverCanvas(c);//faire disparaitre le commentaire
            }
            else//si la valeur a été trouvée dans la liste 
            {
                comPrincipal.Text = "Insertion impossible.\nLa valeur existe déjà";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(0);// faire apparaitre le commentaire
                await Task.Delay(2000);
            }
            comPrincipal.disparaitre(1);//faire disparaitre le commentaire
            algo.disparaitre(Alg);//faire disparaitre l'algorithme déroulant
        }

        public void afficher(Canvas c)
        // c'est le Canvas ou on veut afficher notre tableau 
        {
            foreach (Maillon_bi maillon in list)
            {
                maillon.afficher(c);//afficher le maillon
            }
        }
        public async Task suppression(int valeur, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];     //Le tableau qui sera modifié par la fonction de recherche
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 100, this.coordY - 70, 250, 40, Brushes.BurlyWood, Brushes.White);
            com.ajouterCanvas(c);
            com.opacity = 0;
            Algo algo = new Algo(14, coordX_Algo, coordY_Algo);//instanciation de l'algorithme déroulant
            if (this.list.Count <= 0)//si la liste est vide
            {

                com.Text = "Suppression impossible, la liste est vide !";
                com.CouleurFond = Brushes.Red;
                com.Width = 250;
                com.Height = 50;
                com.CoordX = this.coordX ;
                com.CoordY = this.coordY - 50;
                com.apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                com.disparaitre(Temps.time);
            }
            else//si la liste n'est pas vide
            {
                com.Height = 40;
                com.Width = 250;
                com.CoordX = this.coordX ;
                com.CoordY = this.coordY - 70;
                com.Text = "Recherche de la valeur à supprimer ...";
                await recherche_seq(valeur, tabInfo, c, true, comPrincipal, Alg);//Il retourne un entier qui décrit l'echec (0) ou la réussite (1) de la recherche ainsi que la position
                int typefleche = -1;
                int typebout = -1;
                int typefleche1 = -1;
                int typebout1 = -1;
                int Tb;
                Fleche f2, f1, f3, f4, f5, f6, f7, f8;
                if (tabInfo[0] == 0)//si la valeurrecherchée n'a pas été trouvée
                {

                    await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                    com.Text = "Suppression impossible, la valeur n'existe pas dans la liste !";
                    com.CouleurFond = Brushes.Red;
                    com.Width = 340;
                    com.apparaitre(Temps.time);
                    await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                    com.disparaitre(Temps.time);
                }
                else
                if (tabInfo[0] == 1)//si la valeur recherchée a été trouvée
                {
                    algo.afficher(Alg);//faire apparaitre l'algorithme déroulant
                    com.Text = "Suppression en cours ...";
                    com.CouleurFond = Brushes.Yellow;
                    com.CouleurBordure = Brushes.Black;
                    com.apparaitre(Temps.time);
                    await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 1, Temps.time);
                    await algo.colorer(couleurAlgo, 2, Temps.time);
                    await algo.colorer(couleurAlgo, 3, Temps.time);
                    com.disparaitre(Temps.time);
                    Point[] tabl = new Point[3];
                    if ((tabInfo[1] == this.list.Count - 1) && (tabInfo[1] != 0)) //Supression d'un élément en fin de liste
                    {
                        await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                        tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //point ou va se deplacer le maillon
                        tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);//point ou va disparaitre le maillon
                        f1 = this.list[tabInfo[1]].Prec;
                        f2 = this.list[tabInfo[1] - 1].Adr;
                        f3 = this.list[tabInfo[1]].Adr;
                        f3.retirerCanvas(c);
                        com.Text = "On supprime le dernier maillon de la liste, son précedent devient queue de liste";
                        com.CouleurFond = Brushes.LightSkyBlue;
                        com.CouleurBordure = Brushes.Black;
                        com.Width += 200;
                        com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                        com.CoordY += this.coordY+30 ;
                        com.apparaitre(Temps.time);  //On adapte le commentaire au contexte
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        f1.retirerCanvas(c);
                        f2.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        typefleche = f3.typefleche;
                        typebout = 3;
                        f4 = new Fleche(f2.CoordX, f2.CoordY, f2.Color, tailleFleche, typefleche, typebout);
                        f4.dessiner(1,c);
                        await algo.colorer(couleurAlgo, 9, Temps.time);
                        await algo.colorer(couleurAlgo, 10, Temps.time);
                        this.list[tabInfo[1] - 1].Adr = f4;
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                        await algo.colorer(couleurAlgo, 11, Temps.time);
                        this.list.RemoveAt(this.list.Count - 1);
                        com.disparaitre(Temps.time);
                    }
                    else
                    {
                        if ((tabInfo[1] < this.list.Count - 1) && (tabInfo[1] > 0)) //Supression d'un élément au milieu de la liste
                        {
                            await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 25);
                            f1 = this.list[tabInfo[1] - 1].Adr;
                            f3 = this.list[tabInfo[1]].Prec;
                            Tb = this.list[tabInfo[1]].Adr.bout.TypeBout;
                            f2 = this.list[tabInfo[1]].Adr;
                            f4 = this.list[tabInfo[1] + 1].Prec;
                            f1.retirerCanvas(c);
                            f3.retirerCanvas(c);
                            com.Text = "Le précedent du maillon à supprimer pointe son suivant et son suivant pointe le précedent ";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black; com.Width += 250;
                            com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                            com.CoordY += this.coordY + 30;
                            com.apparaitre(Temps.time);//faire apparaitre le commentaire
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            f2.retirerCanvas(c);
                            f4.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            typefleche = f2.typefleche + 2;
                            typebout = f2.bout.TypeBout;
                            typefleche1 = f4.typefleche + 2;
                            typebout1 = f4.bout.TypeBout;
                            f5 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                            f6 = new Fleche(f4.CoordX, f4.CoordY, f4.Color, tailleFleche, typefleche1, typebout1);
                            f5.dessiner(1,c);
                            await algo.colorer(couleurAlgo, 9, Temps.time);
                            f6.dessiner(1,c);
                            await algo.colorer(couleurAlgo, 10, Temps.time);
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 11, Temps.time);
                            this.list.RemoveAt(tabInfo[1]);
                            com.disparaitre(Temps.time);//faire disparaitre le commentaire
                            f7 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, f1.typefleche, f1.bout.TypeBout);
                            f8 = new Fleche(f4.CoordX, f4.CoordY, f4.Color, tailleFleche, f4.typefleche, f4.bout.TypeBout);
                            f5.decalAr(f7, c);
                            f6.decalAr(f8, c);
                            this.list[tabInfo[1] - 1].Adr = f7;
                            this.list[tabInfo[1]].Prec = f8;
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time / 5));

                        }
                        else    //Supression d'un élément en debut de liste
                        {
                            await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f1 = this.list[tabInfo[1]].Prec;
                            f2 = this.list[tabInfo[1]].Adr;
                            f1.retirerCanvas(c);
                            com.Text = "On supprime le premier maillon de la liste, son suivant devient tête de liste";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black; com.Width += 180;
                            com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                            com.CoordY += this.coordY + 30;
                            com.apparaitre(Temps.time);//faire apparaitre le commentaire
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            f2.retirerCanvas(c);
                            if (list.Count > 1)//si la liste n'est pas vide
                            {
                                f3 = this.list[tabInfo[1] + 1].Prec;
                                f3.retirerCanvas(c);
                                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                                typefleche = f3.typefleche;
                                typebout = 3;
                                f4 = new Fleche(f3.CoordX, f3.CoordY, f3.Color, tailleFleche, typefleche, typebout);
                                f4.dessiner(1,c);
                                this.list[tabInfo[1] + 1].Prec = f4;
                            }
                            await algo.colorer(couleurAlgo, 5, Temps.time);
                            await algo.colorer(couleurAlgo, 6, Temps.time);
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 7, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(tabInfo[1]);
                            com.disparaitre(Temps.time);//faire disparaitre le commentaire

                        }
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time / 2));
                        Point[] tabPoint = new Point[1];
                        for (int i = tabInfo[1]; i < list.Count; i++)//Décalages des maillons
                        {
                            tabPoint[0] = new Point(this.coordX + ((i) * (heightOfmaillon + tailleFleche) + tailleFleche), this.coordY);
                            list[i].Prec.decaler(this.coordX + ((i) * (heightOfmaillon + tailleFleche)) + tailleFleche + 5, this.coordX + ((i) * (heightOfmaillon + tailleFleche)) + 5);
                            list[i].Adr.decaler(this.coordX + ((i) * (heightOfmaillon + tailleFleche)) + tailleFleche + heightOfmaillon - 5, this.coordX + ((i) * (heightOfmaillon + tailleFleche)) + tailleFleche + heightOfmaillon + tailleFleche - 5);
                            list[i].deplacer(tabPoint, 1);//déplacer le maillon
                        }
                        await algo.colorer(couleurAlgo, 12, 0.3 * Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        algo.disparaitre(Alg);//faire disparaitre l'algorithme déroulant
                    }
                }
            }
        }
    }
}
