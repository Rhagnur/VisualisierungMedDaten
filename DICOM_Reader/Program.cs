using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    static class Program
    {

        static String baseDir = Application.StartupPath;
        static String FileWithPath;
        static Dictionary<string, byte[]> dicomMap = new Dictionary<string, byte[]>();

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            Console.WriteLine("Debug!!!!!!");
            baseDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
#endif
            FileWithPath = baseDir + "\\image\\CR-MONO1-10-chest.dcm";

            if (ReadDicom(FileWithPath))
            {
                Application.Run(new Form1(dicomMap));
            } else
            {
                Console.WriteLine("\nError: While trying to open '"+FileWithPath + "' No such file!");
                Console.WriteLine("Please press any key to exit!");
                Console.ReadKey();
            }
            

        }

        public static bool ReadDicom(string file)
        {
            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    BinaryReader reader = new BinaryReader(fs);

                    ushort g;
                    ushort e;
                    string tagID;
                    while (fs.Length > fs.Position)
                    {
                        g = reader.ReadUInt16();
                        e = reader.ReadUInt16();
                        tagID = g.ToString("X4") + e.ToString("X4");

                        long length;
                        length = reader.ReadUInt32();

                        byte[] val = reader.ReadBytes((int)length);
                        dicomMap.Add(tagID, val);

                        if (dicomMap.ContainsKey(tagID))
                        {
                            byte[] values = dicomMap[tagID];
                        }

                    }

                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
