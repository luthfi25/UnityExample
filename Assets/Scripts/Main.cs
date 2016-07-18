using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using PDollarGestureRecognizer;

public class Main : MonoBehaviour {


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


		//Attack jika gesture benar dengan salah satu gesture yang terdapat pada pemain
		if (checkResult == true) {
			
		}
		
		//Tempat Add
	}


	}
	void OnApplicationQuit() {

	}
}



