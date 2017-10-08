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
    class Arbre
    {
        public Noeud Racine { get; set; }
        public int Profondeur { get; set; }
        private SolidColorBrush couleurFondNoeud = Brushes.White;
        private SolidColorBrush couleurBordureNoeud = Brushes.Black;
        private SolidColorBrush couleurComPrincipal = Brushes.Yellow;
        private SolidColorBrush couleurAlgo = Brushes.White;

        /************** CONSTANTES****************/
        private const double distance = 50;
        private const double coordX = 650;
        private const double coordY = 27;
        private const double heightNoeud = 50;
        private const double widthNoued = 50;
        private const double epaisseurBrdNoeud = 2;
        private const double longeurLienDebut = 110;
        private const double angleDebut = 75;
        private const double coordX_Algo = 0;
        private const double coordY_Algo = 0;

        public Arbre()  // Constructeur de l'Arbre
        {
            Racine = null;
            Profondeur = -1;
        }


        public async void insererSansAnim(int valeur) // insere une valeur "valeur" dans l'arbre sans animation
        {
            Noeud p = Racine;
            Noeud q = null;
            Noeud newNoeud = null;

            Boolean trouv = false;

            while (!trouv && p != null)
            {
                q = p;
                if (valeur < p.Valeur) p = p.filsGauche;
                else if (valeur > p.Valeur) p = p.filsDroit;
                else trouv = true;
            }

            if (!trouv)
            {
                if (q == null)
                {
                    Racine = new Noeud(valeur, coordX, coordY, heightNoeud, widthNoued, couleurFondNoeud, couleurBordureNoeud, epaisseurBrdNoeud, longeurLienDebut, angleDebut, 0, null);

                    newNoeud = Racine;


                }
                else
                {
                    if (valeur < q.Valeur)
                    {
                        newNoeud = new Noeud(valeur, q.lienGauche.CoordX2 - widthNoued / 2, q.lienGauche.CoordY2 - heightNoeud / 2, heightNoeud, widthNoued, couleurFondNoeud, couleurBordureNoeud, epaisseurBrdNoeud, longeurLienDebut, angleDebut, q.Niveau + 1, q);
                        q.filsGauche = newNoeud;


                    }
                    else
                    {
                        newNoeud = new Noeud(valeur, q.lienDroit.CoordX2 - widthNoued / 2, q.lienDroit.CoordY2 - heightNoeud / 2, heightNoeud, widthNoued, couleurFondNoeud, couleurBordureNoeud, epaisseurBrdNoeud, longeurLienDebut, angleDebut, q.Niveau + 1, q);
                        q.filsDroit = newNoeud;

                    }
                }
            }


            while (newNoeud != null && newNoeud.pere != null)
            {

                if ((SifilsGauche(newNoeud) == 1) && (SifilsGauche(newNoeud.pere) == 0))
                {
                    deplacerSousArbresansAnim(newNoeud.pere, -longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180), 0);
                    newNoeud.pere.pere.lienGauche.CoordX2 = newNoeud.pere.pere.lienGauche.CoordX2 - longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180);


                }

                else if ((SifilsGauche(newNoeud) == 0) && (SifilsGauche(newNoeud.pere) == 1))
                {

                    deplacerSousArbresansAnim(newNoeud.pere, longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180), 0);
                    newNoeud.pere.pere.lienDroit.CoordX2 = newNoeud.pere.pere.lienDroit.CoordX2 + longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180);


                }

                newNoeud = newNoeud.pere;
            }
        }

        public void afficher(Noeud p, Canvas c)
        //Affiche l'arbre
        {
            if (p != null)
            {
                p.afficher(c);
                if (p.filsGauche != null) afficher(p.filsGauche, c);
                if (p.filsDroit != null) afficher(p.filsDroit, c);
            }
        }

        public void masquer(Noeud p, Canvas c)
        // Masque l'arbre
        {
            if (p != null)
            {
                p.masquer(c);
                if (p.filsGauche != null) masquer(p.filsGauche, c);
                if (p.filsDroit != null) masquer(p.filsDroit, c);
            }
        }

        public void CorrectionDesErreur(Noeud racine)
            // Corrige les erreurs dans la disposition des Noeuds
        {
            if (racine != null)
            {

                if (SifilsGauche(racine) == 0)
                {
                    racine.pere.lienGauche.CoordX2 = racine.CoordX + widthNoued / 2;
                }
                else if (SifilsGauche(racine) == 1)
                {
                    racine.pere.lienDroit.CoordX2 = racine.CoordX + widthNoued / 2;
                }

                CorrectionDesErreur(racine.filsGauche);
                CorrectionDesErreur(racine.filsDroit);
            }

        }

        public async void chrgmentAleatoire(int nbValeur, Canvas c)
            // Intialise l'arbre avec des valeurs aléatoires 
        {
            Random rndNumber = new Random();
            Boolean stop = new Boolean();
            int[] tabValeur = new int[nbValeur];
            int i;
            tabValeur[0] = rndNumber.Next(25, 30);
            for (i = 1; i < nbValeur; i++)
            {
                stop = true;
                while (stop == true)
                {
                    tabValeur[i] = rndNumber.Next(0, 40);
                    stop = false;
                    for (int j = i - 1; j >= 0; j--)
                        if (tabValeur[i] == tabValeur[j]) stop = true;
                }
            }


            for (i = 0; i < nbValeur; i++)
            {
                insererSansAnim(tabValeur[i]);
            }

            CorrectionDesErreur(Racine);
            afficher(Racine, c);
        }


        public void elargir(Noeud p, int niveauInf, int niveauSup, double dAngle, double dLongeur)
            // Elargit l'arbre pour permettre une bonne disposition des noeuds
        {
            if (p != null && p.Niveau <= niveauSup && p.Niveau >= niveauInf)
            {
                p.elargirSansAnim(p.lienDroit.Angle - (dAngle / (p.Niveau + 1)), p.lienGauche.Longeur + (dLongeur / (p.Niveau + 0.5)));
                if (p.filsGauche != null)
                {
                    p.filsGauche.repositionner(p.lienGauche.CoordX2 - widthNoued / 2, p.lienGauche.CoordY2 - heightNoeud / 2);
                    elargir(p.filsGauche, niveauInf, niveauSup, dAngle, dLongeur);
                }
                if (p.filsDroit != null)
                {
                    p.filsDroit.repositionner(p.lienDroit.CoordX2 - widthNoued / 2, p.lienDroit.CoordY2 - heightNoeud / 2);
                    elargir(p.filsDroit, niveauInf, niveauSup, dAngle, dLongeur);
                }
            }
        }

        public async Task recherche(int valeur, int[] trouv, Noeud[] tabNoeud, Canvas c, Commentaire comPrincipal, Canvas Algo)
        //Recherche la valeur 'valeur' dans l'arbre 
        // tabNoeud[1] est le noeud qui contien la valeur 'valeur' si il existe  , sinon il est à null et tabNoeud[0] est le pere de tabNoeud[1]
        {
            Algo algo = new Algo(17, coordX_Algo, coordY_Algo);
            trouv[0] = 0; // Intialise trouv a faux 
            Noeud p = Racine;
            tabNoeud[0] = null;
            tabNoeud[1] = null;
            if (Racine != null) // Si l'arbre n'est pas vide
            {
                List<Noeud> listNouedParcourus = new List<Noeud>(); // on instancie la liste des noeuds qui vont etre parcourus
                int i;
                SolidColorBrush couleurDeparcourt = new SolidColorBrush();
                SolidColorBrush couleurTrouvFond = new SolidColorBrush();
                SolidColorBrush couleurTrouvBrd = new SolidColorBrush();
                couleurTrouvFond = Brushes.LightGreen;
                couleurTrouvBrd = Brushes.Green;
                couleurDeparcourt = Brushes.CornflowerBlue;
                algo.afficher(Algo);
                Lien lienDeParcourt = new Lien(Racine.lienGauche.CoordX1, Racine.lienGauche.CoordY1, 0, Racine.lienGauche.Angle, couleurDeparcourt, 8); // Lien qui trace le chemin suivi dans la recherche 
                Canvas.SetZIndex(lienDeParcourt.Ligne, 1);
                lienDeParcourt.afficher(c);
                comPrincipal.Text = "Recherche dans l'arbre en cours...";
                comPrincipal.Width = 200;
                comPrincipal.Height = 50;
                comPrincipal.CouleurFond = couleurComPrincipal;
                comPrincipal.CouleurBordure = Brushes.Black;
                comPrincipal.apparaitre(0);
                await algo.colorer(couleurAlgo, 0, 0.4 * Temps.time);
                await algo.colorer(couleurAlgo, 1, 0.4 * Temps.time);
                Commentaire comDeParcours = new Commentaire("", Brushes.Black, coordX - 450, coordY, 400, 50, couleurDeparcourt, Brushes.Black); // Commentaire qui explique les étapes du parcours
                comDeParcours.ajouterCanvas(c);
                comDeParcours.disparaitre(0);
                while (trouv[0] == 0 && p != null)
                {
                    await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                    listNouedParcourus.Add(p); // on ajoute le noeud p dans la liste des noeuds parcourus 
                    p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);

                    if (valeur < p.Valeur) // Si la valeur qu'on recherche est inferieure à la valeur du noeud courant on parcourt le sous arbre gauche de ce dernier      
                    {
                        comDeParcours.apparaitre(0.5); // faire apparaitre le commentaire de parcours
                        if (p.filsGauche != null)
                        {
                            comDeParcours.Text = valeur + "<" + p.Valeur + " donc on parcourt le sous arbre gauche du noeud";
                            comDeParcours.apparaitre(0.5);
                            await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                            // await Task.Delay(TimeSpan.FromSeconds(3));
                            comDeParcours.disparaitre(0.5);
                            lienDeParcourt.Ligne.Opacity = 1;
                            lienDeParcourt.CoordX1 = p.lienGauche.CoordX1;
                            lienDeParcourt.CoordY1 = p.lienGauche.CoordY1;
                            await lienDeParcourt.deplacerX2Y2(p.lienGauche.CoordX1, p.lienGauche.CoordY1, p.filsGauche.lienGauche.CoordX1, p.filsGauche.lienGauche.CoordY1);
                            p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                            p.lienGauche.Couleur = couleurDeparcourt;
                            lienDeParcourt.Ligne.Opacity = 0;
                        }
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        tabNoeud[0] = p;
                        p = p.filsGauche;

                    }
                    else if (valeur > p.Valeur)// Si la valeur qu'on recherche est superieure à la valeur du noeud courant on parcourt le sous arbre droit de ce dernier 
                    {
                        await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        comDeParcours.apparaitre(0);
                        if (p.filsDroit != null)
                        {
                            comDeParcours.Text = valeur + ">" + p.Valeur + " donc on parcourt le sous arbre droit du noeud";
                            comDeParcours.apparaitre(0.5);
                            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                            comDeParcours.disparaitre(0.5);
                            lienDeParcourt.Ligne.Opacity = 1;
                            lienDeParcourt.CoordX1 = p.lienDroit.CoordX1;
                            lienDeParcourt.CoordY1 = p.lienDroit.CoordY1;
                            await lienDeParcourt.deplacerX2Y2(p.lienDroit.CoordX1, p.lienDroit.CoordY1, p.filsDroit.lienDroit.CoordX1, p.filsDroit.lienDroit.CoordY1); // Affiche l'état en cours dans l'algorithme déroulant
                            p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                            p.lienDroit.Couleur = couleurDeparcourt;
                            lienDeParcourt.Ligne.Opacity = 0;
                        }
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);  // Affiche l'état en cours dans l'algorithme déroulant
                        tabNoeud[0] = p;
                        p = p.filsDroit;
                    }
                    else // si la valeur du noeud courant est egale à la valeur qu'on cherche
                    {
                        await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        p.colorChamp(couleurTrouvFond, couleurTrouvBrd, 3);
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        trouv[0] = 1; // trouv recoit vrai
                        tabNoeud[1] = p;
                    }
                    await algo.colorer(couleurAlgo, 10, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                }
                await algo.colorer(couleurAlgo, 11, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                comDeParcours.disparaitre(0.5);
                comDeParcours.enleverCanvas(c);
                comPrincipal.disparaitre(0.5);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                if (trouv[0] == 1)
                {
                    comPrincipal.CouleurFond = couleurTrouvFond;
                    comPrincipal.CouleurBordure = couleurTrouvBrd;
                    comPrincipal.Text = "La valeur " + valeur + " a été trouvée";
                }
                else
                {
                    tabNoeud[0].colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    comPrincipal.CouleurFond = Brushes.Red;
                    comPrincipal.CouleurBordure = Brushes.DarkRed;
                    comPrincipal.Text = "La valeur " + valeur + " n'a pas été trouvée";

                }
                comPrincipal.apparaitre(0.5);
                await Task.Delay(TimeSpan.FromSeconds(3.5 + Champ.time));
                comPrincipal.disparaitre(0);
                await algo.colorer(couleurAlgo, 12, 0.5 * Temps.time);
                algo.disparaitre(Algo);
                foreach (Noeud n in listNouedParcourus)
                {

                    n.BackgroundColor = couleurFondNoeud;
                    n.BorderColor = couleurBordureNoeud;
                    n.lienGauche.Couleur = couleurBordureNoeud;
                    n.lienDroit.Couleur = couleurBordureNoeud;
                    n.BorderThick = 2;
                }
            }

        }

        public async Task parcoursPreordre(Commentaire comPrincipal, Canvas c, Canvas Algo)
            // Effectue un parcours préordre dans l'arbre
        {
            Noeud p = Racine;
            int i;
            Noeud[] tabNouedParcoru = new Noeud[1000];
            int longeurTab = 0;
            Case[] tabCases = new Case[1000];
            Boolean stop = false;
            Algo algo = new Algo(18, coordX_Algo, coordY_Algo);
            SolidColorBrush couleurDeNumDeNoeud = new SolidColorBrush();
            couleurDeNumDeNoeud = Brushes.DarkSlateBlue;
            SolidColorBrush couleurDeparcourt = new SolidColorBrush();
            couleurDeparcourt = Brushes.CornflowerBlue;
            Commentaire comDeParcour = new Commentaire("L'ordre des valeurs selon le parcours est :\n", Brushes.White, coordX - 450, coordY, 300, 100, couleurDeparcourt, couleurDeparcourt);
            comDeParcour.ajouterCanvas(c);
            Lien lienDeParcourt = new Lien(Racine.lienGauche.CoordX1, Racine.lienGauche.CoordY1, 0, Racine.lienGauche.Angle, couleurDeparcourt, 8);
            Canvas.SetZIndex(lienDeParcourt.Ligne, 1);
            lienDeParcourt.afficher(c);
            comPrincipal.Text = "Parcours preordre en cours...";
            comPrincipal.Width = 200;
            comPrincipal.Height = 50;
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(0);
            algo.afficher(Algo);
            Stack<Noeud> pile = new Stack<Noeud>();
            pile.Push(null);
            await algo.colorer(couleurAlgo, 0, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
            await algo.colorer(couleurAlgo, 1, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
            await algo.colorer(couleurAlgo, 2, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
            while (pile.Count != 0 && !stop)
            {
                await algo.colorer(couleurAlgo, 3, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                if (p == null)
                {
                    await algo.colorer(couleurAlgo, 4, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                    await algo.colorer(couleurAlgo, 5, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant

                    p = pile.Pop();
                    while (p != null && p.filsDroit == null)
                    {
                        await algo.colorer(couleurAlgo, 6, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 7, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        p = pile.Pop();
                    }
                    if (p == null)
                    {
                        await algo.colorer(couleurAlgo, 8, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 9, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant

                        stop = true;
                    }
                    else
                    {
                        await algo.colorer(couleurAlgo, 10, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                        lienDeParcourt.Angle = p.lienDroit.Angle;
                        lienDeParcourt.CoordX1 = p.lienDroit.CoordX1;
                        lienDeParcourt.CoordY1 = p.lienDroit.CoordY1;
                        lienDeParcourt.Longeur = 0;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);
                        await lienDeParcourt.deplacerX2Y2(p.lienDroit.CoordX1, p.lienDroit.CoordY1, p.filsDroit.lienDroit.CoordX1, p.filsDroit.lienDroit.CoordY1); //lienDeParcourt.ChangeLongeurAnim(p.lienDroit.Longeur, time);
                        p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                        p.lienDroit.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                        p = p.filsDroit;
                    }
                }
                else
                {
                    await algo.colorer(couleurAlgo, 12, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                    p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2); // Change la couleur du noued parcouru
                    tabCases[longeurTab] = new Case(longeurTab + 1, p.CoordX + p.Width / 3, p.CoordY - p.Height / 2, 2, 23, 23, couleurDeNumDeNoeud, couleurDeNumDeNoeud, 2);
                    tabCases[longeurTab].TextBLock.Foreground = Brushes.WhiteSmoke;
                    tabCases[longeurTab].afficher(c);
                    Canvas.SetZIndex(tabCases[longeurTab].Forme, 3);
                    Canvas.SetZIndex(tabCases[longeurTab].TextBLock, 3);
                    if (longeurTab == 0) comDeParcour.Text = comDeParcour.Text + p.Valeur.ToString();
                    else if (longeurTab == 10) comDeParcour.Text = comDeParcour.Text + "," + p.Valeur.ToString() + "\n";
                    else comDeParcour.Text = comDeParcour.Text + "," + p.Valeur.ToString();
                    tabNouedParcoru[longeurTab] = p;
                    longeurTab++;
                    pile.Push(p);
                    if (p.filsGauche != null)
                    {
                        lienDeParcourt.CoordX1 = p.lienGauche.CoordX1;
                        lienDeParcourt.CoordY1 = p.lienGauche.CoordY1;
                        lienDeParcourt.Angle = p.lienGauche.Angle;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await lienDeParcourt.deplacerX2Y2(p.lienGauche.CoordX1, p.lienGauche.CoordY1, p.filsGauche.lienGauche.CoordX1, p.filsGauche.lienGauche.CoordY1);
                        p.lienGauche.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    }
                    await algo.colorer(couleurAlgo, 13, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                    await algo.colorer(couleurAlgo, 14, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                    p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    p = p.filsGauche;
                }
                await algo.colorer(couleurAlgo, 15, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
            }
            await Task.Delay(TimeSpan.FromSeconds(4));
            comPrincipal.disparaitre(0.5);//Disparaitre le commentaire principal
            comDeParcour.disparaitre(0.5);//Disparaitre le commentaire de parcours
            comDeParcour.enleverCanvas(c);// Enleve le commentaire de parcours
            await algo.colorer(couleurAlgo, 16, 0.4 * Temps.time);
            algo.disparaitre(Algo);//Disparaitre l'algorithme 
            for (i = 0; i < longeurTab; i++)
            {
                tabNouedParcoru[i].BackgroundColor = couleurFondNoeud;
                tabNouedParcoru[i].BorderColor = couleurBordureNoeud;
                tabNouedParcoru[i].lienGauche.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].lienDroit.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].BorderThick = 2;
                tabCases[i].masquer(c);
            }
        }

        public async Task parcoursInordre(Commentaire comPrincipal, Canvas c, Canvas Algo)
        // Effectue un parcours inordre dans l'arbre
        {
            Noeud p = Racine;
            int i;
            Noeud[] tabNouedParcoru = new Noeud[1000];
            int longeurTab = 0;
            Case[] tabCases = new Case[1000];
            Boolean stop = false;
            Algo algo = new Algo(19, coordX_Algo, coordY_Algo);
            SolidColorBrush couleurDeNumDeNoeud = new SolidColorBrush();
            couleurDeNumDeNoeud = Brushes.DarkSlateBlue;
            SolidColorBrush couleurDeparcourt = new SolidColorBrush();
            couleurDeparcourt = Brushes.CornflowerBlue;
            Commentaire comDeParcour = new Commentaire("L'ordre des valeurs selon le parcours est :\n", Brushes.White, coordX - 450, coordY, 300, 100, couleurDeparcourt, couleurDeparcourt);
            Lien lienDeParcourt = new Lien(Racine.lienGauche.CoordX1, Racine.lienGauche.CoordY1, 0, Racine.lienGauche.Angle, couleurDeparcourt, 8);
            Canvas.SetZIndex(lienDeParcourt.Ligne, 1);
            lienDeParcourt.afficher(c);
            comPrincipal.Text = "Parcours inordre en cours...";
            comPrincipal.Width = 200;
            comPrincipal.Height = 50;
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(0);
            comDeParcour.ajouterCanvas(c);
            algo.afficher(Algo);// Affiche l'algorithme 
            Stack<Noeud> pile = new Stack<Noeud>();
            while (!stop)
            {
                while (p != null)
                {
                    p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                    pile.Push(p);
                    await algo.colorer(couleurAlgo, 0, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    if (p.filsGauche != null)
                    {
                        await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        lienDeParcourt.CoordX1 = p.lienGauche.CoordX1;
                        lienDeParcourt.CoordY1 = p.lienGauche.CoordY1;
                        lienDeParcourt.Angle = p.lienGauche.Angle;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await lienDeParcourt.deplacerX2Y2(p.lienGauche.CoordX1, p.lienGauche.CoordY1, p.filsGauche.lienGauche.CoordX1, p.filsGauche.lienGauche.CoordY1);
                        p.lienGauche.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                    }
                    else await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    if (p.filsGauche != null)
                    {
                        await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    }
                    p = p.filsGauche;
                }
                if (pile.Count != 0)
                {
                  
                    p = pile.Pop();
                    p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                    tabCases[longeurTab] = new Case(longeurTab + 1, p.CoordX + p.Width / 3, p.CoordY - p.Height / 2, 2, 23, 23, couleurDeNumDeNoeud, couleurDeNumDeNoeud, 2);
                    tabCases[longeurTab].TextBLock.Foreground = Brushes.WhiteSmoke;
                    tabCases[longeurTab].afficher(c);
                    Canvas.SetZIndex(tabCases[longeurTab].Forme, 3);
                    Canvas.SetZIndex(tabCases[longeurTab].TextBLock, 3);
                    await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    if (longeurTab == 0) comDeParcour.Text = comDeParcour.Text + p.Valeur.ToString();
                    else if (longeurTab == 10) comDeParcour.Text = comDeParcour.Text + "," + p.Valeur.ToString() + "\n";
                    else comDeParcour.Text = comDeParcour.Text + "," + p.Valeur.ToString();
                    tabNouedParcoru[longeurTab] = p;
                    longeurTab++;
                    if (p.filsDroit != null)
                    {
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                        lienDeParcourt.Angle = p.lienDroit.Angle;
                        lienDeParcourt.CoordX1 = p.lienDroit.CoordX1;
                        lienDeParcourt.CoordY1 = p.lienDroit.CoordY1;
                        lienDeParcourt.Longeur = 0;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await lienDeParcourt.deplacerX2Y2(p.lienDroit.CoordX1, p.lienDroit.CoordY1, p.filsDroit.lienDroit.CoordX1, p.filsDroit.lienDroit.CoordY1); //lienDeParcourt.ChangeLongeurAnim(p.lienDroit.Longeur, time);
                        p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                        p.lienDroit.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                    }
                    else await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    if (p.filsDroit != null)
                    {
                        await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                    }
                    p = p.filsDroit;
                }
                else {
                    stop = true;
                }

            }
            await Task.Delay(TimeSpan.FromSeconds(4));
            comPrincipal.disparaitre(0.5);//Disparaitre le commentaire principal
            comDeParcour.disparaitre(0.5);//Disparaitre le commentaire de parcours
            comDeParcour.enleverCanvas(c); // Enleve de l'animation le commentaire de parcours
            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
            for (i = 0; i < longeurTab; i++)
            {
                tabNouedParcoru[i].BackgroundColor = couleurFondNoeud;
                tabNouedParcoru[i].BorderColor = couleurBordureNoeud;
                tabNouedParcoru[i].lienGauche.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].lienDroit.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].BorderThick = 2;
                tabCases[i].masquer(c);//Disparaitre le commentaire principal
            }
            algo.disparaitre(Algo);
        }

        public async Task parcoursPostordre(Commentaire comPrincipal, Canvas c, Canvas Algo)
        // Effectue un postordre dans l'arbre
        {
            Noeud p = Racine, pPrime = new Noeud(), q = new Noeud();
            int i;
            Algo algo = new Algo(20, coordX_Algo, coordY_Algo);
            Noeud[] tabNouedParcoru = new Noeud[1000];
            int longeurTab = 0;
            Case[] tabCases = new Case[1000];
            Boolean stop = false, cond = true;
            SolidColorBrush couleurDeNumDeNoeud = new SolidColorBrush();
            couleurDeNumDeNoeud = Brushes.DarkSlateBlue;
            SolidColorBrush couleurDeparcourt = new SolidColorBrush();
            couleurDeparcourt = Brushes.CornflowerBlue;
            Commentaire comDeParcour = new Commentaire("L'ordre des valeurs selon le parcours est :\n", Brushes.White, coordX - 450, coordY, 300, 100, couleurDeparcourt, couleurDeparcourt);
            comDeParcour.ajouterCanvas(c);
            Lien lienDeParcourt = new Lien(Racine.lienGauche.CoordX1, Racine.lienGauche.CoordY1, 0, Racine.lienGauche.Angle, couleurDeparcourt, 8);
            Canvas.SetZIndex(lienDeParcourt.Ligne, 1);
            lienDeParcourt.afficher(c);
            comPrincipal.Text = "Parcours postordre en cours...";
            comPrincipal.Width = 200;
            comPrincipal.Height = 50;
            comPrincipal.CouleurFond = couleurComPrincipal;
            comPrincipal.CouleurBordure = Brushes.Black;
            comPrincipal.apparaitre(0);
            Stack<Noeud> pile = new Stack<Noeud>();
            algo.afficher(Algo); // Affiche l'algorithme 
            while (!stop)
            {
                while (p != null)
                {
                    await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    p.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                    pile.Push(p);
                    pPrime = p;
                    if (p.filsGauche != null)
                    {
                        await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        lienDeParcourt.CoordX1 = p.lienGauche.CoordX1;
                        lienDeParcourt.CoordY1 = p.lienGauche.CoordY1;
                        lienDeParcourt.Angle = p.lienGauche.Angle;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await lienDeParcourt.deplacerX2Y2(p.lienGauche.CoordX1, p.lienGauche.CoordY1, p.filsGauche.lienGauche.CoordX1, p.filsGauche.lienGauche.CoordY1);
                        p.lienGauche.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                    }
                    else await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    p.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    if (p.filsGauche != null)
                    {
                        await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    }
                    p = p.filsGauche;
                }
                if (pile.Count != 0)
                {
                    if (pPrime.filsDroit == null)
                    {
                        cond = true;
                        while (pile.Count != 0 && cond)
                        {
                            pPrime.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                            tabCases[longeurTab] = new Case(longeurTab + 1, pPrime.CoordX + pPrime.Width / 3, pPrime.CoordY - pPrime.Height / 2, 2, 23, 23, couleurDeNumDeNoeud, couleurDeNumDeNoeud, 2);
                            tabCases[longeurTab].TextBLock.Foreground = Brushes.WhiteSmoke;
                            tabCases[longeurTab].afficher(c);
                            Canvas.SetZIndex(tabCases[longeurTab].Forme, 3);
                            Canvas.SetZIndex(tabCases[longeurTab].TextBLock, 3);
                            await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                            if (longeurTab == 0 || longeurTab == 11) comDeParcour.Text = comDeParcour.Text + pPrime.Valeur.ToString();
                            else if (longeurTab == 10) comDeParcour.Text = comDeParcour.Text + "," + pPrime.Valeur.ToString() + ",\n";
                            else comDeParcour.Text = comDeParcour.Text + "," + pPrime.Valeur.ToString();
                            tabNouedParcoru[longeurTab] = pPrime;
                            longeurTab++;
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            pPrime.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                            q = pPrime;
                            pPrime = pile.Pop();
                            if (pile.Count != 0)
                            {
                                pPrime = pile.Peek();
                                cond = (pPrime.filsDroit == q);
                            }
                            else stop = true;
                        }
                    }
                    pPrime.colorChamp(couleurDeparcourt, couleurDeparcourt, 2);
                    if (pPrime.filsDroit != null && pile.Count != 0)
                    {
                        await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);
                        await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);
                        lienDeParcourt.Angle = pPrime.lienDroit.Angle;
                        lienDeParcourt.CoordX1 = pPrime.lienDroit.CoordX1;
                        lienDeParcourt.CoordY1 = pPrime.lienDroit.CoordY1;
                        lienDeParcourt.Longeur = 0;
                        lienDeParcourt.Ligne.Opacity = 1;
                        await lienDeParcourt.deplacerX2Y2(pPrime.lienDroit.CoordX1, pPrime.lienDroit.CoordY1, pPrime.filsDroit.lienDroit.CoordX1, pPrime.filsDroit.lienDroit.CoordY1);
                        pPrime.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                        pPrime.lienDroit.Couleur = couleurDeparcourt;
                        lienDeParcourt.Ligne.Opacity = 0;
                    }
                    else await Task.Delay(TimeSpan.FromSeconds(Champ.time));
                    pPrime.colorChamp(couleurFondNoeud, couleurDeparcourt, 3);
                    if (pPrime.filsDroit != null && pile.Count != 0)
                    {
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    }
                    p = pPrime.filsDroit;
                }
                else {
                    stop = true;
                }
            }
            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
            await Task.Delay(TimeSpan.FromSeconds(4));
            comPrincipal.disparaitre(0.5); //Disparaitre le commentaire principal
            comDeParcour.disparaitre(0.5); // Disparaitre le commentaire de parcours
            comDeParcour.enleverCanvas(c); // Enlever de l'animation le commentaire de parcours
            for (i = 0; i < longeurTab; i++)
            {
                tabNouedParcoru[i].BackgroundColor = couleurFondNoeud;
                tabNouedParcoru[i].BorderColor = couleurBordureNoeud;
                tabNouedParcoru[i].lienGauche.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].lienDroit.Couleur = couleurBordureNoeud;
                tabNouedParcoru[i].BorderThick = 2;
                tabCases[i].masquer(c);
            }
            algo.disparaitre(Algo); // Disparaitre l'algorithme 
        }
        public async Task suivant_inordre(Noeud R, Noeud[] pere_inordre, Noeud[] inordre)
        /* Donne le suivant inordre de R ainsi que son pere */

        {

            pere_inordre[0] = null;
            inordre[0] = null;

            if (R != null)
            {
                inordre[0] = (R.filsDroit);
                pere_inordre[0] = R;

                while ((inordre[0].filsGauche) != null)
                {
                    pere_inordre[0] = inordre[0];
                    inordre[0] = (inordre[0].filsGauche);
                }

            }

        }

        public async Task inserer(int valeur, Canvas c, Commentaire comPrincipal, Canvas Algo)
        {
            int[] trouv = new int[1];
            Noeud[] pere = new Noeud[2];
            Noeud newNoeud;
            Noeud q;
            Point[] tabPoint = new Point[1];
            Point[] tabPoint2 = new Point[1];
            Algo algo = new Algo(21, coordX, coordY);

            await this.recherche(valeur, trouv, pere, c, comPrincipal, Algo); // Effectue une recherche avant l'insertion 



            if (trouv[0] == 0) // le noeud n'existe pas dans l'arbre
            {
                algo.afficher(Algo);
                await algo.colorer(couleurAlgo, 0, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                await algo.colorer(couleurAlgo, 1, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                double coordX2, coordY2, newNoeudCoordX = coordX, newNoeudCoordY = coordY + 70;
                if (pere[0] == null) // l'arbre est vide, ie : le noeud qu'on va ajouter est la racine 
                {
                    await algo.colorer(couleurAlgo, 2, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    Racine = new Noeud(valeur, coordX, coordY, heightNoeud, widthNoued, couleurFondNoeud, couleurBordureNoeud, epaisseurBrdNoeud, longeurLienDebut, angleDebut, 0, null);
                    Racine.afficher(c);// Affiche la racine de l'arbre
                }
                else
                {
                    await algo.colorer(couleurAlgo, 3, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    newNoeud = new Noeud(valeur, newNoeudCoordX, newNoeudCoordY, heightNoeud, widthNoued, couleurFondNoeud, couleurBordureNoeud, 2, longeurLienDebut, angleDebut, pere[0].Niveau + 1, pere[0]);
                    newNoeud.afficher(c);// affiche le nouveau noeud à insérer   
                    await Task.Delay(TimeSpan.FromSeconds(1));


                    if (valeur < pere[0].Valeur)
                    {
                        await algo.colorer(couleurAlgo, 4, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        pere[0].lienGauche.Ligne.Opacity = 1;
                        coordX2 = pere[0].lienGauche.CoordX2;
                        coordY2 = pere[0].lienGauche.CoordY2;
                        tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);
                        pere[0].lienGauche.deplacerX2Y2(pere[0].lienGauche.CoordX1, pere[0].lienGauche.CoordY1, newNoeudCoordX + widthNoued / 2, newNoeudCoordY + heightNoeud / 2);
                        pere[0].lienGauche.deplacerX2Y2(newNoeudCoordX + widthNoued / 2, newNoeudCoordY + heightNoeud / 2, coordX2, coordY2);
                        newNoeud.deplacer(tabPoint, 1);
                        pere[0].filsGauche = newNoeud;
                        newNoeud.pere = pere[0];
                        await algo.colorer(couleurAlgo, 5, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                    }
                    else
                    {
                        await algo.colorer(couleurAlgo, 6, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        pere[0].lienDroit.Ligne.Opacity = 1;
                        coordX2 = pere[0].lienDroit.CoordX2;
                        coordY2 = pere[0].lienDroit.CoordY2;
                        tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);
                        pere[0].lienDroit.deplacerX2Y2(pere[0].lienDroit.CoordX1, pere[0].lienDroit.CoordY1, newNoeudCoordX + widthNoued / 2, newNoeudCoordY + heightNoeud / 2);
                        pere[0].lienDroit.deplacerX2Y2(newNoeudCoordX + widthNoued / 2, newNoeudCoordY + heightNoeud / 2, coordX2, coordY2);
                        newNoeud.deplacer(tabPoint, 1);
                        pere[0].filsDroit = newNoeud;
                        newNoeud.pere = pere[0]; // On affecte le pere du nouveau noeud
                        await algo.colorer(couleurAlgo, 7, 0.4 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    }
                    q = newNoeud;

                    while (q.pere != null)
                    {

                        if ((SifilsGauche(q) == 1) && (SifilsGauche(q.pere) == 0))
                        {
                            await q.pere.pere.lienGauche.deplacerX2Y2(q.pere.CoordX + widthNoued / 2, q.pere.CoordY + heightNoeud / 2, q.pere.CoordX - longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180) + widthNoued / 2, q.pere.CoordY + heightNoeud / 2);
                            deplacerSousArbre(q.pere, -longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180), 0);

                        }
                        else if ((SifilsGauche(q) == 0) && (SifilsGauche(q.pere) == 1))
                        {
                            await q.pere.pere.lienDroit.deplacerX2Y2(q.pere.CoordX + widthNoued / 2, q.pere.CoordY + heightNoeud / 2, q.pere.CoordX + longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180) + widthNoued / 2, q.pere.CoordY + heightNoeud / 2);
                            deplacerSousArbre(q.pere, +longeurLienDebut * Math.Cos((angleDebut * 3.14) / 180), 0);


                        }
                        q = q.pere;

                    }
                }
                await algo.colorer(couleurAlgo, 8, 0.4 * Temps.time); // Affiche l'état en cours dans l'algorithme déroulant
                algo.disparaitre(Algo); // Disparaite l'algorithme déroulant 
            }

        }

        public int SifilsGauche(Noeud p) 
            
        {
            if (p.pere == null)
            {
                return 2; // la racine
            }
            else
            {
                if (p.pere.filsGauche != null && p.pere.filsGauche.Valeur == p.Valeur)
                {
                    return 0; // 0 si c'est fils gauche 
                }
                else
                {
                    return 1; // 1 si fils droite 
                }
            }
        }
        public async Task deplacerSousArbre(Noeud n, double dx, double dy)
            // Déplace le sous arbre de Racine n  avec une distance dx sur les abcisse et dy sur les  ordonnée 
        {
            Noeud p = n;
            Noeud[] tabNouedParcoru = new Noeud[20];
            int longeurTab = 0;
            Stack<Noeud> pile = new Stack<Noeud>();
            Boolean stop = false;
            Point[] tabPoint = new Point[1];
            tabPoint[0] = new Point();
            while (!stop)
            {
                while (p != null)
                {
                    pile.Push(p);
                    p = p.filsGauche;
                }
                if (pile.Count != 0)
                {
                    p = pile.Pop();
                    longeurTab++;
                    tabPoint[0].X = p.CoordX + dx;
                    tabPoint[0].Y = p.CoordY + dy;
                    p.deplacer(tabPoint, 1);
                    p = p.filsDroit;
                }
                else stop = true;

            }
        }

        public async void deplacerSousArbresansAnim(Noeud n, double dx, double dy)
        // Déplace le sous arbre de Racine n sans animation  avec une distance dx sur les abcisse et dy sur les  ordonnée 
        {
            Noeud p = n;
            Noeud[] tabNouedParcoru = new Noeud[500];
            int longeurTab = 0;
            Stack<Noeud> pile = new Stack<Noeud>();
            Boolean stop = false;
            Point[] tabPoint = new Point[1];
            tabPoint[0] = new Point();
            while (!stop)
            {
                while (p != null)
                {
                    pile.Push(p);
                    p = p.filsGauche;
                }
                if (pile.Count != 0)
                {
                    p = pile.Pop();
                    longeurTab++;
                    p.repositionner(p.CoordX + dx, p.CoordY + dy);
                    p = p.filsDroit;
                }
                else stop = true;

            }
        }
        public async Task supprimer_noeud(int valeur, Canvas c, Commentaire comPrincipal, Canvas Algo)
            //Supprime le noeud qui contient la valeur "valeur" de l'arbre
        {

            Noeud[] pere = new Noeud[2];
            double t;
            int[] trouv = new int[1];
            Point[] tabPoint = new Point[1];
            await this.recherche(valeur, trouv, pere, c, comPrincipal, Algo); // d'abord on recherche si la valeur "valeur" est contenu dans l'arbre
            double coordX2, coordY2;

            Noeud[] s = new Noeud[1];
            Noeud[] pere_inordre = new Noeud[1];
            Noeud f = new Noeud();

            Algo algo = new Algo(22, coordX_Algo, coordY_Algo);

            if (pere[1] != null)

            {
                algo.afficher(Algo); // Affiche l'agorithme de la suppression 
                await algo.colorer(couleurAlgo, 0, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                await algo.colorer(couleurAlgo, 1, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                comPrincipal.Text = "Suppression en cours ...";
                comPrincipal.apparaitre(0);
                comPrincipal.CouleurFond = Brushes.Red;

                pere[1].colorChamp(Brushes.Red, Brushes.DarkRed, 3);
                await Task.Delay(TimeSpan.FromSeconds(1.5));

                if ((pere[1].filsDroit == null) && (pere[1].filsGauche == null))
                {
                    if (pere[0] != null)
                    {
                        if ((pere[0].filsGauche) != null)
                        {
                            if ((pere[0].filsGauche.Valeur) == (pere[1].Valeur))
                            {
                                await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                coordX2 = pere[0].lienGauche.Ligne.X2;
                                coordY2 = pere[0].lienGauche.Ligne.Y2;
                                pere[0].filsGauche.masquer(c);
                                await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await pere[0].lienGauche.deplacerX2Y2(pere[0].lienGauche.CoordX2, pere[0].lienGauche.CoordY2, pere[0].lienGauche.CoordX1, pere[0].lienGauche.CoordY1);
                                t = Temps.time;
                                Temps.time = 0;
                                pere[0].lienGauche.deplacerX2Y2(pere[0].lienGauche.CoordX1, pere[0].lienGauche.CoordY1, coordX2, coordY2);
                                Temps.time = t;
                                pere[0].lienGauche.Ligne.Opacity = 0;
                                pere[0].filsGauche = null;
                            }
                            else
                            {
                                await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                coordX2 = pere[0].lienDroit.Ligne.X2;
                                coordY2 = pere[0].lienDroit.Ligne.Y2;
                                pere[0].filsDroit.masquer(c);
                                await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await pere[0].lienDroit.deplacerX2Y2(pere[0].lienDroit.CoordX2, pere[0].lienDroit.CoordY2, pere[0].lienDroit.CoordX1, pere[0].lienDroit.CoordY1);
                                t = Temps.time;
                                Temps.time = 0;
                                pere[0].lienDroit.deplacerX2Y2(pere[0].lienDroit.CoordX1, pere[0].lienDroit.CoordY1, coordX2, coordY2);
                                Temps.time = t;
                                pere[0].lienDroit.Ligne.Opacity = 0;
                                pere[0].filsDroit = null;
                            }
                        }
                        else {
                            await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            coordX2 = pere[0].lienDroit.Ligne.X2;
                            coordY2 = pere[0].lienDroit.Ligne.Y2;
                            pere[0].filsDroit.masquer(c);
                            await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            await pere[0].lienDroit.deplacerX2Y2(pere[0].lienDroit.CoordX2, pere[0].lienDroit.CoordY2, pere[0].lienDroit.CoordX1, pere[0].lienDroit.CoordY1);
                            t = Temps.time;
                            Temps.time = 0;
                            pere[0].lienDroit.deplacerX2Y2(pere[0].lienDroit.CoordX1, pere[0].lienDroit.CoordY1, coordX2, coordY2);
                            Temps.time = t;
                            pere[0].lienDroit.Ligne.Opacity = 0;
                            pere[0].filsDroit = null;
                        }
                    }
                    else
                    {
                        await algo.colorer(couleurAlgo, 2, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        coordX2 = Racine.CoordX;
                        coordY2 = Racine.CoordX;
                        Racine.masquer(c);
                        await algo.colorer(couleurAlgo, 3, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        Racine = null;

                    }

                }
                else if ((pere[1].filsGauche == null) && (pere[1].filsDroit != null))
                {
                    if (pere[0] != null)
                    {
                        if (pere[0].filsGauche != null)
                        {
                            if (pere[1].Valeur == (pere[0].filsGauche.Valeur))
                            {
                                await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                coordX2 = pere[0].lienGauche.CoordX2;
                                coordY2 = pere[0].lienGauche.CoordY2;
                                tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                                await pere[1].lienDroit.deplacerX2Y2(pere[1].filsDroit.CoordX + widthNoued / 2, pere[1].filsDroit.CoordY + heightNoeud / 2, coordX2, coordY2);
                                await deplacerSousArbre(pere[1].filsDroit, pere[0].filsGauche.CoordX - pere[1].filsDroit.CoordX, pere[0].filsGauche.CoordY - pere[1].filsDroit.CoordY);
                                pere[0].filsGauche.masquer(c);
                                pere[0].filsGauche = pere[1].filsDroit;
                                await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                            }
                            else {
                                await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                coordX2 = pere[0].lienDroit.CoordX2;
                                coordY2 = pere[0].lienDroit.CoordY2;
                                tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);
                                t = Temps.time;
                                Temps.time = 0;
                                await pere[1].lienDroit.deplacerX2Y2(pere[1].filsDroit.CoordX + widthNoued / 2, pere[1].filsDroit.CoordY + heightNoeud / 2, coordX2, coordY2);
                                Temps.time = t;
                                await deplacerSousArbre(pere[1].filsDroit, pere[0].filsDroit.CoordX - pere[1].filsDroit.CoordX, pere[0].filsDroit.CoordY - pere[1].filsDroit.CoordY);
                                pere[0].filsDroit.masquer(c);
                                pere[0].filsDroit = pere[1].filsDroit;
                                await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            }
                        }
                        else {
                            await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            coordX2 = pere[0].lienDroit.CoordX2;
                            coordY2 = pere[0].lienDroit.CoordY2;
                            tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                            await pere[1].lienDroit.deplacerX2Y2(pere[1].filsDroit.CoordX + widthNoued / 2, pere[1].filsDroit.CoordY + heightNoeud / 2, coordX2, coordY2);
                            await deplacerSousArbre(pere[1].filsDroit, pere[0].filsDroit.CoordX - pere[1].filsDroit.CoordX, pere[0].filsDroit.CoordY - pere[1].filsDroit.CoordY);
                            pere[0].filsDroit.masquer(c);
                            pere[0].filsDroit = pere[1].filsDroit;
                            await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        }
                    }
                    else {
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 8, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        coordX2 = Racine.lienDroit.CoordX1;
                        coordY2 = Racine.lienDroit.CoordY1;
                        tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                        await Racine.lienDroit.deplacerX2Y2(Racine.filsDroit.CoordX + widthNoued / 2, Racine.filsDroit.CoordY + heightNoeud / 2, coordX2, coordY2);
                        await deplacerSousArbre(Racine.filsDroit, Racine.CoordX - pere[1].filsDroit.CoordX, Racine.CoordY - pere[1].filsDroit.CoordY);
                        Racine.masquer(c);
                        Racine = pere[1].filsDroit;
                        await algo.colorer(couleurAlgo, 9, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 10, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    }


                }
                else if ((pere[1].filsDroit == null) && (pere[1].filsGauche != null))
                {
                    if (pere[0] != null)
                    {
                        if (pere[0].filsGauche != null)
                        {
                            if (pere[1].Valeur == pere[0].filsGauche.Valeur)
                            {
                                await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);
                                await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);
                                coordX2 = pere[0].lienGauche.CoordX2;
                                coordY2 = pere[0].lienGauche.CoordY2;
                                tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                                await pere[1].lienGauche.deplacerX2Y2(pere[1].filsGauche.CoordX + widthNoued / 2, pere[1].filsGauche.CoordY + heightNoeud / 2, coordX2, coordY2);
                                await deplacerSousArbre(pere[1].filsGauche, pere[0].filsGauche.CoordX - pere[1].filsGauche.CoordX, pere[0].filsGauche.CoordY - pere[1].filsGauche.CoordY);
                                pere[0].filsGauche.masquer(c);
                                pere[0].filsGauche = pere[1].filsGauche;

                                await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                            }
                            else {
                                await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                coordX2 = pere[0].lienDroit.CoordX2;
                                coordY2 = pere[0].lienDroit.CoordY2;
                                tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                                await pere[1].lienGauche.deplacerX2Y2(pere[1].filsGauche.CoordX + widthNoued / 2, pere[1].filsGauche.CoordY + heightNoeud / 2, coordX2, coordY2);
                                await deplacerSousArbre(pere[1].filsGauche, pere[0].filsDroit.CoordX - pere[1].filsGauche.CoordX, pere[0].filsDroit.CoordY - pere[1].filsGauche.CoordY);
                                pere[0].filsDroit.masquer(c);
                                pere[0].filsDroit = pere[1].filsGauche;
                                await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                                await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            }
                        }
                        else
                        {
                            await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            coordX2 = pere[0].lienDroit.CoordX2;
                            coordY2 = pere[0].lienDroit.CoordY2;
                            tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                            await pere[1].lienGauche.deplacerX2Y2(pere[1].filsGauche.CoordX + widthNoued / 2, pere[1].filsGauche.CoordY + heightNoeud / 2, coordX2, coordY2);
                            await deplacerSousArbre(pere[1].filsGauche, pere[0].filsDroit.CoordX - pere[1].filsGauche.CoordX, pere[0].filsDroit.CoordY - pere[1].filsGauche.CoordY);
                            pere[0].filsDroit.masquer(c);
                            pere[0].filsDroit = pere[1].filsGauche;
                            await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                            await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        }
                    }
                    else {
                        await algo.colorer(couleurAlgo, 4, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 5, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        coordX2 = Racine.lienGauche.CoordX1;
                        coordY2 = Racine.lienGauche.CoordY1;
                        tabPoint[0] = new Point(coordX2 - widthNoued / 2, coordY2 - heightNoeud / 2);

                        await Racine.lienGauche.deplacerX2Y2(Racine.filsGauche.CoordX + widthNoued / 2, Racine.filsGauche.CoordY + heightNoeud / 2, coordX2, coordY2);
                        await deplacerSousArbre(Racine.filsGauche, Racine.CoordX - pere[1].filsGauche.CoordX, Racine.CoordY - pere[1].filsGauche.CoordY);
                        Racine.masquer(c);
                        Racine = pere[1].filsGauche;
                        await algo.colorer(couleurAlgo, 6, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                        await algo.colorer(couleurAlgo, 7, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                    }


                }
                else
                {
                    await algo.colorer(couleurAlgo, 11, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    await algo.colorer(couleurAlgo, 12, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    await algo.colorer(couleurAlgo, 13, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
                    await suivant_inordre(pere[1], pere_inordre, s);
                    s[0].colorChamp(Brushes.LightGreen, Brushes.Green, 3);
                    coordX2 = pere[1].CoordX;
                    coordY2 = pere[1].CoordY;
                    tabPoint[0] = new Point(coordX2, coordY2);


                    s[0].lienDroit.Ligne.Opacity = 0;
                    t = Temps.time;
                    Temps.time = 1;
                    s[0].deplacer(tabPoint, 1);
                    pere[1].colorChamp(couleurFondNoeud, couleurBordureNoeud, 2);
                    Temps.time = t;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    s[0].colorChamp(couleurFondNoeud, couleurBordureNoeud, 2);
                    pere[1].masquer(c);
                    pere[1].Valeur = s[0].Valeur;
                    await s[0].masquer(c);
                    pere[1].afficher(c);



                    if (pere_inordre[0] != pere[1])
                    {

                        if (s[0].filsDroit == null)
                        {
                            await pere_inordre[0].lienGauche.deplacerX2Y2(pere_inordre[0].lienGauche.CoordX2, pere_inordre[0].lienGauche.CoordY2, pere_inordre[0].lienGauche.CoordX1, pere_inordre[0].lienGauche.CoordY1);
                            pere_inordre[0].lienGauche.deplacerX2Y2(pere_inordre[0].lienGauche.CoordX1, pere_inordre[0].lienGauche.CoordY1, pere_inordre[0].lienGauche.Ligne.X2, pere_inordre[0].lienGauche.Ligne.Y2);
                            pere_inordre[0].lienGauche.Ligne.Opacity = 0;


                        }
                        else { await deplacerSousArbre(s[0].filsDroit, pere_inordre[0].lienGauche.CoordX2 - widthNoued / 2 - s[0].filsDroit.CoordX, pere_inordre[0].lienGauche.CoordY2 - heightNoeud / 2 - s[0].filsDroit.CoordY); }
                        pere_inordre[0].filsGauche = s[0].filsDroit;
                    }
                    else {

                        if (s[0].filsDroit == null)
                        {
                            await pere_inordre[0].lienDroit.deplacerX2Y2(pere_inordre[0].lienDroit.CoordX2, pere_inordre[0].lienDroit.CoordY2, pere_inordre[0].lienDroit.CoordX1, pere_inordre[0].lienDroit.CoordY1);
                            t = Temps.time;
                            Temps.time = 0;
                            pere_inordre[0].lienDroit.deplacerX2Y2(pere_inordre[0].lienDroit.CoordX1, pere_inordre[0].lienDroit.CoordY1, pere_inordre[0].lienDroit.Ligne.X2, pere_inordre[0].lienDroit.Ligne.Y2);
                            Temps.time = t;
                            pere_inordre[0].lienDroit.Ligne.Opacity = 0;
                        }
                        else {
                            await deplacerSousArbre(s[0].filsDroit, pere_inordre[0].lienDroit.CoordX2 - widthNoued / 2 - s[0].filsDroit.CoordX, pere_inordre[0].lienDroit.CoordY2 - heightNoeud / 2 - s[0].filsDroit.CoordY);
                        }
                        pere_inordre[0].filsDroit = s[0].filsDroit;
                        await algo.colorer(couleurAlgo, 14, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant

                    }
                }
                comPrincipal.disparaitre(1); // Dispparaite le commentaire principal
                pere[1].colorChamp(couleurFondNoeud, couleurBordureNoeud, 2);
            }
            await algo.colorer(couleurAlgo, 15, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
            await algo.colorer(couleurAlgo, 16, 0.5 * Temps.time);// Affiche l'état en cours dans l'algorithme déroulant
            algo.disparaitre(Algo);// Dissparaitre l'algorithme déroulant
        }
    }
}
