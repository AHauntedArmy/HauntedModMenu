using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;

namespace HauntedModMenu.Menu
{
	class MenuView : MonoBehaviour
	{
		private static readonly string emptyName = "<------------------>";
		private static readonly Color enabledColor = new Color(0.4951f, 0.7075f, 0.3771f, 1f);
		private static readonly Color disabledColor = new Color(0.8396f, 0.2495f, 0.2495f, 1f);

		private void Awake()
		{
			LoadMenu();
		}

		private void LoadMenu()
		{
			Debug.Log("loading menu");

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
			int loopIndex;

			for (loopIndex = 0; loopIndex < 2; loopIndex++) {
				go = new GameObject(textInfo[loopIndex, 0]);

				if (go != null) {
					SetLocal(go.transform, positions[loopIndex], new Vector3(0.0018f, 0.0018f, 0f), Quaternion.identity);
					AddUI(textInfo[loopIndex, 1], go, 50, new Vector2(555f, 60f), new Color(0.6132f, 0.6132f, 0.6132f, 1f));
				}
			}

			go = null;
			GameObject textObject = null;
			Vector3 buttonPos = new Vector3(0f, 0.3f, -1f);
			Vector3 buttonTextPos = new Vector3(0f, 0f, -0.51f);
			Vector3 buttonScale = new Vector3(0.75f, 0.1f, 0.75f);
			Vector3 buttonTextScale = new Vector3(0.002208869f, 0.01262004f, 1f);

			for (loopIndex = 0; loopIndex < 5; loopIndex++) {

				go = CreateButton($"ModButton{loopIndex}");
				if (go == null)
					continue;

				SetLocal(go.transform, buttonPos, buttonScale, Quaternion.identity);	
				buttonPos.y -= 0.135f;

				textObject = new GameObject($"ButtonText{loopIndex}");
				if (textObject == null)
					continue;

				SetLocal(textObject.transform, buttonTextPos, buttonTextScale, Quaternion.identity, go.transform);
				AddUI(emptyName, textObject, 45, new Vector2(450f, 85f), Color.black);

				Buttons.ModButtonTrigger mbt = go.AddComponent<Buttons.ModButtonTrigger>();
				if (mbt != null) {
					// replace this with color's loaded from config in the future
					mbt.EnabledColor = enabledColor;
					mbt.DisabledColor = disabledColor;
				}
			}
		}

		private void AddUI(string text, GameObject go, int fontSize, Vector2 rectSize, Color textColor)
		{
			RectTransform rect = go.AddComponent<RectTransform>();
			go.AddComponent<CanvasRenderer>();
			Text textUI = go.AddComponent<Text>();

			if (rect != null)
				rect.sizeDelta = rectSize;

			if (textUI != null) {
				textUI.fontSize = fontSize;
				textUI.fontStyle = FontStyle.Normal;
				textUI.alignment = TextAnchor.MiddleCenter;
				textUI.font = Utils.RefCache.CustomFont;
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
			// go.GetComponent<Renderer>()?.material?.SetColor("_Color", disabledColor);

			return go;
		}


		private void SetLocal(Transform goTrans, in Vector3 localPos, in Vector3 localScale, in Quaternion localRotation, Transform parent = null)
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
