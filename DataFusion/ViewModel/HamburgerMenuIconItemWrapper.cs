using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;

namespace DataFusion.ViewModel
{
    public class HamburgerMenuIconItemWrapper : HamburgerMenuIconItem
    {
        public HamburgerMenuIconItemWrapper() : base()
        {

        }
        public Guid Id { get; set; }
    }
}
