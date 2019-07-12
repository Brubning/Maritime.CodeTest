using System.Collections.Generic;

namespace Maritime.CodeTest
{
    /// <summary>
    /// Describes result of IFileReader.
    /// </summary>
    public class FileReadResult<T>
    {
        /// <summary>
        /// Result of the file read operation.
        /// </summary>
        public ReadResult ReadResult { get; set; }

        /// <summary>
        /// Count of lines read from file before conversion.
        /// </summary>
        public int LineCount { get; set; } = 0;

        /// <summary>
        /// Count of lines read from file and converted.
        /// </summary>
        public int ReadCount { get; set; } = 0;

        /// <summary>
        /// Count of lines read from file and failed in conversion.
        /// </summary>
        public int ErrorCount { get; set; } = 0;

        /// <summary>
        /// List of lines read and failed in conversion.
        /// </summary>
        public List<string> ErrorLines { get; set; } = new List<string>();

        /// <summary>
        /// List of lines read and converted correctly.
        /// </summary>
        public List<T> ReadLines { get; set; } = new List<T>();
    }

    public enum ReadResult
    {
        FileNotFound,
        FailedOnRead,
        FileReadOk
    }
}
