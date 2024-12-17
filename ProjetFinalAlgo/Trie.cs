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
            TrieNode current = racine;  

            foreach (char c in mot )  
            {
                if (!current.Enfants.ContainsKey(c))
                {
                    current.Enfants[c] = new TrieNode();  
                }
                current = current.Enfants[c];  
            }

            current.FinDeMot = true;  
        }

        /// <summary>
        /// Cherche un mot dans l'arbre.
        /// </summary>
        /// <param name="mot">Mot à chercher.</param>
        /// <returns>Vrai si il apparait dans l'arbre, faux sinon.</returns>
        public bool ChercherMot(string mot)
        {
            TrieNode current = racine;  

            foreach (char c in mot)  
            {
                if (!current.Enfants.ContainsKey(c)) 
                {
                    return false;  
                }
                current = current.Enfants[c];  
            } 
            return current.FinDeMot;  
        }
    }
}
