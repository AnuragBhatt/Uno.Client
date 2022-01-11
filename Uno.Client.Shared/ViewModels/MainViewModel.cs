using System.Threading.Tasks;

namespace Uno.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string SearchText
        {
            get { return GetValue(() => SearchText); }
            set { SetValue(() => SearchText, value); }
        }
        public bool IsSearchEnabled
        {
            get { return GetValue(() => IsSearchEnabled); }
            set { SetValue(() => IsSearchEnabled, value); }
        }
        public async Task LoadAsync()
        {
            SearchText = "Initial text";
            await Task.CompletedTask;
        }
        public async Task ChangeAndEnableTextBoxAsync()
        {
            IsSearchEnabled = true;
            SearchText = "Changed text";
            await Task.CompletedTask;
        }
    }
}
