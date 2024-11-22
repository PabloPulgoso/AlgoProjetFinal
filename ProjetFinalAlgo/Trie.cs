using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Trie
    {
        /// <summary>
        /// Racine de l'arbre
        /// </summary>
        private readonly TrieNode racine;

        /// <summary>
        /// Crée un nouvel arbre
        /// </summary>
        public Trie()
        {
            racine = new TrieNode();
        }

        /// <summary>
        /// Insère un mot dans l'arbre
        /// </summary>
        /// <param name="mot">Mot à ajouter à l'arbre</param>
        public void InsererMot(string mot)
        {
            TrieNode current = racine; // Commence à parcourir l'arbre à sa racine

            foreach (char c in mot ) // Parcours l'arbre en fonction des lettres dans le mot
            {
                if (!current.Enfants.ContainsKey(c))
                {
                    current.Enfants[c] = new TrieNode(); // Si la branche suivante existe pas, on la crée
                }
                current = current.Enfants[c]; // On passe à  la branche suivante
            }

            current.FinDeMot = true; // Indique qu'un mot peut se terminer à la branche où l'on se situe
        }

        /// <summary>
        /// Cherche un mot dans l'arbre.
        /// </summary>
        /// <param name="mot">Mot à chercher.</param>
        /// <returns>Vrai si il apparait dans l'arbre, faux sinon.</returns>
        public bool ChercherMot(string mot)
        {
            TrieNode current = racine; // Se place à la racine

            foreach (char c in mot) // Parcours l'arbre lettre à lettre
            {
                if (!current.Enfants.ContainsKey(c)) 
                {
                    return false; // Si la branche suivante existe pas, alors le mot n'apparait pas dans le dictionnaire
                }
                current = current.Enfants[c]; // Passe à la branche suivante
            } 
            return current.FinDeMot; // Indique si la branche finale peut être la fin d'un mot ou non.
        }
    }
}
