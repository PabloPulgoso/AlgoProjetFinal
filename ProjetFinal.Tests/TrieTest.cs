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
        public void TestInserer()  
        {
            Trie arbre = new Trie();  
            string[] mots = new string[] { "Pablo", "Maxian", "Louis", "Théophile", "Louise" };  

            foreach (string s in mots)
            {
                arbre.InsererMot(s);    

            }

            foreach (string m in mots)
            {
                Assert.IsTrue(arbre.ChercherMot(m));  
            }

        }

        [TestMethod]
        public void TestTempsDictionnaire()  
        {
            string[] mots = File.ReadAllText(Jeu.pathFR).Split(' ', StringSplitOptions.RemoveEmptyEntries); 

            Dictionnaire dico = new  Dictionnaire(mots, 'f') ;  

            Stopwatch chrono = new Stopwatch(); 
            chrono.Start();
            dico.toTrie();  
            chrono.Stop(); 

            Console.WriteLine(chrono.Elapsed.TotalMilliseconds);  
            Assert.IsTrue(dico.Existe(mots[0]));  
            Assert.IsFalse(dico.Existe("sdqosj"));  

        }

    }
}
