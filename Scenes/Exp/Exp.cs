using Godot;
using System;

public class Exp : Area2D
{
    private Globals globals;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
    }

    private void _on_Exp_body_entered(Node body)
    {
        if (body is Player)
        {
            globals.AddScore(1);
            QueueFree();
        }
    }
}
