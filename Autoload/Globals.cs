using Godot;
using System;

public class Globals : Node
{
    public int exp = 0;
    public int level = 1;

    public void AddScore(int add)
    {
        exp += add;
        if (exp >= levelUpExp())
        {
            exp = 0;
            level++;
            GetNode<Level>("/root/Level").Upgrade();
        }
        GetNode<Label>("/root/Level/HUD/Score").Text = exp.ToString()+'/'+levelUpExp().ToString();
    }

    private int levelUpExp()
    {
        return 2*level+3;
    }
}
