using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataFusion.UserControls
{
    public class ExpanderExtensControl : Control
    {
        static ExpanderExtensControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderExtensControl), new FrameworkPropertyMetadata(typeof(ExpanderExtensControl)));
        }
        public ExpanderExtensControl()
        {

        }

    }
}
