using Godot;
using System;
using System.Drawing;

public class Level : Node2D
{
    private PackedScene enemyScene;
    private Globals globals;
    private Player player;
    private CenterContainer upgrades;
    private Timer spawnTimer;
    private int width;
    private int height;

    public override void _Ready()
    {
        GD.Randomize();
        enemyScene = GD.Load<PackedScene>("res://Scenes/Enemy/Enemy.tscn");
        globals = GetNode<Globals>("/root/Globals");
        globals.AddScore(0);
        player = GetNode<Player>("Player");
        upgrades = GetNode<CenterContainer>("HUD/Upgrades");
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

    public void Upgrade()
    {
        upgrades.Show();
        // Show 3 random upgrades
        Godot.Collections.Array upgradeButtons = upgrades.GetNode<GridContainer>("Grid").GetChildren();
        int i=0;
        while (true)
        {
            int randI = Math.Abs((int) GD.Randi() % upgradeButtons.Count);
            Button upgradeButton = (Button) upgradeButtons[randI];
            if (!upgradeButton.Visible)
            {
                upgradeButton.Show();
                i++;
            } else continue;
            if (i >= 3) break;
        }
        GetTree().Paused = true;
    }

    public void _on_Upgrade1_pressed() // movespeed
    {
        player.movespeed += 20;
        FinishUpgrade();
    }

    public void _on_Upgrade2_pressed() // rate of fire
    {
        player.shootInterval -= 0.05f;
        FinishUpgrade();
    }

    public void _on_Upgrade3_pressed() // damage
    {
        player.damage++;
        FinishUpgrade();
    }

    public void _on_Upgrade4_pressed() // HP
    {
        player.hp += 2;
        FinishUpgrade();
    }

    public void _on_Upgrade5_pressed() // num bullets
    {
        player.burstMax++;
        FinishUpgrade();
    }

    private void FinishUpgrade()
    {
        foreach (Button upgradeButton in upgrades.GetNode<GridContainer>("Grid").GetChildren())
            upgradeButton.Hide();
        upgrades.Hide();
        GetTree().Paused = false;
    }
}
