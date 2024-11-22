using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class TrieNode
    {
        /// <summary>
        /// Pointe vers les mots qui ont le même préfixe que l'actuel
        /// </summary>
        public Dictionary<char, TrieNode> Enfants = new Dictionary<char, TrieNode>();

        /// <summary>
        /// Indique si cette branche peut marquer la fin d'un mot
        /// </summary>
        public bool FinDeMot = false;

        /// <summary>
        /// Crée une nouvelle branche de l'arbre.
        /// </summary>
        public TrieNode()
        {

        }
    }
}
