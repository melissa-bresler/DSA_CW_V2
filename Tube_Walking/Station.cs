using System;


namespace Tube_Walking_Guide
{
    internal class Station
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public string[] LinesConnectedTo { get; set; }
        public string Access { get; set; }
        public int Open { get; set; }

        public Station(int id)
        {
            StationID = id;
        }

        public Station(int id, string name, string[] lines, string access, int open)
        {
            StationID = id;
            Name = name;
            LinesConnectedTo = lines;
            Access = access;
            Open = open;
        }
        public override string ToString()
        {
            string output = "";
            output += $"Station ID No. : {StationID}\n";
            output += $"Name : {Name}\n";
            output += "Lines : ";
            foreach (string line in LinesConnectedTo)
            {
                output += $"{line} ";
            }
            output += "\n";
            output += $"Station Access : {Access}\n";
            if (Open == 1)
            {
                output += "Station Status : Open";
            }
            else
            {
                output += "Station Status : Closed";
            }

            return output;

        }


    }
}
