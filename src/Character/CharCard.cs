using System;
using Newtonsoft.Json;

namespace RPG
{
    public class CharCard
    {
        public string id;
        public string name {get;}
        public int[] ability_scores {get;}
        public int[] ability_modifiers {get;}
        
        private Random random;

        public CharCard(int id)
        {
            this.id = id.ToString("X8");

            name = "default";

            ability_scores = new int[6];
            ability_scores[0] = 15;
            ability_scores[1] = 14;
            ability_scores[2] = 13;
            ability_scores[3] = 12;
            ability_scores[4] = 10;
            ability_scores[5] = 8;

            ability_modifiers = new int[6];
            for (int i = 0; i < 6; i++)
            {
                ability_modifiers[i] = (int) Math.Floor( ( ability_scores[i] - 10 ) / 2f);   
            }
            
        }

        public int AttackMelee()
        {
            if (random == null) random = new Random();
            int roll = random.Next(21);
            roll += ability_modifiers[0];
            return roll;
        }

        public int AttackRanged()
        {
            if (random == null) random = new Random();
            int roll = random.Next(21);
            roll += ability_modifiers[1];
            return roll;
        }

        public void Save(string url)
        {
            string json = JsonConvert.SerializeObject(this);
            System.IO.FileInfo file = new System.IO.FileInfo(url+"charaters\\"+id+".json");
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, json);

            Console.WriteLine(json);
        }

    }
}
