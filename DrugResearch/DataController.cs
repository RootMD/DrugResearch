using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;

namespace DrugResearch
{
    public static class DataController
    {
        public static string[][] Load(string pathName)
        {

            List<string[]> output = new List<string[]>();

            using (FileStream fs = File.Open(pathName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    List<string> row = new List<string>();

                    string line;
                    
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] chLine = line.Split(',');
                        chLine = chLine.Skip(1).ToArray();
                        output.Add(chLine);

                    }
                }
            }
                return output.ToArray();
        }

        public static DataTable MakeData(string pathName)
        {
            string[][] DataInput = Load(pathName);

            DataTable data = new DataTable("The Five Factor Model of personality and evaluation of drug consumption risk");

            data.Columns.Add("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive",
                "SS", "Alcohol", "Amfet", "Amyl", "Benzos", "Cofeine", "Cannabis", "Chocolate", "Coke", "Crac", "Ecstasy", "Heroine",
                "Ketamine", "LegalH", "LSD", "Meth", "Mushrooms", "Nicotine", "Semeron", "VSA");

            foreach (string[] DataRow in DataInput)
            {
                data.Rows.Add(DataRow);
            }

            return data;
        }

        public static DecisionVariable[] GetAttributes()
        {
            DecisionVariable[] attributes =
            {
                new DecisionVariable("Age", 6), // 6 possible values
                new DecisionVariable("Gender", 2), // 2 possible values 
                new DecisionVariable("Education", 9), // 9 possible values  
                new DecisionVariable("Country", 7),  // 7 possible values
                new DecisionVariable("Eticnity", 7), // 7 possible values
                new DecisionVariable("Nscore", 49), // 17 possible values 
                new DecisionVariable("Escore", 42), // 14 possible values (High, normal)    
                new DecisionVariable("Oscore", 35),  // 12 possible values (Weak, strong) 
                new DecisionVariable("Ascore", 41), // 14 possible values (Sunny, overcast, rain)
                new DecisionVariable("Cscore", 41), // 14 possible values (Hot, mild, cool)  
                new DecisionVariable("Impulsive", 10), // 10 possible values (High, normal)    
                new DecisionVariable("SS", 11),  // 11 possible values (Weak, strong) 
                //new DecisionVariable("Alcohol", 7), // 7 possible values (Sunny, overcast, rain)
                //new DecisionVariable("Amfet", 7), // 7 possible values (Hot, mild, cool)  
                //new DecisionVariable("Amyl",    7), // 7 possible values (High, normal)    
                //new DecisionVariable("Benzos",        7),  // 7 possible values (Weak, strong) 
                //new DecisionVariable("Cofeine", 7), // 7 possible values (Sunny, overcast, rain)
                //new DecisionVariable("Cannabis", 7), // 7 possible values (Hot, mild, cool)  
                //new DecisionVariable("Chocolate",    7), // 7 possible values (High, normal)    
                //new DecisionVariable("Coke",        7),  // 7 possible values (Weak, strong) 
                //new DecisionVariable("Crac", 7), // 7 possible values (Hot, mild, cool)  
                //new DecisionVariable("Ecstasy",    7), // 7 possible values (High, normal)    
                //new DecisionVariable("Heroine",        7),  // 7 possible values (Weak, strong) 
                //new DecisionVariable("Ketamine", 7), // 7 possible values (Sunny, overcast, rain)
                //new DecisionVariable("LegalH", 7), // 7 possible values (Hot, mild, cool)  
                //new DecisionVariable("LSD",    7), // 7 possible values (High, normal)    
                //new DecisionVariable("Meth",        7),  // 7 possible values (Weak, strong) 
                //new DecisionVariable("Mushrooms", 7), // 7 possible values (Sunny, overcast, rain)
                //new DecisionVariable("Nicotine", 7), // 7 possible values (Hot, mild, cool)  
                //new DecisionVariable("Semeron",    7), // 7 possible values (High, normal)    
                //new DecisionVariable("VSA",        7),  // 7 possible values (Weak, strong) 
            };

            return attributes;
        }
    }
}
