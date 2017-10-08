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
    class Fleche
    {
        private double coordX; //Coordonée de la fléche dans le canvas 
        private double coordY;
        double[] height = new double[7];
        double[] width = new double[7];
        private Line[] l = new Line[7];
        private int typeFleche; 
        private int nbLines = 7;
        public Bout bout;
        private SolidColorBrush color;
        private double strokeThick = 1;

        public SolidColorBrush Color
        {
            get { return color; }
            set
            {
                color = value;
                for (int i = 0; i < nbLines; i++) l[i].Stroke = value;
            }
        }
        public Line L
        {
            get { return l[0]; }
        }

        public double Height
        {
            get { return height[0]; }
            set { height[0] = value; }
        }
        public int typefleche
        {
            get { return typeFleche; }
            set { typeFleche = value; }
        }
        public double StrokeThick
        {
            get { return strokeThick; }
            set
            {
                strokeThick = value;
                for (int i = 0; i < nbLines; i++)
                { l[i].StrokeThickness = value; }
            }
        }

        public double CoordX
        {
            get { return coordX; }
            set
            {
                coordX = value;
                l[0].X1 = value;
            }
        }

        public double CoordY
        {
            get { return coordY; }
            set
            {
                coordY = value;
                l[0].Y1 = value;
            }
        }
        public Fleche(int nbLines)
        {
            bout = new Bout(0, 0, color, 1, strokeThick);
            this.nbLines = nbLines;
            for (int i = 0; i < nbLines; i++) l[i] = new Line();
        }
        public Fleche(double Coordx, double Coordy, SolidColorBrush Color, double taille, int typeFleche, int typebout)
        {
            this.coordX = Coordx;
            this.coordY = Coordy;
            this.typeFleche = typeFleche;
            this.color = Color;
            for (int i = 0; i < nbLines; i++)
            {
                l[i] = new Line();
            }
            if (typeFleche == 1)//Fléche --->
            {
                l[0].Stroke = Color;
                l[0].X1 = CoordX;
                l[0].Y1 = CoordY;
                height[0] = taille;
                height[1] = 0;
                height[2] = 0;
                width[0] = 0;
                width[1] = 0;
                width[2] = 0;
                bout = new Bout(Coordx + height[0], Coordy + width[0], Color, typebout, strokeThick);
                this.nbLines = 1;

            }
            if (typeFleche == 2)// Fléche <---
            {
                l[0].Stroke = Color;
                l[0].X1 = CoordX;
                l[0].Y1 = CoordY;
                height[0] = -taille;
                height[1] = 0;
                height[2] = 0;
                width[0] = 0;
                width[1] = 0;
                width[2] = 0;
                bout = new Bout(Coordx + height[0], Coordy + width[0], Color, typebout, strokeThick);
                this.nbLines = 1;
            }
            if (typeFleche == 3)//Courbé bout à droite
            {
                l[0].Stroke = Color;
                l[0].X1 = CoordX;
                l[0].Y1 = CoordY;
                height[0] = 22;
                width[0] = 0;

                l[1].Stroke = Color;
                l[1].X1 = CoordX + height[0];
                l[1].Y1 = CoordY + width[0];
                height[1] = 0;
                width[1] = -50;

                l[2].Stroke = Color;
                l[2].X1 = CoordX + height[0] + height[1];
                l[2].Y1 = CoordY + width[0] + width[1];
                height[2] = 145;
                width[2] = 0;

                l[3].Stroke = Color;
                l[3].X1 = CoordX + height[0] + height[1] + height[2];
                l[3].Y1 = CoordY + width[0] + width[1] + width[2];
                height[3] = 0;
                width[3] = 40;

                l[4].Stroke = Color;
                l[4].X1 = CoordX + height[0] + height[1] + height[2] + height[3];
                l[4].Y1 = CoordY + width[0] + width[1] + width[2] + width[3];
                height[4] = 18;
                width[4] = 0;

                bout = new Bout(Coordx + height[0] + height[1] + height[2] + height[3] + height[4], Coordy + width[0] + width[1] + width[2] + width[3] + width[4], Color, typebout, strokeThick);
                this.nbLines = 5;

            }
            if (typeFleche == 4)//Coubé bout à gauche
            {
                l[0].Stroke = Color;
                l[0].X1 = CoordX;
                l[0].Y1 = CoordY;
                height[0] = -22;
                width[0] = 0;

                l[1].Stroke = Color;
                l[1].X1 = CoordX + height[0];
                l[1].Y1 = CoordY + width[0];
                height[1] = 0;
                width[1] = 50;

                l[2].Stroke = Color;
                l[2].X1 = CoordX + height[0] + height[1];
                l[2].Y1 = CoordY + width[0] + width[1];
                height[2] = -145;
                width[2] = 0;

                l[3].Stroke = Color;
                l[3].X1 = CoordX + height[0] + height[1] + height[2];
                l[3].Y1 = CoordY + width[0] + width[1] + width[2];
                height[3] = 0;
                width[3] = -40;

                l[4].Stroke = Color;
                l[4].X1 = CoordX + height[0] + height[1] + height[2] + height[3];
                l[4].Y1 = CoordY + width[0] + width[1] + width[2] + width[3];
                height[4] = -18;
                width[4] = 0;

                bout = new Bout(Coordx + height[0] + height[1] + height[2] + height[3] + height[4], Coordy + width[0] + width[1] + width[2] + width[3] + width[4], Color, typebout, strokeThick);
                this.nbLines = 5;

            }
            if (typeFleche == 5)
            {
                l[0].Stroke = Color;
                l[0].X1 = CoordX;
                l[0].Y1 = CoordY;
                height[0] = 0;
                width[0] = 60;

                l[1].Stroke = Color;
                l[1].X1 = CoordX + height[0];
                l[1].Y1 = CoordY + width[0];
                height[1] = -taille;
                width[1] = 0;

                l[2].Stroke = Color;
                l[2].X1 = CoordX + height[0] + height[1];
                l[2].Y1 = CoordY + width[0] + width[1];
                height[2] = 0;
                width[2] = -60;

                bout = new Bout(Coordx + height[0] + height[1] + height[2], Coordy + width[0] + width[1] + width[2], Color, typebout, strokeThick);
                this.nbLines = 3;
            }
        }
        public async Task dessiner(double time,Canvas c)//Dessiner une fléche 
        {
            for (int i = 0; i < nbLines; i++)
            {
                l[i].X2 = l[i].X1; 
                l[i].Y2 = l[i].Y1;
                try { c.Children.Add(l[i]); }
                catch { }
                DoubleAnimation dx = new DoubleAnimation(l[i].X1 + height[i], TimeSpan.FromSeconds(time / nbLines));
                dx.From = l[i].X1; // coordonnée du début de l'animation
                DoubleAnimation dy = new DoubleAnimation(l[i].Y1 + width[i], TimeSpan.FromSeconds(time / nbLines));
                dy.From = l[i].Y1; //coordonnée du début de l'animation
                l[i].BeginAnimation(Line.X2Property, dx);//lancement de l'animation
                l[i].BeginAnimation(Line.Y2Property, dy);//lancement de l'animation
                await Task.Delay(TimeSpan.FromSeconds(time / nbLines));
            }
            this.bout.afficher(c);//affichage du bout
        }

        public async Task decalAr(Fleche flecheFin, Canvas c)//Décaler une fléche en arrière 
        {

            int i = 0;
            DoubleAnimation dx, dy;
            this.bout.masquer(c);//masquage du bout
            for (i = nbLines - 1; i >= flecheFin.nbLines + 1; i--)
            {
                c.Children.Remove(l[i]); 
                l[i].X2 = l[i].X1;
                l[i].Y2 = l[i].Y1;
                dx = new DoubleAnimation(l[i].X2 - height[i], TimeSpan.FromSeconds(Temps.time / (nbLines - flecheFin.nbLines)));
                dy = new DoubleAnimation(l[i].Y2 - width[i], TimeSpan.FromSeconds(1));
                l[i].BeginAnimation(Line.X2Property, dx);//début de l'animation
                l[i].BeginAnimation(Line.Y2Property, dy);//début de l'animation
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
            i--;
            if (this.typefleche == 9 && flecheFin.typefleche == 3)
            { dx = new DoubleAnimation(l[i].X2 - ((height[i]) / 3), TimeSpan.FromSeconds(Temps.time / (nbLines - flecheFin.nbLines))); }
            else
            { dx = new DoubleAnimation(l[i].X2 - height[i], TimeSpan.FromSeconds(Temps.time / (nbLines - flecheFin.nbLines))); }

            dy = new DoubleAnimation(l[i].Y2 - width[i], TimeSpan.FromSeconds(1));
            l[i].BeginAnimation(Line.X2Property, dx);//début de l'animation
            l[i].BeginAnimation(Line.Y2Property, dy);//début de l'animation
            await Task.Delay(TimeSpan.FromSeconds(0.2));
            flecheFin.dessiner(0.1, c);//dessiner la fléche de fin
            this.retirerCanvas(c);
        }
        public void decaler(double x1, double x2)//Décaler une fléche en avant
        {
            DoubleAnimation da1 = new DoubleAnimation(x1, TimeSpan.FromSeconds(Temps.time));
            DoubleAnimation da2 = new DoubleAnimation(x2, TimeSpan.FromSeconds(Temps.time));
            l[0].BeginAnimation(Line.X1Property, da1);//début de l'animation
            l[0].BeginAnimation(Line.X2Property, da2);//début de l'animation
            this.bout.deplacer(x2, l[0].Y2, Temps.time);//déplacer le bout 
            coordX = x1;
        }

        private void ajouterCanvas(Canvas c)//Ajouter une fléche au canvas
        {
            try { for (int i = 0; i < l.Length; i++) c.Children.Add(l[i]); }
            catch { }
        }

        public void retirerCanvas(Canvas c)//Retirer une fléche du canvas
        {
            try { for (int i = 0; i < l.Length; i++) c.Children.Remove(l[i]); }
            catch { }
            this.bout.masquer(c);
        }

    }
}
