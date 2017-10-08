using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mah
{
    class Tableau
    {
        private Case[] tab;
        private Case[] tabIndices;
        private int tailleTab;
        private Boolean triee;
        private double coordX;
        private double coordY;
        // Les attributs coordX et coordY sont intialisées à l'instance et sont utilisées que à l'interieur de la classe 
        //donc ils ont pas de getters ni de setters  

        private SolidColorBrush couleurAlgo = Brushes.White;
        private SolidColorBrush couleurFondCase = Brushes.White;
        private SolidColorBrush couleurBordureCase = Brushes.Black;
        private SolidColorBrush couleurComPrincipal = Brushes.Yellow;
        // Les attributs couleurFondCase et couleurBordureCase sont utilisés 
        //qu'à l'interieur de la classe donc pas de getters ni de setters

        /************************** LES CONSTANTES ***********************************/
        private const int tailleMaxTab = 20;
        private const double widthOfcase = 50;
        private const double heightOfcase = 50;
        private const double a = widthOfcase / 2 - 10, b = 55;
        // a et b sont utilisé pour positionner les indices sous les cases du tableau 
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;
        /****************************************************************************/

        public Tableau(Boolean triee, double coordX, double coordY)
        {
            int i;
            this.tab = new Case[tailleMaxTab];
            this.triee = triee;
            this.tailleTab = 0;
            this.coordX = coordX;
            this.coordY = coordY;
            tabIndices = new Case[tailleMaxTab];
            for (i = 0; i < tailleMaxTab; i++)
            {
                tabIndices[i] = new Case(i + 1, coordX + widthOfcase * i + a, coordY + b, 2, 25, 25, Brushes.Transparent, Brushes.Transparent, 1);
            }
        }

        public Case[] Tab
        {
            get { return tab; }
            set { tab = value; }
        }
        public int TailleTab
        {
            get { return tailleTab; }
            set { tailleTab = value; }
        }

        public void afficher(Canvas c)
        // c est le Canvas ou on veut afficher notre tableau 
        {
            int i;
            for (i = 0; i < this.tailleTab; i++)
            {

                this.tab[i].afficher(c);
                this.tabIndices[i].afficher(c);

            }
        }

        public void chargementAleatoire(int nbValeur)
        // Effectue un chargement aléatoire du tableau 'tab' 
        {
            Random rndNumber = new Random();
            Boolean stop = new Boolean();
            int[] tabValeur = new int[nbValeur];
            int i;
            for (i = 0; i < nbValeur; i++)
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
            this.tailleTab = nbValeur;
            for (i = 0; i < nbValeur; i++)
            {
                this.tab[i] = new Case(tabValeur[i], coordX + i * widthOfcase, coordY, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
            }
        }


        public async Task recherche(int val, int[] tabInfo, Canvas c, Boolean inserSupp, Commentaire comPrincipal, Canvas Alg)
        // Rechereche la valeur 'val' dans le tableau tab(attribut) qu'il soit trié ou non. 
        // tabInfo[0] correspond au boolean trouv et tabInfo[1] correspond à l'indice de la valeur val si elle est trouvée sinon l'indice ou devrait être insérée cette valeur .
        // inserSup est un boolean qui indique si il ya une insertion ou suppression aprés cette recherche  
        {
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.Width = 200;
            comPrincipal.Height = 50;
            comPrincipal.apparaitre(0);
            if (this.triee) // Si le tableau est trié on utilise la recherche dichotomique .
            {
                comPrincipal.Text = "Recherche dichotomique en cours...";
                await rechercheDicho(val, tabInfo, c, inserSupp, comPrincipal, Alg);
            }
            else // sinon ( le tableau n'est pas trié) on utilise la recherche squentiel.
            {
                comPrincipal.Text = "Recherche sequentiel en cours...";
                await rechercheSeq(val, tabInfo, c, inserSupp, comPrincipal, Alg);
            }


        }
        private async Task rechercheDicho(int val, int[] tabInfo, Canvas c, Boolean inserSupp, Commentaire comPrincipal, Canvas Alg)
        //Effectue une recherche dichotomique dans le tableau tab 
        // tabInfo[0] correspond au boolean trouv et tabInfo[1] correspond à l'indice de la valeur val si il est trouvée sinon l'indice ou devrait être insérée cette valeur .
        // inserSup est un boolean qui indique si il ya une insertion ou suppression aprés cette recherche  
        {
            SolidColorBrush couleurBorneSup = Brushes.Red;
            SolidColorBrush couleurBorneInf = Brushes.Blue;
            SolidColorBrush couleurDeCaseMed = Brushes.SeaGreen;
            SolidColorBrush couleurTrouv = Brushes.Green;
            Algo algo = new Algo(2, coordX_Algo, coordY_Algo);
            algo.afficher(Alg);
            int bSup = this.tailleTab - 1, bInf = 0, med = 0, i, e = -5, d = -55;
            Point[] tabDepBSup = new Point[1], tabDepBInf = new Point[1], tabDepMedCase = new Point[1], tabDepBSupInd = new Point[1], tabDepBInfInd = new Point[1], tabDepMedCaseInd = new Point[1];
            await algo.colorer(couleurAlgo, 0, Temps.time);
            await algo.colorer(couleurAlgo, 1, Temps.time);
            await algo.colorer(couleurAlgo, 2, Temps.time);
            tabDepBSup[0] = new Point();
            tabDepBSup[0].Y = this.coordY;
            tabDepBInf[0] = new Point();
            tabDepBInf[0].Y = this.coordY;
            tabDepMedCase[0] = new Point();
            tabDepMedCase[0].Y = this.coordY;

            tabDepBSupInd[0] = new Point();
            tabDepBSupInd[0].Y = this.coordY + b;
            tabDepBInfInd[0] = new Point();
            tabDepBInfInd[0].Y = this.coordY + b;
            tabDepMedCaseInd[0] = new Point();
            tabDepMedCaseInd[0].Y = this.coordY + b;
            Boolean firstIteration = true;
            Champ borneSup = new Champ(this.coordX + widthOfcase * bSup, this.coordY, 1, heightOfcase, widthOfcase, Brushes.Transparent, couleurBorneSup, 6);
            Champ borneInf = new Champ(this.coordX + widthOfcase * bInf, this.coordY, 1, heightOfcase, widthOfcase, Brushes.Transparent, couleurBorneInf, 6);
            Champ medCase = new Champ();
            Champ borneSupInd = new Champ(this.coordX + widthOfcase * bSup + a, this.coordY + b, 2, 25, 25, couleurBorneSup, couleurBorneSup, 1);
            Champ borneInfInd = new Champ(this.coordX + widthOfcase * bInf + a, this.coordY + b, 2, 25, 25, couleurBorneInf, couleurBorneInf, 1);
            Champ medCaseInd = new Champ();
            borneSup.Forme.Opacity = 0.7;
            borneInf.Forme.Opacity = 0.7;
            borneSupInd.Forme.Opacity = 0.5;
            borneInfInd.Forme.Opacity = 0.5;
            borneSup.afficher(c);
            borneInf.afficher(c);
            borneSupInd.afficher(c);
            borneInfInd.afficher(c);
            tabInfo[0] = 0;
            Commentaire comBorneSup = new Commentaire("On positionne la borne supérieure \nà la positon du milieu -1 ", Brushes.Black, tab[bSup].CoordX + e, tab[bSup].CoordY + d, 188, 50, couleurBorneSup, couleurBorneSup);
            Commentaire comBorneInf = new Commentaire("On positionne la borne inférieure \nà la positon du milieu +1 ", Brushes.Black, tab[bInf].CoordX + e, tab[bInf].CoordY + d, 188, 50, couleurBorneInf, couleurBorneInf);
            Commentaire comMed = new Commentaire("", Brushes.Black, tab[med].CoordX + e, tab[med].CoordY + d, 50, 50, couleurDeCaseMed, couleurDeCaseMed);
            comBorneSup.opacity = 0;
            comBorneInf.opacity = 0;
            comBorneSup.ajouterCanvas(c);
            comBorneInf.ajouterCanvas(c);

            while ((bInf <= bSup) && (tabInfo[0] == 0))
            {
                await algo.colorer(couleurAlgo, 3, Temps.time);
                med = (bSup + bInf) / 2;
                comMed.Text = "On positionne le \nmilieu du tableau";
                comMed.Height = 50;
                comMed.Width = 100;
                comMed.CoordX = this.tab[med].CoordX + e;
                comMed.CoordY = this.tab[med].CoordY + d;
                if (firstIteration)
                {
                    medCase = new Champ(this.coordX + widthOfcase * med, this.coordY, 1, heightOfcase, widthOfcase, Brushes.Transparent, couleurDeCaseMed, 6);
                    medCaseInd = new Champ(this.coordX + widthOfcase * med + a, this.coordY + b, 2, 25, 25, couleurDeCaseMed, couleurDeCaseMed, 1);
                    medCase.Forme.Opacity = 0.7;
                    medCaseInd.Forme.Opacity = 0.5;
                    medCase.afficher(c);
                    medCaseInd.afficher(c);
                    firstIteration = false;
                    comMed.ajouterCanvas(c);
                    await algo.colorer(couleurAlgo, 4, Champ.time);
                    comMed.disparaitre(Champ.time);
                    await Task.Delay(TimeSpan.FromSeconds(Champ.time + 1));
                }
                else
                {
                    tabDepBSup[0].X = this.coordX + widthOfcase * bSup;
                    tabDepBInf[0].X = this.coordX + widthOfcase * bInf;
                    tabDepMedCase[0].X = this.coordX + widthOfcase * med;
                    tabDepBSupInd[0].X = this.coordX + widthOfcase * bSup + a;
                    tabDepBInfInd[0].X = this.coordX + widthOfcase * bInf + a;
                    tabDepMedCaseInd[0].X = this.coordX + widthOfcase * med + a;
                    borneSup.deplacer(tabDepBSup, 1);
                    borneInf.deplacer(tabDepBInf, 1);
                    borneSupInd.deplacer(tabDepBSupInd, 1);
                    borneInfInd.deplacer(tabDepBInfInd, 1);
                    await Task.Delay(TimeSpan.FromSeconds(Champ.time + 1));
                    medCase.deplacer(tabDepMedCase, 1);
                    medCaseInd.deplacer(tabDepMedCaseInd, 1);
                    comMed.apparaitre(Champ.time);
                    await algo.colorer(couleurAlgo, 4, 2 * Temps.time);
                    comMed.disparaitre(Champ.time);
                    await Task.Delay(TimeSpan.FromSeconds(Champ.time + 1));
                    if (bSup != this.tailleTab)
                    {
                        for (i = bSup + 1; i < this.tailleTab; i++)
                        {
                            tab[i].colorChamp(Brushes.Gray, couleurBordureCase, 1);
                            tab[i].Forme.Opacity = 0.2;
                            tab[i].TextBLock.Foreground = Brushes.Gray;
                        }
                    }
                    if (bInf != 0)
                    {
                        for (i = bInf - 1; i > -1; i--)
                        {
                            tab[i].colorChamp(Brushes.Gray, couleurBordureCase, 1);
                            tab[i].Forme.Opacity = 0.2;
                            tab[i].TextBLock.Foreground = Brushes.Gray;
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                }

                if (tab[med].Valeur == val) { tabInfo[0] = 1; }
                else {
                    await algo.colorer(couleurAlgo, 6, Temps.time);

                    if (tab[med].Valeur > val)
                    {

                        comMed.Text = val + " est inférieure à la \nvaleur du milieu du tableau";
                        comMed.Height = 50;
                        comMed.Width = 152;
                        comMed.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 9, 2 * Temps.time + 3);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time + 1.5));
                        comMed.disparaitre(Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time + 1.5));
                        comBorneSup.CoordX = this.tab[bSup].CoordX + e;
                        comBorneSup.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 10, 2 * Temps.time);
                        comBorneSup.disparaitre(Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time + 2));
                        bSup = med - 1;
                    }
                    else
                    {
                        comMed.Text = val + " est supérieure à la \nvaleur du milieu du tableau";
                        comMed.Height = 50;
                        comMed.Width = 152;
                        comMed.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 7, 2 * Temps.time);
                        comMed.disparaitre(Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time + 1));
                        comBorneInf.CoordX = this.tab[bInf].CoordX + e;
                        comBorneInf.apparaitre(Temps.time);
                        await algo.colorer(couleurAlgo, 8, 2 * Temps.time);
                        comBorneInf.disparaitre(Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time + 2));
                        bInf = med + 1;
                    }
                }
            }
            if (tabInfo[0] == 0)
            {
                tabDepBSup[0].X = this.coordX + widthOfcase * bSup;
                tabDepBInf[0].X = this.coordX + widthOfcase * bInf;
                tabDepBSupInd[0].X = this.coordX + widthOfcase * bSup + a;
                tabDepBInfInd[0].X = this.coordX + widthOfcase * bInf + a;
                borneSup.deplacer(tabDepBSup, 1);
                borneInf.deplacer(tabDepBInf, 1);
                borneSupInd.deplacer(tabDepBSupInd, 1);
                borneInfInd.deplacer(tabDepBInfInd, 1);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                if (bSup != this.tailleTab)
                {
                    for (i = bSup + 1; i < this.tailleTab; i++)
                    {
                        tab[i].colorChamp(Brushes.Gray, couleurBordureCase, 1);
                        tab[i].Forme.Opacity = 0.2;
                        tab[i].TextBLock.Foreground = Brushes.Gray;
                    }
                }
                if (bInf != 0)
                {
                    for (i = bInf - 1; i > -1; i--)
                    {
                        tab[i].colorChamp(Brushes.Gray, couleurBordureCase, 1);
                        tab[i].Forme.Opacity = 0.2;
                        tab[i].TextBLock.Foreground = Brushes.Gray;
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                tabInfo[1] = bInf;
                comPrincipal.disparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                comPrincipal.Width = 350;
                comPrincipal.Height = 50;
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.Text = "Condition d'arrêt : Borne supérieure < Borne inférieure";
                comPrincipal.apparaitre(Temps.time);

            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                await algo.colorer(couleurAlgo, 5, 2 * Temps.time);
                tabInfo[1] = med;
                for (i = 0; i < this.tailleTab; i++)
                {
                    this.tab[i].colorChamp(couleurFondCase, couleurBordureCase, 1);
                    this.tab[i].Forme.Opacity = 1;
                    this.tab[i].TextBLock.Foreground = Brushes.Black;
                }
                c.Children.Remove(borneSup.Forme);
                c.Children.Remove(borneInf.Forme);
                c.Children.Remove(medCase.Forme);
                c.Children.Remove(borneSupInd.Forme);
                c.Children.Remove(borneInfInd.Forme);
                c.Children.Remove(medCaseInd.Forme);
                comPrincipal.disparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                comPrincipal.CouleurFond = couleurTrouv;
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                comPrincipal.Text = "Valeur " + val + " a été trouvée";
                if (inserSupp)
                {
                    this.tab[med].colorChamp(couleurTrouv, couleurTrouv, 1);
                    this.tab[med].Forme.Opacity = 0.7;
                }
                else
                {
                    this.tab[med].colorChamp(couleurTrouv, couleurTrouv, 1);
                    this.tab[med].Forme.Opacity = 0.7;
                  // this.tab[med].vibrate(2, 19);
                }
                comPrincipal.apparaitre(Temps.time);

            }
            await Task.Delay(TimeSpan.FromSeconds(3));
            for (i = 0; i < this.tailleTab; i++)
            {
                this.tab[i].colorChamp(couleurFondCase, couleurBordureCase, 1);
                this.tab[i].Forme.Opacity = 1;
                this.tab[i].TextBLock.Foreground = Brushes.Black;
            }
            comPrincipal.disparaitre(0.5);
            c.Children.Remove(borneSup.Forme);
            c.Children.Remove(borneInf.Forme);
            c.Children.Remove(medCase.Forme);
            c.Children.Remove(borneSupInd.Forme);
            c.Children.Remove(borneInfInd.Forme);
            c.Children.Remove(medCaseInd.Forme);
            comBorneInf.enleverCanvas(c);
            comBorneSup.enleverCanvas(c);
            comMed.enleverCanvas(c);
            await algo.colorer(couleurAlgo, 11, Temps.time);
            algo.disparaitre(Alg);
        }

        private async Task rechercheSeq(int val, int[] tabInfo, Canvas c, Boolean insSup, Commentaire comPrincipal, Canvas Alg)
        {
            //Effectue une recherche séquentiel dans le tableau tab 
            // tabInfo[0] correspond au boolean trouv et tabInfo[1] correspond à l'indice de la valeur val si il est trouvée sinon l'indice ou devrait être insérée cette valeur .
            // inserSup est un boolean qui indique si il ya une insertion ou suppression aprés cette recherche  
            int i = 0;
            tabInfo[0] = 0;
            SolidColorBrush couleurparcours = Brushes.Red;
            SolidColorBrush couleurtrouve = Brushes.Green;
            Commentaire com = new Commentaire(" ", Brushes.Black, this.tab[i].CoordX - 5, this.tab[i].CoordY - 35, 50, 50, couleurparcours, couleurparcours);
            Algo algo = new Algo(1, coordX_Algo, coordY_Algo);
            algo.afficher(Alg);
            com.ajouterCanvas(c);
            com.opacity = 0;
            com.Height = 30;
            com.Width = 160;
            await algo.colorer(couleurAlgo, 0, Temps.time);
            await algo.colorer(couleurAlgo, 1, Temps.time);
            await algo.colorer(couleurAlgo, 2, Temps.time);

            while (i < this.tailleTab & tabInfo[0] == 0)
            {
                await algo.colorer(couleurAlgo, 3, Temps.time);
                this.tab[i].Forme.Opacity = 0.7;
                this.tabIndices[i].Forme.Opacity = 0.7;
                if (this.tab[i].Valeur != val)
                {
                    com.CoordX = this.tab[i].CoordX;
                    this.tab[i].colorChamp(couleurparcours, Brushes.Black, 1);
                    this.tabIndices[i].colorChamp(couleurparcours, Brushes.Black, 0);
                    com.Text = "  " + this.tab[i].Valeur + " est différente de " + val;
                    com.apparaitre(Temps.time);
                    await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                    this.tab[i].colorChamp(couleurFondCase, couleurBordureCase, 1); 
                    this.tabIndices[i].colorChamp(couleurFondCase, couleurBordureCase, 0);
                  
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    i++;

                }
                else
                {
                    tabInfo[0] = 1;
                    comPrincipal.disparaitre(1);
                    comPrincipal.Text = "La valeur " + val + " a été trouvé";
                    await algo.colorer(couleurAlgo, 4, Temps.time);
                    comPrincipal.CouleurFond = couleurtrouve;
                    this.tab[i].Forme.Opacity = 0.7;
                    comPrincipal.apparaitre(Temps.time);
                    this.tabIndices[i].Forme.Opacity = 0.7;
                    if (insSup == false)
                    {
                        this.tab[i].colorChamp(couleurtrouve, couleurtrouve, 1);
                        this.tabIndices[i].colorChamp(couleurtrouve, couleurtrouve, 0);
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        com.disparaitre(0);
                       // this.tab[i].vibrate(4, 19);
                        await Task.Delay(TimeSpan.FromSeconds(4));
                    }
                    else
                    {


                        this.tab[i].colorChamp(couleurtrouve, couleurtrouve, 1);
                        this.tabIndices[i].colorChamp(couleurtrouve, couleurtrouve, 0);
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        com.disparaitre(0);
                    }
                    tabInfo[1] = i;
                }
           
            }
            if (tabInfo[0] == 0)
            {
                comPrincipal.disparaitre(2.5);
                comPrincipal.Width = 200;
                comPrincipal.Text = "La valeur " + val + " n'a pas été trouvé";
                comPrincipal.CouleurFond = couleurparcours;
                comPrincipal.apparaitre(Temps.time);
           

                com.disparaitre(0.5);
                com.enleverCanvas(c);
                i--;
            }
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            for (i = 0; i < this.tailleTab; i++)
            {
                this.tab[i].colorChamp(couleurFondCase, couleurBordureCase, 1);
                this.tab[i].Forme.Opacity = 1;
                this.tabIndices[i].colorChamp(Brushes.Transparent, Brushes.Transparent, 0);
                this.tab[i].TextBLock.Foreground = Brushes.Black;
            }         
            comPrincipal.disparaitre(0.5);
            com.enleverCanvas(c);  
            await algo.colorer(Brushes.Red, 7, 0.5 * Temps.time);
            algo.disparaitre(Alg);
         
        }

        public async Task suppression(int valeur, Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int[] tabInfo = new int[2];     //Le tableau qui sera modifié par la fonction de recherche
            await recherche(valeur, tabInfo, c, true, comPrincipal, Alg);//Il retourne un entier qui décrit l'echec (0) ou la réussite (1) de la recherche ainsi que la position
            comPrincipal = new Commentaire("Suppression en cours ...", Brushes.Black, this.coordX, this.coordY - 150, 150, 70, Brushes.PaleGreen, Brushes.White);
            Algo algo = new Algo(7, coordX_Algo, coordY_Algo);
            if (tabInfo[0] == 1)
            {
                Commentaire com = new Commentaire("On décrémente la taille du tableau", Brushes.Black, this.coordX + this.tab[tabInfo[1]].Width * tabInfo[1], this.coordY - tab[0].Height, 200, 30, Brushes.GreenYellow, Brushes.White);
                com.ajouterCanvas(c);       //On crée le commentaire qui explique l'avacncement de l'algorithme
                Point[] tabl = new Point[3];
                if (this.triee && (tabInfo[1] < tailleTab - 1))     //Cas trié, on traitera le cas où la valeur recherchée se trouve à la fin du tableau à part
                {
                    algo.afficher(Alg);
                    await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 1, Temps.time);
                    com.Text = "On décale toutes les cases suivantes";
                    com.Width = 200;
                    com.Height += 20;
                    com.apparaitre(0);  //On adapte le commentaire au contexte
                    tabl[0] = new Point(tabInfo[1] * widthOfcase + coordX, coordY); //On fait disparaître la case à supprimer
                    this.tab[tabInfo[1]].disappear(tabl, 1);
                    int i;
                    for (i = tabInfo[1]; i < this.tailleTab - 1; i++)       //On effectue les décalages
                    {
                        await algo.colorer(couleurAlgo, 2, Temps.time);
                        tabl[0] = new Point(i * widthOfcase + coordX, coordY);
                        this.tab[i + 1].deplacer(tabl, 1);
                        await algo.colorer(couleurAlgo, 3, Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                        this.tab[i] = this.tab[i + 1];
                        await algo.colorer(couleurAlgo, 4, Temps.time);
                    }
                    await algo.colorer(couleurAlgo, 5, Temps.time);
                    await algo.colorer(couleurAlgo, 6, Temps.time);
                    await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    algo.disparaitre(Alg);
                }
                else if (tabInfo[1] != tailleTab - 1)     //Cas non trié, supression d'un élément au milieu
                {
                    com.Text = "On remplace le contenu de la case\npar celui de la dernière case\net on décrémente la taille";
                    com.Width = 200;
                    com.Height = 70;
                    com.CoordX = com.CoordX - this.tab[0].Width;
                    com.CoordY -= 30;
                    com.apparaitre(0);                              //On adapte le commentaire et on l'affiche
                    tabl[0] = new Point(tabInfo[1] * widthOfcase + coordX, coordY);     //La case à supprimer disparaît sur place
                    this.tab[tabInfo[1]].disappear(tabl, 1);
                    this.tab[tabInfo[1]] = this.tab[this.tailleTab - 1];      //On reemplcae la première valeur par la dernière
                    tabl[0] = new Point((tailleTab - 1) * widthOfcase + coordX, coordY - heightOfcase);
                    tabl[1] = new Point(tabInfo[1] * widthOfcase + coordX, coordY - heightOfcase);
                    tabl[2] = new Point(tabInfo[1] * widthOfcase + coordX, coordY);     //On définit les points par où passe la case à déplacer
                    this.tab[tailleTab - 1].deplacer(tabl, 3);      //Animation du remplacement
                    await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    com.disparaitre(Champ.time);
                }
                else        //Cas non trié, suppression d'un élément à la fin
                {
                    tabl[0] = new Point(tabInfo[1] * widthOfcase + coordX, coordY);     //Gestion de l'animation (disparition de la dernière case)
                    this.tab[tabInfo[1]].disappear(tabl, 1);
                }
                this.tailleTab--;               //On décrémente la taille
                this.tabIndices[tailleTab].masquer(c);      //On masque Le dernier indice
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                com.enleverCanvas(c);                   //Fin de l'animation
                comPrincipal.CouleurFond = Brushes.Green;           //Commentaire de réussite de la suppression
                comPrincipal.Text = "Suppression réussie";
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            else        //Cas de non suppression (élément non trouvé)
            {
                comPrincipal.CouleurFond = Brushes.Red;         //Commentaire d'échec de suppression
                comPrincipal.Text = "Suppression impossible\nélément non trouvé";
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
        }
        public async Task inserer(int valeur, Canvas c, Commentaire comPrincipal, Canvas Alg)
        //Insetion d'un nouvel élément dans le tableau //
        {
            int[] tabInfo = new int[2];
            Point[] tab_point = new Point[2];
            Commentaire comDecalage = new Commentaire("On décale les valeurs pour l'insertion ", Brushes.Black, this.coordX + 50, this.coordY - 50, 210, 30, Brushes.GreenYellow, Brushes.White);
            Algo algo = new Algo(6, coordX_Algo, coordY_Algo);
            Algo algo1 = new Algo(27, coordX_Algo, coordY_Algo);
            int pos;//Position d'insertion
            int i = tailleTab;
            comPrincipal.disparaitre(0);
            if (i < tailleMaxTab)
            {
                Case nouvelleCase = new Case(valeur, coordX, this.coordY - 100, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                if (tailleTab > 0)//Si il exite au moins une case
                {
                    await recherche(valeur, tabInfo, c, true, comPrincipal, Alg); //Recherche Dichothomique de la valeur
                    comPrincipal.disparaitre(0);
                }
                if (tabInfo[0] == 0) //Si la valeur n'existe pas
                {
                    pos = tabInfo[1];
                    tabIndices[tailleTab].afficher(c);//Afficher le nouvel indice
                    nouvelleCase.afficher(c);//Affichage de la nouvelle case crée
                    if (this.triee)
                    {
                        algo.afficher(Alg);
                        await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 1, Temps.time);
                        comDecalage.ajouterCanvas(c);//Affichage du message d'insertion
                        for (i = this.tailleTab; i > pos; i--)//Décalage des cases pour insérer la nouvelle valeur
                        {
                            await algo.colorer(couleurAlgo, 2, Temps.time);
                            tab_point[0] = new Point(coordX + i * widthOfcase, coordY);
                            tab[i] = tab[i - 1]; //Décalage interne des cases du tableau
                            tab[i - 1].deplacer(tab_point, 1);//Décalage graphique des cases du tableau
                            await algo.colorer(couleurAlgo, 3, Temps.time);
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            await algo.colorer(couleurAlgo, 4, Temps.time);

                        }
                        await algo.colorer(couleurAlgo, 5, Temps.time);
                        comDecalage.enleverCanvas(c);
                        tab_point[0] = new Point(coordX + i * widthOfcase, coordY);
                        tab[i] = nouvelleCase;
                        tab[i].deplacer(tab_point, 1);//Insértion graphique de la case 
                        comPrincipal.Width = 200;
                        comPrincipal.CouleurFond = couleurComPrincipal;
                        comPrincipal.Text = "Insertion de la valeur ";
                        comPrincipal.apparaitre(0);
                        await algo.colorer(couleurAlgo, 6, Temps.time);
                        comPrincipal.disparaitre(0);
                        await algo.colorer(couleurAlgo, 7, Temps.time);
                        await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                        this.tailleTab++;//Incrémentation de la taille du tableau 
                        algo.disparaitre(Alg);
                    }
                    else {
                        algo1.afficher(Alg);
                        tab_point[0] = new Point(coordX + i * widthOfcase, coordY);
                        tab[i] = nouvelleCase;
                        tab[i].deplacer(tab_point, 1);//Insértion graphique de la case 
                        comPrincipal.Width = 200;
                        comPrincipal.CouleurFond = couleurComPrincipal;
                        comPrincipal.Text = "Insertion de la valeur ";
                        comPrincipal.apparaitre(0);
                        comPrincipal.disparaitre(0);
                        this.tailleTab++;//Incrémentation de la taille du tableau 
                        algo1.disparaitre(Alg);
                    }
                }
                else
                {

                    comPrincipal.CouleurFond = couleurComPrincipal;
                    comPrincipal.Text = "Insertion impossible , la valeur existe déja ";
                    comPrincipal.Width = 320;
                    comPrincipal.apparaitre(0);
                    await Task.Delay(TimeSpan.FromSeconds(Temps.time + 1));
                    comPrincipal.disparaitre(0);
                }
            }
            else
            {
                comPrincipal.CouleurFond = couleurComPrincipal;
                comPrincipal.Text = "Insertion impossible , taille du tableau maximum atteinte ";
                comPrincipal.Width = 320;
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time + 1));
                comPrincipal.disparaitre(0);
            }
        }
        public void permuterDeuxCase(int i1, int i2, SolidColorBrush colorPermute, SolidColorBrush colorpermutePas)
        {
            Case a = new Case();
            Point[] TableauDePoints = new Point[3];
            Point[] TableauDePoints2 = new Point[3];

            tab[i1].colorChamp(colorPermute, Brushes.Black, 1);
            tab[i2].colorChamp(colorPermute, Brushes.Black, 1);

            tab[i1].Forme.Opacity = 0.5;
            tab[i2].Forme.Opacity = 0.5;

            if (i1 == i2)
            {
                tab[i1].colorChamp(colorpermutePas, Brushes.Black, 1);
            }
            else if ((i1 == i2 + 1) || (i2 == i1 + 1))
            {

                tab[i1].colorChamp(colorPermute, Brushes.Black, 1);
                tab[i2].colorChamp(colorPermute, Brushes.Black, 1);

                TableauDePoints[0] = new Point(coordX + widthOfcase * i1, coordY + heightOfcase / 2);
                TableauDePoints[1] = new Point((coordX + widthOfcase * i1) + widthOfcase * (i2 - i1), coordY + heightOfcase / 2);
                TableauDePoints[2] = new Point((coordX + widthOfcase * i1) + widthOfcase * (i2 - i1), coordY);

                TableauDePoints2[0] = new Point(coordX + widthOfcase * i2, coordY - heightOfcase / 2);
                TableauDePoints2[1] = new Point((coordX + widthOfcase * i2) - widthOfcase * (i2 - i1), coordY - heightOfcase / 2);
                TableauDePoints2[2] = new Point((coordX + widthOfcase * i2) - widthOfcase * (i2 - i1), coordY);


                tab[i1].deplacer(TableauDePoints, 3);
                tab[i2].deplacer(TableauDePoints2, 3);

            }
            else
            {

                tab[i1].colorChamp(colorPermute, Brushes.Black, 1);
                tab[i2].colorChamp(colorPermute, Brushes.Black, 1);
                TableauDePoints[0] = new Point(coordX + widthOfcase * i1, coordY + heightOfcase);
                TableauDePoints[1] = new Point((coordX + widthOfcase * i1) + widthOfcase * (i2 - i1), coordY + heightOfcase);
                TableauDePoints[2] = new Point((coordX + widthOfcase * i1) + widthOfcase * (i2 - i1), coordY);


                TableauDePoints2[0] = new Point(coordX + widthOfcase * i2, coordY - heightOfcase);
                TableauDePoints2[1] = new Point((coordX + widthOfcase * i2) - widthOfcase * (i2 - i1), coordY - heightOfcase);
                TableauDePoints2[2] = new Point((coordX + widthOfcase * i2) - widthOfcase * (i2 - i1), coordY);

                tab[i1].deplacer(TableauDePoints, 3);
                tab[i2].deplacer(TableauDePoints2, 3);

            }

            a = tab[i1];
            tab[i1] = tab[i2];
            tab[i2] = a;
            tab[i1].Forme.Opacity = 1;
            tab[i2].Forme.Opacity = 1;

        }
        public int indcinePlusPetit(int b1, int b2)
        {
            int petit = new int();
            int ind = new int();
            int i = new int();
            petit = tab[b1].Valeur;
            ind = b1;

            for (i = b1; i <= b2; i++)
            {
                if (tab[i].Valeur < petit)
                {
                    petit = this.tab[i].Valeur;
                    ind = i;
                }
            }

            return ind;
        }

        public async Task trieeSelection(Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int i, j, i1, j1;

            SolidColorBrush couleurPermute = Brushes.Red;
            SolidColorBrush couleurPermutePas = Brushes.Green;
            SolidColorBrush couleurCaseAleurPlace = Brushes.LightGreen;
            comPrincipal.Text = "trie par selection\nPrincipe: Le plus petit élément du tableau est permuté avec\n le 1er élément du tableau,\n puis le plus petit élément du tableau restant\n est permuté avec le 2ième,\n etc...  ";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.Width = 340;
            comPrincipal.Height = 110;
            comPrincipal.apparaitre(0);
            Algo algo = new Algo(3, coordX_Algo, coordY_Algo);
            Commentaire com = new Commentaire("", Brushes.Black, coordX, coordY + 100, 260, 50, couleurPermute, couleurPermute);
            com.ajouterCanvas(c);
            com.opacity = 0;
            algo.afficher(Alg);
            await algo.colorer(couleurAlgo, 0, Temps.time);
            Commentaire com2 = new Commentaire("", Brushes.Black, coordX, coordY - 60, 140, 50, couleurPermutePas, couleurPermutePas);
            com2.ajouterCanvas(c);
            com2.opacity = 0;

            for (i = 0; i < this.tailleTab; i++)
            {
                await algo.colorer(couleurAlgo, 1, Temps.time);
                j = indcinePlusPetit(i, this.tailleTab - 1);

                tab[j].colorChamp(couleurFondCase, Brushes.Green, 4);

                com2.Text = "le plus petit element \ndans le tableau restant.  ";
                com2.CoordX = widthOfcase * (j + 1);

                com2.CouleurFond = couleurPermutePas;
                com2.CouleurBordure = couleurPermutePas;

                com2.apparaitre(Temps.time);
                await algo.colorer(couleurAlgo, 2, Temps.time);
                com2.disparaitre(Temps.time / 2);

                await Task.Delay((TimeSpan.FromSeconds(Temps.time * 3)));
                tab[j].colorChamp(couleurFondCase, couleurBordureCase, 1);
                await algo.colorer(couleurAlgo, 3, Temps.time);
                permuterDeuxCase(i, j, couleurPermute, couleurPermute);

                i1 = i + 1;
                j1 = j + 1;

                if (i == j)
                {
                    com.Text = "On permute pas ";
                    com.Width = 100;


                    com.CouleurFond = couleurPermutePas;
                    com.CouleurBordure = couleurPermutePas;
                }
                else {

                    com.Text = " le plus petit element dans le tableau est : " + tab[j].Valeur + ".\n On permute " + "les cases : " + i1 + " et " + j1;
                    com.Width = 260;
                    com.CouleurFond = couleurPermute;
                    com.CouleurBordure = couleurPermute;
                }

                com.apparaitre(Temps.time);
                await Task.Delay((TimeSpan.FromSeconds(Temps.time * 3)));
                com.disparaitre(Temps.time / 2);


                tab[j].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                tab[i].colorChamp(couleurCaseAleurPlace, couleurPermutePas, 1);


            }
            for (i = 0; i < TailleTab; i++)
            {
                tab[i].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
            }
            comPrincipal.disparaitre(0);
            await algo.colorer(couleurAlgo, 4, Temps.time);
            await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
            algo.disparaitre(Alg);
        }

        public async Task trieeTransposition(Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int i, j, i1, j1;
            SolidColorBrush couleurPermute = Brushes.Red;
            SolidColorBrush couleurPermutePas = Brushes.Green;
            Algo algo = new Algo(4, coordX_Algo, coordY_Algo);
            comPrincipal.Text = "trie par transposition\n Deux éléments qui se suivent sont comparés puis \npermutés si le 2ième élément est plus petit que le premier,\n(dans ce cas un retour en arrière est effectué afin de vérifier si\nl'ordre n'a pas été modifié, auquel cas on le rétablit). ";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.Width = 340;
            comPrincipal.Height = 100;
            comPrincipal.apparaitre(0);
            algo.afficher(Alg);
            await algo.colorer(couleurAlgo, 0, Temps.time);
            Commentaire com = new Commentaire("", Brushes.Black, coordX, coordY + 100, 120, 50, couleurPermute, couleurPermute);
            com.ajouterCanvas(c);
            com.opacity = 0;
            for (i = 0; i < this.tailleTab - 1; i++)
            {
                j = i;
                await algo.colorer(couleurAlgo, 1, Temps.time);
                await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
                while ((j >= 0) && (tab[j].Valeur > tab[j + 1].Valeur))
                {
                    permuterDeuxCase(j, j + 1, couleurPermute, couleurPermute);
                    i1 = j + 1;
                    j1 = j + 2;
                    com.Text = "On permute \n" + "les cases : " + i1 + " et " + j1;
                    com.CouleurFond = couleurPermute;
                    com.CouleurBordure = couleurPermute;
                    com.apparaitre(Temps.time);
                    if (i == j)
                    {
                        await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                    }
                    else
                    {
                        await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);

                    }
                    await Task.Delay((TimeSpan.FromSeconds(Temps.time * 3)));
                    com.disparaitre(Temps.time / 2);
                    tab[j].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                    tab[j + 1].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                    j = j - 1;

                }
                await algo.colorer(couleurAlgo, 9, Temps.time);
                if (j != tailleTab - 2 && tab[j + 1].Valeur <= tab[j + 2].Valeur)
                {
                    com.Text = "On permute pas ";
                    com.CouleurFond = couleurPermutePas;
                    com.CouleurBordure = couleurPermutePas;

                    if (j != -1)
                    {

                        tab[j].colorChamp(couleurPermutePas, couleurBordureCase, 1);
                        tab[j + 1].colorChamp(couleurPermutePas, couleurBordureCase, 1);


                        com.apparaitre(Temps.time);
                        await Task.Delay((TimeSpan.FromSeconds(Temps.time * 3)));
                        com.disparaitre(Temps.time / 2);

                        tab[j].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                        tab[j + 1].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                    }


                }
            }
            await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);
            await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
            algo.disparaitre(Alg);
            comPrincipal.disparaitre(0);
        }

        public async Task trieeBulle(Canvas c, Commentaire comPrincipal, Canvas Alg)
        {
            int i, m, i1, j1;
            Algo algo = new Algo(5, coordX_Algo, coordY_Algo);
            Boolean modif;
            m = tailleTab;
            modif = true;

            SolidColorBrush couleurPermute = Brushes.Red;
            SolidColorBrush couleurPermutePas = Brushes.Green;
            SolidColorBrush couleurCaseAleurPlace = Brushes.LightGreen;
            comPrincipal.Text = "trie par bulle\n Principe : on parcourt tout le tableau et si \nelem[i] > elem[i + 1], on les permute.\n Il est évident que plusieurs passages sur l'ensemble \ndes éléments sont nécessaires.\nEt on s’arrêtera lorsqu’il n’y a plus de permutations.   ";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.Width = 300;
            comPrincipal.Height = 110;
            algo.afficher(Alg);
            await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
            Commentaire com = new Commentaire("", Brushes.Black, coordX, coordY + 100, 120, 50, couleurPermute, couleurPermute);
            com.ajouterCanvas(c);
            com.opacity = 0;
            comPrincipal.apparaitre(0);
            while ((modif == true) || (m > 2))
            {
                modif = false;
                await algo.colorer(couleurAlgo, 1, Temps.time);
                for (i = 0; i < m - 1; i++)
                {
                    await algo.colorer(couleurAlgo, 2, Temps.time);
                    if (tab[i].Valeur > tab[i + 1].Valeur)
                    {
                        await algo.colorer(couleurAlgo, 3, Temps.time);
                        permuterDeuxCase(i, i + 1, couleurPermute, couleurPermutePas);
                        await algo.colorer(couleurAlgo, 4, 2 * Temps.time);
                        i1 = i + 1;
                        j1 = i + 2;
                        com.Text = tab[i + 1].Valeur + " > " + tab[i].Valeur + " ." + "\nOn permute \n" + "les cases : " + i1 + " et " + j1;
                        com.CouleurFond = couleurPermute;
                        com.CouleurBordure = couleurPermute;
                        com.Width = 130;
                        com.Height = 60;
                        com.apparaitre(Temps.time);
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time * 3));
                        com.disparaitre(Temps.time / 2);
                        tab[i].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                        tab[i + 1].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);

                        modif = true;
                    }
                    else
                    {
                        com.Text = "On permute pas \n on passe au suivant ";

                        com.CouleurFond = couleurPermutePas;
                        com.CouleurBordure = couleurPermutePas;
                        com.Width = 120;
                        com.Height = 50;
                        com.apparaitre(Temps.time);

                        tab[i].colorChamp(couleurPermutePas, this.couleurBordureCase, 1);
                        tab[i + 1].colorChamp(couleurPermutePas, this.couleurBordureCase, 1);


                        await Task.Delay(TimeSpan.FromSeconds(Temps.time * 3));
                        com.disparaitre(Temps.time / 2);

                        tab[i].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
                        tab[i + 1].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);

                    }
                    await algo.colorer(couleurAlgo, 5, Temps.time);

                }
                await algo.colorer(couleurAlgo, 6, Temps.time);

                tab[m - 1].colorChamp(couleurCaseAleurPlace, Brushes.Green, 1);
                m = m - 1;

            }
            for (i = 0; i < TailleTab; i++)
            {
                tab[i].colorChamp(this.couleurFondCase, this.couleurBordureCase, 1);
            }

            comPrincipal.disparaitre(0);
            await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
            algo.disparaitre(Alg);
        }

    }
}
