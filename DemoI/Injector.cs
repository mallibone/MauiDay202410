namespace DemoI;

public class Injector
{
    private readonly string? _cascadingStyleSheetsToInject;
    private readonly string? _javaScriptToInject;

    public Injector(string? javaScriptToInject = null, string? cascadingStyleSheetsToInject = null)
    {
        _javaScriptToInject = javaScriptToInject;
        _cascadingStyleSheetsToInject = cascadingStyleSheetsToInject;
    }

    public async Task DoInjection(WebView theWebview)
    {
        if (!string.IsNullOrWhiteSpace(_cascadingStyleSheetsToInject))
        {
            var cssInjection =
                $"var styles = `{_cascadingStyleSheetsToInject}`; var stylesheet = document.createElement('style'); stylesheet.innerText = styles; document.head.appendChild(stylesheet);";
            await theWebview.EvaluateJavaScriptAsync(cssInjection);
        }

        if (!string.IsNullOrWhiteSpace(_javaScriptToInject))
            await theWebview.EvaluateJavaScriptAsync(_javaScriptToInject);
    }
}