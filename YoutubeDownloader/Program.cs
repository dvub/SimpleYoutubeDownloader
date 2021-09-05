using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            AppSettings appSettings = AppSettings.IsCreated //check for config file
                ? AppSettings.Load()
                : AppSettings.Create();

            var youtube = new YoutubeClient();
            try
            {
                string choice = "";
                while (choice != "2")
                {
                    Console.WriteLine("Please enter URL");
                    string url = Console.ReadLine().Substring(32);
                    var video = await youtube.Videos.GetAsync(url);
                    Console.WriteLine($"Now converting: {video.Title} to mp3...");
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);
                    Console.WriteLine("Got stream manifest..");
                    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    Console.WriteLine("Got audio stream from manifest..");
                    string path = $@"{appSettings.path}{video.Title}.mp3";
                    if (File.Exists(path))
                    {
                        Console.WriteLine("Error: A file already exists with that name!");
                        return;
                    }
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, path);
                    Console.WriteLine("Finished downloading!");
                    Console.WriteLine("\nSelect an option:");
                    Console.WriteLine("1. Download another mp3");
                    Console.WriteLine("2. Exit");
                    choice = Console.ReadLine();
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e} \n Shutting Down..");
                return;
            }
        }

    }
}
