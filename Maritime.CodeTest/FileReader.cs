using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maritime.CodeTest.Interfaces;

namespace Maritime.CodeTest
{
    public class FileReader : IFileReader<decimal>
    {
        /// <inheritdoc />
        public FileReadResult<decimal> ReadAllLines(string path)
        {
            var result = new FileReadResult<decimal>();

            try
            {
                var allLines = ReadAsString(path);
                result.ReadResult = ReadResult.FileReadOk;
                result.LineCount = allLines.Count;
//TODO SIMPLIFY
                // Could simply use .Select(...) but don't get the same level of transparency around problems.
                foreach (var line in allLines)
                {
                    var numbers = line.Split(',');
                    foreach(var x in numbers)
                    {
                        // Safely parse to avoid report errors at line level.
                        var isValid = decimal.TryParse(x, out var decResult);
                        if (!isValid)
                        {
                            // Keep a record of lines that didn't parse correctly.
                            result.ErrorCount++;
                            result.ErrorLines.Add(line);
                            continue;
                        }

                        result.ReadCount++;
                        result.ReadLines.Add(decResult);
                    }
                }
            }
            // TODO Add Logging with detail of exception (ExceptionHelper.Log)
            catch (FileNotFoundException)
            {
                result.ReadResult = ReadResult.FileNotFound;
                return result;
            }
            catch
            {
                result.ReadResult = ReadResult.FailedOnRead;
                return result;
            }

            return result;
        }

        /// <summary>
        /// Pre-read file as string content before converting each line to int
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<string> ReadAsString(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            return File.ReadAllLines(path).ToList();
        }
    }
}
