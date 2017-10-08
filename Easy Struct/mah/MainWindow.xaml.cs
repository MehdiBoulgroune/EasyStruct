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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Threading;
using WpfAnimatedGif;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace mah
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        int f = 0;
        public  MainWindow()
        {
            Thread.Sleep(6000);
            InitializeComponent();

        

        }
        #region v acces au fenetres
        private void tableautrie_Click(object sender, RoutedEventArgs e)
        {
            simulation sim = new simulation();
            sim.Show();
        }

        private void tableaunontrie_Click(object sender, RoutedEventArgs e)
        {
            similationtabnontrie sim1 = new similationtabnontrie();
            sim1.Show();
        }

  
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"www\index.html");
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            remerciment rem = new remerciment();
            rem.Show();
        }

      
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(arbre);
           if (c != null) c.Play();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(arbre);
             if(c != null) c.Pause();
        }

        private void binaire_Click(object sender, RoutedEventArgs e)
        {
            similationarbre simarbre = new similationarbre();
            simarbre.Show();
        }

        private void avl_Click(object sender, RoutedEventArgs e)
        {
            simulation sim = new simulation();
            sim.Show();
        }

        private void pile_Click(object sender, RoutedEventArgs e)
        {
            simulation sim = new simulation();
            sim.Show();
            
        }

        private void tableau_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void tableau_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void tableaunontrie_Click_1(object sender, RoutedEventArgs e)
        {
            similationtabnontrie sim1 = new similationtabnontrie();
            sim1.Show();
        }

        private void Matrice_Click(object sender, RoutedEventArgs e)
        {
            similationMatrices simmatrice= new similationMatrices();
            simmatrice.Show();
        }

        private void pile_Click_1(object sender, RoutedEventArgs e)
        {
            similationPile simpile = new similationPile();
            simpile.Show();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            similationFile simfile = new similationFile();
            simfile.Show();
        }

        private void chaine_Click(object sender, RoutedEventArgs e)
        {
            similationLlc simllc = new similationLlc();
            simllc.Show();
        }

        private void bdicertion_Click(object sender, RoutedEventArgs e)
        {
            similationLlcbidirect simbidirec = new similationLlcbidirect();
            simbidirec.Show();
        }

        private void circulaire_Click(object sender, RoutedEventArgs e)
        {
            similationLlccircul simcirculaire = new similationLlccircul();
            simcirculaire.Show();
        }

        private void chainenontrie_Click(object sender, RoutedEventArgs e)
        {
            similationLlcnontrie simllcnontrie = new similationLlcnontrie();
            simllcnontrie.Show();
        }

        private void bdicertionnontrie_Click(object sender, RoutedEventArgs e)
        {
            similationLlcbidercnontrie simllcbidernontrie = new similationLlcbidercnontrie();
            simllcbidernontrie.Show();
        }

        private void circulairenontrie_Click(object sender, RoutedEventArgs e)
        {
            similationLlccirculnontrie simllccirculnontrie = new similationLlccirculnontrie();
            simllccirculnontrie.Show();
        }
        #endregion

        #region animation sur qlq buttins 
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

        private void helpLeave(object sender, MouseEventArgs e)
        {
            imgHelp.Width -= 2;
            imgHelp.Height -= 2;
        }
        #endregion


        #region animation sur les grid
        private void Grid_MouseEnter_1(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(File);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_1(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(File);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_2(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_2(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_3(object sender, MouseEventArgs e)
        {

            ImageAnimationController c = ImageBehavior.GetAnimationController(Pile);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_3(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_4(object sender, MouseEventArgs e)
        {

            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_4(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_5(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Play();

        }

        private void Grid_MouseLeave_5(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(tableau);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_6(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(matrice);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_6(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(matrice);
            if (c != null) c.Pause();
        }

        private void Grid_MouseLeave_7(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(Pile);
            if (c != null) c.Pause();
        }

        private void Grid_MouseEnter_7(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(liste);
            if (c != null) c.Play();
        }

        private void Grid_MouseLeave_8(object sender, MouseEventArgs e)
        {
            ImageAnimationController c = ImageBehavior.GetAnimationController(liste);
            if (c != null) c.Pause();
        }
        #endregion

        private async void Grid_MouseEnter_8(object sender, MouseEventArgs e)
        {

           
            
        }

        private void Grid_MouseLeave_9(object sender, MouseEventArgs e)
        {
         
           
            
        }
    }
}

