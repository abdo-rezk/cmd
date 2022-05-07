using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section1
{
    public class Directory : Directory_Entry
    {
        public List<Directory_Entry> DirOrFiles;
        public Directory parent;
        public Directory(string name, byte dir_attr, int dir_firstCluster, Directory pa) : base(name, dir_attr, dir_firstCluster)
        {
            DirOrFiles = new List<Directory_Entry>();
        
            if (pa != null)
            {
                parent = pa;
            }

        }
        public void updateContent(Directory_Entry d)
        {
            int index = searchDirectory(new string(d.dir_name));
            if (index != -1)
            {
                DirOrFiles.RemoveAt(index);
                DirOrFiles.Insert(index, d);
            }
        }
        public Directory_Entry GetDirectory_Entry()
        {
            Directory_Entry me = new Directory_Entry(new string(this.dir_name), this.dir_attr, this.dir_firstCluster);
            return me;
        }
        public void writeDirectory()
        {
            byte[] dirsorfilesBYTES = new byte[DirOrFiles.Count * 32];
            for (int i = 0; i < DirOrFiles.Count; i++)
            {
                byte[] b = Converter.Directory_EntryToBytes(this.DirOrFiles[i]);
                for (int j = i * 32, k = 0; k < b.Length; k++, j++)
                    dirsorfilesBYTES[j] = b[k];
            }
            List<byte[]> bytesls = Converter.splitBytes(dirsorfilesBYTES);
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
                if (clusterFATIndex != -1)//disk has space
                {
                    Virtual_Disk.writeCluster(bytesls[i], clusterFATIndex, 0, bytesls[i].Length);
                    Mini_FAT.setClusterPointer(clusterFATIndex, -1);//temp
                    if (lastCluster != -1)//
                        Mini_FAT.setClusterPointer(lastCluster, clusterFATIndex);
                    lastCluster = clusterFATIndex;
                    clusterFATIndex = Mini_FAT.getAvilableCluster();
                }
            }
            if (this.parent != null)
            {
                this.parent.updateContent(this.GetDirectory_Entry());//since fc changes from 0 to the used fc for example
                this.parent.writeDirectory();
            }
            Mini_FAT.writeFAT();
        }
        public void readDirectory()
        {
            if (this.dir_firstCluster != 0)
            {
                DirOrFiles = new List<Directory_Entry>();
                int cluster = this.dir_firstCluster;
                int next = Mini_FAT.getClusterPointer(cluster);
                List<byte> ls = new List<byte>();//1024*count/32 bytes
                do
                {
                    ls.AddRange(Virtual_Disk.readCluster(cluster));
                    cluster = next;
                    if (cluster != -1)// not last
                        next = Mini_FAT.getClusterPointer(cluster);
                }
                while (next != -1);
                for (int i = 0; i < ls.Count; i++)
                {
                    byte[] b = new byte[32];
                    for (int k = i * 32, m = 0; m < b.Length && k < ls.Count; m++, k++)
                    {
                        b[m] = ls[k];
                    }
                    if (b[0] == 0)
                        break;
                    DirOrFiles.Add(Converter.BytesToDirectory_Entry(b));
                }
            }
        }
        public void deleteDirectory()
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
                    //Mini_FAT.writeFAT();
                }
            }
            if (Program.current == this)
            {
                if (this.parent != null)
                {
                    Program.current = this.parent;
                    Program.currentPath = Program.currentPath.Substring(0, Program.currentPath.LastIndexOf('\\'));
                    Program.current.readDirectory();
                }
            }
            Mini_FAT.writeFAT();
        }
        public int searchDirectory(string name)
        {
            if (name.Length < 11)
            {
                name += "\0";
                for (int i = name.Length + 1; i < 12; i++)
                    name += " ";
            }
            else
            {
                name = name.Substring(0, 11);
            }
            for (int i = 0; i < DirOrFiles.Count; i++)
            {
                string n = new string(DirOrFiles[i].dir_name);
                //int l = n.Length;
                //int ln = name.Length;
                if (n.Equals(name))
                    return i;
            }
            return -1;
        }
    }
}
