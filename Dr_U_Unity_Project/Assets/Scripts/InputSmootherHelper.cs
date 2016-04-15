using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class InputSmootherHelper : MonoBehaviour {
	public static readonly int BEACON_UPDATE_INTERVAL = 1500;
	public static readonly int CLOSEST_BEACON_QUEUE_SIZE = 3;
	
	public delegate void ClosestBeaconChanged(HashSet<Beacon> new_closest, HashSet<Beacon> old_closest);
	public static event ClosestBeaconChanged ClosestBeaconChangedEvent;
	
	/** This Queue keeps track of the last few sets of closest beacons.  */
	public static Queue<HashSet<Beacon>> closest_beacons = new Queue<HashSet<Beacon>>();
	
	private static Dictionary<Beacon, InputSmoother> smoothers = new Dictionary<Beacon, InputSmoother>();
	private static Timer update_timer = new Timer(BEACON_UPDATE_INTERVAL);
	
	// when the program starts or ends
	void Start()
	{
		Debug.Log("starting input smoothing...");
		
		iBeaconReceiver.BeaconRangeChangedEvent += TrackNewBeaconConnections;
		iBeaconReceiver.BluetoothStateChangedEvent += TrackBluetoothStateChanges;
		iBeaconReceiver.CheckBluetoothLEStatus();
		iBeaconReceiver.EnableBluetooth();
		
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
		Debug.Log("updating " + smoothers.Count + " beacon smoother(s)...");
		
		// update all beacons and check for disconnected and closest beacons
		HashSet<Beacon> disconnected_beacons = new HashSet<Beacon>(), new_closest_beacons = new HashSet<Beacon>();
		float new_closest_beacon_strength = 0;
		foreach (Beacon beacon in smoothers.Keys)
		{
			InputSmoother smoother = smoothers[beacon];
			smoother.Update(beacon);
			
			if (!smoother.IsConnected())
			{
				disconnected_beacons.Add(beacon);
			}
			
			if (new_closest_beacons.Count == 0 || smoother.GetSignalStrength() > new_closest_beacon_strength)
			{
				new_closest_beacons = new HashSet<Beacon>();
				new_closest_beacons.Add(smoother.GetBeacon());
				new_closest_beacon_strength = smoother.GetSignalStrength();
			}
		}
		
		// remove disconnected smoothers
		foreach (Beacon disconnected_beacon in disconnected_beacons)
		{
			smoothers.Remove(disconnected_beacon);
		}
		
		// call a ClosestBeaconChangedEvent if needed
		if (closest_beacons.Count == 0 || !new_closest_beacons.SetEquals(closest_beacons.ToArray()[closest_beacons.Count - 1]))
		{
			Debug.Log("firing ClosestBeaconChangedEvent...");
			if (ClosestBeaconChangedEvent != null)
			{
				ClosestBeaconChangedEvent(new_closest_beacons, closest_beacons.ToArray()[closest_beacons.Count - 1]);
			}
			
			closest_beacons.Enqueue(new_closest_beacons);
			if (closest_beacons.Count > CLOSEST_BEACON_QUEUE_SIZE)
			{
				closest_beacons.Dequeue();
			}
		}
	}
	
	private static void TrackNewBeaconConnections(List<Beacon> updated_beacons)
	{
		Debug.Log("analyzing beacon update event...");
		
		// update all the beacons with the new info
		foreach (Beacon updated_beacon in updated_beacons)
		{
			if (!smoothers.ContainsKey(updated_beacon))
			{
				Debug.Log("found new beacon: " + updated_beacon.UUID);
				
				smoothers.Add(updated_beacon, new InputSmoother(updated_beacon));
			}
			else
			{
				Debug.Log("beacon updated: " + updated_beacon.UUID);
				
				smoothers[updated_beacon].Update(updated_beacon);
			}
		}
		
		// any beacons we saw before that are not on the list now have been disconnected
		foreach (Beacon old_beacon in smoothers.Keys)
		{
			if (!updated_beacons.Contains(old_beacon))
			{
				smoothers[old_beacon].UpdateEmpty();
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
