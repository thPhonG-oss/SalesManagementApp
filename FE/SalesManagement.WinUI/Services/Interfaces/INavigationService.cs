using Microsoft.UI.Xaml.Controls;

public interface INavigationService
{
    void SetFrame(Frame frame);
    bool NavigateTo(Type pageType);
    bool NavigateTo(Type pageType, object? parameter);
    void SetHeader(string? header);
    void GoBack();
}