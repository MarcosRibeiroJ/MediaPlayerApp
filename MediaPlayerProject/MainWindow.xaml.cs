using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Threading;

namespace MediaPlayerProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            MediaFile.Play();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            MediaFile.Pause();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            MediaFile.Stop();
        }

        private void VolumeUpdate(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaFile.Volume = (double)volume.Value;
        }
        
        private DispatcherTimer timer;

        private void InitializeSliderValue(object sender, EventArgs e)
        {
            mediaPosition.Maximum = MediaFile.NaturalDuration.TimeSpan.TotalMilliseconds;
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500); // Define o intervalo de atualização

            // Define o evento que será chamado a cada intervalo
            timer.Tick += SliderPositionUpdate;

            // Inicia o timer
            timer.Start();
        }

        private void SliderPositionUpdate(object sender, EventArgs e)
        {
            // Verifica se a mídia está carregada e em reprodução
            if (MediaFile.NaturalDuration.HasTimeSpan && MediaFile.NaturalDuration.TimeSpan.TotalMilliseconds > 0 && MediaFile.Position.TotalMilliseconds > 0)
            {
                // Atualiza o valor do Slider com a posição atual da mídia
                mediaPosition.Value = MediaFile.Position.TotalMilliseconds;
            }
        }

        private void ChangeMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SliderValue = (int)mediaPosition.Value;
            TimeSpan newPosition = new TimeSpan(0, 0, 0, 0, SliderValue);
            MediaFile.Position = newPosition;
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.mp3;*.mp4;*.avi;*.mkv;*.mov;*.wmv";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MediaFile.Source = new Uri(filePath);
                MediaFile.Play();
            }
        }
    }
}
