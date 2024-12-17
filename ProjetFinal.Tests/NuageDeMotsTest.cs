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
        public void NuageDeMots()  
        {
            Joueur[] joueurs = new Joueur[2];
            var mots = new Dictionary<string, int>
            {
                { "C#", 5},
                { "Word", 3 },
                { "Cloud", 4 },
                { "Programming", 2 },
                { "Visual", 1 },
                { "Studio", 1 },
                { "Development", 2 }
            };
    
            for (int i = 0; i < 2; i++)
            {
                joueurs[i] = new Joueur($"{i}");
                foreach (var mot in mots.Keys)
                {
                    joueurs[i].AjouterMotJoue(mot, mots[mot]);
                }
            }

            Bitmap nuage = Jeu.CreateWordCloud(mots,800,600);
            DateTime now = DateTime.Now;
            string date = now.ToString("dd-MM-yyyy_HH-mm");
            string outputPath = $"{Jeu.pathImages}nuage-{date}.png";

            nuage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);


            Assert.IsTrue(File.Exists(outputPath));

        }


    }
}