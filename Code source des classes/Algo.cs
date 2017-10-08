using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace mah
{
    class Algo
    {
        class Champ_Algo //Classe interne 
        {
            private Rectangle champ; // Bordure de l'algorithme
            private TextBlock[] textBlock; // Ligne de l'algorithme
            private SolidColorBrush couleurFondAlgo = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc9c"));
            public void Settext(String text, int i) //Affecter un texte au textBlock
            {
                this.textBlock[i].Text = text;
            }
            public double opacity 
            {
                get { return champ.Opacity; }
                set
                {
                    champ.Opacity = value;
                    for (int i = 0; i < textBlock.Length; i++) textBlock[i].Opacity = value;
                }
            }

            public double Height
            {
                get { return champ.Height; }
                set
                {
                    champ.Height = value;
                    for (int i = 0; i < textBlock.Length; i++) textBlock[i].Height = value;
                }
            }


            public double Width
            {
                get { return champ.Width; }
                set
                {
                    champ.Width = value;
                    for (int i = 0; i < textBlock.Length; i++) textBlock[i].Width = value;
                }
            }


            public double CoordX
            {
                get { return Canvas.GetLeft(champ); }
                set
                {
                    Canvas.SetLeft(champ, value);
                    for (int i = 0; i < textBlock.Length; i++) Canvas.SetLeft(textBlock[i], value + 5);
                }
            }
            public double CoordY
            {
                get { return Canvas.GetTop(champ); }
                set
                {
                    Canvas.SetTop(champ, value);
                    for (int i = 0; i < textBlock.Length; i++) Canvas.SetTop(textBlock[i], value + 5);
                }
            }
            public SolidColorBrush CouleurText
            {
                get { return (SolidColorBrush)textBlock[0].Foreground; }
                set { for (int i = 0; i < textBlock.Length; i++) textBlock[i].Foreground = value; }
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

            public Champ_Algo()
            {

            }
            public Champ_Algo(int nbligne, SolidColorBrush couleurText, double coordx, double coordy, double width, double height, SolidColorBrush couleurFond, SolidColorBrush couleurBordure)
              //Constructeur d'un champ d'algorithme
            {
                textBlock = new TextBlock[nbligne];
                for (int i = 0; i < nbligne; i++) //Inistialisation des TextBlocks
                {
                    textBlock[i] = new TextBlock();
                    textBlock[i].FontFamily = new FontFamily("Century Gothic");
                    textBlock[i].FontSize = 12;
                    textBlock[i].FontStretch = FontStretches.UltraExpanded;
                    textBlock[i].FontStyle = FontStyles.Italic;
                    textBlock[i].FontWeight = FontWeights.UltraBold;
                    textBlock[i].Typography.NumeralStyle = FontNumeralStyle.OldStyle;
                    textBlock[i].Typography.SlashedZero = true;

                }
                champ = new Rectangle();
                Canvas.SetLeft(champ, coordx);
                Canvas.SetTop(champ, coordy - 3);

                for (int i = 0; i < nbligne; i++)//Inistialisation des TextBlocks
                {
                    Canvas.SetLeft(textBlock[i], coordx);
                    Canvas.SetTop(textBlock[i], coordy + i * height);
                    textBlock[i].Width = width;
                    textBlock[i].Height = height;
                    textBlock[i].Background = couleurFond;
                    textBlock[i].LineHeight = Double.NaN;
                    textBlock[i].TextAlignment = TextAlignment.Left;
                    textBlock[i].TextWrapping = TextWrapping.Wrap;
                }
                champ.Width = width;
                champ.Height = height * nbligne + 3;
                champ.Fill = Brushes.Transparent;
                champ.StrokeThickness = 3;
                champ.Stroke = couleurBordure;
                champ.Opacity = 1;
                for (int i = 0; i < nbligne; i++)
                    textBlock[i].Foreground = couleurText;
            }
            public void ajouterCanvas(Canvas c) //Ajoute au canvas 
            {
                for (int i = 0; i < textBlock.Length; i++) c.Children.Add(this.textBlock[i]);
                c.Children.Add(this.champ);

            }

            public void enleverCanvas(Canvas c)//Enleve du canvas
            {
                c.Children.Remove(this.champ);
                for (int i = 0; i < textBlock.Length; i++)
                    c.Children.Remove(this.textBlock[i]);
            }

            public void disparaitre(double time, int i) //Fais disparaitre une ligne de l'algorithme
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = 1;
                da.To = 0.5;
                da.Duration = TimeSpan.FromSeconds(time);
                textBlock[i].BeginAnimation(TextBlock.OpacityProperty, da);
            }

            public void apparaitre(double time, int i)//Fais apparaitre un textblock(une ligne) de l'algorithme
            {

                DoubleAnimation da = new DoubleAnimation();
                da.From = 0.5;
                da.To = 1;
                da.Duration = TimeSpan.FromSeconds(time);
                textBlock[i].BeginAnimation(TextBlock.OpacityProperty, da);
            }


            public async Task colorer(SolidColorBrush CouleurFond, int i, double time)/*Colore une ligne de l'algorithme*/
            {
                disparaitre(time * 0.5, i); //faire disparaitre la ligne
                textBlock[i].Background = CouleurFond;  //changer la couleur du fond 
                textBlock[i].FontWeight = FontWeights.ExtraBold;
                apparaitre(0.5 * time, i); //faire réapparaitre la ligne
                await Task.Delay(TimeSpan.FromSeconds(time)); //affichage de la couleur pendant un temps 
                disparaitre(0.5 * time, i); //remise de la couleur initial 
                textBlock[i].Background = couleurFondAlgo;
                textBlock[i].FontWeight = FontWeights.DemiBold;
                apparaitre(0.5 * time, i);
            }
        }
        Champ_Algo tab_algo;
        /***Coordonées de l'algorithme ***/
        private double coordX;
        private double coordY;
        /***Couleur du bloc d'algorithme***/
        private SolidColorBrush couleurFondAlgo = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc9c"));
        private SolidColorBrush couleurBordureAlgo = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1abc9c"));
        private SolidColorBrush couleurTexte = Brushes.Black;
        public Algo(int type_algo, double coordX, double coordY)
            //Constructeur du bloc d'algorithme
        {
            /**Initialisation des coordonées **/
            this.coordX = coordX;
            this.coordY = coordY;
            /** Intanciation des différents algorithmes de l'application**/
            /**************************TABLEAU ************************************/
            if (type_algo == 1)//Recherche séquentielle 
            {
                tab_algo = new Champ_Algo(8, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         Trouve ← Faux ", 1);
                tab_algo.Settext("         i ← 0 ", 2);
                tab_algo.Settext("         Tant que (non trouv et i< taille ) faire ", 3);
                tab_algo.Settext("             Si (valeur=T[i]) alors", 4);
                tab_algo.Settext("                Trouve ← Vrai ", 5);
                tab_algo.Settext("             i++ ", 6);
                tab_algo.Settext("      FIN ", 7);
            }
            if (type_algo == 2)//Recherche dichotomique
            {
                tab_algo = new Champ_Algo(12, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("        min ← 0 ", 1);
                tab_algo.Settext("        max ←  taille - 1", 2);
                tab_algo.Settext("        Tant que (non trouve et min < max ) faire", 3);
                tab_algo.Settext("            milieu ←  (min + max) / 2", 4);
                tab_algo.Settext("            si (T[milieu] = valeur) alors trouve=vrai ", 5);
                tab_algo.Settext("            sinon  ", 6);
                tab_algo.Settext("                si valeur > T[milieu] alors ", 7);
                tab_algo.Settext("                   min ←  milieu + 1", 8);
                tab_algo.Settext("                sinon valeur < T[milieu] ", 9);
                tab_algo.Settext("                   max ← milieu - 1", 10);
                tab_algo.Settext("      FIN ", 11);
            }
            if (type_algo == 3)//Tri par sélection
            {
                tab_algo = new Champ_Algo(6, Brushes.Black, coordX, coordY, 400, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         pour i allant de 0 à taille-1 ", 1);
                tab_algo.Settext("             min ← L'indice du plus petit élèment du tableau", 2);
                tab_algo.Settext("             si min ≠ i, alors permute entre T[i] et T[min]  ", 3);
                tab_algo.Settext("         Fin pour ", 4);
                tab_algo.Settext("      FIN ", 5);
            }
            if (type_algo == 4)//Tri par transposition
            {
                tab_algo = new Champ_Algo(12, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        Pour i allant de 0 à taille-1 ", 1);
                tab_algo.Settext("          Si T[i] > T[i+1] alors", 2);
                tab_algo.Settext("              permuter(T[i],T[i+1])", 3);
                tab_algo.Settext("              j ← i - 1", 4);
                tab_algo.Settext("              Tant que (j > 0 Et T[j] < T[j - 1]) faire ", 5);
                tab_algo.Settext("                       permuter(T[j],T[j-1])  ", 6);
                tab_algo.Settext("                       j ← j - 1 ", 7);
                tab_algo.Settext("              FIN Tant que ", 8);
                tab_algo.Settext("          FIN Si ", 9);
                tab_algo.Settext("        FIN pour ", 10);
                tab_algo.Settext("      FIN ", 11);
            }
            if (type_algo == 5)//Tri à bulle
            {
                tab_algo = new Champ_Algo(8, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         pour i allant de taille - 1 à 0 ", 1);
                tab_algo.Settext("            pour j allant de 0 à i -1", 2);
                tab_algo.Settext("                  si T[j + 1] < T[j]", 3);
                tab_algo.Settext("                     permuter(T[j + 1], T[j]) ", 4);
                tab_algo.Settext("            Fin pour ", 5);
                tab_algo.Settext("         Fin pour ", 6);
                tab_algo.Settext("      FIN ", 7);
            }
            if (type_algo == 6)//Insertion trié
            {
                tab_algo = new Champ_Algo(9, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("        i ← taille ", 1);
                tab_algo.Settext("        Tant que (i > 0 et T[i-1] > valeur) faire", 2);
                tab_algo.Settext("           T[i] ← T[i-1]", 3);
                tab_algo.Settext("           i ← i-1", 4);
                tab_algo.Settext("        Fin tantque ", 5);
                tab_algo.Settext("        T[i] ← valeur ", 6);
                tab_algo.Settext("        taille ← taille + 1", 7);
                tab_algo.Settext("      FIN ", 8);
            }
            if (type_algo == 7)//Suppression trié
            {
                tab_algo = new Champ_Algo(8, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        i ← p ", 1);
                tab_algo.Settext("        Tantque (i < taille) faire", 2);
                tab_algo.Settext("           T[i] ← T[i+1]", 3);
                tab_algo.Settext("           i ← i+1", 4);
                tab_algo.Settext("        Fin tantque ", 5);
                tab_algo.Settext("        taille ← taille - 1", 6);
                tab_algo.Settext("      FIN ", 7);

            }
            /******************************************LISTE ***************************************************************/
            if (type_algo == 8)//Recherche liste séquentielle
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("       SI (Tete ≠ Nil) ALORS ", 1);
                tab_algo.Settext("          P ← Tete", 2);
                tab_algo.Settext("          Trouve ← Faux", 3);
                tab_algo.Settext("          TANTQUE (P ≠ Nil ET Non Trouve)", 4);
                tab_algo.Settext("               SI P.Info = Val ALORS ", 5);
                tab_algo.Settext("                     Trouve ← Vrai", 6);
                tab_algo.Settext("               SINON", 7);
                tab_algo.Settext("                     P ← P.Suivant", 8);
                tab_algo.Settext("               FINSI", 9);
                tab_algo.Settext("          FIN TANT QUE", 10);
                tab_algo.Settext("       FIN SI", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 9)//Recherche liste par position
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("       SI (Tete ≠ Nil) ALORS ", 1);
                tab_algo.Settext("          P ← Tete", 2);
                tab_algo.Settext("          Trouve ← Faux", 3);
                tab_algo.Settext("          TANTQUE (P ≠ Nil ET Non Trouve)", 4);
                tab_algo.Settext("               SI (P = Pos) ALORS ", 5);
                tab_algo.Settext("                     Trouve ← Vrai", 6);
                tab_algo.Settext("               SINON", 7);
                tab_algo.Settext("                     P ← P.Suivant", 8);
                tab_algo.Settext("               FINSI", 9);
                tab_algo.Settext("          FIN TANT QUE", 10);
                tab_algo.Settext("       FIN SI", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 10)//Suppression par valeur 
            {
                tab_algo = new Champ_Algo(10, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← La postion renvoyée par la recherche ", 1);
                tab_algo.Settext("        SI (P ≠ Nil) ALORS ", 2);
                tab_algo.Settext("          SI (P = tete) ALORS ", 3);
                tab_algo.Settext("             Tete ← Tete.Suivant", 4);
                tab_algo.Settext("             Désallouer(P)", 5);
                tab_algo.Settext("          SINON ", 6);
                tab_algo.Settext("             Préc.Suivant ← P.Suivant", 7);
                tab_algo.Settext("             Désallouer(P)", 8);
                tab_algo.Settext("      FIN", 9);
            }
            if (type_algo == 11)//Insertion par valeur 
            {
                tab_algo = new Champ_Algo(9, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        SI (Tete = NIL) ALORS ", 1);
                tab_algo.Settext("            Tete ← Nouvel élement ", 2);
                tab_algo.Settext("            Nouvel élement.Suivant ← NIL ", 3);
                tab_algo.Settext("        SINON ", 4);
                tab_algo.Settext("            P ← La postion renvoyée par la recherche ", 5);
                tab_algo.Settext("            Nouvel élement.Suivant ← P.SUIVANT", 6);
                tab_algo.Settext("            P.Suivant ← Nouvel élement", 7);
                tab_algo.Settext("      FIN", 8);
            }
            if (type_algo == 12)//Recherche liste_Bi qeue 
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("       SI (Tete ≠ Nil) ALORS ", 1);
                tab_algo.Settext("          P ← Tete", 2);
                tab_algo.Settext("          Trouve ← Faux", 3);
                tab_algo.Settext("          TANTQUE (P ≠ Nil ET Non Trouve)", 4);
                tab_algo.Settext("               SI P.Info = Val ALORS ", 5);
                tab_algo.Settext("                     Trouve ← Vrai", 6);
                tab_algo.Settext("               SINON", 7);
                tab_algo.Settext("                     P ← P.Précédent ", 8);
                tab_algo.Settext("               FINSI", 9);
                tab_algo.Settext("          FIN TANT QUE", 10);
                tab_algo.Settext("       FIN SI", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 13)//Insertion Bi par valeur 
            {
                tab_algo = new Champ_Algo(12, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        SI (Tete = NIL) ALORS ", 1);
                tab_algo.Settext("            Tete ← Nouvel_Element ", 2);
                tab_algo.Settext("            Nouvel_Element.Suivant ← NIL ", 3);
                tab_algo.Settext("            Nouvel_Element.Précedent ← NIL ", 4);
                tab_algo.Settext("        SINON ", 5);
                tab_algo.Settext("            P ← La postion renvoyée par la recherche ", 6);
                tab_algo.Settext("            Nouvel_Element.Suivant ← P.Suivant", 7);
                tab_algo.Settext("            P.Precédent ← Nouvel_Element", 8);
                tab_algo.Settext("            P.Suivant ← Nouvel_Element", 9);
                tab_algo.Settext("            Nouvel_Element.Précedent ← P", 10);
                tab_algo.Settext("      FIN", 11);
            }
            if (type_algo == 14)//Suppression Bi par valeur 
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← La postion renvoyée par la recherche ", 1);
                tab_algo.Settext("        Q ← Le précedent de P ", 2);
                tab_algo.Settext("        SI (P ≠ Nil) ALORS ", 3);
                tab_algo.Settext("          SI (P = tete) ALORS ", 4);
                tab_algo.Settext("             Tete ← Tete.Suivant", 5);
                tab_algo.Settext("             Tete.Préc ← NIL", 6);
                tab_algo.Settext("             Désallouer(P)", 7);
                tab_algo.Settext("          SINON ", 8);
                tab_algo.Settext("             Q.Suivant ← P.Suivant", 9);
                tab_algo.Settext("             P.Suivant.Préc ← P.Préc", 10);
                tab_algo.Settext("             Désallouer(P)", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 15)//Recherche liste_Circulaire séquentielle
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("       SI (Tete ≠ Nil) ALORS ", 1);
                tab_algo.Settext("          P ← Tete", 2);
                tab_algo.Settext("          Trouve ← Faux", 3);
                tab_algo.Settext("          Faire ", 4);
                tab_algo.Settext("               SI (P.Info = Val) ALORS ", 5);
                tab_algo.Settext("                     Trouve ← Vrai", 6);
                tab_algo.Settext("               SINON", 7);
                tab_algo.Settext("                     P ← P.Suivant", 8);
                tab_algo.Settext("               FINSI", 9);
                tab_algo.Settext("         TANTQUE (P ≠ Tete ET Non Trouve)", 10);
                tab_algo.Settext("       FIN SI", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 16)//Suppression circulaire par valeur 
            {
                tab_algo = new Champ_Algo(12, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← La postion renvoyée par la recherche ", 1);
                tab_algo.Settext("        Préc ← Le précedent de P ", 2);
                tab_algo.Settext("        SI (P ≠ Nil) ALORS ", 3);
                tab_algo.Settext("          SI (P = tete) ALORS ", 4);
                tab_algo.Settext("             Désallouer(P)", 5);
                tab_algo.Settext("             Tete ← Préc", 6);
                tab_algo.Settext("             Tete.Suivant ← P.Suivant", 7);
                tab_algo.Settext("          SINON ", 8);
                tab_algo.Settext("             Préc.Suivant ← P.Suivant", 9);
                tab_algo.Settext("             Désallouer(P)", 10);
                tab_algo.Settext("      FIN", 11);
            }
            /*******************************************ARBRE**********************************/
            if (type_algo == 17)//Recherche arbre
            {
                tab_algo = new Champ_Algo(13, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← Racine", 1);
                tab_algo.Settext("        TANT QUE (P ≠ Nil ET Non Trouve) FAIRE", 2);
                tab_algo.Settext("            SI (valeur < val(P)) alors ", 3);
                tab_algo.Settext("                P ← fils gauche(P) ", 4);
                tab_algo.Settext("            SINON ", 5);
                tab_algo.Settext("               SI (valeur > val(P)) ", 6);
                tab_algo.Settext("                   P ← fils droit(P)", 7);
                tab_algo.Settext("               SINON ", 8);
                tab_algo.Settext("                   Trouve ← Vrai  ", 9);
                tab_algo.Settext("            FINSI", 10);
                tab_algo.Settext("        FIN TANT QUE", 11);
                tab_algo.Settext("      FIN", 12);
            }
            if (type_algo == 18)//Parcourt pré-ordre
            {
                tab_algo = new Champ_Algo(17, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← Racine", 1);
                tab_algo.Settext("        Q ← Crée Pile ", 2);
                tab_algo.Settext("        TANT QUE (Non Pile Vide(P) ET Non Stop) FAIRE", 3);
                tab_algo.Settext("            SI (P=NIL) alors ", 4);
                tab_algo.Settext("                Dépiler(P) ", 5);
                tab_algo.Settext("                TANT QUE (P ≠ NIL ET P.fils droit = NIL FAIRE", 6);
                tab_algo.Settext("                   Dépiler(P) ", 7);
                tab_algo.Settext("                SI (P=NIL) alors ", 8);
                tab_algo.Settext("                   Stop ← Vrai  ", 9);
                tab_algo.Settext("                SINON ", 10);
                tab_algo.Settext("                   P ← P.fils droit", 11);
                tab_algo.Settext("            SINON", 12);
                tab_algo.Settext("                 Empiler(P)", 13);
                tab_algo.Settext("                 P ← P.fils gauche", 14);
                tab_algo.Settext("        FIN TANT QUE", 15);
                tab_algo.Settext("      FIN", 16);

            }
            if (type_algo == 19)//Parcourt InOrdre
            {
                tab_algo = new Champ_Algo(7, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        SI (P.fils gauche ≠ NIL )", 1);
                tab_algo.Settext("           Inordre(P.fils gauche)", 2);
                tab_algo.Settext("        Visiter(P) ", 3);
                tab_algo.Settext("        SI (P.fils droit ≠ NIL )", 4);
                tab_algo.Settext("           Inordre(P.fils droit)", 5);
                tab_algo.Settext("      FIN", 6);
            }
            if (type_algo == 20)//Parcourt PostOrdre
            {
                tab_algo = new Champ_Algo(7, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        SI (P.fils gauche ≠ NIL )", 1);
                tab_algo.Settext("           Postordre(P.fils gauche)", 2);
                tab_algo.Settext("        SI (P.fils droit ≠ NIL )", 3);
                tab_algo.Settext("           Postordre(P.fils droit)", 4);
                tab_algo.Settext("        Visiter(P) ", 5);
                tab_algo.Settext("      FIN", 6);
            }
            if (type_algo == 21)//Insértion Arbre Binaire 
            {
                tab_algo = new Champ_Algo(9, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← L'adresse renvoyé par la recherche", 1);
                tab_algo.Settext("        SI (P = NIL) alors  Racine ← Crée_noeud(Valeur)", 2);
                tab_algo.Settext("        SINON ", 3);
                tab_algo.Settext("             SI (valeur < clé(P)) alors", 4);
                tab_algo.Settext("                  P.fils_gauche ← Crée_noeud(Valeur)", 5);
                tab_algo.Settext("             SINON SI (valeur>clé(P)) ", 6);
                tab_algo.Settext("                  P.fils_droit ← Crée_noeud(Valeur)", 7);
                tab_algo.Settext("      FIN", 8);
            }
            if (type_algo == 22)//Suppression Arbre Binaire
            {
                tab_algo = new Champ_Algo(17, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        P ← Noeud renvoyé par la recherche", 1);
                tab_algo.Settext("        SI (P.fils_gauche = NIL ET P.fils_droit = NIL)", 2);
                tab_algo.Settext("            Libérer(P)", 3);
                tab_algo.Settext("        SINON  ", 4);
                tab_algo.Settext("          SI(P.fils_droit = NIL)", 5);
                tab_algo.Settext("                  P ← P.fils_gauche ", 6);
                tab_algo.Settext("                  Libérer(P.fils_gauche) ", 7);
                tab_algo.Settext("          SINON SI(P.fils_gauche = NIL) ", 8);
                tab_algo.Settext("                  P ← P.fils_droit ", 9);
                tab_algo.Settext("                  Libérer(P.fils_droit) ", 10);
                tab_algo.Settext("                SINON", 11);
                tab_algo.Settext("                  Q ← Suivant_inorde(P)", 12);
                tab_algo.Settext("                  P ← Q", 13);
                tab_algo.Settext("                  Libérer(Q)", 14);
                tab_algo.Settext("        FINSIN", 15);
                tab_algo.Settext("      FIN", 16);

            }
            /*******************************************File et Pile**************************************************/
            if (type_algo == 23)//Empiler
            {
                tab_algo = new Champ_Algo(1, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      Ajout de la valeur dans la Pile ", 0);
            }
            if (type_algo == 24)//Dépiler
            {
                tab_algo = new Champ_Algo(1, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      Suppression de la valeur de Pile ", 0);
            }
            if (type_algo == 25)//Enfiler
            {
                tab_algo = new Champ_Algo(1, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      Ajout de la valeur dans la File ", 0);
            }
            if (type_algo == 26)//Défiler
            {
                tab_algo = new Champ_Algo(1, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      Suppression de la valeur de la File ", 0);
            }

            if (type_algo == 27)//Insertion Tableau non trié 
            {
                tab_algo = new Champ_Algo(1, Brushes.Black, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      Insertion de la valeur à la fin du tableau ", 0);
            }
            /******************************MATRICE******************************/
            if (type_algo == 28)//Produit matriciel
            {
                tab_algo = new Champ_Algo(10, couleurTexte, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         Pour i allant de 1 à n faire ", 1);
                tab_algo.Settext("             Pour j allant de 1 à m faire  ", 2);
                tab_algo.Settext("                C[i,j] ← 0 ", 3);
                tab_algo.Settext("                Pour k allant de 1 à p faire ", 4);
                tab_algo.Settext("                    C[i,j] ← C[i,j] + A[i,k]*B[k,j]", 5);
                tab_algo.Settext("                FinPour ", 6);
                tab_algo.Settext("              FinPour ", 7);
                tab_algo.Settext("         FinPour ", 8);
                tab_algo.Settext("      FIN ", 9);

            }
            if (type_algo == 29)//transposée Matrice
            {
                tab_algo = new Champ_Algo(7, couleurTexte, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         Pour i allant de 1 à n faire ", 1);
                tab_algo.Settext("             Pour j allant de 1 à m faire  ", 2);
                tab_algo.Settext("                  B[i,j] ← A[j,i] ", 3);
                tab_algo.Settext("              FinPour ", 4);
                tab_algo.Settext("         FinPour ", 5);
                tab_algo.Settext("      FIN ", 6);

            }
            if (type_algo == 30)//Déterminant sarus Matrice
            {
                tab_algo = new Champ_Algo(16, couleurTexte, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT", 0);
                tab_algo.Settext("        Pour i allant de 1 à 3 faire ", 1);
                tab_algo.Settext("           produit ← 1", 2);
                tab_algo.Settext("           Pour j allant de 1 à 3 faire ", 3);
                tab_algo.Settext("              Produit ← produit * T[j, j + i]  ", 4);
                tab_algo.Settext("           FinPour ", 5);
                tab_algo.Settext("           déterminant ← déterminant + produit ", 6);
                tab_algo.Settext("        FinPour", 7);
                tab_algo.Settext("        Pour i allant 5 à 3 faire", 8);
                tab_algo.Settext("           produit ← 1;", 9);
                tab_algo.Settext("           Pour j allant de 1 à 3 faire", 10);
                tab_algo.Settext("              produit ← produit * T[j, i - j]", 11);
                tab_algo.Settext("           FinPour", 12);
                tab_algo.Settext("           déterminant ← déterminant - produit", 13);
                tab_algo.Settext("        FinPour", 14);
                tab_algo.Settext("      FIN", 15);

            }
            if (type_algo == 31)//Déterminant 2*2
            {
                tab_algo = new Champ_Algo(3, couleurTexte, coordX, coordY, 300, 20, couleurFondAlgo, couleurBordureAlgo);
                tab_algo.Settext("      DEBUT ", 0);
                tab_algo.Settext("         Déterminant ← T[0,0]*T[1,1]-T[0,1]*T[1,0] ", 1);
                tab_algo.Settext("      FIN", 2);
            }
        }
        public void afficher(Canvas c)//Affiche l'algorithme
        {
            tab_algo.ajouterCanvas(c);
        }
        public void disparaitre(Canvas c)//Enleve l'algorithme du canvas
        {
            tab_algo.enleverCanvas(c);
        }
        public async Task colorer(SolidColorBrush CouleurFond, int i, double time) //Colore la ligne i de l'algorithme
        {
            await this.tab_algo.colorer(CouleurFond, i, time);
        }
    }
}
