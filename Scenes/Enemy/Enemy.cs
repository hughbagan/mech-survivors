using Godot;
using System;

public class Enemy : KinematicBody2D
{
    private const float Speed = 50.0f;
    private const int MaxHealth = 4;
    private int currentHealth = MaxHealth;
    private Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        if (player == null)
            player = GetNode<Player>("/root/Level/Player");
        else
        {
            LookAt(player.GlobalPosition);
            MoveAndSlide(new Vector2(Speed, 0).Rotated(Rotation));
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            QueueFree();
    }
}
