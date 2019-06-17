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
            QueryParams filter;
            
            try
            {
                //check number of commandline arguments
                filter = SanitizeInput(args);
                //SanitizeInput(args, out fullPath, out maxNumberOfYearsPlayed, out minimumRating, out pathToOutputFile, out filteredYear);
                players = AccessData(filter.fullPath, filter.minimumRating, filter.filteredYear);
                PrintToFile(players, filter.pathToOutputFile);

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

        
        private static QueryParams SanitizeInput(string[] args)
        {
            QueryParams result=new QueryParams() { };
            if (args.Length < 4) { throw new Exception("not enough commandline arguments provided"); }
            //print arguments
            //for (int i = 0; i < args.Length; i++)
            //{
            //    Console.WriteLine(args[i]);
            //}
            //get data source
            if (!File.Exists(args[0])) { throw new FileNotFoundException("File not found! Empty/invalid datasource directory"); }
            result.fullPath = args[0];
            //get years    
            try { result.maxNumberOfYearsPlayed = Byte.Parse(args[1]); } catch(Exception e) { throw new ArgumentException("Invalid numberOfYears "+e.Message); }
            result.filteredYear = (ushort)DateTime.Now.AddYears(-result.maxNumberOfYearsPlayed).Year;
            //get rating
            try { result.minimumRating = Byte.Parse(args[2]); }catch (Exception e) { throw new ArgumentException("Invalid rating " + e.Message); }
            //get output
            if (!String.IsNullOrEmpty(args[3]) && !String.IsNullOrWhiteSpace(args[3]))
            {
                result.pathToOutputFile = args[3];
            }
            else
            {
                throw new ArgumentException("Empty/invalid output directory");
            }
            return result;
        }
    }
}
