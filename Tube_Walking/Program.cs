using System;
using System.IO;
using Tube_Walking_Guide;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tube_Walking_Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stationData = File.ReadAllLines("/Users/melissabresler/Projects/DSA_CW/Tube_Walking/station_data.csv");
            //Station[] stations = new Station[stationData.Length - 1];
            List<Station> stations = new List<Station>();

            for (int i = 1; i < stationData.Length; i++)
            {
                string[] values = stationData[i].Split(',');
                int id = int.Parse(values[0]);
                string stationName = values[1];
                string access = values[3];
                string[] stationLines = values[2].Split('-');
                int open = int.Parse(values[4]);

                //stations[i - 1] = new Station(id, stationName, stationLines, access, open);
                stations.Add(new Station(id, stationName, stationLines, access, open));

            }

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

            Menu menu = new Menu(stations, routes);
            menu.MainMenu();
        }

    }
}