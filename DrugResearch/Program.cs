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
using Accord.Neuro.Networks;
using Accord.Neuro;
using Accord.Neuro.Learning;

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
                        Neural_Network(true);
                        Console.Clear();
                        break;
                    case "3":
                        double TreesError = Decision_Tree(false);
                        double NetworkError = Neural_Network(false);
                        Console.WriteLine(TreesError + " (Trees Error) - " + NetworkError + " (Network Error) = " + Math.Round(Math.Abs(TreesError-NetworkError),5));
                        Console.WriteLine();
                        if(NetworkError>TreesError)
                            Console.WriteLine("Sieci neuronowe okazały się mniej skuteczną metodą w tym przykładzie");
                        else
                            Console.WriteLine("Drzewa decyzyjne okazały się mniej skuteczną metodą w tym przykładzie");
                        Console.ReadKey();
                        Console.Clear();
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
            DataTable data = DataController.MakeDataTable("../../drug_consumption.txt");
            DataTable entireData = DataController.MakeDataTable("../../drug_consumption.txt");
            DataTable tests = DataController.MakeDataTable("../../drug_consumption.txt");
            Codification codebook = new Codification(entireData);
            DecisionVariable[] attributes = DataController.GetAttributes();
            int classCount = 7; // (7) "Never Used", "Used over a Decade Ago", "Used in Last Decade", "Used in Last Year", "Used in Last Month", "Used in Last Week", and "Used in Last Day"

            DecisionTree tree = new DecisionTree(attributes, classCount);
            ID3Learning id3learning = new ID3Learning(tree);

            DataTable symbols = codebook.Apply(data);
            string LookingFor = "Crac";
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
                Console.WriteLine("Blad - " + Math.Round(error,4) +"%");
                Console.ReadKey();
            }
            return error;
        }

        static double Neural_Network(bool show)
        {
            double error = new double();
            DataTable entireData = DataController.MakeDataTable("../../drug_consumption.txt");
            Codification codebook = new Codification(entireData);
            //"Alcohol", "Amfet", !!"Amyl", "Benzos", "Cofeine", "Cannabis", "Chocolate", "Coke", !!!!"Crac", ///"Ecstasy", !!"Heroine",
            //    !!"Ketamine", //"LegalH", "LSD", !!"Meth", //"Mushrooms", "Nicotine", lol "Semeron", "VSA"
            string LookingFor = "Crac";
            int good = 0;
            string[][] outputs;
            string[][] inputs = DataController.MakeString("../../drug_consumption.txt", out outputs);
            string[][] testOutputs;
            string[][] testInputs = DataController.MakeString("../../drug_consumption.txt", out testOutputs);

            DataTable outputs1 = DataController.MakeDataFromString(outputs,"output");
            DataTable inputs1 = DataController.MakeDataFromString(inputs,"input");
            DataTable testOutputs1 = DataController.MakeDataFromString(testOutputs,"output");
            DataTable testInputs1 = DataController.MakeDataFromString(testInputs,"input");

            DataTable Isymbols = codebook.Apply(inputs1);
            DataTable Osymbols = codebook.Apply(outputs1);
            DataTable TIsymbols = codebook.Apply(testInputs1);
            DataTable TOsymbols = codebook.Apply(testOutputs1);
            double[][] inputsD = Isymbols.ToJagged<double>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            double[][] outputsD = Osymbols.ToJagged<double>(LookingFor);
            outputsD = DataController.convertDT(outputsD);
            double[][] inputsT = TIsymbols.ToJagged<double>("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", "SS");
            double[][] outputsT = TOsymbols.ToJagged<double>(LookingFor);
            outputsT = DataController.convertDT(outputsT);

            DeepBeliefNetwork network = new DeepBeliefNetwork(inputs.First().Length, 15, 7);
            new GaussianWeights(network, 0.1).Randomize();
            network.UpdateVisibleWeights();
            DeepBeliefNetworkLearning FirstLearner = new DeepBeliefNetworkLearning(network)
            {
                Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
                {
                    LearningRate = 0.2,
                    Momentum = 0.5,
                    Decay = 0.01,
                }
            };
            
            int batchCount = Math.Max(1, inputs.Length / 100);
            int[] groupsNew = Accord.Statistics.Classes.Random(inputsD.Length, batchCount);
            double[][][] batchesNew = Accord.Statistics.Classes.Separate(inputsD, groupsNew);
            double[][][] layerData;
            
            for (int layerIndex = 0; layerIndex < network.Machines.Count - 1; layerIndex++)
            {
                FirstLearner.LayerIndex = layerIndex;
                layerData = FirstLearner.GetLayerInput(batchesNew);
                for (int i = 0; i < 500; i++)
                {
                    error = FirstLearner.RunEpoch(layerData) / inputsD.Length;
                }
            }

            var SecondLearner = new BackPropagationLearning(network)
            {
                LearningRate = 0.15,
                Momentum = 0.5
            };

            for (int i = 0; i < 800; i++)
            {
                error = SecondLearner.RunEpoch(inputsD, outputsD) / inputsD.Length;
            }
            
            for (int i = 0; i < inputsD.Length; i++)
            {
                double[] outputValues = network.Compute(inputsT[i]);
                if (outputValues.ToList().IndexOf(outputValues.Max()) == outputsT[i].ToList().IndexOf(outputsT[i].Max()))
                {
                    good++;
                }
            }
            if(show==true)
            {
                Console.WriteLine("Poprawność - " + Math.Round(((double)good / (double)inputsD.Length * 100), 4) + "%");
                Console.ReadKey();
            }
            
            return error;
        }
    }
}
