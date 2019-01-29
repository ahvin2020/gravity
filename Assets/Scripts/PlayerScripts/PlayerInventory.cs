using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

	public GUIText StarCountText;
	public GUIText KeyCountText;
	public const string STAR_TAG = "Star";
	public const string KEY_TAG = "Key";

	private Dictionary<string, int> items;

	// Use this for initialization
	void Start () {
		items = new Dictionary<string, int>();
	}
	
	public void obtainItem(string itemTag) {
		if (items.ContainsKey(itemTag) == false) {
			items.Add(itemTag, 0);
		}
	
		items[itemTag]++;

		// collected a star?
		if (itemTag == STAR_TAG) {
			StarCountText.text = items[itemTag] + "/3";
		} else if (itemTag == KEY_TAG) {
			KeyCountText.text = items[itemTag] + "/1";
		}
	}

	public bool hasItem(string itemTag) {
		int itemCount;
		if (items.TryGetValue(itemTag, out itemCount) && itemCount > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool useItem(string itemTag) {
		int itemCount;
		if (items.TryGetValue(itemTag, out itemCount) && itemCount > 0) {
			items[itemTag]--;
			return true;
		} else {
			return false;
		}
	}
}
