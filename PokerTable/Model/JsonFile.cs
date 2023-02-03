using DecisionMaking;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace PokerTable.Model
{
    public class JsonFile
    {
        #region Methods
        public static void SavingFileJson(List<DecisionState> decisionStates, string path)
        {
            try
            {
                string json = "";
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    json = JsonConvert.SerializeObject(decisionStates);
                });
                SaveFileDialog saveFileDialog = new();
                saveFileDialog.DefaultExt = "json";
                bool? resultDialog = saveFileDialog.ShowDialog();
                if (resultDialog == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, json);
                    path = saveFileDialog.FileName;
                    if (!File.Exists(path))
                    {
                        using (var h = File.Create(path));
                    }
                    MessageBox.Show("Saved!");
                }
            }
            catch (Exception ex)
            {
                Singleton.Log("Exception in saving Json file: " + ex.ToString(), LogLevel.Error);
            }
        }
        public static List<DecisionState> LoadingFileJson(ref string pathName)
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                pathName = openFileDialog.FileName;
            }
            string json = File.ReadAllText(pathName);
            return JsonConvert.DeserializeObject<List<DecisionState>>(json);
        }
        #endregion
    }
}
