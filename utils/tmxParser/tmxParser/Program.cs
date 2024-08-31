using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace tmxParser
{
    public class Program
    {
        public static void AppendAllBytes(string path, byte[] bytes)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void AppendByte(string path, byte byte_)
        {
            //argument-checking here.

            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.WriteByte(byte_);
            }
        }


        static void Main(string[] args)
        {

            const int TILE_KEY = 28;
            const int TILE_DOOR = 26;
            const int TILE_HERO_START = 34;
            const int TILE_FLOOR_DOOR = 42;

            //4 keys max 2 bytes per ket at end 1 byte total keys
            byte[] keyBytes = new byte[8+1];
            byte totalKeys = 0;

            //за экраном ключи по дефолту у=127
            keyBytes[1] = 127;
            keyBytes[3] = 127;
            keyBytes[5] = 127;
            keyBytes[7] = 127;

            //HERO
            // xy-cell xy-pixel
            byte[] heroBytes = new byte[4];

            //x,y
            byte[] doorBytes = new byte[2];

            //x,y,xPixel,yPixel - MAX 4 doors
            byte[] floorDoorsBytes = new byte[16];
            byte totalFloorDoors = 0;

            //yx - 2bytes
            //1 -byte direction 1-left 2-right
            //GUNS
            //byte[] gunz = new byte[9 * 3];
            //int gunsCount = 0;

            //our tilemap
            byte[] tileMap = new byte[16 * 24];

            //collisions table
          //  byte[] collisions = new byte[256];

    
           // args[0] = "map00.tmx";
                  
            Console.WriteLine("TMX Parser by Denis Grachev");


            TmxMap map = new TmxMap(args[0]);
        //  TmxMap map = new TmxMap("map08.tmx");

            Console.WriteLine("Build collision table");

     

            Console.WriteLine("===========================");


            Console.WriteLine("Parsing tilemap!");

            //int TILE_KEY = 105;            
            
            //import tiles from level layer
            for (byte j = 0; j < 24; j++)
            {
                for (byte i = 0; i < 16; i++)
                {
                    int tile = (map.TileLayers["level"].Tiles[i + j * 16].Gid);

                    switch (tile-1)
                    {

                        case TILE_HERO_START:
                            //put coords after map
                            // xy-cell xy-pixel
                            heroBytes[0] = i;
                            heroBytes[1] = j;
                            heroBytes[2] = (byte)(i*12);
                            heroBytes[3] = (byte)(j*8);
                            //reset in tile map
                          //  tile = 1;
                            break;
                        case TILE_KEY:
                            //put coords after map
                            // xy-cell xy-pixel
                            keyBytes[0 + totalKeys*2] = i;
                            keyBytes[1 + totalKeys * 2] = j;
                            totalKeys++;
                            keyBytes[8] = totalKeys;
                            break;

                        case TILE_DOOR:
                            //put coords after map
                            // xy-cell xy-pixel
                            doorBytes[0] = i;
                            doorBytes[1] = j;
                            break;
                    

                        case TILE_FLOOR_DOOR:
                            //put coords after map
                            // xy-cell xy-pixel
                            floorDoorsBytes[0 + totalFloorDoors * 4] = i;
                            floorDoorsBytes[1 + totalFloorDoors * 4] = j;
                            floorDoorsBytes[2 + totalFloorDoors * 4] = (byte)(i*12);
                            floorDoorsBytes[3 + totalFloorDoors * 4] = (byte)(j*8);
                            totalFloorDoors++;                                                        
                            break;

                        default:
                            break;
                    }

                    tileMap[i + j * 16] = (byte)((tile - 1) * 2);

                }
            }

            /*
            //objects layer
            for (byte j = 0; j < 18; j++)
            {
                for (byte i = 0; i < 16; i++)
                {
                    int tile = (map.TileLayers["objects"].Tiles[i + j * 16].Gid);


                    switch (tile - 1)
                    {

                        default:
                            break;
                    }

                    //tileMap[i + j * 16] = (byte)((tile - 1) * 2);

                }
            }
            */

            //File.WriteAllBytes(args[0] + ".coins", coins);
            //File.WriteAllBytes(args[0] + ".enemies", enemies);
            File.WriteAllBytes(args[0] + ".mapa", tileMap);
            AppendAllBytes(args[0] + ".mapa", heroBytes); // +4 bytes
            AppendAllBytes(args[0] + ".mapa", keyBytes); //  +9 bytes
            AppendAllBytes(args[0] + ".mapa", doorBytes); //  +2 bytes
            AppendAllBytes(args[0] + ".mapa", floorDoorsBytes); //  +16 bytes

            // File.WriteAllBytes(args[0] + ".doors", exits);

            //  File.WriteAllBytes(args[0] + ".mapa", coins);
            //  AppendAllBytes(args[0] + ".mapa", enemies);
            //  AppendAllBytes(args[0] + ".mapa", exits);            
            //  AppendAllBytes(args[0] + ".mapa", gunz);
            //  AppendByte(args[0] + ".mapa", totalDots);
            //  AppendAllBytes(args[0] + ".mapa", tileMap);
            ///  AppendAllBytes(args[0] + ".mapa", heroes);


            //  File.WriteAllBytes("tiles.collisions", collisions);


            //  File.WriteAllBytes("gunz.gunz", gunz);

            Console.WriteLine();

           
            

            
            //===================================================

            //                Console.ReadKey();

        }
            

        }
}
