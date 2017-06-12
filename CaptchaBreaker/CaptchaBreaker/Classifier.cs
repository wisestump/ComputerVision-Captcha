using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaBreaker
{
    public class Classifier
    {
        private string proto;
        private string model;
        private string means;
        private string labels;
        private string classifierPath;

        public Classifier(string netFolder, string classifierFolder)
        {
            var netFolderInfo = new DirectoryInfo(netFolder);
            proto = netFolderInfo.GetFiles("*.prototxt")[0].FullName;
            model = netFolderInfo.GetFiles("*.caffemodel")[0].FullName;
            means = netFolderInfo.GetFiles("*.binaryproto")[0].FullName;
            labels = netFolder + "/" + "labels.txt";
            classifierPath = classifierFolder;
        }

        public string Classify(string picPath)
        {
            var process =
                Process.Start(
                    new ProcessStartInfo(classifierPath + "/classification.exe", $"\"{proto}\" \"{model}\" \"{means}\" \"{labels}\" \"{picPath}\"")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });

            var output = string.Format("{0}{1}", process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd());

            process.WaitForExit();

            return output;
        }
    }
}
