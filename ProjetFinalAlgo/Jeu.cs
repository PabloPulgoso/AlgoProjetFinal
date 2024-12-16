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
        /// <param name="number">Nombre de dossiers à remonter.</param>
        /// <returns>Path vers le n-ième parent de la localisation actuelle.</returns>
        public static string GetParentLoop(string Path, int number)
        {
            string result = Path;
            for (int i = 0; i < number; i++)
            {
                result = Directory.GetParent(result).FullName;
            }
            return result;
        }


        /// <summary>
        /// Affiche ligne par ligne le contenu d'un fichier.
        /// </summary>
        /// <param name="path">Le chemin du fichier que l'on veut afficher.</param>
        /// <exception cref="ArgumentException">Renvoie une erreur si le fichier n'est pas accessible.</exception>
        static void AfficherFichier(string path)
        {
            if (File.Exists(path)) // Verifie que le fichier existe.
            {


                using (StreamReader reader = new StreamReader(path)) // Ouvre une liseuse et qui sera utilisée uniquement dans l'espace suivant.
                {   

                    
                    string line;
                    while ((line = reader.ReadLine()) != null) // Affiche toutes les lignes.
                    {
                        Console.WriteLine(line);
                    }
                    
                }
            }
            else
            {
                throw new ArgumentException("Path is incorrect"); // Si le fichier n'existe il renvoie une erreur.
            }

        }

        /// <summary>
        /// Crée un dictionnaire avec la langue que choisira l'utilisateur.
        /// </summary>
        /// <returns>Dictionnaire</returns>
        static Dictionnaire ChoisirLangue()
        {
            Console.WriteLine("Choisissez la langue dans laquelle vous voulez jouer (F pour FR ou E pour EN): ");

            ConsoleKey langueTouche = Console.ReadKey().Key;

            while (langueTouche != ConsoleKey.F && langueTouche != ConsoleKey.E)
            {
                Console.WriteLine("\nVeuillez saisir une langue valide");
                langueTouche = Console.ReadKey().Key;
            }


            string motsString = "";

            using (StreamReader reader = new StreamReader(langueTouche switch
                   {
                       ConsoleKey.F => pathFR,
                       ConsoleKey.E => pathEN,

                   }))
            {
                string line;
                while ((line = reader.ReadLine()) != null) // Affiche toutes les lignes.
                {
                    motsString+=line+" ";
                }
            }

            string[] mots =motsString.Split(' ', StringSplitOptions.RemoveEmptyEntries); // Va chercher un dictionnaire de mots

            Dictionnaire dico = new Dictionnaire(mots, langueTouche switch
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

                while ((line = reader.ReadLine()) != null) // Lis toutes les lignes.
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
        /// <param name="wordFrequencies">Dictionnaire avec les fréquences des mots</param>
        /// <param name="width">Largeur de l'image</param>
        /// <param name="height">Hauteur de l'image</param>
        /// <returns></returns>
        public static Bitmap CreateWordCloud(Dictionary<string, int> wordFrequencies, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                Random rand = new Random();
                List<RectangleF> rectangles = new List<RectangleF>();

                // Center of the bitmap
                float centerX = width / 2;
                float centerY = height / 2;

                foreach (var word in wordFrequencies)
                {
                    // Random font size based on frequency
                    int fontSize = Math.Min(10 + (int)Math.Pow(word.Value, 2.1), 100);
                    Font font = new Font("Arial", fontSize, FontStyle.Bold);

                    // Measure the size of the word
                    SizeF wordSize = g.MeasureString(word.Key, font);
                    bool placed = false;

                    // Spiral layout parameters
                    float angle = 0;
                    float radius = 0;

                    // Try to place the word in a spiral pattern
                    while (!placed && radius < Math.Max(width, height) / 2)
                    {
                        // Calculate position based on angle and radius
                        float x = centerX + radius * (float)Math.Cos(angle) - wordSize.Width / 2;
                        float y = centerY + radius * (float)Math.Sin(angle) - wordSize.Height / 2;

                        RectangleF wordRect = new RectangleF(x, y, wordSize.Width, wordSize.Height);

                        // Check for collision
                        bool collision = false;
                        foreach (var rect in rectangles)
                        {
                            if (rect.IntersectsWith(wordRect))
                            {
                                collision = true;
                                break;
                            }
                        }

                        // If no collision, draw the word
                        if (!collision)
                        {
                            // Generate a random color
                            Color color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                            g.DrawString(word.Key, font, new SolidBrush(color), new PointF(x, y));
                            rectangles.Add(wordRect); // Add the rectangle to the list
                            placed = true;
                        }

                        // Increment angle and radius for spiral effect
                        angle += 0.1f; // Adjust for tighter or looser spiral
                        radius += 0.5f; // Adjust for spacing between words
                    }

                    // If the word couldn't be placed, skip it
                    if (!placed)
                    {
                        Console.WriteLine($"Could not place the word: {word.Key}");
                    }
                }
            }
            return bitmap;
        }


        static async Task Main(string[] args)
        {

            // Vérifie que la librairie WordCloud soit installée.
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



            // Setup
            var cts = new CancellationTokenSource();  // Crée un CancellationTokenSource

            Dictionary<char, int> ValeursLettres = ValeurLettres();

            Dictionnaire dico = ChoisirLangue(); // Choisit la langue du dictionnaire à utiliser pour le reste du jeu
            dico.TriRapide(0, dico.Length - 1); // Trie le dictionnaire
            dico.toTrie(); // Crée l'arbre de recherche qui va servir à faire marcher le dictionnaire.

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

            // Partie
            for (int round = 0; round < nbTours; round++) // Boucle principale
            {

                foreach (Joueur JoueurActif in joueurs)
                { // Fait le tour de tout les joueurs
                    Console.Clear();
                    Console.WriteLine("Scores: ");
                    foreach (Joueur joueur in joueurs)
                    {
                        Console.WriteLine($"{joueur.Name}: {joueur.Score}pts ");
                    }


                    Console.WriteLine($"\n\nC'est le tour de {JoueurActif.Name}\n");
                    plateau.Melanger();
                    Console.WriteLine(plateau.toString());

                    // Commence une nouvelle tache qui annulera le token quand 60 secondes se seront déroulées.
                    Task timerTask = Task.Run(async () =>
                    {
                        await Task.Delay(60000); // Attends 60 secondes
                        cts.Cancel(); // Annule le token
                    });

                    // Commence une tache parallèle qui lira les entrés du joueur
                    Task inputTask = Task.Run(() =>
                    {
                        StringBuilder inputBuffer = new StringBuilder();
                        while
                            (!cts.Token.IsCancellationRequested) // Cette ligne s'assure que la boucle tourne tant que le token a pas été annulé
                        {

                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    string motJoue = inputBuffer.ToString().Trim().ToUpper(); // Lit le mot écrit par l'utilisateur
                                    inputBuffer.Clear();
                                    Console.WriteLine();

                                    if (!string.IsNullOrEmpty(motJoue))
                                    {
                                        bool ApparaitPlateau = plateau.Rechercher(motJoue); // Recherche dans le plateau si le mot apparait

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
                                    // Check if there is any character to delete
                                    if (inputBuffer.Length > 0)
                                    {
                                        // Move the cursor back one position
                                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                        // Overwrite the character with a space
                                        Console.Write(' ');
                                        // Move the cursor back again
                                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                        // Remove the last character from the buffer
                                        inputBuffer.Remove(inputBuffer.Length - 1, 1);
                                    }
                                }
                                else
                                {
                                    // Append the character to the input buffer
                                    inputBuffer.Append(keyInfo.KeyChar);
                                    // Write the character to the console
                                    Console.Write(keyInfo.KeyChar); // Display the character
                                }
                            }

                        }
                    });

                    await timerTask;

                    // Quand le timer se termine, on s'assure que toutes les taches sont finies
                    cts.Cancel();

                    await inputTask;

                    // Recrée un nouveau token
                    cts = new CancellationTokenSource();
                    Console.Clear();
                    Console.Clear();

                }
            }


            // Fin de la partie
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
                Console.WriteLine($"Le gagnant est {gagnants[0].Name}!!!");
            }

            Console.WriteLine("Rappel des scores: ");
            foreach (Joueur j in joueurs)
            {
                Console.WriteLine($"{j.Name}: {j.Score}pts   Son meilleur mot était: {j.MeilleurMot().Item1} qui lui a valu {j.MeilleurMot().Item2}pts");
            }
            Console.WriteLine($"\n{MeilleurMot.Item3.Name} a joué le meilleur mot de la partie: {MeilleurMot.Item1} qui vaut {MeilleurMot.Item2}pts!!");

            CreerNuage(joueurs);

            Console.WriteLine($"\nNuage de mots enregistré à: {pathImages}");


        }
    }
} 