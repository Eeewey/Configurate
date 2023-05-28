using OxyPlot;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using static Configurate.WindowSettingsVM;

namespace Configurate
{
    public class ModelConfig
    {
        public float Kp { get; set; }
        public float Ki { get; set; }
        public float Kd { get; set; }
        public int Step { get; set; }
        public List<object> modulesConf {  get; set; }
    }

    public class Data
    {
        public float Time { get; set; }
        public double Value { get; set; }
    }

    public class Responce
    {
        public Data pidRes { get; set; }
        public Data interRes { get; set; }
    }

    public class Client : BindableBase
    {
        private static string text;
        private static ModelConfig _confData;
        public static Responce Result;

        public static List<DataPoint> pidRes = new List<DataPoint>();
        public static List<DataPoint> interRes = new List<DataPoint>();

        public static ModelConfig CreateModelConfig(float kp, float ki, float kd, int step, List<object> modules)
        {
            if(_confData == null) 
            {
                _confData = new ModelConfig()
                {
                    Kp = kp,
                    Ki = ki,
                    Kd = kd,
                    Step = step,
                };

                _confData.modulesConf = new List<object>
                {
                    new ResModule
                    {
                        IsActive = false,
                        ResW = 0,
                        Type = ModuleTypes.ResModule,
                    }
                };
            }
            else
            {
                _confData.Kp = kp;
                _confData.Ki = ki;
                _confData.Kd = kd;
                _confData.Step = step;

                _confData.modulesConf.Clear();
                foreach (var module in modules)
                {
                    _confData.modulesConf.Add(module);
                }
            }

            return _confData;
        }

        public static ModelConfig GetModelConfig()
        {
            if (_confData == null)
            {
                var list = new List<object> {
                    new ResModule
                    {
                        IsActive = false,
                        ResW = 0,
                        Type = ModuleTypes.ResModule,
                    }
                };
            
                CreateModelConfig(0, 0, 0, 0, list);
            }

            return _confData;
        }

        public static Module GetModuleConfig(ModuleTypes type)
        {
            foreach (var module in GetModelConfig().modulesConf)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                Module mod = new Module();

                switch (type)
                {
                    case ModuleTypes.ResModule:
                        Debug.WriteLine(module.ToString() + " / module");
                        mod = JsonSerializer.Deserialize<ResModule>(module.ToString(), options);
                        break;
                }

                return mod;
            }

            return null;
        }

        public static async void InitConnectAsync(MainMenuVM enter, string messgToSend)
        {
            Result = null;

            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 8888);
            var stream = tcpClient.GetStream();

            var response = new List<byte>();
            int bytesRead = 10;


            byte[] data = Encoding.UTF8.GetBytes(messgToSend + '\n');

            await stream.WriteAsync(data, 0, data.Length);

            while ((bytesRead = stream.ReadByte()) != '\n')
            {
                response.Add((byte)bytesRead);
            }

            text = Encoding.UTF8.GetString(response.ToArray());

            if(messgToSend.EndsWith("START"))
            {
                Debug.WriteLine(messgToSend + " ответ - " + text);

                enter.IsActive = true;

                enter.CreateSeries(0, pidRes);
                enter.CreateSeries(1, interRes);
            }

            if (messgToSend.EndsWith("WEIGHT")) 
            {
                Result = JsonSerializer.Deserialize<Responce>(text);

                //Debug.WriteLine(messgToSend + " ответ - " + Result);


                //Debug.WriteLine(Result.pidRes.Time + " / " + Result.pidRes.Value);


                enter.AddToGraph(0, Result.pidRes);
                enter.AddToGraph(1, Result.interRes);

                response.Clear();
            }

            await stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"), 0, Encoding.UTF8.GetBytes("END\n").Length);
        }
    }
}
