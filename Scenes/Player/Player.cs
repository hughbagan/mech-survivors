using Godot;
using System;

public class Player : KinematicBody2D
{
    private PackedScene bulletScene;
    private Vector2 velocity = new Vector2();
    private float shootTimer = 0.0f;
    private float burstTimer = 0.0f;
    private int burstRounds = 0;

    public int movespeed = 100;
    public float shootInterval = 0.5f; // time between rounds in seconds
    public float burstInterval = 0.1f; // time between individual shots
    public int burstMax = 2; // shots per round
    public int damage = 1; // passed to bullet
    public int hp = 10;

    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/Bullet.tscn");
    }

    public override void _Process(float delta)
    {
        // Move
        velocity = new Vector2();
        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;
        velocity = velocity.Normalized() * movespeed;
        MoveAndSlide(velocity);

        // Shoot
        shootTimer += delta;
        burstTimer += delta;
        if (shootTimer >= shootInterval && burstRounds == 0)
            burstRounds = burstMax;
        else if (shootTimer >= shootInterval
                && burstRounds > 0
                && burstTimer >= burstInterval)
        {
            // LookAt the nearest enemy
            float nearestDist = float.PositiveInfinity;
            Enemy nearestEnemy = null;
            // this could become expensive...
            foreach (Enemy enemy in GetNode<YSort>("/root/Level/Enemies").GetChildren())
            {
                float dist = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (dist < nearestDist && enemy.onScreen)
                {
                    nearestDist = dist;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null)
            {
                LookAt(nearestEnemy.GlobalPosition);
                Bullet bulletInstance = (Bullet) bulletScene.Instance();
                bulletInstance.Position = Position;
                bulletInstance.Rotation = Rotation;
                bulletInstance.damage = damage;
                GetParent().AddChild(bulletInstance);
                burstTimer = 0.0f;
                burstRounds--;
                if (burstRounds == 0)
                    shootTimer = 0.0f;
            }
        }
    }
}
