using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Threading;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Effects;
using WpfAnimatedGif;
using System.Text.RegularExpressions;

namespace mah
{
    /// <summary>
    /// Interaction logic for similationMatrices.xaml
    /// </summary>
    public partial class similationMatrices : MetroWindow

    {
        static double x = 300, y = 100;
        Matrice mat, mat2, produit, transposée;
        Commentaire comPrincipal = new Commentaire("", Brushes.Black, x - 280, y - 80, 255, 70, Brushes.Yellow, Brushes.Black);
        Case det = new Case(0, 1, 1, 1, 1, 1, Brushes.White, Brushes.Black, 1);



        #region declaration
        StringBuilder apropos = new StringBuilder();
        ScaleTransform st = new ScaleTransform();
        int menuutiliser = 0, algoutiliser = 0; int donnerentrer; int chargementdjafait = 0; int choixutuliser = 0;
        double time;
        Commentaire erreur = new Commentaire("", Brushes.Black, 50, 90, 200, 50, Brushes.Red, Brushes.Red);

        int[] tabl2 = new int[4];
        StackPanel stackPnl = new StackPanel();
        Image img = new Image();
        Boolean b = false;
        StackPanel stackPnl1 = new StackPanel();
        Image img1 = new Image();
        StackPanel stackPnl2 = new StackPanel();
        Image img2 = new Image();
        StackPanel stackPnl3 = new StackPanel();
        Image img3 = new Image();
        StackPanel stackPnl4 = new StackPanel();
        Image img4 = new Image();
        Boolean b1 = false;
        #endregion
        public similationMatrices()
        {

            InitializeComponent();


            #region initalisation des images 
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            stackPnl.Height = 26;
            stackPnl.Width = 55;
            stackPnl.Orientation = Orientation.Horizontal;
            stackPnl.Margin = new Thickness(6.3);
            stackPnl.Children.Add(img);
            affichermenubutton.Content = stackPnl;
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            src1.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src1.EndInit();
            img1.Source = src1;
            img1.Stretch = Stretch.UniformToFill;
            stackPnl1.Height = 26;
            stackPnl1.Width = 55;
            stackPnl1.Orientation = Orientation.Horizontal;
            stackPnl1.Margin = new Thickness(6.3);
            stackPnl1.Children.Add(img1);
            afficheralgobutton.Content = stackPnl1;
            stackPnl2.Height = 26;
            stackPnl2.Width = 55;
            stackPnl2.Orientation = Orientation.Horizontal;
            stackPnl2.Margin = new Thickness(6.3);
            stackPnl2.Children.Add(img2);



            BitmapImage src3 = new BitmapImage();
            src3.BeginInit();
            src3.UriSource = new Uri("Zoom IN.png", UriKind.Relative);
            src3.EndInit();
            img3.Source = src3;
            img3.Stretch = Stretch.UniformToFill;
            stackPnl3.Height = 19;
            stackPnl3.Width = 25;
            stackPnl3.Orientation = Orientation.Horizontal;
            stackPnl3.Margin = new Thickness(6.3);
            stackPnl3.Children.Add(img3);
            zoumavant.Content = stackPnl3;

            BitmapImage src4 = new BitmapImage();
            src4.BeginInit();
            src4.UriSource = new Uri("Zoom out.png", UriKind.Relative);
            src4.EndInit();
            img4.Source = src4;
            img4.Stretch = Stretch.UniformToFill;
            stackPnl4.Height = 19;
            stackPnl4.Width = 25;
            stackPnl4.Orientation = Orientation.Horizontal;
            stackPnl4.Margin = new Thickness(6.3);
            stackPnl4.Children.Add(img4);
            zoumarriere.Content = stackPnl4;




            #endregion

            comPrincipal.ajouterCanvas(princcanvas);
            comPrincipal.disparaitre(0);
            Temps.time = 1.5;
            slider.Value = slider.Maximum / 2;
            deplacer_droit(grid1, 232, 1);
            apropos = apropos.Append("Membre de l'équipe :").Append((char)10).Append((char)10).Append("             - AMAR Anis                   Chef d'équipe").Append((char)10).Append("             - BOUAM Mellila").Append((char)10).Append("             - BOULEGROUNE Mehdi").Append((char)10).Append("             - IMAD Iheb ").Append((char)10).Append("             - MEKKI Isra").Append((char)10).Append("             - RELIZANI Doria ");
            erreur.ajouterCanvas(princcanvas);
            erreur.disparaitre(0);



        }

        #region methodes de deplacment des grids
        public async Task deplacer_droit(Grid grid, double deplacmet, double time)
        {
            double actual = Canvas.GetLeft(grid);
            DoubleAnimation var = new DoubleAnimation(actual, actual + (double)deplacmet, new Duration(TimeSpan.FromSeconds(time)));
            grid.BeginAnimation(Canvas.LeftProperty, var);
            await Task.Delay(TimeSpan.FromSeconds(time));
        }
        public async Task deplacer_gauche(Grid grid, double deplacmet, double time)
        {
            double actual = Canvas.GetLeft(grid);
            DoubleAnimation var = new DoubleAnimation(actual, actual - (double)deplacmet, new Duration(TimeSpan.FromSeconds(time)));
            grid.BeginAnimation(Canvas.LeftProperty, var);
            await Task.Delay(TimeSpan.FromSeconds(time));
        }
        #endregion

        #region buttons de status bars
        private void about_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("A propos de nous", apropos.ToString());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"www\index.html");
        }



        // gestion des button dans la bar de menu





        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }



        private void zoom_av_Click(object sender, RoutedEventArgs e)
        {

            st.ScaleX *= 1.2;
            st.ScaleY *= 1.2;

            princcanvas.RenderTransform = st;
        }

        private void zoom_ar_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX /= 1.2;
            st.ScaleY /= 1.2;
            princcanvas.RenderTransform = st;
        }
        private void tips_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"www\index.html");
        }



        private void easystruct_about_Click(object sender, RoutedEventArgs e)
        {
            remerciment rem = new remerciment();
            rem.Show();
        }





        //les choix de menu 

        // gestion des button dans interfaces

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;

            Temps.time = 3.4 - slider.Value;            // ... Get Value.
            time = slider.Value;
            // ... Set Window Title.
            textslider.Text = "  Vitesse   :    " + time.ToString("0.0") + "/" + slider.Maximum;
        }

        private void zoumarriere_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX /= 1.2;
            st.ScaleY /= 1.2;
            princcanvas.RenderTransform = st;
        }

        private void zoumavant_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX *= 1.2;
            st.ScaleY *= 1.2;

            princcanvas.RenderTransform = st;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (menuutiliser == 0) { deplacer_gauche(grid1, 155, 1); deplacer_gauche(grid2, 155, 0.5); menuutiliser = 1; }
            else { deplacer_droit(grid1, 155, 1); menuutiliser = 0; }

        }
        #endregion

        #region evenment des operation
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBox)sender).SelectedIndex)
            {
                case 0:
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                    textBchargementaleap.Text = ""; textBchargement2p.Text = "";

                    grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Visible;
                    choixutuliser = 1;

                    try { det.masquer(princcanvas); }
                    catch { }

                    try { mat2.masquer(princcanvas); }
                    catch { }

                    try { transposée.masquer(princcanvas); }
                    catch { }

                    try { produit.masquer(princcanvas); }
                    catch { }

                    try { mat.masquer(princcanvas); }
                    catch { }
                    ; break;
                case 1:
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;
                    grid2.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                    textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                    choixutuliser = 2; ; break;

                case 2:
                    grid2.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;

                    grid3.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                    textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                    choixutuliser = 3; ; break;
                case 3:
                    grid3.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid4.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                    textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                    choixutuliser = 4;

                    try { det.masquer(princcanvas); }
                    catch { }

                    try { transposée.masquer(princcanvas); }
                    catch { }

                    try { produit.masquer(princcanvas); }
                    catch { }
                    break;
            }
        }




        private async void affichermenubutton_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();

            if (menuutiliser == 0)
            {
                deplacer_gauche(grid1, 232, 0.3); grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden; menuutiliser = 1;
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);

            }
            else {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;
                src.UriSource = new Uri("arrows.png", UriKind.Relative);
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2:
                        grid2.Visibility = Visibility.Visible; textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                        textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                        break;
                    case 3:
                        grid3.Visibility = Visibility.Visible; textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                        textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                        break;
                    case 4:
                        grid4.Visibility = Visibility.Visible; textBchargementalea.Text = ""; textBchargementalea2.Text = "";
                        textBchargementaleap.Text = ""; textBchargement2p.Text = "";
                        break;
                }
            }
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;

        }
        private void afficheralgobutton_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;
                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
            }
            else {
                deplacer_gauche(gridalgo, 311, 0.5); algoutiliser = 1;
                src1.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            }
            src1.EndInit();
            img1.Source = src1;
            img1.Stretch = Stretch.UniformToFill;
            // stackPnl.Children.a
            afficheralgobutton.Content = stackPnl1;
        }





        private void buttonchargementalea_Click(object sender, RoutedEventArgs e)

        {



            deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden;
            menuutiliser = 1;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            chargementdjafait = 1;

            if ((int.Parse(textBchargementalea.Text) > 5) || (int.Parse(textBchargementalea2.Text) > 5))
            {
                erreur.Text = "Entrez une valeur entre 1 et 5 ";
                erreur.CoordX = 350;
                erreur.CoordY = 358;
                erreur.apparaitre(0.5);
                erreur.disparaitre(3.9);
                textBchargementalea.Text = "";
                textBchargementalea2.Text = "";
            }
            else {
                mat = new Matrice(int.Parse(textBchargementalea2.Text), int.Parse(textBchargementalea.Text), x, y);
                mat.chargementAleatoir(comPrincipal, princcanvas);
                mat.afficher(princcanvas);
                chargementdjafait = 1;
                chargmentalealiste.IsEnabled = false;
                buttoninsertion.IsEnabled = true;
                okrecherche.IsEnabled = true;

            }
        }
        private void textBchargementalea_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {



                deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden; menuutiliser = 1;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;

                if ((int.Parse(textBchargementalea.Text) > 5) || (int.Parse(textBchargementalea2.Text) > 5))
                {
                    erreur.Text = "Entrer une valeur entre 1 et 5 ";
                    erreur.CoordX = 350;
                    erreur.CoordY = 358;
                    erreur.apparaitre(0.5);
                    erreur.disparaitre(3.9);
                    textBchargementalea.Text = "";
                }
                else {
                    mat = new Matrice(int.Parse(textBchargementalea2.Text), int.Parse(textBchargementalea.Text), x, y);
                    mat.chargementAleatoir(comPrincipal, princcanvas);
                    mat.afficher(princcanvas);
                    chargementdjafait = 1;
                    chargmentalealiste.IsEnabled = false;
                    buttoninsertion.IsEnabled = true;
                    okrecherche.IsEnabled = true;
                }


            }
        }


        private void buttonchargementalea2_Click(object sender, RoutedEventArgs e)
        {


            if (textBchargementalea.Text != "") { donnerentrer = int.Parse(textBchargementalea.Text); }


            deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden;
            menuutiliser = 1;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            chargementdjafait = 1;


            mat = new Matrice(int.Parse(textBchargementalea2.Text), int.Parse(textBchargementalea.Text), x, y);
            mat.chargement(princcanvas, comPrincipal);

            chargementdjafait = 1;
            chargmentalealiste.IsEnabled = false;
            buttoninsertion.IsEnabled = true;
            okrecherche.IsEnabled = true;


        }
        private void buttonchargementalea2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {



                affichermenubutton.IsEnabled = false;
                deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden; menuutiliser = 1;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;

                mat = new Matrice(int.Parse(textBchargementalea2.Text), int.Parse(textBchargementalea.Text), x, y);
                mat.chargement(princcanvas, comPrincipal);
                chargementdjafait = 1;
                chargmentalealiste.IsEnabled = false;
                buttoninsertion.IsEnabled = true;
                okrecherche.IsEnabled = true;
                affichermenubutton.IsEnabled = true;



            }
        }


        private async void ListBoxItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BitmapImage src1 = new BitmapImage();
                src1.BeginInit();
                BitmapImage src2 = new BitmapImage();
                src2.BeginInit();

                if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;
                algoutiliser = 1;
                deplacer_gauche(grid1, 232, 0.3); grid2.Visibility = Visibility.Hidden; menuutiliser = 1;


                try { mat2.masquer(princcanvas); }
                catch { }

                try { det.masquer(princcanvas); }
                catch { }

                try { transposée.masquer(princcanvas); }
                catch { }

                try { produit.masquer(princcanvas); }
                catch { }

                try
                {
                    affichermenubutton.IsEnabled = false;
                    transposée = new Matrice(mat.NbColonnes, mat.NbLignes, x + 50 * (mat.NbColonnes + 1), y);
                    await mat.trans(x + 50 * (mat.NbColonnes + 1), y, princcanvas, comPrincipal, transposée, algocanvas);
                    affichermenubutton.IsEnabled = true;
                }
                catch { }



            }

        }
        private async void okrecherche_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            BitmapImage src2 = new BitmapImage();
            src2.BeginInit();
            // if (textboxrecherche.Text != "") { donnerentrer = int.Parse(textboxrecherche.Text); }
            if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            algoutiliser = 1;
            deplacer_gauche(grid1, 232, 0.3); grid2.Visibility = Visibility.Hidden; menuutiliser = 1;


            try { mat2.masquer(princcanvas); }
            catch { }

            try { det.masquer(princcanvas); }
            catch { }

            try { transposée.masquer(princcanvas); }
            catch { }

            try { produit.masquer(princcanvas); }
            catch { }

            try
            {
                affichermenubutton.IsEnabled = false;
                transposée = new Matrice(mat.NbColonnes, mat.NbLignes, x + 50 * (mat.NbColonnes + 1), y);
                await mat.trans(x + 50 * (mat.NbColonnes + 1), y, princcanvas, comPrincipal, transposée, algocanvas);
                affichermenubutton.IsEnabled = true;
            }
            catch { }




        }

        private async void ListBoxItem_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BitmapImage src1 = new BitmapImage();
                src1.BeginInit();
                BitmapImage src2 = new BitmapImage();
                src2.BeginInit();

                if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;
                algoutiliser = 1;
                deplacer_gauche(grid1, 232, 0.3); grid3.Visibility = Visibility.Hidden; menuutiliser = 1;


                try { mat2.masquer(princcanvas); }
                catch { }

                try { det.masquer(princcanvas); }
                catch { }

                try { transposée.masquer(princcanvas); }
                catch { }

                try { produit.masquer(princcanvas); }
                catch { }

                try
                {
                    affichermenubutton.IsEnabled = false;
                    await mat.Determinant(det, princcanvas, comPrincipal, algocanvas);
                    affichermenubutton.IsEnabled = true;
                }
                catch { }

            }
        }
        private async void buttoninsertion_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            BitmapImage src2 = new BitmapImage();
            src2.BeginInit();

            if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            algoutiliser = 1;
            deplacer_gauche(grid1, 232, 0.3); grid3.Visibility = Visibility.Hidden; menuutiliser = 1;


            try { mat2.masquer(princcanvas); }
            catch { }

            try { det.masquer(princcanvas); }
            catch { }

            try { transposée.masquer(princcanvas); }
            catch { }

            try { produit.masquer(princcanvas); }
            catch { }

            try
            {
                affichermenubutton.IsEnabled = false;
                await mat.Determinant(det, princcanvas, comPrincipal, algocanvas);
                affichermenubutton.IsEnabled = true;
            }
            catch { }







        }

        private async void buttonchargementalea2P_Click(object sender, RoutedEventArgs e)
        {
            deplacer_gauche(grid1, 232, 0.5); grid4.Visibility = Visibility.Hidden;
            menuutiliser = 1;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            chargementdjafait = 1;
            if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
            algoutiliser = 1;
            if ((int.Parse(textBchargement2p.Text) > 5) || (int.Parse(textBchargementaleap.Text) > 5))
            {
                erreur.Text = "Entrer une valeur entre 1 et 5 ";
                erreur.CoordX = 350;
                erreur.CoordY = 358;
                erreur.apparaitre(0.5);
                erreur.disparaitre(3.9);
                textBchargementalea.Text = "";
            }
            else {
                try { mat2.masquer(princcanvas); }
                catch { }

                try { produit.masquer(princcanvas); }
                catch { }



                affichermenubutton.IsEnabled = false;
                mat2 = new Matrice(int.Parse(textBchargement2p.Text), int.Parse(textBchargementaleap.Text), x + (mat.NbColonnes + 1) * 50, y);
                await mat2.chargement(princcanvas, comPrincipal);
                if (mat.NbColonnes == mat2.NbLignes) produit = new Matrice(mat.NbLignes, mat2.NbColonnes, x + 50 * (mat.NbColonnes + 1), y);
                int a = int.Parse(textBchargement2p.Text);
                if (a < int.Parse(textBchargementaleap.Text)) a = int.Parse(textBchargementaleap.Text);
                await Matrice.produit(mat, mat2, produit, princcanvas, x + (mat.NbColonnes + 1) * 50 / 2, y + 50 * (a + 1) + 25, comPrincipal, algocanvas);
                affichermenubutton.IsEnabled = true;

                chargmentalealiste.IsEnabled = false;
            }
        }

        private async void buttonchargementaleaP_Click(object sender, RoutedEventArgs e)
        {

            deplacer_gauche(grid1, 232, 0.5); grid4.Visibility = Visibility.Hidden;
            menuutiliser = 1;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            chargementdjafait = 1;
            if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
            algoutiliser = 1;

            if ((int.Parse(textBchargement2p.Text) > 5) || (int.Parse(textBchargementaleap.Text) > 5))
            {
                erreur.Text = "Entrer une valeur entre 1 et 5 ";
                erreur.CoordX = 350;
                erreur.CoordY = 358;
                erreur.apparaitre(0.5);
                erreur.disparaitre(3.9);
                textBchargementalea.Text = "";
            }
            else {
                try { mat2.masquer(princcanvas); }
                catch { }

                try { produit.masquer(princcanvas); }
                catch { }

                affichermenubutton.IsEnabled = false;
                mat2 = new Matrice(int.Parse(textBchargement2p.Text), int.Parse(textBchargementaleap.Text), x + (mat.NbColonnes + 1) * 50, y);
                mat2.chargementAleatoir(comPrincipal, princcanvas);
                mat2.afficher(princcanvas);
                if (mat.NbColonnes == mat2.NbLignes) produit = new Matrice(mat.NbLignes, mat2.NbColonnes, x + 50 * (mat.NbColonnes + 1), y);
                int a = int.Parse(textBchargement2p.Text);
                if (a < int.Parse(textBchargementaleap.Text)) a = int.Parse(textBchargementaleap.Text);
                await Matrice.produit(mat, mat2, produit, princcanvas, x + (mat.NbColonnes + 1) * 50 / 2, y + 50 * (a + 1) + 25, comPrincipal, algocanvas);
                affichermenubutton.IsEnabled = true;

                chargmentalealiste.IsEnabled = false;
            }
        }

        #endregion

        private async void textBchargementalea2P_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                deplacer_gauche(grid1, 232, 0.5); grid4.Visibility = Visibility.Hidden;
                menuutiliser = 1;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;
                chargementdjafait = 1;
                if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
                algoutiliser = 1;

                if ((int.Parse(textBchargement2p.Text) > 5) || (int.Parse(textBchargementaleap.Text) > 5))
                {
                    erreur.Text = "Entrer une valeur entre 1 et 5 ";
                    erreur.CoordX = 350;
                    erreur.CoordY = 358;
                    erreur.apparaitre(0.5);
                    erreur.disparaitre(3.9);
                    textBchargementalea.Text = "";
                }
                else {
                    try { mat2.masquer(princcanvas); }
                    catch { }

                    try { produit.masquer(princcanvas); }
                    catch { }

                    affichermenubutton.IsEnabled = false;
                    mat2 = new Matrice(int.Parse(textBchargement2p.Text), int.Parse(textBchargementaleap.Text), x + (mat.NbColonnes + 1) * 50, y);
                    mat2.chargementAleatoir(comPrincipal, princcanvas);
                    mat2.afficher(princcanvas);
                    if (mat.NbColonnes == mat2.NbLignes) produit = new Matrice(mat.NbLignes, mat2.NbColonnes, x + 50 * (mat.NbColonnes + 1), y);
                    int a = int.Parse(textBchargement2p.Text);
                    if (a < int.Parse(textBchargementaleap.Text)) a = int.Parse(textBchargementaleap.Text);
                    await Matrice.produit(mat, mat2, produit, princcanvas, x + (mat.NbColonnes + 1) * 50 / 2, y + 50 * (a + 1) + 25, comPrincipal, algocanvas);
                    affichermenubutton.IsEnabled = true;

                    chargmentalealiste.IsEnabled = false;
                }
            }
        }

        #region controle des textse box
        private void textBchargementalea_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonchargementale.IsEnabled = ((textBchargementalea.Text.Length > 0) & (textBchargementalea2.Text.Length > 0));
            buttonchargementalea2.IsEnabled = ((textBchargementalea.Text.Length > 0) & (textBchargementalea2.Text.Length > 0));
        }

        private void textBchargementalea2_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonchargementalea2.IsEnabled = ((textBchargementalea.Text.Length > 0) & (textBchargementalea2.Text.Length > 0));
            buttonchargementale.IsEnabled = ((textBchargementalea.Text.Length > 0) & (textBchargementalea2.Text.Length > 0));
        }


        private void textBchargementaleap_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonchargementalea2p.IsEnabled = ((textBchargementaleap.Text.Length > 0) & (textBchargement2p.Text.Length > 0) & (chargementdjafait == 1));
            buttonchargementalep.IsEnabled = ((textBchargementaleap.Text.Length > 0) & (textBchargement2p.Text.Length > 0) & (chargementdjafait == 1));
        }

        private void textBchargementale2p_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonchargementalea2p.IsEnabled = ((textBchargementaleap.Text.Length > 0) & (textBchargement2p.Text.Length > 0) & (chargementdjafait == 1));
            buttonchargementalep.IsEnabled = ((textBchargementaleap.Text.Length > 0) & (textBchargement2p.Text.Length > 0) & (chargementdjafait == 1));
        }
        private void textboxrecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            // okrecherche.IsEnabled = ((textboxrecherche.Text.Length > 0) & (chargementdjafait == 1));
        }

        private void textBoxinsertion_TextChanged(object sender, TextChangedEventArgs e)
        {
            //buttoninsertion.IsEnabled = (textBoxinsertion.Text.Length > 0);
        }

        private void textBoxsuppression_TextChanged(object sender, TextChangedEventArgs e)
        {
            //buttonsuppression.IsEnabled = ((textBoxsuppression.Text.Length > 0) & (chargementdjafait == 1));
        }

        #endregion

        #region animation sur qlq buttons
        private void chargmentalealiste_MouseEnter(object sender, MouseEventArgs e)
        {
            imgchargment.Width += 2.5;
            imgchargment.Height += 2.5;
        }

        private void chargmentalealiste_MouseLeave(object sender, MouseEventArgs e)
        {
            imgchargment.Width -= 2.5;
            imgchargment.Height -= 2.5;
        }

        private void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            imgRecherch.Width += 2.5;
            imgRecherch.Height += 2.5;
        }

        private void ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            imgRecherch.Width -= 2.5;
            imgRecherch.Height -= 2.5;
        }

        private void ListBoxItem_MouseEnter_1(object sender, MouseEventArgs e)
        {
            imgInsertion.Width += 2.5;
            imgInsertion.Height += 2.5;

        }

        private void ListBoxItem_MouseLeave_1(object sender, MouseEventArgs e)
        {
            imgInsertion.Width -= 2.5;
            imgInsertion.Height -= 2.5;
        }

        private void ListBoxItem_MouseEnter_2(object sender, MouseEventArgs e)
        {
            imgsuppression.Width += 2.5;
            imgsuppression.Height += 2.5;
        }

        private void ListBoxItem_MouseLeave_2(object sender, MouseEventArgs e)
        {
            imgsuppression.Width -= 2.5;
            imgsuppression.Height -= 2.5;
        }

        private void affichermenubutton_MouseEnter(object sender, MouseEventArgs e)
        {
            stackPnl.Width += 2;
            stackPnl.Height += 1;

        }

        private void affichermenubutton_MouseLeave(object sender, MouseEventArgs e)
        {
            stackPnl.Width -= 2;
            stackPnl.Height -= 1;
        }

        private void afficheralgobutton_MouseEnter(object sender, MouseEventArgs e)
        {
            stackPnl1.Width += 2;
            stackPnl1.Height += 1;
        }

        private void textBchargementalea2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBchargementalea_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBchargement2p_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBchargementaleap_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBchargementale2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
        }

        private void textBchargementalea2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
        }

        private void textBchargementaleaA_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBchargementaleaP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
        }



        private void afficheralgobutton_MouseLeave(object sender, MouseEventArgs e)
        {
            stackPnl1.Width -= 2;
            stackPnl1.Height -= 1;
        }



        private void helpEnter(object sender, MouseEventArgs e)
        {
            imgHelp.Width += 2;
            imgHelp.Height += 2;
        }

        private void helpLeave(object sender, MouseEventArgs e)
        {
            imgHelp.Width -= 2;
            imgHelp.Height -= 2;
        }


        private void aboutEnter(object sender, MouseEventArgs e)
        {
            imgAbout.Width += 2;
            imgAbout.Height += 2;

        }



        private void about_MouseLeave(object sender, MouseEventArgs e)
        {

            imgAbout.Width -= 2;
            imgAbout.Height -= 2;
        }


        #endregion









    }
}
