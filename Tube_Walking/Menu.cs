using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using Tube_Walking_Guide;
using static System.Collections.Specialized.BitVector32;

namespace Tube_Walking_Calc
{
    internal class Menu
    {
        //TFL Managers can:
        //-Add or remove journey walking time delays due to an incident to the walking times between tube stations
        //-Indicate a route is now impossible (bridge closed, building on fire) or becomes possible again between tube stations
        //-Print out the list of impossible walking routes
        //-Print out the list of delayed walking routes, with normal time and delayed times

        //Customers can:
        //-Find a route by entering a start and end station and find the fastest route between them
        //-Display information about a tube, similar to that available from the TfL web site
        private Station[] stationInfo;
        private WalkingRoute[] routeInfo;
        public Menu(Station[] stationInfo, WalkingRoute[] routeInfo)
        {
            this.stationInfo = stationInfo;
            this.routeInfo = routeInfo;
        }

        public void MainMenu()
        {
            //RouteFinder routeFinder = new RouteFinder(routeInfo, stationInfo); //Original Version
            RouteFinderNet routeFinderNet = new RouteFinderNet(routeInfo, stationInfo); //Dot Net Version
            bool exit = false;
            Console.Clear(); //This is probably unnecessary
            while (exit == false)
            {
                Console.Clear();
                Console.WriteLine("/////////////");
                Console.WriteLine("CHOOSE A MENU");
                Console.WriteLine("/////////////");
                Console.WriteLine();
                Console.WriteLine("1 Manager");
                Console.WriteLine("2 Customer");
                Console.WriteLine();
                Console.Write("Please type the number of the option you'd like: ");
                string menuS = Console.ReadLine();//Choice selection

                if (menuS == "1")
                {
                    //menuS = "Reset";
                    Console.Clear();
                    string exitM = "false";
                    while (exitM != "true")
                    {
                        Console.Clear();
                        Console.WriteLine("/////////////");
                        Console.WriteLine("MANAGER MENU");
                        Console.WriteLine("/////////////");
                        Console.WriteLine();
                        Console.WriteLine("1 Add or Remove delay"); //Separate these into two options?
                        Console.WriteLine("2 Open/Close access to route"); //Separate these into two options?
                        Console.WriteLine("3 Print out impossible walking routes");
                        Console.WriteLine("4 Print out delayed walking routes");
                        Console.WriteLine("5 Go back");
                        Console.WriteLine();
                        Console.Write("Please type the number of the option you'd like: ");
                        string mMenu = Console.ReadLine();//Choice selection


                        if (mMenu == "1")
                        {

                            Console.Clear();
                            int tab = 0;
                            Console.WriteLine("Stations:");
                            for (int i = 0; i < stationInfo.Length; i++)
                            {
                                if (tab == 1)
                                {
                                    tab = 0;
                                    Console.WriteLine($"{i + 1}. {stationInfo[i].Name}\n");
                                }
                                else
                                {
                                    Console.WriteLine($"{i + 1}. {stationInfo[i].Name}\t");
                                }
                            }
                            Console.WriteLine();
                            Console.Write("Start of delayed route: ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            Station start = stationInfo[choice - 1];

                            Console.Write("End of delayed route: ");
                            int choice2 = Convert.ToInt32(Console.ReadLine());
                            Station end = stationInfo[choice2 - 1];

                            Console.Clear();

                            bool routeFound = false;

                            foreach (WalkingRoute route in routeInfo)
                            {
                                if (route.StartStation == start && route.EndStation == end)
                                {
                                    routeFound = true;
                                    Console.Write("Assign delay: ");
                                    int delay = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Delay reason: ");
                                    string delayReason = Console.ReadLine();

                                    route.DelayTime = delay;
                                    route.DelayReason = delayReason;
                                    Console.WriteLine("Success");
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();
                                }
                            }
                            if (routeFound == false)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Route not found");
                                Console.WriteLine("Press any key to continue.");
                                Console.ReadKey();
                            }

                        }//Add or remove delay
                        else if (mMenu == "2")
                        {
                            WalkingRoute routeToClose = RouteSelector();
                            foreach (WalkingRoute route in routeInfo)
                            {
                                if (route.Equals(routeToClose))
                                {
                                    Console.Write("Open (1) or close (2) route: "); //This doesn't run?
                                    int choice = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Closure reason: ");
                                    string closureReason = Console.ReadLine();

                                    if (choice == 1)
                                    {
                                        route.IsOpen = true;

                                    }
                                    else if (choice == 2)
                                    {
                                        route.IsOpen = false;
                                    }

                                    route.ClosureReason = closureReason;
                                    Console.WriteLine("Success");
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();

                                }
                            }
                        }//Open/Close access to route
                        else if (mMenu == "3")
                        {
                            string output = "";
                            Console.WriteLine("Close routes:");
                            foreach (WalkingRoute route in routeInfo)
                            {
                                if (route.IsOpen == false)
                                {
                                    output += $"{route.Line} Line: {route.StartStation} - {route.EndStation} : {route.ClosureReason}\n";
                                }
                            }
                            Console.WriteLine(output);
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Print out impossible routes
                        else if (mMenu == "4")
                        {
                            string output = "";
                            Console.WriteLine("Delayed routes:");
                            foreach (WalkingRoute route in routeInfo)
                            {
                                if (route.DelayTime > 0)
                                {
                                    output += $"{route.Line} Line: {route.StartStation} - {route.EndStation} : {route.EstimatedTime} min now {route.TotalTime} min\n";
                                }
                            }
                            Console.WriteLine(output);
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Print out delayed routes
                        else if (mMenu == "5") //This is causing problems for some reason
                        {
                            exitM = "true";
                            menuS = "This is a problem"; //Delete later
                            Console.Clear();
                        }//Go back
                        else
                        {
                            Console.Clear();
                            //Console.WriteLine();
                            Console.WriteLine($"Option {mMenu} does not exist.");
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Error message

                    }

                }

                if (menuS == "2")
                {
                    menuS = "Reset";
                    Console.Clear();
                    string exitC = "false";
                    while (exitC != "true")
                    {
                        Console.Clear();
                        Console.WriteLine("/////////////");
                        Console.WriteLine("CUSTOMER MENU");
                        Console.WriteLine("/////////////");
                        Console.WriteLine();
                        Console.WriteLine("1 Find a route");
                        Console.WriteLine("2 Display Information"); //Code not done yet
                        Console.WriteLine("3 Go back");
                        Console.WriteLine();
                        Console.Write("Please type the number of the option you'd like: ");
                        string cMenu = Console.ReadLine();//Choice selection

                        if (cMenu == "1")
                        {
                            Console.Clear();
                            WalkingRoute searchRoute = RouteSelector();
                            Console.Clear(); //This does nothing at the moment
                            //Console.WriteLine(routeFinder.PublishRoute(searchRoute.StartStation, searchRoute.EndStation));
                            Console.WriteLine(routeFinderNet.PublishRoute(searchRoute.StartStation, searchRoute.EndStation));
                            Console.WriteLine();
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Find a route
                        else if (cMenu == "2")
                        {
                            Console.Clear();
                            int tab = 0;
                            Console.WriteLine("Please select station: ");
                            for (int i = 0; i < stationInfo.Length; i++)
                            {
                                if (tab == 1)
                                {
                                    tab = 0;
                                    Console.WriteLine($"{i + 1}. {stationInfo[i].Name}\n");
                                }
                                else
                                {
                                    Console.WriteLine($"{i + 1}. {stationInfo[i].Name}\t");
                                }
                            }
                            Console.WriteLine();
                            Console.Write("Selection: ");
                            int choice = Convert.ToInt32(Console.ReadLine());

                            Station station = stationInfo[choice - 1];
                            Console.Clear();
                            Console.WriteLine(station.ToString());
                            Console.WriteLine();
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                            Console.Clear();

                        }//Display information
                        else if (cMenu == "3")
                        {
                            exitC = "true";
                        }//Go back
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Option {cMenu} does not exist.");
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Error message

                    }

                }
                
            }//end of programme loop
        }

        public WalkingRoute RouteSelector()
        {

            Station choice1 = null;
            Station choice2 = null;

            Console.WriteLine("SELECT START STATION");
            Console.WriteLine();
            choice1 = StationSearch();
            Console.WriteLine();
            Console.WriteLine("SELECT FINAL STATION");
            Console.WriteLine();
            choice2 = StationSearch();

            return new WalkingRoute(choice1, choice2);

        }
        public Station StationSearch()
        {
            string[] lines = { "Bakerloo", "Central", "Circle", "District", "Hammersmith & City",
                "Jubilee", "Metropolitan", "Northern", "Piccadilly", "Victoria", "Waterloo & City" };
            int tab = 0;
            Console.WriteLine("Please select line: ");
            for (int i = 0; i < lines.Length; i++)
            {
                if (tab == 1)
                {
                    tab = 0;
                    Console.WriteLine($"{i + 1}. {lines[i]}\n");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {lines[i]}\t");
                }
            }
            Console.WriteLine();
            Console.Write("Selection: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            string line = lines[choice - 1];

            Station[] stations = new Station[0];
            for (int i = 0; i < stationInfo.Length; i++)
            {
                foreach (string tubeLine in stationInfo[i].LinesConnectedTo)
                {
                    if (tubeLine == line)
                    {
                        stations = Utils.ExtendStationArray(stations);
                        stations[stations.Length - 1] = stationInfo[i];
                    }
                }
            }
            tab = 0;
            Console.WriteLine();
            Console.WriteLine("Choose station:");
            for (int i = 0; i < stations.Length; i++)
            {
                if (tab == 1)
                {
                    Console.WriteLine($"{i + 1}. {stations[i].Name}\n");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {stations[i].Name}\t");
                }
            }
            Console.WriteLine();
            Console.Write("Selection: ");
            choice = Convert.ToInt32(Console.ReadLine());

            return stations[choice - 1];
        }
    }

}

