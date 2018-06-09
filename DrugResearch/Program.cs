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


namespace DrugResearch
{
    class Program
    {
        static void Main(string[] args)
        {

        //DataTable data = DataController.MakeData("../../drug_consumption_50.txt");
        //DataTable entireData = DataController.MakeData("../../drug_consumption.txt");
        //Codification codebook = new Codification(entireData);
        //DataTable symbols = codebook.Apply(data);
        //int[][] inputs = symbols.ToJagged<int>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
        //int[] outputs = symbols.ToArray<int>("Cofeine");

        //var id3learning = new ID3Learning()
        //{

        //    new DecisionVariable("Age", 6), // 6 possible values
        //    new DecisionVariable("Gender", 2), // 2 possible values 
        //    new DecisionVariable("Education", 9), // 9 possible values  
        //    new DecisionVariable("Country", 7),  // 7 possible values
        //    new DecisionVariable("Eticnity", 7), // 7 possible values
        //    new DecisionVariable("Nscore", 49), // 17 possible values 
        //    new DecisionVariable("Escore", 42), // 14 possible values (High, normal)    
        //    new DecisionVariable("Oscore", 35),  // 12 possible values (Weak, strong) 
        //    new DecisionVariable("Ascore", 41), // 14 possible values (Sunny, overcast, rain)
        //    new DecisionVariable("Cscore", 41), // 14 possible values (Hot, mild, cool)  
        //    new DecisionVariable("Impulsive", 10), // 10 possible values (High, normal)    
        //    new DecisionVariable("SS", 11),  // 11 possible values (Weak, strong) 
        //};
        //DecisionTree tree = id3learning.Learn(inputs, outputs);

        DataTable data = DataController.MakeData("../../drug_consumption.txt");
            DataTable entireData = DataController.MakeData("../../drug_consumption.txt");
            Codification codebook = new Codification(entireData);
            DecisionVariable[] attributes = DataController.GetAttributes();
            int classCount = 7; // (7) "Never Used", "Used over a Decade Ago", "Used in Last Decade", "Used in Last Year", "Used in Last Month", "Used in Last Week", and "Used in Last Day"

            DecisionTree tree = new DecisionTree(attributes, classCount);
            ID3Learning id3learning = new ID3Learning(tree);

            DataTable symbols = codebook.Apply(data);

            int[][] inputs = symbols.ToJagged<int>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            int[] outputs = symbols.ToArray<int>("Cofeine");

            //int[][] inputs = symbols.ToIntArray("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            //int[] outputs = symbols.ToIntArray("Alcohol").GetColumn(0);

            id3learning.Learn(inputs, outputs);

            int[] query1 = codebook.Transform("-0.07854", "0.48246", "1.98437", "-0.09765", "-0.31685", "0.13606", "-0.43999", "-1.55521", "-0.01729", "0.93949", "-1.37983", "-1.54858");
            int predicted = tree.Decide(query1);
            string answer = codebook.Revert("Coke", predicted);
            Console.WriteLine(answer);
            double error = new ZeroOneLoss(query1).Loss(tree.Decide(inputs));

            DecisionTreeView NewTree = new DecisionTreeView();
            NewTree.TreeSource = tree;


            Console.WriteLine(error);
            Console.ReadKey();
        }
    }
}
