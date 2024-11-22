using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Joueur
    {
        private string name;
        private int score;
        private Dictionary<string, int> mot_joues_partie;

        public string Name
        {
            get { return name; }
        }
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

        public Joueur(string name) 
        { 
            this.name = name;
            this.score = 0;
            this.mot_joues_partie = new Dictionary<string, int>();
        }

        public override string ToString()
        {
            return $"Nom: {name}\nScore: {score}"; 

        }

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

        public void AjouterMotJoue(string mot,int score)
        {
            mot_joues_partie[mot] = score;
            this.score += score;
        }
        public bool Jouable(string mot)
        {
            return !mot_joues_partie.ContainsKey(mot);
        }
    }



    internal static class JoueurExtensionTableaux
    {
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
