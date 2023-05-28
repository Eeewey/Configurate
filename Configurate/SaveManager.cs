using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.IO.Packaging;
using System.Collections.ObjectModel;
using static Configurate.WindowSettingsVM;
using System.Text.Encodings.Web;

namespace Configurate
{
    public class ConfigsSave
    {
        public ObservableCollection<ConfigParams>? ConfigsDatas { get; set; }
        public ConfigParams ActualConf { get; set; }
    }

    public class SaveData
    {
        public ModelConfig configData { get; set; }
    }

    public class SaveManager
    {
        public static SaveData? Saves;

        public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "save.txt";

        public static void Init()
        {
            if (Saves == null)
            {
                Saves = new SaveData();
                Saves.configData = new ModelConfig
                {
                    Kp = 0,
                    Ki = 0,
                    Kd = 0,
                    Step = 0
                };

                Saves.configData.modulesConf = new List<object>
                {
                    new ResModule
                    {
                        IsActive = false,
                        ResW = 0,
                        Type = ModuleTypes.ResModule,
                    }
                };
            }
        }

        public static void SaveConfig(string pathSave, ModelConfig data)
        {
            if (Saves.configData != null)
            {
                Saves.configData.Kp = data.Kp;
                Saves.configData.Ki = data.Ki;
                Saves.configData.Kd = data.Kd;
                Saves.configData.Step = data.Step;

                Saves.configData.modulesConf.Clear();

                foreach(var kvp in data.modulesConf)
                {
                    Saves.configData.modulesConf.Add(kvp);
                }

                Debug.WriteLine(Saves.configData.modulesConf + " / " + Saves.configData.modulesConf.Count);
            }
            else
            {
                Saves.configData = new ModelConfig
                {
                    Kp = data.Kp,
                    Ki = data.Ki,
                    Kd = data.Kd,
                    Step = data.Step,
                };

                Saves.configData.modulesConf = new List<object>();

                foreach (var kvp in data.modulesConf)
                {
                    Saves.configData.modulesConf.Add(kvp);
                }

                //Debug.WriteLine(Saves.configData.modulesConf + " / " + Saves.configData.modulesConf.Count);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            Debug.WriteLine(JsonSerializer.Serialize(Saves, options));
            WriteToSaveFile(pathSave,JsonSerializer.Serialize(Saves, options));
        }


        public static string GetSavePidConf()
        {
            return JsonSerializer.Serialize(Saves);
        }

        public static void WriteToSaveFile(string pathSave,string textSave)
        {
            try
            {
                StreamWriter sw = new StreamWriter(pathSave, false);
            
                sw.WriteLine(textSave);

                sw.Close();
            }
            catch
            {
                Debug.WriteLine("save write error");
            }
        }

        public static ModelConfig GetConfigSave(string savePath)
        {
            
            LoadSave(savePath);

            return Saves.configData;
        }

        public static Module GetModuleSave(string savePath, ModuleTypes type) 
        {
            var conf = GetConfigSave(savePath);

            foreach (var module in conf.modulesConf) 
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
                        Debug.WriteLine(module.ToString() + " / module + save");
                        mod = JsonSerializer.Deserialize<ResModule>(module.ToString(), options);
                        break;
                }

                return mod;
            }

            return null;
        }

        public static bool LoadSave(string filePath)
        {
            var text = "";
            var lett = "";

            try
            {
                if (!File.Exists(filePath))
                {
                    var newConf = new ModelConfig
                    {
                        Kp = 0,
                        Ki = 0,
                        Kd = 0,
                        Step = 0,
                        modulesConf = new List<object>()
                    };

                    newConf.modulesConf.Add(new ResModule
                    {
                        IsActive = false,
                        ResW = 0,
                        Type = ModuleTypes.ResModule,
                    });


                    SaveConfig(filePath, newConf);
                }

                using (StreamReader sr = new StreamReader(filePath))
                {
                    lett = sr.ReadLine();

                    while (lett != null)
                    {
                        text += lett;
                        lett = sr.ReadLine();
                    }

                    sr.Close();


                    Saves = JsonSerializer.Deserialize<SaveData>(text);

                    return true;
                }

                
            }
            catch
            {
                Debug.WriteLine("save doesnt read");
                return false;
            }
        }
    }
}
