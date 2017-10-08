using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace mah
{
    class Pile
    {
        private Case[] tab;
        private int tailleTab;
        private int sommetPile;
        private double coordX;
        private double coordY;
        private Point[] tabpt = new Point[1];  // emplacement derniere val 
        private Commentaire comPrincipal;

        private SolidColorBrush couleurFondCase = Brushes.Red;//couleur fond de la case lors initialisation
        private SolidColorBrush couleurBordureCase = Brushes.Black;
        private SolidColorBrush couleurFondPile = Brushes.White;//couleur fond de la pile
        private SolidColorBrush couleur_Deplace_dans_Pile = Brushes.LightGreen;//couleur de deplacement dans la pile
        private SolidColorBrush couleurAlgo = Brushes.Red;


        /*********** LES CONSTANTES ***********/
        private const int tailleMaxPile = 6;
        private const double widthOfcase = 136;
        private const double heightOfcase = 50;
        private const double a = widthOfcase / 2 - 10, b = 55;
        private const double coordX_Algo = 400;
        private const double coordY_Algo = 100;
        /***************************************/
        public Pile()
        {

        }

        public Pile(double coordX, double coordY)//constructeur qui initialise les attributs de la pile
        {
            double d = -170;
            this.tab = new Case[tailleMaxPile];
            this.coordX = coordX;
            this.coordY = coordY;
            comPrincipal = new Commentaire("   Pile vide !", Brushes.Black, this.coordX + d, this.coordY, 140, 50, Brushes.LightGreen, Brushes.LightGreen);
            this.sommetPile = 0;
            tabpt[0].X = this.coordX; //sauvegarde de premier ou on vas dépiler
            tabpt[0].Y = heightOfcase * tailleMaxPile + (coordY - heightOfcase);

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
        {
            Shape forme;
            forme = new Rectangle();//Rectangle qui représente la pile
            forme.Opacity = 1;
            forme.Height = heightOfcase * tailleMaxPile; //initialisation de la taille de la pile
            forme.Width = widthOfcase;
            forme.Fill = couleurFondPile;//initialisation du fond  de la pile
            forme.Stroke = couleurBordureCase;
            forme.StrokeThickness = 3.5; //initialisation de la bordure de la pile
            Canvas.SetLeft(forme, coordX);
            Canvas.SetTop(forme, coordY);
            c.Children.Add(forme);
            comPrincipal.ajouterCanvas(c);

        }
        public async void empiler(int val, Canvas c, Canvas Algo)
            // Empilement d'une valeur 
        {
            Algo algo = new Algo(23, coordX_Algo, coordY_Algo);
            algo.afficher(Algo);//Affichage de l'algorithme

            if (this.sommetPile < tailleMaxPile)
            {
                comPrincipal.Text = "Empilement en cours ...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Yellow;
                comPrincipal.apparaitre(1);

                this.tab[sommetPile] = new Case(val, this.coordX, this.coordY - (heightOfcase + 10), 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                this.tab[sommetPile].Forme.Opacity = 0.7;
                this.tab[sommetPile].afficher(c); //Affichage d'une nouvelle case 

                await Task.Delay(TimeSpan.FromSeconds(.6));
                this.tab[sommetPile].deplacer(tabpt, 1); //Déplacer la case pour l'empiler
                this.tab[sommetPile].colorChamp(couleur_Deplace_dans_Pile, couleur_Deplace_dans_Pile, 1); //colorer la case


                tabpt[0].Y = this.tab[sommetPile].CoordY - heightOfcase;

                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                this.tab[sommetPile].colorChamp(couleurFondPile, couleurBordureCase, 1);


                sommetPile++;
                comPrincipal.disparaitre(.5);
            }
            else
            {
                comPrincipal.Text = "  Pile pleine !";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(1.3);
                comPrincipal.disparaitre(1.5);

            }
            algo.disparaitre(Algo);// Disparaitre l'algorithme
        }
        public async void depiler(Canvas c, Canvas Algo)
            //Dépilement d'une valeur de la pile
        {
            Algo algo = new Algo(24, coordX_Algo, coordY_Algo); //Initialisation de l'algorithme
            algo.afficher(Algo);//Affichage del'algorithme
            Point[] tabpt2 = new Point[2];//tableau pour le déplacement pendant la disparition
            tabpt2[0].X = coordX;
            tabpt2[0].Y = coordY - 60;
            tabpt2[1].X = coordX + 400;
            tabpt2[1].Y = tabpt2[0].Y - 200;
            if (this.sommetPile > 0) // Si il y a une valeur dans la pile
            {
                comPrincipal.Text = "Dépilement en cours ...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Yellow;
                comPrincipal.apparaitre(1);
                sommetPile--;
                tabpt[0].X = this.tab[sommetPile].CoordX;
                tabpt[0].Y = this.tab[sommetPile].CoordY;
                this.tab[sommetPile].colorChamp(couleur_Deplace_dans_Pile, couleur_Deplace_dans_Pile, 1);
                this.tab[sommetPile].disappear(tabpt2, 2);//Faire disparaitre la case (valeur)
                comPrincipal.disparaitre(Temps.time); 
                await Task.Delay(TimeSpan.FromSeconds(.6));
            }
            else {
                comPrincipal.Text = "  Pile vide !";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(1.3);
                comPrincipal.disparaitre(1.5);

            }
            algo.disparaitre(Algo); //Disparaitre l'algorithme
        }


    }
}
