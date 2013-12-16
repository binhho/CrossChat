using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.WP.Silverlight.Seedwork.Controls;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class InputTextViewModel : BaseViewModel
    {
        private TaskCompletionSource<string> _taskSource = null;
        private string _title;
        private string _text;
        private bool _isSubmitted = false;
        private Validator _validator;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public Validator Validator
        {
            get { return _validator; }
            set { SetProperty(ref _validator, value); }
        }

        public ICommand SubmitCommand { get { return CreateCommand(OnSubmit, OnCanSubmit); } }

        private bool OnCanSubmit()
        {
            return true;
        }

        private void OnSubmit()
        {
            _isSubmitted = true;
            _taskSource.TrySetResult(Text);
            RequestClose();
        }

        public Task<string> Ask(string title, string defaultValue, Validator valueValidator)
        {
            _isSubmitted = false;
            Title = title;
            Text = defaultValue ?? string.Empty;
            Validator = valueValidator;

            _taskSource = new TaskCompletionSource<string>();

            Show();

            return _taskSource.Task;
        }

        public Task<string> Ask(string title, string defaultValue = "", int minLength = 3, int maxLength = 150)
        {
            return Ask(title, defaultValue, new Validator(s => DefaultValidator(s, minLength, maxLength)));
        }

        private string DefaultValidator(string str, int minLength, int maxLength)
        {
            if (str.Length < minLength)
                return "value is too small";
            if (str.Length > maxLength)
                return "value is too large";
            return string.Empty;
        }

        public override void OnClose()
        {
            if (!_isSubmitted)
            {
                _taskSource.TrySetResult(null);
            }
            base.OnClose();
        }
    }
}
