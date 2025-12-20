using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

public class NavigationService : INavigationService
{
    private Frame? _frame;
    private NavigationView? _navView; 

    public void SetFrame(Frame frame) => _frame = frame;
    public void SetNavView(NavigationView navView) => _navView = navView;

    public bool NavigateTo(Type pageType)
    {
        if (_frame != null && _frame.CurrentSourcePageType != pageType)
        {
            return _frame.Navigate(pageType, null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        return false;
    }

    public void SetHeader(string? header)
    {
        
        if (_navView != null)
        {
            _navView.Header = header;
        }
    }
}