using System;

namespace ShrapBruter{
    public class Interactive{
        public Interactive(){}

        String Question(String text, bool noempty = true, String mustcontain = ""){
            Messages.PrintColored(">>> ", "blue", end: "");
            Console.Write($"{text}: ");
            String answ = Console.ReadLine();
            if(noempty && answ.Length == 0){
                Console.WriteLine("Please specify value!");
                answ = Question(text, noempty: noempty);
            }
            if(mustcontain.Length > 0){
                if(!answ.Contains(mustcontain)){
                    Console.WriteLine($"Value must contain: '{mustcontain}'!");
                    answ = Question(text, noempty: noempty, mustcontain: mustcontain);
                }
            }
            return answ;
        }

        bool YesNo(String text){
            Messages.PrintColored(">>> ", "blue", end: "");
            Console.Write($"{text} [y/n]: ");
            String inpt = Console.ReadLine();
            bool answ;
            switch(inpt){
                case "y":
                    answ = true;
                break;
                case "n":
                    answ = false;
                break;
                default:
                    Console.WriteLine("Wrong answer!");
                    answ = YesNo(text);
                break;
            }
            return answ;
        }

        public Object[] GetArguments(){
            // PRINT BANNER HERE

            String url = Question("Target URL(with http://)", mustcontain: "http");
            String login = Question("Login");
            String passfile = Question("Passwd Dictionary File");
            bool verbose = YesNo("Verbose Mode?");

            String rawheaders = Question("Headers(/h for help with correct format)", mustcontain: ",");
            String[] headers = rawheaders.Split(",");

            Object[] args = new Object[] {login, passfile, verbose, headers, url};
            return args;
        }
        
    }
}