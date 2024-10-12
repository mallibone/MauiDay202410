namespace DemoIII;

public class FakeBleService : IBleService
{
    private bool _isPublishingHeartRateValues;

    public event EventHandler<int>? HeartRateValue;

    public async Task ConnectToPolar()
    {
        // Simulate connection to Polar device
        await Task.Delay(1000);

        // Start publishing heart rate values
        _ = Task.Run(PublishHeartRateValues);
    }

    private async Task PublishHeartRateValues()
    {
        Random random = new Random();

        // Check if the loop is already running
        if (_isPublishingHeartRateValues)
        {
            return;
        }

        _isPublishingHeartRateValues = true;

        while (true)
        {
            // Generate a random heart rate value between 70 and 90
            int heartRate = random.Next(70, 91);

            // Raise the HeartRateValue event
            HeartRateValue?.Invoke(this, heartRate);

            // Wait for a random interval between 1 and 5 seconds before publishing the next value
            await Task.Delay(1000);
        }
    }
}