using _01._ComputerInventory.Data;
using _01._ComputerInventory.Models;
using System;
using System.Linq;

namespace _01._ComputerInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            var result = -1;
            while (result != 9)
            {
                result = MainMenu();
            }


        }

        private static int MainMenu()
        {
            var result = -1;
            ConsoleKeyInfo cki;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Welcome to Newbie Data System");
                WriteHeader("Main Menu");
                Console.WriteLine("Please select from the list below for what you would like to do today");
                Console.WriteLine("1. List All Machines in Inventory");
                Console.WriteLine("2. List All Operating Systems");
                Console.WriteLine("3. Data Entry Menu");
                Console.WriteLine("4. Data Modification Menu");
                Console.WriteLine("9. Exit");
                cki = Console.ReadKey();
                Console.WriteLine();

                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    switch (result)
                    {
                        case 1:

                            break;
                        case 2:
                            DisplayOperatingSystems();
                            break;
                        case 3:
                            DataEntryMenu();
                            break;
                        case 4:
                            DataModificationMenu();
                            break;
                        case 9:
                            cont = true;
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please type in one of the required numbers.");
                }
            } while (!cont);

            return result;
        }

        private static void DataModificationMenu()
        {
            ConsoleKeyInfo cki;
            int result = -1;
            bool cont = false;
            do
            {
                Console.Clear();

                WriteHeader("Data Modification Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you would like to do today");
                Console.WriteLine("1. Delete Operating System");
                Console.WriteLine("2. Modify Operating System");
                Console.WriteLine("3. Delete All Unsupported Operating Systems");
                Console.WriteLine("9. Exit Menu");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        SelectOperatingSystem("Delete");
                    }
                    else if (result == 2)
                    {
                        SelectOperatingSystem("Modify");
                    }
                    else if (result == 3)
                    {
                        DeleteAllUnsupportedOperatingSystems();
                    }
                    else if (result == 9)
                    {
                        // We are exiting so nothing to do
                        cont = true;
                    }
                }
                catch (System.FormatException)
                {
                    // a key that wasn't a number
                }
            } while (!cont);
        }

        static void ModifyOperatingSystem(int id)
        {
            OperatingSys os = GetOperatingSystemById(id);
            Console.Clear();
            char operation = '0';
            bool cont = false;
            ConsoleKeyInfo cki;
            WriteHeader("Update Operating System");
            if (os != null)
            {
                Console.WriteLine($"\r\nOS Name: {os.Name} Still Supported: { os.StillSupported}");
                Console.WriteLine("To modify the name press 1\r\nTo modify if the OS is Still Supported press 2");
                Console.WriteLine("Hit Esc to exit this menu");
                do
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        cont = true;
                    else
                    {
                        if (char.IsNumber(cki.KeyChar))
                        {
                            if (cki.KeyChar == '1')
                            {
                                Console.WriteLine("Updated Operating System Name: ");
                                operation = '1';
                                cont = true;
                            }
                            else if (cki.KeyChar == '2')
                            {
                                Console.WriteLine("Update if the OS is Still Supported [y or n]: ");
                                operation = '2';
                                cont = true;
                            }
                        }
                    }
                } while (!cont);
            }
            if (operation == '1')
            {
                string osName;
                cont = false;
                do
                {
                    osName = Console.ReadLine();
                    if (osName.Length >= 4)
                    {
                        cont = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid OS name of at least 4 characters.\r\nPress and key to continue...");
                        Console.ReadKey();
                    }
                } while (!cont);
                os.Name = osName;
            }
            else if (operation == '2')
            {
                string k;
                do
                {
                    cki = Console.ReadKey(true);
                    k = cki.KeyChar.ToString();
                    cont = ValidateYourN(k);
                } while (!cont);

                if (k == "y")
                {
                    os.StillSupported = true;
                }
                else
                {
                    os.StillSupported = false;
                }
            }
            using (var context = new MachineContext())
            {
                var o = context.OperatingSys.FirstOrDefault(i => i.OperatingSysId == os.OperatingSysId);
                if (o != null)
                {
                    o.Name = os.Name;
                    o.StillSupported = os.StillSupported;
                    Console.WriteLine("\r\nUpdating the database...");
                    context.SaveChanges();
                    Console.WriteLine("Done!\r\nHit any key to continue...");
                }
            }

            Console.ReadKey();
        }

        static void DeleteAllUnsupportedOperatingSystems()
        {
            using (var context = new MachineContext())
            {
                var os = context.OperatingSys.Where(o => o.StillSupported == false);
                Console.WriteLine("\r\nDeleting all Unsupported Operating Systems...");
                context.OperatingSys.RemoveRange(os);
                int i = context.SaveChanges();
                Console.WriteLine($"We have deleted {i} records");
                Console.WriteLine("Hit any key to continue...");
                Console.ReadKey();
            }
        }

        private static void DeleteOperatingSystem(int id)
        {
            OperatingSys os = GetOperatingSystemById(id);
            if (os != null)
            {
                Console.WriteLine($"\r\nAre you sure you want to delete {os.Name}?[y or n]");
                ConsoleKeyInfo cki;
                string result;
                bool cont;
                do
                {
                    cki = Console.ReadKey(true);
                    result = cki.KeyChar.ToString();
                    cont = ValidateYourN(result);
                } while (!cont);
                if ("y" == result.ToLower())
                {
                    Console.WriteLine("\r\nDeleting record");
                    using (var context = new MachineContext())
                    {
                        context.Remove(os);
                        context.SaveChanges();
                    }

                    Console.WriteLine("Record Deleted");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Delete Aborted\r\nHit any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\r\nOperating System Not Found!");
                Console.ReadKey();
                SelectOperatingSystem("Delete");
            }
        }

        private static void SelectOperatingSystem(string operation)
        {
            ConsoleKeyInfo cki;
            Console.Clear();
            WriteHeader($"{operation} an Existing Operating System Entry");
            Console.WriteLine($"{"ID",-7}|{"Name",-50}|Still Supported");
            Console.WriteLine("-------------------------------------- -----------");
            using (var context = new MachineContext())
            {
                var lOperatingSys = context.OperatingSys.ToList();
                foreach (var os in lOperatingSys)
                {
                    Console.WriteLine($"{os.OperatingSysId,-7}|{os.Name,-50}|{os.StillSupported}");
                }
            }

            Console.WriteLine($"\r\nEnter the ID of the record you wish to { operation} and hit Enter\r\nYou can hit Esc to exit this menu");
            bool cont = false;
            string id = "";
            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                {
                    cont = true;
                    id = "";
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (id.Length > 0)
                    {
                        cont = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter an ID that is at least 1 digit.");
                    }
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b \b");
                    try
                    {
                        id = id.Substring(0, id.Length - 1);
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        // at the 0 position, can't go any further back
                    }
                }
                else
                {
                    if (char.IsNumber(cki.KeyChar))
                    {
                        id += cki.KeyChar.ToString();
                        Console.Write(cki.KeyChar.ToString());
                    }
                }
            } while (!cont);
            int osId = Convert.ToInt32(id);
            if ("Delete" == operation)
            {
                DeleteOperatingSystem(osId);
            }
            else if ("Modify" == operation)
            {
                ModifyOperatingSystem(osId);
            }
        }

        private static OperatingSys GetOperatingSystemById(int id)
        {
            var context = new MachineContext();
            var os = context.OperatingSys.FirstOrDefault(o => o.OperatingSysId == id);
            return os;
        }

        private static void DisplayOperatingSystems()
        {
            using (var context = new MachineContext())
            {
                foreach (var os in context.OperatingSys.ToList())
                {
                    if (os.StillSupported == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write($"Name: {os.Name,-40}\tStill Supported = ");
                    Console.WriteLine(os.StillSupported);
                }
            }

            Console.WriteLine("\r\nAny key to continue...");
            Console.ReadKey();
        }

        private static void WriteHeader(string headerText)
        {
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + headerText.Length / 2) + "}", headerText));
        }

        private static bool ValidateYourN(string entry)
        {
            bool result = false;
            if (entry.ToLower() == "y" || entry.ToLower() == "n")
            {
                result = true;
            }
            return result;
        }

        static void DataEntryMenu()
        {
            ConsoleKeyInfo cki;
            int result = -1;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Data Entry Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you would like to do today");
                Console.WriteLine("1. Add a New Machine");
                Console.WriteLine("2. Add a New Operating System");
                Console.WriteLine("3. Add a New Warranty Provider");
                Console.WriteLine("9. Exit Menu");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        //AddMachine();
                    }
                    else if (result == 2)
                    {
                        AddOperatingSystem();
                    }
                    else if (result == 3)
                    {
                        //AddNewWarrantyProvider();
                    }
                    else if (result == 9)
                    {
                        // We are exiting so nothing to do
                        cont = true;
                    }
                }
                catch (System.FormatException)
                {
                    // a key that wasn't a number
                }
            } while (!cont);
        }

        private static bool CheckForExistingOS(string osName)
        {
            bool exists = false;
            using (var context = new MachineContext())
            {
                var os = context.OperatingSys.Where(o => o.Name == osName).Count();
                if (os > 0)
                {
                    exists = true;
                }
            }

            return exists;
        }

        private static void AddOperatingSystem()
        {
            Console.Clear();
            ConsoleKeyInfo cki;
            string result;
            bool cont = false;
            var os = new OperatingSys();
            var osName = string.Empty;
            do
            {
                WriteHeader("Add new Operating System");
                Console.WriteLine("Enter the name of new operating sys and hit enter");
                osName = Console.ReadLine();
                if (osName.Length > 4)
                {
                    cont = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid OS name of at least 4 characters.\r\nPress and key to continue...");
                    Console.ReadKey();
                }
            } while (!cont);

            cont = false;
            os.Name = osName;
            Console.WriteLine("Is the Operating System still supported? [y or n]");

            do
            {
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYourN(result);
            } while (!cont);

            if (result.ToLower() == "y")
            {
                os.StillSupported = true;
            }
            else
            {
                os.StillSupported = false;
            }

            cont = false;

            do
            {
                Console.Clear();
                Console.WriteLine($"You entered {os.Name} as the Operating System Name\r\nIs the OS still supported, you entered {os.StillSupported}.\r\nDo you wish to continue? [y or n]");
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYourN(result);
            } while (!cont);
            if (result.ToLower() == "y")
            {
                bool exists = CheckForExistingOS(os.Name);
                if (exists)
                {
                    Console.WriteLine("\r\nOperating System already exists in the database\r\nPress any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    using (var contex = new MachineContext())
                    {
                        Console.WriteLine("\r\nAttempting to save changes...");
                        contex.OperatingSys.Add(os);
                        var i = contex.SaveChanges();
                        if (i == 1)
                        {
                            Console.WriteLine("Contents Saved\r\nPress any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
    }
}
