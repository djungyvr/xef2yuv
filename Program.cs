using System;
using System.IO;
using Microsoft.Kinect;
using Microsoft.Kinect.Tools;

namespace xef2yuv
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("ERROR: Missing filename");
                return;
            }
            if (args.Length < 2)
            {
                Console.WriteLine("ERROR: Missing output directory");
                return;
            }
            string fileName = args[0];
            string outputDir = args[1];
     
            var outputDirPath = Environment.CurrentDirectory + "/" + outputDir;
            if(Directory.Exists(outputDirPath))
            {
                Directory.Delete(outputDirPath, true);
            }
            Directory.CreateDirectory(outputDirPath);
            Console.WriteLine(fileName);
            KStudioClient client = KStudio.CreateClient();
            KStudioEventFile file = client.OpenEventFile(fileName);
            foreach (KStudioSeekableEventStream stream in file.EventStreams)
            {
                if (stream.DataTypeName.Equals("Nui Uncompressed Color"))
                {
                    int width = 1920;
                    int height = 1080;
                    byte[] bufferYuv = new byte[width * height * 2];
                    uint length = stream.EventCount;
                    for (uint i = 0; i < length; i++)
                    {
                        var currentEvent = stream.ReadEvent(i);
                        currentEvent.CopyEventDataToArray(bufferYuv, 0);
                        string filePath = outputDirPath + "/" + i.ToString("D4");
                        File.WriteAllBytes(filePath, bufferYuv);
                    }
                }
            }
        }
    }
}
