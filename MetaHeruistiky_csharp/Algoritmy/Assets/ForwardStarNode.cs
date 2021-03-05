using System;
using System.Collections.Generic;
using System.Text;

namespace MetaHeruistiky_csharp.Algoritmy.Assets
{
    class ForwardStarNode
    {
        public int ID { get; set; }
        public int StartNode { get; set; }
        public ForwardStarNode StartNodeRef { get; set; } = null;
        public int EndNode { get; set; }
        public ForwardStarNode EndNodeRef { get; set; } = null;
        public double Distance { get; set; }
    }
}
