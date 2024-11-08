using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Dictionnaire
    {
        /// <summary>
        /// Ensemble de strings qui constituent le dictionnaire.
        /// </summary>
        private string[] mots;


        /// <summary>
        /// Ensemble de strings qui constituent le dictionnaire.
        /// </summary>
        public string[] Mots
        {
            get { return mots; }
        }
        

        /// <summary>
        /// Constructeur du dictionnaire.
        /// </summary>
        /// <param name="dico">Prend une chaine de caractères séparés par des espaces.</param>
        /// <exception cref="ArgumentNullException">Renvoie une erreur si la chaine est nulle</exception>
        public Dictionnaire(string dico)
        {
            if (dico != null)
            {
                this.mots = dico.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            }
            else
            {
                throw new ArgumentNullException("La chaine de caracters est nulle");
            }
        }

        /// <summary>
        /// Constructeur du dictionnaire. Enlève les elements nuls du tableau.
        /// </summary>
        /// <param name="mots">Tableau de string où chaque string sera un mot.</param>
        /// <exception cref="ArgumentNullException">Renvoie une erreur si le tableau est null</exception>
        public Dictionnaire(string[] mots)
        {
            if (mots != null)
            {
                this.mots = mots.Where(m=> m!=null).ToArray();
            }
            else
            {
                throw new ArgumentNullException("Le tableau est null");
            }
        }
        
        /// <summary>
        /// Renvoie le nombre de mots du dictionnaire
        /// </summary>
        public int Length
        {
            get { return mots.Length; }
        }

        /// <summary>
        /// Verifie si le tableau est trié.
        /// </summary>
        /// <returns>Vrai si le tableau est trié, faux sinon.</returns>
        public bool IsSorted()
        {
            for (int i = 0; i < this.Length - 1; i++)
            {
                if (string.Compare(this.mots[i], this.mots[i + 1]) > 0) // Compare A et B dans l'ordre alphabetique. Renvoie un nombre negatif si A vient avant B, 0 si les deux mots sont identiques et un nombre positif si A vient apres B.
                {
                    return false;
                }
            }
            return true;
        }


        public void TriRapide( int debut, int fin)
        {
            if (debut < fin)
            {
                int indexPivot = Partition(this.mots, debut, fin);  // Partitionne le tableau

                // Trie récursivement les sous-tableaux
                this.TriRapide(debut, indexPivot - 1);
                this.TriRapide(indexPivot + 1, fin);
            }
        }

        private int Partition(string[] tab, int debut, int fin)
        {
            string pivot = tab[fin]; // Choisit l'élément le plus à droite comme pivot
            int i = debut - 1;

            for (int j = debut; j < fin; j++)
            {
                if (String.Compare(tab[j], pivot) <= 0)  // Si l'élément actuel vient avant ou est dans la même position que le pivot, on échange les deux

                {
                    i++;
                    Echange(tab, i, j);
                }
            }

            // Échanger tab[i + 1] et tab[fin] (ou le pivot)
            Echange(tab, i + 1, fin);
            return i + 1;
        }

        private void Echange(string[] tab, int i, int j)
        {
            string temp = tab[i];
            tab[i] = tab[j];
            tab[j] = temp;
        }

    }
}
