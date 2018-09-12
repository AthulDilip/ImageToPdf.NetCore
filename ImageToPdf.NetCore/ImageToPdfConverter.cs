using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace ImageToPdf.NetCore
{
    public class ImageToPdfConvertor
    {
        private const string _wkhtmlExe = @"wkhtmltopdf.exe";
        
        /// <summary>
        /// Converts an Image to Html from its path
        /// </summary>
        /// <param name="imagePath">String containing image path</param>
        /// <returns>Pdf as byte array</returns>
        public static byte[] Convert(string imagePath, string switches="")
        {
            // Get the path
            string fullWkhtmlDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            fullWkhtmlDir = Path.Combine(fullWkhtmlDir, "Wkhtml");

            // Convert Image path to html
            string html = ImagePathToHtml(imagePath);

            // If switches are not given take default switches
            if (String.IsNullOrEmpty(switches))
            {
                try
                {
                    using (var image = new Bitmap(System.Drawing.Image.FromFile(imagePath)))
                    {
                        if (image != null)
                            switches = $"-T 0 -B 0 -L 0 -R 0 --page-width {image.Width}pt --page-height {image.Height}pt";
                    }
                }
                catch (Exception ex)
                {
                } 
            }

            // switches:
            //     "-q"  - silent output, only errors - no progress messages
            //     " -"  - switch output to stdout
            //     "- -" - switch input to stdin and output to stdout
            switches = $"-q {switches} - -";

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(fullWkhtmlDir,_wkhtmlExe),
                    Arguments = switches,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WorkingDirectory = fullWkhtmlDir,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            
            using (var sIn = proc.StandardInput)
            {
                sIn.WriteLine(html);
            }

            using (var ms = new MemoryStream())
            {
                using (var sOut = proc.StandardOutput.BaseStream)
                {
                    byte[] buffer = new byte[4096];
                    int read;

                    while ((read = sOut.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                string error = proc.StandardError.ReadToEnd();

                if (ms.Length == 0)
                {
                    throw new Exception(error);
                }

                proc.WaitForExit();

                return ms.ToArray();
            }
        }

        private static string ImagePathToHtml(string imagePath)
        {
            return $@"
                    <HTML>
                        <style>
                        body {{
                            background-position: top;
	                        background-repeat: no-repeat;
	                        background-size: contain;
                        }}
                        </style>
                        <body background='{imagePath}'></body>
                    </HTML>";
                    
        }
    }
}
