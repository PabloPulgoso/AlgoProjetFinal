using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFinal;
using System;
using System.Diagnostics;
using System.IO;

namespace ProjetFinal.Tests
{

    [TestClass]
    [TestSubject(typeof(Dictionnaire))]
    public class DictionnaireTest
    {
        /// <summary>
        /// Cree une fonction deleguée qui executera l'algorithme de tri choisi selon ce que l'on donnera en paramètre du test unitaire.
        /// </summary>
        /// <param name="debut">Debut du tri</param>
        /// <param name="fin">Fin du tri</param>
        public delegate void FonctionDeTri(int debut = 0, int fin = 0);

        /// <summary>
        /// Teste si le dictionnaire est trié.
        /// </summary>
        [TestMethod]
        public void VerifierTri()
        {
            Dictionnaire dico =
                new Dictionnaire(
                    "chat arbre lune vélo maison montagne fleur soleil livre clavier musique pomme gâteau vélo ciel ordinateur nuage",
                    'T');
            bool attendu = false;
            bool resultat = dico.IsSorted();

            Assert.AreEqual(attendu, resultat);  
        }


        /// <summary>
        /// Teste la vitesse de plusieurs algorithmes de tri.
        /// </summary>
        /// <param name="methodeDeTri">Le nom de la methode de tri.</param>
        [TestMethod]
        [DataRow("TriRapide")]
        [DataRow("TriBulle")]
        [DataRow("TriFusion")]
        public void ChronometrerTri(string methodeDeTri)
        {
            Stopwatch chrono = new Stopwatch();

 
            string loc = $"{Jeu.GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\\\ProjetFinalAlgo\\\\Assets\\\\MotsTests.txt";


            string motsString = "";

            using (StreamReader reader = new StreamReader(loc))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    motsString += line + " ";
                }
            }

            string[] mots = motsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);


            Dictionnaire dico = new Dictionnaire(mots, 'T');  
            int fin = dico.Length - 1;

            FonctionDeTri fonctionDeTri =
                methodeDeTri
                    switch  
                    {
                        "TriRapide" => dico.TriRapide,
                        "TriBulle" => dico.TriBulle,
                        "TriFusion" => dico.TriFusion,
                    };


            chrono.Start(); 
            fonctionDeTri(0, fin);  
            chrono.Stop();  

            TimeSpan tempsEcoule = chrono.Elapsed;  


            Console.WriteLine(
                $"Longueur de la liste à trier {fin + 1} \nTri: {methodeDeTri} \nTemps écoulé: {tempsEcoule.TotalMilliseconds} ms");


            Assert.IsTrue(dico.IsSorted(), "Le dictionnaire n'est pas trié");  
        }


    }
}