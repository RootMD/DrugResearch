using Accord;
using Accord.IO;
using Accord.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            string[][] DataInput = DataController.Load("../../drug_consumption_100.txt");

            DataTable data = new DataTable("The Five Factor Model of personality and evaluation of drug consumption risk");

            data.Columns.Add("Age", "Gender", "Education", "Country", "Eticnity", "Nscore", "Escore", "Oscore", "Ascore", "Cscore", "Impulsive", 
                "SS", "Alcohol", "Amfet", "Amyl" , "Benzos" , "Cofeine" , "Cannabis" , "Chocolate" , "Coke" , "Crac" , "Ecstasy" , "Heroine" , 
                "Ketamine" , "LegalH" , "LSD" , "Meth" , "Mushrooms" , "Nicotine" , "Semeron" , "VSA");

            data.Rows.Add("1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3", "1", "2", "3",
                "1", "2", "3", "1", "2", "3", "31");

            Console.WriteLine(data.Rows.ToString());
            //foreach(string[] DataRow in DataInput)
            //{
            //    data.Rows.Add(DataRow);
            //}
            //foreach(DataRow row in data.Rows)
            //{
            //    Console.WriteLine(row.ToString());
            //}
            Console.ReadKey();
        }
    }
}
