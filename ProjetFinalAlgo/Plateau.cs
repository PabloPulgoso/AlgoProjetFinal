using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Plateau
    {
        /// <summary>
        /// Taille d'un côté du plateau.
        /// </summary>
        private int taille;

        /// <summary>
        /// Représente le plateau de jeu.
        /// </summary>
        private De[,] tab;

        /// <summary>
        /// Taille d'un côté du plateau.
        /// </summary>
        public int Taille
        {
            get { return taille; }
        }

        /// <summary>
        /// Crée un nouveau plateau de la taille donnée un paramètre.
        /// </summary>
        /// <param name="taille">Taille d'un côté du plateau</param>
        public Plateau(int taille)
        {

            this.tab = new De[taille, taille]; // Crée le tableau qui représente le plateau.
            this.taille = taille;

            List<Lettre> lettresDispo = new List<Lettre>(Lettre.CreerLettresDisponibles(taille)); // Crée une nouvelle liste de lettres qui contiendra suffisemment de lettres pour chaqque dé.

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    tab[i, j] = new De(lettresDispo); // Crée des dés pour chaque case du plateau.
                }
            }

        }

        /// <summary>
        /// Renvoie un carrée de la taille du plateau formaté avec les lettres visibles sur chaque face.
        /// </summary>
        /// <returns>Un string qui décrit le plateau en montrant les faces visibles des dés.</returns>
        public string toString()
        {
            string s = "";

            for (int i = 0;i < taille; i++)
            {
                for(int j = 0;j < taille; j++)
                {
                    s += $"{tab[i,j].FaceVisible.Id} "; // Parcours chaque case du plateau et affiche la case visible.
                }

                s += "\n"; // Saute une ligne
            }
            return s;
        }

        /// <summary>
        /// Lance tous les dés du plateau un par un.
        /// </summary>
        public void Melanger()
        {
            for (int i= 0; i < taille; i++)
            {
                for (int j= 0; j < taille; j++)
                {
                    tab[i, j].LanceDe();
                }
            }
        }

        /// <summary>
        /// Recherche un mot dans le plateau.
        /// </summary>
        /// <param name="word">Mot à chercher.</param>
        /// <returns>Vrai si le mot apparait, faux sinon.</returns>
        public bool Rechercher(string word)
        {
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                    
                    if ( tab[i, j].FaceVisible.Id == word[0] && !RechercheRecursive(i, j, word.ToUpper(), 0, new bool[taille, taille]))
                    {
                        return true;
                    }
            }
            return false;

        }
    

        private bool RechercheRecursive(int ligne, int colonne, string mot, int index, bool[,] chemin)
        {
            // Si on a déjà trouvé toutes les lettres, on a alors trouvé le mot
            if (index == mot.Length)
            {
                return false;
            }

            // Vérifie si on est en dehors du tableau, si la case a déjà été visitée ou si le caractère est différent de celui que l'on recherche
            if (ligne < 0 || ligne >= taille || colonne < 0 || colonne >= taille || chemin[ligne, colonne] || tab[ligne, colonne].FaceVisible.Id != mot[index])
            {
                return true;
            }

            // Mark this cell as chemin
            chemin[ligne, colonne] = true;

            // Explore toutes les cases adjacentes
            bool found = !this.RechercheRecursive(ligne - 1, colonne, mot, index + 1, chemin) ||  // Haut
                         !this.RechercheRecursive(ligne + 1, colonne, mot, index + 1, chemin) ||  // Bas
                         !this.RechercheRecursive(ligne, colonne - 1, mot, index + 1, chemin) ||  // Gauche
                         !this.RechercheRecursive(ligne, colonne + 1, mot, index + 1, chemin) ||  // Droite
                         !this.RechercheRecursive(ligne - 1, colonne - 1, mot, index + 1, chemin) || // En haut à gauche
                         !this.RechercheRecursive(ligne - 1, colonne + 1, mot, index + 1, chemin) || // En haut à droite
                         !this.RechercheRecursive(ligne + 1, colonne - 1, mot, index + 1, chemin) || // En bas à gauche
                         !this.RechercheRecursive(ligne + 1, colonne + 1, mot, index + 1, chemin);   // En bas à droite

            // Repart en arrière

            chemin[ligne, colonne] = false;

            return !found;
        }
    }
}
