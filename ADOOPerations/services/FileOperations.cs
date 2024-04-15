using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOPerations.services
{
    public static class FileOperations
    {
        public static string ReadJsonFromFile(string fileName)
        {
            string value = string.Empty;
            try
            {
                Console.WriteLine();
             
                var filePath = GetDirectory()+"\\"+ fileName;
                Console.WriteLine($"Reading from  a file: {filePath}......");
                // Open the text file using a StreamReader
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the entire file and display its contents
                    string contents = sr.ReadToEnd();
                    value = contents;
                }
                Console.WriteLine("Finished reading from a file.");
            }
            catch (Exception e)
            {
                // Display any errors that occur
                Console.WriteLine("Error reading the file:");
                Console.WriteLine(e.Message);
            }
            return value;
        }

        public static void WriteToFile(string jsonString, string fileName)
        {
            try
            {
                var filePath = GetDirectory();
                var fullFileName = $"{filePath}\\{fileName}";
                // Open the text file using a StreamWriter
                using (StreamWriter sw = new StreamWriter(fullFileName))
                {
                    // Write the string to the file
                    sw.Write(jsonString);
                }
                Console.WriteLine($"A backup has been created for this pr. Location : {fullFileName}");
            }
            catch (Exception e)
            {
                // Display any errors that occur
                Console.WriteLine("Error writing to the file:");
                Console.WriteLine(e.Message +" inner exception: "+e?.InnerException?.Message);
            }
        }

        public static (Dictionary<string, List<long>>, Dictionary<long, List<string>>) GetCurrentFilesHashTable()
        {
            var files = GetAllDirectoryFiles();
           var results =  GenericHelper.GetFileNameHashMap(files);
            return results;
        }

        private static string[] GetAllDirectoryFiles()
        {
            List <string>res = new List<string>();
            try
            {
                var filePath = GetDirectory();
                Console.WriteLine("Getting all files in a directory");
                // Get all files in the directory
                var files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    res.Add(Path.GetFileName(file));
            }
            catch (Exception e)
            {
                // Display any errors that occur
                Console.WriteLine("Error accessing the directory:");
                Console.WriteLine(e.Message);
            }

            return res.ToArray();
        }

        private static string GetDirectory()
        {
            var result = Directory.CreateDirectory("backup");
            return result.FullName;
        }
    }
}
