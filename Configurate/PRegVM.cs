using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Configurate
{
    public class PRegVM
    {
        /*private Client client;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        public bool isSave = false;

        private string _pParam;
        public string Pparam
        {
            get { return _pParam; }
            set
            {
                _pParam = value;
                float pram;

                bool Psuccess = float.TryParse(value, out pram);

                Client.GetPidData().Kp = pram;

                if (isSave)
                {
                    SaveManager.SavePidData(Client.GetPidData());
                }
            }
        }
        private string _iParam;
        public string Iparam
        {
            get { return _iParam; }
            set
            {
                _iParam = value;
                float iram;

                bool Isuccess = float.TryParse(value, out iram);

                Client.GetPidData().Ki = iram;

                if (isSave)
                {
                    SaveManager.SavePidData(Client.GetPidData());
                }
            }
        }
        private string _dParam;
        public string Dparam
        {
            get { return _dParam; }
            set
            {
                _dParam = value;
                float dram;

                bool Dsuccess = float.TryParse(value, out dram);

                Client.GetPidData().Kd = dram;

                if (isSave)
                {
                    SaveManager.SavePidData(Client.GetPidData());
                }
            }
        }
        private string _weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;

                float weight;
                
                bool Wsuccess = float.TryParse(value, out weight);
                Client.GetPidData().Weight = weight;

                if (isSave)
                {
                    SaveManager.SavePidData(Client.GetPidData());
                }
            }
        }      


        public string ConfStringToSend(PidData data)
        {
            Debug.WriteLine("ser / " + JsonSerializer.Serialize(data));
            return JsonSerializer.Serialize(data);
        }

        public PRegVM()
        {
            
            SaveManager.LoadSave();

            Pparam = SaveManager.GetPidDataSave().Kp.ToString();
            Iparam = SaveManager.GetPidDataSave().Ki.ToString();
            Dparam = SaveManager.GetPidDataSave().Kd.ToString();
            Weight = SaveManager.GetPidDataSave().Weight.ToString();

            isSave = true;


            SaveManager.SavePidData(Client.GetPidData());
            

            SendParams = new DelegateCommand<string>(str => {
                //Client.InitConnectAsync(this, ConfStringToSend(Client.GetPidData()));
            });
        }

        public DelegateCommand<string> SendParams { get; }*/
    }
}
