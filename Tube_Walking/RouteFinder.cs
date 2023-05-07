using System;
using System.Collections.Generic;


namespace Tube_Walking_Guide
{
    internal class RouteFinder
    {
        public WalkingRoute[] Routes { get; set; }
        private WalkingRoute[] edgeTo = new WalkingRoute[88];
        private int[] distTo = new int[88];
        private PriorityQueue<Station, Array> priQueueDotNet = new PriorityQueue<Station, Array>();  //.Net Priority Queue
        //public Station[] Stations { get; set; }
        public List<Station> Stations { get; set; }

        public int[,] NetworkMatrix { get; set; }

        public RouteFinder(WalkingRoute[] routes, List<Station> stations)
        {
            Routes = routes;
            Stations = stations;

            int[,] NetworkMatrix = new int[Stations.Count, Stations.Count];
            foreach (WalkingRoute route in Routes)
            {
                if (route.IsOpen == true)
                {
                    int i = route.StartStation.StationID;
                    int j = route.EndStation.StationID;

                    NetworkMatrix[i, j] = route.TotalTime;
                    NetworkMatrix[j, i] = route.TotalTime;
                }
            }
            this.NetworkMatrix = NetworkMatrix;

        }

        // Returns EdgeTo array from lecture
        private void DjiktrasShortestPath(Station start)
        {
            int stationsToVisit = Stations.Count;

            for (int stationID = 0; stationID < stationsToVisit; stationID++)
            {
                distTo[stationID] = int.MaxValue;
            }
            distTo[start.StationID] = 0;


            priQueueDotNet.Enqueue(start, distTo);  //.Net Priority Queue


            while (priQueueDotNet.Count > 0)
            {  //.Net Priority Queue
                Station nearestStation = priQueueDotNet.Dequeue(); //.Net Priority Queue
                for (int i = 0; i < stationsToVisit; i++)
                {
                    int walkingTime = NetworkMatrix[nearestStation.StationID, i];
                    if (walkingTime > 0)
                    {
                        RelaxEdge(new WalkingRoute("", new Station(nearestStation.StationID),
                            new Station(i), walkingTime));
                    }


                }
            }
        }
        private void RelaxEdge(WalkingRoute route)
        {
            Station start = route.StartStation;
            Station end = route.EndStation;

            if (distTo[end.StationID] > distTo[start.StationID] + route.TotalTime)
            {
                distTo[end.StationID] = distTo[start.StationID] + route.TotalTime;

                edgeTo[end.StationID] = route;
                priQueueDotNet.Enqueue(end, distTo); //.Net Priority Queue
            }
        }
        // Returns path from start to finish
        private List<Station> FindWalkingRoute(Station start, Station destination)
        {
            DjiktrasShortestPath(start);
            List<Station> finalRoute = new List<Station>(); //.Net List instead of Array
            Station previousStation = destination;
            while (true)
            {
                if (previousStation.StationID == start.StationID)
                {
                    break;
                }
                for (int i = 0; i < edgeTo.Length; i++)
                {
                    if (edgeTo[i] == null)
                    {
                        continue;
                    }
                    if (edgeTo[i].EndStation.StationID == previousStation.StationID)
                    {
                        previousStation = edgeTo[i].StartStation;
                        finalRoute.Add(previousStation);
                    }

                }

            }
            finalRoute.Reverse(); //.Net Method
            finalRoute.Add(destination);//.Net Method
            return finalRoute;
        }

        public string PublishRoute(Station start, Station finish)
        {
            List<Station> fastestRoute = FindWalkingRoute(start, finish); //.Net List instead of Array
            int routeTime = 0;
            int lineNo = 1;
            int timeBeforeChange = 0;
            bool changeNextStop = false;

            string currentLine = CommonLineExists(GetStationDetails(fastestRoute[0]),
                GetStationDetails(fastestRoute[1]));
            Station sectionStart = GetStationDetails(fastestRoute[0]);
            Station sectionEnd = GetStationDetails(fastestRoute[fastestRoute.Count - 1]);

            string route = $"Route:\t{sectionStart.Name} to {sectionEnd.Name}\n\n";
            route += $"({lineNo}) Start: \t{sectionStart.Name} ({currentLine})\n\n";
            lineNo++;
            for (int i = 0; i < fastestRoute.Count; i++)
            {
                Station previousStop = fastestRoute[i];
                Station nextStop = fastestRoute[i + 1];

                if (changeNextStop == true)
                {
                    sectionEnd = GetStationDetails(fastestRoute[i]);
                    string previousLine = currentLine;

                    route += $"({lineNo}) \t\t{sectionStart.Name} ({currentLine}) to {sectionEnd.Name} ({currentLine}) {timeBeforeChange} min\n\n";
                    lineNo++;

                    changeNextStop = false;
                    timeBeforeChange = 0;
                    currentLine = CommonLineExists(GetStationDetails(fastestRoute[i]),
                                    GetStationDetails(fastestRoute[i + 1]));
                    sectionStart = GetStationDetails(fastestRoute[i]);
                    route += $"({lineNo}) Change: \t{sectionEnd.Name} ({previousLine}) to {sectionEnd.Name} ({currentLine})\n\n";
                    lineNo++;
                    // print Change: Station (previous) to Station (currentLine)
                }
                if (i <= fastestRoute.Count - 3)
                {
                    if (CommonLineExists(GetStationDetails(fastestRoute[i]),
                                        GetStationDetails(fastestRoute[i + 2])) !=
                                        CommonLineExists(GetStationDetails(fastestRoute[i + 1]),
                                        GetStationDetails(fastestRoute[i + 2])))
                    {
                        changeNextStop = true;

                    }
                }
                if (previousStop != null && nextStop != null)
                {
                    timeBeforeChange += NetworkMatrix[previousStop.StationID, nextStop.StationID];
                    routeTime += NetworkMatrix[previousStop.StationID, nextStop.StationID];
                }
                if (i == fastestRoute.Count - 2)
                {
                    sectionEnd = GetStationDetails(fastestRoute[i + 1]);
                    route += $"({lineNo}) \t\t{sectionStart.Name} ({currentLine}) to {sectionEnd.Name} ({currentLine}) {timeBeforeChange} min\n\n";
                    lineNo++;
                    route += $"({lineNo}) End: \t{sectionEnd.Name} ({currentLine})\n\n";
                    route += $"Total Journey Time: {routeTime} minutes";
                    break;
                }



            }
            return route;
        }
        private int GetRouteTime(Station current, Station next)
        {
            return NetworkMatrix[current.StationID, next.StationID];
        }
        private Station GetStationDetails(Station undetailed)
        {
            for (int i = 0; i < Stations.Count; i++)
            {
                if (undetailed.StationID == Stations[i].StationID)
                {
                    return Stations[i];
                }
            }
            return null;
        }
        private string CommonLineExists(Station first, Station next)
        {
            string[] lines1 = first.LinesConnectedTo;
            string[] lines2 = next.LinesConnectedTo;
            for (int i = 0; i < lines1.Length; i++)
            {
                for (int j = 0; j < lines2.Length; j++)
                {
                    if (lines1[i] == lines2[j])
                    {
                        return lines1[i];
                    }
                }
            }
            return null;

        }

    }
}