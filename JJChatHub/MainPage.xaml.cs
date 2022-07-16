using Microsoft.AspNetCore.SignalR.Client;

namespace JJChatHub;

public partial class MainPage : ContentPage
{
	int count = 0;

	private HubConnection _hubConnection;

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = this;

		_hubConnection = new HubConnectionBuilder()
			.WithUrl("http://jjchathub.azurewebsites.net/chat")
			.Build();

        _hubConnection.On<string>("ReceiveMessage", (message) =>
        {
			LabelChatMessages.Text += $"{Environment.NewLine}{message}";
        });

		Task.Run(() =>
		{
			Dispatcher.Dispatch(async () =>
			{
				await _hubConnection.StartAsync();
			});
		});
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		await _hubConnection.InvokeCoreAsync("SendMessage", args: new[] { LabelMyMessage.Text });
        
	}
}

