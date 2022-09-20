using System.Collections.Generic;
using System.Linq;

namespace DataverseClientTools.Extensions
{
    public static class ListExtension
    {
        public static IEnumerable<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            var listOfData = source.ToList();
            while (listOfData.Any())
            {
                yield return listOfData.Take(chunkSize).ToList();
                listOfData = listOfData.Skip(chunkSize).ToList();
            }
        }
    }
}