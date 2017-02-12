using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheQuestionReborn.Helpers.AttachProperties
{
    public static class TextChangedTextBoxCommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(TextChangedTextBoxCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = d as TextBox;
            if (control != null)
                control.TextChanged += TextChanged;
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            var control = sender as TextBox;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(control?.Text))
                command.Execute(control?.Text);
        }

    }
}

