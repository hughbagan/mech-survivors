using Godot;
using System;
using System.Drawing;

public class Level : Node2D
{
    private PackedScene enemyScene;
    private Timer spawnTimer;
    private int width;
    private int height;

    public override void _Ready()
    {
        GD.Randomize();
        enemyScene = GD.Load<PackedScene>("res://Scenes/Enemy/Enemy.tscn");
        spawnTimer = GetNode<Timer>("SpawnTimer");
        spawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");
        spawnTimer.Start();
        width = (int) GetViewport().Size.x;
        height = (int) GetViewport().Size.y;
    }

    public void _on_SpawnTimer_timeout()
    {
        for (int i=0; i<3; i++)
        {
            Enemy enemyInstance = (Enemy) enemyScene.Instance();
            enemyInstance.Hide();
            GetNode<YSort>("/root/Level/Enemies").AddChild(enemyInstance);

            int spawnX;
            int spawnY;
            if (GD.Randf() > 0.5f)
            {
                spawnX = (int) GD.RandRange(32, width-32);
                spawnY = GD.Randf() > 0.5f ? -32 : height+32;
            }
            else
            {
                spawnX = GD.Randf() > 0.5f ? -32 : width+32;
                spawnY = (int) GD.RandRange(32, height-32);
            }
            Vector2 canvasPosition = enemyInstance.GetCanvasTransform().origin;
            enemyInstance.GlobalPosition = new Vector2(spawnX, spawnY) - canvasPosition;
            enemyInstance.Show();
        }
    }
}
