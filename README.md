# Haunted Mod Menu
### **Important:** This only works in modded rooms
A PC tool for Gorilla Tag to be able to turn mods on and off without needing to go back to the computer. Tap your palm while looking at it to bring up the menu, tap it again to close it.

## Config
There are multiple configuration options available.
- LeftHand: which hand you want the menu to appear on
- Auto Close: if you want the menu to automatically close once its outside the look sensitivity.
- Look Sensitivty: how much your palm needs to be facing your eyes and how much it needs to be in view.
- Speed Sensitivty: how slow the hand needs to be moving to be able to press the buttons or activate the menu.

the colours for each button, text, and background are also fully customizable.

## For Developers
Make sure your mod implements unity's OnEnable and OnDisable in your plugin class, and add the `[Description("")]` attribute from `System.ComponentModel` that contains `"HauntedModMenu"`
```cs
using System;
using System.ComponentModel;
using BepInEx;
using Utilla;

namespace ExamplePlugin
{
    [Description("HauntedModMenu")]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin("com.ahauntedarmy.gorillatag.exampleplugin", "Example Plugin", "1.0.0")]
    public class ExamplePlugin : BaseUnityPlugin
    {
        bool inAllowedRoom = false;

       void OnEnable()
       {
           // do enable stuff
       }

       void OnDisable()
       {
           // do disable
       }
    }
}
```
