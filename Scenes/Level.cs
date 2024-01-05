using Godot;
using System;
using System.Drawing;

public class Level : Node2D
{
    private float width;
    private float height;
    private Timer spawnTimer;

    public override void _Ready()
    {
        width = GetViewport().Size.x;
        height = GetViewport().Size.y;
        spawnTimer = GetNode<Timer>("SpawnTimer");
        spawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");
        spawnTimer.Start();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }

    public void _on_SpawnTimer_timeout()
    {
        // get random 0 - viewport width and height
        // then convert to global position and instance an Enemy
    }
}
