using System;
using System.IO;

namespace Revida.Sitecore.Assurance.Console
{
    public abstract class RunnerBase
    {
        protected FileStream CreateOutputFile(string fileName)
        {
            string outputFolder = GetOutputFolder();

            DateTime now = DateTime.Now;
            string timeStamp = $"{now.Year}{now.Month.ToString("00")}{now.Day.ToString("00")}{now.Hour.ToString("00")}{now.Minute.ToString("00")}{now.Second.ToString("00")}";

            string fileNameForCurrentRun = $"{outputFolder}\\{fileName}-{timeStamp}.csv";

            FileStream outputFile = File.Create(fileNameForCurrentRun);

            return outputFile;
        }

        private static string GetOutputFolder()
        {
            string outputFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Output";

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            return outputFolder;
        }
    }
}
