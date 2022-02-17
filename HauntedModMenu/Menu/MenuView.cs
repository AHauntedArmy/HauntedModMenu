using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace HauntedModMenu.Menu
{
	class MenuView : MonoBehaviour
	{
		private static Font defaultFont = null;

		private static readonly Color enabledColor = new Color(0.4951f, 0.7075f, 0.3771f, 1f);
		private static readonly Color disabledColor = new Color(0.8396f, 0.2495f, 0.2495f, 1f);

		private void Awake()
		{
			LoadMenu();
		}

		private void LoadMenu()
		{
			Debug.Log("loading menu");

			if (defaultFont == null)
				defaultFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

			this.gameObject.GetComponent<Renderer>()?.material?.SetColor("_Color", Color.black); 

			RectTransform rect = this.gameObject.AddComponent<RectTransform>();
			Canvas canvas = this.gameObject.AddComponent<Canvas>();

			/*
			if (canvas != null) {
				Debug.Log("setting canvas rendering mode");
				canvas.renderMode = RenderMode.WorldSpace;
			}
			*/

			if (rect != null) {
				Debug.Log("setting menu canvas size");
				rect.sizeDelta = new Vector2(1f, 1f);
			}

			string[,] textInfo = new string[,] { {"MenuTitle", "Haunted Mod Menu" }, {"PageText", "Pages" } };
			Vector3[] positions = new Vector3[] { new Vector3(0f, 0.435f, -0.51f), new Vector3(0f, -0.4f, -0.51f) };

			GameObject go = null;

			for (int index = 0; index < 2; index++) {
				go = new GameObject(textInfo[index, 0]);

				if (go != null) {
					SetLocal(go.transform, positions[index], new Vector3(0.0018f, 0.0018f, 0f), Quaternion.identity);
					AddUI(textInfo[index, 1], go, new Vector2(555f, 60f), new Color(0.6132f, 0.6132f, 0.6132f, 1f));
				}
			}

			go = null;
			Vector3 buttonPos = new Vector3(0f, 0.3f, -1f);
			Vector3 buttonTextPos = new Vector3(0f, 0f, -0.51f);
			Vector3 buttonScale = new Vector3(0.75f, 0.1f, 0.75f);
			Vector3 buttonTextScale = new Vector3(0.002208869f, 0.01262004f, 1f);
			GameObject textObject = null;

			for (int i = 0; i < 5; i++) {

				go = CreateButton($"ModButton{i}");
				if (go == null)
					continue;

				go.transform.localPosition = buttonPos;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localScale = buttonScale;
				
				buttonPos.y -= 0.135f;

				textObject = new GameObject("ButtonText");
				if (textObject == null)
					continue;

				textObject.transform.SetParent(go.transform);
				textObject.transform.localRotation = Quaternion.identity;
				textObject.transform.localPosition = buttonTextPos;
				textObject.transform.localScale = buttonTextScale;

				AddUI("", textObject, new Vector2(450f, 85f), Color.black);

				Buttons.ModButtonTrigger mbt = go.AddComponent<Buttons.ModButtonTrigger>();
			}
		}

		private void AddUI(string text, GameObject go, Vector2 rectSize, Color textColor)
		{
			RectTransform rect = go.AddComponent<RectTransform>();
			go.AddComponent<CanvasRenderer>();
			Text textUI = go.AddComponent<Text>();

			if (rect != null)
				rect.sizeDelta = rectSize;

			if (textUI != null) {
				textUI.fontSize = 50;
				textUI.fontStyle = FontStyle.Bold;
				textUI.alignment = TextAnchor.MiddleCenter;
				textUI.font = defaultFont;
				textUI.supportRichText = false;
				textUI.color = textColor;
				textUI.text = text;
			}
		}

		private GameObject CreateButton(string name)
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			if (go == null)
				return null;

			Collider col = go.GetComponent<BoxCollider>();
			if (col != null)
				col.isTrigger = true;

			go.transform.SetParent(this.gameObject.transform);
			go.name = name != null ? name : "HauntedModMenuButton";
			go.GetComponent<Renderer>()?.material?.SetColor("_Color", disabledColor);

			return go;
		}


		private void SetLocal(Transform goTrans, Vector3 localPos, Vector3 localScale, Quaternion localRotation, Transform parent = null)
		{
			if (goTrans == null)
				return;

			if (parent == null)
				parent = this.gameObject.transform;

			goTrans.SetParent(parent);
			goTrans.localPosition = localPos;
			goTrans.localRotation = localRotation;
			goTrans.localScale = localScale;
		}
	}
}
