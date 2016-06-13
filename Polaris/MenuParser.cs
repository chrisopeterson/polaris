using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Polaris
{
    class MenuParser
    {
        private int _MenuId { get; set; }
        private int NextId()
        {
            _MenuId++;
            return _MenuId;
        }

        public void Parse(XDocument doc, string displayName)
        {
            var topLevelMenuItems = doc.Root.Elements("item").ToList();

            // Starting at the top, recursively parse all the menu items
            var results = ParseMenuItems(topLevelMenuItems, 0 , 0);

            // Get menu ids of the active menu items
            var activeMenuIds = GetActiveMenuIds(results, displayName);

            PrintResults(results, activeMenuIds);
        }

        /// <summary>
        /// Converts list of xml elements into Menu Items
        /// </summary>
        private List<MenuItem> ParseMenuItems(List<XElement> menuItems, int parentId, int depth)
        {
            List<MenuItem> items = new List<MenuItem>();

            foreach(XElement menu in menuItems) {
                var id = NextId();

                items.Add( new MenuItem {
                    DisplayName = menu.Element("displayName").Value,
                    Path = menu.Element("path").Attribute("value").Value,
                    MenuId = id,
                    ParentMenuId = parentId,
                    Depth = depth
                });

                // Recursively parse submenu as well if that exists
                if (menu.Element("subMenu") != null)
                {
                    var subMenuElements = menu.Element("subMenu").Elements("item").ToList();
                    items.AddRange(ParseMenuItems(subMenuElements, id, depth+1));
                }
            }

            return items;
        }

        /// <summary>
        /// Returns list of active menu ids
        /// </summary>
        private List<int> GetActiveMenuIds(List<MenuItem> menuItems, string displayName)
        {
            var activeMenuItem = menuItems.SingleOrDefault(i => i.DisplayName == displayName);

            // If name has no match then none are active
            if (activeMenuItem == null)
            {
                return new List<int>();
            }

            return GetMenuHierarchy(menuItems, activeMenuItem.MenuId);
        }

        /// <summary>
        /// Returns list of menu ids for given menu id and its parents
        /// </summary>
        private List<int> GetMenuHierarchy(List<MenuItem> menuItems, int menuId)
        {
            List<int> hierarchy = new List<int>();

            var item = menuItems.Where(i => i.MenuId == menuId).SingleOrDefault();

            // If item is not null then there are more parent items,  recursively check for this menu item's parent
            if (item != null)
            {
                hierarchy.Add(item.MenuId);
                hierarchy.AddRange(GetMenuHierarchy(menuItems, item.ParentMenuId));
            }

            return hierarchy;
        }

        /// <summary>
        /// Prints results of XML Parse to console with formatting
        /// </summary>
        private void PrintResults(List<MenuItem> menuItems, List<int> activeMenuIds)
        {
            foreach(MenuItem item in menuItems) 
            {
                string tabs = "";
                string active = activeMenuIds.Contains(item.MenuId) ? "**ACTIVE**" : "";

                // set tab length based on depth
                for(int x = 0; x < item.Depth; x++) {
                    tabs += "\t";
                }

                Console.WriteLine(tabs + item.DisplayName + " - " + item.Path + " " + active);
            }
  
        }

    }
}
