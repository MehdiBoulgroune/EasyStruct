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
    class LLC
    {
        List<Maillon> list;
        private Boolean triee; //Boolean liste triée ou non triée
        private double coordX;// Coordonnées de la listes 
        private double coordY;
        /*************LES COULEURS**************/
        private SolidColorBrush couleurAlgo = Brushes.White;
        private SolidColorBrush couleurFondMaillon = Brushes.Gainsboro;
        private SolidColorBrush couleurFondCase = Brushes.White;
        private SolidColorBrush couleurBordureMaillon = Brushes.Black;
        /*********** LES CONSTANTES ***********/
        private const double widthOfmaillon = 50;
        private const double heightOfmaillon = 100;
        private const double tailleFleche = 40;
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;
        /***************************************/

        public LLC(Boolean triee, double coordX, double coordY)
        {
            this.triee = triee;
            this.coordX = coordX;
            this.coordY = coordY;
            list = new List<Maillon>();
        }
        public void chargementAleatoire(int nbValeur)
        /**Chargement de nbvaleur **/
        {
            Random rndNumber = new Random();
            Boolean stop = new Boolean();
            int[] tabValeur = new int[nbValeur];
            list = new List<Maillon>();
            Maillon maillon;
            int i;
            for (i = 0; i < nbValeur; i++)//Création d'un tableau avec nbvaleur aléatoire
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
            for (i = 0; i < nbValeur - 1; i++)//On crée nbvaleur -1 maillons de la liste 
            {
                maillon = new Maillon(tabValeur[i], this.coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 1, 1, tailleFleche);
                list.Add(maillon);//On ajoute le maillon à la liste 
            }
            maillon = new Maillon(tabValeur[nbValeur - 1], this.coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 1, 3, tailleFleche);
            list.Add(maillon);//On ajoute le dernier maillon qui est à nil à la liste
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
            comPrincipal.apparaitre(1);//Apparaitre le commentaire principale
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
            Fleche parcours = new Fleche(1);//Création de la fléche de parcourt 
            parcours.Height = tailleFleche;
            parcours.StrokeThick = 3;
            parcours.Color = couleurparcours;
            parcours.L.Opacity = 0.9;
            parcours.CoordY = this.coordY + widthOfmaillon / 2;
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
                        parcours.CoordX = list[i].Adr.CoordX;//On passe au coordonnée de la fléche du maillon suivant 
                        parcours.bout = new Bout(list[i].Adr.bout.coordX, list[i].Adr.bout.coordY, couleurparcours, list[i].Adr.bout.TypeBout, 2.5);
                        await parcours.dessiner(Temps.time,c);//On dessine la fléche du partcourt 
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        parcours.retirerCanvas(c);
                        com.disparaitre(0);
                        this.list[i].colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorCase(couleurFondMaillon, couleurBordureMaillon, 2);
                        await algo.colorer(couleurAlgo, 9, Temps.time);

                    }
                    else//Si on a trouver la valeur 
                    {
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        tabInfo[0] = 1;//Indiquer qu'on a trouvé la valeur 
                        comPrincipal.disparaitre(1);
                        comPrincipal.Text = "La valeur " + val + " a été trouvée";
                        comPrincipal.CouleurFond = couleurtrouve;
                        comPrincipal.CouleurBordure = couleurtrouve;
                        this.list[i].opacity = 0.7;
                        comPrincipal.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        if (insSup == false)//Si il n'y a pas d'insertion aprés 
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 5);
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
                while (i < this.list.Count && tabInfo[0] == 0 && list[i].Valeur <= val)//On s'arrete si on arrive à nil ou si on trouve une valeur supérieur à la valeur rechercher
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
                        this.list[i].colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                        this.list[i].colorCase(couleurFondMaillon, couleurBordureMaillon, 2);
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
                        if (insSup == false)
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 5);
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
            if (tabInfo[0] == 0)
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
            comPrincipal.disparaitre(2);
            await algo.colorer(couleurAlgo, 12, Temps.time);
            algo.disparaitre(Alg);
            foreach (Maillon maillon in list)
            {
                maillon.colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2);
                maillon.colorCase(couleurFondCase, couleurBordureMaillon, 2);
            }
        }
        public async Task recherche_pos(int pos, int[] tabInfo, Canvas c, Boolean insSup, Commentaire comPrincipal, Canvas Alg)
        {
            //recherche de la position pos 
            int i = 0;
            tabInfo[0] = 0;
            Algo algo = new Algo(9, coordX_Algo, coordY_Algo); //Création d'un algorithme de déroulement
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
            parcours.Height = tailleFleche + 10;
            parcours.StrokeThick = 5;
            parcours.Color = couleurparcours;
            parcours.L.Opacity = 0.9;
            parcours.CoordY = this.coordY + widthOfmaillon / 2;
            while (i < this.list.Count && tabInfo[0] == 0 && i <= pos) //Parcourt de la liste 
            {
                await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                this.list[i].opacity = 0.7;
                if (i != pos) //Si la position en cours est différent de la position recherchée
                {
                    com.CoordX = this.list[i].CoordX;
                    com.CoordY = this.list[i].CoordY - 35;
                    this.list[i].colorMaillon(couleurparcours, couleurBordureMaillon, 2);
                    this.list[i].colorCase(couleurparcours, couleurBordureMaillon, 2);
                    com.Text = "  " + i + " est différent de " + pos;
                    com.apparaitre(Temps.time);
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    parcours.CoordX = list[i].Adr.CoordX;//On passe au coordonnée de la fléche du maillon suivant 
                    parcours.bout = new Bout(list[i].Adr.bout.coordX, list[i].Adr.bout.coordY, couleurparcours, list[i].Adr.bout.TypeBout, 2.5);
                    await parcours.dessiner(Temps.time, c);
                    await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                    parcours.retirerCanvas(c);
                    com.disparaitre(0);
                    this.list[i].colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2); tabInfo[1] = tabInfo[1] + 1;
                    this.list[i].colorCase(couleurFondMaillon, couleurBordureMaillon, 2);
                }
                else //Si la position est égal à la position recherchée
                {
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                    tabInfo[0] = 1;
                    comPrincipal.disparaitre(1);
                    comPrincipal.Text = "La position " + pos + " a été trouvée";
                    comPrincipal.CouleurFond = couleurtrouve;
                    comPrincipal.CouleurBordure = couleurtrouve;
                    this.list[i].opacity = 0.7;
                    comPrincipal.apparaitre(Temps.time);
                    await algo.colorer(couleurAlgo, 6, Temps.time);

                    if (insSup == false) //Si il n'y a pas d'insertion/Suppression aprés la recherche
                    {
                        this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                        this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                        com.disparaitre(0);
                        this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 3);
                        await Task.Delay(TimeSpan.FromSeconds(4));


                    }
                    else//Si la recherche est suivie d'une insertion/Suppression 
                    {
                        this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                        this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                        com.disparaitre(0);
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                }
                i++;
                await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
            }
            await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
            if (i - 1 > pos || pos >= list.Count)//Si la position n'existe pas dans la liste
            {
                comPrincipal.disparaitre(2.5);
                comPrincipal.Width = 200;
                comPrincipal.Text = "La position " + pos + " n'existe pas ";
                comPrincipal.CouleurFond = couleurparcours;
                comPrincipal.CouleurBordure = couleurparcours;
                comPrincipal.apparaitre(Temps.time);
                com.enleverCanvas(c);
                tabInfo[1] = tabInfo[1] - 1;
            }
            comPrincipal.disparaitre(2);//disparition du commentaire
            await algo.colorer(couleurAlgo, 12, Temps.time);
            algo.disparaitre(Alg);//Disparition de l'algorithme
            foreach (Maillon maillon in list)//Remise de la couleur initial de la liste
            {
                maillon.colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2);
                maillon.colorCase(couleurFondCase, couleurBordureMaillon, 2);
            }
        }
        public async void afficher(Canvas c)
        // c'est le Canvas ou on veut afficher notre tableau 
        {
            foreach (Maillon maillon in list) await maillon.afficher(c);
        }

        public async Task suppression(int valeur, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];     //Le tableau qui sera modifié par la fonction de recherche
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 100, this.coordY - 70, 250, 40, Brushes.BurlyWood, Brushes.White);
            com.ajouterCanvas(c);
            com.opacity = 0;
            if (this.list.Count <= 0)
            {

                com.Text = "Suppression impossible, la liste est vide !";
                com.CouleurFond = Brushes.Red;
                com.Width = 250;
                com.Height = 50;
                com.CoordX = this.coordX - 50;
                com.CoordY = this.coordY - 50;
                com.apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                com.disparaitre(Temps.time);
            }
            else
            {
                com.Height = 40;
                com.Width = 250;
                com.CoordX = this.coordX ;
                com.CoordY = this.coordY - 70;
                com.Text = "Recherche de la valeur à supprimer ...";
                await recherche_seq(valeur, tabInfo, c, true, comPrincipal, Alg);//Il retourne un entier qui décrit l'echec (0) ou la réussite (1) de la recherche ainsi que la position

                int typefleche = -1;
                int typebout = -1;
                int Tb;
                Fleche f, f2, f1, f4;
                if (tabInfo[0] == 0)
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
                  if (tabInfo[0] == 1)
                {
                    Algo algo = new Algo(10, coordX_Algo, coordY_Algo);
                    com.Text = " Suppression en cours ...";
                    com.CouleurFond = Brushes.Yellow;
                    com.CouleurBordure = Brushes.Black;
                    com.apparaitre(Temps.time);
                    algo.afficher(Alg);
                    await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                    com.disparaitre(Temps.time);
                    await algo.colorer(couleurAlgo, 0, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 1, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 2, 0.3 * Temps.time);
                    Point[] tabl = new Point[3];
                    if ((tabInfo[1] == this.list.Count - 1) && (tabInfo[1] != 0)) //Supression d'un élément en fin de liste
                    {
                        await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                        tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //point ou va se deplacer le maillon
                        tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);//point ou va disparaitre le maillon
                        f1 = this.list[tabInfo[1] - 1].Adr;
                        f2 = this.list[tabInfo[1]].Adr;
                        f1.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        com.Text = "On supprime le dernier maillon de la liste, son précedent pointera NIL";
                        com.CouleurFond = Brushes.LightSkyBlue;
                        com.CouleurBordure = Brushes.Black;
                        com.Width += 180;
                        com.CoordX = this.coordX + (tabInfo[1] - 1) * (heightOfmaillon);
                        com.CoordY += this.coordY + 30;
                        com.apparaitre(Temps.time);  //On adapte le commentaire au contexte
                        f2.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        typefleche = f1.typefleche;
                        typebout = 3;
                        f4 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                        f4.dessiner(1,c);
                        await algo.colorer(couleurAlgo, 7, Temps.time);
                        this.list[tabInfo[1] - 1].Adr = f4;
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        this.list.RemoveAt(this.list.Count - 1);
                        com.disparaitre(Temps.time);
                    }
                    else
                    {
                        if ((tabInfo[1] < this.list.Count - 1) && (tabInfo[1] > 0)) //Supression d'un élément au milieu de la liste
                        {
                            await algo.colorer(couleurAlgo, 6, Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f1 = this.list[tabInfo[1] - 1].Adr;
                            Tb = this.list[tabInfo[1]].Adr.bout.TypeBout;
                            f2 = this.list[tabInfo[1]].Adr;
                            f1.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            com.Text = "Le précedent du maillon à supprimer pointe son suivant";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black;
                            com.Width += 50;
                            com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                            com.CoordY += this.coordY +30;
                            com.apparaitre(Temps.time);
                            f2.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                            typefleche = f2.typefleche + 2;
                            typebout = f2.bout.TypeBout;
                            f4 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                            f4.dessiner(1,c);
                            await algo.colorer(couleurAlgo, 7, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            com.disparaitre(Temps.time);
                            await algo.colorer(couleurAlgo, 8, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(tabInfo[1]);
                            f = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, f1.typefleche, f1.bout.TypeBout);
                            f4.decalAr(f, c);
                            this.list[tabInfo[1] - 1].Adr = f;
                        }
                        else    //Supression d'un élément en debut de liste
                        {
                            await algo.colorer(couleurAlgo, 3, Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f2 = this.list[tabInfo[1]].Adr;
                            f2.retirerCanvas(c);
                            com.Text = "On supprime le premier maillon de la liste, son suivant devient tête de liste";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black;
                            com.Width += 180;
                            com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                            com.CoordY += this.coordY + 30;
                            com.apparaitre(Temps.time);
                            await algo.colorer(couleurAlgo, 4, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 5, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(tabInfo[1]);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            com.disparaitre(Temps.time);

                        }
                        Point[] tabPoint = new Point[1];
                        for (int i = tabInfo[1]; i < list.Count; i++)      //Décalages
                        {
                            tabPoint[0] = new Point(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + 10, this.coordY);
                            list[i].Adr.decaler(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + 5, this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + tailleFleche + 5);
                            list[i].deplacer(tabPoint, 1);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        for (int i = tabInfo[1]; i < list.Count; i++)
                        {
                            list[i].Adr.CoordX = (this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + 5);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.3 * Temps.time);
                        algo.disparaitre(Alg);
                    }
                }
            }
        }

        public async Task suppression_pos(int pos, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];     //Le tableau qui sera modifié par la fonction de recherche
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 100, this.coordY - 70, 250, 40, Brushes.BurlyWood, Brushes.White);
            com.ajouterCanvas(c);
            com.opacity = 0;
            if (this.list.Count <= 0)
            {

                com.Text = "Suppression impossible, la liste est vide !";
                com.CouleurFond = Brushes.Red;
                com.Width = 250;
                com.Height = 50;
                com.CoordX = this.coordX - 50;
                com.CoordY = this.coordY - 50;
                com.apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                com.disparaitre(Temps.time);
            }
            else
            {
                com.Height = 40;
                com.Width = 250;
                com.CoordX = this.coordX - 50;
                com.CoordY = this.coordY - 70;
                com.Text = "Recherche de la position où supprimer ...";
                await recherche_pos(pos, tabInfo, c, true, comPrincipal, Alg);//Il retourne un entier qui décrit l'echec (0) ou la réussite (1) de la recherche ainsi que la position
                int typefleche = -1;
                int typebout = -1;
                int Tb;
                Fleche f, f2, f1, f4;
                if (tabInfo[0] == 0)
                {

                    await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                    com.Text = "Suppression impossible, la position n'existe pas dans la liste !";
                    com.CouleurFond = Brushes.Red;
                    com.Width = 340;
                    com.apparaitre(Temps.time);
                    await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                    com.disparaitre(Temps.time);
                }
                else
                  if (tabInfo[0] == 1)
                {
                    Algo algo = new Algo(10, coordX_Algo, coordY_Algo);
                    algo.afficher(Alg);
                    com.Text = " Suppression en cours ...";
                    com.CouleurFond = Brushes.Yellow;
                    com.CouleurBordure = Brushes.Black;
                    com.apparaitre(Temps.time);
                    await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                    com.disparaitre(Temps.time);
                    await algo.colorer(couleurAlgo, 0, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 1, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 2, 0.3 * Temps.time);
                    Point[] tabl = new Point[3];
                    if ((pos == this.list.Count - 1) && (pos != 0)) //Supression d'un élément en fin de liste
                    {
                        await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                        tabl[0] = new Point(this.list[pos].CoordX, this.list[pos].CoordY); //point ou va se deplacer le maillon
                        tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);//point ou va disparaitre le maillon
                        f1 = this.list[pos - 1].Adr;
                        f2 = this.list[pos].Adr;
                        f1.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        com.Text = "On supprime le dernier maillon de la liste,son précedent pointera NIL";
                        com.CouleurFond = Brushes.LightSkyBlue;
                        com.CouleurBordure = Brushes.Black;
                        com.Width += 150;
                        com.CoordX = this.coordX + (pos - 1) * (heightOfmaillon);
                        com.CoordY += this.coordY +30;
                        com.apparaitre(Temps.time);  //On adapte le commentaire au contexte
                        f2.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        typefleche = f1.typefleche;
                        typebout = 3;
                        f4 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                        f4.dessiner(1,c);
                        await algo.colorer(couleurAlgo, 7, Temps.time);
                        this.list[pos - 1].Adr = f4;
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        this.list[pos].disappear(Temps.time, tabl, 2);
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        this.list.RemoveAt(this.list.Count - 1);
                        com.disparaitre(Temps.time);
                    }
                    else
                    {
                        if ((pos < this.list.Count - 1) && (pos > 0)) //Supression d'un élément au milieu de la liste
                        {
                            await algo.colorer(couleurAlgo, 6, Temps.time);
                            tabl[0] = new Point(this.list[pos].CoordX, this.list[pos].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f1 = this.list[pos - 1].Adr;
                            Tb = this.list[pos].Adr.bout.TypeBout;
                            f2 = this.list[pos].Adr;
                            f1.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            com.Text = "Le précedent du maillon à supprimer pointe son suivant";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black;
                            com.Width += 100;
                            com.CoordX = this.coordX + pos * (heightOfmaillon);
                            com.CoordY += this.coordY +30;
                            com.apparaitre(Temps.time);
                            f2.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                            typefleche = f2.typefleche + 2;
                            typebout = f2.bout.TypeBout;
                            f4 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                            f4.dessiner(1, c);
                            await algo.colorer(couleurAlgo, 7, Temps.time);
                            this.list[pos].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 8, Temps.time);
                            com.disparaitre(Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(pos);
                            f = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, f1.typefleche, f1.bout.TypeBout);
                            f4.decalAr(f, c);
                            this.list[pos - 1].Adr = f;
                        }
                        else    //Supression d'un élément en debu de liste
                        {
                            await algo.colorer(couleurAlgo, 3, Temps.time);
                            tabl[0] = new Point(this.list[pos].CoordX, this.list[pos].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f2 = this.list[pos].Adr;
                            f2.retirerCanvas(c);
                            com.Text = "On supprime le premier maillon de la liste, son suivant devient tete de liste";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black; com.Width += 180;
                            com.CoordX = this.coordX + tabInfo[1] * (heightOfmaillon);
                            com.CoordY += this.coordY+30;
                            com.apparaitre(Temps.time);
                            await algo.colorer(couleurAlgo, 4, Temps.time);
                            this.list[pos].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 5, Temps.time);

                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(pos);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            com.disparaitre(Temps.time);

                        }
                        Point[] tabPoint = new Point[1];
                        for (int i = pos; i < list.Count; i++)      //Décalages
                        {
                            tabPoint[0] = new Point(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + 10, this.coordY);
                            list[i].Adr.decaler(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + 5, this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + tailleFleche + 5);
                            list[i].deplacer(tabPoint, 1);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        for (int i = pos; i < list.Count; i++)
                        {
                            list[i].Adr.CoordX = (this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + 5);
                        }
                        await algo.colorer(couleurAlgo, 9, 0.3 * Temps.time);
                        algo.disparaitre(Alg);
                    }
                }
            }
        }

        public async Task insert(int val, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];
            int pos = 0;
            Algo algo = new Algo(11, coordX_Algo, coordY_Algo);//Instantier l'algorithme d'insertion
            if (list.Count != 0)//Si la liste n'est pas vide
            {
                await recherche_seq(val, tabInfo, c, true, comPrincipal, Alg);//Lancement de la recherche
                await Task.Delay(2000);
            }
            if (tabInfo[0] == 0)//Si la valeur n'existe pas 
            {
                comPrincipal.Text = "Insertion en cours...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.White;
                comPrincipal.apparaitre(0);
                algo.afficher(Alg);//Affichage de l'algorithme déroulant
                await algo.colorer(couleurAlgo, 0, Temps.time);

                if (this.triee) pos = tabInfo[1]; //Si la liste est triée 
                Commentaire com = new Commentaire("Insertion au début de la liste", Brushes.Black, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, coordY - widthOfmaillon, 200, 30, Brushes.LightSkyBlue, Brushes.White);
                if (triee && pos != 0) com.Text = "Insertion à la position : " + pos;
                Point[] tabPoint = new Point[1];
                list.Add(new Maillon());//Ajouter un nouveau maillon
                /****Déroulement de l'algorithme ******/
                if ((pos == this.list.Count - 1) && (pos != 0))
                {
                    await algo.colorer(couleurAlgo, 4, Temps.time);
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                }
                else if (list.Count - 1 == 0)
                {
                    await algo.colorer(couleurAlgo, 1, Temps.time);
                }
                else {
                    await algo.colorer(couleurAlgo, 4, Temps.time);
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                }
                for (int i = list.Count - 1; i > pos; i--)      //Décalages des maillons
                {
                    tabPoint[0] = new Point(coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY);
                    list[i - 1].deplacer(tabPoint, 1);
                    list[i - 1].Adr.decaler(coordX + (i + 1) * (heightOfmaillon + tailleFleche + 5) - tailleFleche - 5, coordX + (i + 1) * (heightOfmaillon + tailleFleche + 5));
                    list[i] = list[i - 1];
                }
                if ((pos == this.list.Count - 1) && (pos != 0)) //Si on insere en début de liste 
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 3, tailleFleche);
                    list[pos - 1].Adr.retirerCanvas(c);
                    list[pos - 1].Adr = new Fleche(list[pos - 1].CoordX + heightOfmaillon - 5, list[pos - 1].Adr.CoordY, list[pos - 1].Adr.Color, tailleFleche, 1, 1);
                    await list[pos - 1].Adr.dessiner(Temps.time, c);
                }
                else if (list.Count - 1 == 0)//Si la liste est vide 
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 3, tailleFleche);
                }
                else { //Si on insére au milieu
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 1, tailleFleche);
                }
                list[pos].appear(c);//Faire apparaitre le maillon
                com.ajouterCanvas(c);//Faire apparaitre un commentaire
                com.apparaitre(0.5);
                if ((pos == this.list.Count - 1) && (pos != 0)) //Si on insére à la fin
                {
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    await algo.colorer(couleurAlgo, 7, Temps.time);
                }
                else if (list.Count - 1 == 0)//Si la liste est vide 
                {
                    await algo.colorer(couleurAlgo, 2, Temps.time);
                    await algo.colorer(couleurAlgo, 3, Temps.time);

                }
                else {//Si on insére au milieu
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    await algo.colorer(couleurAlgo, 7, Temps.time);
                }
                com.disparaitre(2000);//Faire disparaitre le commentaire
                await Task.Delay(2000);
                await algo.colorer(couleurAlgo, 8, Temps.time);
                com.enleverCanvas(c);//enlever le commentaire 
            }
            else//Si la valeur n'existe pas dans la liste
            {
                comPrincipal.Text = "Insertion impossible.\nLa valeur existe déjà";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(0);
                await Task.Delay(2000);
            }
            comPrincipal.disparaitre(1);//faire disparaitre le commentaire
            algo.disparaitre(Alg);//faire disparaitre l'algorithme déroulant 
        }

        public async Task insertPos(int pos, int val, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];
            Algo algo = new Algo(11, coordX_Algo, coordY_Algo);//Instansiation de l'algorithme déroulant 
            if (list.Count != 0)//Si la liste n'est pas vide 
            {
                await recherche_pos(pos, tabInfo, c, true, comPrincipal, Alg);//lancement de la recherche
                await Task.Delay(2000);
            }
            if (tabInfo[0] == 1)//Si la valeur n'existe
            {
                comPrincipal.Text = "Insertion en cours...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Black;
                comPrincipal.apparaitre(0);
                algo.afficher(Alg);//Affichage de l'algorithme déroulant 
                await algo.colorer(couleurAlgo, 0, Temps.time);

                if (this.triee) pos = tabInfo[1];//Si la liste est triée 
                Commentaire com = new Commentaire("Insertion à la position : " + pos, Brushes.Black, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, coordY - widthOfmaillon, 200, 30, Brushes.PaleTurquoise, Brushes.White);
                Point[] tabPoint = new Point[1];
                list.Add(new Maillon());//ajout d'un nouveau maillon
                /******Déroulement des algorithmes***********/
                if ((pos == this.list.Count - 1) && (pos != 0))
                {
                    await algo.colorer(couleurAlgo, 4, Temps.time);
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                }
                else if (list.Count - 1 == 0)
                {
                    await algo.colorer(couleurAlgo, 1, Temps.time);
                }
                else {
                    await algo.colorer(couleurAlgo, 4, Temps.time);
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                }
                for (int i = list.Count - 1; i > pos; i--)      //Décalages des maillons 
                {
                    tabPoint[0] = new Point(coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY);
                    list[i - 1].deplacer(tabPoint, 1);
                    list[i - 1].Adr.decaler(coordX + (i + 1) * (heightOfmaillon + tailleFleche + 5) - tailleFleche - 5, coordX + (i + 1) * (heightOfmaillon + tailleFleche + 5));
                    list[i] = list[i - 1];
                }
                if ((pos == this.list.Count - 1) && (pos != 0))//Si on insére en fin de liste
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 3, tailleFleche);
                    list[pos - 1].Adr.retirerCanvas(c);
                    list[pos - 1].Adr = new Fleche(list[pos - 1].CoordX + heightOfmaillon - 5, list[pos - 1].Adr.CoordY, list[pos - 1].Adr.Color, tailleFleche, 1, 1);
                    await list[pos - 1].Adr.dessiner(Temps.time, c);
                }
                else if (list.Count - 1 == 0)//Si la liste est vide
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 3, tailleFleche);

                }
                else {//Si on insére au milieu
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 1, tailleFleche);
                }
                list[pos].appear(c);//Faire apparaitre le maillon
                com.ajouterCanvas(c);//Ajout d'un commentaire
                com.apparaitre(0.5);
                if ((pos == this.list.Count - 1) && (pos != 0))//Si la position est la fin de la liste
                {
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    await algo.colorer(couleurAlgo, 7, Temps.time);
                }
                else if (list.Count - 1 == 0)//si la liste est vide
                {
                    await algo.colorer(couleurAlgo, 2, Temps.time);
                    await algo.colorer(couleurAlgo, 3, Temps.time);

                }
                else {
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    await algo.colorer(couleurAlgo, 7, Temps.time);
                }
                com.disparaitre(2000);
                await Task.Delay(2000);
                await algo.colorer(couleurAlgo, 8, Temps.time);
                com.enleverCanvas(c);//enlever le commentaire 
            }
            else
            {
                comPrincipal.Text = "Insertion impossible.\nLa position n'existe pas";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(0);
                await Task.Delay(2000);
            }
            comPrincipal.disparaitre(1);//faire disparaitre le commentaire 
            algo.disparaitre(Alg);//enlever l'algorithme déroulant
        }

    }
}
