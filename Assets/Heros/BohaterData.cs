using UnityEngine;

[CreateAssetMenu(fileName = "NowyBohater", menuName = "Bohater/Nowy Bohater")]
public class BohaterData : ScriptableObject
{
    public string name;
    public Sprite Image;
    public string text1;
    public string text2;
    public string text3;
    public int bohaterId;
    public Fraction.fractionType fraction;
}
