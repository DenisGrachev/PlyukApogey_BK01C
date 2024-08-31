using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace tilesConv
{
    class Program
    {


        //       ;цвет 10XXGBXR
        //;       ||  |
        //;       ||   мигание
        //;       | негативное изображение
        //;       |
        //;        подчеркивание
        //g = 10000101b
        //r = 10001100b
        //b = 10001001b
        //w = 10000000b
        //c = 10000001b
        //m = 10001000b
        //n = 10001101b
        //y = 10000100b

      

        static void Main(string[] args)
        {



            //classic coder pallete apogeyz
            Color[] palette = { Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 248), Color.FromArgb(248, 0, 0), Color.FromArgb(248, 0, 248), Color.FromArgb(0, 248, 0), Color.FromArgb(0, 248, 248), Color.FromArgb(248, 248, 0), Color.FromArgb(248, 248, 248) };
            byte[] palByte = { 0b10001101, 0b10001001, 0b10001100, 0b10001000, 0b10000101, 0b10000001, 0b10000100, 0b10000000 };


            List<byte> outBytes = new List<byte>();


            int GetColorNum(Color color)
            {
                int outNum = 0;

                for (int i = 0; i < palette.Count(); i++)
                {
                    if (palette[i] == color)
                        return i;
                }

                return outNum;
            }


           // Bitmap bitmap = new Bitmap("tileset.png");
            Bitmap bitmap = new Bitmap(args[0]);
            int width = bitmap.Width;
            int height = bitmap.Height;

            //tiles 8x6
            int tilesX = width / 12;
            int tilesY = height / 8;


            //int x = 0;
            //int y = 0;

            //read one tile


            for (int y = 0; y < tilesY; y++)
                for (int x = 0; x < tilesX; x++)
                {

                    for (int yy = 0; yy < 4; yy++)
                    {
                        //find color in line, by default is black
                        //байт цвета на 2 линии пиксельных
                        int colorNum = 0;

                           
                            for (int xx = 0; xx < 12; xx++)
                            {
                                if (GetColorNum(bitmap.GetPixel(x * 12 + xx, y * 8 + yy*2)) != 0)
                                    colorNum = GetColorNum(bitmap.GetPixel(x * 12 + xx, y * 8 + yy*2));
                            if (GetColorNum(bitmap.GetPixel(x * 12 + xx, y * 8 + yy * 2 + 1)) != 0)
                                colorNum = GetColorNum(bitmap.GetPixel(x * 12 + xx, y * 8 + yy * 2 + 1));

                        }

                            //ok there is not pixels make a white attribute for better clashing
                            if (colorNum == 0)
                            {
                                colorNum = 7;
                            }

                            outBytes.Add(palByte[colorNum]);

                        for (int xx = 0; xx < 4; xx++)
                        {

                            byte outByte = 0;

                            //012
                            //345

                            //any color
                            Color pixel0 = bitmap.GetPixel(x * 12 + xx * 3 + 0, y * 8 + yy * 2 + 0);

                            if (pixel0.R > 0 || pixel0.G > 0 || pixel0.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 0));
                            }

                            Color pixel1 = bitmap.GetPixel(x * 12 + xx * 3 + 1, y * 8 + yy * 2 + 0);
                            if (pixel1.R > 0 || pixel1.G > 0 || pixel1.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 1));
                            }

                            Color pixel2 = bitmap.GetPixel(x * 12 + xx * 3 + 2, y * 8 + yy * 2 + 0);
                            if (pixel2.R > 0 || pixel2.G > 0 || pixel2.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 2));                                
                            }

                            Color pixel3 = bitmap.GetPixel(x * 12 + xx * 3 + 0, y * 8 + yy * 2 + 1);
                            if (pixel3.R > 0 || pixel3.G > 0 || pixel3.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 3));
                            }

                            Color pixel4 = bitmap.GetPixel(x * 12 + xx * 3 + 1, y * 8 + yy * 2 + 1);
                            if (pixel4.R > 0 || pixel4.G > 0 || pixel4.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 4));
                            }

                            Color pixel5 = bitmap.GetPixel(x * 12 + xx * 3 + 2, y * 8 + yy * 2 + 1);
                            if (pixel5.R > 0 || pixel5.G > 0 || pixel5.B > 0)
                            {
                                outByte = (byte)(outByte | (1 << 5));
                            }



                            //outByte = outByte | (1 << bitPosition);
                            /*
                            byte outByte = 0;

                            //any color
                            Color pixelL = bitmap.GetPixel(x * 8 + xx * 2 + 0, y * 5 + yy);
                            if (pixelL.R > 0 || pixelL.G > 0 || pixelL.B > 0)
                            {
                                outByte += 1;
                            }

                            Color pixelR = bitmap.GetPixel(x * 8 + xx * 2 + 1, y * 5 + yy);
                            if (pixelR.R > 0 || pixelR.G > 0 || pixelR.B > 0)
                            {
                                outByte += 2;
                            }
                            */

                            outBytes.Add(outByte);
                        }



                    }

                }


            //System.IO.File.WriteAllBytes("tileSet.png" + ".tls", outBytes.ToArray());
            System.IO.File.WriteAllBytes(args[0] + ".tls", outBytes.ToArray());

        }

    }
}
