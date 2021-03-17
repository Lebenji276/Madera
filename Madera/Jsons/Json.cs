using Madera.Classe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Madera.Jsons
{
    class Json
    {
        public static string getPath(string filename)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string[] currentDirSplitted = currentDir.Split("\\bin");
            return currentDirSplitted[0] + "\\Jsons\\" + filename + ".json";
        }
        public static string writeJson(string filename, string jsonToWrite, bool update = false)
        {
            string path = getPath(filename);

            if (update && File.Exists(path))
            {
                File.Delete(path);
            }

            if (!File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(jsonToWrite.ToString());
                    tw.Close();
                }
            }

            return path;
        }
    }
}
