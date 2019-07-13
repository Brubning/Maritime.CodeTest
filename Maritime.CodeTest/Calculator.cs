using System;
using System.Collections.Generic;
using System.Linq;

namespace Maritime.CodeTest
{
    /// <summary>
    /// Provides static methods for required maths operations.
    /// This is one option, could also look at extension methods or an interface based class.
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Calculate the arithmetic mean of a list of decimals.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static decimal ArithmeticMean(List<decimal> numbers)
        {
            var n = 0;
            var sum = 0m;

            foreach (var x in numbers)
            {
                n++;
                sum += x;
            }

            return sum / Convert.ToDecimal(n);
        }

        /// <summary>
        /// Calculate the standard deviation of a list of integer.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal StandardDeviation(List<decimal> decimals)
        {
            /*
            1. Work out the Mean (the simple average of the numbers)
            2. Then for each number: subtract the Mean and square the result
            3. Then work out the mean of those squared differences.
            4. Take the square root of that and we are done!
            */
            
            var mean = ArithmeticMean(decimals);
            var deviations = new List<decimal>();

            foreach (var x in decimals)
            {
                var deviation = Convert.ToDecimal(x) - mean;
                deviations.Add(deviation * deviation);
            }

            var meanDeviation = Convert.ToDouble(ArithmeticMean(deviations));
            return Convert.ToDecimal(Math.Sqrt(meanDeviation));
        }

        /// <summary>
        /// Calculate frequencies of numbers in specified bins.
        /// </summary>
        /// <param name="bins">Contigous set of bins covering the full range of supplied integers.</param>
        /// <param name="numbers">Used to calculate frequency.</param>
        /// <returns></returns>
        public static List<Bin> Frequency(List<Bin> bins, List<decimal> numbers)
        {
            var sortedBins = SortBins(bins);
            var sortedNumbers = SortNumbers(numbers);
            var bin = 0;
            var currentBin = sortedBins[bin++];

            foreach (var x in sortedNumbers)
            {
                // When current number goes over the upper value, move to the next bin.
                while (x > currentBin.UpperValue)
                {
                    if (bin >= sortedBins.Count)
                        return sortedBins;

                    currentBin = sortedBins[bin++];
                }

                if (x >= currentBin.LowerValue)
                    currentBin.Frequency++;

                // Note: If bin ranges are not contiguous some integers could be missed.
            }

            return sortedBins;
        }

        /// <summary>
        /// Sort bins in to order
        /// </summary>
        /// <returns></returns>
        private static List<Bin> SortBins(List<Bin> bins)
        {
            return bins.OrderBy(x => x.LowerValue).ToList();
        }

        /// <summary>
        /// Sort numbers into order
        /// </summary>
        /// <returns></returns>
        private static List<decimal> SortNumbers(List<decimal> numbers)
        {
            return numbers.OrderBy(x => x).ToList();
        }
    }

    /// <summary>
    /// Bin class used in frequency calculations.
    /// </summary>
    public class Bin
    {
        /// <summary>
        /// Lower value for this bin. If no lower value, includes all below upper value.
        /// </summary>
        public decimal LowerValue { get; set; }
        
        /// <summary>
        /// Upper value for this bin. If no upper value, includes all above lower value.
        /// </summary>
        public decimal UpperValue { get; set; }

        /// <summary>
        /// Count of item in this bin.
        /// </summary>
        public int Frequency { get; set; }
    }
}
