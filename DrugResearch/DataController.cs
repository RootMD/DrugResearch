﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.IO;

namespace DrugResearch
{
    public class DataController
    {
        public string[][] Load(string pathName)
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

                    }
                }
            }
                return output.ToArray();
        }
    }
}