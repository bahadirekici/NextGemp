using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicMan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MediaElement _currentMediaElement = new MediaElement();
        ObservableCollection<Track> _currentMp3List = new ObservableCollection<Track>();
        public MainPage()
        {
            this.InitializeComponent();
            lbMp3.ItemsSource = _currentMp3List;
           _currentMediaElement.AudioCategory=AudioCategory.BackgroundCapableMedia;

           MediaControl.PlayPressed += MediaControl_PlayPressed;
           MediaControl.PausePressed += MediaControl_PausePressed;
           MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
           MediaControl.StopPressed += MediaControl_StopPressed;
            //this is test

        }
        void MediaControl_StopPressed(object sender, object e)
        {
            _currentMediaElement.Stop();
        }

        void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
        }

        void MediaControl_PausePressed(object sender, object e)
        {
            _currentMediaElement.Pause();
        }

        void MediaControl_PlayPressed(object sender, object e)
        {
            _currentMediaElement.Play();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //for (int i = 0; i < 200; i++)
            //{
            //    var mp3Item = new Track();
            //    lbMp3.Items.Add(mp3Item);
            //}
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.ViewMode = PickerViewMode.List;
            filePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            filePicker.SettingsIdentifier = "Mp3Picker";
            filePicker.CommitButtonText = "Select Files";

            var selectedFiles = await filePicker.PickMultipleFilesAsync();
            if (selectedFiles != null)
            {
                foreach (var selectedFile in selectedFiles)
                {
                    var id = selectedFile.FolderRelativeId.Substring(0, selectedFile.FolderRelativeId.IndexOf('\\'));
                    if (!StorageApplicationPermissions.FutureAccessList.ContainsItem(id))
                        StorageApplicationPermissions.FutureAccessList.AddOrReplace(id, selectedFile);
                    var musicProp = await selectedFile.Properties.GetMusicPropertiesAsync();
                    var track = new Track(musicProp) { Player = _currentMediaElement, Path = selectedFile.Path };
                    _currentMp3List.Add(track);
                }
            }
        }
        void Compll(IAsyncOperation<MusicProperties> handler, AsyncStatus statusc)
        {
            if (statusc == AsyncStatus.Completed)
            {
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var music = handler.GetResults();

                });

            }
        }

        private void lbMp3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var track = (Track)lbMp3.SelectedItem;
            track.Play();
        }
    }
}
