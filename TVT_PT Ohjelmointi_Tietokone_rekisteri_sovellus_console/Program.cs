using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;
using System.IO;

namespace TVT_PT_Ohjelmointi_Tietokone_rekisteri_sovellus_console
{

    #region CLASSES FOR COMPUTER AND ROOM
    class Computer
    {
        // Creating class for Computers including Name and FieldIndex for LINQtoCSV

        [CsvColumn (Name = "ComputerID", FieldIndex = 1)]
        public int computerID { get; set; }
        [CsvColumn(Name = "ComputerBrand", FieldIndex = 2)]
        public string computerBrand { get; set; }
        [CsvColumn(Name = "ComputerModel", FieldIndex = 3)]
        public string computerModel { get; set; }
        [CsvColumn(Name = "ComputerOwnerShip", FieldIndex = 4)]
        public string computerOwnership { get; set; }
        [CsvColumn(Name = "RoomIncludedID", FieldIndex = 5)]
        public int roomIncludedID { get; set; }

        // Constructor for future
        public string ComputerDetails()
        {
            return string.Format("ID: {1}, {2}, {3}, {4} which is in room {5}",
                computerID, computerBrand, computerModel, computerOwnership, roomIncludedID);
        }
    }
    class Room
    {
        // Creating class for Rooms including Name and FieldIndex for LINQtoCSV
        [CsvColumn(Name = "RoomID", FieldIndex = 1)]
        public int roomID { get; set; }
        [CsvColumn(Name = "RoomName", FieldIndex = 2)]
        public string roomName { get; set; }
        [CsvColumn(Name = "RoomNumber", FieldIndex = 3)]
        public string roomNumber { get; set; }

        // Constructor for future
        public string RoomDetails()
        {
            return string.Format("ID {1}, {2}, {3}",
                roomID, roomName, roomNumber);

        }

    }
    #endregion
    #region CREATING LISTS
    class Program
    {
        static void Main(string[] args)
        {
            // File description for LINQtoCSV
            var csvFileDescriptionBegin = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false
            };

            // New context for LINQtoCSV
            var csvContextBegin = new CsvContext(); // For computers
            var csvContextBegin2 = new CsvContext(); // For rooms

            // Data paths
            string computers_Source = "computer.csv";
            string room_Source = "rooms.csv";

            // Reading computers from computers_Source
            var computers_get = csvContextBegin.Read<Computer>(computers_Source, csvFileDescriptionBegin);

            // Creating list and listing computers from computers_get to ComputerList
            List<Computer> ComputerList = new List<Computer>();
            foreach (var c in computers_get)
            {
                ComputerList.Add(new Computer { computerID = c.computerID, computerBrand = c.computerBrand, computerModel = c.computerModel, computerOwnership = c.computerOwnership, roomIncludedID = c.roomIncludedID });
            }
            // Reading rooms from room_Source
            var rooms_get = csvContextBegin2.Read<Room>(room_Source, csvFileDescriptionBegin);

            // Creating list and listing computers from rooms_get to RoomList
            List<Room> RoomList = new List<Room>();
            foreach (var r in rooms_get)
            {
                RoomList.Add(new Room { roomID = r.roomID, roomName = r.roomName, roomNumber = r.roomNumber });

            }
            // Menu loops
            bool showmenu = true;
            while (showmenu)
            {
                // New alphabetical lists for index
                List<Computer> ComputerListByID = ComputerList.OrderBy(o => o.computerID).ToList();
                List<Room> RoomListByID = RoomList.OrderBy(o => o.roomID).ToList();

                Console.Clear();
                Heading("ComputerViaRoom2CSV");
                Console.WriteLine($"Current files for write&read: {computers_Source} || {room_Source} \n");

                // Printing all computers from ALPHABETICAL list to index when Menu loop starts
                // Loop fetches all computers in list and compares them to Room ID:s finally combining rooms into computers included to them
                ListHeaders(1, "computers");
                foreach (Computer computer in ComputerListByID)
                {
                    foreach (Room room in RoomListByID)
                    {
                        if (computer.roomIncludedID == room.roomID)
                        {
                            Console.WriteLine($"ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership} => Room ID: {room.roomID}, {room.roomName}, {room.roomNumber} ");
                        }
                    }
                }
                #endregion
                #region MENU PRINT
                // Printing menu selections
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Please select an option:");
                Console.ResetColor();

                Console.WriteLine("X = save list on CSV file  Y = Info about software and saving methods");
                Console.WriteLine(@"Current CSV save path = \rooms.csv & \computers.csv");
                Console.WriteLine("A => List computers  |   C => Add computers   |   D => Edit existing computer   |  E => Remove computer");
                Console.WriteLine("B => List rooms      |   F => Add room        |   G => Edit existing room       |  H => Remove room");
                Console.WriteLine("I => Move computers to new rooms");
                Console.WriteLine("\n\n");
                Console.WriteLine("Q => Quit application");

                Console.Write("\nPlease make selection:");
                char selection = Convert.ToChar(Console.ReadLine());
                #endregion
                #region LIST -> COMPUTERS
                if (selection == 'A' || selection == 'a')
                {
                    // Listing all computers in ComputerList
                    Console.Clear();
                    ListHeaders(1, "computers");

                    foreach (Computer computer in ComputerListByID)
                    {
                        foreach (Room room in RoomListByID)
                        {
                            if (computer.roomIncludedID == room.roomID)
                            {
                                Console.WriteLine($"ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership} => Room ID: {room.roomID}, {room.roomName}, {room.roomNumber} ");
                            }
                        }
                    }
                    ListMessages("AnyKey");
                    Console.ReadKey();
                }
                #endregion
                #region LIST -> ROOMS
                else if (selection == 'B' || selection == 'b')
                {
                    // Listing rooms in RoomListByID
                    Console.Clear();
                    ListHeaders(1, "rooms");

                    foreach (Room rooms in RoomListByID)
                    {
                        Console.WriteLine($"ID: {rooms.roomID}, {rooms.roomName}, {rooms.roomNumber}");
                    }

                    ListMessages("AnyKey");
                    Console.ReadKey();
                }
                #endregion
                #region ADD -> COMPUTERS
                else if (selection == 'C' || selection == 'c')
                {
                    Console.Clear();


                    // Printing computer list to user for comparason
                    foreach (Computer computer in ComputerListByID)
                    {
                        foreach (Room room in RoomList)
                        {
                            if (computer.roomIncludedID == room.roomID)
                            {
                                Console.WriteLine($"ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership} => Room ID: {room.roomID}, {room.roomName}, {room.roomNumber} ");
                            }
                        }
                    }
                    // Asking values from user
                    ListMessages("Add");

                    ListMessages("Select_");
                    Labels("computerID_upper");
                    Console.Write("of computer you want to add:");
                    int newComputerID = Convert.ToInt32(Console.ReadLine());

                    ListMessages("Select_");
                    Labels("computerBrand_upper");
                    Console.Write("of computer you want to add:");
                    string newComputerBrand = Console.ReadLine();

                    ListMessages("Select_");
                    Labels("computerModel_upper");
                    Console.Write("of computer you want to add:");
                    string newComputerModel = Console.ReadLine();

                    ListMessages("Select_");
                    Labels("computerOwnership_upper");
                    Console.Write("of computer you want to add:");
                    string newComputerOwnerShip = Console.ReadLine();
                    
                    // Printing alphabetical room list for user for room selection
                    foreach (Room room in RoomListByID)
                    {
                        Console.WriteLine($"ID {room.roomID}, {room.roomName}, {room.roomNumber}");

                    }
                    ListMessages("Select_");
                    Console.Write("room by ID you want to add new computer: ");
                    int newComputerRoom = Convert.ToInt32(Console.ReadLine());

                    Console.Clear();
                    // Asking user to either save or discard data to list (not writing to file yet)

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Do you want to add:");
                    Console.ResetColor();
                    Console.WriteLine($"ID: {newComputerID}, {newComputerBrand}, {newComputerModel}, {newComputerOwnerShip}, storaged in room {newComputerRoom}");
                    Console.WriteLine("to list? (Y = yes | N = no");
                    char saveSelection = Convert.ToChar(Console.ReadLine());

                    if (saveSelection == 'Y' || saveSelection == 'y')
                    {
                        // If want to write to file after Y-selection, insert file writing code here
                        ComputerList.Add(new Computer { computerID = newComputerID, computerBrand = newComputerBrand, computerModel = newComputerModel, computerOwnership = newComputerOwnerShip, roomIncludedID = newComputerRoom });
                        Console.Clear();
                        ListMessages("Saved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                    if (saveSelection == 'N' || saveSelection == 'n')
                    {
                        Console.Clear();
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                }
                #endregion
                #region REMOVE -> COMPUTERS
                else if (selection == 'E' || selection == 'e')
                {

                    int rangeMax = ComputerList.Count;
                    Console.Clear();
                    int i = 0;

                    ListMessages("Remove");

                    // printing List of computers for user
                    foreach (Computer computer in ComputerListByID)
                    {
                        i++;
                        Console.WriteLine($"#{i} ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership}");
                    }

                    // asking for deletion number
                    ListMessages("Select_");
                    Console.Write("# - number of computer which you want to delete:");
                    int deletionNumber = Convert.ToInt32(Console.ReadLine());
                    if (deletionNumber <= ComputerList.Count)
                    {

                        // list object number is deletionNumber -1
                        ComputerList.RemoveAt(deletionNumber - 1);
                        ListMessages("Saved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                    else
                    {
                        Error("Faulty selection.");
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                }
                #endregion
                #region EDIT -> COMPUTERS
                else if (selection == 'D' || selection == 'd')
                {

                    int rangeMax = ComputerList.Count;
                    Console.Clear();
                    int i = 0;

                    ListMessages("Edit");
                    // Printing out list aplhabetical list of computers
                    foreach (Computer computer in ComputerListByID)
                    {
                        //Growing i gives index numbers
                        i++;
                        Console.WriteLine($"#{i} ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership}");
                    }
                    Console.Write("# reference for item you want to edit");
                    int indexSelect = Convert.ToInt32(Console.ReadLine());
                    if (indexSelect <= rangeMax)
                    {
                        // Asking user input for new computer info
                        ListMessages("Select_");
                        Labels("computerID_upper");
                        Console.Write($" of computer (#: {indexSelect}) you want to add:");
                        int newComputerID = Convert.ToInt32(Console.ReadLine());

                        ListMessages("Select_");
                        Labels("computerBrand_upper");
                        Console.Write($" of computer (#: {indexSelect}, ID {newComputerID}) you want to add:");
                        string newComputerBrand = Console.ReadLine();

                        ListMessages("Select_");
                        Labels("computerModel_upper");
                        Console.Write($" of computer (#: {indexSelect}, ID {newComputerID}, {newComputerBrand}) you want to add:");
                        string newComputerModel = Console.ReadLine();

                        ListMessages("Select_");
                        Labels("computerOwnership_upper");
                        Console.Write($"of computer (#: {indexSelect}, ID {newComputerID}, model {newComputerBrand}, brand {newComputerModel}) you want to add:");
                        string newComputerOwnership = Console.ReadLine();

                        // Asking for save or no (Y or N)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Do you want to add:");
                        Console.ResetColor();
                        Console.WriteLine($"#: {indexSelect}, ID {newComputerID}, {newComputerBrand}, {newComputerModel}, {newComputerOwnership} ");
                        Console.WriteLine("to list? (Y = yes | N = no");
                        char saveSelection = Convert.ToChar(Console.ReadLine());
                        if (saveSelection == 'Y' || saveSelection == 'y')
                        {
                            // Adding new data to list
                            ComputerList[indexSelect - 1].computerID = newComputerID;
                            ComputerList[indexSelect - 1].computerBrand = newComputerBrand;
                            ComputerList[indexSelect - 1].computerModel = newComputerModel;
                            ComputerList[indexSelect - 1].computerOwnership = newComputerOwnership;

                            ListMessages("Saved");
                            ListMessages("AnyKey");
                            Console.ReadKey();

                        }
                        if (saveSelection == 'N' || saveSelection == 'n')
                        {
                            ListMessages("NotSaved");
                            ListMessages("AnyKey");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Error("Bad list item selection!");
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }

                }
                #endregion
                #region ADD -> ROOM
                else if (selection == 'F' || selection == 'f')
                {
                    Console.Clear();
                    ListHeaders(1, "rooms");

                    foreach (Room rooms in RoomListByID)
                    {
                        Console.WriteLine($"ID: {rooms.roomID}, {rooms.roomName}, {rooms.roomNumber}");
                    }

                    ListMessages("Add");

                    ListMessages("Select_");
                    Labels("roomID_upper");
                    Console.Write("of room you want to add:");
                    int newRoomID = Convert.ToInt32(Console.ReadLine());

                    ListMessages("Select_");
                    Labels("roomName_upper");
                    Console.Write("of room you want to add:");
                    string newRoomName = Console.ReadLine();

                    ListMessages("Select_");
                    Labels("roomNumber_upper");
                    Console.Write("of room you want to add:");
                    string newRoomNumber = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Do you want to add:");
                    Console.ResetColor();
                    Console.WriteLine($"ID: {newRoomID}, {newRoomName}, {newRoomNumber}");
                    Console.WriteLine("to list? (Y = yes | N = no");
                    char saveSelection = Convert.ToChar(Console.ReadLine());

                    if (saveSelection == 'Y' || saveSelection == 'y')
                    {
                        RoomList.Add(new Room { roomID = newRoomID, roomName = newRoomName, roomNumber = newRoomNumber});
                        Console.Clear();
                        ListMessages("Saved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                    if (saveSelection == 'N' || saveSelection == 'n')
                    {
                        Console.Clear();
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }


                }
                #endregion
                #region EDIT -> ROOM
                if (selection == 'G' || selection == 'g')
                {
                    int rangeMax = RoomList.Count;
                    Console.Clear();
                    int i = 0;

                    ListMessages("Edit");

                    foreach (Room room in RoomListByID)
                    {
                        i++;
                        Console.WriteLine($"#{i} ID: {room.roomID}, {room.roomName}, {room.roomNumber}");
                    }
                    Console.Write("# reference for item you want to edit");
                    int indexSelect = Convert.ToInt32(Console.ReadLine());
                    if (indexSelect <= rangeMax)
                    {
                        ListMessages("Select_");
                        Labels("roomID_upper");
                        Console.Write($" of room (#: {indexSelect}) you want to add:");
                        int newRoomID = Convert.ToInt32(Console.ReadLine());

                        ListMessages("Select_");
                        Labels("roomName_upper");
                        Console.Write($" of room (#: {indexSelect}, ID {newRoomID}) you want to add:");
                        string newRoomName = Console.ReadLine();

                        ListMessages("Select_");
                        Labels("roomNumber_upper");
                        Console.Write($" of room (#: {indexSelect}, ID {newRoomID}, {newRoomName}) you want to add:");
                        string newRoomNumber = Console.ReadLine();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Do you want to add:");
                        Console.ResetColor();
                        Console.WriteLine($"#: {indexSelect}, ID {newRoomID}, {newRoomName}, {newRoomNumber}");
                        Console.WriteLine("to list? (Y = yes | N = no");
                        char saveSelection = Convert.ToChar(Console.ReadLine());
                        if (saveSelection == 'Y' || saveSelection == 'y')
                        {
                            RoomList[indexSelect - 1].roomID = newRoomID;
                            RoomList[indexSelect - 1].roomName = newRoomName;
                            RoomList[indexSelect - 1].roomNumber = newRoomNumber;

                            ListMessages("Saved");
                            ListMessages("AnyKey");
                            Console.ReadKey();

                        }
                        if (saveSelection == 'N' || saveSelection == 'n')
                        {
                            ListMessages("NotSaved");
                            ListMessages("AnyKey");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Error("Bad list item selection!");
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                }
                #endregion
                #region REMOVE -> ROOM
                if (selection == 'H' || selection == 'h')
                {
                    int rangeMax = RoomList.Count;
                    Console.Clear();
                    int i = 0;

                    ListMessages("Remove");

                    foreach (Room room in RoomListByID)
                    {
                        i++;
                        Console.WriteLine($"#{i} ID: {room.roomID}, {room.roomName}, {room.roomNumber}");
                    }

                    ListMessages("Select_");
                    Console.Write("# - number of room which you want to delete:");
                    int deletionNumber = Convert.ToInt32(Console.ReadLine());
                    if (deletionNumber <= RoomList.Count)
                    {
                        RoomList.RemoveAt(deletionNumber - 1);
                        ListMessages("Saved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                    else
                    {
                        Error("Faulty selection.");
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
          

                }
                #endregion
                #region MOVE -> COMPUTERS IN ROOMS
                else if (selection == 'I' || selection == 'i')
                {
                    Console.Clear();
                    ListHeaders(1, "computers");
                    foreach (Computer computer in ComputerList)
                    {
                        foreach (Room room in RoomListByID)
                        {
                            if (computer.roomIncludedID == room.roomID)
                            {
                                Console.WriteLine($"ID: {computer.computerID}, {computer.computerBrand}, {computer.computerModel}, {computer.computerOwnership} => Room ID: {room.roomID}, {room.roomName}, {room.roomNumber} ");
                            }
                        }
                    }
                    Console.WriteLine("Please select which computer you want to move:");
                    int indexSelect = Convert.ToInt32(Console.ReadLine());
                    ListHeaders(1, "rooms");

                    foreach (Room rooms in RoomList)
                    {
                        Console.WriteLine($"ID: {rooms.roomID}, {rooms.roomName}, {rooms.roomNumber}");
                    }
                    Console.WriteLine("Please select room you want to move computer");
                    int roomSelect = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine($"Do you want to move computer {indexSelect} to room {roomSelect} ?");
                    Console.WriteLine("Y = yes | N = no");

                    char saveSelection = Convert.ToChar(Console.ReadLine());

                    if (saveSelection == 'Y' || saveSelection == 'y')
                    {
                        ComputerList[indexSelect -1].roomIncludedID = roomSelect;
                        ListMessages("Saved");
                        ListMessages("AnyKey");
                        Console.ReadKey();
                    }
                    else if (saveSelection == 'N' || saveSelection == 'n')
                    {
                        ListMessages("NotSaved");
                        ListMessages("AnyKey");
                        Console.ReadKey();

                    }

                }
                #endregion
                #region SAVE TO CSV
                if (selection == 'X' || selection == 'x')
                {
                    var csvFileDescription = new CsvFileDescription
                    {
                        FirstLineHasColumnNames = true,
                        SeparatorChar = ','
                    };

                    string datapath = "computer.csv";

                    var csvContext = new CsvContext();
                    csvContext.Write(ComputerList, datapath, csvFileDescription);

                    string datapath2 = "rooms.csv";

                    var csvContext2 = new CsvContext();
                    csvContext2.Write(RoomList, datapath2, csvFileDescription);
                    if (File.Exists(datapath) && File.Exists(datapath2))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Data saved to {datapath} and {datapath2} successfully");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    else
                    {
                        Error($"One of files {datapath}, {datapath2} missing!");

                    }

                    var computers_refresh = csvContextBegin.Read<Computer>("computer.csv", csvFileDescriptionBegin);
                    var rooms_refresh = csvContextBegin2.Read<Room>("rooms.csv", csvFileDescriptionBegin);

                    ComputerList.Clear();
                    RoomList.Clear();

                    foreach (var c in computers_refresh)
                    {
                        ComputerList.Add(new Computer { computerID = c.computerID, computerBrand = c.computerBrand, computerModel = c.computerModel, computerOwnership = c.computerOwnership, roomIncludedID = c.roomIncludedID });
                    }
                    foreach (var r in rooms_refresh)
                    {
                        RoomList.Add(new Room { roomID = r.roomID, roomName = r.roomName, roomNumber = r.roomNumber });

                    }

                }
                #endregion
                #region INFO ABOUT COMPUTERSINROOM2CSV
                else if (selection == 'Y' || selection == 'y')
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Info about CSV");
                    Console.ResetColor();

                    Console.WriteLine(@"CSV files are included into same file path where your ComputerInRoom2CSV.exe
is included (for example C:\ComputerInRoom2CSV\ComputerInRoom2CSV.exe)");
                    Console.WriteLine($"File names are for computers: {computers_Source} || for rooms:  {room_Source}");
                    Console.WriteLine("ComputerInRoom2CSV fetches information from those files when you launch program and when you save\ncurrent data on CSV-file selecting X from main menu. When file is saved program will automaticly\nfetch information from saved files updating lists same time.");

                    ListMessages("AnyKey");
                    Console.ReadKey();

                }
                #endregion
                #region QUIT PROGRAM
                else if (selection == 'Q' || selection == 'q')
                {
                    ListMessages("Exit");

                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                #endregion
            }
            Console.ReadKey();
        }
        #region Methods
        // Method for headers
        static public string Heading(string heading)
            {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Welcome to: {heading}!");
            Console.ResetColor();

            return heading;
            }
            // Method for errors
            static public string Error(string errortype)
            {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR:");
            Console.ResetColor();
            Console.WriteLine($"{errortype}");

            return errortype;
            }
            static public string ListHeaders(int num, string header)
            {
            if (num == 1)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Current list of {header}");
                Console.ResetColor();
                return header;
            }
            else if (num == 2)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"-- {header} --");
            }
            else
            {
                Console.WriteLine($"Header {header} not found in header system, check method ListHeaders()");
                return header;
            }

            return header;

            }
            static public string ListMessages(string type)
            {
            if (type == "AnyKey")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Press any key to continue");
            }
            else if (type == "Exit")
            {
                Console.WriteLine($"Terminating program selected");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Press any key to exit");
            }
            else if (type == "Remove")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Select item you want to remove");
                Console.ResetColor();
            }
            else if (type == "Edit")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Select item you want to edit:");
                Console.ResetColor();

            }
            else if (type == "Add")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Give details to item you want to add");
                Console.ResetColor();

            }
            else if (type == "Select_")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Select ");
                Console.ResetColor();
            }
            else if (type == "Saved")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data saved successfully!");
                Console.ResetColor();

            }
            else if (type == "NotSaved")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Data was not saved!");
                Console.ResetColor();

            }
            else
            {
                Console.WriteLine("Message not found...");

            }
            return type;
            }

            static public string Labels(string type)
            { 
            if (type == "computerName_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("NAME ");
                Console.ResetColor();
            }
            else if (type == "computerBrand_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("BRAND ");
                Console.ResetColor();
            }
            else if (type == "computerModel_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("MODEL ");
                Console.ResetColor();
            }
            else if (type == "computerOwnership_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("OWNERSHIP STATUS ");
                Console.ResetColor();
            }
            else if (type == "computerID_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("ID ");
                Console.ResetColor();
            }
            else if (type == "roomID_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("ROOM ID ");
                Console.ResetColor();
            }
            else if (type == "roomName_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("NAME ");
                Console.ResetColor();
            }
            else if (type == "roomNumber_upper")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("NUMBER ");
                Console.ResetColor();
            }
            return type;
            }
        #endregion
    }
}