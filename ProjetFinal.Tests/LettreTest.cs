using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFinal;
using System;
using System.Diagnostics;
using System.IO;


namespace ProjetFinal.Tests
{


    [TestClass]
    [TestSubject(typeof(Lettre))]
    public class LettreTest
    {
        [TestMethod]

        public void VerifierLettres()
        {
            Lettre[] l = Jeu.CreerLettres();


            Lettre[] centLettres = Lettre.Ponderation(l);

            Lettre[] t = Lettre.CreerPossibilitesLettres(centLettres, 5);

            Console.WriteLine(t.Length);

            Assert.IsTrue(centLettres.Length == 100);
        }

    }
}

