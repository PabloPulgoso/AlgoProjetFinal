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
        private List<string> mots_joues_tour;
        private Dictionary<string, int> mot_joues_partie;

        public string Name
        {
            get { return name; }
        }
        public int Score
        {
            get { return score; }
        }

        public Joueur(string name) 
        { 
            this.name = name;
            this.score = 0;
            this.mots_joues_tour = new List<string>();
            this.mot_joues_partie = new Dictionary<string, int>();
        }

        public override string ToString()
        {
            return $"Nom: {name}\nScore: {score}"; 

        }
    }
}
