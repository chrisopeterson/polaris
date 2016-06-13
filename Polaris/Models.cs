using System;

namespace Polaris
{
    class MenuItem
    {
        public string DisplayName { get; set; }
        public string Path { get; set; }
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public int Depth { get; set; } // used to make console printing nicer
    }
}
