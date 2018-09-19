using System;

namespace VideoCutterApp
{
    using System.Diagnostics;

    public class VideoEditor
    {
      
        public static void CutVideo (string sourceFolder,string startTime,string endTime,string desinationFolder)
        {
            string command = @"C:\ffmpeg\bin\ffmpeg.exe -i " + sourceFolder + " -ss " + startTime + " -t " + endTime + " " + desinationFolder;
            Process cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            try
            {
                cmd.Start();
                cmd.StandardInput.WriteLine(command);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
