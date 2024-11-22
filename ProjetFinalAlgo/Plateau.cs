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
    }
}
