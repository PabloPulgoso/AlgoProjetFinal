using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;


namespace ProjetFinal
{
    internal class Jeu
    {
        /// <summary>
        /// Path vers le dictionnaire en Anglais.
        /// </summary>
        public static readonly string pathEN = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory,5)}\\ProjetFinalAlgo\\Assets\\MotsPossiblesEN.txt";
        /// <summary>
        /// Path vers le dictionnaire en Français.
        /// </summary>
        public static readonly string pathFR = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\ProjetFinalAlgo\\Assets\\MotsPossiblesFR.txt";
        /// <summary>
        /// Path vers les lettres.
        /// </summary>
        public static readonly string pathLettre = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\ProjetFinalAlgo\\Assets\\Lettres.txt";
        /// <summary>
        /// Path vers le dossier où seront stockées les images.
        /// </summary>
        public static readonly string pathImages = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\ProjetFinalAlgo\\Images\\";

        /// <summary>
        /// Renvoie le path vers le n-ième parent de la localisation actuelle.
        /// </summary>
        /// <param name="Path">Localisation actuelle</param>
        /// <param name="n">Nombre de dossiers à remonter.</param>
        /// <returns>Path vers le n-ième parent de la localisation actuelle.</returns>
        public static string GetParentLoop(string Path, int n)
        {
            string pathActuelle = Path;
            for (int i = 0; i < n; i++)
            {
                pathActuelle = Directory.GetParent(pathActuelle).FullName;
            }
            return pathActuelle;
        }


        /// <summary>
        /// Affiche ligne par ligne le contenu d'un fichier.
        /// </summary>
        /// <param name="path">Le chemin du fichier que l'on veut afficher.</param>
        /// <exception cref="ArgumentException">Renvoie une erreur si le fichier n'est pas accessible.</exception>
        static void AfficherFichier(string path)
        {
            if (File.Exists(path))  
            {


                using (StreamReader reader = new StreamReader(path))  
                {   

                    
                    string line;
                    while ((line = reader.ReadLine()) != null)  
                    {
                        Console.WriteLine(line);
                    }
                    
                }
            }
            else
            {
                throw new ArgumentException("Path is incorrect");  
            }

        }

        /// <summary>
        /// Crée un dictionnaire avec la langue que choisira l'utilisateur.
        /// </summary>
        /// <returns>Dictionnaire</returns>
        static Dictionnaire ChoisirLangue()
        {
            Console.WriteLine("Choisissez la langue dans laquelle vous voulez jouer (F pour FR ou E pour EN): ");

            ConsoleKey toucheLangue = Console.ReadKey().Key;

            while (toucheLangue != ConsoleKey.F && toucheLangue != ConsoleKey.E)
            {
                Console.WriteLine("\nVeuillez saisir une langue valide");
                toucheLangue = Console.ReadKey().Key;
            }


            string motsString = "";

            using (StreamReader reader = new StreamReader(toucheLangue switch
                   {
                       ConsoleKey.F => pathFR,
                       ConsoleKey.E => pathEN,

                   }))
            {
                string line;
                while ((line = reader.ReadLine()) != null) 
                {
                    motsString += line + " ";
                }
            }

            string[] mots = motsString.Split(' ', StringSplitOptions.RemoveEmptyEntries); 

            Dictionnaire dico = new Dictionnaire(mots, toucheLangue switch
            {
                ConsoleKey.F => 'F',
                ConsoleKey.E => 'E'
            });


            Console.Clear();

            return dico;
        }

        /// <summary>
        /// Crée une liste de lettres en se basant sur le fichier donné. Le fichier doit être un fichier texte où chaque ligne est une lettre sous la forme Lettre;Poids;Quantité
        /// </summary>
        /// <returns>Liste de lettres.</returns>
        public static Lettre[] CreerLettres()
        {
            List<Lettre> lettres = new List<Lettre>();   

            using (StreamReader reader = new StreamReader(pathLettre))
            {
                string line;

                while ((line = reader.ReadLine()) != null)  
                {
                    string[] infos = line.Split(';');

                    lettres.Add(new Lettre(char.Parse(infos[0]), int.Parse((infos[2])), int.Parse(infos[1])));
                }
            }

            return lettres.ToArray();
        }

        /// <summary>
        /// Crée un dictionnaire avec toutes les lettres et leur poids à partir d'un fichier donné. Le fichier doit être un fichier texte où chaque ligne est une lettre sous la forme Lettre;Poids;Quantité
        /// </summary>
        /// <returns>Dictionnaire avec toutes les lettres et leur poids.</returns>
        public static Dictionary<char, int> ValeurLettres()
        {
            Dictionary<char, int> valeurs = new Dictionary<char, int>();

            using (StreamReader reader = new StreamReader(pathLettre))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] infos = line.Split(';');
                    valeurs[char.Parse(infos[0])] = int.Parse(infos[1]);
                }
            }

            return valeurs;
        }

        /// <summary>
        /// Assure une saisie sécurisée d'un entier.
        /// </summary>
        /// <param name="minimum">Valeur minimum que peux prendre l'entier.</param>
        /// <param name="maximum">Valeur maximum que peux prendre l'entier.</param>
        /// <returns>L'entier une fois que la saisie est correcte.</returns>
        static int SaisieInt(int minimum = 0, int maximum = int.MaxValue)
        {
            int valeur;

            while (true) 
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out valeur))
                {
                    if (valeur >= minimum && valeur <= maximum)
                    {
                        return valeur; 
                    }
                    else
                    {
                        Console.WriteLine("Veuillez saisir une valeur correcte.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez saisir un nombre entier.");
                }
            }
        }

        /// <summary>
        /// Crée un nuage de mot à partir des mots joués par une liste de joueurs.
        /// </summary>
        /// <param name="joueurs">Liste de joueurs à partir de laquelle on veut un nuage de mots.</param>
        public static void CreerNuage(Joueur[] joueurs)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();


            foreach (Joueur j in joueurs)
            {
                foreach (string mot in j.MotJoues)
                {
                    if (d.ContainsKey(mot))
                    {
                        d[mot] += 1;
                    }
                    else
                    {
                        d[mot] = 1;
                    }

                }
            }

            
            Bitmap nuageDeMotsImage = CreateWordCloud(d,800,600);

            DateTime now = DateTime.Now;
            string date = now.ToString("dd-MM-yyyy_HH-mm");


            string outputPath = $"{Jeu.pathImages}nuage-{date}.png";

            nuageDeMotsImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
        }


        /// <summary>
        /// Fonction demandée à BlackBoxAI
        /// </summary>
        /// <param name="frequenceMots">Dictionnaire avec les fréquences des mots</param>
        /// <param name="largeur">Largeur de l'image</param>
        /// <param name="hauteur">Hauteur de l'image</param>
        /// <returns></returns>
        public static Bitmap CreateWordCloud(Dictionary<string, int> frequenceMots, int largeur, int hauteur)
        {
            Bitmap bitmap = new Bitmap(largeur, hauteur);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                Random r = new Random();
                List<RectangleF> rectangles = new List<RectangleF>();


                float centerX = largeur / 2;
                float centerY = hauteur / 2;

                foreach (var mot in frequenceMots)
                {
                     int fontSize = Math.Min(10 + (int)Math.Pow(mot.Value, 2.1), 100);
                    Font font = new Font("Arial", fontSize, FontStyle.Bold);

                 
                    SizeF wordSize = g.MeasureString(mot.Key, font);
                    bool place = false;

               
                    float angle = 0;
                    float rayon = 0;

                     while (!place && rayon < Math.Max(largeur, hauteur) / 2)
                    {
                         float x = centerX + rayon * (float)Math.Cos(angle) - wordSize.Width / 2;
                        float y = centerY + rayon * (float)Math.Sin(angle) - wordSize.Height / 2;

                        RectangleF wordRect = new RectangleF(x, y, wordSize.Width, wordSize.Height);

                         bool collision = false;
                        foreach (var rect in rectangles)
                        {
                            if (rect.IntersectsWith(wordRect))
                            {
                                collision = true;
                                break;
                            }
                        }

                         if (!collision)
                        {
                             Color color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                            g.DrawString(mot.Key, font, new SolidBrush(color), new PointF(x, y));
                            rectangles.Add(wordRect);  
                            place = true;
                        }

                         angle += 0.1f;  
                        rayon += 0.5f; 
                    }

                     if (!place)
                    {
                        Console.WriteLine($"Could not place the word: {mot.Key}");
                    }
                }
            }
            return bitmap;
        }


        static async Task Main(string[] args)
        {

             bool lib = true;
            try
            {
                var nuage = new WordCloud.WordCloud(800, 600);
            }
            catch (TypeLoadException)
            {
                Console.WriteLine("Veuillez installer la librarie WordCloud afin de pouvoir génerer le nuage de mots.");
                lib = false;
            }

            if (!lib)
            {
                return;
            }



             var cts = new CancellationTokenSource();  

            Dictionary<char, int> ValeursLettres = ValeurLettres();

            Dictionnaire dico = ChoisirLangue();  
            dico.TriRapide(0, dico.Length - 1);  
            dico.toTrie();  

            Console.WriteLine($"Vous avez choisis le dictionnaire en {dico.Langue switch { 'F' => "Français", 'E' => "Anglais" }}\n");


            Console.WriteLine("Veuillez saisir le nombre de joueurs (minimum 2)");
            int nbJoueurs = SaisieInt(2);

            Joueur[] joueurs = new Joueur[nbJoueurs];

            Console.Clear();
            for (int i = 0; i < nbJoueurs; i++)
            {
                Console.WriteLine($"Entrez le nom du joueur {i + 1}:");
                joueurs[i] = new Joueur(Console.ReadLine());
                Console.Clear();
            }

            Console.WriteLine("Veuillez saisir la taille du plateau (minimum 4)");
            int taille = SaisieInt(4);


            Console.Clear();
            Plateau plateau = new Plateau(taille);

            Console.Clear();
            Console.WriteLine("Combien de tours voulez vous jouer par joueur? Attention, chaque tour dure 1 minute. (Minimum 1 tour)");
            int nbTours = SaisieInt(1);

            var MeilleurMot = new Tuple<string, int, Joueur>("", 0, joueurs[0]);


             for (int round = 0; round < nbTours; round++)  
            {

                foreach (Joueur JoueurActif in joueurs)
                { 

                    Console.Clear();
                    Console.WriteLine("Scores: ");
                    foreach (Joueur joueur in joueurs)
                    {
                        Console.WriteLine($"{joueur.Name}: {joueur.Score}pts ");
                    }


                    Console.WriteLine($"\n\nC'est le tour de {JoueurActif.Name}\n");
                    plateau.Melanger();
                    Console.WriteLine(plateau.toString());


                    Task timerTask = Task.Run(async () =>
                    {
                        await Task.Delay(60000); 
                        cts.Cancel();  
                    });


                    Task inputTask = Task.Run(() =>
                    {
                        StringBuilder inputBuffer = new StringBuilder();
                        while
                            (!cts.Token.IsCancellationRequested)  
                        {

                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    string motJoue = inputBuffer.ToString().Trim().ToUpper();  
                                    inputBuffer.Clear();
                                    Console.WriteLine();

                                    if (!string.IsNullOrEmpty(motJoue))
                                    {
                                        bool ApparaitPlateau = plateau.Rechercher(motJoue);  

                                        bool ExisteDico = false;

                                        if (ApparaitPlateau)
                                        {
                                            ExisteDico = dico.Existe(motJoue);
                                        }

                                        bool jouable = JoueurActif.Jouable(motJoue);

                                        bool valide = ApparaitPlateau && ExisteDico && jouable;
                                        int pointsRapportes = 0;

                                        (bool, bool, bool) bools = (ApparaitPlateau, ExisteDico, jouable);



                                        if (valide)
                                        {
                                            foreach (char c in motJoue)
                                            {
                                                pointsRapportes += ValeursLettres[c];
                                            }

                                            Console.WriteLine($"{JoueurActif.Name} a joué le mot {motJoue} qui est valide et lui rapporte {pointsRapportes} points!");
                                            JoueurActif.AjouterMotJoue(motJoue, pointsRapportes);

                                            if (pointsRapportes > MeilleurMot.Item2)
                                            {
                                                Console.WriteLine("Nouveau meilleur mot!!!");
                                                MeilleurMot = new Tuple<string, int, Joueur>(motJoue, pointsRapportes,JoueurActif);
                                            }

                                        }
                                        else
                                        {
                                            switch (bools)
                                            {
                                                case (false, false, true):
                                                    Console.WriteLine($"{motJoue} n'apparait pas dans le plateau.");
                                                    break;
                                                case (false, true, false):
                                                    Console.WriteLine($"{motJoue} n'apparait pas dans le plateau.");
                                                    break;
                                                case (false, true, true):
                                                    Console.WriteLine($"{motJoue} n'apparait pas dans le plateau.");
                                                    break;
                                                case (true, false, false):
                                                    Console.WriteLine(
                                                        $"{motJoue} n'apparait pas dans le dictionnaire.");
                                                    break;
                                                case (true, false, true):
                                                    Console.WriteLine(
                                                        $"{motJoue} n'apparait pas dans le dictionnaire.");
                                                    break;
                                                case (true, true, false):
                                                    Console.WriteLine($"Vous avez déjà joué le mot {motJoue}.");
                                                    break;

                                            }

                                        }

                                    }


                                }

                                else if (keyInfo.Key == ConsoleKey.Backspace)
                                {
                                     if (inputBuffer.Length > 0)
                                    {
                                         Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                         Console.Write(' ');
                                         Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                         inputBuffer.Remove(inputBuffer.Length - 1, 1);
                                    }
                                }
                                else
                                {
                                     inputBuffer.Append(keyInfo.KeyChar);
                                     Console.Write(keyInfo.KeyChar);  
                                }
                            }

                        }
                    });

                    await timerTask;

                     cts.Cancel();

                    await inputTask;

                     cts = new CancellationTokenSource();
                    Console.Clear();
                    Console.Clear();

                }
            }


             Console.Clear();
            Console.WriteLine("LA PARTIE EST TERMINEE!!\n\n");

            Joueur[] gagnants = joueurs.Maximum();

            if (gagnants.Length > 1)
            {
                Console.Write("Nous avons une égalité entre ");
                for (int i = 0; i < gagnants.Length; i++)
                {
                    Console.Write($"{gagnants[i].Name}{((i == gagnants.Length - 2) ? " et " : ", ")}");
                }
                Console.WriteLine("!!");
            }
            else
            {
                Console.WriteLine($"Le gagnant est {gagnants[0]}!!!");
            }

            Console.WriteLine("Rappel des scores: ");
            foreach (Joueur j in joueurs)
            {
                Console.WriteLine($"{j.Name}: {j.Score}pts   Son meilleur mot était: {j.MeilleurMot().Item1} qui lui a valu {j.MeilleurMot().Item2}");
            }
            Console.WriteLine($"{MeilleurMot.Item3.Name} a joué le meilleur mot de la partie: {MeilleurMot.Item1} qui vaut {MeilleurMot.Item2}pts!!");

            CreerNuage(joueurs);

            Console.WriteLine($"Nuage de mots enregistré à: {pathImages}");


        }
    }
} 