using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class InputSmootherHelper : MonoBehaviour {
	private static HashSet<InputSmoother> smoothers = new HashSet<InputSmoother>();
	private static HashSet<Beacon> beacons = new HashSet<Beacon>();
	private static Timer update_timer = new Timer(100);
	
	// when the program starts or ends
	void Start()
	{
		// TODO TEMP
		Debug.Log("starting input smoothing...");
		
		iBeaconReceiver.BeaconRangeChangedEvent += TrackNewBeaconConnections;
		iBeaconReceiver.BluetoothStateChangedEvent += TrackBluetoothStateChanges;
		iBeaconReceiver.CheckBluetoothLEStatus();
		
		update_timer.Elapsed += UpdateBeaconRanges;
		update_timer.AutoReset = true;
		
		Debug.Log("listening for beacons...");
	}
	
	void OnDestroy()
	{
		iBeaconReceiver.BeaconRangeChangedEvent -= TrackNewBeaconConnections;
		iBeaconReceiver.BluetoothStateChangedEvent -= TrackBluetoothStateChanges;
	}
	
	// Bluetooth event handling
	private static void UpdateBeaconRanges(object source, ElapsedEventArgs e)
	{
		// TODO TEMP
		Debug.Log("updating all beacon smoothers...");
		
		HashSet<InputSmoother> disconnected_smoothers = new HashSet<InputSmoother>();
		foreach (InputSmoother smoother in smoothers)
		{
			smoother.Update();
			if (!smoother.IsConnected())
			{
				disconnected_smoothers.Add(smoother);
			}
		}
		
		// remove disconnected smoothers
		foreach (InputSmoother disconnected_smoother in disconnected_smoothers)
		{
			beacons.Remove(disconnected_smoother.GetBeacon());
			smoothers.Remove(disconnected_smoother);
		}
	}
	
	private static void TrackNewBeaconConnections(List<Beacon> updated_beacons)
	{
		Debug.Log("analyzing beacon update event...");
		
		foreach (Beacon updated_beacon in updated_beacons)
		{
			if (!beacons.Contains(updated_beacon))
			{
				// TODO TEMP
				Debug.Log("found new beacon: " + updated_beacon.UUID);
				
				beacons.Add(updated_beacon);
				smoothers.Add(new InputSmoother(updated_beacon));
			}
		}
	}
	
	private static void TrackBluetoothStateChanges(BluetoothLowEnergyState newstate)
	{
		switch (newstate)
		{
		case BluetoothLowEnergyState.POWERED_ON:
			iBeaconReceiver.Init();
			update_timer.Enabled = true;
			Debug.Log ("Bluetooth is on!");
			break;
		case BluetoothLowEnergyState.POWERED_OFF:
			update_timer.Enabled = false;
			iBeaconReceiver.EnableBluetooth();
			Debug.Log ("Bluetooth turned off; I'll try to turn it back on.");
			break;
		case BluetoothLowEnergyState.UNAUTHORIZED:
			Debug.Log("The user doesn't want this app to use Bluetooth!");
			break;
		case BluetoothLowEnergyState.UNSUPPORTED:
			Debug.Log ("This device doesn't support Bluetooth Low Energy!");
			break;
		default:
			Debug.Log ("I don't know what this \"" + newstate + "\" Bluetooth state means!");
			break;
		}
	}
}
