using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;

namespace fMod.Pages
{
    /// <summary>
    /// Interaction logic for BrowseMods.xaml
    /// </summary>
    public partial class BrowseCategories : UserControl
    {

        public BrowseCategories()
        {
            InitializeComponent();

            LoadCategories();

        }

        private async void LoadCategories()
        {
            var loader = (ModLoader) ModernListCategories.ContentLoader;
            try
            {
                ModernListCategories.Links = await loader.GetCategories();
                ModernListCategories.SelectedSource = ModernListCategories.Links.Select(l => l.Source).FirstOrDefault();
            }
            catch (Exception e)
            {
                ModernDialog.ShowMessage(e.Message, "Failure", MessageBoxButton.OK);
            }
        }
    }
}
