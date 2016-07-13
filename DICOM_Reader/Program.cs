using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

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

                    ushort subTag1, subTag2;
                    string tagID;
                    Console.WriteLine(fs.Length);
                    while (fs.Position < fs.Length)
                    {
                        subTag1 = reader.ReadUInt16();
                        subTag2 = reader.ReadUInt16();
                        tagID = subTag1.ToString("X4") + subTag2.ToString("X4");

                        long length;
                        length = reader.ReadUInt32();
                        

                        byte[] val = reader.ReadBytes((int)length);
                        dicomMap.Add(tagID, val);

                        //Debugausgabe, Pixeldata für eigentliches Bild wird aus offensichtlichen Gründen rausgelassen
                        if (length < 60)
                        {
                            Console.WriteLine(tagID + " : " + length + " - " + Encoding.Default.GetString(val));
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
