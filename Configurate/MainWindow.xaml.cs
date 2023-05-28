using MahApps.Metro.Controls;
using OxyPlot;
using OxyPlot.Legends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


public enum ViewType
{
    PReg,
    IReg,
    DReg
}

namespace Configurate
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SaveManager.Init();
        }

        private void ConfWindowClick(object sender, RoutedEventArgs e)
        {
            ConfigWindow confWin = new ConfigWindow();
            confWin.Owner = this;
            confWin.Show();
        }
    }
}