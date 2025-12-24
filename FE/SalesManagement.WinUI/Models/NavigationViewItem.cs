using Microsoft.UI.Xaml.Controls;

namespace SalesManagement.WinUI.Models;

public class AppMenuItem
{

    public string Title { get; set; } = string.Empty;
    public Symbol Icon { get; set; }
    public Type? PageType { get; set; }
    public string Tag { get; set; } = string.Empty;

    public bool ShowHeader { get; set; } = true;
}