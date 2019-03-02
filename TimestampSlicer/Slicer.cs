using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TimestampSlicer
{
    public class Slicer
    {
        public List<Slice> Slice(IEnumerable<string> timestampLines, string separator)
        {
            var slices = new List<Slice>();
            
            foreach (var timestampLine in timestampLines)
            {
                var split = timestampLine.Split(separator, 2)
                    .Select(s => s.Trim())
                    .ToList();
                
                var timestampString = Regex.Match(split.First(), "[0-9]{2}:[0-9]{2}:[0-9]{2}").Value;
                if (!TimeSpan.TryParse(timestampString, out var timestamp))
                {
                    Console.WriteLine($"[!] Invalid timestamp {timestampString}.");
                    return null;
                }
                var name = split.Last();
                Console.WriteLine($"{name} from {timestampString} ({timestamp})");

                var newSlice = new Slice
                {
                    Name = name,
                    Start = timestamp
                };
                slices.Add(newSlice);
                
                // If this is not the first slice, we can use this timestamp to know when previous slice ends:
                if (slices.IndexOf(newSlice) > 0) 
                {
                    slices.ElementAt(slices.IndexOf(newSlice) - 1).End = timestamp;
                }
            }

            return slices;
        }
    }
}