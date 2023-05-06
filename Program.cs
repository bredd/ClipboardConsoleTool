using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace ClipboardTool
{

    class Program
    {
        const string cTypeSvg = "image/svg+xml";

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                bool hasSvg = false;
                bool hasHtml = false;

                var dto = Clipboard.GetDataObject();
                foreach (var format in dto.GetFormats())
                {
                    Console.WriteLine(format);
                    if (format.Equals(cTypeSvg, StringComparison.OrdinalIgnoreCase))
                        hasSvg = true;
                    if (format.Equals(DataFormats.Html))
                        hasHtml = true;
                }
                Console.WriteLine();

                if (hasSvg)
                {
                    var stream = dto.GetData(cTypeSvg) as Stream;
                    if (stream != null)
                    {
                        using (var dst = new FileStream(@"C:\users\brand\downloads\clipboard.svg", FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            stream.CopyTo(dst);
                        }
                        Console.WriteLine("Copied SVG clipboard to downloads\\clipboard.svg");
                    }
                }

                if (hasHtml)
                {
                    String str = dto.GetData(DataFormats.Html, true) as String;
                    if (str != null)
                    {
                        int start = str.IndexOf("<!--StartFragment");
                        start = str.IndexOf("-->", start + 17) + 3;
                        int end = str.IndexOf("<!--EndFragment");
                        using (var dst = new StreamWriter(@"C:\users\brand\downloads\clipboard.html", false, new UTF8Encoding(false)))
                        {
                            dst.Write(str.Substring(start, end-start));
                        }
                        Console.WriteLine("Copied HTML clipboard to downloads\\clipboard.html");
                    }
                }
            }
            catch (Exception err)
            {
#if DEBUG
                Console.WriteLine(err.ToString());
#else
                Console.WriteLine(err.Message);
#endif
            }

#if DEBUG
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
#endif
        }
    }
}
