using System;
using System.Linq;

namespace ShrapBruter{
    public class Arguments{
        public static Object[] Args(String[] argss){

            String[] args = argss;

            string[] KNOWN = new string[] {"/u", "/l", "/p", "/v", "/headers"};
            string usage = "SharpBrute.exe /u URL /l LOGIN /p PASS_FILE [/v] [/headers HEADERS]";

            void HelpPage(){
                Console.WriteLine("Usage:\n" +
                                $"{usage}\n" +
                                "Arguments:\n" +
                                "\t/u\t\tForm URL address\n" +
                                "\t/l\t\tLogin for brute forcing.\n" +
                                "\t/p\t\tDictionary with possible passwords.\n" +
                                "\t/v\t\tVerbose mode of this program.\n" +
                                "\t/headers\tHeaders of POST requests for brute forcing.\n" +
                                "\t\t\t\tExample: --headers login,pass\n" +
                                "\t\t\t\tThis will use data headers: {login: ^LOGIN^, pass: ^PASS^}\n" +
                                "\t\t\t\tWhere ^LOGIN^ will be given login and ^PASS^ will be current pass from file.");
                Environment.Exit(0);
            }

            void UsagePage(){
                Console.WriteLine($"Usage: {usage}\n" +
                                "Use /h or /help for more help.");
                Environment.Exit(0);
            }
            
            if(args.Length > 0){
                if(args.Contains("/h") || args.Contains("/help")) HelpPage();

                String url = "", login = "", passfile = "";
                String[] headers = new String[] {"", ""};
                bool verbose = false;

                int i = 0;
                while(i < args.Length){
                    String arg = args[i];
                    if(!KNOWN.Contains(arg)) { Console.WriteLine("Unknown argumen: " + arg); Environment.Exit(0); }

                    try{
                        switch(arg){
                            case "/u":
                                url = args[i+1];
                                if(KNOWN.Contains(url)) throw new Exception();
                                if(!url.Contains("http")){
                                    Console.WriteLine($"'{url}' is not valid URL! (Don't forget about http/https)");
                                    Environment.Exit(0);
                                }
                                i += 2;
                            break;
                            case "/l":
                                login = args[i+1];
                                if(KNOWN.Contains(login)) throw new Exception();
                                i += 2;
                            break;
                            case "/p":
                                passfile = args[i+1];
                                if(KNOWN.Contains(passfile)) throw new Exception();
                                i += 2;
                            break;
                            case "/v":
                                verbose = true;
                                i++;
                            break;
                            case "/headers":
                                String subheaders = args[i+1];
                                if(KNOWN.Contains(subheaders)) throw new Exception();
                                headers = subheaders.Split(",");
                                i += 2;
                            break;
                            default:
                                Console.WriteLine("ERROR OCCURED!");
                                Environment.Exit(0);
                            break;
                        }
                    } catch(Exception){
                        // Console.WriteLine(e.Message);
                        Console.WriteLine("Please specify all values!");
                        UsagePage();
                    }
                }
                if(login == "" || passfile == "" || headers[0] == "" || headers[1] == "" || url == ""){
                    Console.WriteLine("Please specify all values!");
                    UsagePage();
                }
                // Console.WriteLine($"{login}, {passfile}, {verbose}, [{string.Join(", ", headers)}]"); 
                return new Object[] {login, passfile, verbose, headers, url};
            }
            else UsagePage();
            return new Object[] {null};
        }
    }
}