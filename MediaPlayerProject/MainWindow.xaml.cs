﻿using System;
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

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaFile.Volume = (double)volume.Value;
        }
        private void ChangePosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaFile.Pause();
            double sliderValue = posicao.Value;
            TimeSpan mediaDuration = MediaFile.NaturalDuration.TimeSpan;
            TimeSpan newPosition = TimeSpan.FromTicks((long)(sliderValue * mediaDuration.Ticks));
            MediaFile.Play();

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
