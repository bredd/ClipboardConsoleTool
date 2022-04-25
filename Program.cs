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
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var dto = Clipboard.GetDataObject();
                foreach (var format in dto.GetFormats())
                {
                    Console.WriteLine(format);
                }
                Console.WriteLine();

                var stream = dto.GetData("image/svg+xml") as Stream;
                if (stream != null)
                {
                    using (var dst = new FileStream(@"C:\users\brand\downloads\clipboard.svg", FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        stream.CopyTo(dst);
                    }
                    Console.WriteLine("Copied SVG clipboard to downloads\\clipboard.svg");
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
