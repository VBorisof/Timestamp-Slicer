using System;
using System.Text.RegularExpressions;

namespace TimestampSlicer
{
    public class Slice
    {
        public string Name { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan? End { get; set; }

        public string ToFfmpegArgs()
        {
            // The 'time offset' option - i.e. the start.
            var result = $"-ss {Start} ";
            if (End != null)
            {
                // The 'duration' option.
                result += $"-t {End-Start} ";
            }
            // Append the output file name.
            result += $"\"{Name}.mp3\"";
            return result;
        }
    }
}