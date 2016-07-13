using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    class DicomService
    {

        //only for this purpose...with 16 bits allocted
        public Bitmap createPicture(int rows, int columns, byte[] pixelData, int cValue, int wValue)
        {
            Bitmap picture = new Bitmap(columns, rows);
            int counter = 0;
            int colorValue;
            Color color;

            double min = cValue - 0.5 - ((wValue -1 ) / 2);
            double max = cValue + ((wValue - 1) / 2);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    //get the color value from the byte array and convert it into integer
                    colorValue = BitConverter.ToInt16(pixelData, counter);
  
                    //check if maybe one color value is out of range and adjust it
                    if (colorValue <= min) {
                        colorValue = 0;
                    }
                    else if (colorValue > max) {
                        colorValue = 255;
                    }
                    else {
                        colorValue = (int)((colorValue - min) * (255 / (max - min)));
                    }

                    //Switch color black to white
                    colorValue = 255 - colorValue;

                    //one last test, had strange behaviour with overfloating numbers
                    if (colorValue > 255) {
                        colorValue = 255;
                    }
                    else if (colorValue < 0)
                    {
                        colorValue = 0;
                    }

                    color = Color.FromArgb(colorValue, colorValue, colorValue);
                    
                    picture.SetPixel(x, y, color);

                    counter = counter + 2;
                }
            }
            return picture;
        }
    }
}
