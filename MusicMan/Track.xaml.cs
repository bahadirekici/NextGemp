using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MusicMan
{
    public sealed partial class Track : UserControl
    {
        private MusicProperties _musicProperties;
        public MediaElement Player { get; set; }
        public string Path { get; set; }
        public Track()
        {
            this.InitializeComponent();
            HorizontalContentAlignment=HorizontalAlignment.Stretch;
        }
        public Track(MusicProperties musicProp):this()
        {
            _musicProperties = musicProp;
            txtArtist.Text = _musicProperties.Artist;
            txtAlbum.Text = _musicProperties.Album;
            txtGenre.Text = _musicProperties.Duration.ToString();
            txtTrackName.Text = _musicProperties.Title;
        }

        public async void Play()
        {
            var file =await StorageFile.GetFileFromPathAsync(Path);
            var stream = await file.OpenReadAsync();
            Player.SetSource(stream,file.ContentType);
        }
    }
}
