using Accord;
using Accord.IO;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.MachineLearning.DecisionTrees.Rules;

namespace DrugResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            int Quitting = 0;
            while(Quitting == 0)
            {
                Console.WriteLine("Menu : wybór metody uczenia");
                Console.WriteLine("1.Drzewa Decyzyjne");
                Console.WriteLine("2.Sieci neuronowe");
                Console.WriteLine("3.Porównanie metod");
                Console.WriteLine("4.Zamknij");
                string name = Console.ReadLine();
                Console.Clear();
                switch (name)
                { 
                    
                    case "1":
                        Decision_Tree(true);
                        Console.Clear();
                        break;
                    case "2":

                        
                        break;
                    case "3":
                        double TreesError = Decision_Tree(false);

                        break;
                    case "4":
                        Quitting = 1;
                        break;
                    default:
                        
                        Console.WriteLine("Wybierz od 1-4");
                        Console.WriteLine();
                        break;
                }
            }
            
        }

        static double Decision_Tree(bool show)
        {
            DataTable data = DataController.MakeData("../../drug_consumption_20.txt");
            DataTable entireData = DataController.MakeData("../../drug_consumption.txt");
            DataTable tests = DataController.MakeData("../../drug_consumption_test.txt");
            Codification codebook = new Codification(entireData);
            DecisionVariable[] attributes = DataController.GetAttributes();
            int classCount = 7; // (7) "Never Used", "Used over a Decade Ago", "Used in Last Decade", "Used in Last Year", "Used in Last Month", "Used in Last Week", and "Used in Last Day"

            DecisionTree tree = new DecisionTree(attributes, classCount);
            ID3Learning id3learning = new ID3Learning(tree);

            DataTable symbols = codebook.Apply(data);
            string LookingFor = "Cannabis";
            int[][] inputs = symbols.ToJagged<int>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            int[] outputs = symbols.ToArray<int>(LookingFor);

            id3learning.Learn(inputs, outputs);
            DataTable testSymbols = codebook.Apply(tests);
            int[][] testIn = testSymbols.ToJagged<int>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            int[] testOut = testSymbols.ToArray<int>(LookingFor);
            DecisionSet rules = tree.ToRules();
            string ruleText = rules.ToString(codebook, LookingFor, System.Globalization.CultureInfo.InvariantCulture);
            double error = new ZeroOneLoss(testOut).Loss(tree.Decide(testIn));
            if (show == true)
            {
                Console.WriteLine(ruleText);
                Console.ReadKey();
                Console.WriteLine(error);
                Console.ReadKey();
            }
            return error;
        }

    }
}
