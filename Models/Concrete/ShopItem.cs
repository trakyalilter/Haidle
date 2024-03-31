using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGAME.Models.Concrete;

namespace WPFGAME.Models
{
    public class ShopItem
    {
        public ShopItemEnum ShopItemEnum { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
