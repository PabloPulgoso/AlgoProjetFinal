using System;
using System.IO;

//Je met des commentaires içi pour qu'on écrive les choses à faire





namespace ProjetFinal
{
    internal class Program
    {

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
        
        



        static void Main(string[] args)
        {
            const string pathEN = "C:\\Users\\pablo\\source\\repos\\New folder\\ProjetFinalAlgo\\Assets\\MotsPossiblesEN.txt";
            const string pathFR = "C:\\Users\\pablo\\source\\repos\\New folder\\ProjetFinalAlgo\\Assets\\MotsPossiblesFR.txt";
            string[] mots = File.ReadAllText(pathFR).Split(' ', StringSplitOptions.RemoveEmptyEntries);


        }
    }
} 