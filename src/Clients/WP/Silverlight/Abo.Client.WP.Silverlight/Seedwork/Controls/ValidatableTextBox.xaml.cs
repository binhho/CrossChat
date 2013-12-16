using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Abo.Client.WP.Silverlight.Seedwork.Controls
{
    public partial class ValidatableTextBox : UserControl
    {
        public static readonly DependencyProperty ValidatorProperty =
            DependencyProperty.Register("Validator", typeof(Validator), typeof(ValidatableTextBox),
                                        new PropertyMetadata(null, ValidatorPropertyChangedStatic));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (ValidatableTextBox),
                                        new PropertyMetadata("", HeaderPropertyChangedStatic));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (ValidatableTextBox),
                                        new PropertyMetadata(null, TextPropertyChangedStatic));



        public InputScope TextInputScope
        {
            get { return (InputScope)GetValue(TextInputScopeProperty); }
            set { SetValue(TextInputScopeProperty, value); }
        }

        public static readonly DependencyProperty TextInputScopeProperty =
            DependencyProperty.Register("TextInputScope", typeof(InputScope), typeof(ValidatableTextBox), new PropertyMetadata(null, OnTextInputScopeChanged));

        private static void OnTextInputScopeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = ((ValidatableTextBox) d).textBoxValue;
            tb.InputScope = e.NewValue as InputScope;
        }
        
        public const string ValidationFailedStateName = "ValidationFailedState";
        public const string NormalStateName = "NormalState";

        public ValidatableTextBox()
        {
            InitializeComponent();
            errorTextBox.Text = string.Empty;
        }

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Validator Validator
        {
            get { return (Validator)GetValue(ValidatorProperty); }
            set { SetValue(ValidatorProperty, value); }
        }
        
        public bool IsMultiline
        {
            get { return (bool)GetValue(IsMultilineProperty); }
            set { SetValue(IsMultilineProperty, value); }
        }

        public Control NextControl
        {
            get { return (Control)GetValue(NextControlProperty); }
            set { SetValue(NextControlProperty, value); }
        }

        public static readonly DependencyProperty NextControlProperty =
            DependencyProperty.Register("NextControl", typeof(Control), typeof(ValidatableTextBox), new PropertyMetadata(null));

        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(ICommand), typeof(ValidatableTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty IsMultilineProperty =
            DependencyProperty.Register("IsMultiline", typeof(bool), typeof(ValidatableTextBox), new PropertyMetadata(false, IsMultilinePropertyChangedStatic));

        private static void IsMultilinePropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
            {
                box.IsMultilinePropertyChanged();
            }
        }

        private void IsMultilinePropertyChanged()
        {
            if (IsMultiline)
            {
                textBoxValue.TextWrapping = TextWrapping.Wrap;
                textBoxValue.Height = 110;
            }
            else
            {
                textBoxValue.TextWrapping = TextWrapping.NoWrap;
                textBoxValue.Height = 72;
            }
        }

        private static void HeaderPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
            {
                if (e.NewValue != null &&
                    !string.IsNullOrWhiteSpace(e.NewValue.ToString()) &&
                    !e.NewValue.ToString().EndsWith(":"))
                    box.Header += ":";
            }
        }

        private static void TextPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
                box.TextPropertyChanged();
        }

        private static void ValidatorPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
                box.ValidatorPropertyChanged();
        }

        private void ValidatorPropertyChanged()
        {
            if (Validator != null)
            {
                Validator.ManualValidating += ValidatorValidating;
                Validator.HideErrorsRequest += ValidatorHideErrorsRequest;
            }
        }

        void ValidatorHideErrorsRequest(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, NormalStateName, true);
            errorBorder.Visibility = Visibility.Collapsed;
        }

        void ValidatorValidating(object sender, ValidatingEventArgs e)
        {
            Text = textBoxValue.Text;
            e.ValidationSuccess = Validate();
        }

        private void TextPropertyChanged()
        {
            Validate();
        }

        private bool Validate()
        {
            if (Validator == null)
                return true;
            string error = Validator.ValidatorFunc(Text);
            errorTextBox.Text = error;
            bool errorEmpty = string.IsNullOrEmpty(error);
            if (errorEmpty)
                hiddenButton.Focus();

            errorBorder.Visibility = errorEmpty ? Visibility.Collapsed : Visibility.Visible;
            VisualStateManager.GoToState(this, errorEmpty ? NormalStateName : ValidationFailedStateName, true);
            return errorEmpty;
        }

        private async void TextBoxValueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                Text = textBoxValue.Text;

                if (!Validate())
                    return;

                if (EnterCommand != null)
                    EnterCommand.Execute(null);
                //if (NextControl != null)
                //    NextControl.Focus();
            }
        }
    }

    public class ValidatableTextBoxDesignData
    {
        public string Text
        {
            get { return "Some design data text"; }
        }

        public string Header
        {
            get { return "Some header:"; }
        }
    }

    public class Validator
    {
        public Validator(Func<string, string> validatorFunc)
        {
            ValidatorFunc = validatorFunc;
        }

        public Validator()
        {
        }

        public Func<string, string> ValidatorFunc { get; set; }

        public event EventHandler<ValidatingEventArgs> ManualValidating = delegate { };
        public event EventHandler HideErrorsRequest = delegate { }; 

        public bool Validate()
        {
            ValidatingEventArgs args = new ValidatingEventArgs();
            ManualValidating(this, args);
            return args.ValidationSuccess;
        }

        public void HideErrors()
        {
            HideErrorsRequest(this, EventArgs.Empty);
        }
    }

    public class ValidatingEventArgs : EventArgs
    {
        public bool ValidationSuccess { get; set; }
    }
}