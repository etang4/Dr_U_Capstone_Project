using UnityEngine;

public class InputSmoother {
	private const int DISCONNECTION_THRESHOLD = 5;

	private Beacon beacon;
	private Queue<uint> window = new Queue<uint>();
	private uint window_size;
	private uint current_sum = 0;
	private bool is_connected = true;
	
	public InputSmoother(Beacon beacon, uint window_size)
	{
		this.beacon = beacon;
		this.window_size = window_size;
		
		Update();
	}
	
	public void Update()
	{
		// TODO TEMP
		Debug.Log("[" + i + "] direct strength = " + beacon.strength.ToString());
		
		// add the new signal strength value to the window
		window.Enqueue(beacon.strength);
		current_sum += beacon.strength;
		// remove the old signal strength value from the window if needed
		if (window.Count > window_size)
		{
			current_sum -= window.Dequeue();
			// if the window is full and connection is very weak, discard the connection
			if (GetSignalStrength() < DISCONNECTION_THRESHOLD)
			{
				is_connected = false;
			}
		}
		
		// TODO TEMP
		Debug.Log("[" + i + "] smoothed strength = " + GetSignalStrength().ToString() + " (size=" + window.Count + ")");
	}

	public InputSmoother(Beacon beacon)
	{
		this.beacon = beacon;
	}
	
	public Beacon GetBeacon()
	{
		return beacon;
	}
	
	public bool IsConnected()
	{
		return is_connected;
	}
	
	public uint GetSignalStrength()
	{
		return current_sum / window.Count;
	}
}
