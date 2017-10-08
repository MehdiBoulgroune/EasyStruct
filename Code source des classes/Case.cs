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
    class Case : Champ
    {
        private TextBlock textBlock;
        private int valeur;


        public Case()
        {

        }

        public Case(int valeur, double coordx, double coordy, int typeForme, double height, double width, SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
            : base(coordx, coordy, typeForme, height, width, bgColor, brdColor, brdThick)
            //Constructeur de case qui initialise tout ces attributs
        {
            this.textBlock = new TextBlock();
            this.valeur = valeur;
            this.textBlock.Text = valeur.ToString();
            textBlock.FontSize = 15;
            Canvas.SetLeft(this.textBlock, coordx + width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5);
            Canvas.SetTop(this.textBlock, coordy + height / 2 - 10);
        }


        public int Valeur
        {
            get { return valeur; }
            set
            {
                valeur = value;
                this.textBlock.Text = value.ToString();
                // Repositionnement du texteBlock car on a consataté que en modifiant sa valeur sa position change aussi 
                Canvas.SetLeft(this.textBlock, this.CoordX + this.Width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5);
                Canvas.SetTop(this.textBlock, this.CoordY + this.Height / 2 - 10);
            }
        }
        public TextBlock TextBLock
        {
            get { return textBlock; }
            set { textBlock = value; }
        }

        public virtual new double CoordX
        {
            get { return base.CoordX; }
            set
            {
                base.CoordX = value;
                Canvas.SetLeft(this.textBlock, base.CoordX + this.Width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5);
            }
        }

        public new double CoordY
        {
            get { return base.CoordY; }
            set
            {
                base.CoordY = value;
                Canvas.SetTop(this.textBlock, base.CoordY + this.Height / 2 - 10);
            }
        }
        public override void deplacer(Point[] tabPoint, int nbPoint)
        {
            /*========================================= PARITE DEPALACMENT DU TEXTBLOCK =================================================*/
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x  
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            int ix = 1, iy = 1;
            double a = this.Width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5, b = this.Height / 2 - 10;
            dax.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des x 
            day.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des y
                                                             //  dax.From = Canvas.GetLeft(base.Forme) + base.Width / 5;
            dax.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = tabPoint[0].X + a; // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            day.From = this.CoordY + b; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = tabPoint[0].Y + b; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            dax.Completed += (s, e) =>  // méthode à executer une fois le déplacment par rapport au axe x terminé
            {
                if (ix < nbPoint)  // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des x
                {
                    dax.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax.To = tabPoint[ix].X + a; // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement
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

        public virtual void deplacer2(Point[] tabPoint, int nbPoint)
        // Efféctue des deplacements au nbPoint qui sont sont contenus dans le tableau tabPoint
        // tels que entre un déplacement et un autre il ya une durée de "duration" en seconde
        {
            DoubleAnimation dax = new DoubleAnimation(); // Double animation axe des x
            DoubleAnimation day = new DoubleAnimation(); // Double animation axe des y
            DoubleAnimation dax2 = new DoubleAnimation(); // Double animation axe des x
            DoubleAnimation day2 = new DoubleAnimation(); // Double animation axe des y
            int ix = 1, iy = 1;
            double a = this.Width / 2 - 5 - (valeur.ToString().Length - 1) * 3.5, b = this.Height / 2 - 10;
            dax.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des y
            dax2.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des x 
            day2.Duration = TimeSpan.FromSeconds(Temps.time); // intialisation de la durée d'un déplacment sur l'axe des y
            dax.From = this.CoordX; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax.To = tabPoint[0].X + (this.Width / 3.5); // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement

            day.From = this.CoordY; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day.To = tabPoint[0].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du 1er dépalcement
            dax2.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du 1er dépalcement
            dax2.To = tabPoint[0].X + a + (this.Width / 3.5); // affectation de la coordonné d'arrivée par rapport au axe des x du 1er dépalcement
            day2.From = this.CoordY + b; // affectation de la coordonné de départ par rapport au axe des y du 1er dépalcement
            day2.To = tabPoint[0].Y + b;
            this.CoordX = tabPoint[0].X + (this.Width / 3.5); // sauvgarde de la nouvelle coordonné par rapport au axe des x du 1er dépalcement
            this.CoordY = tabPoint[0].Y; // sauvgarde de la nouvelle coordonné par rapport au axe des y du 1er dépalcement
            dax.Completed += (s, e) =>  // méthode à executer une fois le déplacment par rapport au axe x terminé
            {
                if (ix < nbPoint)  // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des x
                {
                    dax.From = this.CoordX;  // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax.To = tabPoint[ix].X + (this.Width / 3.5); // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement

                    dax2.From = this.CoordX + a; // affectation de la coordonné de départ par rapport au axe des x du (ix+1) eme dépalcement
                    dax2.To = tabPoint[ix].X + +a + (this.Width / 3.5); // affectation de la coordonné d'arrivée par rapport au axe des x du (ix+1) eme dépalcement
                    this.CoordX = tabPoint[ix].X + (this.Width / 3.5);
                    this.textBlock.BeginAnimation(Canvas.LeftProperty, dax2);
                    this.Forme.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du (ix+1) eme déplacement par rapport au axe des x
                    ix++;
                }

            };

            day.Completed += (s1, e1) =>
            {
                if (iy < nbPoint) // si le déplacement ne s'est pas fait envers tout les points on continue le déplacement sur l'axe des y
                {
                    day.From = this.CoordY; // affectation de la coordonné de départ par rapport au axe des y du (iy+1) eme dépalcement
                    day.To = tabPoint[iy].Y; // affectation de la coordonné d'arrivée par rapport au axe des y du (iy+1) eme dépalcement

                    day2.From = this.CoordY + b; // affectation de la coordonné de départ par rapport au axe des y du (iy+1) eme dépalcement
                    day2.To = tabPoint[iy].Y + b; // affectation de la coordonné d'arrivée par rapport au axe des y du (iy+1) eme dépalcement
                    this.CoordY = tabPoint[iy].Y; // sauvgarde de la nouvelle coordonné par rapport au axe des y du (iy+1) eme dépalcement
                    this.textBlock.BeginAnimation(Canvas.TopProperty, day2);
                    this.Forme.BeginAnimation(Canvas.TopProperty, day); // effectuation du (iy+1) eme déplacement par rapport au axe des y
                    iy++;
                }
            };
            this.Forme.BeginAnimation(Canvas.LeftProperty, dax); // effectuation du 1er déplacement par rapport au axe des x
            this.Forme.BeginAnimation(Canvas.TopProperty, day); //  effectuation du 1er déplacement par rapport au axe des y
            this.textBlock.BeginAnimation(Canvas.LeftProperty, dax2); // effectuation du 1er déplacement par rapport au axe des x
            this.textBlock.BeginAnimation(Canvas.TopProperty, day2);
        }


        public void disappear(Point[] tabPoint, int nbPoint)
        //Faire disparaitre une case tout en effectuant un deplacement. 
        {
            this.deplacer(tabPoint, nbPoint); // Deplacement de la case 

            /*=====Animation de la disparition====*/
            DoubleAnimation disp = new DoubleAnimation(0, TimeSpan.FromSeconds(time * nbPoint));//Double animation 'le zero dans les paramatres' veut dire que la forme va etre invisible 
            this.Forme.BeginAnimation(Shape.OpacityProperty, disp); // Effectuation de l'animation de réduction d'opacity sur la forme 
            this.textBlock.BeginAnimation(TextBlock.OpacityProperty, disp); // Effectuation de l'animation de réduction d'opacity sur le textblock

        }


        public async Task appear(Point[] tabPoint, int nbPoint)
        {

            /*=====Animation de la disparition====*/

            this.deplacer(tabPoint, nbPoint);
            DoubleAnimation app = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Temps.time));
            this.textBlock.BeginAnimation(TextBlock.OpacityProperty, app);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
        }


        public override void afficher(Canvas c)
        {
            base.afficher(c);
            c.Children.Remove(this.textBlock);//***
            c.Children.Add(this.textBlock);
        }

        public override void masquer(Canvas c)
        {
            base.masquer(c);
            c.Children.Remove(this.textBlock);
        }


        public async override void clignoter(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick, int nbfois)
        {
            //faire clignoter un champ l'idée consiste a faire diminuer opacité en premier lieu trés rapidement
            // apres faire elever cette derniere avec une duré =time

            DoubleAnimation aff1 = new DoubleAnimation(0, TimeSpan.FromSeconds(0));//Double animation 'le zero dans les paramatres' veut dire que la forme va etre invisible 
            //initialisation des nouveaux couleur                                                                        
            base.Forme.Fill = bgColor;
            base.BackgroundColor = bgColor;
            base.Forme.Stroke = brdColor;
            base.BorderColor = brdColor;
            base.Forme.StrokeThickness = brdThick;
            base.BorderThick = brdThick;

            for (int i = 0; i < nbfois; i++)
            {//clignoter nbfois

                aff1.Completed += (s, e) =>
                {
                    DoubleAnimation aff2 = new DoubleAnimation(1, TimeSpan.FromSeconds(Temps.time));
                    base.Forme.BeginAnimation(Rectangle.OpacityProperty, aff2);//Debut de l'animation        

                };
                base.Forme.BeginAnimation(Rectangle.OpacityProperty, aff1);
                await Task.Delay(TimeSpan.FromSeconds(.7));
            }

        }
    }
}
