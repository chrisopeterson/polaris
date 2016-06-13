using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace Polaris
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get console arguments and confirm they are valid
            //
            string xmlFilePath = args.ElementAtOrDefault(0);
            string displayName = args.ElementAtOrDefault(1);

            if (string.IsNullOrEmpty(xmlFilePath) || string.IsNullOrEmpty(displayName))
            {
                Console.WriteLine("Path to XML file and menu name must be provided");
                return;
            }

            if(!File.Exists(xmlFilePath)) {
                Console.WriteLine("XML File cannot be opened or does not exist.");
                return;
            }

            // Load file into an XML object and pass to menu parser to read and display results
            //
            string xml = File.ReadAllText(xmlFilePath);
            XDocument doc = XDocument.Parse(xml);
            MenuParser menuParse = new MenuParser();
            menuParse.Parse(doc, displayName);

            // End application
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
