using UnityEngine;
using System.Collections;
//Sound and option manager
public class Manager : MonoBehaviour{

	private static Manager instance = null;
	public static Manager Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	void Start(){
		
	}
}
