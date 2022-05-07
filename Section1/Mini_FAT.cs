using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section1
{
    [Serializable]
    public static class Mini_FAT
    {
        public static int[] FAT = new int[1024];
        public static void createFAT()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                if (i == 0 || i == 4)
                {
                    FAT[i] = -1;
                }
                else if (i > 0 && i <= 3)
                {
                    FAT[i] = i + 1;
                }
                else
                {
                    FAT[i] = 0;
                }
            }
        }
        public static void writeFAT()
        {
            byte[] FATBYTES = Converter.ToBytes(Mini_FAT.FAT);
            List<byte[]> ls = Converter.splitBytes(FATBYTES);
            for (int i = 0; i < ls.Count; i++)
            {
               Virtual_Disk.writeCluster(ls[i], i + 1, 0, ls[i].Length);
            }
        }
        public static void readFAT()
        {
            List<byte> ls = new List<byte>();
            for(int i=1;i<=4;i++)
            {
                ls.AddRange(Virtual_Disk.readCluster(i));
            }
            FAT = Converter.ToInt(ls.ToArray());
        }
        public static void printFAT()
        {
            Console.WriteLine("FAT has the following: ");
            for (int i = 0; i < FAT.Length; i++)
                Console.WriteLine("FAT[" + i + "] = " + FAT[i]);
        }
        public static void setFAT(int[] arr)
        {
            if (arr.Length <= 1024)
                FAT = arr;
        }
        public static int getAvilableCluster()
        {
            for (int i = 0; i < FAT.Length; i++)
            {
                if (FAT[i] == 0)
                    return i;
            }
            return -1;//our disk is full
        }
        public static void setClusterPointer(int clusterIndex, int pointer)
        {
            FAT[clusterIndex] = pointer;
        }
        public static int getClusterPointer(int clusterIndex)
        {
            if (clusterIndex >= 0 && clusterIndex < FAT.Length)
                return FAT[clusterIndex];
            else
                return -1;
        }
    }
}
