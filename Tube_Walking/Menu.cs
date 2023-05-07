using Tube_Walking_Guide;

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

        //private Station[] stationInfo = null;
        private List<Station> stationInfo = null;
        private WalkingRoute[] routeInfo;
        private RouteFinder routeFinder;

        public Menu(List<Station> stationInfo, WalkingRoute[] routeInfo)
        {
            this.stationInfo = stationInfo;
            this.routeInfo = routeInfo;
            this.routeFinder = new RouteFinder(routeInfo, stationInfo);
        }

        public void MainMenu()
        {
            bool exit = false;
            Console.Clear();
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
                    Console.Clear();
                    string exitM = "false";
                    while (exitM != "true")
                    {
                        Console.Clear();
                        Console.WriteLine("/////////////");
                        Console.WriteLine("MANAGER MENU");
                        Console.WriteLine("/////////////");
                        Console.WriteLine();
                        Console.WriteLine("1 Add or Remove delay");
                        Console.WriteLine("2 Open/Close access to route");
                        Console.WriteLine("3 Print out impossible walking routes");
                        Console.WriteLine("4 Print out delayed walking routes");
                        Console.WriteLine("5 Go back");
                        Console.WriteLine();
                        Console.Write("Please type the number of the option you'd like: ");
                        string mMenu = Console.ReadLine();//Choice selection


                        if (mMenu == "1")
                        {
                            WalkingRoute routeToDelay = RouteSelector();
                            Console.Clear();
                            bool routeFound = false;

                            foreach (WalkingRoute route in routeInfo)
                            {
                                if (route.Equals(routeToDelay))
                                {
                                    routeFound = true;
                                    Console.Write("Assign delay (0 for no delay): ");
                                    int delay = Convert.ToInt32(Console.ReadLine());

                                    route.DelayTime = delay;
                                    route.TotalTime = route.EstimatedTime + route.DelayTime;
                                    if (route.DelayTime == 0)
                                    {
                                        route.DelayReason = null;
                                    }
                                    else
                                    {
                                        Console.Write("Delay reason: ");
                                        route.DelayReason = Console.ReadLine();
                                    }
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
                                    Console.Write("Open (1) or close (2) route: ");
                                    int choice = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Closure reason: "); //This seems repetetive
                                    string closureReason = Console.ReadLine(); //And this

                                    if (choice == 1)
                                    {
                                        route.IsOpen = true;
                                        route.ClosureReason = null;

                                    }
                                    else if (choice == 2)
                                    {
                                        route.IsOpen = false;
                                        Console.Write("Closure reason: ");
                                        route.ClosureReason = Console.ReadLine();
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
                        else if (mMenu == "5")
                        {
                            exitM = "true";
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
                        Console.WriteLine("2 Display Information");
                        Console.WriteLine("3 Go back");
                        Console.WriteLine();
                        Console.Write("Please type the number of the option you'd like: ");
                        string cMenu = Console.ReadLine();//Choice selection

                        if (cMenu == "1")
                        {
                            Console.Clear();
                            WalkingRoute searchRoute = RouteSelector();
                            Console.Clear();
                            Console.WriteLine(routeFinder.PublishRoute(searchRoute.StartStation, searchRoute.EndStation));
                            Console.WriteLine();
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadKey();
                        }//Find a route
                        else if (cMenu == "2")
                        {
                            Console.Clear();
                            Console.WriteLine("Select station you would like information on:");
                            Station choice = StationSearch();
                            Console.Clear();
                            Console.WriteLine(choice.ToString());


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
            Console.Clear();
            Console.WriteLine("SELECT START STATION:");
            choice1 = StationSearch();
            Console.Clear();
            Console.WriteLine("SELECT FINAL STATION:");
            choice2 = StationSearch();

            return new WalkingRoute(choice1, choice2);

        }
        public Station StationSearch()
        {
            string[] lines = { "Bakerloo", "Central", "Circle", "District", "Hammersmith & City",
                "Jubilee", "Metropolitan", "Northern", "Piccadilly", "Victoria", "Waterloo & City" };
            int tab = 0;

            string rightCol;
            string leftCol = null;

            Console.WriteLine("Please select line: ");
            for (int i = 0; i < lines.Length; i++)
            {
                if (tab == 1)
                {
                    tab = 0;
                    rightCol = $"{i + 1}. {lines[i]}\n";
                    Console.WriteLine(String.Format("{0,-30} {1,-15}", leftCol, rightCol));
                }
                else
                {
                    leftCol = $"{i + 1}. {lines[i]}";
                    tab++;
                }
            }
            Console.WriteLine();
            Console.Write("Selection: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            string line = lines[choice - 1];

            List<Station> stations = new List<Station>(); //.Net List instead of Array
            for (int i = 0; i < stationInfo.Count; i++)
            {
                foreach (string tubeLine in stationInfo[i].LinesConnectedTo)
                {
                    if (tubeLine == line)
                    {
                        Console.WriteLine(stationInfo[i].Name);
                        stations.Add(stationInfo[i]);
                    }
                }
            }
            tab = 0;
            Console.Clear();
            Console.WriteLine("Choose station:");
            for (int i = 0; i < stations.Count; i++)
            {
                if (tab == 1)
                {
                    tab = 0;
                    rightCol = $"{i + 1}. {stations[i].Name}\n";
                    Console.WriteLine(String.Format("{0,-30} {1,-15}", leftCol, rightCol));
                }
                else
                {
                    leftCol = $"{i + 1}. {stations[i].Name}";
                    tab++;
                }
            }
            Console.WriteLine(leftCol);
            Console.Write("\nSelection: ");
            choice = Convert.ToInt32(Console.ReadLine());

            return stations[choice - 1];
        }
    }

}