using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TimestampSlicer
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("[!] Not enough arguments. Usage: \n" +
                                  "  $ timestamp-splitter <input-file> <timestamp-file>\n" +
                                  "Exiting...");
                return;
            }

            
            // Try load the given timestamp file and exit if not found:
            IEnumerable<string> timestamps;
            try
            {
                timestamps = File.ReadLines(args[1]);
            }
            catch (IOException e)
            {
                Console.WriteLine($"[!] Cannot open file {args[1]}: {e}.\nExiting...");
                return;
            }
           
            
            // Generate a list of slices from the timestamps and exit if something went wrong:
            var slices = new Slicer().Slice(timestamps, "--");
            if (slices == null)
            {
                Console.WriteLine($"[!] No slices could be extracted. Exiting...");
                return;
            }

            
            var ffmpegStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                ErrorDialog = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            slices.ForEach(s =>
                {
                    // For each slice, we want to execute ffmpeg and extract from the music file
                    // a portion from the start to the end of the slice, and save output as a file with
                    // the name of the slice.
                    
                    Console.WriteLine($"[+] Executing ffmpeg -i \"{Regex.Escape(args[0])}\" {s.ToFfmpegArgs()}");
                    ffmpegStartInfo.Arguments = $"-i \"{args[0]}\" {s.ToFfmpegArgs()}";
                    var process = Process.Start(ffmpegStartInfo);
                    process?.WaitForExit();
                }
            );
        }
    }
}
