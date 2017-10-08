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
    class Bout //Le bout de la fléche d'un maillon 
    {
        private Line[] l; //Tableau de ligne pour construire le bout
        private double CoordX; //Coordonnée du bout dans le canvas 
        private double CoordY;
        private int typeBout;
        public int TypeBout
        {
            get { return typeBout; }
            set { typeBout = value; }
        }
        public double coordX
        {
            get { return CoordX; }
        }
        public double coordY
        {
            get { return CoordY; }
        }
        public Bout(double Coordx, double Coordy, SolidColorBrush Color, int typeBout, double thick)
            //Constructeur de bout qui initialise ces champs
        {
            this.CoordX = Coordx;
            this.CoordY = Coordy;
            this.typeBout = typeBout;
            l = new Line[5];
            l[0] = new Line();
            l[1] = new Line();
            l[2] = new Line();
            l[3] = new Line();
            l[4] = new Line();
            if (typeBout == 1)// Bout ->
            {

                l[0].Stroke = Color;
                l[0].StrokeThickness = thick;
                Canvas.SetLeft(l[0], CoordX);
                l[0].X2 = 10;
                Canvas.SetTop(l[0], CoordY);

                l[1].Stroke = Color;
                l[1].StrokeThickness = thick;
                Canvas.SetLeft(l[1], CoordX);
                l[1].X2 = 10;
                Canvas.SetTop(l[1], CoordY);
                l[1].Y1 = -5;

                l[2].Stroke = Color;
                l[2].StrokeThickness = thick;
                Canvas.SetLeft(l[2], CoordX);
                l[2].X2 = 10;
                Canvas.SetTop(l[2], CoordY);
                l[2].Y1 = 5;
            }

            if (typeBout == 2)//Bout <-
            {
                l[0].Stroke = Color;
                l[0].StrokeThickness = thick;
                Canvas.SetLeft(l[0], CoordX);
                l[0].X2 = -10;
                Canvas.SetTop(l[0], CoordY);

                l[1].Stroke = Color;
                l[1].StrokeThickness = thick;
                Canvas.SetLeft(l[1], CoordX);
                l[1].X1 = -10;
                Canvas.SetTop(l[1], CoordY);
                l[1].Y2 = -5;

                l[2].Stroke = Color;
                l[2].StrokeThickness = thick;
                Canvas.SetLeft(l[2], CoordX);
                l[2].X1 = -10;
                Canvas.SetTop(l[2], CoordY);
                l[2].Y2 = 5;
            }
            if (typeBout == 3)//NIL
            {
                l[0].Stroke = Color;
                l[0].StrokeThickness = thick;
                Canvas.SetLeft(l[0], CoordX);
                Canvas.SetTop(l[0], CoordY);
                l[0].Y2 = 10;

                l[1].Stroke = Color;
                l[1].StrokeThickness = thick;
                Canvas.SetLeft(l[1], CoordX);
                l[1].X1 = -10;
                l[1].X2 = 10;
                Canvas.SetTop(l[1], CoordY);
                l[1].Y1 = 10;
                l[1].Y2 = 10;

                l[2].Stroke = Color;
                l[2].StrokeThickness = thick;
                Canvas.SetLeft(l[2], CoordX);
                l[2].X1 = -5;
                Canvas.SetTop(l[2], CoordY);
                l[2].Y1 = 10;
                l[2].Y2 = 20;

                l[3].Stroke = Color;
                l[3].StrokeThickness = thick;
                Canvas.SetLeft(l[3], CoordX);
                l[3].X2 = 5;
                Canvas.SetTop(l[3], CoordY);
                l[3].Y1 = 10;
                l[3].Y2 = 20;

                l[4].Stroke = Color;
                l[4].StrokeThickness = thick;
                Canvas.SetLeft(l[4], CoordX);
                l[4].X1 = 5;
                l[4].X2 = 10;
                Canvas.SetTop(l[4], CoordY);
                l[4].Y1 = 10;
                l[4].Y2 = 20;
            }


        }
        public void afficher(Canvas c) //Afficher le bout 
        {
            int i;
            for (i = 0; i <= 4; i++)
            {
                c.Children.Remove(l[i]);
                try { c.Children.Add(l[i]); }
                catch { }
            }
        }


        public void masquer(Canvas c) //Afficher le bout 
        {
            int i;
            for (i = 0; i <= 4; i++)
            {
                c.Children.Remove(l[i]);
            }
        }


        public void deplacer(double coordX, double coordY, double duration)
        {
            int i;
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            dax.Duration = TimeSpan.FromSeconds(duration); // intialisation de la durée d'un déplacment sur l'axe des x
            day.Duration = TimeSpan.FromSeconds(duration); // intialisation de la durée d'un déplacment sur l'axe des y
            dax.From = this.CoordX; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = coordX; // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            this.CoordX = coordX; // sauvgarde de la nouvelle coordonné par rapport au axe des x du 1er dépalcement
            day.From = this.CoordY; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = coordY; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            this.CoordY = coordY; // sauvgarde de la nouvelle coordonné par rapport au axe des y du 1er dépalcement
            for (i = 0; i < l.Length; i++)
            {
                this.l[i].BeginAnimation(Canvas.LeftProperty, dax); // effectuation du 1er déplacement par rapport au axe des x
                this.l[i].BeginAnimation(Canvas.TopProperty, day); //  effectuation du 1er déplacement par rapport au axe des y            
            }
        }


        public void rotation(double Toangle)
        {
            RotateTransform rt = new RotateTransform(Toangle);
            for (int i = 0; i < l.Length; i++) l[i].RenderTransform = rt;
        }
    }
}
