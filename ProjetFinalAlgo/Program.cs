using System;
using System.Diagnostics;
using System.IO;

//Je met des commentaires içi pour qu'on écrive les choses à faire





namespace ProjetFinal
{
    internal class Program
    {
        public static readonly string pathEN = "C:\\Users\\pablo\\source\\repos\\projetfinal\\ProjetFinalAlgo\\Assets\\MotsPossiblesEN.txt";
        public static readonly string pathFR = "C:\\Users\\pablo\\source\\repos\\projetfinal\\ProjetFinalAlgo\\Assets\\MotsPossiblesFR.txt";

        

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


        static Lettre[] CreerLettres()
        {
            List<Lettre> lettres = new List<Lettre>();   

            using (StreamReader reader =
                   new StreamReader(
                       "C:\\Users\\pablo\\source\\repos\\projetfinal\\ProjetFinalAlgo\\Assets\\Lettres.txt"))
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



        static void Main(string[] args)
        {


            Console.WriteLine(s / 6);
            De d = new De(lettres);

            Dictionnaire dico = ChoisirLangue(); // Choisit la langue du dictionnaire à utiliser pour le reste du jeu
            dico.TriRapide(0, dico.Length - 1);

            Dictionnaire dico = ChoisirLangue(); // Choisit la langue du dictionnaire à utiliser pour le reste du jeu

            //Console.WriteLine($"Vous avez choisis le dictionnaire en {dico.Langue switch{'F' => "Français", 'E' => "Anglais"}}");



        }
    }
} 