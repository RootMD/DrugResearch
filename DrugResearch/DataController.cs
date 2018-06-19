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

        public static double[][] convertDT(double[][] StringTable)
        {
            List<double[]> output = new List<double[]>();
            List<double> One = new List<double>();
            for(int i=0; StringTable.GetLength(0)>i; i++)
            {
                for (int j = 0; j < 7; j++){
                    
                    if (j == StringTable[i][0])
                    {
                        One.Add(1);
                    }
                    else
                    {
                        One.Add(0);
                    }
                }
                output.Add(One.ToArray());
                One = new List<double>();
            }
            

            return output.ToArray();
        }

        public static string[][] MakeString(string pathName, out string[][] outputs)
        {
            string[][] start = Load(pathName);
            List<string[]> input = new List<string[]>();
            List<string[]> output = new List<string[]>();
            List<string> attr = new List<string>();
            List<string> allOutputs = new List<string>();
            for(int i=0; start.GetLength(0)>i; i++)
            {
                for (int j=0; j<start[i].GetLength(0);j++)
                {
                    if (j <= 11)
                        attr.Add(start[i][j]);
                    else
                        allOutputs.Add(start[i][j]);
                }
                input.Add(attr.ToArray());
                output.Add(allOutputs.ToArray());
                attr = new List<string>();
                allOutputs = new List<string>();
            }
            
            
            outputs = output.ToArray();
            return input.ToArray();
        }

        public static DataTable MakeDataTable(string pathName)
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

        public static DataTable MakeDataFromString(string[][] Input, string inp)
        {
            DataTable data = new DataTable("The Five Factor Model of personality and evaluation of drug consumption risk");
            if(inp.Equals("input"))
            {
                data.Columns.Add("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive",
                "SS");
            }
            else
            {
                data.Columns.Add("Alcohol", "Amfet", "Amyl", "Benzos", "Cofeine", "Cannabis", "Chocolate", "Coke", "Crac", "Ecstasy", "Heroine",
                "Ketamine", "LegalH", "LSD", "Meth", "Mushrooms", "Nicotine", "Semeron", "VSA");
            }
            
            foreach (string[] DataRow in Input)
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
                new DecisionVariable("Escore", 42), // 14 possible values
                new DecisionVariable("Oscore", 35),  // 12 possible values
                new DecisionVariable("Ascore", 41), // 14 possible values 
                new DecisionVariable("Cscore", 41), // 14 possible values 
                new DecisionVariable("Impulsive", 10), // 10 possible values   
                new DecisionVariable("SS", 11),  // 11 possible values 
            };

            return attributes;
        }
    }
}
