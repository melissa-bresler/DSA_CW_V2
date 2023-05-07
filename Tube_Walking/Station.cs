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

        public override string ToString()
        {
            string lines = $"Lines=\n";

            for (int i = 0; i < LinesConnectedTo.Length; i++)
            {
                lines += $"          {LinesConnectedTo[i]}\n";
            }

            return $"Station ID = {StationID}\n" +
                $"Name = {Name}\n" +
                lines;
        }

    }
}
