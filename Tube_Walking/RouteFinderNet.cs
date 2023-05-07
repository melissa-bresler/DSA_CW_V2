using System;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic; //Needed for Dot Net Priority Queue


namespace Tube_Walking_Guide
{
    internal class RouteFinderNet
    {
        public WalkingRoute[] Routes { get; set; }
        private WalkingRoute[] edgeTo = new WalkingRoute[88];
        private int[] distTo = new int[88];
        //private PriorityQueue priQueue = new PriorityQueue(); //Changed from this...
        private PriorityQueue<Station, Array> priQueueDotNet = new PriorityQueue<Station, Array>(); //...to this.
        public Station[] Stations { get; set; }

        public int[,] NetworkMatrix { get; set; }
        public RouteFinderNet(WalkingRoute[] routes, Station[] stations)
        {
            Routes = routes;
            Stations = stations;

            int[,] NetworkMatrix = new int[Stations.Length, Stations.Length];
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
            int stationsToVisit = Stations.Length;

            for (int stationID = 0; stationID < stationsToVisit; stationID++)
            {
                distTo[stationID] = int.MaxValue;
            }
            distTo[start.StationID] = 0;


            //priQueue.Enqueue(start, distTo); //Changed from this...
            priQueueDotNet.Enqueue(start, distTo); //...to this.


            //while (!priQueue.IsEmpty()) { //Changed from this...
            while (priQueueDotNet.Count > 0)
            { //...to this.
                //Station nearestStation = priQueue.Dequeue(distTo); //Changed from this...
                Station nearestStation = priQueueDotNet.Dequeue();//...to this.
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
                //priQueue.Enqueue(end, distTo);//Changed from this...
                priQueueDotNet.Enqueue(end, distTo); //...to this.
            }
        }
        // Returns path from start to finish - To do
        private Station[] FindWalkingRoute(Station start, Station destination)
        {
            DjiktrasShortestPath(start);
            Station[] finalRoute = new Station[0];
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
                        finalRoute = Utils.ExtendStationArray(finalRoute);
                        finalRoute[finalRoute.Length - 1] = previousStation;
                    }

                }

            }
            Array.Reverse(finalRoute);
            finalRoute = Utils.ExtendStationArray(finalRoute);
            finalRoute[finalRoute.Length - 1] = destination;
            return finalRoute;
        }

        public string PublishRoute(Station start, Station finish)
        {
            Station[] fastestRoute = FindWalkingRoute(start, finish);
            int routeTime = 0;
            int lineNo = 1;
            int timeBeforeChange = 0;
            bool changeNextStop = false;

            string currentLine = CommonLineExists(GetStationDetails(fastestRoute[0]),
                GetStationDetails(fastestRoute[1]));
            Station sectionStart = GetStationDetails(fastestRoute[0]);
            Station sectionEnd = GetStationDetails(fastestRoute[fastestRoute.Length - 1]);

            string route = $"Route:\t{sectionStart.Name} to {sectionEnd.Name}\n\n";
            route += $"({lineNo}) Start: \t{sectionStart.Name} ({currentLine})\n\n";
            lineNo++;
            for (int i = 0; i < fastestRoute.Length; i++)
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
                if (i <= fastestRoute.Length - 3)
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
                if (i == fastestRoute.Length - 2)
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
            for (int i = 0; i < Stations.Length; i++)
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