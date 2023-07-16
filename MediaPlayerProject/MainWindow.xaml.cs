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
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;

namespace MediaPlayerProject
{
    public partial class MainWindow : Window
    {
        private const double MaxSpeed = 2.0;
        private const double MinSpeed = 0.5;
        private const double SpeedValue = 0.25;
        DispatcherTimer? timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            MediaFile.Play();
            timer?.Start();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            MediaFile.Pause();
            timer?.Stop();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            MediaFile.Stop();
            MediaFile.Position = TimeSpan.Zero;
            timer?.Stop();
            position.Value = 0;
        }

        private void FastForward(object sender, RoutedEventArgs e)
        {
            if(MediaFile.SpeedRatio < MaxSpeed)
                MediaFile.SpeedRatio += SpeedValue;

            LabelUpdate();
        }

        private void Rewind(object sender, RoutedEventArgs e)
        {
            if(MediaFile.SpeedRatio > MinSpeed)
                MediaFile.SpeedRatio -= SpeedValue;
            
            LabelUpdate();
        }

        private void LabelUpdate()
        {
            speed.Content = $"{MediaFile.SpeedRatio.ToString("F2")}x";
        }

        private void VolumeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaFile.Volume = (double)volume.Value;
        }

        private void UpdateSliderPosition(object? sender, EventArgs e)
        {
            position.Value = MediaFile.Position.TotalSeconds;
        }
        private void UpdateMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaFile.Position = TimeSpan.FromSeconds(position.Value);
        }
        private void DragStarted(object sender, DragStartedEventArgs e)
        {
            MediaFile.Pause();
            timer?.Stop();
        }

        private void DragCompleted(object sender, DragCompletedEventArgs e)
        {
            MediaFile.Play();
            timer?.Start();
        }

        private void MediaOpened(object sender, RoutedEventArgs e)
        {
            //Método que inicia o timer e configura seu intervalo de 1 em 1 segundo
            //Atualiza o valor do timer com a posicao atual do slider
            //Exibe no label o tempo total da mídia carregada
            position.Maximum = MediaFile.NaturalDuration.TimeSpan.TotalSeconds;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateSliderPosition;
            timer.Start();

            totalMediaTime.Content = MediaFile.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");    
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
                LabelUpdate();
            }
        }

        private void MediaError(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("O arquivo selecionado não é suportado", "Erro ao carregar arquivo", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
