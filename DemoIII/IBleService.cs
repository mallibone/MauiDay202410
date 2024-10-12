namespace DemoIII;

public interface IBleService
{
    event EventHandler<int>? HeartRateValue;
    Task ConnectToPolar();
}
