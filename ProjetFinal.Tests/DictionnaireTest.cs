using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjetFinal;
using System;
using System.Diagnostics;
using System.IO;

namespace ProjetFinal.Tests;

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
        Dictionnaire dico = new Dictionnaire("chat arbre lune vélo maison montagne fleur soleil livre clavier musique pomme gâteau vélo ciel ordinateur nuage",'T');
        bool attendu = false;
        bool resultat = dico.IsSorted();

        Assert.AreEqual(attendu, resultat); // S'assure que le dictionnaire soit trié
    }


    [TestMethod]

    public void VerifierLettres()
    {
        Lettre[] l = Program.CreerLettres();


        Lettre[] centLettres = Lettre.Ponderation(l);


        foreach (Lettre lettre in centLettres)
        {

            Console.WriteLine($"{lettre.Id}: {lettre.Quantite} {lettre.Poids}");

        }
        Console.WriteLine(centLettres.Length);

        Assert.IsTrue(centLettres.Length == 100);
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
        
        // Va chercher un dictionnaire de mots à taille réduite (550 mots) pour tester rapidement les vitesses des algorithmes de tri.

        string loc = $"{Program.GetParentLoop(AppDomain.CurrentDomain.BaseDirectory, 5)}\\\\ProjetFinalAlgo\\\\Assets\\\\MotsTests.txt";

        string[] mots = File.ReadAllText(loc).Split(' ', StringSplitOptions.RemoveEmptyEntries); // Va chercher un dictionnaire de mots



        Dictionnaire dico = new Dictionnaire(mots, 'T'); // Crée un nouveau dictionnaire
        int fin = dico.Length - 1;

        FonctionDeTri fonctionDeTri = methodeDeTri switch // Choisit l'algorithme à chronometrer en fonction des paramètres donnés au test unitaire.
        {
            "TriRapide" => dico.TriRapide,
            "TriBulle" => dico.TriBulle,
            "TriFusion" => dico.TriFusion,
        };


        chrono.Start(); // Commence le chrono
        fonctionDeTri(0, fin); // Apelle l'algorithme de tri que l'on veut chronometrer
        chrono.Stop(); // Arrête le chrono

        TimeSpan tempsEcoule = chrono.Elapsed; // Calcule le temps écoulé

        // Affiche la longueur de la liste à trier, le nom de la methode utilisée et le temps écoulé pour trier.
        Console.WriteLine($"Longueur de la liste à trier {fin + 1} \nTri: {methodeDeTri} \nTemps écoulé: {tempsEcoule.TotalMilliseconds} ms"); 


        Assert.IsTrue(dico.IsSorted(), "Le dictionnaire n'est pas trié"); // Verifie que le dictionnaire est trié.
    }


}