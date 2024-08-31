using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace sprConv
{
    class Program
    {
        static void Main(string[] args)
        {



            //Bitmap bitmap = new Bitmap("font.png");
            Bitmap bitmap = new Bitmap(args[0]);
            int width = bitmap.Width;
            int height = bitmap.Height;

            int tilesX = width / 6;
            int tilesY = height / 8;

            List<byte> outBytes = new List<byte>();

            for (int y = 0; y < tilesY; y++)

            { 
                for (int x = 0; x < tilesX; x++)
                {

                    for (int yy = 0; yy < 4; yy++)
                    {
                        for (int xx = 0; xx < 2; xx++)
                        {
                            byte outByte = 0;

                            //012
                            //345

                            //any color
                            Color pixel0 = bitmap.GetPixel(x * 6 + xx * 3 + 0, y*8 + yy * 2 + 0);

                            if (pixel0.R > 0 || pixel0.G > 0 || pixel0.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 0));
                            }

                            Color pixel1 = bitmap.GetPixel(x * 6 + xx * 3 + 1, y * 8 + yy * 2 + 0);
                            if (pixel1.R > 0 || pixel1.G > 0 || pixel1.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 1));
                            }

                            Color pixel2 = bitmap.GetPixel(x * 6 + xx * 3 + 2, y * 8 + yy * 2 + 0);
                            if (pixel2.R > 0 || pixel2.G > 0 || pixel2.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 2));
                            }

                            Color pixel3 = bitmap.GetPixel(x * 6 + xx * 3 + 0, y * 8 + yy * 2 + 1);
                            if (pixel3.R > 0 || pixel3.G > 0 || pixel3.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 3));
                            }

                            Color pixel4 = bitmap.GetPixel(x * 6 + xx * 3 + 1, y * 8 + yy * 2 + 1);
                            if (pixel4.R > 0 || pixel4.G > 0 || pixel4.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 4));
                            }

                            Color pixel5 = bitmap.GetPixel(x * 6 + xx * 3 + 2, y * 8 + yy * 2 + 1);
                            if (pixel5.R > 0 || pixel5.G > 0 || pixel5.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 5));
                            }


                            outBytes.Add(outByte);

                        }
                    }
                }

            }
            System.IO.File.WriteAllBytes(args[0] + ".spr", outBytes.ToArray());

        }
    }
}
