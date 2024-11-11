using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjetFinal
{
    internal class De
    {
        private Lettre[] faces;

        Random random = new Random();
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

        
        public De(Lettre[] choix)
        {
            this.faces = new Lettre[6]; 

            for (int i = 0; i < 6; i++)
            {
                int n = random.Next(0, 26);
                while (choix[n].Quantite <= 0)
                {
                    n = random.Next(0, 26);
                }

                this.faces[i] = choix[n];
                choix[n].Quantite--;

            }

        }




    }
}
