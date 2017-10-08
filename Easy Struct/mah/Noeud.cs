using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace mah
{
    class Noeud : Champ
    {
        private TextBlock textBlock; // Champ ou écrire la valeur que contient le noeud
        private int valeur; // valaur que contient le noeud 
        public int Valeur
        {
            get { return valeur; }
            set
            {
                this.textBlock = new TextBlock();
                valeur = value;
                textBlock.FontSize = 15;
                textBlock.Text = value.ToString();
                Canvas.SetLeft(this.textBlock, CoordX + Width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5);
                Canvas.SetTop(this.textBlock, CoordY + Height / 2 - 10);
            }
        }

        public Noeud filsGauche { get; set; } // fils gauche du noeud   
        public Noeud filsDroit { get; set; } // fils drorit du neoud 
        public Noeud pere { get; set; } // pere du noeud
        public Lien lienGauche { get; set; } // lien gauche du noeud ( le lien qui le relie avec son fils gauche )
        public Lien lienDroit { get; set; }// lien droit du noeud ( le lien qui le relie avec son fils droit )
        public int Niveau { get; set; } // niveau du neoud 

        public Noeud() // constructeur vide du noeud 
        {

        }

        public Noeud(int valeur, double coordx, double coordy, double height, double width, SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick, double loungeurLien, double angle, int niveau, Noeud pere)
           : base(coordx, coordy, 2, height, width, bgColor, brdColor, brdThick)
        {
            this.textBlock = new TextBlock();
            this.Valeur = valeur;
            this.textBlock.Text = valeur.ToString();
            textBlock.FontSize = 15;
            Canvas.SetLeft(this.textBlock, coordx + width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5);
            Canvas.SetTop(this.textBlock, coordy + height / 2 - 10);
            filsGauche = null;
            filsDroit = null;
            lienDroit = new Lien(this.CoordX + this.Width / 2, this.CoordY + this.Height / 2, loungeurLien, angle, brdColor, brdThick);
            lienGauche = new Lien(this.CoordX + this.Width / 2, this.CoordY + this.Height / 2, loungeurLien, angle + (90 - angle) * 2, brdColor, brdThick);
            Niveau = niveau;
            this.pere = pere;
        }

        public override void deplacer(Point[] tabPoint, int nbPoint)
        {
            /*========================================= PARITE DEPALACMENT DU TEXTBLOCK =================================================*/
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x  
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            int ix = 1, iy = 1;
            double dx, dy;
            double a = this.Width / 2 - 5 - (Valeur.ToString().Length - 1) * 3.5, b = this.Height / 2 - 10;
            dax.Duration = TimeSpan.FromSeconds(time); // intialisation de la durée d'un déplacment sur l'axe des x 
            day.Duration = TimeSpan.FromSeconds(time); // intialisation de la durée d'un déplacment sur l'axe des y
            dax.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = tabPoint[0].X + a; // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            day.From = this.CoordY + b; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = tabPoint[0].Y + b; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            dx = tabPoint[0].X - this.CoordX;
            dy = tabPoint[0].Y - this.CoordY;
            lienGauche.deplacerX1Y1(lienGauche.Ligne.X1 + dx, lienGauche.Ligne.Y1 + dy);
            lienGauche.deplacerX2Y2(lienGauche.Ligne.X2 + dx, lienGauche.Ligne.Y2 + dy);
            lienDroit.deplacerX1Y1(lienDroit.Ligne.X1 + dx, lienDroit.Ligne.Y1 + dy);
            lienDroit.deplacerX2Y2(lienDroit.Ligne.X2 + dx, lienDroit.Ligne.Y2 + dy);
            lienGauche.CoordX1 = tabPoint[0].X + Width / 2;
            lienDroit.CoordX1 = tabPoint[0].X + Width / 2;
            lienGauche.CoordY1 = tabPoint[0].Y + Height / 2;
            lienDroit.CoordY1 = tabPoint[0].Y + Height / 2;
            dax.Completed += (s, e) =>  // méthode à executer une fois le déplacment par rapport au axe x terminé
            {
                if (ix < nbPoint)  // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des x
                {
                    dax.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax.To = tabPoint[ix].X + a; // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement
                    lienGauche.deplacerX1Y1(lienGauche.Ligne.X1 + dx, lienGauche.Ligne.Y1 + dy);
                    lienGauche.deplacerX2Y2(lienGauche.Ligne.X2 + dx, lienGauche.Ligne.Y2 + dy);
                    lienDroit.deplacerX1Y1(lienDroit.Ligne.X1 + dx, lienDroit.Ligne.Y1 + dy);
                    lienDroit.deplacerX2Y2(lienDroit.Ligne.X2 + dx, lienDroit.Ligne.Y2 + dy);
                    this.lienGauche.CoordX1 = tabPoint[ix].X + Width / 2;
                    this.lienDroit.CoordX1 = tabPoint[ix].X + Width / 2;
                    this.textBlock.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du (ix+1) eme déplacement par rapport au axe des x
                    ix++;
                }
            };

            day.Completed += (s1, e1) =>
            {
                if (iy < nbPoint) // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des y
                {
                    day.From = this.CoordY + b; // affectation de la coordonné de départ par rapport au axe des y du (iy+1) eme dépalcement
                    day.To = tabPoint[iy].Y + b; // affectation de la coordonné d'arrivée par rapport au axe des y du (iy+1) eme dépalcement
                    dx = tabPoint[ix].X - this.CoordX;
                    dy = tabPoint[iy].Y - this.CoordY;
                    this.lienGauche.CoordY1 = tabPoint[iy].Y + Height / 2;
                    this.lienDroit.CoordY1 = tabPoint[iy].Y + Height / 2;
                    this.textBlock.BeginAnimation(Canvas.TopProperty, day); // effectuation du (iy+1) eme déplacement par rapport au axe des y
                    iy++;
                }
            };
            this.textBlock.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du 1er déplacement par rapport au axe des x
            this.textBlock.BeginAnimation(Canvas.TopProperty, day); //  effectuation du 1er déplacement par rapport au axe des y

            /*===========================================================================================================================*/


            /*======================================== PARITE DEPALACMENT DE LA FORME  =================================================*/
            base.deplacer(tabPoint, nbPoint); // Deplacement de la form qui contient la valeur
            /*===========================================================================================================================*/


        }




        public void afficher(Canvas c)
            // Affiche le noeud
        {
            lienGauche.afficher(c);
            lienDroit.afficher(c);
            if (filsDroit == null) this.lienDroit.Ligne.Opacity = 0;
            if (filsGauche == null) this.lienGauche.Ligne.Opacity = 0;
            Canvas.SetZIndex(lienGauche.Ligne, 1);
            Canvas.SetZIndex(lienDroit.Ligne, 1);
            base.afficher(c);
            Canvas.SetZIndex(this.Forme, 2);
            c.Children.Add(textBlock);
            Canvas.SetZIndex(textBlock, 3);
        }

        public async Task masquer(Canvas c)
            // Masque le noeud
        {
            if (filsDroit != null) lienDroit.masquer(c);
            if (filsGauche != null) lienGauche.masquer(c);
            base.masquer(c);
            c.Children.Remove(textBlock);
        }

        public void repositionner(double coordX, double coordY)
            // repostionne le noeud
        {
            this.CoordX = coordX;
            this.CoordY = coordY;
            Canvas.SetLeft(this.textBlock, coordX + this.Width / 2 - 5 - (Valeur.ToString().Length - 1) * 3.5);
            Canvas.SetTop(this.textBlock, coordY + this.Height / 2 - 10);
            this.lienGauche.CoordX1 = coordX + this.Width / 2;
            this.lienGauche.CoordY1 = coordY + this.Height / 2;
            this.lienDroit.CoordX1 = coordX + this.Width / 2;
            this.lienDroit.CoordY1 = coordY + this.Height / 2;

        }

        public void elargirSansAnim(double newAngle, double newLongeur)
            // elargie les liens du noeud sans animation
        {
            this.lienDroit.Angle = newAngle;
            this.lienGauche.Angle = newAngle + (90 - newAngle) * 2;
            this.lienDroit.Longeur = newLongeur;
            this.lienGauche.Longeur = newLongeur;
        }
    }
}
