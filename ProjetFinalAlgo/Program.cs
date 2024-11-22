using System;
using System.Diagnostics;
using System.IO;

//Je met des commentaires içi pour qu'on écrive les choses à faire





namespace ProjetFinal
{
    internal class Program
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

            string[] mots = File.ReadAllText(langueTouche switch
                {
                    ConsoleKey.F => pathFR,
                    ConsoleKey.E => pathEN
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


        static int SaisieInt(int minimum = 0, int maximum = int.MaxValue)
        {
            int valeur = 0;


            while (valeur <minimum || valeur>maximum)
            {
                try
                {
                    valeur = int.Parse(Console.ReadLine());
                }
                catch
                {
                    valeur = 0;
                }

            }
            return valeur;
        }


        static async Task Main(string[] args)
        {
            // Create a CancellationTokenSource
            var cts = new CancellationTokenSource();



            Dictionnaire dico = ChoisirLangue(); // Choisit la langue du dictionnaire à utiliser pour le reste du jeu
            dico.TriRapide(0, dico.Length - 1);


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


            while (true) // Boucle principale
            {

                foreach (Joueur j in joueurs) { // Fait le tour de tout les joueurs

                    Console.WriteLine("Scores: ");
                    foreach (Joueur joueur in joueurs)
                    {
                        Console.WriteLine($"{joueur.Name}: {joueur.Score}pts ");
                    }


                    Console.WriteLine($"\n\nC'est le tour de {j.Name}\n");
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
                            string mot = Console.ReadLine();
                            Console.WriteLine($"{j.Name} a joué le mot {mot}");
                        }
                    });

                    await timerTask;

                    // Quand le timer se termine, on s'assure que toutes les taches sont finies
                    cts.Cancel();

                    // Recrée un nouveau token
                    cts = new CancellationTokenSource();
                    Console.Clear();

                }
            }




        }
    }
} 