﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Net;

//Ref http://qiita.com/oishihiroaki/items/bb2977c72052f5dd5bd9

public class CameraController : MonoBehaviour {
    private float _cameraMotionRadius;
    private GUIStyle _labelStyle;
    private GameObject _parent;

    private float RAD2DEG = (float)(180.0 / System.Math.PI);
    private float DEG2RAD = (float)(System.Math.PI / 180.0);
    private float _pitch=0f;
	private float _yaw=0f;
    
	public string _wsAddress = "ws://0.0.0.0:3000";
	WebSocket _ws;
	string _testMsg;

    // Use this for initialization
    void Start (){
        _parent = this.transform.root.gameObject;

        _cameraMotionRadius = System.Math.Abs(this.transform.position.z);
        _labelStyle = new GUIStyle();
        _labelStyle.fontSize = Screen.height / 22;
        _labelStyle.normal.textColor = Color.white;

		_testMsg = "aaa";
		Connect ();
    }


    // Update is called once per frame
    void Update () {
        Vector3 localPos = new Vector3(this._cameraMotionRadius * (float)System.Math.Cos(_pitch) * (float)System.Math.Cos(_yaw),
                                        this._cameraMotionRadius * (float)System.Math.Sin(_pitch),
                                        this._cameraMotionRadius * (float)System.Math.Cos(_pitch) * (float)System.Math.Sin(_yaw));

        this.transform.position = new Vector3(this._parent.transform.position.x + localPos.x,
                                               this._parent.transform.position.y + localPos.y,
                                               this._parent.transform.position.z + localPos.z);
        this.transform.LookAt(this._parent.transform.position);
    }

	void Connect () {	
		_ws = new WebSocket (_wsAddress);
		
		_ws.OnOpen += (sender, e) => {
			Debug.Log ("WebSocket Open");
		};
		
		_ws.OnMessage += (sender, e) => {
            JSONObject json = new JSONObject(e.Data);
            _pitch = (float)json.GetField("pitch").n*DEG2RAD;
            _yaw = (float)json.GetField("yaw").n * DEG2RAD;
            _testMsg = string.Format("{0}", json.GetField("pitch").n);
            //_testMsg = e.Data;
			Debug.Log ("WebSocket Message Type: " + e.Type + ", Data: " + e.Data);
		};
		
		_ws.OnError += (sender, e) => {
			Debug.Log ("WebSocket Error Message: " + e.Message);
		};
		
		_ws.OnClose += (sender, e) => {
			Debug.Log ("WebSocket Close");
		};
		
		_ws.Connect ();	
	}
	void Disconnect () {
		_ws.Close ();
		_ws = null;
	}
	
	void Send (string message) {
		_ws.Send (message);
	}
    void OnGUI(){
        float x = Screen.width / 2;
        float y = 0.0f;
        float w = Screen.width * 8 / 10;
        float h = Screen.height * 8 / 10;

		//string text = "";
        //		if (this.accel != null) {
        //			text += "\n" + string.Format ("accel-X:{0}", System.Math.Round (this.accel.x, 3));
        //			text += "\n" + string.Format ("accel-Y:{0}", System.Math.Round (this.accel.y, 3));
        //			text += "\n" + string.Format ("accel-Z:{0}", System.Math.Round (this.accel.z, 3));
        //			text += "\n";
        //			text += "\n" + string.Format ("accel-X:{0}", System.Math.Round (this.transform.position.x, 3));
        //			text += "\n" + string.Format ("accel-Y:{0}", System.Math.Round (this.transform.position.y, 3));
        //			text += "\n" + string.Format ("accel-Z:{0}", System.Math.Round (this.transform.position.z, 3));
        //			text += "\n";
        //		} 

        //GUI.Label(new Rect(x, y, w, h), _testMsg, this._labelStyle);

    }
}
