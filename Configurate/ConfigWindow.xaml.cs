using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

public interface IConfWindowCodeBehind
{
    CheckBox? GetCheckBox(ViewType type);
    void UpdateGraph();
}

namespace Configurate
{

    public partial class ConfigWindow : MetroWindow, IConfWindowCodeBehind
    {
        public ConfigWindow()
        {
            InitializeComponent();

            Loaded += ConfigWindow_Loaded;
        }

        public CheckBox? GetCheckBox(ViewType type)
        {
            switch (type)
            {
                case ViewType.PReg:
                    return checkBox0;
                case ViewType.IReg:
                    return checkBox1;
                case ViewType.DReg:
                    return checkBox2;
                default:
                    return null;
            }
        }

        public void UpdateGraph()
        {
            //(Owner as MainWindow).GraphUpd();
        }

        private void ConfigWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigWindowVM vm = new ConfigWindowVM();

            vm.CodeBehind = this;
            DataContext = vm;
        }
    }
}
