using System.Linq;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;

using HauntedModMenu.Utils;

namespace HauntedModMenu.Menu
{
	class MenuView : MonoBehaviour
	{
		private static readonly string emptyName = "<------------------>";
		private static readonly Color enabledColor = new Color(0.4951f, 0.7075f, 0.3771f, 1f);
		private static readonly Color disabledColor = new Color(0.8396f, 0.2495f, 0.2495f, 1f);

		private Buttons.PageButtonTrigger nextPageButton = null;
		private Buttons.PageButtonTrigger previousPageButton = null;
		private Buttons.ModButtonTrigger[] modButtonArray = new Buttons.ModButtonTrigger[5] { null, null, null, null, null };

		private int page;
		private int pageMax;
		const int pageSize = 5;

		private void Awake()
		{
			page = 0;
			pageMax = Mathf.FloorToInt(RefCache.ModList.Count / (float)pageSize);

			LoadMenu();
			UpdateButtons();
		}

		private void NextPage()
		{
			if (page < pageMax) {
				page += 1;
				UpdateButtons();
			}
		}

		private void PreviousPage()
		{
			if (page > 0) {
				page -= 1;
				UpdateButtons();
			}
		}

		private void UpdateButtons()
		{
			List<ModInfo> currentMods = RefCache.ModList?.Skip(page * pageSize).Take(pageSize).ToList();
			int? modCount = currentMods?.Count;

			for (int index = 0; index < modButtonArray?.Length; index++) {
				Buttons.ModButtonTrigger button = modButtonArray[index];
				if (index < modCount) {
					
					if(button == null) 
						continue;

					button.ModTarget = currentMods[index];
					button.ButtonText.text = currentMods[index].Name;
				
				} else {
					Debug.Log("setting button to default");
					button.ModTarget = null;
					button.ButtonText.text = emptyName;
				}
			}

			if (page < pageMax) {
				nextPageButton.SetColour(true);
			
			} else {
				nextPageButton.SetColour(false);
			}

			if (page > 0) {
				previousPageButton.SetColour(true);
			
			} else {
				previousPageButton.SetColour(false);
			}
		}

		#region CREATE_MENU
		private void LoadMenu()
		{
			Debug.Log("loading menu");

			this.gameObject.GetComponent<Renderer>()?.material?.SetColor("_Color", Config.LoadData("Button Menu Config", "Background Colour", "The colour for the background", Color.black)); 

			RectTransform rect = this.gameObject.AddComponent<RectTransform>();
			Canvas canvas = this.gameObject.AddComponent<Canvas>();


			if (rect != null) {
				Debug.Log("setting menu canvas size");
				rect.sizeDelta = new Vector2(1f, 1f);
			}

			string[,] textInfo = new string[,] { {"MenuTitle", "Haunted Mod Menu" }, {"PageText", "Pages" } };
			Vector3[] positions = new Vector3[] { new Vector3(0f, 0.435f, -0.51f), new Vector3(0f, -0.4f, -0.51f) };

			int loopIndex;
			GameObject go = null;
			Quaternion zeroRotation = Quaternion.identity;

			for (loopIndex = 0; loopIndex < positions?.Length; loopIndex++) {
				go = new GameObject(textInfo[loopIndex, 0]);

				if (go != null) {
					SetLocal(go.transform, positions[loopIndex], new Vector3(0.0018f, 0.0018f, 0f), zeroRotation);
					AddUI(textInfo[loopIndex, 1], go, 50, new Vector2(555f, 60f), Config.LoadData("Button Menu Config", $"{textInfo[loopIndex, 0]} Text Colour", $"The colour for the {textInfo[loopIndex, 1]} text", new Color(0.6132f, 0.6132f, 0.6132f, 1f)));
					go = null;
				}
			}

			GameObject textObject = null;
			Vector2 rectSize = new Vector2(450f, 85f);
			Vector3 buttonPos = new Vector3(0f, 0.3f, -1f);
			Vector3 buttonTextPos = new Vector3(0f, 0f, -0.51f);
			Vector3 buttonScale = new Vector3(0.75f, 0.1f, 0.75f);
			Vector3 buttonTextScale = new Vector3(0.002208869f, 0.01262004f, 1f);

			for (loopIndex = 0; loopIndex < modButtonArray?.Length; loopIndex++) {
				
				// create the button
				go = CreateButton($"ModButton{loopIndex}");
				SetLocal(go?.transform, buttonPos, buttonScale, zeroRotation);	

				// creat the text object
				textObject = new GameObject($"ButtonText{loopIndex}");

				if (textObject != null) {
					SetLocal(textObject.transform, buttonTextPos, buttonTextScale, zeroRotation, go?.transform);
					AddUI(emptyName, textObject, 45, rectSize, Config.LoadData($"cButtonButton{loopIndex} Config", "Text Colour", "The button text colour", Color.black));

					// add the trigger script
					Buttons.ModButtonTrigger mbt = go?.AddComponent<Buttons.ModButtonTrigger>();
					if (mbt != null) {
						mbt.EnabledColor = Config.LoadData($"cButtonButton{loopIndex} Config", "Enabled Colour", "The enabled colour for the button", enabledColor);
						mbt.DisabledColor = Config.LoadData($"cButtonButton{loopIndex} Config", "Disabled Colour", "The disabled colour for the button", disabledColor);

						mbt.SetColour(false);
						modButtonArray[loopIndex] = mbt;
					}
				}

				// move the button position down
				buttonPos.y -= 0.135f;
			}

			buttonPos.x = -0.35f;
			buttonPos.y = -0.4f;
			buttonScale.x = 0.2f;

			textInfo[0, 0] = "Previous";
			textInfo[0, 1] = "<<<<<<<<<<";
			textInfo[1, 0] = "Next";
			textInfo[1, 1] = ">>>>>>>>>>";

			Buttons.PageButtonTrigger[] pageTriggers = new Buttons.PageButtonTrigger[] { null, null };

			for (loopIndex = 0; loopIndex < pageTriggers?.Length; loopIndex++) {
				go = CreateButton($"{textInfo[loopIndex, 0]}PageButton");
				textObject = new GameObject($"{textInfo[loopIndex, 0]}PageText");

				AddUI(textInfo[loopIndex, 1], textObject, 50, rectSize, Config.LoadData($"c{textInfo[loopIndex, 0]} Button Config", "Text Colour", "The button text colour", Color.black));
				SetLocal(go?.transform, buttonPos, buttonScale, zeroRotation);
				SetLocal(textObject?.transform, buttonTextPos, buttonTextScale, zeroRotation, go?.transform);

				Buttons.PageButtonTrigger pbt = go?.AddComponent<Buttons.PageButtonTrigger>();
				if (pbt != null) {
					pbt.EnabledColor = Config.LoadData($"c{textInfo[loopIndex, 0]} Button Config", "Enabled Colour", "The enabled colour for the button", enabledColor);
					pbt.DisabledColor = Config.LoadData($"c{textInfo[loopIndex, 0]} Button Config", "Disabled Colour", "The disabled colour for the button", disabledColor);

					pbt.SetColour(false);
				}

				buttonPos.x *= -1f;
				pageTriggers[loopIndex] = pbt;
			}

			previousPageButton = pageTriggers[0];
			nextPageButton = pageTriggers[1];

			if(previousPageButton != null) 
				previousPageButton.PageUpdate = PreviousPage;

			if (nextPageButton != null)
				nextPageButton.PageUpdate = NextPage;
		}

		private void AddUI(string text, GameObject go, in int fontSize, in Vector2 rectSize, in Color textColor)
		{
			if (go == null)
				return;

			RectTransform rect = go.AddComponent<RectTransform>();
			go.AddComponent<CanvasRenderer>();
			Text textUI = go.AddComponent<Text>();

			if (rect != null)
				rect.sizeDelta = rectSize;

			if (textUI != null) {
				textUI.fontSize = fontSize;
				textUI.fontStyle = FontStyle.Normal;
				textUI.alignment = TextAnchor.MiddleCenter;
				textUI.font = RefCache.CustomFont;
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

			go.name = name != null ? name : "HauntedModMenuButton";

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

		#endregion
	}
}
