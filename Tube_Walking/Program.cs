using System;
using System.IO;
using Tube_Walking_Guide;
using static System.Collections.Specialized.BitVector32;

namespace Tube_Walking_Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] stationData = File.ReadAllLines("C:\\Dev\\DS_A\\Tube_Walking_Guide\\Tube_Walking_Guide\\station_data.csv"); //Change this
            string[] stationData = File.ReadAllLines("/Users/melissabresler/Projects/DSA_CW/Tube_Walking/station_data.csv");
            Station[] stations = new Station[stationData.Length - 1];

            for (int i = 1; i < stationData.Length; i++)
            {
                string[] values = stationData[i].Split(',');
                int id = int.Parse(values[0]);
                string stationName = values[1];
                string[] stationLines = values[2].Split('-');

                stations[i - 1] = new Station(id, stationName, stationLines);

            }

            //string[] lines = File.ReadAllLines("C:\\Dev\\DS_A\\Tube_Walking_Guide\\Tube_Walking_Guide\\journey_data.csv"); //Change this
            string[] lines = File.ReadAllLines("/Users/melissabresler/Projects/DSA_CW/Tube_Walking/journey_data.csv");
            WalkingRoute[] routes = new WalkingRoute[lines.Length - 1];

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                Station station1 = null;
                Station station2 = null;

                int time = int.Parse(values[3]);

                // Find the stations with the given names in the array of stations
                foreach (Station station in stations)
                {
                    if (station.Name == values[1])
                    {
                        station1 = station;
                    }
                    else if (station.Name == values[2])
                    {
                        station2 = station;
                    }

                    if (station1 != null || station2 != null)
                    {
                        continue;
                    }
                }

                routes[i - 1] = new WalkingRoute(values[0], station1, station2, time);
            }

            //RouteFinder routeFinder = new RouteFinder(routes, stations);
            Menu menu = new Menu(stations, routes);
            menu.MainMenu();
            //string testRoute = routeFinder.PublishRoute(stations[14], stations[45]);
            //Console.WriteLine(testRoute);
        }

    }
}