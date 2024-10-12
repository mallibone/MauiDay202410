namespace DemoI;

public partial class MainPage : ContentPage
{
	private readonly Stack<string> _previousUrls = new();
	
	// private const string _cssInjection = ".dd-burger { display: none; } .breadcrumb-wrapper { display: none;} .dd-gnav > div:nth-child(2) > ul:nth-child(1) > li:nth-child(1) > a:nth-child(1) { display: none;} .justify-content-center > div:nth-child(2) { display: none; } #header > div > div > div.col-9 > div > div:nth-child(1) > div.dd-gnav > div > ul > li:nth-child(2) > a {display: none; }";
	private const string _cssInjection = "";
	
	private readonly Injector _injector = new(null, _cssInjection);
	
	public MainPage()
	{
		InitializeComponent();

		WebView.Navigating += OnNavigating;
		WebView.Navigated += OnNavigated;

		WebView.Source = "https://www.rey-technology.com/en/products/idip-platform";
	}

	private async void OnNavigated(object? sender, WebNavigatedEventArgs e)
	{
		if (WebView == null) return;

		await _injector.DoInjection(WebView);
		if (!e.Url.StartsWith("http://") 
			&& !e.Url.StartsWith("tel:") 
			&& !e.Url.StartsWith("mailto:")
			&& CheckIfBaseUrlWasAlreadyCalled(e.Url))
		{
			_previousUrls.Push(e.Url);
		}

		await Task.Delay(500);
		Loading.IsVisible = false;
	}

	private bool CheckIfBaseUrlWasAlreadyCalled(string url)
	{
		if (_previousUrls.Count == 0) return true;

		if (!url.Contains('#') && url.Last() == '#') return true;

		var baseUrl = url.Split('#').First();
		var peekBaseUrl = _previousUrls.Peek().Split('#').First();
		return peekBaseUrl != baseUrl;
	}


	private void OnNavigating(object? sender, WebNavigatingEventArgs e)
	{
		var connectivity = Connectivity.NetworkAccess;
		if (connectivity != NetworkAccess.Internet)
		{
			Navigation.PushModalAsync(new OfflinePage());
			return;
		}

		Loading.IsVisible = true;
		if (WebView == null) return;
	}
}