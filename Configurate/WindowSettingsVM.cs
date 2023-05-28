using ControlzEx.Standard;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Configurate
{
    public enum ModuleTypes
    {
        ResModule,
        CompZeroModule,
        CompAsymModule
    }

    public class Module
    {
        public ModuleTypes Type { get; set; }
        public bool IsActive { set; get; }
    }

    public class WindowSettingsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public class ResModule : Module
        {
            public float ResW { set; get; }
        }

        public string ConfigPath;

        private string _pParam;
        public string Pparam
        {
            get { return _pParam; }
            set
            {
                _pParam = value;

                OnPropertyChanged(nameof(Pparam));
            }
        }

        private string _iParam;
        public string Iparam
        {
            get { return _iParam; }
            set
            {
                _iParam = value;

                OnPropertyChanged(nameof(Iparam));
            }
        }
        private string _dParam;
        public string Dparam
        {
            get { return _dParam; }
            set
            {
                _dParam = value;

                OnPropertyChanged(nameof(Dparam));
            }
        }

        private string _step;

        public string Step
        {
            get { return _step; }
            set
            {
                _step = value;

                OnPropertyChanged(nameof(Step));
            }
        }

        private bool _resCheck;
        public bool ResCheck
        {
            get { return _resCheck; }
            set
            {
                _resCheck = value;

                OnPropertyChanged(nameof(ResCheck));
            }
        }

        private string _resW;
        public string ResW
        {
            get { return _resW; }
            set
            {
                _resW = value;

                OnPropertyChanged(nameof(ResW));
            }
        }

        public WindowSettingsVM()
        {
            ConfigPath = SaveManager.FilePath;

            UpdProperties();

            LoadConfig = new DelegateCommand<string>(str =>
            {
                LoadConf();

                UpdProperties();
            });

            SaveConfigFile = new DelegateCommand<string>(str =>
            {
                float pram;
                float iram;
                float dram;
                int step;
                float resW;

                bool Psuccess = float.TryParse(Pparam, out pram);
                bool Isuccess = float.TryParse(Iparam, out iram);
                bool Dsuccess = float.TryParse(Dparam, out dram);
                bool StepSucceess = int.TryParse(Step, out step);

                bool ResWSucceess = float.TryParse(ResW, out resW);

                Client.GetModelConfig().Kp = pram;
                Client.GetModelConfig().Ki = iram;
                Client.GetModelConfig().Kd = dram;
                Client.GetModelConfig().Step = step;

                Client.GetModelConfig().modulesConf.Clear();
                Client.GetModelConfig().modulesConf.Add(new ResModule
                {
                    IsActive = ResCheck,
                    ResW = resW,
                    Type = ModuleTypes.ResModule,
                });


                SaveManager.SaveConfig(ConfigPath, Client.GetModelConfig());
            });

            SaveConfig = new DelegateCommand<string>(str =>
            {
                float pram;
                float iram;
                float dram;
                int step;
                float resW;

                bool Psuccess = float.TryParse(Pparam, out pram);
                bool Isuccess = float.TryParse(Iparam, out iram);
                bool Dsuccess = float.TryParse(Dparam, out dram);
                bool StepSucceess = int.TryParse(Step, out step);

                bool ResWSucceess = float.TryParse(ResW, out resW);

                Client.GetModelConfig().Kp = pram;
                Client.GetModelConfig().Ki = iram;
                Client.GetModelConfig().Kd = dram;
                Client.GetModelConfig().Step = step;

                Client.GetModelConfig().modulesConf.Clear();
                Client.GetModelConfig().modulesConf.Add(new ResModule
                {
                    IsActive = ResCheck,
                    ResW = resW,
                    Type = ModuleTypes.ResModule,
                });

                Debug.WriteLine(resW + " / "  + ResCheck);

                SaveManager.SaveConfig(ConfigPath, Client.GetModelConfig());
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

        public void UpdProperties()
        {
            var save = SaveManager.GetConfigSave(ConfigPath);

            Pparam = save.Kp.ToString();
            Iparam = save.Ki.ToString();
            Dparam = save.Kd.ToString();
            Step = save.Step.ToString();

            ResModule res = (ResModule)SaveManager.GetModuleSave(ConfigPath, ModuleTypes.ResModule);

            ResCheck = res.IsActive;
            ResW = res.ResW.ToString();
        }

        public DelegateCommand<string> LoadConfig { get; }
        public DelegateCommand<string> SaveConfigFile { get; }
        public DelegateCommand<string> SaveConfig { get; }
    }
}
