using UnityEngine;
using System.Collections;

public class DontDestroyManager : MonoBehaviour{

    private static DontDestroyManager instance = null;
    public static DontDestroyManager Instance
    {
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
