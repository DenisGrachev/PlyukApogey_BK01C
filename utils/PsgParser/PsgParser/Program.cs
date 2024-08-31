using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PsgParser
{
    class Program
    {
        static void Main(string[] args)
        {

            //    Console.WriteLine("FFFFF");

            bool IsBitSet(byte b, int pos)
            {
                return (b & (1 << pos)) != 0;
            }

            byte[] AYregs = new byte[16];

            List<byte> outBytes = new List<byte>();

            bool chEnabled = false;

            void PushFrame()
            {
                //читаем частоту канала А
                 int freq = AYregs[1] * 256 + AYregs[0];
                //float freqFloat = 16.25f * freq;//16.0f * freq * 0.8571428571428571f;                
                //float freqFloat = 16.0f * freq * 0.8571428571428571f;                
                float freqFloat = 16.25f * freq;
                byte lowByte  =  (byte)(Math.Round(freqFloat) % 256);
                 byte highByte =  (byte)(Math.Round(freqFloat) / 256);

              //  byte lowByte = AYregs[0];
              //  byte highByte = AYregs[1];


                //string str = " R0:" + AYregs[0] + "  R1:" + AYregs[1] + "  R7:" + AYregs[7] +"  R8:" + AYregs[8];
                //Console.WriteLine(str);
              //  Console.Write(AYregs[8]+" ");
                                                                           

                if (!chEnabled)
                {
                   // if (AYregs[8] != 0 && !IsBitSet(AYregs[7], 0))
                //    if (!IsBitSet(AYregs[7], 0))
                      if (AYregs[8] !=0)
                    {
                        //включить канал
                        //   outBytes.Add(0xfd); outBytes.Add(0xff);
                        chEnabled = true;
                    }
                }
                else
                {
                 //   if (AYregs[8] == 0 && IsBitSet(AYregs[7], 0))
                //        if (IsBitSet(AYregs[7], 0))
                        if (AYregs[8] == 0)
                        {
                            //вылючить канал
                            //   outBytes.Add(0xfe); outBytes.Add(0xff);
                            chEnabled = false;
                        }
                }





                if (chEnabled)
                {
                    //частота
                    //outBytes.Add(lowByte);
                    //outBytes.Add(highByte);

                    //частота с заменой порядка байт, так команды удобно класть
                    outBytes.Add(highByte);
                    outBytes.Add(lowByte);
                }
                else
                {
                    //пустой фрейм
                    // outBytes.Add(0xff); outBytes.Add(0xff);
                    outBytes.Add(0xff);
                }


            }

            // Open the file to read from.
            //byte[] inPSG = File.ReadAllBytes("music.psg");
            byte[] inPSG = File.ReadAllBytes(args[0]);

            int counter = 17;
            
            //clear ay regs
            Array.Clear(AYregs, 0, 16);


            while (counter < inPSG.Length)
            {
                byte psgByte = inPSG[counter];


                switch (psgByte)
                {
                    //ff - end of interrupt
                    case 0xff:
                        PushFrame();
                        counter++;
                    //    Console.WriteLine("FF");
                   //     Console.ReadKey();
                        break;

                    //fe - end of interrupt N times
                    case 0xfe:
                        
                        counter++;
                        int frames = inPSG[counter];

                    //    Console.WriteLine("FE "+frames.ToString());
                    //    Console.ReadKey();
                        for (int i = 0; i < frames*4; i++)
                        {
                            PushFrame();
                        }
                        counter++;
                        break;

                    //fd end of music
                    case 0xfd:
                        counter++;
                        //Console.WriteLine("END OF MUSIC");
                        //psgByte = inPSG[counter];
                        //Console.WriteLine(psgByte);

                        break;

                    //collect ay data by defualt
                    default:
                        int regNumber = inPSG[counter];
                        counter++;
                        byte regValue = inPSG[counter];
                        AYregs[regNumber] = regValue;                      
                        counter++;
                        break;

                }
            }



            // Console.ReadKey();


            //точка лупа в
            //            outBytes.Add(0xfe); outBytes.Add(0xff);
            outBytes.Add(0xfd);
            File.WriteAllBytes(args[0]+".tiny", outBytes.ToArray());

            //финальная пост обработка, заменяем последовательности FF на FE + сколько кадров FF

            List<byte> outBytesRLE = new List<byte>();

            byte ffCounter = 0;

            foreach (byte b in outBytes)
            {
                if (b == 0xff)
                {
                    ffCounter++;
                }
                else
                {

                    if (ffCounter > 0)
                    {
                        if (ffCounter > 1)
                        {
                            outBytesRLE.Add(0xfe);
                            outBytesRLE.Add(ffCounter);
                            ffCounter = 0;
                        }
                        else
                        {
                            outBytesRLE.Add(0xff);
                            ffCounter = 0;
                        }
                    }                    
                        outBytesRLE.Add(b);
                 }


            }

            File.WriteAllBytes(args[0] + ".tinyRLE", outBytesRLE.ToArray());


        }


        
    }
}
