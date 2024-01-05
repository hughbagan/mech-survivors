using Godot;
using System;

public class Bullet : KinematicBody2D
{
    private const float Speed = 1000.0f;
    private const int Damage = 1;

    public override void _Ready()
    {
        GetNode<Timer>("DeathTimer").Connect("timeout", this, "_on_Timer_timeout");
        GetNode<Timer>("DeathTimer").Start();
    }

    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(Speed, 0).Rotated(Rotation);
        // MoveAndSlide(new Vector2(Speed, 0).Rotated(Rotation));
        KinematicCollision2D collision = MoveAndCollide(velocity * delta);
        if (collision != null)
        {
            if (collision.Collider is Enemy)
            {
                Enemy enemy = (Enemy) collision.Collider;
                enemy.TakeDamage(Damage);
                QueueFree();
            }
        }
    }

    private void _on_Timer_timeout()
    {
        QueueFree();
    }
}
