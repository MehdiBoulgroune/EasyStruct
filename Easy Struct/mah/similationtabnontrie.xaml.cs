﻿using System;
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
    /// Interaction logic for similationtabnontrie.xaml
    /// </summary>
    public partial class similationtabnontrie : MetroWindow
    {
        #region declaration
        StringBuilder apropos = new StringBuilder();
        ScaleTransform st = new ScaleTransform();
        int menuutiliser = 0, algoutiliser = 0; int donnerentrer; int chargementdjafait = 0; int choixutuliser = 0;
        double time;
        Tableau tabtrie = new Tableau(false, 170, 190);
        
        Commentaire princ = new Commentaire("", Brushes.Black, 10, 30, 50, 50, Brushes.Yellow, Brushes.Yellow);
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
        public similationtabnontrie()
        {
            InitializeComponent();

            

            #region initialisation des images
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
            Temps.time = 2.5;
            slider.Value = slider.Maximum / 2;

            princ.ajouterCanvas(princcanvas);
            princ.disparaitre(0);

            erreur.ajouterCanvas(princcanvas);
            erreur.opacity = 0;

            deplacer_droit(grid1, 232, 1);

            apropos = apropos.Append("Membres de l'équipe :").Append((char)10).Append((char)10).Append("             - AMAR Anis Zoubir                   Chef d'équipe").Append((char)10).Append("             - BOUAM Mellila").Append((char)10).Append("             - BOULEGROUNE Mehdi").Append((char)10).Append("             - IMAD Iheb ").Append((char)10).Append("             - MEKKI Isra").Append((char)10).Append("             - RELIZANI Doria ");
        }

        #region methode de deplacment de grids
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
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"www\index.html");
        }

       
        private void about_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("A propos de nous", apropos.ToString());
        }

    

        // gestion des button dans la bar de menu

        private void info_Click(object sender, RoutedEventArgs e)
        {

        }



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

        private void contact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void easystruct_about_Click(object sender, RoutedEventArgs e)
        {
            remerciment REM1 = new remerciment();
            REM1.Show();
        }

        #endregion


        #region 
        //les choix de menu 
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBox)sender).SelectedIndex)
            {
                case 0:
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Visible;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden;
                    choixutuliser = 1;
                    ; break;
                case 1:
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;
                    grid2.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 2; ; break;

                case 2:
                    grid2.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden;

                    grid3.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 3; ; break;
                case 3:
                    grid3.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid4.Visibility = Visibility.Visible;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden;
                    textBchargementalea.Text = ""; textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 4; ; break;

                case 4:
                    grid3.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid4.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Visible;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden;
                    textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 5; ; break;
                case 5:
                    grid3.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid4.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Visible;

                    textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 6; ; break;
                case 6:
                    grid3.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid4.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Visible;
                    grid7.Visibility = Visibility.Hidden;

                    textBoxsuppression.Text = ""; textboxrecherche.Text = ""; textBoxinsertion.Text = "";
                    choixutuliser = 7; ; break;
            }
        }

        // gestion des button dans interfaces

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;

            Temps.time = 5.1 - slider.Value;
            // ... Get Value.
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





        #region envenment sur les operation
        private async void affichermenubutton_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();

            if (menuutiliser == 0)
            {
                deplacer_gauche(grid1, 232, 0.3); grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;
                grid6.Visibility = Visibility.Hidden;
                grid7.Visibility = Visibility.Hidden;
                grid8.Visibility = Visibility.Hidden;
                menuutiliser = 1;
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);

            }
            else {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;
                grid6.Visibility = Visibility.Hidden;
                grid7.Visibility = Visibility.Hidden;
                grid8.Visibility = Visibility.Hidden;
                src.UriSource = new Uri("arrows.png", UriKind.Relative);
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    case 5: grid6.Visibility = Visibility.Visible;  break;
                    case 6: grid7.Visibility = Visibility.Visible;  break;
                    case 7: grid8.Visibility = Visibility.Visible; break;
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





        private  void buttonchargementalea_Click(object sender, RoutedEventArgs e)

        {

            if (textBchargementalea.Text != "") { donnerentrer = int.Parse(textBchargementalea.Text); }
            if ((donnerentrer < 1) || (donnerentrer > 20))
            {
                erreur.Text = "Entrez une valeur entre 1 et 20 ";
                erreur.CoordX = 350;
                erreur.CoordY = 358;
                erreur.apparaitre(0.5);
                erreur.disparaitre(3.9);
                textBchargementalea.Text = "";
            }
            else {
                deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden;
                menuutiliser = 1;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;


                 tabtrie.chargementAleatoire(donnerentrer);
                buttonbulle.IsEnabled = true;
                buttonselection.IsEnabled = true;
                buttontransposition.IsEnabled = true;
                chargementdjafait = 1;

                tabtrie.afficher(princcanvas);
                chargmentalealiste.IsEnabled = false;
            }

        }
        private void textBchargementalea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textBchargementalea.Text != "") { donnerentrer = int.Parse(textBchargementalea.Text); }
                if ((donnerentrer < 1) || (donnerentrer > 20))
                {
                    erreur.Text = "Entrer une valeur entre 1 et 20 ";
                    erreur.CoordX = 350;
                    erreur.CoordY = 358;
                    erreur.apparaitre(0.5);
                    erreur.disparaitre(3.9);
                    textBchargementalea.Text = "";
                }
                else {
                    deplacer_gauche(grid1, 232, 0.5); grid5.Visibility = Visibility.Hidden; menuutiliser = 1;
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                    src.EndInit();
                    img.Source = src;
                    img.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl;

                    tabtrie.chargementAleatoire(donnerentrer);
                    buttonbulle.IsEnabled = true;
                    buttonselection.IsEnabled = true;
                    buttontransposition.IsEnabled = true;
                    chargementdjafait = 1;

                    tabtrie.afficher(princcanvas);
                    chargmentalealiste.IsEnabled = false;

                }

            }
        }
        private async void textboxrecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
                BitmapImage src1 = new BitmapImage();
                src1.BeginInit();
                BitmapImage src2 = new BitmapImage();
                src2.BeginInit();
                if (textboxrecherche.Text != "") { donnerentrer = int.Parse(textboxrecherche.Text); }
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
                await tabtrie.recherche(donnerentrer, tabl2, princcanvas, false, princ, algocanvas);
                affichermenubutton.IsEnabled = true;

                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    // stackPnl.Children.a
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;

                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    }
                }

            }
        }

        private async void okrecherche_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            BitmapImage src2 = new BitmapImage();
            src2.BeginInit();
            if (textboxrecherche.Text != "") { donnerentrer = int.Parse(textboxrecherche.Text); }
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
            await tabtrie.recherche(donnerentrer, tabl2, princcanvas, false, princ, algocanvas);


       
            affichermenubutton.IsEnabled = true;
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                src1.EndInit();
                img1.Source = src1;
                img1.Stretch = Stretch.UniformToFill;
                // stackPnl.Children.a
                afficheralgobutton.Content = stackPnl1;
            }
            if (menuutiliser == 1)
            {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;

                src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                src2.EndInit();
                img2.Source = src2;
                img2.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl2;
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                }
            }

        }

        private async void buttoninsertion_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            BitmapImage src2 = new BitmapImage();
            src2.BeginInit();
            if (textBoxinsertion.Text != "") { donnerentrer = int.Parse(textBoxinsertion.Text); }
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
            await tabtrie.inserer(donnerentrer, princcanvas, princ, algocanvas);
            affichermenubutton.IsEnabled = true;
            if (tabtrie.TailleTab > 1)
            {
                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    // stackPnl.Children.a
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;

                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    }
                }
            }
            if (chargementdjafait == 0) { chargementdjafait = 1;
                buttonbulle.IsEnabled = true;
                buttonselection.IsEnabled = true;
                buttontransposition.IsEnabled = true;
                chargmentalealiste.IsEnabled = false; }
        }
        private async void textBoxinsertion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
                BitmapImage src1 = new BitmapImage();
                src1.BeginInit();
                BitmapImage src2 = new BitmapImage();
                src2.BeginInit();
                if (textBoxinsertion.Text != "") { donnerentrer = int.Parse(textBoxinsertion.Text); }
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
                await tabtrie.inserer(donnerentrer, princcanvas, princ, algocanvas);
                affichermenubutton.IsEnabled = true;
                if (tabtrie.TailleTab > 1)
                {
                    if (algoutiliser == 1)
                    {
                        deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                        src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                        src1.EndInit();
                        img1.Source = src1;
                        img1.Stretch = Stretch.UniformToFill;
                        // stackPnl.Children.a
                        afficheralgobutton.Content = stackPnl1;
                    }
                    if (menuutiliser == 1)
                    {
                        grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                        grid5.Visibility = Visibility.Hidden;

                        src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                        src2.EndInit();
                        img2.Source = src2;
                        img2.Stretch = Stretch.UniformToFill;
                        affichermenubutton.Content = stackPnl2;
                        await deplacer_droit(grid1, 232, 0.3);
                        menuutiliser = 0;
                        switch (choixutuliser)
                        {
                            case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                            case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                            case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                            case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                        }
                    }
                }
                if (chargementdjafait == 0) { chargementdjafait = 1;
                    buttonbulle.IsEnabled = true;
                    buttonselection.IsEnabled = true;
                    buttontransposition.IsEnabled = true;
                    chargmentalealiste.IsEnabled = false; }
            }
        }
        private async void buttonsuppression_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
            BitmapImage src1 = new BitmapImage();
            src1.BeginInit();
            BitmapImage src2 = new BitmapImage();
            src2.BeginInit();
            if (textBoxsuppression.Text != "") { donnerentrer = int.Parse(textBoxsuppression.Text); }
            if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
            src.EndInit();
            img.Source = src;
            img.Stretch = Stretch.UniformToFill;
            affichermenubutton.Content = stackPnl;
            algoutiliser = 1;
            deplacer_gauche(grid1, 232, 0.3); grid4.Visibility = Visibility.Hidden; menuutiliser = 1;
            await tabtrie.suppression(donnerentrer, princcanvas, princ, algocanvas);
            buttonbulle.IsEnabled = (tabtrie.TailleTab != 0);
            buttonselection.IsEnabled = (tabtrie.TailleTab != 0);
            buttontransposition.IsEnabled = (tabtrie.TailleTab != 0);
            chargmentalealiste.IsEnabled = (tabtrie.TailleTab == 0);
            affichermenubutton.IsEnabled = true;
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                src1.EndInit();
                img1.Source = src1;
                img1.Stretch = Stretch.UniformToFill;
                // stackPnl.Children.a
                afficheralgobutton.Content = stackPnl1;
            }
            if (menuutiliser == 1)
            {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;

                src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                src2.EndInit();
                img2.Source = src2;
                img2.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl2;
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                }
            }


        }
        private async void textBoxsuppression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
                BitmapImage src1 = new BitmapImage();
                src1.BeginInit();
                BitmapImage src2 = new BitmapImage();
                src2.BeginInit();
                if (textBoxsuppression.Text != "") { donnerentrer = int.Parse(textBoxsuppression.Text); }
                if (algoutiliser == 0) deplacer_gauche(gridalgo, 311, .7);
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri("arrows-1.png", UriKind.Relative);
                src.EndInit();
                img.Source = src;
                img.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl;
                algoutiliser = 1;
                deplacer_gauche(grid1, 232, 0.3); grid4.Visibility = Visibility.Hidden; menuutiliser = 1;
                await tabtrie.suppression(donnerentrer, princcanvas, princ, algocanvas);
                buttonbulle.IsEnabled = (tabtrie.TailleTab != 0);
                buttonselection.IsEnabled = (tabtrie.TailleTab != 0);
                buttontransposition.IsEnabled = (tabtrie.TailleTab != 0);

                chargmentalealiste.IsEnabled = (tabtrie.TailleTab == 0);
                affichermenubutton.IsEnabled = true;
                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    // stackPnl.Children.a
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;

                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    }
                }


            }
        }

        private async void buttonbulle_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
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
            deplacer_gauche(grid1, 232, 0.3); grid6.Visibility = Visibility.Hidden; menuutiliser = 1;
            await tabtrie.trieeBulle(princcanvas, princ, algocanvas);
            affichermenubutton.IsEnabled = true;
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                src1.EndInit();
                img1.Source = src1;
                img1.Stretch = Stretch.UniformToFill;
                // stackPnl.Children.a
                afficheralgobutton.Content = stackPnl1;
            }
            if (menuutiliser == 1)
            {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden; grid6.Visibility = Visibility.Hidden;
                grid7.Visibility = Visibility.Hidden; grid8.Visibility = Visibility.Hidden;
                src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                src2.EndInit();
                img2.Source = src2;
                img2.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl2;
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    case 5: grid6.Visibility = Visibility.Visible; break;
                    case 6: grid7.Visibility = Visibility.Visible; break;
                    case 7: grid8.Visibility = Visibility.Visible; break;
                }
            }
        }
        private async void ListBoxItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
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
                deplacer_gauche(grid1, 232, 0.3);
                grid6.Visibility = Visibility.Hidden; menuutiliser = 1;
                await tabtrie.trieeBulle(princcanvas, princ, algocanvas);
                affichermenubutton.IsEnabled = true;
                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    // stackPnl.Children.a
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden; grid6.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden; grid8.Visibility = Visibility.Hidden;
                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                        case 5: grid6.Visibility = Visibility.Visible; break;
                        case 6: grid7.Visibility = Visibility.Visible; break;
                        case 7: grid8.Visibility = Visibility.Visible; break;
                    }
                }
            }
        }
        private async void buttontransposition_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
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
            deplacer_gauche(grid1, 232, 0.3); grid7.Visibility = Visibility.Hidden; menuutiliser = 1;
            await tabtrie.trieeTransposition(princcanvas, princ, algocanvas);
            affichermenubutton.IsEnabled = true;
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                src1.EndInit();
                img1.Source = src1;
                img1.Stretch = Stretch.UniformToFill;
                // stackPnl.Children.a
                afficheralgobutton.Content = stackPnl1;
            }
            if (menuutiliser == 1)
            {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;
                grid6.Visibility = Visibility.Hidden;
                grid7.Visibility = Visibility.Hidden; grid8.Visibility = Visibility.Hidden;

                src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                src2.EndInit();
                img2.Source = src2;
                img2.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl2;
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    case 5: grid6.Visibility = Visibility.Visible; break;
                    case 6: grid7.Visibility = Visibility.Visible; break;
                    case 7: grid8.Visibility = Visibility.Visible; break;
                }
            }
        }
        private async void ListBoxItem_KeyDown_2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
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
                deplacer_gauche(grid1, 232, 0.3); grid8.Visibility = Visibility.Hidden; menuutiliser = 1;
                await tabtrie.trieeSelection(princcanvas, princ, algocanvas);
                affichermenubutton.IsEnabled = true;
                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    // stackPnl.Children.a
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden;
                    grid7.Visibility = Visibility.Hidden; grid8.Visibility = Visibility.Hidden;

                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                        case 5: grid6.Visibility = Visibility.Visible; break;
                        case 6: grid7.Visibility = Visibility.Visible; break;
                        case 7: grid8.Visibility = Visibility.Visible; break;
                    }
                }
            }

        }
        private async void buttonselection_Click(object sender, RoutedEventArgs e)
        {
            affichermenubutton.IsEnabled = false;
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
            deplacer_gauche(grid1, 232, 0.3); grid8.Visibility = Visibility.Hidden; menuutiliser = 1;
            await tabtrie.trieeSelection(princcanvas, princ, algocanvas);
            affichermenubutton.IsEnabled = true;
            if (algoutiliser == 1)
            {
                deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                src1.EndInit();
                img1.Source = src1;
                img1.Stretch = Stretch.UniformToFill;
                // stackPnl.Children.a
                afficheralgobutton.Content = stackPnl1;
            }
            if (menuutiliser == 1)
            {
                grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                grid5.Visibility = Visibility.Hidden;
                grid6.Visibility = Visibility.Hidden; grid7.Visibility = Visibility.Hidden;
                grid8.Visibility = Visibility.Hidden;

                src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                src2.EndInit();
                img2.Source = src2;
                img2.Stretch = Stretch.UniformToFill;
                affichermenubutton.Content = stackPnl2;
                await deplacer_droit(grid1, 232, 0.3);
                menuutiliser = 0;
                switch (choixutuliser)
                {
                    case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                    case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                    case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                    case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                    case 5: grid6.Visibility = Visibility.Visible; break;
                    case 6: grid7.Visibility = Visibility.Visible; break;
                    case 7: grid8.Visibility = Visibility.Visible; break;
                }
            }
        }

   

        private async void ListBoxItem_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                affichermenubutton.IsEnabled = false;
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
                deplacer_gauche(grid1, 232, 0.3); grid7.Visibility = Visibility.Hidden; menuutiliser = 1;
                await tabtrie.trieeTransposition(princcanvas, princ, algocanvas);
                affichermenubutton.IsEnabled = true;
                if (algoutiliser == 1)
                {
                    deplacer_droit(gridalgo, 311, 0.5); algoutiliser = 0;

                    src1.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src1.EndInit();
                    img1.Source = src1;
                    img1.Stretch = Stretch.UniformToFill;
                    afficheralgobutton.Content = stackPnl1;
                }
                if (menuutiliser == 1)
                {
                    grid3.Visibility = Visibility.Hidden; grid4.Visibility = Visibility.Hidden; grid2.Visibility = Visibility.Hidden;
                    grid5.Visibility = Visibility.Hidden;
                    grid6.Visibility = Visibility.Hidden; grid7.Visibility = Visibility.Hidden;
                    grid8.Visibility = Visibility.Hidden;

                    src2.UriSource = new Uri("arrows.png", UriKind.Relative);
                    src2.EndInit();
                    img2.Source = src2;
                    img2.Stretch = Stretch.UniformToFill;
                    affichermenubutton.Content = stackPnl2;
                    await deplacer_droit(grid1, 232, 0.3);
                    menuutiliser = 0;
                    switch (choixutuliser)
                    {
                        case 1: if (chargementdjafait == 0) { grid5.Visibility = Visibility.Visible; textBchargementalea.Text = ""; }; break;
                        case 2: grid2.Visibility = Visibility.Visible; textboxrecherche.Text = ""; break;
                        case 3: grid3.Visibility = Visibility.Visible; textBoxinsertion.Text = ""; break;
                        case 4: grid4.Visibility = Visibility.Visible; textBoxsuppression.Text = ""; break;
                        case 5: grid6.Visibility = Visibility.Visible; break;
                        case 6: grid7.Visibility = Visibility.Visible; break;
                        case 7: grid8.Visibility = Visibility.Visible; break;
                    }
                }
            }
        }

        #endregion
        #region control de text box
        private void textBchargementalea_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonchargementalea.IsEnabled = textBchargementalea.Text.Length > 0;
        }

        private void textboxrecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            okrecherche.IsEnabled = ((textboxrecherche.Text.Length > 0) & (tabtrie.TailleTab!=0));
        }

        private void textBoxinsertion_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttoninsertion.IsEnabled = (textBoxinsertion.Text.Length > 0);
        }

        private void textBoxsuppression_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonsuppression.IsEnabled = ((textBoxsuppression.Text.Length > 0) & (tabtrie.TailleTab != 0));
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
            stackPnl.Width += 5;
            stackPnl.Height += 4;

        }

        private void affichermenubutton_MouseLeave(object sender, MouseEventArgs e)
        {
            stackPnl.Width -= 5;
            stackPnl.Height -= 4;
        }

        private void afficheralgobutton_MouseEnter(object sender, MouseEventArgs e)
        {
            stackPnl1.Width += 5;
            stackPnl1.Height += 4;
        }

        private void ListBoxItem_MouseEnter_3(object sender, MouseEventArgs e)
        {
            imgtriparbulle.Width += 2.5;
            imgtriparbulle.Height += 2.5;
        }

      

        private void ListBoxItem_MouseLeave_3(object sender, MouseEventArgs e)
        {
            imgtriparbulle.Width -= 2.5;
            imgtriparbulle.Height -= 2.5;
        }

        private void ListBoxItem_MouseEnter_4(object sender, MouseEventArgs e)
        {
            imgtrieTransposition.Width += 2.5;
            imgtrieTransposition.Height += 2.5;
        }

        private void ListBoxItem_MouseLeave_4(object sender, MouseEventArgs e)
        {
            imgtrieTransposition.Width -= 2.5;
            imgtrieTransposition.Height -= 2.5;
        }

        private void ListBoxItem_MouseEnter_5(object sender, MouseEventArgs e)
        {
            imgtrieselection.Width += 2.5;
            imgtrieselection.Height += 2.5;
        }

        private void ListBoxItem_MouseLeave_5(object sender, MouseEventArgs e)
        {
            imgtrieselection.Width -= 2.5;
            imgtrieselection.Height -= 2.5;
        }

    

        private void afficheralgobutton_MouseLeave(object sender, MouseEventArgs e)
        {
            stackPnl1.Width -= 5;
            stackPnl1.Height -= 4;
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

        private void helpEnter(object sender, MouseEventArgs e)
        {
            imgHelp.Width += 2;
            imgHelp.Height += 2;
        }

        private void textBchargementalea_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBoxsuppression_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBoxinsertion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textboxrecherche_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void helpLeave(object sender, MouseEventArgs e)
        {
            imgHelp.Width -= 2;
            imgHelp.Height -= 2;
        }

        #endregion





    }
}
