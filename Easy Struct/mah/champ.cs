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
    class Champ: Temps
    {
        private Shape forme;
        private double coordX;
        private double coordY;
        private double height;
        private double width;
        private SolidColorBrush backgroundColor;
        private SolidColorBrush borderColor;
        private double borderThick;

        public Champ()
        {

        }
        public Champ(double coordx, double coordy, int typeForme, double height, double width, SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
        {
            if (typeForme == 1) this.forme = new Rectangle();
            else if (typeForme == 2) this.forme = new Ellipse();

            Canvas.SetLeft(this.forme, coordx);
            this.coordX = coordx;

            Canvas.SetTop(this.forme, coordy);
            this.coordY = coordy;

            this.forme.Height = height;
            this.height = height;

            this.forme.Width = width;
            this.width = width;

            this.forme.Fill = bgColor;
            this.backgroundColor = bgColor;

            this.forme.Stroke = brdColor;
            this.borderColor = brdColor;

            this.forme.StrokeThickness = brdThick;
            this.borderThick = brdThick;

        }

        public double CoordX
        {
            get { return coordX; }
            set
            {
                coordX = value;
                Canvas.SetLeft(this.forme, value);
            }
        }

        public double CoordY
        {
            get { return coordY; }
            set
            {
                coordY = value;
                Canvas.SetTop(this.forme, value);
            }
        }

        public Shape Forme
        {
            get { return forme; }
            set { forme = value; }
        }
        public double BorderThick
        {
            get { return borderThick; }
            set
            {
                borderThick = value;
                this.forme.StrokeThickness = value;
            }
        }


        public SolidColorBrush BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.forme.Stroke = value;
            }
        }


        public SolidColorBrush BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                this.forme.Fill = value;
            }
        }
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                this.forme.Width = value;
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                this.forme.Height = value;
            }
        }

        public void colorChamp(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
        {
            //Change la couleur du champ
            this.forme.Fill = bgColor;
            this.backgroundColor = bgColor;
            this.forme.Stroke = brdColor;
            this.borderColor = brdColor;
            this.forme.StrokeThickness = brdThick;
            this.borderThick = brdThick;
        }

        public async virtual void clignoter(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick, int nbfois)
        {
            //faire clignoter un champ l'idée consiste a faire diminuer opacité en premier lieu trés rapidement
            // apres faire elever cette derniere avec une duré =time

            DoubleAnimation aff1 = new DoubleAnimation(0, TimeSpan.FromSeconds(0));//Double animation 'le zero dans les paramatres' veut dire que la forme va etre invisible 
            //initialisation des nouveaux couleur                                                                        
            this.forme.Fill = bgColor;
            this.backgroundColor = bgColor;
            this.forme.Stroke = brdColor;
            this.borderColor = brdColor;
            this.forme.StrokeThickness += brdThick;
            this.borderThick += brdThick;

            for (int i = 0; i < nbfois; i++)
            {//clignoter nbfois

                aff1.Completed += (s, e) =>
                {
                    DoubleAnimation aff2 = new DoubleAnimation(1, TimeSpan.FromSeconds(time));
                    this.forme.BeginAnimation(Rectangle.OpacityProperty, aff2);//Debut de l'animation        

                };
                this.forme.BeginAnimation(Rectangle.OpacityProperty, aff1);
                await Task.Delay(TimeSpan.FromSeconds(.7));
            }

        }
        public async virtual void vibrate(int nbfois, double angle)
        {
            //faire vibrer une forme vers la gauche  court delai ensuite vers la droite dans court delai aussi 
            RotateTransform r2 = new RotateTransform();
            this.forme.RenderTransform = r2;
            this.forme.RenderTransformOrigin = new Point(.5, .5);//rotation apartit de centre de la forme
            DoubleAnimation z2 = new DoubleAnimation(-angle, TimeSpan.FromSeconds(0.5));// Double animation 'rotation de -(angle) ver le gacuhe '

            for (int i = 0; i < nbfois; i++)//boucle pour exécuter nbfois la vibration
            {

                z2.Completed += (s, em) =>//fin de rotation vers gacuhe
                {
                    DoubleAnimation z1 = new DoubleAnimation(+angle, TimeSpan.FromSeconds(0.5));// Double animation 'rotation de -(angle) ver la droite '
                    r2.BeginAnimation(RotateTransform.AngleProperty, z1);//debut de l'animation vers la droite
                };
                await Task.Delay(TimeSpan.FromSeconds(.7));
                r2.BeginAnimation(RotateTransform.AngleProperty, z2);//debut de l'animation vers le gacuhe
                await Task.Delay(TimeSpan.FromSeconds(.5));
            }
            DoubleAnimation z3 = new DoubleAnimation(0, TimeSpan.FromSeconds(0));// Double animation 'returne a l'origine'
            r2.BeginAnimation(RotateTransform.AngleProperty, z3);
        }


        public virtual void deplacer(Point[] tabPoint, int nbPoint)
        // Efféctue des deplacements au nbPoint qui sont sont contenus dans le tableau tabPoint
        // tels que entre un déplacement et un autre il ya une durée de "duration" en seconde
        {
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            int ix = 1, iy = 1;
            dax.Duration = TimeSpan.FromSeconds(time); // intialisation de la durée d'un déplacment sur l'axe des x
            day.Duration = TimeSpan.FromSeconds(time); // intialisation de la durée d'un déplacment sur l'axe des y
            dax.From = this.coordX; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = tabPoint[0].X; // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            this.coordX = tabPoint[0].X; // sauvgarde de la nouvelle coordonné par rapport au axe des x du 1er dépalcement
            day.From = this.coordY; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = tabPoint[0].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            this.coordY = tabPoint[0].Y; // sauvgarde de la nouvelle coordonné par rapport au axe des y du 1er dépalcement
            dax.Completed += (s, e) =>  // méthode à executer une fois le déplacment par rapport au axe x terminé
            {
                if (ix < nbPoint)  // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des x
                {
                    dax.From = this.coordX; // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax.To = tabPoint[ix].X; // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement
                    this.coordX = tabPoint[ix].X; // sauvgarde de la nouvelle coordonné par rapport au axe des x du (ix+1) eme dépalcement
                    this.forme.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du (ix+1) eme déplacement par rapport au axe des x
                    ix++;
                }

            };

            day.Completed += (s1, e1) =>
            {
                if (iy < nbPoint) // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des y
                {
                    day.From = this.coordY; // affectation de la coordonné de départ par rapport au axe des y du (iy+1) eme dépalcement
                    day.To = tabPoint[iy].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du (iy+1) eme dépalcement
                    this.coordY = tabPoint[iy].Y; // sauvgarde de la nouvelle coordonné par rapport au axe des y du (iy+1) eme dépalcement
                    this.forme.BeginAnimation(Canvas.TopProperty, day); // effectuation du (iy+1) eme déplacement par rapport au axe des y
                    iy++;
                }
            };
            this.forme.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du 1er déplacement par rapport au axe des x
            this.forme.BeginAnimation(Canvas.TopProperty, day); //  effectuation du 1er déplacement par rapport au axe des y

        }


        public virtual void afficher(Canvas c)
        // Affiche le champ dans le Canvas c
        {
            c.Children.Add(this.forme);
        }
        public virtual void masquer(Canvas c)
        // Enleve le champ du Canvas c
        {
            c.Children.Remove(this.forme);
        }


    }
}
