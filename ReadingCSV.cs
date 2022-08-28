using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;


namespace classical_genetic
{
    class ReadingCSV
    {


        public static List<List<double>> Parse(string path_str)
        {
            string coloumn1;
            string coloumn2;
            List<List<double>> csvList = new List<List<double>>();
            //var path = @"C:\pobrane\deskryptory partycji do predykcji\22\descriptors_user_3.csv";
            var path = @path_str;
            using (TextFieldParser csvReader = new TextFieldParser(path_str))
            {
                csvReader.CommentTokens = new string[] { "#" };
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvReader.ReadLine();

                while (!csvReader.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvReader.ReadFields();
                    List<string> fieldsList = fields.ToList();
                    fieldsList = fieldsList.GetRange(1, 16);
                    List<double> result = fieldsList.Select(x => double.Parse(x)).ToList();
                    csvList.Add(result);
                    
                }
                
                return csvList;
            }
        }
    }
}

