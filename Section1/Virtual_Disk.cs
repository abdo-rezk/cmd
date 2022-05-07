using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Section1
{
    public static class Virtual_Disk
    {
      
        public static FileStream Disk;
        public static void CREATEorOPEN_Disk(string path)
        {
            Disk = new FileStream(path, FileMode.OpenOrCreate,FileAccess.ReadWrite);
        }
        public static int getFreeSpace()
        {
            return (1024 * 1024) - (int)Disk.Length;
        }
        public static void initalize(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    CREATEorOPEN_Disk(path);
                    byte[] b = new byte[1024];
                    for (int i = 0; i < b.Length; i++)
                        b[i] = 0;
                    writeCluster(b, 0);
                    Mini_FAT.createFAT();
                    Directory root = new Directory("Root:", 0x10, 5,null);
                       root.writeDirectory();
                    Mini_FAT.setClusterPointer(5, -1);
                    Program.current = root;
                    Mini_FAT.writeFAT();
                }
                else
                {
                    CREATEorOPEN_Disk(path);
                    Mini_FAT.readFAT();
                    Directory root = new Directory("Root:", 0x10, 5,null);
                    root.readDirectory();
                    Program.current = root;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void writeCluster(byte[] cluster, int clusterIndex, int offset=0, int count=1024)
        {
            Disk.Seek(clusterIndex * 1024, SeekOrigin.Begin);
            Disk.Write(cluster, offset, count);
            Disk.Flush();
        }
        public static byte[] readCluster(int clusterIndex)
        {
            Disk.Seek(clusterIndex * 1024, SeekOrigin.Begin);
            byte[] bytes = new byte[1024];
            Disk.Read(bytes, 0, 1024);
            return bytes;
        }
    }
}
