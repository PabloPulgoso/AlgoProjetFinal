using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFinal;
using System;
using System.Diagnostics;
using System.IO;


namespace ProjetFinal.Tests
{


    [TestClass]
    [TestSubject(typeof(Trie))]
    public class TrieTest
    {
        [TestMethod]
        public void TestInserer() // Teste la methode pour inserer des éléments dans l'arbre
        {
            Trie arbre = new Trie(); // Crée un nouvel arbre
            string[] mots = new string[] { "Pablo", "Maxian", "Louis", "Théophile", "Louise" }; // Lsite de mots à inserer

            foreach (string s in mots)
            {
                arbre.InsererMot(s);    // Insère mot par mot

            }

            foreach (string m in mots)
            {
                Assert.IsTrue(arbre.ChercherMot(m)); // Verufie que tout les mots sont bien dans l'arbre
            }

        }

        [TestMethod]
        public void TestTempsDictionnaire()
        {
            string[] mots = File.ReadAllText(Jeu.pathFR).Split(' ', StringSplitOptions.RemoveEmptyEntries); // Va chercher un fichier contenant de mots

            Dictionnaire dico = new  Dictionnaire(mots, 'f') ; // Crée un nouveau dictionnaire

            Stopwatch chrono = new Stopwatch(); // Crée et lance un chrono
            chrono.Start();
            dico.toTrie(); // Crée un arbre à partir du dictionnaire
            chrono.Stop(); // Arrete le chrono

            Console.WriteLine(chrono.Elapsed.TotalMilliseconds); // Affiche le temps mis pour la conversion
            Assert.IsTrue(dico.Existe(mots[0])); // Verifie que le mot existe
            Assert.IsFalse(dico.Existe("sdqosj")); // Verifie que le mot inventé n'apparait pas

        }

    }
}
