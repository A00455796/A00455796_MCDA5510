using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace MCDA5510_Assignment1
{
    public class Fileprocessor
    {
        public string path = "C:\\Users\\Admin\\Documents\\Masters-Computing and Data Analytics\\Sep-Dec2021\\Software Dev in Business Env - 5510\\Assignment1\\Sample Data\\";
        public int badRecordCount = 0;
        public int recordCount = 0;
        public void processDir(string path)
        {
            string[] list = Directory.GetDirectories(path);
            
            if (list == null) return;
            
            foreach (string dir in list)
            {
                processDir(dir);
            }

            string[] files = Directory.GetFiles(path);

            foreach (string fil in files)
            {
                processFile(fil);
            }
        }
        public void processFile(string path)
        {
            if (Path.GetExtension(path) != ".csv")
            {
                Fileprocessor.Logger("The files in this path "+path+" is not .csv extension");
                //Console.WriteLine("No csv file in the path {0}", path);
            }
            else
            {
                csvParser(path);
            }
            
        }

        public void csvParser(string filepath)
        {
            string o_path = "C:\\Users\\Admin\\Documents\\output.csv";
            
            bool write = false;

            StringBuilder sboutput = new StringBuilder();
            TextFieldParser parser = new TextFieldParser(filepath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            int i = 0;
           
            while (!parser.EndOfData)
            {
                write = true;
                string[] fields = parser.ReadFields();
                if (i > 0)
                {
                    foreach (string field in fields)
                    {
                        if (string.IsNullOrEmpty(field))
                        {
                            badRecordCount += 1;
                            //
                            Fileprocessor.Logger("Error in the file " + filepath + " : Missing record - Any field is Null or Empty");
                            write = false;
                            break;
                        }
                        if (fields.Length != 10)
                        {
                            badRecordCount += 1;
                            Fileprocessor.Logger("Error in the file " + filepath + " :  The records are imcomplete");
                            write = false;
                            break;
                        }
                        
                    }
                    recordCount += 1;
                }
                else
                {
                    write = false;
                }

                if (write == true)
                {
                    sboutput.AppendLine($"\"{fields[0]}\",\"{fields[1]}\",\"{fields[2]}\",\"{fields[3]}\",\"{fields[4]}\",\"{fields[5]}\",\"{fields[6]}\",\"{fields[7]}\",\"{fields[8]}\",\"{fields[9]}\"") ;
                }
                i++;
            }

            File.AppendAllText(o_path, sboutput.ToString());
        }

        public static void Logger(string lines)
        {
            string path = "C:\\Users\\Admin\\Documents\\DirWalker_Logs.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
            file.WriteLine(DateTime.Now.ToString() + ": " + lines);
            file.Close();
            
        }

        static void Main(string[] args)
        {
            if (System.IO.File.Exists("C:\\Users\\Admin\\Documents\\DirWalker_Logs.txt"))
            {
                File.Delete("C:\\Users\\Admin\\Documents\\DirWalker_Logs.txt");
            }
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            
            Fileprocessor fp = new Fileprocessor();
            File.WriteAllText("C:\\Users\\Admin\\Documents\\output.csv", "First Name,Last Name,Street Number,Street,City,Province,Postal Code,Country,Phone Number,email Address\n");
            if (Directory.Exists(fp.path))
            {
                fp.processDir(fp.path);
            }
            else
            {
                fp.processFile(fp.path);
            }

            stopwatch.Stop();
            
            
            Logger("Total Number of Missing/Bad Records : "+ fp.badRecordCount.ToString());
            Logger("Total Number of Valid Records : " + (fp.recordCount - fp.badRecordCount).ToString());
            Logger("Total Execution Time in ms : " + stopwatch.ElapsedMilliseconds.ToString());
        }
    }
}
