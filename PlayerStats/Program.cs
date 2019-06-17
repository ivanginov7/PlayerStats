using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStats
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<Player> players;
            string fullPath /*= @"D:\vssolutions\PlayerStats\PlayerStats\playerdata.json"*/;
            byte maxNumberOfYearsPlayed /*= 10*/;
            byte minimumRating /*= 55*/;
            string pathToOutputFile /*= @"D:\vssolutions\PlayerStats\PlayerStats\output.txt"*/;
            ushort filteredYear /*= (ushort) DateTime.Now.AddYears(-maxNumberOfYearsPlayed).Year*/;
            try
            {
                //check number of commandline arguments
                SanitizeInput(args, out fullPath, out maxNumberOfYearsPlayed, out minimumRating, out pathToOutputFile, out filteredYear);
                players = AccessData(fullPath, minimumRating, filteredYear);
                PrintToFile(players, pathToOutputFile);

                //else { throw new Exception("not enough commandline arguments provided"); }
            }
            catch (Exception e)
            {
                Console.WriteLine("Uh-oh! " + e.Message);
            }
            Console.ReadLine();
        }

        private static void PrintToFile(List<Player> players, string pathToOutputFile)
        {
            using (StreamWriter w = new StreamWriter(pathToOutputFile))
            {

                w.WriteLine("Name, Rating");
                foreach (Player player in players)
                {
                    Console.WriteLine(player.Name + " " + player.Rating);
                    w.WriteLine("{0}, {1}", player.Name, player.Rating);
                }
            }
        }

        private static List<Player> AccessData(string fullPath, byte minimumRating, ushort filteredYear)
        {
            List<Player> players;
            using (StreamReader r = new StreamReader(fullPath))
            {
                string jsonData = r.ReadToEnd();
                players = JsonConvert.DeserializeObject<List<Player>>(jsonData);
                players = players.Where(p => p.Rating >= minimumRating && p.PlayingSince >= filteredYear).OrderByDescending(p => p.Rating).ToList<Player>();
            }

            return players;
        }

        private static void SanitizeInput(string[] args, out string fullPath, out byte maxNumberOfYearsPlayed, out byte minimumRating, out string pathToOutputFile, out ushort filteredYear)
        {
            if (args.Length < 4) { throw new Exception("not enough commandline arguments provided"); }
            //print arguments
            //for (int i = 0; i < args.Length; i++)
            //{
            //    Console.WriteLine(args[i]);
            //}
            //get data source
            if (!File.Exists(args[0])) { throw new FileNotFoundException("File not found! Empty/invalid datasource directory"); }
            fullPath = args[0];
            //get years    
            if (!Byte.TryParse(args[1], out maxNumberOfYearsPlayed)) { throw new ArgumentException("Invalid numberOfYears"); }
            filteredYear = (ushort)DateTime.Now.AddYears(-maxNumberOfYearsPlayed).Year;
            //get rating
            if (!Byte.TryParse(args[2], out minimumRating)) { throw new ArgumentException("Invalid rating"); }
            //get output
            if (!String.IsNullOrEmpty(args[3]) && !String.IsNullOrWhiteSpace(args[3]))
            {
                pathToOutputFile = args[3];
            }
            else
            {
                throw new ArgumentException("Empty/invalid output directory");
            }
            //pathToOutputFile = args[3];
        }
    }
}
