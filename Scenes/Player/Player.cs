using Godot;
using System;

public class Player : KinematicBody2D
{
    private PackedScene bulletScene;
    private Vector2 velocity = new Vector2();
    private float shootTimer = 0.0f;
    private float burstTimer = 0.0f;
    private int burstRounds = 0;
    private const float ShootInterval = 2.0f; // time between rounds in seconds
    private const float BurstInterval = 0.25f; // time between individual shots
    private const int BurstMax = 4; // shots per round

    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/Bullet.tscn");
    }

    public override void _Process(float delta)
    {
        velocity = new Vector2();
        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;
        velocity = velocity.Normalized() * 200;
        MoveAndSlide(velocity);

        shootTimer += delta;
        burstTimer += delta;
        if (shootTimer >= ShootInterval && burstRounds == 0)
            burstRounds = BurstMax;
        else if (shootTimer >= ShootInterval
                && burstRounds > 0
                && burstTimer >= BurstInterval)
        {
            // LookAt the nearest enemy
            float nearestDist = float.PositiveInfinity;
            Enemy nearestEnemy = null;
            // this could become expensive...
            foreach (Enemy enemy in GetNode<YSort>("/root/Level/Enemies").GetChildren())
            {
                float dist = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (dist < nearestDist)
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
                GetParent().AddChild(bulletInstance);
                burstTimer = 0.0f;
                burstRounds--;
                if (burstRounds == 0)
                    shootTimer = 0.0f;
            }
        }
    }
}
