using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using PDollarGestureRecognizer;

public class Main : MonoBehaviour {
	//Game
	private Player player1;
	private Player player2;
	private Player player3;

	private GameObject enemyObject;
	public GameObject[] enemyArray;
	private Enemy enemy;

	private bool gestureErr = false;
	private Vector3 mouseStart;
	//GUI for Game
	public GameObject player1Gesture;
	public GameObject player2Gesture;
	public GameObject player3Gesture;
	public GameObject enemyGesture;

	public GameObject enemyCloud;

	public Sprite[] gestureArr;
	public Sprite[] enemyGestureArr;
	public Sprite question;

	private SpriteRenderer spriteRenderer1;
	private SpriteRenderer spriteRenderer2;
	private SpriteRenderer spriteRenderer3;
	private SpriteRenderer enemyRenderer;

	private Animator animP1;
	private Animator animP2;
	private Animator animP3;

	public Slider player1Energy;
	public Slider player2Energy;
	public Slider player3Energy;

	public Slider enemyHp;
	public Slider player1Hp;
	public Slider player2Hp;
	public Slider player3Hp;

	private bool attack = false;

	private Gesture candidate;
	private Result gestureResult;
	private bool checkResult = false;
	//Origin
	public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();
	
	private List<Point> points = new List<Point>();
	private int strokeId = -1;
	
	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;
	
	private RuntimePlatform platform;
	private int vertexCount = 0;
	
	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;
	
	//GUI
	private string message;
	private bool recognized;
	private string newGestureame = "";

	void Start () {
		GameObject dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
		CharacterSelected friendsSelected = dontDestroy.GetComponent<CharacterSelected> ();
		string player1Name = friendsSelected.SelectedChar[0];
		string player2Name = friendsSelected.SelectedChar[1];
		string player3Name = friendsSelected.SelectedChar[2];


		//Ambil object player1
		GameObject player1Object = Instantiate (Resources.Load (player1Name)) as GameObject;;
		if (player1Object != null) {
			player1 = player1Object.GetComponent <Player>();
		}
		if (player1 == null) {
			Debug.Log ("Cannot find 'player1' script");
		}

		//Ambil object player2
		GameObject player2Object = Instantiate (Resources.Load (player2Name)) as GameObject;;
		if (player1Object != null) 
			player2 = player2Object.GetComponent <Player>();

		if (player2 == null) {
			Debug.Log ("Cannot find 'player2' script");
		}

		//Ambil object player3
	    GameObject player3Object = Instantiate (Resources.Load (player3Name)) as GameObject;;
		if (player3Object != null) {
			player3 = player3Object.GetComponent <Player>();
		}
		if (player3 == null) {
			Debug.Log ("Cannot find 'player3' script");
		}

		//Ambil object Enemy
	    enemyObject = Instantiate(enemyArray[UnityEngine.Random.Range(0,enemyArray.Length)]);
		if (enemyObject != null) {
			enemy = enemyObject.GetComponent <Enemy>();
		}
		if (enemy == null) {
			Debug.Log ("Cannot find 'Enemy' script");
		}
		player1Object.transform.position = GameObject.Find ("PlayerPosition1").transform.position;
		player2Object.transform.position = GameObject.Find ("PlayerPosition2").transform.position;
		player3Object.transform.position = GameObject.Find ("PlayerPosition3").transform.position;
		enemyObject.transform.position = GameObject.Find ("EnemyPosition").transform.position;

		platform = Application.platform;
		drawArea = new Rect(0, 0, Screen.width, Screen.height);
		
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		
		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths){
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
		}
		setGestureToPlayer ();
		setPlayerEnergyOnBar ();

		player1Hp.maxValue = player1.maxHp;
		player2Hp.maxValue = player2.maxHp;
		player3Hp.maxValue = player3.maxHp;

		enemyHp.maxValue = enemy.maxHp;

		player1Energy.maxValue = player1.maxEnergy;
		player2Energy.maxValue = player2.maxEnergy;
		player3Energy.maxValue = player3.maxEnergy;
		StartCoroutine (EnemyGestureGenerator());

		spriteRenderer1 = player1Gesture.GetComponent<SpriteRenderer> ();
		spriteRenderer2 = player2Gesture.GetComponent<SpriteRenderer> ();
		spriteRenderer3 = player3Gesture.GetComponent<SpriteRenderer> ();
		setPlayerGestureOnGUI ();
		enemyRenderer = enemyGesture.GetComponent<SpriteRenderer>();
		enemyRenderer.sprite = question;
		animP1 = player1Object.GetComponent<Animator>();
		animP2 = player2Object.GetComponent<Animator>();
		animP3 = player3Object.GetComponent<Animator>();

	}
	
	void Update () {
		GamePlay ();
		if (Input.GetMouseButtonUp(0)) {
			points.Clear();
			foreach (LineRenderer lineRenderer in gestureLinesRenderer) {
				
				lineRenderer.SetVertexCount(0);
				Destroy(lineRenderer.gameObject);
			}
			
			gestureLinesRenderer.Clear();
		}
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}
		
		if (drawArea.Contains(virtualKeyPosition)) {
			if (Input.GetMouseButtonDown(0)) {
				
				if (recognized) {
					
					recognized = false;
					strokeId = -1;
					
					points.Clear();
					foreach (LineRenderer lineRenderer in gestureLinesRenderer) {
						
						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}
					
					gestureLinesRenderer.Clear();
				}
				++strokeId;
				
				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
				
				gestureLinesRenderer.Add(currentGestureLineRenderer);
				
				vertexCount = 0;
			}
			
			if (Input.GetMouseButton(0)) {

				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
				
				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}
		}
	}
	void GamePlay(){


		setPlayerEnergyOnBar ();
		player1Hp.value = player1.hp;
		player3Hp.value = player3.hp;
		player2Hp.value = player2.hp;
		enemyHp.value = enemy.hp;
		if(enemy.hp<=0){
			player1.exp += enemy.exp;
			player2.exp += enemy.exp;
			player3.exp += enemy.exp;
			Destroy(enemyObject);
			enemyObject = Instantiate(enemyArray[UnityEngine.Random.Range(0,enemyArray.Length)]);
			enemy = enemyObject.GetComponent <Enemy>();
			enemyObject.transform.position = GameObject.Find ("EnemyPosition").transform.position;

		}
		if(player1.hp<=0 &&player2.hp <=0&&player3.hp<=0){
			Application.LoadLevel(3);
		}
		//Attack jika gesture benar dengan salah satu gesture yang terdapat pada pemain
		if (checkResult == true) {
			string player = "";
			if (gestureResult.GestureClass == enemy.getGesture () && gestureResult.Score > 0.6 ) {
				enemy.setGesture("");
				message = "You Attack The Enemy";
				enemyRenderer.sprite = question;

			}
			else if (gestureResult.GestureClass == player1.getGesture () && gestureResult.Score > 0.6 ) {
				player = player1.getName ();

				if(player1.currentEnergy > player1.energyCost){
					animP1.SetTrigger("attackTrig");
					message = gestureResult.GestureClass + " " + gestureResult.Score + " " + player + " Attack!!!";
					setGestureToPlayer ();
					setPlayerGestureOnGUI ();
					player1.minEnergy();
					//attack
					enemy.setHp(player1.damage);
					checkResult = false;

				}
				else{
					message = player + " has not enough energy";
					checkResult = false;

				}
			} else if (gestureResult.GestureClass == player2.getGesture () && gestureResult.Score > 0.6 ) {
				player = player2.getName ();

				if(player2.currentEnergy > player2.energyCost){
					animP2.SetTrigger("attackTrig");

					message = gestureResult.GestureClass + " " + gestureResult.Score + " " + player + " Attack!!!";
					setGestureToPlayer ();
					setPlayerGestureOnGUI ();
					player2.minEnergy();
					//attack
					enemy.setHp(player2.damage);

					checkResult = false;

				}
				else{
					message = player + " has not enough energy";
					checkResult = false;

				}
			
			} else if (gestureResult.GestureClass == player3.getGesture () && gestureResult.Score > 0.6 ) {
				player = player3.getName ();

				if(player3.currentEnergy > player3.energyCost){
					animP3.SetTrigger("attackTrig");

					message = gestureResult.GestureClass + " " + gestureResult.Score + " " + player + " Attack!!!";
					setGestureToPlayer ();
					setPlayerGestureOnGUI ();
					player3.minEnergy();
					//attack
					enemy.setHp(player3.damage);

					checkResult = false;

				}
				else{
					message = player + " has not enough energy";
					checkResult = false;

				}

			
			} else {
				message = "Wrong Gesture";
				checkResult = false;


			}
		}
		

		

	}
	
	void OnGUI() {
		
		//GUI.Box(drawArea,"Draw Area");
		GUI.Label(new Rect(10, Screen.height - 40, 500, 50), "<color=red>"+message+"</color>");
		if(Input.GetMouseButtonDown(0)){
			mouseStart= Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0)) {

			if(mouseStart == Input.mousePosition){
				gestureErr = true;
				return;
			}
				gestureErr = false;
				recognized = true;
				
				candidate = new Gesture(points.ToArray());
				gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
				checkResult = true;

		}
		
		//Tempat Add
	}

	//membuat list random gesture
	List<string> getRandomGesture (){
		List<string> result = new List<string>();
		bool full = false;
		int i = 0;


		while (full == false) {
			string rand = gestureArr[UnityEngine.Random.Range(0,gestureArr.Length)].name;

			if(!result.Exists(element => element == rand)){
				result.Add(rand);
				i++;
			}
			if(i == 3){
				full =  true;
			}

		}
		return result;
	}
	//Method for getGesture()
	void setPlayerGestureOnGUI(){
		spriteRenderer1.sprite = gestureArr[findSprite (player1.getGesture ())];
		spriteRenderer2.sprite = gestureArr[findSprite (player2.getGesture ())];
		spriteRenderer3.sprite = gestureArr[findSprite (player3.getGesture ())];
	}
	//method untuk menampilkan energy di GUI
	void setPlayerEnergyOnBar(){
		player1Energy.value = player1.currentEnergy;
		player2Energy.value = player2.currentEnergy;
		player3Energy.value = player3.currentEnergy;

	}
	//Method untuk memasukan randomGesture ke Player (dan Enemy)
	void setGestureToPlayer(){
		List<string> randomGesture = getRandomGesture ();
		player1.setGesture (randomGesture [0]);
		player2.setGesture (randomGesture [1]);
		player3.setGesture (randomGesture [2]);
	}
	//generate gesture untuk musuh dan kapan meyerang
	IEnumerator EnemyGestureGenerator(){
		yield return new WaitForSeconds (3);
		while (true) {
			if(enemy.getGesture() ==""){
				enemy.setGesture(enemyGestureArr[UnityEngine.Random.Range(0,enemyGestureArr.Length)].name);
				enemyRenderer.sprite = findEnemySprite(enemy.getGesture ());

				}
			yield return new WaitForSeconds (4);
			if(enemy.getGesture() !=""){
				player1.setHp (enemy.getDamage());
				player2.setHp (enemy.getDamage());
				player3.setHp (enemy.getDamage());
				enemy.setGesture("");
				enemyRenderer.sprite = question;

			}
			yield return new WaitForSeconds (3);
		}
	}
	public int findSprite(string spriteName){
		for(int i = 0 ; i<gestureArr.Length;i++){
			if (gestureArr[i].name == spriteName){
				return i;
			}
		}
		return -1;
	}
	public Sprite findEnemySprite(string spriteName){
		for(int i = 0 ; i<enemyGestureArr.Length;i++){
			if (enemyGestureArr[i].name == spriteName){
				return enemyGestureArr[i];
			}
		}
		return gestureArr[0];
	}
	public void Save()
	{
		Player[] player = new Player[3];
		player [0] = player1;
		player [1] = player2;
		player [2] = player3;

		BinaryFormatter bf = new BinaryFormatter ();

		for(int i = 0 ; i < 3;i++){
			FileStream file = File.Create (Application.persistentDataPath + "/" + player[i].getName() + "Info.dat");

			CharacterData data = new CharacterData();
			data.exp = player[i].exp;
			data.hp = player[i].hp;
			data.damage = player[i].damage;
			data.maxEnergy = player[i].maxEnergy;
			data.energyCost = player[i].energyCost;
			data.energyCharge = player[i].energyCharge;
			bf.Serialize(file,data);
			file.Close ();
		}



	}
	void OnApplicationQuit() {

	}
}



