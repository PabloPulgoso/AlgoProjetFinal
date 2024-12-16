using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Joueur
    {
        private string name; // Nom du joueur
        private int score; // Score du joueur
        private Dictionary<string, int> mot_joues_partie;// Mots joués dans la partie et points rapportés par chaque mot

        /// <summary>
        /// Nom du joueur.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Score du joueur, modifiable uniquement si c'est pour augmenter le score.
        /// </summary>
        public int Score
        {
            get { return score; }
            set
            {
                if (score + value >= score)
                {
                    score += value;
                }
                else
                {
                    score = value;
                }
            }
        }

        /// <summary>
        /// Liste des mots joués.
        /// </summary>
        public List<string> MotJoues
        {
            get { return new List<string>(mot_joues_partie.Keys); }
        }

        /// <summary>
        /// Constructeur de Joueur.
        /// </summary>
        /// <param name="name">Nom du joueur.</param>
        public Joueur(string name) 
        { 
            this.name = name;
            this.score = 0;
            this.mot_joues_partie = new Dictionary<string, int>();
        }
        

        /// <summary>
        /// Retourne le meilleur mot joué par ce joueur.
        /// </summary>
        /// <returns>Tuple du format (mot,score)</returns>
        public Tuple<string, int> MeilleurMot()
        {
            var meilleurMot = new Tuple<string, int>("", -1);
            foreach (var kvp in mot_joues_partie)
            {
                if (kvp.Value > meilleurMot.Item2)
                {
                    meilleurMot = new Tuple<string, int>(kvp.Key, kvp.Value);
                }

            }

            return meilleurMot;

        }

        /// <summary>
        /// String qui décrit l'état du joueur. Nom,score et liste de mots joués avec les scores de chaque mot.
        /// </summary>
        /// <returns>String qui décrit l'état du joueur.</returns>
        public override string ToString()
        {
            string s = $"{this.Name}\nScore: {this.score}\nMotsJoués:\n";


            foreach (var kvp in mot_joues_partie)
            {
                s += $"{kvp.Key}: {kvp.Value}pts\n";
            }

            return s;
        }

        /// <summary>
        /// Ajoute un mot à la liste de mots joués par le joueur.
        /// </summary>
        /// <param name="mot">Mot à ajouter.</param>
        /// <param name="score">Score du mot.</param>
        public void AjouterMotJoue(string mot,int score)
        {
            mot_joues_partie[mot] = score;
            this.score += score;
        }
       
        /// <summary>
        /// Indique si un mot a déjà été joué.
        /// </summary>
        /// <param name="mot">Mot à vérifier.</param>
        /// <returns>Vrai si il a pas été joué, faux sinon.</returns>
        public bool Jouable(string mot)
        {
            return !mot_joues_partie.ContainsKey(mot);
        }
    }



    internal static class JoueurExtensionTableaux
    {
        /// <summary>
        /// Trouve le joueur avec le score le plus haut parmis une liste de joueurs.
        /// </summary>
        /// <param name="tab">Liste de joueurs dont on veut le meilleur score.</param>
        /// <returns>Liste de joueurs qui contient les joueurs avec le plus haut score. Si il y en a plus d'un, cela veut dire que plusieurs joueurs ont le même score.</returns>
        public static Joueur[] Maximum(this Joueur[] tab)
        {
            List<Joueur> gagnants = new List<Joueur>();

            int maximum = tab[0].Score;

            foreach (Joueur j in tab)   
            {
                maximum = (j.Score>=maximum)?j.Score:maximum;
            }

            foreach (Joueur j in tab)
            {
                if (j.Score == maximum)
                {
                    gagnants.Add(j);
                }

            }

            return gagnants.ToArray();

        }
    }
}
