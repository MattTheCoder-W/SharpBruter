using System;

namespace ShrapBruter{
    public class Messages{
        public static void Message(String text, String end = "\n", String type = "info"){
            String prefix = "";
            ConsoleColor prefix_col = ConsoleColor.Gray;
            switch(type){
                case "info":
                    prefix = ">>>";
                    prefix_col = ConsoleColor.Green;
                    
                break;
                case "error":
                    prefix = "!!!";
                    prefix_col = ConsoleColor.Red;
                break;
                default:
                    prefix = ">>>";
                    prefix_col = ConsoleColor.Gray;
                break;
            }
            Console.ForegroundColor = prefix_col;
            Console.Write($"{prefix} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{text}" + end);
        }

        public static void PrintColored(String text, String color = "gray", String end = "\n"){
            ConsoleColor col = ConsoleColor.Gray;
            switch(color){
                case "green":
                    col = ConsoleColor.Green;
                break;
                case "yellow":
                    col = ConsoleColor.Yellow;
                break;
                case "red":
                    col = ConsoleColor.Red;
                break;
                case "blue":
                    col = ConsoleColor.Blue;
                break;
                case "white":
                    col = ConsoleColor.White;
                break;
            }

            Console.ForegroundColor = col;
            Console.Write($"{text}" + end);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}