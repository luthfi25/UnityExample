using UnityEngine;
using System.Collections;

public class FriendDDManager : MonoBehaviour {

	private static FriendDDManager instance = null;
	public static FriendDDManager Instance
	{
		get { return instance; }
	}
	void Awake() {

	}
	void Start(){
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}