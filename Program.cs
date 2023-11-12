using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.Write("Enter the path to the MP4 file: ");
        string mp4Path = Console.ReadLine();

        if (File.Exists(mp4Path))
        {
            Console.WriteLine("Processing frames... Please be patient.");

            using (var process = new Process())
            {
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments = $"-i \"{mp4Path}\" -vf \"fps=30\" frame_%05d.png";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }

            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string outputFolder = downloadsPath + "\\Frames";

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string[] frameFiles = Directory.GetFiles(".", "frame_*.png");
            int frameCount = 1;

            foreach (string frameFile in frameFiles)
            {
                string destinationPath = Path.Combine(outputFolder, frameCount.ToString("D5") + ".png");
                File.Move(frameFile, destinationPath);
                frameCount++;
            }

            Console.WriteLine("Frames extracted and saved in the Downloads folder.");
        }
        else
        {
            Console.WriteLine("Invalid file path. Make sure the file exists and try again.");
        }
    }
}