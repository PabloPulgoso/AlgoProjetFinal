using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Dictionnaire
    {

        // Propriétés privées

        /// <summary>
        /// Ensemble de strings qui constituent le dictionnaire.
        /// </summary>
        private string[] mots;

        /// <summary>
        /// Represente la langue des mots dans le dictionnaire
        /// </summary>
        private char langue;

        /// <summary>
        /// Arbre qui sert à chercher les mots dans le dictionnaire.
        /// </summary>
        private Trie arbre =  new Trie() ;


        // Propriétés publiques

        /// <summary>
        /// Ensemble de strings qui constituent le dictionnaire.
        /// </summary>
        public string[] Mots
        {
            get { return mots; }
        }

        /// <summary>
        /// Renvoie le nombre de mots du dictionnaire
        /// </summary>
        public int Length
        {
            get { return mots.Length; }
        }

        public char Langue
        {
            get { return langue; }
        }




        // Constructeurs

        /// <summary>
        /// Constructeur du dictionnaire.
        /// </summary>
        /// <param name="dico">Prend une chaine de caractères séparés par des espaces.</param>
        /// <exception cref="ArgumentNullException">Renvoie une erreur si la chaine est nulle</exception>
        public Dictionnaire(string dico, char langue)
        {
            if (dico != null)
            {
                this.mots = dico.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                this.langue = char.ToUpper(langue);

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
        public Dictionnaire(string[] mots, char langue)
        {
            if (mots != null)
            {
                this.mots = mots.Where(m => m != null).ToArray();
                this.langue = langue;
            }
            else
            {
                throw new ArgumentNullException("Le tableau est null");
            }
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


        /// <summary>
        /// Retourne une chaine de caractères décrivant le dictionnaire. (Langue, nombre de mots par longueur et nombre de mots par la première lettre)
        /// </summary>
        /// <returns>String</returns>
        public string toString()
        {
            Dictionary<int, int> longueurs = new Dictionary<int, int>(); // Création d'un dictionnaire pour compter le nombre de mots par longueur
            Dictionary<char, int> pLettre = new Dictionary<char, int>(); // Création d'un dictionnaire pour compter le nombre de mots par premiere lettre

            string des = $"Langue du dictionnaire: {this.langue switch {    // Création d'un string pour stocker les informations
                'F' => "Français",
                'E' => "Anglais"
            }} \n\n";

            foreach (string s in this.mots) // Parcours la liste de mots pour remplir les dictionnaires
            {

                if (pLettre.ContainsKey((s[0]))) // Vérifie l'existance d'une clef
                {
                    pLettre[s[0]]++; // Si elle existe on ajoute une ocurrence
                }
                else
                {
                    pLettre.Add(s[0],1); // Sinon on la crée
                }


                if (longueurs.ContainsKey(s.Length)) // Pareil
                {
                    longueurs[s.Length]++;
                }
                else
                {
                    longueurs.Add(s.Length,1);
                }

            }

            des += "Nombre de mots par longueur:\n";

            foreach (var l in longueurs)
            {
                des += $"Il y a {l.Value} mots de longueur {l.Key}\n"; // Ajoute une ligne pour chaque longueur de mots
            }

            des += "\n\nNombre de mots par la première lettre:\n";

            foreach (var l in pLettre)
            {
                des += $"Il y a {l.Value} mots qui commencent par {l.Key}\n"; // Pareil mais pour la première lettre
            }

            return des;
        }

        /// <summary>
        /// Crée l'arbre associé au dictionnaire.
        /// </summary>
        public void toTrie()
        {
            foreach (string mot in mots)
            {
                arbre.InsererMot(mot);
            }
        }

        /// <summary>
        /// Vérifie si un mot apparait dans le dictionnaire.
        /// </summary>
        /// <param name="mot">Mot dont on veut verifier l'existance</param>
        /// <returns>Vrai s'il apparait, faux sinon.</returns>
        public bool Existe(string mot)
        {
            return arbre.ChercherMot(mot);
        }




        // Methodes de tri

        /// <summary>
        /// Trie le tableau avec l'algorithme de tri rapide.
        /// </summary>
        /// <param name="debut">Debut du tri</param>
        /// <param name="fin">Pivot du tri</param>
        public void TriRapide(int debut, int fin) // Le string[] est là pour normaliser la signature pour les tests.
        {
            if (debut < fin)
            {
                int indexPivot = Partition(this.mots, debut, fin);  // Partitionne le tableau

                // Trie récursivement les sous-tableaux
                this.TriRapide(debut, indexPivot - 1);
                this.TriRapide(indexPivot + 1, fin);
            }
        }

        /// <summary>
        /// Trie le tableau avec l'algorithme du tri à bulle.
        /// </summary>
        /// <param name="tab">Tableau à trier</param>
        public void TriBulle(int debut = 0, int fin = 0) // Les paramètres sont là pour normaliser la signature pour les tests.
        {
            int n = this.Length;
            bool echange;

            for (int i = 0; i < n - 1; i++)
            {
                echange = false; 
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Compare et échange deux éléments si c'est nécessaire
                    if (string.Compare(this.mots[j], this.mots[j + 1]) > 0)
                    {
                        Echange(this.mots, j, j + 1);
                        echange = true;
                    }
                }

                // Si on a pas échangé d'éléments on considère que le tri est fini
                if (!echange)
                    break;
            }
        }

        /// <summary>
        /// Trie le tableau avec l'algorithme de trie par fusion.
        /// </summary>
        /// <param name="gauche">Début du tableau à trier</param>
        /// <param name="droite">Fin du tableau à trier</param>
        public void TriFusion(int gauche, int droite)
        {
            if (gauche < droite)
            {
                int millieu = (gauche + droite) / 2;

                // Trie la première moitié
                TriFusion( gauche, millieu);
                // Trie la deuxième moitié
                TriFusion( millieu + 1, droite);
                
                // Fusiononne les deux moitiés.
                Fusion(this.mots, gauche, millieu, droite);
            }
        }



        // Méthodes outils pour les algorithmes de tri

        /// <summary>
        /// Trie les sous-tableaux
        /// </summary>
        /// <param name="tab">Tableau à trier</param>
        /// <param name="gauche">Début du premier sous-tableau</param>
        /// <param name="millieu">Fin du premier sous-tableau et début du deuxième</param>
        /// <param name="droite">Fin du deuxième sous-tableau</param>
        private void Fusion(string[] tab, int gauche, int millieu, int droite)
        {
            // Calcule la taille des deux tableaux à fusionner.
            int n1 = millieu - gauche + 1;
            int n2 = droite - millieu;
            

            // crée des tableaux temporaires
            string[] tabGauche = new string[n1];
            string[] tabDroite = new string[n2];

            // Copie les données vers les tableaux temporaires
            for (int i = 0; i < n1; i++)
                tabGauche[i] = tab[gauche + i];
            for (int j = 0; j < n2; j++)
                tabDroite[j] = tab[millieu + 1 + j];

            // Fusion des tableaux temporaires

            // Index initiaux des deux sous-tableaux
            int k = gauche;
            int indexI = 0;
            int indexJ = 0;

            while (indexI < n1 && indexJ < n2)
            {
                if (string.Compare(tabGauche[indexI], tabDroite[indexJ]) <= 0)
                {
                    tab[k] = tabGauche[indexI];
                    indexI++;
                }
                else
                {
                    tab[k] = tabDroite[indexJ];
                    indexJ++;
                }
                k++;
            }

            // Copie les éléments restants dans tabGauche
            while (indexI < n1)
            {
                tab[k] = tabGauche[indexI];
                indexI++;
                k++;
            }

            // Copie les éléments restants dans tabDroite
            while (indexJ < n2)
            {
                tab[k] = tabDroite[indexJ];
                indexJ++;
                k++;
            }
        }

        /// <summary>
        /// Trie une partition du tableau.
        /// </summary>
        /// <param name="tab">Tableau à trier</param>
        /// <param name="debut">Debut du tri.</param>
        /// <param name="fin">Pivot du tri.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Echange deux éléments d'un tableau de strings.
        /// </summary>
        /// <param name="tab">Tableau avec les éléments à échanger.</param>
        /// <param name="i">Indice du premier élément à échanger.</param>
        /// <param name="j">Indice du deuxième élément à échanger.</param>
        private void Echange(string[] tab, int i, int j)
        {
            string temp = tab[i];
            tab[i] = tab[j];
            tab[j] = temp;
        }

    }
}
