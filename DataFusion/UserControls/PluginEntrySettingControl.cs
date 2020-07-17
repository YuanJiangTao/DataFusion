using DataFusion.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        public ObservableCollection<PluginEntryViewModel> PluginEntryVms
        {
            get { return (ObservableCollection<PluginEntryViewModel>)GetValue(PluginEntryVmsProperty); }
            set { SetValue(PluginEntryVmsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PluginCatalogEntrySg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PluginEntryVmsProperty =
            DependencyProperty.Register("PluginEntryVms", typeof(ObservableCollection<PluginEntryViewModel>), typeof(PluginEntrySettingControl), new PropertyMetadata());

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }





    }
}
