### Timestamp-Slicer

Timestamp-Slicer is a wrapper around [ffmpeg][1] that slices audio files into slices that are 
defined by a file containing timestamps. 

This can be used to, for example, break a monolithic music album recording into separate songs.
In order to do this, you will need to feed the Timestamp-Slicer a file that contains information
about when each slice begins, like so:

    00:00:00 -- Song 1
    00:01:00 -- Song 2
    00:02:00 -- Song 3
    00:03:00 -- Song 4
    00:04:00 -- Song 5

Then, provided you have this file saved as `ts.txt` and input audio file like `input.mp3`, you can do: 

    $ dotnet Timestamp-Slicer.dll input.mp3 ts.txt

This will launch a ffmpeg instance for every slice the program extracts from `ts.txt` and place mp3 output as in the current
working directory (let it be called `out`):

    out/Song 1.mp3
	out/Song 2.mp3
    out/Song 3.mp3
    out/Song 4.mp3
    out/Song 5.mp3


[1]:https://github.com/FFmpeg/FFmpeg
