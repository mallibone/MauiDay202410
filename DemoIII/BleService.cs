using System.Collections.ObjectModel;
using System.Diagnostics;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace DemoIII;

public class BleService : IBleService
{
    private readonly IBluetoothLE _ble;
    private readonly IAdapter _adapter;
    private IDevice? _polarDevice;

    public event EventHandler<int>? HeartRateValue;
    public ObservableCollection<IDevice> Devices { get; } = new();

    public BleService()
    {
        _ble = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
    }

    public async Task ConnectToPolar()
    {
        if (!_ble.IsOn && !_ble.IsAvailable)
        {
            throw new Exception("Bluetooth is not available or turned off");
        }

        try
        {
            // Code to scan for devices - use to scan for Polar device
            // _adapter.DeviceDiscovered += (sender, args) =>
            // {
            //     Devices.Add(args.Device);
            //     Debug.WriteLine($"Device found: {args.Device.Name}");
            // };
            // await _adapter.StartScanningForDevicesAsync();
            // _adapter.ScanTimeout = 3000;
            // await Task.Delay(3000);
            // await _adapter.StopScanningForDevicesAsync();
            _polarDevice = await _adapter.ConnectToKnownDeviceAsync(new Guid(BleConfiguration.PolarDeviceId));
            var heartBeatService = await _polarDevice.GetServiceAsync(new Guid("0000180d-0000-1000-8000-00805f9b34fb"));
            var heartBeatCharacteristic = await heartBeatService.GetCharacteristicAsync(new Guid("00002a37-0000-1000-8000-00805f9b34fb"));
            heartBeatCharacteristic.ValueUpdated += NewHeartRate;
            await heartBeatCharacteristic.StartUpdatesAsync();
            Debug.WriteLine("Connected to Polar Device");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void NewHeartRate(object? sender, CharacteristicUpdatedEventArgs e)
    {
        var heartRateValue = ParseHeartRate(e.Characteristic.Value);
        HeartRateValue?.Invoke(this, heartRateValue);
    }

    private int ParseHeartRate(byte[] data)
    {
        if (data.Length >= 2)
        {
            var flags = data[0];
            var heartRateValueFormat = flags & 0x01;
            if (heartRateValueFormat == 0)
            {
                // Heart Rate is in 8-bit format
                return data[1];
            }
            else
            {
                // Heart Rate is in 16-bit format
                return BitConverter.ToUInt16(data, 1);
            }
        }
        return 42;
    }
}
