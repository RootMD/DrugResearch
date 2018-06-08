using Accord.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugResearch
{
    class Program
    {
        static void Main(string[] args)
        {

            CsvReader bla = CsvReader.FromText(@"C:\Users\Michau\Desktop\Projekt SI\DrugResearch\DrugResearch\Zeszyt1.csv", false);
            DataTable data = bla.ToTable();
        }
    }
}
