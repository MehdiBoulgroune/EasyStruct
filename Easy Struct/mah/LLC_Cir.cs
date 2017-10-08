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
    class LLC_Cir
    {
        List<Maillon> list;
        private Boolean triee;//si la liste est triée ou non
        private double coordX;//coordonnées de la liste
        private double coordY;

        private SolidColorBrush couleurAlgo = Brushes.White;
        private SolidColorBrush couleurFondMaillon = Brushes.Gainsboro;
        private SolidColorBrush couleurFondCase = Brushes.White;
        private SolidColorBrush couleurBordureMaillon = Brushes.Black;
        /*********** LES CONSTANTES ***********/
        private const double widthOfmaillon = 50;
        private const double heightOfmaillon = 100;
        private const int nbmaillonmax = 15;
        private const double tailleFleche = 40;
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;
        /***************************************/
        public LLC_Cir(Boolean triee, double coordX, double coordY)
        {
            this.triee = triee;
            this.coordX = coordX;
            this.coordY = coordY;
            list = new List<Maillon>();//instanciation de la liste
        }
        public void chargementAleatoire(int nbValeur)
        {
            //Crée une liste circulaire de nbValeur valeurs
            Random rndNumber = new Random();
            Boolean stop = new Boolean();
            int[] tabValeur = new int[nbValeur];
            list = new List<Maillon>();
            Maillon maillon;
            int i;
            for (i = 0; i < nbValeur; i++)//Génération de nombres aléatoire
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
            for (i = 0; i < nbValeur - 1; i++)//On crée les maillons de la liste
            {
                maillon = new Maillon(tabValeur[i], this.coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 1, 1, tailleFleche);
                list.Add(maillon);//ajouter le maillon dans la liste
            }
            maillon = new Maillon(tabValeur[nbValeur - 1], this.coordX + i * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, 50, 100, couleurFondMaillon, couleurBordureMaillon, 2, 5, 1, i * (heightOfmaillon + tailleFleche + 5) + heightOfmaillon + 5);
            list.Add(maillon);//ajouter le maillon dans la liste
        }
        public async Task recherche_seq(int val, int[] tabInfo, Canvas c, Boolean insSup, Commentaire comPrincipal, Canvas Algo)
        {
            //recherche de la valeur val et returne indice qui se trouve dans tabInfo
            int i = 0;
            tabInfo[0] = 0;
            Algo algo = new Algo(15, coordX_Algo, coordY_Algo); //Création d'un algorithme de déroulement
            SolidColorBrush couleurparcours = Brushes.Red;
            SolidColorBrush couleurtrouve = Brushes.Green;
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 5, this.coordY - 35, 50, 50, couleurparcours, couleurparcours);
            comPrincipal.CouleurFond = Brushes.Yellow;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(1);//Apparaitre le commentaire principale
            comPrincipal.Text = "Recherche en cours ...";
            algo.afficher(Algo); //Affichage de l'algorithme
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
                while (i < this.list.Count && tabInfo[0] == 0)//Si on est pas arriver à la fin de la liste
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
                        if (i < list.Count - 1) await parcours.dessiner(Temps.time,c);//On dessine la fléche du partcourt 
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
                        com.apparaitre(Temps.time);//faire apparaitre le commentaire
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        parcours.CoordX = list[i].Adr.CoordX;
                        parcours.bout = new Bout(list[i].Adr.bout.coordX, list[i].Adr.bout.coordY, couleurparcours, list[i].Adr.bout.TypeBout, 2.5);
                        if (i < list.Count - 1) await parcours.dessiner(Temps.time,c);
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        com.disparaitre(0);//faire disparaitre le commentaire
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
                        if (insSup == false)//si la recherche est suivie d'une insértion/suppression
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);//faire disparaitre le commentaire
                            this.list[i].clignoter(couleurtrouve, couleurtrouve, 2, 4);
                            await Task.Delay(TimeSpan.FromSeconds(4));
                        }
                        else
                        {
                            this.list[i].colorMaillon(couleurtrouve, couleurtrouve, 2);
                            this.list[i].colorCase(couleurtrouve, couleurtrouve, 2);
                            com.disparaitre(0);//faire disparaitre le commentaire
                        }
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                    }
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
                    i++;
                }
                await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
            }
            if (tabInfo[0] == 0)//Si on a pas trouvé la valeur
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
            algo.disparaitre(Algo);//faire disparaitre l'algorithme déroulant
            foreach (Maillon maillon in list)//Remettre les maillons de la liste en leurs couleurs d'origine
            {
                maillon.colorMaillon(couleurFondMaillon, couleurBordureMaillon, 2);
                maillon.colorCase(couleurFondCase, couleurBordureMaillon, 2);
            }
        }
        public async Task suppression(int valeur, Canvas c, Commentaire comPrincipal, Canvas Algo)
        {
            int[] tabInfo = new int[2];     //Le tableau qui sera modifié par la fonction de recherche
            Commentaire com = new Commentaire(" ", Brushes.Black, this.coordX - 100, this.coordY - 70, 250, 40, Brushes.BurlyWood, Brushes.White);
            com.ajouterCanvas(c);
            com.opacity = 0;
            Algo algo = new Algo(16, coordX_Algo, coordY_Algo);//instanciation de l'algorithme déroulant
            if (this.list.Count <= 0)//si la liste est vide
            {

                com.Text = "Suppression impossible, la liste est vide !";
                com.CouleurFond = Brushes.Red;
                com.Width = 250;
                com.Height = 50;
                com.CoordX = this.coordX ;
                com.CoordY = this.coordY - 50;
                com.apparaitre(Temps.time);//faire apparaitre le commentaire
                await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                com.disparaitre(Temps.time);//faire disparaitre le commentaire
            }
            else
            {
                com.Height = 40;
                com.Width = 250;
                com.CoordX = this.coordX ;
                com.CoordY = this.coordY - 70;
                if (list.Count != 0)//si la liste n'est pas vide
                {
                    await recherche_seq(valeur, tabInfo, c, true, comPrincipal, Algo);//Il retourne un entier qui décrit l'echec (0) ou la réussite (1) de la recherche ainsi que la position
                }
                int typefleche = -1;
                int typebout = -1;
                int Tb;
                Fleche f, f2, f1, f3, f4, f5;
                if (tabInfo[0] == 0)//si la valeur n'a pas été trouvée
                {

                    await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                    com.Text = "Suppression impossible, la valeur n'existe pas dans la liste !";
                    com.CouleurFond = Brushes.Red;
                    com.Width = 340;
                    com.apparaitre(Temps.time);//faire apparaitre le commentaire
                    await Task.Delay(TimeSpan.FromSeconds(5 * Temps.time));
                    com.disparaitre(Temps.time);//faire disparaitre le commentaire
                }
                else
               if (tabInfo[0] == 1)// si la valeur a été trouvée
                {
                    comPrincipal.Text = "Suppression en cours ...";
                    comPrincipal.CouleurBordure = Brushes.Black;
                    comPrincipal.CouleurFond = Brushes.Yellow;
                    comPrincipal.apparaitre(Temps.time);
                    algo.afficher(Algo);//faire apparaitre l'algorithme déroulant
                    await algo.colorer(couleurAlgo, 0, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 1, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 2, 0.3 * Temps.time);
                    await algo.colorer(couleurAlgo, 3, 0.3 * Temps.time);
                    Point[] tabl = new Point[3];
                    if ((tabInfo[1] == this.list.Count - 1) && (tabInfo[1] > 0)) //Supression d'un élément en fin de liste
                    {
                        await algo.colorer(couleurAlgo, 8, Temps.time);
                        tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //point ou va se deplacer le maillon
                        tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);//point ou va disparaitre le maillon
                        f1 = this.list[tabInfo[1] - 1].Adr;
                        f2 = this.list[tabInfo[1]].Adr;
                        f1.retirerCanvas(c);
                        com.Text = "Le précedent du maillon à supprimer pointe son suivant ";
                        com.CouleurFond = Brushes.LightSkyBlue;
                        com.CouleurBordure = Brushes.Black;
                        com.Width += 80;
                        com.CoordX = this.coordX + (tabInfo[1]) * (heightOfmaillon);
                        com.CoordY += this.coordY + 30;
                        com.apparaitre(Temps.time);  //On adapte le commentaire au contexte
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        f2.retirerCanvas(c);
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        typefleche = 5;
                        typebout = 1;
                        f4 = new Fleche(f1.CoordX, f1.CoordY, f2.Color, ((list.Count - 2) * (heightOfmaillon + tailleFleche + 5) + heightOfmaillon + 5), typefleche, typebout);
                        f4.dessiner(1,c);
                        await algo.colorer(couleurAlgo, 9, Temps.time);
                        this.list[tabInfo[1] - 1].Adr = f4;
                        await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                        this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                        await algo.colorer(couleurAlgo, 10, Temps.time);
                        this.list.RemoveAt(this.list.Count - 1);
                        com.disparaitre(Temps.time);//faire disparaitre le commentaire
                    }
                    else
                    {
                        if ((tabInfo[1] < this.list.Count - 1) && (tabInfo[1] > 0)) //Supression d'un élément au milieu de la liste
                        {
                            await algo.colorer(couleurAlgo, 8, Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f1 = this.list[tabInfo[1] - 1].Adr;
                            Tb = this.list[tabInfo[1]].Adr.bout.TypeBout;
                            f2 = this.list[tabInfo[1]].Adr;
                            f1.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            com.Text = "Le précedent du maillon à supprimer pointe son suivant";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black; com.Width += 80;
                            com.CoordX = this.coordX + tabInfo[1] * (heightOfmaillon);
                            com.CoordY += this.coordY +30;
                            com.apparaitre(Temps.time);//faire apparaitre le commentaire
                            f2.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                            typefleche = f2.typefleche + 2;
                            typebout = f2.bout.TypeBout;
                            f4 = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, typefleche, typebout);
                            f4.dessiner(1,c);
                            await algo.colorer(couleurAlgo, 9, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 10, Temps.time);
                            com.disparaitre(Temps.time);//faire disparaitre le commentaire
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(tabInfo[1]);
                            f = new Fleche(f1.CoordX, f1.CoordY, f1.Color, tailleFleche, f1.typefleche, f1.bout.TypeBout);
                            f4.decalAr(f, c);
                            this.list[tabInfo[1] - 1].Adr = f;
                        }
                        else    //Supression d'un élément en debut de liste
                        {
                            await algo.colorer(couleurAlgo, 4, Temps.time);
                            tabl[0] = new Point(this.list[tabInfo[1]].CoordX, this.list[tabInfo[1]].CoordY); //On fait disparaître la case à supprimer
                            tabl[1] = new Point(tabl[0].X, tabl[0].Y + 50);
                            f1 = this.list[list.Count - 1].Adr;
                            f2 = this.list[tabInfo[1]].Adr;
                            f1.retirerCanvas(c);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            f2.retirerCanvas(c);
                            com.Text = "Le précedent du maillon à supprimer pointe son suivant";
                            com.CouleurFond = Brushes.LightSkyBlue;
                            com.CouleurBordure = Brushes.Black;
                            com.Width += 80;
                            com.CoordX = this.coordX + tabInfo[1] * (heightOfmaillon);
                            com.CoordY += this.coordY + 30;
                            com.apparaitre(Temps.time);//faire apparaitre le commentaire
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list[tabInfo[1]].disappear(Temps.time, tabl, 2);
                            await algo.colorer(couleurAlgo, 5, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(2 * Temps.time));
                            this.list.RemoveAt(tabInfo[1]);


                        }
                        Point[] tabPoint = new Point[1];
                        for (int i = tabInfo[1]; i < list.Count; i++)      //Décalages
                        {
                            tabPoint[0] = new Point((i) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY);
                            tabPoint[0] = new Point(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)), this.coordY);
                            list[i].Adr.decaler(this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon - 5, this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon + tailleFleche - 5);// (i + 1) * (heightOfmaillon + tailleFleche + 5), time);
                            list[i].deplacer(tabPoint, 1);//deplacer le maillon
                        }
                        if (list.Count >= 1)//si la liste n'est pas vide
                        {
                            f3 = list[list.Count - 1].Adr;
                            f3.retirerCanvas(c);

                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            f5 = new Fleche(this.coordX + ((list.Count - 1) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon - 5, f3.CoordY, f3.Color, (list.Count - 1) * (heightOfmaillon + tailleFleche + 5) + heightOfmaillon + 5, 5, 1);
                            f5.dessiner(1,c);
                            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                            await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                            list[list.Count - 1].Adr = f5;

                        }
                        await algo.colorer(couleurAlgo, 11, 0.3 * Temps.time);
                        if (tabInfo[1] == 0)
                            com.disparaitre(Temps.time);//faire disparaitre le commentaire
                        for (int i = tabInfo[1]; i < list.Count - 1; i++)
                        {
                            list[i].Adr.CoordX = (this.coordX + ((i) * (heightOfmaillon + tailleFleche + 5)) + heightOfmaillon - 5);
                        }
                        comPrincipal.disparaitre(Temps.time);//faire disparaitre le commentaire
                    }
                    algo.disparaitre(Algo);//faire disparaitre l'algorithme déroulant
                }
            }
        }
        public async Task insert(int val, Canvas c, Commentaire comPrincipal, Canvas Algo)
        {
            //Insertion de la valeur val dans la liste 
            int[] tabInfo = new int[2];
            int pos = 0;
            Algo algo = new Algo(11, coordX_Algo, coordY_Algo);
            if (list.Count != 0)//Recherche si la valeur existe dans la liste
            {
                await recherche_seq(val, tabInfo, c, true, comPrincipal, Algo);
                await Task.Delay(2000);
            }
            if (tabInfo[0] == 0)//Si la valeur n'existe pas
            {
                comPrincipal.Text = "Insertion en cours...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Black;
                comPrincipal.apparaitre(0);//faire apparaitre le commentaire
                algo.afficher(Algo);//faire apparaitre l'algorithme déroulant
                await algo.colorer(couleurAlgo, 0, Temps.time);

                if (this.triee) pos = tabInfo[1];
                Commentaire com = new Commentaire("Insertion au début de la liste", Brushes.Black, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, coordY - widthOfmaillon, 200, 30, Brushes.PaleTurquoise, Brushes.White);
                if (triee && pos != 0) com.Text = "Insertion à la position : " + pos;
                Point[] tabPoint = new Point[1];
                list.Add(new Maillon());//ajouter le maillon dans la liste
                /******Déroulement de l'algorithme******/
                if ((pos == this.list.Count - 1) && (pos != 0))//insértion en fin de liste
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                }
                else if (list.Count - 1 == 0)//insértion en début de liste
                {
                    await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);
                }
                else//insértion au milieu de la liste
                {
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                }
                for (int i = list.Count - 2; i >= pos; i--)      //Décalages des maillons pour l'insertion
                {
                    tabPoint[0] = new Point(coordX + (i + 1) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY);
                    list[i].deplacer(tabPoint, 1);
                    if (i != list.Count - 2)
                    {
                        list[i].Adr.decaler(coordX + (i + 2) * (heightOfmaillon + tailleFleche + 5) - tailleFleche + 5, coordX + (i + 2) * (heightOfmaillon + tailleFleche + 5));
                    }
                    else
                    {
                        list[i].Adr.retirerCanvas(c);
                        list[i].Adr = new Fleche(coordX + (i + 2) * (heightOfmaillon + tailleFleche + 5) - tailleFleche, list[i].Adr.CoordY, list[i].Adr.Color, (list.Count - 1) * (heightOfmaillon + tailleFleche + 5) + heightOfmaillon + 5, 5, 1);
                        list[i].Adr.dessiner(Temps.time,c);
                    }
                    list[i + 1] = list[i];
                }
                if ((pos == this.list.Count - 1) && (pos != 0))//Si le maillon a inséré est à la fin de la liste
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 5, 1, (list.Count - 1) * (heightOfmaillon + tailleFleche + 5) + heightOfmaillon + 5);
                    list[pos - 1].Adr.retirerCanvas(c);
                    list[pos - 1].Adr = new Fleche(list[pos - 1].CoordX + heightOfmaillon - 5, list[pos - 1].Adr.CoordY, list[pos - 1].Adr.Color, tailleFleche, 1, 1);
                    list[pos - 1].Adr.dessiner(Temps.time,c);
                }
                else if (list.Count - 1 == 0)//Si la liste est vide
                {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 5, 1,heightOfmaillon + 5);

                }
                else {
                    list[pos] = new Maillon(val, coordX + (pos) * (heightOfmaillon + tailleFleche + 5) + 10, this.coordY, widthOfmaillon, heightOfmaillon, couleurFondMaillon, couleurBordureMaillon, 2, 1, 1, tailleFleche);

                }
                list[pos].appear(c);//faire apparaitre le maillon à insérer
                com.ajouterCanvas(c);//ajouter le commentaire
                if ((pos == this.list.Count - 1) && (pos != 0))//insértion en fin de liste
                {
                    await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                }
                else if (list.Count - 1 == 0)//insértion en début de liste
                {
                    await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);

                }
                else//insértion au milieu de la liste
                {
                    await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);

                }
                com.apparaitre(0.5);// faire apparaitre le commentaire
                com.disparaitre(2000);//faire disparaitre le commentaire
                await Task.Delay(2000);
                await algo.colorer(couleurAlgo, 8, Temps.time);
                com.enleverCanvas(c);
            }
            else//si la valeur a été trouvée dans la liste
            {
                comPrincipal.Text = "Insertion impossible.\nLa valeur existe déjà";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(0);//faire apparaitre le commentaire
                await Task.Delay(2000);
            }
            comPrincipal.disparaitre(1);//faire disparaitre le commentaire
            algo.disparaitre(Algo);//faire disparaitre l'algorithme déroulant
        }
        public void afficher(Canvas c)
        // c'est le Canvas ou on veut afficher notre tableau 
        {
            foreach (Maillon maillon in list)
            {
                maillon.afficher(c);//afficher le maillon
            }
        }
    }
}
