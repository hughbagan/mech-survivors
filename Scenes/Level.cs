using Godot;
using System;
using System.Drawing;

public class Level : Node2D
{
    private PackedScene enemyScene;
    private int width;
    private int height;
    private Timer spawnTimer;

    public override void _Ready()
    {
        enemyScene = GD.Load<PackedScene>("res://Scenes/Enemy/Enemy.tscn");
        width = (int) GetViewport().Size.x;
        height = (int) GetViewport().Size.y;
        spawnTimer = GetNode<Timer>("SpawnTimer");
        spawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");
        spawnTimer.Start();
        GD.Randomize();
    }

    public void _on_SpawnTimer_timeout()
    {
        for (int i=0; i<3; i++)
        {
            Enemy enemyInstance = (Enemy) enemyScene.Instance();
            int spawnX;
            int spawnY;
            if (GD.Randf() > 0.5)
            {
                spawnX = (int) GD.RandRange(32, width-32);
                spawnY = GD.Randf() > 0.5 ? -32 : height+32;
            }
            else
            {
                spawnX = GD.Randf() > 0.5 ? -32 : width+32;
                spawnY = (int) GD.RandRange(32, height-32);
            }
            Vector2 viewportTransform = GetViewportTransform().origin;
            // enemyInstance.GlobalPosition = new Vector2(
            //     viewportTransform.x + spawnX,
            //     viewportTransform.y + spawnY);
            GD.Print(GlobalPosition, enemyInstance.GlobalPosition, GetViewportTransform());
            GetNode<YSort>("/root/Level/Enemies").AddChild(enemyInstance);
        }
    }
}
