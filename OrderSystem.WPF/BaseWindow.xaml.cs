using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace OrderSystem.WPF
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        public BaseWindow()
        {
            InitializeComponent();
            PreviewKeyDown += OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if(Keyboard.FocusedElement is FrameworkElement element)
                {
                    var enumerator = element.GetLocalValueEnumerator();
                    while (enumerator.MoveNext())
                    {
                        LocalValueEntry entry = enumerator.Current;
                        BindingExpressionBase binding = BindingOperations.GetBindingExpressionBase(element, entry.Property);
                        binding?.UpdateSource();
                    }
                }
            }
        }
    }
}
