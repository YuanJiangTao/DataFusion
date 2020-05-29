using DataFusion.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DataFusion.UserControls
{
    public class PluginEntrySettingControl : Control
    {
        static PluginEntrySettingControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PluginEntrySettingControl), new FrameworkPropertyMetadata(typeof(PluginEntrySettingControl)));
        }

        public PluginEntrySettingControl()
        {
        }


        public List<PluginEntryViewModel> PluginEntryVms
        {
            get { return (List<PluginEntryViewModel>)GetValue(PluginEntryVmsProperty); }
            set { SetValue(PluginEntryVmsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PluginCatalogEntrySg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PluginEntryVmsProperty =
            DependencyProperty.Register("PluginEntryVms", typeof(List<PluginEntryViewModel>), typeof(PluginEntrySettingControl), new PropertyMetadata());

        private ItemsControl _pluginEntriesControl;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }





    }
}
