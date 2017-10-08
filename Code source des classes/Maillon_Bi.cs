using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace mah
{
    class Maillon_bi : Maillon
    {
        private Fleche prec;
        public Fleche Prec
        {
            get { return prec; }
            set { prec = value; }
        }
        public Maillon_bi()
        {

        }
        public Maillon_bi(int val, double coordx, double coordy, double height, double width, SolidColorBrush couleurFond, SolidColorBrush couleurBordure, double epaisseurBord, int typefleche_Prec, int typefleche_Suiv, int typebout_Prec, int typebout_Suiv, double tailleFleche)
        //constructeur de maillon bidirectionnelle qui initalise tout ces attibuts 
        {
            champ = new Rectangle();// création d'un rectangle 
            Canvas.SetLeft(champ, coordx);//positionnement du rectangle
            Canvas.SetTop(champ, coordy);
            cas = new Case(val, coordx + (width / 5), coordy, 1, height, width - 2 * (width / 5), Brushes.White, couleurBordure, epaisseurBord);
            adr = new Fleche(coordx + width - 5, coordy + (height / 3), Brushes.Black, tailleFleche, typefleche_Suiv, typebout_Suiv);
            prec = new Fleche(coordx + 5, coordy + (2 * height / 3), Brushes.Black, tailleFleche, typefleche_Prec, typebout_Prec);
            champ.Width = width;//initialisation de la taille
            champ.Height = height;
            champ.Fill = couleurFond;//initialisation du fond
            champ.Stroke = couleurBordure;
            champ.StrokeThickness = epaisseurBord;
            this.champ.RadiusX = 5;//initialisation du degré de courbure 
            this.champ.RadiusY = 5;
        }

        public override void deplacer(Point[] tabPoint, int nbPoint)
        {
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            int ix = 1, iy = 1;
            Canvas c = new Canvas();
            dax.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des x
            day.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des y
            dax.From = Canvas.GetLeft(champ); // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = tabPoint[0].X; // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            Canvas.SetLeft(champ, tabPoint[0].X); // sauvgarde de la nouvelle coordonné par rapport au axe des x du 1er dépalcement
            day.From = Canvas.GetTop(champ); // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = tabPoint[0].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            Canvas.SetTop(champ, tabPoint[0].Y); // sauvgarde de la nouvelle coordonné par rapport au axe des y du 1er dépalcement
            dax.Completed += (s, e) =>  // méthode à executer une fois le déplacment par rapport au axe x terminé
            {
                if (ix < nbPoint)  // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des x
                {
                    dax.From = Canvas.GetLeft(champ); // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax.To = tabPoint[ix].X; // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement
                    Canvas.SetLeft(champ, tabPoint[ix].X); // sauvgarde de la nouvelle coordonné par rapport au axe des x du (ix+1) eme dépalcement
                    this.champ.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du (ix+1) eme déplacement par rapport au axe des x
                    ix++;
                }
            };
            day.Completed += (s1, e1) =>
            {
                if (iy < nbPoint) // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des y
                {
                    day.From = Canvas.GetTop(champ); // affectation de la coordonné de départ par rapport au axe des y du (iy+1) eme dépalcement
                    day.To = tabPoint[iy].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du (iy+1) eme dépalcement
                    Canvas.SetTop(champ, tabPoint[iy].Y); // sauvgarde de la nouvelle coordonné par rapport au axe des y du (iy+1) eme dépalcement
                    this.champ.BeginAnimation(Canvas.TopProperty, day); // effectuation du (iy+1) eme déplacement par rapport au axe des y                                           
                    iy++;
                }
            };

            this.champ.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du 1er déplacement par rapport au axe des x   
            this.champ.BeginAnimation(Canvas.TopProperty, day); //  effectuation du 1er déplacement par rapport au axe des y
            Point[] tabPoint2 = new Point[nbPoint];
            for (int i = 0; i < nbPoint; i++)
            {
                tabPoint2[i].X = tabPoint[i].X + this.Width / 5;
                tabPoint2[i].Y = tabPoint[i].Y;
            }
            this.cas.deplacer(tabPoint2, nbPoint);
            this.CoordX = tabPoint[nbPoint - 1].X;
            this.CoordY = tabPoint[nbPoint - 1].Y;
        }

        public async void afficher(Canvas c) //Affiche un maillon
        {
            base.afficher(c);
            await prec.dessiner(1,c);
        }


        public async override void appear(Canvas c) //Faire apparaitre un maillon
        {
            base.appear(c);
            this.prec.dessiner(Temps.time,c);//Dessiner la fléche du précedent 
        }
    }
}
