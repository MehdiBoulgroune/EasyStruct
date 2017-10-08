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
    class Lien :  Temps // C'est le lien qui relie entre les noeuds des arbres
    {
        private Line ligne;

        public Line Ligne
        {

            get { return ligne; }

        }
        public double Epaisseur
        {
            get { return ligne.StrokeThickness; }
            set { ligne.StrokeThickness = value; }
        }


        public SolidColorBrush Couleur // Couleur du lien
        {
            get { return (SolidColorBrush)ligne.Stroke; }
            set { ligne.Stroke = value; }
        }

        public double CoordX1 
        {
            get { return ligne.X1; }
            set
            {
                ligne.X1 = value;
                ligne.X2 = longeur * Math.Cos(this.angle * Math.PI / 180) + ligne.X1;
            }
        }

        public double CoordY1
        {
            get { return ligne.Y1; }
            set
            {
                ligne.Y1 = value;
                this.ligne.Y2 = longeur * Math.Sin(this.angle * Math.PI / 180) + ligne.Y1;
            }
        }

        public double CoordX2
        {
            get { return ligne.X2; }
            set
            {
                ligne.X2 = value;
            }

        }

        public double CoordY2
        {
            get { return ligne.Y2; }
            set
            {
                ligne.Y2 = value;
            }
        }

        private double angle;

        public double Angle // Angle de positionnement du lien 
        {
            get { return angle; }
            set
            {
                angle = value;
                this.ligne.X2 = longeur * Math.Cos(this.angle * Math.PI / 180) + ligne.X1;
                this.ligne.Y2 = longeur * Math.Sin(this.angle * Math.PI / 180) + ligne.Y1;
            }


        }

        private double longeur; // Longeur du lien 

        public double Longeur
        {
            get { return longeur; }
            set
            {
                longeur = value;
                this.ligne.X2 = value * Math.Cos(this.angle * Math.PI / 180) + this.ligne.X1;
                this.ligne.Y2 = value * Math.Sin(this.angle * Math.PI / 180) + this.ligne.Y1;
            }
        }

        public Lien()
        {
            ligne = new Line();
        }

        public Lien(double coordX, double coordY, double longeur, double angle, SolidColorBrush couleur, double epaisseur)
        {
            ligne = new Line();
            this.angle = angle;
            this.CoordX1 = coordX;
            this.CoordY1 = coordY;
            this.Couleur = couleur;
            this.Epaisseur = epaisseur;
            this.Longeur = longeur;


        }


        public void afficher(Canvas c) // Affiche le lien 
        {
            c.Children.Add(this.ligne);
        }
        public void masquer(Canvas c) // masque le lien de l'animation
        {
            c.Children.Remove(this.ligne);
        }
        public async Task ChangeLongeurAnim(double newLongeur) // change la longeur du lien
        {
            DoubleAnimation daX2 = new DoubleAnimation();
            DoubleAnimation daY2 = new DoubleAnimation();
            daX2.Duration = TimeSpan.FromSeconds(time);
            daY2.Duration = TimeSpan.FromSeconds(time);
            daX2.From = ligne.X2;
            daX2.To = newLongeur * Math.Cos(this.angle * Math.PI / 180) + ligne.X1;
            daY2.From = ligne.Y2;
            daY2.To = newLongeur * Math.Sin(this.angle * Math.PI / 180) + ligne.Y1;
            ligne.BeginAnimation(Line.X2Property, daX2);
            ligne.BeginAnimation(Line.Y2Property, daY2);
            await Task.Delay(TimeSpan.FromSeconds(time));
            this.longeur = newLongeur;
        }

        public async Task rotationAnim(double newAngle) // fait  une rotation animé du lien 
        {
            DoubleAnimation da = new DoubleAnimation();
            RotateTransform rt = new RotateTransform();
            da.From = 0;
            angle = newAngle - angle;
            da.To = angle;
            da.Duration = TimeSpan.FromSeconds(time);
            ligne.RenderTransform = rt;
            rt.CenterX = ligne.X1;
            rt.CenterY = ligne.Y1;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
            da.Completed += (s, e) =>
            {
                ligne.X2 = longeur * Math.Sin(this.angle * Math.PI / 180) + ligne.X1;
                ligne.Y2 = longeur * Math.Sin(this.angle * Math.PI / 180) + ligne.Y1;
            };
            await Task.Delay(TimeSpan.FromSeconds(time));
        }

        public async Task deplacerX2Y2(double ancienCoordX2, double ancienCoordY2, double NouvCoordX2, double NouvCoordY2)
            // Déplace le point M2(x2,y2) du lien
        {
            DoubleAnimation daX2 = new DoubleAnimation(ancienCoordX2, NouvCoordX2, TimeSpan.FromSeconds(time));
            DoubleAnimation daY2 = new DoubleAnimation(ancienCoordY2, NouvCoordY2, TimeSpan.FromSeconds(time));
            this.ligne.BeginAnimation(Line.X2Property, daX2);
            this.ligne.BeginAnimation(Line.Y2Property, daY2);
            await Task.Delay(TimeSpan.FromSeconds(time));
            this.ligne.X2 = NouvCoordX2;
            this.ligne.Y2 = NouvCoordY2;
        }

        public async Task deplacerX1Y1(double ancienCoordX1, double ancienCoordY1, double NouvCoordX1, double NouvCoordY1)
        // Déplace le point M2(x1,y1) du lien
        {
            DoubleAnimation daX1 = new DoubleAnimation(ancienCoordX1, NouvCoordX1, TimeSpan.FromSeconds(time));
            DoubleAnimation daY1 = new DoubleAnimation(ancienCoordY1, NouvCoordY1, TimeSpan.FromSeconds(time));
            this.ligne.BeginAnimation(Line.X2Property, daX1);
            this.ligne.BeginAnimation(Line.Y2Property, daY1);
            await Task.Delay(TimeSpan.FromSeconds(time));
            this.CoordX1 = NouvCoordX1;
            this.CoordY1 = NouvCoordY1;
        }

        public async Task deplacerX1Y1(double NouvCoordX1, double NouvCoordY1)
        // Déplace le point M2(x1,y1) du lien
        {
            DoubleAnimation daX1 = new DoubleAnimation(this.ligne.X1, NouvCoordX1, TimeSpan.FromSeconds(time));
            DoubleAnimation daY1 = new DoubleAnimation(this.ligne.Y1, NouvCoordY1, TimeSpan.FromSeconds(time));
            this.ligne.BeginAnimation(Line.X1Property, daX1);
            this.ligne.BeginAnimation(Line.Y1Property, daY1);
        }

        public async Task deplacerX2Y2(double NouvCoordX2, double NouvCoordY2)
        // Déplace le point M2(x2,y2) du lien
        {
            DoubleAnimation daX2 = new DoubleAnimation(this.ligne.X2, NouvCoordX2, TimeSpan.FromSeconds(time));
            DoubleAnimation daY2 = new DoubleAnimation(this.ligne.Y2, NouvCoordY2, TimeSpan.FromSeconds(time));
            this.ligne.BeginAnimation(Line.X2Property, daX2);
            this.ligne.BeginAnimation(Line.Y2Property, daY2);
        }
    }
}
