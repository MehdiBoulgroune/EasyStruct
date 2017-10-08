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
    class Maillon
    {

        protected Rectangle champ;
        protected Case cas;
        protected Fleche adr;

        public Fleche Adr
        {
            get { return adr; }
            set { adr = value; }
        }

        public int Valeur
        {
            get { return cas.Valeur; }

        }
        public double Height
        {
            get { return champ.Height; }
            set { champ.Height = value; }
        }


        public double Width
        {
            get { return champ.Width; }
            set { champ.Width = value; }
        }


        public double CoordX
        {
            get { return Canvas.GetLeft(champ); }
            set { Canvas.SetLeft(champ, value); }
        }
        public double CoordY
        {
            get { return Canvas.GetTop(champ); }
            set { Canvas.SetTop(champ, value); }
        }

        public double EpaisseurBord
        {
            get { return (double)champ.StrokeThickness; }
            set { champ.StrokeThickness = value; }

        }

        public SolidColorBrush CouleurBordure
        {
            get { return (SolidColorBrush)champ.Stroke; }
            set { champ.Stroke = value; }
        }



        public SolidColorBrush CouleurFond
        {
            get { return (SolidColorBrush)champ.Fill; }
            set { champ.Fill = value; }
        }

        public double opacity
        {
            get { return champ.Opacity; }
            set { champ.Opacity = value; }
        }



        public Maillon()
        {

        }

        public Maillon(int val, double coordx, double coordy, double height, double width, SolidColorBrush couleurFond, SolidColorBrush couleurBordure, double epaisseurBord, int typefleche, int typebout, double tailleFleche)
         //Constructeur de maillon qui initialise tout ces attributs
        {
            champ = new Rectangle(); // création d'un rectangle 
            Canvas.SetLeft(champ, coordx);//positionnement du rectangle
            Canvas.SetTop(champ, coordy);
            cas = new Case(val, coordx, coordy, 1, height, width - (width / 5), Brushes.White, couleurBordure, epaisseurBord);
            adr = new Fleche(coordx + width - 5, coordy + (height / 2), Brushes.Black, tailleFleche, typefleche, typebout);
            champ.Width = width;//initialisation de la taille
            champ.Height = height;
            champ.Fill = couleurFond;//initialisation du fond 
            champ.Stroke = couleurBordure;
            champ.StrokeThickness = epaisseurBord;
            this.champ.RadiusX = 5;//initialisation du degré de courbure 
            this.champ.RadiusY = 5;

        }
        public async Task afficher(Canvas c)//affiche le maillon
        {
            c.Children.Remove(this.champ);//**
            c.Children.Add(this.champ);
            this.cas.afficher(c);
            await adr.dessiner(1, c);
        }

        public async void clignoter(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick, int nbfois)
        {
            //faire clignoter le maillon en faisant diminuer l'opacité rapidement
            // apres faire elever cette derniere avec une duré =time
            DoubleAnimation aff1 = new DoubleAnimation(0, TimeSpan.FromSeconds(0));//Double animation 'le zero dans les paramatres' veut dire que le maillon va etre invisible 
            this.champ.Opacity = 0.7;
            //initialisation des nouveaux couleur                                                                        
            this.champ.Fill = bgColor;
            this.CouleurFond = bgColor;
            this.champ.Stroke = brdColor;
            this.CouleurBordure = brdColor;
            this.champ.StrokeThickness = brdThick;
            this.EpaisseurBord = brdThick;
            for (int i = 0; i < nbfois; i++)
            {//clignoter nbfois
                aff1.Completed += (s, e) =>
                {
                    DoubleAnimation aff3 = new DoubleAnimation(1, TimeSpan.FromSeconds(Temps.time / 3));
                    DoubleAnimation aff2 = new DoubleAnimation(1, TimeSpan.FromSeconds(Temps.time / 3));
                    this.cas.Forme.BeginAnimation(Rectangle.OpacityProperty, aff3);//Debut de l'animation  
                    this.champ.BeginAnimation(Rectangle.OpacityProperty, aff2);//Debut de l'animation   
                };
                this.champ.BeginAnimation(Rectangle.OpacityProperty, aff1);
                this.cas.Forme.BeginAnimation(Rectangle.OpacityProperty, aff1);
                await Task.Delay(TimeSpan.FromSeconds(.7));
            }
        }

        public virtual void deplacer(Point[] tabPoint, int nbPoint)
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
            this.cas.deplacer(tabPoint, nbPoint);
            this.CoordX = tabPoint[nbPoint - 1].X;
            this.CoordY = tabPoint[nbPoint - 1].Y;

        }

        public void disappear(double time, Point[] tabPoint, int nbPoint)
        //Faire disparaitre un maillon tout en effectuant un deplacement. 
        {
            this.deplacer(tabPoint, nbPoint); // Deplacement du maillon
                                              //=====Animation de la disparition====
            DoubleAnimation disp = new DoubleAnimation(0, TimeSpan.FromSeconds(time * nbPoint));//Double animation 'le zero dans les paramatres' veut dire que la forme va etre invisible
            DoubleAnimation disp2 = new DoubleAnimation(0, TimeSpan.FromSeconds(time * nbPoint));
            this.champ.BeginAnimation(Shape.OpacityProperty, disp); // Effectuation de l'animation de réduction d'opacity sur la forme 
            this.cas.TextBLock.BeginAnimation(TextBlock.OpacityProperty, disp2); // Effectuation de l'animation de réduction d'opacity sur le textblock
            this.cas.Forme.BeginAnimation(Shape.OpacityProperty, disp2);
        }
        public async void vibrate(int nbfois, double angle)
        {
            //faire vibrer une forme vers la gauche  court delai ensuite vers la droite dans court delai aussi 
            RotateTransform r2 = new RotateTransform();
            this.champ.RenderTransform = r2;
            this.cas.Forme.RenderTransform = r2;
            this.champ.RenderTransformOrigin = new Point(.5, .5);//rotation apartit de centre de la forme
            this.cas.Forme.RenderTransformOrigin = new Point(.5, .5);
            DoubleAnimation z2 = new DoubleAnimation(-angle, TimeSpan.FromSeconds(0.5));// Double animation 'rotation de (angle) ver le gacuhe '
            for (int i = 0; i < nbfois - 1; i++)//boucle pour exécuter nbfois la vibration
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
        public void colorMaillon(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
        {
            //Change la couleur du maillon
            this.colorCase(bgColor, brdColor, brdThick);
            this.colorAdrSuiv(bgColor, brdColor, brdThick);
        }

        public void colorCase(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
        {
            this.cas.Forme.Fill = bgColor;
            this.cas.Forme.Stroke = brdColor;
            this.cas.Forme.StrokeThickness = brdThick;
        }
        public void colorAdrSuiv(SolidColorBrush bgColor, SolidColorBrush brdColor, double brdThick)
        {
            //Change la couleur du maillon
            this.champ.Fill = bgColor;
            this.champ.Stroke = brdColor;
            this.champ.StrokeThickness = brdThick;

        }

        public void ajouterCanvas(Canvas c)
        {
            c.Children.Remove(this.champ);
            c.Children.Remove(this.cas.TextBLock);
            c.Children.Remove(this.cas.Forme);

            c.Children.Add(this.champ);
            c.Children.Add(this.cas.Forme);
            c.Children.Add(this.cas.TextBLock);

        }

        public void enleverCanvas(Canvas c) //enlever le maillon du canvas
        {
            c.Children.Remove(this.champ);
            c.Children.Remove(this.cas.TextBLock);
            c.Children.Remove(this.cas.Forme);
        }
        public virtual async void appear(Canvas c)
        {
            ajouterCanvas(c);
            DoubleAnimation ap = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Temps.time * 3));//Double animation 'le zero dans les paramatres' veut dire que la forme va etre invisible
            DoubleAnimation ap1 = new DoubleAnimation(0, 0.7, TimeSpan.FromSeconds(Temps.time));
            this.cas.Forme.BeginAnimation(Shape.OpacityProperty, ap);
            this.champ.BeginAnimation(Shape.OpacityProperty, ap1); // Effectuation de l'animation de réduction d'opacity sur la forme 
            this.cas.TextBLock.BeginAnimation(TextBlock.OpacityProperty, ap); // Effectuation de l'animation de réduction d'opacity sur le textblock
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            await adr.dessiner(1,c);
        }
    }
}
