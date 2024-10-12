namespace DemoII;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnSendMessageButtonClicked(object sender, EventArgs e)
	{
		hybridWebView.SendRawMessage(Entry.Text);
	}

	private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
	{
		await DisplayAlert("Raw Message Received", e.Message, "OK");
	}
}
