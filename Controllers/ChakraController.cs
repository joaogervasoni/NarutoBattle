using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class ChakraController
    {
        public int Taijutsu { get; set; }
        public int Bloodline { get; set; }
        public int Ninjutsu { get; set; }
        public int Genjutsu { get; set; }
        private static int ChakraRoll;


        public ChakraController()
        {
            initialChakra();

            Random random = new Random();

            ChakraRoll = random.Next(1, 3);
        }


        public List<int> returnChakras()
        {
            List<int> Chakras = new List<int>();
            Chakras.Add(Taijutsu);
            Chakras.Add(Bloodline);
            Chakras.Add(Ninjutsu);
            Chakras.Add(Genjutsu);

            return Chakras;
        }

        public string printChakra()
        {
            string chakras = ("Taijutsu: " + Taijutsu + ", Ninjutsu: " + Ninjutsu + ", Genjutsu: " + Genjutsu + ", Bloodline: " + Bloodline + "////");
            return chakras;
        }

        public void turnChakra()
        {
            List<int> Chakras = new List<int>();
            int maxChakra = 3;
            Random random = new Random();

            //Initialize 5 chakras, no more
            for (int i = 0; i < 4; i++)
            {

                int numChakra = random.Next(1, 3);

                if (maxChakra - numChakra > -1)
                {
                    Chakras.Add(numChakra);
                    maxChakra -= numChakra;


                }
                else
                {
                    Chakras.Add(0);
                }


            }
            //if rest
            if (maxChakra == 1)
            {
                int randomNum = random.Next(1, 3);
                Chakras[randomNum] += 1;
            }

            if (ChakraRoll == 1)
            {
                Taijutsu += Chakras[0];
                Bloodline += Chakras[1];
                Ninjutsu += Chakras[2];
                Genjutsu += Chakras[3];
                ChakraRoll = 2;
            }
            else if (ChakraRoll == 2)
            {
                Taijutsu += Chakras[3];
                Bloodline += Chakras[2];
                Ninjutsu += Chakras[1];
                Genjutsu += Chakras[0];
                ChakraRoll = 1;
            }

        }

        public void initialChakra()
        {
            List<int> Chakras = new List<int>();
            int maxChakra = 4;
            Random random = new Random();

            //Initialize 5 chakras, no more
            for (int i = 0; i < 5; i++)
            {
                
                int numChakra = random.Next(1, 3);

                if  (maxChakra - numChakra > -1)
                {
                    Chakras.Add(numChakra);
                    maxChakra -= numChakra;
                    

                }
                else
                {
                    Chakras.Add(0);
                }

                
            }
            //if rest
            if (maxChakra == 1)
            {
                int randomNum = random.Next(1, 3);
                Chakras[randomNum] += 1;
            }

            //add chakras
            Taijutsu = Chakras[0];
            Bloodline = Chakras[1];
            Ninjutsu = Chakras[2];
            Genjutsu = Chakras[3];

        }
    }
}
