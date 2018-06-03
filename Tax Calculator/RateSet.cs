using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Calculator
{
    [Serializable]
    class RateSet
    {
        public RateSet(Hashtable r) => this.Rates = r; //Constructor
        public Hashtable Rates { get; set; } //Defined by constructor param
        public string Name { get; set; }

        public void DoParse(string command)
        {
            //Ex Cmd: declare set TaxSet1 sized 5 // new 'empty' ("UNDEFINED") set where the matrix size is [2, 5]
            //Ex Cmd: declare set TaxSet2 as [ cap1 -> rate1 cap2 -> rate2 ] // new set where matrix size is [2, 2]

            string[] wrds = command.Split(new char[] { ' ' });
            RateSet local = new RateSet(
                new Hashtable());
            local.Name = wrds[2];
            if (wrds[3] == "sized")
            {
                Random r = new Random();
                for (int i = 0; i < int.Parse(wrds[4]); i++)
                {
                    int RandomValue = r.Next(9999);
                    local.Add($"<CPCV { RandomValue }>", $"<RPCV { RandomValue }>");
                }
                this.Name = local.Name;
                this.Rates = local.Rates;
            }
            else if (wrds[3] == "as")
            {
                bool IsInBrackets = false;
                List<string> InBrackets = new List<string>();
                foreach (string s in wrds)
                {
                    if (s == "[") IsInBrackets = true;
                    else if (s == "]") IsInBrackets = false;
                    else if (IsInBrackets)
                    {
                        InBrackets.Add(s);
                    }
                }

                List<string> localcaps = new List<string>();
                List<string> localrates = new List<string>();
                Int32 e = 0;

                foreach (string wrd in InBrackets)
                {
                    if (wrd == "->")
                    {
                        localcaps.Add(InBrackets[e - 1]);
                        localrates.Add(InBrackets[e + 1]);
                    }
                    e++;
                }

                Hashtable localhashtable = new Hashtable();
                for(int i = 0; i < localcaps.Count; i++)
                {
                    localhashtable.Add(localcaps[i], localrates[i]);
                }

                this.Name = local.Name;
                this.Rates = localhashtable;
            }
            //Otherwise undefined
        }

        public void Add(object Cap, object Rate) => this.Rates.Add(Cap, Rate);

        public string[,] GetMatrix()
        {
            string[,] local = new string[2, this.Rates.Count];
            int i = 0;
            foreach (DictionaryEntry rate in this.Rates)
            {
                local[0, i] = rate.Key as string;
                local[1, i] = rate.Value as string;
                i++;
            }
            return local;
        } 

        public void SaveMe(string path)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (FileStream s = File.Create(path))
            {
                binformatter.Serialize(s, this);
                s.Close();
            }
        }

        public void LoadMe(string path)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binloader = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (FileStream s = File.OpenRead(path))
            {
                RateSet temp = binloader.Deserialize(s) as RateSet;

                //Update Properties
                this.Name = temp.Name;
                this.Rates = temp.Rates;
                //</>

                s.Close();
            }
        }
    }
}
