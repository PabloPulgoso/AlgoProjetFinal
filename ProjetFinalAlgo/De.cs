using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetFinal
{
    internal class De
    {
        /// <summary>
        /// Représente les faces du dé.
        /// </summary>
        private Lettre[] faces;

        /// <summary>
        /// Représente la face visible du dé.
        /// </summary>
        private Lettre faceVisible;


        Random random = new Random();

        /// <summary>
        /// Lettres sur chaque face du dé.
        /// </summary>
        public char[] Faces
        {
            get {

                char[] facesIds = new char[faces.Length];

                for (int i = 0; i < faces.Length; i++)
                {
                    facesIds[i] = faces[i].Id;
                }

                return facesIds;
            }
        }

        /// <summary>
        /// Face visible du dé.
        /// </summary>
        public Lettre FaceVisible
        {
            get{ return faceVisible; } 
        }

        /// <summary>
        /// Crée un nouveau dé en prenant au hasard des lettres dans une liste de lettres. Supprime de la liste les lettres utilisée.
        /// </summary>
        /// <param name="choix">Liste de lettres parmis lesquelles il faut choisir.</param>
        public De(List<Lettre> choix)
        {
            this.faces = new Lettre[6]; 

            for (int i = 0; i < 6; i++)
            {
                int n = random.Next(0, choix.Count);

                while (choix[n].Quantite <= 0)
                {
                    n = random.Next(0, choix.Count);
                }

                this.faces[i] = choix[n];
                choix.RemoveAt(n);

            }
            this.faceVisible = faces[0];

        }

        /// <summary>
        /// Lance le dé et choisit au hasard une lettre parmis les 6 faces du dés pour devenir visible.
        /// </summary>
        public void LanceDe()
        {
            faceVisible = faces[random.Next(0, 6)];
        }




    }
}
