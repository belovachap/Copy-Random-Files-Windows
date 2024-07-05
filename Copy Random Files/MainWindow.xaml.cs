using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Copy_Random_Files
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var files = new List<string>(
                Directory.GetFiles(SourceFolderTextBlock.Text, "*.*", SearchOption.AllDirectories));
            var rand = new Random();
            var randFiles = new List<string>();
            for (int i = 0; i < NumberOfFilesValue.Value; i++)
            {
                var next = rand.Next(files.Count);
                randFiles.Add(files.ElementAt(next));
                System.Diagnostics.Debug.WriteLine(files.ElementAt(next));
                files.RemoveAt(next);
            }

            // Copy randomly selected files
            foreach (var file in randFiles)
            {
                System.IO.File.Copy(file,
                    Path.Combine(DestinationFolderTextBlock.Text, Path.GetFileName(file)), true);
            }
        }

        private async void PickSourceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            SourceFolderTextBlock.Text = "No Source Folder Selected";

            // Create a folder picker
            FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = this;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the folder picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your folder picker
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a folder
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                SourceFolderTextBlock.Text = folder.Path;
            }
            else
            {
                SourceFolderTextBlock.Text = "No Source Folder Selected";
            }
        }

        private async void PickDestinationFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            DestinationFolderTextBlock.Text = "No Destination Folder Selected";

            // Create a folder picker
            FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = this;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the folder picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your folder picker
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a folder
            StorageFolder folder = await openPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                DestinationFolderTextBlock.Text = folder.Path;
            }
            else
            {
                DestinationFolderTextBlock.Text = "No Destination Folder Selected";
            }
        }
    }
}
