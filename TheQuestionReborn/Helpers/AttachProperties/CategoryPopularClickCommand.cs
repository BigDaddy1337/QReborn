using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TheQuestionReborn.Helpers.AttachProperties
{
    public static class CategoryPopularClickCommand
    {
        public static readonly DependencyProperty CommandProperty =
                DependencyProperty.RegisterAttached("Command", typeof(ICommand),
                typeof(CategoryPopularClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

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
            var control = d as Button;
            if (control != null)
                control.Click += OnClick;
        }

        private static void OnClick(object sender, RoutedEventArgs e)
        {
            var control = sender as Button;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(control?.Content))
                command.Execute(control?.Content);
        }
    }

}
