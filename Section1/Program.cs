using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section1
{
    public enum TokenType
    {
        Command, Not_Recognized, FullPathToDirectory, FileName,DirName, FullPathToFile
    }
    public struct Token
    {
        public TokenType key;
        public string value;
    }
    class Program
    {
        public static Directory current;
        public static string currentPath;
        static void Main(string[] args)
        {
           Virtual_Disk.initalize("virtualDisk");
            currentPath=new string(current.dir_name);
            currentPath = currentPath.Trim(new char[] { '\0', ' ' });
            while (true)
            {
                Console.Write(currentPath+ "\\" + ">>> ");
                current.readDirectory();
                string input;
                input = Console.ReadLine();
                Parser.parse(input);
            }
        }
    }
}
