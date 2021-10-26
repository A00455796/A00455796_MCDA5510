Assignment #1
-------------------------------------------------------------------

The assignment's purpose is to go through the directory and check the records in the CSV files.
- Using the methods below, I built a c# code to achieve the goal.
	- processDir() traverses the directories in a recursive manner
	- processFile() validates CSV files before sending them to csvParser(). 
	- csvParser() reads the file and validates the records. In addition, the good records are recorded in a separate csv file. 
	- Logger() creates a log file with information about the number of faulty records, valid records, execution time, and bad record filepath.
	
Instructions:-

Please configure the following variables in the class Fileprocessor
- <path> The path for csv files
- <o_path> Desitination path of the output file having valid records along with the <filename>.csv
- <logpath> Desitination path of the log file including <logfilename>.txt 
