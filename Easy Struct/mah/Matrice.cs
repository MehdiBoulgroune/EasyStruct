using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mah
{

    class Matrice
    {
        private int nbLignes;
        private int nbColonnes;
        private Case[,] tab;
        private double coordX;
        private double coordY;
        private static SolidColorBrush couleurComPrincipal = Brushes.Yellow;
        private static SolidColorBrush couleurCom = Brushes.DarkTurquoise;
        private SolidColorBrush couleurFondCase = Brushes.White;
        private SolidColorBrush couleurBordureCase = Brushes.Black;
        private static SolidColorBrush couleurClignottement = Brushes.PaleTurquoise;
        private static SolidColorBrush couleurSelection = Brushes.Gainsboro;
        private static SolidColorBrush couleurAlgo = Brushes.White;
        // Les attributs couleurFondCase et couleurBordureCase ne sont utilisés 
        //qu'à l'interieur de la classe donc pas de getters ni de setters

        /*********** LES CONSTANTES ***********/
        private const int tailleMaxTab = 20;
        private const double widthOfcase = 50;
        private const double heightOfcase = 50;
        private const double a = widthOfcase / 2 - 10, b = 55;
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;
        /***************************************/

        public Matrice(int nbLines, int nbColumns, double coordX, double coordY)    //Constructeur de la matrice
        {
            nbLignes = nbLines;         //Initialisation du nombre de lignes et de colonnes, et des coordonnées
            nbColonnes = nbColumns;
            this.coordX = coordX;
            this.coordY = coordY;
            this.tab = new Case[nbLines, nbColumns];
        }

        public Matrice() { }

        public void chargementAleatoir(Commentaire comPrincipal, Canvas c)      //Chargement aléatoir de la matrice
        {
            comPrincipal.Text = "La matrice est remplie de manière aléatoire";      //Indication de l'opération en cours dans l'interface
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            Random i = new Random();
            for (int j = 0; j < nbLignes; j++)
            {
                for (int k = 0; k < nbColonnes; k++) this.tab[j, k] = new Case(i.Next(0, 5), coordX + k * widthOfcase, coordY + j * heightOfcase, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
            }

        }




        public async Task chargement(Canvas c, Commentaire comPrincipal)        //chargement manuel de la matrice
        {
            comPrincipal.Text = "Chargement manuel en cours...";        //Indication de l'opération en cours dans l'interface
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            bool ok = false;        //Indique si la touche entrer a été appuyée
            Commentaire com1 = new Commentaire("Entrez les valeurs une à une\ndans la matrice", Brushes.Black, coordX, coordY - 60, 160, 50, couleurCom, Brushes.White);    //Interaction avec l'utilisateur
            com1.ajouterCanvas(c);
            com1.apparaitre(Temps.time);
            TextBox txtBox = new TextBox();     //Textbox qui servira à remplir la matrice
            txtBox.Width = widthOfcase / 2;
            txtBox.Height = heightOfcase / 2;
            txtBox.BorderThickness = new Thickness(0);
            txtBox.Text = "";
            txtBox.FontSize = 15;
            txtBox.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Return) ok = true;        //Traitement du cas où l'utilisateur appuie sur entrer
            };
            for (int i = 0; i < nbLignes; i++)      //Chargement initial de la matrice avec des 0
            {
                for (int j = 0; j < nbColonnes; j++) this.tab[i, j] = new Case(0, coordX + j * widthOfcase, coordY + i * heightOfcase, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
            }
            afficher(c);
            c.Children.Add(txtBox);
            for (int i = 0; i < nbLignes; i++) //Déplacement du textBox dans la matrice
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    Canvas.SetTop(txtBox, coordY + (i + 0.30) * heightOfcase);
                    Canvas.SetLeft(txtBox, coordX + (j + 0.35) * widthOfcase);
                    while ((txtBox.Text.Length == 0) || (!ok)) await Task.Delay(10);    //Le textBox change de position lorsque l'utilisateur appuie sur entrer
                    this.tab[i, j].Valeur = int.Parse(txtBox.Text);
                    txtBox.Text = "";
                    ok = false;
                }
            }
            c.Children.Remove(txtBox);
            com1.disparaitre(Temps.time);
            comPrincipal.Text = "Chargement manuel effectué";       //Indication que l'opération est terminée
        }

        public void afficher(Canvas c)
        {
            for (int j = 0; j < nbLignes; j++)      //Affichage de chaque case
            {
                for (int k = 0; k < nbColonnes; k++) this.tab[j, k].afficher(c);
            }

        }

        public void masquer(Canvas c)
        {
            for (int j = 0; j < nbLignes; j++)      //Masquer chaque case
            {
                for (int k = 0; k < nbColonnes; k++) this.tab[j, k].masquer(c);
            }

        }

        public async static Task produit(Matrice matA, Matrice matB, Matrice produit, Canvas c, double x, double y, Commentaire comPrincipal, Canvas Algo)
        /* Produit matriciel de la matrice A x la matrice B, et rangement du résultat dans la matrice Produit*/
        {
            Algo algo = new Algo(28, coordX_Algo, coordY_Algo);
            Commentaire com1 = new Commentaire("Nombre de colonnes de la première\nmatrice: " + matA.nbColonnes.ToString(), Brushes.Black, matA.coordX, matA.coordY - 60, 210, 50, couleurCom, Brushes.White);
            Commentaire com2 = new Commentaire("Nombre de lignes de la deuxieme\nmatrice: " + matB.nbLignes.ToString(), Brushes.Black, matB.coordX + matB.nbColonnes * widthOfcase + 20, matB.coordY + 50, 210, 50, couleurCom, Brushes.White);
            comPrincipal.Text = "Produit matriciel";        //Animation de la comparaison entre le nombre de colonnes de la matrice A et le nombre de lignes de la matrice B
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            com1.ajouterCanvas(c);
            com1.apparaitre(Temps.time);
            com2.ajouterCanvas(c);
            com2.apparaitre(Temps.time);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
            com1.disparaitre(Temps.time);
            com2.disparaitre(Temps.time);
            if (matA.nbColonnes == matB.nbLignes)
            {
                algo.afficher(Algo);        //Affichage de l'algorithme
                await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);   //Indication de l'étape en cours dans l'algo
                produit.nbLignes = matA.nbLignes;
                produit.nbColonnes = matB.nbColonnes;
                produit.coordX = x;
                produit.coordY = y;
                produit.tab = new Case[matA.nbLignes, matB.nbColonnes]; //Initialisation de la matrice produit
                comPrincipal.Width = 250;
                comPrincipal.Text = "Nombre de lignes = Nombre de colonnes\nProduit matriciel en cours...";
                comPrincipal.CouleurFond = couleurComPrincipal;
                await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                com1.Text = "Nombre de lignes de la première\nmatrice: " + matA.NbLignes.ToString();        //Récupération du nombre de lignes de la matrice A et du nombre de colonnes de la matrice B
                com1.CoordX = matA.coordX - 220;                                                            //Pour la création de la matrice produit
                com1.CoordY = matA.coordY + 50;
                com1.apparaitre(Temps.time);
                com2.Text = "Nombre de colonnes de la deuxième\nmatrice: " + matA.NbLignes.ToString();
                com2.CoordX = matB.coordX;
                com2.CoordY = matB.coordY - 60;
                com2.apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
                Commentaire com3 = new Commentaire("Matrice produit de type\n" + matA.nbLignes.ToString() + " x " + matB.nbColonnes.ToString(), Brushes.Black, x - 180, y + heightOfcase, 150, 50, couleurCom, Brushes.White);
                com3.ajouterCanvas(c);
                com3.apparaitre(Temps.time);
                Matrice squelette = new Matrice(matA.nbLignes, matB.nbColonnes, x, y);
                for (int i = 0; i < matA.nbLignes; i++)     //Création d'un corps vide pour la matrice produit
                {
                    for (int j = 0; j < matB.nbColonnes; j++)
                    {
                        squelette.tab[i, j] = new Case(0, x + j * widthOfcase, y + i * heightOfcase, 1, heightOfcase, widthOfcase, matA.couleurFondCase, couleurClignottement, 1);
                        squelette.tab[i, j].TextBLock.Opacity = 0;
                    }
                }
                squelette.afficher(c);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));

                TextBlock textBlock = new TextBlock();      //textBlock pour afficher l'oépration en cours (multiplications)
                textBlock.FontFamily = new FontFamily("Poiret One");
                textBlock.Foreground = Brushes.Blue;
                Canvas.SetLeft(textBlock, x /*+ (matB.nbColonnes + 1) * widthOfcase /2*/);
                Canvas.SetTop(textBlock, y - 50);
                c.Children.Add(textBlock);

                Point[] tabPoint = new Point[1];
                for (int i = 0; i < matA.nbLignes; i++)
                {
                    await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);       //Affichage de l'étape n cours dans l'algo
                    if (i != 0) com1.Text = (i + 1).ToString() + "ème ligne";   //Explication de l'étape en commentaire
                    else com1.Text = (i + 1).ToString() + "ère ligne";
                    com1.CoordX = matA.coordX - 100;
                    com1.CoordY = matA.coordY + 50 * i + 5;
                    com1.Height = 40;
                    com1.Width = 80;
                    com1.apparaitre(Temps.time);
                    for (int p = 0; p < matA.nbColonnes; p++) matA.tab[i, p].BackgroundColor = couleurSelection;    //Sélection de la ligne de la matrice A
                    for (int j = 0; j < matB.nbColonnes; j++)
                    {
                        if (j != 0) com2.Text = (j + 1).ToString() + "ème colonne";
                        else com2.Text = (j + 1).ToString() + "ère colonne";
                        com2.CoordX = matB.coordX + 50 * j - 20;
                        com2.CoordY = matB.coordY - 50;
                        com2.Height = 40;
                        com2.Width = 90;
                        com2.apparaitre(Temps.time);
                        for (int p = 0; p < matB.nbLignes; p++) matB.tab[p, j].BackgroundColor = couleurSelection;      //Sélection de la colonne de la matrice B
                        await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);       //Affichag de l'étape en cours dans l'algo
                        await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
                        int a = 0;
                        await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                        for (int k = 0; k < matA.nbColonnes; k++)       //Produit matriciel
                        {
                            await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                            await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                            //await Task.Delay(50);
                            matA.tab[i, k].clignoter(couleurClignottement, matA.couleurBordureCase, matA.tab[i, k].BorderThick, 2);     //clignottement des cases à multiplier
                            matB.tab[k, j].clignoter(couleurClignottement, matB.couleurBordureCase, matB.tab[k, j].BorderThick, 2);
                            textBlock.FontSize = 15;
                            if (k != 0) textBlock.Text += " + " + matA.tab[i, k].Valeur.ToString() + " x " + matB.tab[k, j].Valeur.ToString();
                            else textBlock.Text = matA.tab[i, k].Valeur.ToString() + " x " + matB.tab[k, j].Valeur.ToString();        //Affichage de l'opération dans le textBlock
                            a += matA.tab[i, k].Valeur * matB.tab[k, j].Valeur;
                            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                            matA.tab[i, k].colorChamp(couleurSelection, matA.couleurBordureCase, 1);
                            matB.tab[k, j].colorChamp(couleurSelection, matB.couleurBordureCase, 1);
                            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                        }
                        produit.tab[i, j] = new Case(a, x, y - heightOfcase / 2, 1, heightOfcase, widthOfcase, matA.couleurFondCase, couleurClignottement, 1);
                        produit.tab[i, j].Forme.Opacity = 0;
                        produit.tab[i, j].afficher(c);
                        textBlock.Text = "";
                        tabPoint[0] = new Point(x + j * widthOfcase, y + i * heightOfcase);
                        await produit.tab[i, j].appear(tabPoint, 1);        //calcul du résultat et déplacement dans la matrice produit
                        await Task.Delay(50);
                        produit.tab[i, j].Forme.Opacity = 1;
                        for (int p = 0; p < matB.nbLignes; p++) matB.tab[p, j].colorChamp(matB.couleurFondCase, matB.couleurBordureCase, 1);
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
                    }
                    for (int p = 0; p < matA.nbColonnes; p++) matA.tab[i, p].colorChamp(matA.couleurFondCase, matA.couleurBordureCase, 1);
                    await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);
                }
                com1.disparaitre(Temps.time);
                com2.disparaitre(Temps.time);
                com3.disparaitre(Temps.time);
                squelette.masquer(c);
                comPrincipal.Text = "Produit effectué avec succès !";
                comPrincipal.CouleurFond = couleurComPrincipal;
                for (int i = 0; i < produit.nbLignes; i++) for (int j = 0; j < produit.nbColonnes; j++) produit.tab[i, j].BorderColor = matA.couleurBordureCase;
                await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                algo.disparaitre(Algo);
            }
            else
            {
                comPrincipal.CouleurFond = Brushes.Red;         //Commentaire d'échec de produit
                comPrincipal.Text = "Impossible de faire le produit\nLe nombre de colonnes de la première matrice\nest différent du nombre de lignes de la 2ème";
                await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
            }
        }

        public async Task trans(double x, double y, Canvas c, Commentaire comPrincipal, Matrice transposée, Canvas Algo)        //Transposée d'une matrice
        {
            Algo algo = new Algo(29, coordX_Algo, coordY_Algo);
            algo.afficher(Algo);
            Commentaire com1 = new Commentaire("Matrice de type\n" + nbLignes + " x " + nbColonnes.ToString(), Brushes.Black, coordX, coordY - 60, 210, 50, couleurCom, Brushes.White);
            comPrincipal.Text = "Transposition d'une matrice";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            com1.ajouterCanvas(c);
            com1.apparaitre(Temps.time);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            Commentaire com2 = new Commentaire("Matrice transposée de type\n" + nbColonnes.ToString() + " x " + nbLignes.ToString(), Brushes.Black, x + nbLignes * widthOfcase + 30, y + heightOfcase, 150, 50, couleurCom, Brushes.White);
            com2.ajouterCanvas(c);
            com2.apparaitre(Temps.time);
            await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
            Matrice squelette = new Matrice(nbColonnes, nbLignes, x, y);        //Création d'un corps de la matrice transposée
            for (int i = 0; i < nbColonnes; i++)
            {
                for (int j = 0; j < nbLignes; j++)
                {
                    squelette.tab[i, j] = new Case(0, x + j * widthOfcase, y + i * heightOfcase, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                    squelette.tab[i, j].TextBLock.Opacity = 0;
                }
            }
            squelette.afficher(c);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
            com1.disparaitre(Temps.time);
            com2.disparaitre(Temps.time);
            Point[] tabPoint = new Point[1];
            for (int i = 0; i < nbLignes; i++)
            {
                await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);       //Affichage de l'étape dans l'algo
                if (i != 0)
                {
                    com1.Text = (i + 1).ToString() + "ème ligne";       //Explication de l'étape dans les commentaires
                    com2.Text = (i + 1).ToString() + "ème colonne";
                }
                else
                {
                    com1.Text = (i + 1).ToString() + "ère ligne";
                    com2.Text = (i + 1).ToString() + "ère colonne";
                }
                com1.CoordX = coordX - 100;
                com1.CoordY = coordY + 50 * i + 5;
                com1.Height = 40;
                com1.Width = 80;
                com1.apparaitre(Temps.time);
                com2.CoordX = x + 50 * i - 20;
                com2.CoordY = y - 50;
                com2.Height = 40;
                com2.Width = 90;
                com2.apparaitre(Temps.time);
                for (int j = 0; j < nbColonnes; j++) tab[i, j].colorChamp(couleurSelection, couleurBordureCase, 1);     //Les lignes la matrice sont les colonnes de la matrice transposée
                for (int j = 0; j < nbColonnes; j++) squelette.tab[j, i].colorChamp(couleurSelection, couleurBordureCase, 1);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                for (int j = 0; j < nbColonnes; j++)
                {
                    await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
                    await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
                    //await Task.Delay(50);
                    transposée.tab[j, i] = new Case(tab[i, j].Valeur, coordX + j * widthOfcase, coordY + i * heightOfcase, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                    transposée.tab[j, i].Forme.Opacity = 0;
                    transposée.tab[j, i].afficher(c);
                    tab[i, j].clignoter(couleurClignottement, couleurBordureCase, 1, 2);        //Déplacement de chaque élément de la ligne de la matrice
                    tabPoint[0] = new Point(x + i * widthOfcase, y + j * heightOfcase);         //Vers la colonne de la transposée
                    await transposée.tab[j, i].appear(tabPoint, 1);
                    await Task.Delay(50);
                    transposée.tab[j, i].Forme.Opacity = 1;
                    tab[i, j].colorChamp(couleurSelection, couleurBordureCase, 1);
                    await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);

                }
                for (int j = 0; j < nbColonnes; j++) tab[i, j].colorChamp(couleurFondCase, couleurBordureCase, 1);
                for (int j = 0; j < nbColonnes; j++) squelette.tab[j, i].colorChamp(couleurFondCase, couleurBordureCase, 1);
                await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);

            }
            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
            com1.disparaitre(Temps.time);
            com2.disparaitre(Temps.time);
            squelette.masquer(c);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time * 2));
            comPrincipal.Text = "Transposition de la matrice réussie !";
            algo.disparaitre(Algo);
        }

        private async void sarus(Case det, Canvas c, Commentaire comPrincipal, Canvas Algo)     //Déterminant par la méthode de Sarus
        {
            Algo algo = new Algo(30, coordX_Algo, coordY_Algo);     //Affichage de l'étape en cours dans l'algo
            algo.afficher(Algo);
            comPrincipal.Text = "Calcul de déterminant d'une matrice 3x3\npar la méthode de Sarus...";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            Commentaire[] com = new Commentaire[8];
            await algo.colorer(couleurAlgo, 0, Temps.time);
            for (int i = 0; i < 3; i++)     //Affichage des commentaires d'illustration
            {
                com[i] = new Commentaire("c" + (i + 1), Brushes.Black, coordX + (i + 0.25) * widthOfcase, coordY - 30, 25, 25, couleurCom, couleurCom);
                com[i].ajouterCanvas(c);
                com[i].apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            Matrice plus = new Matrice(nbLignes, nbColonnes + 2, coordX + (nbColonnes + 1) * widthOfcase, coordY);      //Création d'une matrice 3x5 contentant
            for (int i = 0; i < plus.nbLignes; i++)                                                           //les colonnes c1, c2, c3, c1, c2
            {
                for (int j = 0; j < nbColonnes + 2; j++)
                {
                    plus.tab[i, j] = new Case(tab[i, j % nbColonnes].Valeur, plus.coordX + j * widthOfcase, plus.coordY + i * heightOfcase, 1, heightOfcase, widthOfcase, couleurFondCase, couleurBordureCase, 1);
                }
            }
            plus.afficher(c);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time)); //Affichage des commentaires d'illustration
            for (int i = 0; i < 3; i++)
            {
                com[i + 3] = new Commentaire("c" + (i + 1), Brushes.Black, plus.coordX + (i + 0.25) * widthOfcase, plus.coordY - 30, 25, 25, couleurCom, couleurCom);
                com[i + 3].ajouterCanvas(c);
                com[i + 3].apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            for (int i = 0; i < 2; i++)
            {
                com[i + 6] = new Commentaire("c" + (i + 1), Brushes.Black, plus.coordX + (i + 3 + 0.25) * widthOfcase, plus.coordY - 30, 25, 25, Brushes.LightPink, Brushes.White);
                com[i + 6].ajouterCanvas(c);
                com[i + 6].apparaitre(Temps.time);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            TextBlock textBlock = new TextBlock();      //Textblock pour affichier l'opération en cours
            textBlock.FontFamily = new FontFamily("Poiret One");
            textBlock.Foreground = Brushes.Blue;
            Canvas.SetLeft(textBlock, coordX);
            Canvas.SetTop(textBlock, plus.coordY + (nbLignes + 1) * heightOfcase);
            c.Children.Add(textBlock);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            int b = 1;
            for (int i = 0; i < 3; i++)     //Parcours des diagonales de gauche à droite (sens positif)
            {
                int j;
                b = 1;
                await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);       //Affichage de l'étape en cours dans l'algo
                await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);
                for (j = 0; j < 3; j++)
                {
                    plus.tab[j, j + i].BackgroundColor = couleurSelection;
                }
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                for (j = 0; j < 3; j++)     //Cases de la diagonale
                {
                    await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
                    plus.tab[j, j + i].clignoter(couleurClignottement, plus.couleurBordureCase, plus.tab[j, j + i].BorderThick, 2); //clignottement de la case
                    textBlock.FontSize = 15;                                                                                //Et affichage de l'opération dans le textBlock
                    if (j != 0 && j != 2) textBlock.Text += " x " + plus.tab[j, j + i].Valeur.ToString();
                    else if (j != 0 && j == 2) textBlock.Text += " x " + plus.tab[j, j + i].Valeur.ToString() + ")";
                    else if (i != 0) textBlock.Text += plus.tab[j, j + i].Valeur.ToString();
                    else if (i == 0) textBlock.Text = "(" + plus.tab[j, j + i].Valeur.ToString();
                    await algo.colorer(couleurAlgo, 4, Temps.time);     //étape en cours dans l'algo
                    if (i != 2 && j == 2) textBlock.Text += " + (";
                    plus.tab[j, j + i].colorChamp(couleurSelection, plus.couleurBordureCase, 1);
                    b = b * plus.tab[j, j + i].Valeur;
                    await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                }
                for (j = 0; j < 3; j++) plus.tab[j, j + i].BackgroundColor = couleurFondCase;
                await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);
                det.Valeur += b;
                await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);
            }
            for (int i = 4; i > 1; i--)     //Parcours des diagonales de droite à gauche (sens négatif)
            {
                await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);   //Étapes de l'algo en cours
                await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);
                int j;
                b = 1;
                for (j = 0; j < 3; j++)
                {
                    plus.tab[j, i - j].BackgroundColor = couleurSelection;      //illustration avec couleurs
                }
                textBlock.Text += " - (";
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
                for (j = 0; j < 3; j++)     //Parcours des cases de la diagonale
                {
                    await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);      //Affichage de l'étape en cours dans l'algo
                    plus.tab[j, i - j].clignoter(couleurClignottement, plus.couleurBordureCase, plus.tab[j, i - j].BorderThick, 2);
                    textBlock.FontSize = 15;
                    if (j != 0 && j != 2) textBlock.Text += " x " + plus.tab[j, i - j].Valeur.ToString();
                    else if (j != 0 && j == 2) textBlock.Text += " x " + plus.tab[j, i - j].Valeur.ToString() + ")";
                    else textBlock.Text += /*" - (" +*/ plus.tab[j, i - j].Valeur.ToString();
                    await algo.colorer(couleurAlgo, 11, Temps.time);
                    plus.tab[j, i - j].colorChamp(couleurSelection, plus.couleurBordureCase, 1);
                    b = b * plus.tab[j, i - j].Valeur;
                    await algo.colorer(couleurAlgo, 12, 0.5 * Temps.time);
                }
                for (j = 0; j < 3; j++) plus.tab[j, i - j].BackgroundColor = couleurFondCase;
                await algo.colorer(couleurAlgo, 13, 0.5 * Temps.time);
                await algo.colorer(couleurAlgo, 14, 0.5 * Temps.time);
                det.Valeur -= b;
            }
            det.Height = heightOfcase * 0.5;        //Mise en forme de la case contenant le déterminant
            det.Width = widthOfcase * 2;
            det.CoordX = coordX + 3 * widthOfcase;
            det.CoordY = plus.coordY + (nbLignes + 1) * heightOfcase;
            det.Forme.Opacity = 0;
            det.afficher(c);
            textBlock.Text = "";
            Point[] tabPoint = new Point[1];
            tabPoint[0] = new Point(coordX + widthOfcase / 2, coordY + (nbLignes + 0.5) * heightOfcase);
            await det.appear(tabPoint, 1);          //Affichage du déterminant sous la matrice
            await Task.Delay(50);
            det.Forme.Opacity = 1;
            det.clignoter(Brushes.LightSkyBlue, Brushes.White, 1, 2);
            for (int i = 0; i < 8; i++) com[i].disparaitre(Temps.time);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            for (int i = 0; i < 8; i++) com[i].enleverCanvas(c);
            comPrincipal.Text = "Le déterminant de la matrice est " + det.Valeur;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            plus.masquer(c);
            await algo.colorer(couleurAlgo, 15, 0.5 * Temps.time);
            algo.disparaitre(Algo);
        }

        private async void detMat2(Case det, Canvas c, Commentaire comPrincipal, Canvas Algo)       //Déterminant d'une matrice 2x2
        {
            Algo algo = new Algo(31, coordX_Algo, coordY_Algo);
            algo.afficher(Algo);
            comPrincipal.Text = "Calcul de déterminant d'une matrice 2x2...";
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.apparaitre(0);
            await algo.colorer(couleurAlgo, 0, Temps.time);
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            TextBlock textBlock = new TextBlock();
            textBlock.FontFamily = new FontFamily("Poiret One");
            textBlock.Foreground = Brushes.Blue;
            textBlock.FontSize = 15;
            Canvas.SetLeft(textBlock, coordX);
            Canvas.SetTop(textBlock, coordY + (nbLignes + 0.5) * heightOfcase);
            c.Children.Add(textBlock);


            await algo.colorer(couleurAlgo, 1, Temps.time);     //Clignottement des valeurs de la diagonale (sens positif)
            tab[0, 0].BackgroundColor = couleurSelection;       //et calcul du déterminant avec illustration dans le textblock
            tab[1, 1].BackgroundColor = couleurSelection;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 0].clignoter(couleurClignottement, couleurBordureCase, 1, 2);
            textBlock.Text = "(" + tab[0, 0].Valeur;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 0].BackgroundColor = couleurSelection;
            await Task.Delay(50);
            tab[1, 1].clignoter(couleurClignottement, couleurBordureCase, 1, 2);
            textBlock.Text += " x " + tab[1, 1].Valeur + ")";
            tab[1, 1].BackgroundColor = couleurSelection;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 0].BackgroundColor = couleurFondCase;
            tab[1, 1].BackgroundColor = couleurFondCase;
            textBlock.Text += " - (";

            tab[0, 1].BackgroundColor = couleurSelection;       //Clignottement des valeurs de la diagonale (sens négatif)
            tab[1, 0].BackgroundColor = couleurSelection;       //et calcul du déterminant avec illustration dans le textblock
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 1].clignoter(couleurClignottement, couleurBordureCase, 1, 2);
            textBlock.Text += tab[0, 1].Valeur;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 1].BackgroundColor = couleurSelection;
            await Task.Delay(50);
            tab[1, 0].clignoter(couleurClignottement, couleurBordureCase, 1, 2);
            textBlock.Text += " x " + tab[1, 0].Valeur + ")";
            tab[1, 0].BackgroundColor = couleurSelection;
            await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            tab[0, 1].BackgroundColor = couleurFondCase;
            tab[1, 0].BackgroundColor = couleurFondCase;

            det.Valeur = tab[0, 0].Valeur * tab[1, 1].Valeur - tab[1, 0].Valeur * tab[0, 1].Valeur;     //Mise en forme de la case contenant le determinant
            det.Height = heightOfcase * 0.5;
            det.Width = widthOfcase * 2;
            det.CoordX = coordX;
            det.CoordY = coordY + (nbLignes + 0.5) * heightOfcase;
            det.Forme.Opacity = 0;
            det.afficher(c);
            textBlock.Text = "";
            Point[] tabPoint = new Point[1];
            tabPoint[0] = new Point(coordX, coordY + (nbLignes + 0.5) * heightOfcase);
            await det.appear(tabPoint, 1);      //Déplacement du déterminant sous la matrice
            await Task.Delay(50);
            det.Forme.Opacity = 1;
            det.clignoter(Brushes.LightSkyBlue, Brushes.White, 1, 2);      //Clignottement du résultat
            comPrincipal.Text = "Le déterminant de la matrice est " + det.Valeur;
            await algo.colorer(couleurAlgo, 2, Temps.time);
            algo.disparaitre(Algo);
        }

        public async Task Determinant(Case det, Canvas c, Commentaire comPrincipal, Canvas Algo)
        /*Méthode qui traite toutes sortes de déterminants (1x1, 2x2, 3x3 et gestion des erreurs)*/
        {
            if ((nbLignes == 1) && (nbColonnes == 1))       //Matrice 1x1
            {
                det.Valeur = tab[0, 0].Valeur;      //Determinant = valeur de la case
                det.Height = heightOfcase * 0.5;        //Mise en forme de la case du déterminant
                det.Width = widthOfcase * 2;
                det.CoordX = coordX - widthOfcase / 2;
                det.CoordY = coordY + (nbLignes + 0.5) * heightOfcase;
                det.Forme.Opacity = 0;
                det.afficher(c);
                Point[] tabPoint = new Point[1];
                tabPoint[0] = new Point(coordX - widthOfcase / 2, coordY + (nbLignes + 0.5) * heightOfcase);
                await det.appear(tabPoint, 1);
                await Task.Delay(50);
                det.Forme.Opacity = 1;
                det.clignoter(Brushes.LightSkyBlue, Brushes.White, 1, 2);
                comPrincipal.Text = "Le déterminant de la matrice est " + det.Valeur;
                comPrincipal.CouleurFond = couleurComPrincipal;
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            else if ((nbLignes == 2) && (nbColonnes == 2)) detMat2(det, c, comPrincipal, Algo);     //Cas matrice 2x2
            else if ((nbLignes == 3) && (nbColonnes == 3)) sarus(det, c, comPrincipal, Algo);       //Cas matrice 3x3
            else if (nbLignes != nbColonnes)        //cas de la matrice qui n'est pas carrée
            {
                comPrincipal.Text = "Impossible de calculer le déterminant\nLa matrice n'est pas carrée";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
            else            //Cas non raités précédemment
            {
                comPrincipal.Text = "Impossible de calculer le déterminant\nL'ordre maximal est 3";
                comPrincipal.CouleurFond = Brushes.Red;
                comPrincipal.apparaitre(0);
                await Task.Delay(TimeSpan.FromSeconds(Temps.time));
            }
        }


        /*Getters et setters*/

        public Case[,] Tab
        {
            get { return tab; }
            set { tab = value; }
        }

        public int NbColonnes
        {
            get { return nbColonnes; }
            set { nbColonnes = value; }
        }

        public int NbLignes
        {
            get { return nbLignes; }
            set { nbLignes = value; }
        }

        public double CoordX
        {
            get { return coordX; }
            set
            {
                coordX = value;
                for (int j = 0; j < nbLignes; j++)
                {
                    for (int k = 0; k < nbColonnes; k++)
                    {
                        this.tab[j, k].CoordX = value + k * widthOfcase;
                    }
                }
            }
        }

    }

}

