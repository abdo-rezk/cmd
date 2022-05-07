using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Section1
{
    public static class Tokenizer
    {
        static Token generateToken(string arg,TokenType tokenType)
        {
            Token token;
            token.key = tokenType;
            token.value = arg;
            return token;
        }
        static bool checkArg(string arg)
        {
            if(arg=="cd"|| arg == "cls" || arg == "dir" || arg == "quit" 
                || arg == "copy" || arg == "del" || arg == "help"
                || arg == "md" || arg == "rd" || arg == "rename"
                || arg == "type" || arg == "import" || arg == "export" )
            {
                return true;
            }
            return false;
        }
        static bool isFullPathd(string arg)
        {
            if ((arg.Contains(":") || arg.Contains("\\"))&& !arg.Contains('.'))
            {
                return true;
            }
            return false;
        }
        static bool isFullPathf(string arg)
        {
            if ((arg.Contains(":") || arg.Contains("\\")) && arg.Contains('.'))
            {
                return true;
            }
            return false;
        }
        static bool isFileName(string arg)
        {
            if (arg.Contains('.')/*&&!arg.Contains("..")*/)
            {
                return true;
            }
            return false;
        }
        public static List<Token> GetTokens(string input)
        {
            List<Token> Tokens = new List<Token>();

            if (input.Length == 0)
                return null;
            string[] inputs = input.Split(' ');
            List<string> ls = new List<string>();
            for (int i = 0; i < inputs.Length; i++)
                if (inputs[i] != "" && inputs[i] != " ")
                    ls.Add(inputs[i]);
            string[] arguments = ls.ToArray();
            arguments[0] = arguments[0].ToLower();
            switch(arguments[0])
            {
                case "cd":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else if (arguments.Length == 2)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        if (isFullPathd(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToDirectory));
                        else if(isFullPathf(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToFile));
                        else if(isFileName(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FileName));
                        else
                            Tokens.Add(generateToken(arguments[1], TokenType.DirName));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "cls":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    if(arguments.Length>1)
                    {
                        for(int i=1;i<arguments.Length;i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "dir":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else if (arguments.Length == 2)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        if (isFullPathd(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToDirectory));
                        else if (isFullPathf(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToFile));
                        else if (isFileName(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FileName));
                        else
                            Tokens.Add(generateToken(arguments[1], TokenType.DirName));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "quit":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "copy":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                case "del":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                case "help":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else if (arguments.Length == 2)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        arguments[1] = arguments[1].ToLower();
                        if(checkArg(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.Command));
                        else
                            Tokens.Add(generateToken(arguments[1], TokenType.Not_Recognized));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "md":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else if (arguments.Length == 2)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        if (isFullPathd(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToDirectory));
                        else if (isFullPathf(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToFile));
                        else if (isFileName(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FileName));
                        else
                            Tokens.Add(generateToken(arguments[1], TokenType.DirName));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "rd":
                    if (arguments.Length == 1)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    }
                    else if (arguments.Length == 2)
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        if (isFullPathd(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToDirectory));
                        else if (isFullPathf(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FullPathToFile));
                        else if (isFileName(arguments[1]))
                            Tokens.Add(generateToken(arguments[1], TokenType.FileName));
                        else
                            Tokens.Add(generateToken(arguments[1], TokenType.DirName));
                    }
                    else
                    {
                        Tokens.Add(generateToken(arguments[0], TokenType.Command));
                        for (int i = 1; i < arguments.Length; i++)
                        {
                            Tokens.Add(generateToken(arguments[i], TokenType.Not_Recognized));
                        }
                    }
                    break;
                case "rename":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                case "type":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                case "import":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                case "export":
                    Tokens.Add(generateToken(arguments[0], TokenType.Command));
                    break;
                default:
                    Tokens.Add(generateToken(arguments[0], TokenType.Not_Recognized));
                    break;

            }
            return Tokens;
        }
    }
}
