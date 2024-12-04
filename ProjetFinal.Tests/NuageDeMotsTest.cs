using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFinal;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using WordCloud;


namespace ProjetFinal.Tests
{


    [TestClass]
    [TestSubject(typeof(Bitmap))]
    public class NuageDeMotsTest
    {
        [TestMethod]
        public void NuageDeMots() // Teste si on peut créer un nuage de mots à partir d'un dictionnaire.
        {
            Joueur[] joueurs = new Joueur[2];
            var mots = new Dictionary<string, int>
            {
                { "C#", 50 },
                { "Word", 30 },
                { "Cloud", 40 },
                { "Programming", 20 },
                { "Visual", 25 },
                { "Studio", 15 },
                { "Development", 10 }
            };
    
            for (int i = 0; i < 2; i++)
            {
                joueurs[i] = new Joueur($"{i}");
                foreach (var mot in mots.Keys)
                {
                    joueurs[i].AjouterMotJoue(mot, mots[mot]);
                }
            }

            Jeu.CreerNuage(joueurs);
            DateTime now = DateTime.Now;
            string date = now.ToString("dd-MM-yyyy_HH-mm");
            string outputPath = $"{Jeu.pathImages}nuage-{date}.png";

            Assert.IsTrue(File.Exists(outputPath));

        }


    }
}