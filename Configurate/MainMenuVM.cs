
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using static Configurate.WindowSettingsVM;
using System.IO;
using System.Text.Encodings.Web;

namespace Configurate
{
    public class ConfigParams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
    }

    public class MainMenuVM : INotifyPropertyChanged
    {
        public bool isSave = false;
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public bool IsActive;
        public MainWindow mainWindow;

        private PlotModel _model;
        public PlotModel Model 
        {
            get => _model;
            set 
            {
                _model = value;
                OnPropertyChanged("Model");
            } 
        }

        public string ConfigPath;


        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        private string _weight;

        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;

                OnPropertyChanged("Weight");
            }
        }

        public string ConfStringToSend(ModelConfig data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return JsonSerializer.Serialize(data, options);
        }

        public MainMenuVM()
        {
            SaveManager.Init();

            Weight = "0";

            var graph = new GraphConfigure();

            var tmp = new PlotModel { Title = "pid" };

            this.Model = tmp;

            Apply = new DelegateCommand<string>(str =>
            {
                LoadConf();

                IsActive = false;

                Model = new PlotModel { Title = "Graph" };
                Model.ResetAllAxes();

                var config = Client.GetModelConfig();

                config.Kp = SaveManager.GetConfigSave(ConfigPath).Kp;
                config.Ki = SaveManager.GetConfigSave(ConfigPath).Ki;
                config.Kd = SaveManager.GetConfigSave(ConfigPath).Kd;
                config.Step = SaveManager.GetConfigSave(ConfigPath).Step;

                Client.GetModelConfig().modulesConf.Clear();
                Client.GetModelConfig().modulesConf.Add(new ResModule
                {
                    IsActive = ((ResModule)SaveManager.GetModuleSave(ConfigPath, ModuleTypes.ResModule)).IsActive,
                    ResW = ((ResModule)SaveManager.GetModuleSave(ConfigPath, ModuleTypes.ResModule)).ResW,
                    Type = ModuleTypes.ResModule,
                });

            });

            Settings = new DelegateCommand<string>(str =>
            {
                WindowSettingsOpen();
            });

            SendParams = new DelegateCommand<string>(str =>
            {
                Client.InitConnectAsync(this, ConfStringToSend(Client.GetModelConfig()) + "START");
                //IsActive = true;
                //ClickSendToServ();
            });

            StopParams = new DelegateCommand<string>(str =>
            {
                IsActive = false;
            });
        }

        public void LoadConf()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (openFileDialog.ShowDialog() == true)
            {
                ConfigPath = openFileDialog.FileName;

                SaveManager.LoadSave(ConfigPath);
            }
        }

        public void WindowSettingsOpen()
        {
            WindowSettings settings = new WindowSettings();

            if(ConfigPath != null)
            {
                SaveManager.FilePath = ConfigPath;
            }
            
            settings.Show();
        }

        public void ClickSaveConfigurate(string TextToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, TextToSave);
            }
        }

        public async void ClickSendToServ()
        {
            while (IsActive)
            {
                Client.InitConnectAsync(this, Weight + "WEIGHT");

                await Task.Delay(StepCheck(SaveManager.GetConfigSave(ConfigPath).Step));
            }
        }

        private int StepCheck(int step)
        {
            if(step < 5)
            {
                return step = 5;
            }
            else
            {
                return step;
            }
        }

        public void CreateSeries(int plotSeries, List<DataPoint> points)
        {
            if (plotSeries < Model.Series.Count) return;

            var newSeries = new LineSeries
            {
                Title = "Series " + plotSeries + 1,
                LineStyle = LineStyle.Automatic,
                ItemsSource = points
            };

            Debug.WriteLine("created" + plotSeries);
            Model.Series.Add(newSeries);

            IsActive = true;
        }

        public List<DataPoint> GetPoints(int plotSeries)
        {
            if (plotSeries < Model.Series.Count)
            {
                return (List<DataPoint>)(Model.Series[plotSeries] as LineSeries).ItemsSource;
            }

            return null;
        }

        public void AddToGraph(int plotSeries, Data data)
        {
            GetPoints(plotSeries).Add(new DataPoint(data.Time, data.Value));

            //Debug.WriteLine(GetPoints(plotSeries).Count + " / " + plotSeries + " / " + (Model.Series[plotSeries] as LineSeries).Points.Count);

            Model.InvalidatePlot(true);
        }

        public DelegateCommand<string> SendParams { get; }
        public DelegateCommand<string> StopParams { get; }
        public DelegateCommand<string> Settings { get; }
        public DelegateCommand<string> Apply { get; }
    }
}
