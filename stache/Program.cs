using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stache
{
    public class ParseException : Exception 
    {
        public ParseException(string error) : base(error) { }
    }

    class Program
    {
        const string STACHE_SRC_DIR = ".stache-src";
        static string _BasePath = "";

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Either run within a directory with a " + STACHE_SRC_DIR + " folder or pass in the name of a folder containing one.");
                return;
            }

            var directory = Directory.GetCurrentDirectory();
            if (args.Length == 1)
                directory = args[0];

            _BasePath = Path.GetFullPath(directory) + Path.DirectorySeparatorChar + STACHE_SRC_DIR;
            BuildDirectory(_BasePath);
        }

        private static void BuildDirectory(string directory)
        {
            try
            {
                var includeFiles = Directory.GetFiles(directory, "*._.*");

                var files = Directory.GetFiles(directory);
                foreach (var file in files)
                {
                    if (includeFiles.Contains<string>(file))
                        continue;

                    var targetDir = directory.Replace(STACHE_SRC_DIR, "") + Path.DirectorySeparatorChar;
                    if (!Directory.Exists(targetDir))
                        Directory.CreateDirectory(targetDir);

                    File.WriteAllText(targetDir + Path.GetFileName(file), Parse(file));
                }
                foreach (var nested in Directory.GetDirectories(directory))
                {
                    BuildDirectory(nested);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("A " + STACHE_SRC_DIR + " was not found");
            }
        }

        private static string Parse(string fileName)
        {
            string[] lines = null;
            var dirName = Path.GetDirectoryName(fileName);

            try { lines = File.ReadAllLines(fileName); }
            catch (FileNotFoundException)
            {
                if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                {
                    var files = Directory.GetFiles(dirName, Path.GetFileNameWithoutExtension(fileName) + "._.*");
                    if (files.Count() == 0)
                    {
                        files = Directory.GetFiles(_BasePath, Path.GetFileNameWithoutExtension(fileName) + "._.*");
                        if (files.Count() == 0)
                        {
                            throw new ParseException("Snippet not found: " + Path.GetFileName(fileName));
                        }
                    }

                    if (files.Count() > 1)
                        throw new ParseException("Ambiguos snippet file: " + Path.GetFileName(fileName));

                    lines = File.ReadAllLines(files[0]);
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i=0; i<lines.Length; i++)
            {
                var line = lines[i];
                var left = line.IndexOf("{{");
                if (left != -1)
                {
                    if (left != 0)
                        sb.Append(line.Substring(0, left));

                    left += 2;

                    var right = line.IndexOf("}}");
                    if (right == -1)
                        throw new ParseException("Expected }} in " + fileName + " at line " + i);

                    sb.Append(Parse(dirName + Path.DirectorySeparatorChar + line.Substring(left, right - left)));

                    // get the rest
                    sb.Append(line.Substring(right + 2));
                }
                else
                {
                   sb.Append(line.TrimEnd() + Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
