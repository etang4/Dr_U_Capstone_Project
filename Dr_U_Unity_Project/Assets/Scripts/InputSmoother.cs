using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSmoother : MonoBehaviour {
	private static SortedDictionary<iBeaconReceiver, InputSmoother> smoothers = new SortedDictionary<>();
	private static 

	// when the program starts or ends
	void Start() {
		iBeaconReceiver.BeaconRangeChangedEvent += TryUpdateBeaconRanges;
		iBeaconReceiver.BluetoothStateChangedEvent += TrackBluetoothStateChanges;
		iBeaconReceiver.CheckBluetoothLEStatus();
		Debug.Log("listening for beacons...");
	}
	
	void OnDestroy() {
		iBeaconReceiver.BeaconRangeChangedEvent -= TryUpdateBeaconRanges;
		iBeaconReceiver.BluetoothStateChangedEvent -= TrackBluetoothStateChanges;
	}
	
	// Bluetooth event handling
	private void TryUpdateBeaconRanges(List<Beacon> beacons) {
		
	}
}
