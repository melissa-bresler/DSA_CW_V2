using System;
using static System.Collections.Specialized.BitVector32;


namespace Tube_Walking_Guide
{
    internal class WalkingRoute : IComparable
    {
        public string Line { get; set; }
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public int EstimatedTime { get; set; }
        public int DelayTime { get; set; }
        public string DelayReason { get; set; }
        public int TotalTime { get; set; }
        public bool IsOpen { get; set; }
        public string ClosureReason { get; set; }

        public WalkingRoute(Station startStation, Station endStation)
        {
            StartStation = startStation;
            EndStation = endStation;
        }
        public WalkingRoute(string line, Station startStation, Station endStation, int estimatedTime)
        {
            Line = line;
            StartStation = startStation;
            EndStation = endStation;
            EstimatedTime = estimatedTime;
            DelayTime = 0;
            DelayReason = null;
            TotalTime = EstimatedTime + DelayTime;
            IsOpen = true;
            ClosureReason = null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            WalkingRoute other = (WalkingRoute)obj;
            return (StartStation == other.StartStation && EndStation == other.EndStation)
                || (StartStation == other.EndStation && EndStation == other.StartStation);
        }
        public override int GetHashCode()
        {
            return StartStation.GetHashCode() ^ EndStation.GetHashCode();
        }
        public int CompareTo(object route)
        {
            WalkingRoute comparison = (WalkingRoute)route;
            if (comparison.TotalTime < this.TotalTime)
            {
                return -1;
            }
            else if (comparison.TotalTime > this.TotalTime)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
