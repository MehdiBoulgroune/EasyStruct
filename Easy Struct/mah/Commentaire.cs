using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace mah
{
    class Commentaire
    {
        private Rectangle champ;
        private TextBlock textBlock;


        public double opacity
        {
            get { return champ.Opacity; }
            set { champ.Opacity = value; textBlock.Opacity = value; }
        }

        public double Height
        {
            get { return champ.Height; }
            set { champ.Height = value; textBlock.Height = value; }
        }


        public double Width
        {
            get { return champ.Width; }
            set { champ.Width = value; textBlock.Width = value; }
        }


        public double CoordX
        {
            get { return Canvas.GetLeft(champ); }
            set { Canvas.SetLeft(champ, value); Canvas.SetLeft(textBlock, value + 5); }
        }
        public double CoordY
        {
            get { return Canvas.GetTop(champ); }
            set { Canvas.SetTop(champ, value); Canvas.SetTop(textBlock, value + 5); }
        }


        public String Text
        {
            get { return textBlock.Text; }
            set { textBlock.Text = value; }
        }


        public SolidColorBrush CouleurText
        {
            get { return (SolidColorBrush)textBlock.Foreground; }
            set { textBlock.Foreground = value; }
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

        public Commentaire()
        {

        }

        public Commentaire(String text, SolidColorBrush couleurText, double coordx, double coordy, double width, double height, SolidColorBrush couleurFond, SolidColorBrush couleurBordure)

        {
            textBlock = new TextBlock();
            champ = new Rectangle();
            Canvas.SetLeft(champ, coordx);
            Canvas.SetTop(champ, coordy);
            Canvas.SetLeft(textBlock, coordx + 5);
            Canvas.SetTop(textBlock, coordy + 5);
            textBlock.Width = width;
            textBlock.Height = height;
            champ.Width = width;
            champ.Height = height;
            champ.Fill = couleurFond;
            champ.Stroke = couleurBordure;
            champ.Opacity = 0.7;
            textBlock.Text = text;
            textBlock.Foreground = couleurText;
            this.champ.RadiusX = 5;
            this.champ.RadiusY = 5;
        }

        public void ajouterCanvas(Canvas c)
        {
            c.Children.Add(this.champ);
            c.Children.Add(this.textBlock);
        }

        public void enleverCanvas(Canvas c)
        {
            c.Children.Remove(this.champ);
            c.Children.Remove(this.textBlock);
        }

        public void disparaitre(double time)
        {
            DoubleAnimation da1 = new DoubleAnimation();
            DoubleAnimation da2 = new DoubleAnimation();
            da1.From = 0.7;
            da1.To = 0;
            da1.Duration = TimeSpan.FromSeconds(time);
            da2.From = 1;
            da2.To = 0;
            da2.Duration = TimeSpan.FromSeconds(time);
            champ.BeginAnimation(Rectangle.OpacityProperty, da1);
            textBlock.BeginAnimation(TextBlock.OpacityProperty, da2);
        }

        public void apparaitre(double time)
        {

            DoubleAnimation da1 = new DoubleAnimation();
            DoubleAnimation da2 = new DoubleAnimation();
            da1.From = 0;
            da1.To = 0.7;
            da1.Duration = TimeSpan.FromSeconds(time);
            da2.From = 0;
            da2.To = 1;
            da2.Duration = TimeSpan.FromSeconds(time);
            champ.BeginAnimation(Rectangle.OpacityProperty, da1);
            textBlock.BeginAnimation(TextBlock.OpacityProperty, da2);
        }
    }
}
