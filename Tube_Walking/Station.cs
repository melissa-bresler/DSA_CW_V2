using System;


namespace Tube_Walking_Guide
{
    internal class Station
    {
        public int StationID { get; set; }
        public string Name { get; set; }
        public string[] LinesConnectedTo { get; set; }

        public Station(int id)
        {
            StationID = id;
        }

        public Station(int id, string name, string[] lines)
        {
            StationID = id;
            Name = name;
            LinesConnectedTo = lines;
        }


    }
}
