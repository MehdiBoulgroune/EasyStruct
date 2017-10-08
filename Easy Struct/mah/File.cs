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
    class File
    {
        private Case[] tab;
        private int tailleTab;
        private int sommetFile;
        private double coordX;
        private double coordY;
        private Point[] tabpt = new Point[2];
        private Commentaire comPrincipal;


        private SolidColorBrush couleurAlgo = Brushes.Red;
        private SolidColorBrush couleurFondCase = Brushes.Red;
        private SolidColorBrush couleurBordureCase = Brushes.Black;
        private SolidColorBrush couleurFondFile = Brushes.White;
        private SolidColorBrush couleur_Deplace_dans_File = Brushes.LightGreen;

        /*********** LES CONSTANTES ***********/
        private const int tailleMaxFile = 6;
        private const double widthOfcase = 50;
        private const double heightOfcase = 50;
        private const double a = widthOfcase / 2 - 10, b = 55;
        private const double coordX_Algo = 500;
        private const double coordY_Algo = 300;
        /***************************************/
        public File()
        {

        }
        public File(double coordX, double coordY)
        {

            this.tab = new Case[tailleMaxFile];
            this.coordX = coordX;
            this.coordY = coordY;
            this.sommetFile = 0;
            comPrincipal = new Commentaire("   File vide !", Brushes.Black, this.coordX + 70, this.coordY - 65, 140, 50, Brushes.LightGreen, Brushes.LightGreen);
            tabpt[0].X = widthOfcase * tailleMaxFile + (coordX - widthOfcase); //sauvegarde de premier ou on vas défiler
            tabpt[0].Y = coordY;





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
            forme = new Rectangle();

            Canvas.SetLeft(forme, coordX);
            Canvas.SetTop(forme, coordY);
            forme.Height = heightOfcase;
            forme.Width = widthOfcase * tailleMaxFile;
            forme.Fill = couleurFondFile;
            forme.Stroke = couleurBordureCase;
            forme.StrokeThickness = 3.5;
            forme.Opacity = 1;
            c.Children.Add(forme);
            comPrincipal.ajouterCanvas(c);
        }

        public async Task enFiler(int val, Canvas c, Canvas Algo)
        {
            Algo algo = new Algo(25, coordX_Algo, coordY_Algo);
            algo.afficher(Algo);

            if (this.sommetFile < tailleMaxFile)
            {
                comPrincipal.Text = "Enfilment en cours ...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Yellow;
                comPrincipal.apparaitre(1);

                this.tab[sommetFile] = new Case(val, this.coordX - (widthOfcase + 10), this.coordY, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                this.tab[sommetFile].afficher(c);
                this.tab[sommetFile].Forme.Opacity = 0.7;
                await Task.Delay(TimeSpan.FromSeconds(.4));

                this.tab[sommetFile].deplacer(tabpt, 1);
                this.tab[sommetFile].colorChamp(couleur_Deplace_dans_File, couleur_Deplace_dans_File, 1);


                tabpt[0].X = this.tab[sommetFile].CoordX - widthOfcase;

                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                this.tab[sommetFile].colorChamp(couleurFondFile, couleurBordureCase, 1);
                sommetFile++;

                comPrincipal.disparaitre(.5);

            }

            else
            {
                comPrincipal.Text = "    File plien !";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(1.3);
                comPrincipal.disparaitre(1.5);
            }
            algo.disparaitre(Algo);

        }
        public async Task deFiler(Canvas c, Canvas Algo)
        {

            Algo algo = new Algo(26, coordX_Algo, coordY_Algo);
            Point[] tabpt2 = new Point[2];//tableau pour le déplacement pendant la disparition
            tabpt2[0].X = this.tab[0].CoordX + widthOfcase;
            tabpt2[0].Y = this.tab[0].CoordY;
            tabpt2[1].X = tabpt2[0].X + 100;
            tabpt2[1].Y = tabpt2[0].Y - 100;
            algo.afficher(Algo);
            if (this.sommetFile > 0)
            {
                comPrincipal.Text = "DéFilment en cours ...";
                comPrincipal.CouleurFond = Brushes.Yellow;
                comPrincipal.CouleurBordure = Brushes.Yellow;
                comPrincipal.apparaitre(1);
                sommetFile--;
                tabpt[0].X = this.tab[sommetFile].CoordX;
                tabpt[0].Y = this.tab[sommetFile].CoordY;

                this.tab[0].colorChamp(couleur_Deplace_dans_File, couleur_Deplace_dans_File, 1);
                this.tab[0].disappear(tabpt2, 2);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));

                comPrincipal.disparaitre(.5);


                for (int i = 0; i < sommetFile; i++)//decalage interfile
                {
                    comPrincipal.Text = "Décalage  ...";
                    comPrincipal.CouleurFond = couleur_Deplace_dans_File;
                    comPrincipal.CouleurBordure = couleur_Deplace_dans_File;
                    comPrincipal.apparaitre(1);


                    this.tab[i] = this.tab[i + 1];
                    tabpt[0].X = this.tab[i].CoordX + widthOfcase;
                    this.tab[i].deplacer(tabpt, 1);

                    this.tab[i].colorChamp(couleur_Deplace_dans_File, couleur_Deplace_dans_File, 1);
                    await Task.Delay(TimeSpan.FromSeconds(.5));
                    this.tab[i].colorChamp(couleurFondFile, couleurBordureCase, 1);



                    tabpt[0].X = this.tab[i].CoordX - widthOfcase;
                    comPrincipal.disparaitre(.5);
                }


            }
            else
            {
                comPrincipal.Text = "    File vide !";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.CouleurBordure = Brushes.Red;
                comPrincipal.apparaitre(1.3);
                comPrincipal.disparaitre(1.5);
            }
            algo.disparaitre(Algo);
        }


    }
}
