using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section1
{
    public class File_Entry : Directory_Entry
    {
        public string content;
        public Directory parent;
        public File_Entry(string name, byte dir_attr, int dir_firstCluster, Directory pa) : base(name, dir_attr, dir_firstCluster)
        {
            content = string.Empty;
            if (pa != null)
                parent = pa;
        }
        public Directory_Entry GetDirectory_Entry()
        {
            Directory_Entry me = new Directory_Entry(new string(this.dir_name), this.dir_attr, this.dir_firstCluster);
            return me;
        }
        public void writeFileContent()
        {
            byte[] contentBYTES = Converter.StringToBytes(content);
            List<byte[]> bytesls = Converter.splitBytes(contentBYTES);
            int clusterFATIndex;
            if (this.dir_firstCluster != 0)
            {
                clusterFATIndex = this.dir_firstCluster;
            }
            else
            {
                clusterFATIndex = Mini_FAT.getAvilableCluster();
                this.dir_firstCluster = clusterFATIndex;
            }
            int lastCluster = -1;
            for (int i = 0; i < bytesls.Count; i++)
            {
                if (clusterFATIndex != -1)
                {
                    Virtual_Disk.writeCluster(bytesls[i], clusterFATIndex, 0, bytesls[i].Length);
                    Mini_FAT.setClusterPointer(clusterFATIndex, -1);
                    if (lastCluster != -1)
                        Mini_FAT.setClusterPointer(lastCluster, clusterFATIndex);
                    lastCluster = clusterFATIndex;
                    clusterFATIndex = Mini_FAT.getAvilableCluster();
                }
            }
        }
        public void readFileContent()
        {
            if (this.dir_firstCluster != 0)
            {
                content = string.Empty;
                int cluster = this.dir_firstCluster;
                int next = Mini_FAT.getClusterPointer(cluster);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(Virtual_Disk.readCluster(cluster));
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_FAT.getClusterPointer(cluster);
                }
                while (next != -1);
                content = Converter.BytesToString(ls.ToArray());
            }
        }
        public void deleteFile()
        {
            if (this.dir_firstCluster != 0)
            {
                int cluster = this.dir_firstCluster;
                int next = Mini_FAT.getClusterPointer(cluster);
                do
                {
                    Mini_FAT.setClusterPointer(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_FAT.getClusterPointer(cluster);
                }
                while (cluster != -1);
            }
            if (this.parent != null)
            {
                int index = this.parent.searchDirectory(new string(this.dir_name));
                if (index != -1)
                {
                    this.parent.DirOrFiles.RemoveAt(index);
                    this.parent.writeDirectory();
                    Mini_FAT.writeFAT();
                }
            }
        }
    }
}
