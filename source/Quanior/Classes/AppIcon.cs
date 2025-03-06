using UnityEngine;

[CreateAssetMenu(fileName = "AppIcon", menuName = "Scriptable Objects/AppIcon")]
public class AppIcon : ScriptableObject
{
    public Texture2D Icon;
    public string AppName;
    public string SceneName;
    public Color TransitionColor;
}
