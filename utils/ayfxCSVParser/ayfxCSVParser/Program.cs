using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ayfxCSVParser
{
    class Program
    {
        static void Main(string[] args)
        {
          //struct TONE-enabled, NOISE-enable,TONEVALUE,NOISE VOLUME,VOLUME
          List<byte> notes  = new List<byte>();

            //enable channel
            notes.Add(0xfd); notes.Add(0xff);

            // args[0] = "sfx0.csv";

            bool wasMute = false;

            // Open the file to read from.
            string[] readCSV = File.ReadAllLines(args[0]);
          foreach (string s in readCSV)
            {
                int tone = Convert.ToInt32(s.Split(',')[2],16);


                switch (tone)
                {
                    case 0:
                        //mute channel
                        notes.Add(0xfe); notes.Add(0xff);
                        wasMute = true;
                    break;
                    default:

                    if (wasMute)
                        {
                            //enable channel
                            notes.Add(0xfd); notes.Add(0xff);
                            wasMute = false;
                        }

                        //conver from AY tone to VI                    
                        // float toneFloat = 16.0f * tone * 0.8571428571428571f;
                         float toneFloat = 16.25f * tone;

                        notes.Add((byte)(Math.Round(toneFloat) % 256));
                    notes.Add((byte)(Math.Round(toneFloat) / 256));

                    break;
                }

                
                //Console.WriteLine(Math.Round(toneFloat));
            }

            //mute channel
            notes.Add(0xfe); notes.Add(0xff);
            notes.Add(0xff); notes.Add(0xff);

            File.WriteAllBytes(args[0] + ".sfx", notes.ToArray());



        }
    }
}
