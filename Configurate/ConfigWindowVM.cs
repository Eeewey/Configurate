using CommunityToolkit.Mvvm.Input;

namespace Configurate
{
    public class ConfigWindowVM
    {
        public ConfigWindowVM() { }

        public IConfWindowCodeBehind CodeBehind { get; set; }

        private RelayCommand? _loadPRegCommand;
        public RelayCommand LoadPRegCommand => _loadPRegCommand ??= new RelayCommand(OnLoadPReg, CanLoadPReg);
        private bool CanLoadPReg()
        {
            return true;
        }
        private void OnLoadPReg()
        {
            //SaveManager.AddToSave(ViewType.PReg, (bool)CodeBehind.GetCheckBox(ViewType.PReg).IsChecked);
            CodeBehind.UpdateGraph();
        }

        private RelayCommand? _loadIRegCommand;
        public RelayCommand LoadIRegCommand => _loadIRegCommand ??= new RelayCommand(OnLoadIReg, CanLoadIReg);
        private bool CanLoadIReg()
        {
            return true;
        }
        private void OnLoadIReg()
        {
            //SaveManager.AddToSave(ViewType.IReg, (bool)CodeBehind.GetCheckBox(ViewType.IReg).IsChecked);
            CodeBehind.UpdateGraph();
        }

        private RelayCommand? _loadDRegCommand;
        public RelayCommand LoadDRegCommand => _loadDRegCommand ??= new RelayCommand(OnLoadDReg, CanLoadDReg);
        private bool CanLoadDReg()
        {
            return true;
        }
        private void OnLoadDReg()
        {
            //SaveManager.AddToSave(ViewType.DReg, (bool)CodeBehind.GetCheckBox(ViewType.DReg).IsChecked);
            CodeBehind.UpdateGraph();
        }
    }
}
