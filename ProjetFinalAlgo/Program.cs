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

                    lettres.Add(new Lettre(char.Parse(infos[0]), int.Parse(infos[2]), int.Parse(infos[1])));
                }
            }

            return lettres.ToArray();
        }



        public static Lettre[] CreerLettresDisponibles(int taille)
        {
            Lettre[] l = Program.CreerLettres();
            Lettre[] centLettres = Lettre.Ponderation(l);
            Lettre[] t = Lettre.CreerPossibilitesLettres(centLettres, taille);

            return t;
        }


        static void Main(string[] args)
        {




            Dictionnaire dico = ChoisirLangue(); // Choisit la langue du dictionnaire à utiliser pour le reste du jeu
            dico.TriRapide(0, dico.Length - 1);


            Console.WriteLine($"Vous avez choisis le dictionnaire en {dico.Langue switch{'F' => "Français", 'E' => "Anglais"}}");

            Console.WriteLine("Veuillez saisir la taille du plateau (minimum 4)");

            int taille = 0;


            while (taille < 4)
            {
                try
                {
                    taille = int.Parse(Console.ReadLine());
                }
                catch {
                    taille = 0;
                }

            }

            Console.WriteLine($"Taille {taille}");

            De[] des = new De[taille*taille];

            List<Lettre> lettresDispo = new List<Lettre> (CreerLettresDisponibles(taille));



            for (int i  = 0; i < des.Length; i++)
            {
                des[i] = new De(lettresDispo);
            }
            
            foreach (Lettre l in lettresDispo)
            {
                Console.WriteLine($"{l.Id}: {l.Quantite}");
            }


        }
    }
} 