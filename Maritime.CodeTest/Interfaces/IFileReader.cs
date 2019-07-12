using System.Collections.Generic;

namespace Maritime.CodeTest.Interfaces
{
    /// <summary>
    /// Provides logic to read files.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFileReader<T>
    {
        /// <summary>
        /// Read all lines in the specified file and return a list of the contents.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileReadResult<T> ReadAllLines(string path);
    }
}
