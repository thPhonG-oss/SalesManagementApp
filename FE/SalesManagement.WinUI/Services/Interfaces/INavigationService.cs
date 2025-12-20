using Microsoft.UI.Xaml.Controls;

public interface INavigationService
{
    void SetFrame(Frame frame);
    bool NavigateTo(Type pageType);
    void SetHeader(string? header);
}