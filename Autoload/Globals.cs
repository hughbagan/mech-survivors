using Godot;
using System;

public class Globals : Node
{
    public int exp = 0;

    public void AddScore(int add)
    {
        exp += add;
        GetNode<Label>("/root/Level/Score").Text = exp.ToString();
    }
}
