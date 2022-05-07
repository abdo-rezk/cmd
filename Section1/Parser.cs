using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Section1
{
    public static class Parser
    {
        public static void parse(string input)
        {
            List<Token> tokens = Tokenizer.GetTokens(input);
            if (tokens != null)
            {
                if (tokens[0].key == TokenType.Not_Recognized)
                    Console.WriteLine(tokens[0].value + " is not recognized as an internal or external command,operable program or batch file.");
                else
                {
                    if (tokens[0].key == TokenType.Command)
                    {
                        switch (tokens[0].value)
                        {
                            case "cd":
                                Commands.changeDirectory(tokens);
                                break;
                            case "cls":
                                Commands.clear(tokens);
                                break;
                            case "dir":
                                Commands.listDirectory(tokens);
                                break;
                            case "quit":
                                Commands.quit(tokens);
                                break;
                            case "copy":
                                break;
                            case "del":
                                break;
                            case "help":
                                Commands.help(tokens);
                                break;
                            case "md":
                                Commands.createDirectory(tokens);
                                break;
                            case "rd":
                                Commands.removeDirectory(tokens);
                                break;
                            case "rename":
                                break;
                            case "type":
                                break;
                            case "import":
                                break;
                            case "export":
                                break;
                        }
                    }
                }
            }
        }

    }
}
