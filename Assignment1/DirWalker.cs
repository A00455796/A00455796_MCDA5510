using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Diagnostics;


namespace MCDA5510_Assignment1
{
    public class Fileprocessor
    {
        public string path = "C:\\Users\\Admin\\Documents\\Masters-Computing and Data Analytics\\Sep-Dec2021\\Software Dev in Business Env - 5510\\Assignment1\\Sample Data\\";
        public string logpath = "C:\\Users\\Admin\\Documents\\DirWalker_Logs.txt";
        public string o_path = "C:\\Users\\Admin\\Documents\\output.csv";
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
                Fileprocessor.Logger(logpath,"The files in this path "+path+" is not .csv extension");
            }
            else
            {
                csvParser(path, o_path);
            }   
        }

        public void csvParser(string filepath, string outpath)
        {
            bool write = false;
            StringBuilder sboutput = new StringBuilder();
            TextFieldParser parser = new TextFieldParser(filepath);
            parser.TextFieldType = FieldType.Delimited;
            parser.HasFieldsEnclosedInQuotes = true;
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
                        if (string.IsNullOrEmpty(field) || fields.Length != 10)
                        {
                            badRecordCount += 1;
                            Fileprocessor.Logger(logpath,"Error in the file " + filepath + " : Missing record - Any field is Null or Empty");
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

            File.AppendAllText(outpath, sboutput.ToString());
        }

        public static void Logger(string logpath, string lines)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(logpath, true);
            file.WriteLine(DateTime.Now.ToString() + ": " + lines);
            file.Close(); 
        }

        static void Main(string[] args)
        {
            Fileprocessor fp = new Fileprocessor();
            if (System.IO.File.Exists(fp.logpath))
            {
                File.Delete(fp.logpath);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            
            File.WriteAllText(fp.o_path, "First Name,Last Name,Street Number,Street,City,Province,Postal Code,Country,Phone Number,email Address\n");
            if (Directory.Exists(fp.path))
            {
                fp.processDir(fp.path);
            }
            else
            {
                fp.processFile(fp.path);
            }

            stopwatch.Stop();
            
            Logger(fp.logpath,"Total Number of Missing/Bad Records : "+ fp.badRecordCount.ToString());
            Logger(fp.logpath,"Total Number of Valid Records : " + (fp.recordCount - fp.badRecordCount).ToString());
            Logger(fp.logpath,"Total Execution Time in ms : " + stopwatch.ElapsedMilliseconds.ToString());
        }
    }
}
