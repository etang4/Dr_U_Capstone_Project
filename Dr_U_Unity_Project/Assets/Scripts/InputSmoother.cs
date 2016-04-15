using UnityEngine;
using System.Collections.Generic;

public class InputSmoother {
	private const int DISCONNECTION_THRESHOLD = 0;
	public const int DEFAULT_WINDOW_SIZE = 20;

	private Beacon beacon;
	private Queue<uint> window = new Queue<uint>();
	private uint window_size;
	private uint current_sum = 0;
	private bool is_connected = true;
	
	public InputSmoother(Beacon beacon)
	{
		this.window_size = DEFAULT_WINDOW_SIZE;
		
		Update(beacon);
	}
	
	public InputSmoother(Beacon beacon, uint window_size)
	{
		this.window_size = window_size;
		
		Update(beacon);
	}
	
	public void UpdateEmpty()
	{
		beacon.range = (BeaconRange) 0;
		Update(beacon);
	}
	
	public void Update(Beacon new_beacon_info)
	{
		beacon = new_beacon_info;
		
		uint beacon_strength = (uint) beacon.range;
		Debug.Log("[" + beacon.UUID + "] beacon raw strength = " + beacon_strength.ToString());
		
		// add the new signal strength value to the window
		window.Enqueue(beacon_strength);
		current_sum += beacon_strength;
		// remove the old signal strength value from the window if needed
		if (window.Count > window_size)
		{
			current_sum -= window.Dequeue();
			// if the window is full and connection is very weak, discard the connection
			if (GetSignalStrength() <= DISCONNECTION_THRESHOLD)
			{
				is_connected = false;
			}
		}
		
		Debug.Log("[" + beacon.UUID + "] beacon smoothed strength = " + GetSignalStrength().ToString() + " (size=" + window.Count + ")");
	}
	
	public Beacon GetBeacon()
	{
		return beacon;
	}
	
	public bool IsConnected()
	{
		return is_connected;
	}
	
	public float GetSignalStrength()
	{
		return ((float) current_sum) / ((float) window.Count);
	}
}
