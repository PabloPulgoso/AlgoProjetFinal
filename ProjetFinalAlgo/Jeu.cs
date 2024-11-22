using System;
using System.Diagnostics;
using System.IO;

//Je met des commentaires içi pour qu'on écrive les choses à faire





namespace ProjetFinal
{
    internal class Jeu
    {
        public static readonly string pathEN = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory,5)}\\ProjetFinalAlgo\\Assets\\MotsPossiblesEN.txt";
        public static readonly string pathFR = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\ProjetFinalAlgo\\Assets\\MotsPossiblesFR.txt";
        public static readonly string pathLettre = $"{GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\ProjetFinalAlgo\\Assets\\Lettres.txt";
        


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

            string[] mots = File.ReadAllText(langueTouche switch
                {
                    ConsoleKey.F => pathFR,
                    ConsoleKey.E => pathEN,

                }
            ).Split(' ', StringSplitOptions.RemoveEmptyEntries); // Va chercher un dictionnaire de mots

            Dictionnaire dico = new Dictionnaire(mots, langueTouche switch
            {
                ConsoleKey.F => 'F',
                ConsoleKey.E => 'E'
            });


            Console.Clear();

            return dico;
        }


        public static Lettre[] CreerLettres()
        {
            List<Lettre> lettres = new List<Lettre>();   

            using (StreamReader reader = new StreamReader(pathLettre))
            {
                string line;

                while ((line = reader.ReadLine()) != null) // Affiche toutes les lignes.
                {
                    string[] infos = line.Split(';');

                    lettres.Add(new Lettre(char.Parse(infos[0]), int.Parse((infos[2])), int.Parse(infos[1])));
                }
            }

            return lettres.ToArray();
        }

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




        static async Task Main(string[] args)
        {
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
                Console.WriteLine($"Entrez le nom du joueur {i+1}:");
                joueurs[i] = new Joueur(Console.ReadLine());
                Console.Clear();
            }

            Console.WriteLine("Veuillez saisir la taille du plateau (minimum 4)");
            int taille = SaisieInt(4);


            Console.Clear();
            Plateau plateau = new Plateau(taille);

            Console.Clear();
            Console.WriteLine("Combien de tours voulez vous jouer par joueur? Attention, chaque tour dure 1 minute. (Minimum 1 tour)");
            int nbTours = SaisieInt(1)*nbJoueurs;

            var MeilleurMot = new Tuple<string, int, Joueur>("", 0, joueurs[0]);

            // Partie
            for (int round = 0; round < nbTours; round++) // Boucle principale
            {

                foreach (Joueur JoueurActif in joueurs) { // Fait le tour de tout les joueurs
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
                    { while (!cts.Token.IsCancellationRequested) // Cette ligne s'assure que la boucle tourne tant que le token a pas été annulé
                        {


                            string motJoue = Console.ReadLine().ToUpper(); // Lit le mot écrit par l'utilisateur
                            while (motJoue == null || motJoue == "")
                            {
                                motJoue = Console.ReadLine().ToUpper(); // Securise la saisie

                            }

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
                                JoueurActif.AjouterMotJoue(motJoue,pointsRapportes);

                                if (pointsRapportes > MeilleurMot.Item2)
                                {
                                    Console.WriteLine("Nouveau meilleur mot!!!");
                                    MeilleurMot = new Tuple<string, int, Joueur>(motJoue, pointsRapportes, JoueurActif);
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
                                        Console.WriteLine($"{motJoue} n'apparait pas dans le dictionnaire.");
                                        break;
                                    case (true, false, true):
                                        Console.WriteLine($"{motJoue} n'apparait pas dans le dictionnaire.");
                                        break;
                                    case (true, true, false):
                                        Console.WriteLine($"Vous avez déjà joué le mot {motJoue}.");
                                        break;

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

                }
            }


            // Fin de la partie
            Console.Clear();
            Console.WriteLine("LA PARTIE EST TERMINEE!!\n\n");

            Joueur[] gagnants = joueurs.Maximum();

            if (gagnants.Length > 1)
            {
                Console.Write("Nous avons une égalité entre ");
                for (int i = 0; i<gagnants.Length; i++)
                {
                    Console.Write($"{gagnants[i].Name}{((i==gagnants.Length-2)?" et ":", ")}");
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
        }
    }
} 