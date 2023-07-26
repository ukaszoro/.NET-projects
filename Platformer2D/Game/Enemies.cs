using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Platformer2D;

public class Goomba : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }

    float max_fall;
    float _world_gravity;
    int Anim_current;
    float Anim_timer;
    float Anim_threshold;
    int rotation;
    Vector2 Dead_anim;
    Rectangle[] SourceRect;
    float X;

    public Goomba(int x, int y, ref ContentManager Content)
    {
        Texture = Content.Load<Texture2D>("goomba");
        Type = "goomba";
        Collision = new bool[4];
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        gravity_accel = 1f;
        _world_gravity = 10f;
        max_fall = 400;

        Anim_threshold = 180;
        Dead_anim = new(0,0);
        walk_speed = 0;
        Anim_current = 0;
        X = 0;

        SourceRect = new Rectangle[3];
        SourceRect[0] = new(0,0,40,40);
        SourceRect[1] = new(40,0,40,40);
        SourceRect[2] = new(80,0,40,40);
        
        Height = 40; Width = 40;
    }
    
    public void Update(GameTime gameTime)
    {

        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }
        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
        }
        Pos_Y += gravity_accel * Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Hurt_up == true)
        {
            Type = "ded";
            if (Anim_timer >= 10)
            {
                Dead_anim.Y = (0.01f * -X * X + X);
                X += 4;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (X > 300)
                remove = true;
        }
        else if (Hurt == true)
        {
            Anim_current = 2;
            if (Anim_timer >= 400)
            {
                remove = true;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        else
        {
            if (Anim_timer >= Anim_threshold)
            {
                Anim_current = (Anim_current + 1) % 2;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        if (Pos_X - Camera2D.X < 1000 && walk_speed == 0)
            walk_speed = -60;

        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        SourceRect[Anim_current],
        Color.White,
        0f,
        Camera2D + Dead_anim,
        new Vector2(1, 1),
        (Hurt_up ? SpriteEffects.FlipVertically : (rotation == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally)),
        0f
        );
    }
}
public class Koopa : IGameEntity
{
    public string Type { get; set;}
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }

    float max_fall;
    float _world_gravity;
    int Anim_current;
    float Anim_timer;
    float Anim_threshold;
    int rotation;
    Vector2 Dead_anim;
    Rectangle[] SourceRect;
    bool spawned;

    public Koopa(int x, int y, ref ContentManager Content)
    {
        Texture = Content.Load<Texture2D>("koopa");
        Type = "koopa";
        Collision = new bool[4];
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        gravity_accel = 1f;
        _world_gravity = 10f;
        max_fall = 400;

        Anim_threshold = 180;
        Dead_anim = new(0,0);
        walk_speed = 0;
        Anim_current = 0;

        SourceRect = new Rectangle[3];
        SourceRect[0] = new(0,0,40,60);
        SourceRect[1] = new(40,0,40,60);
        SourceRect[2] = new(80,0,40,60);
        
        Height = 40; Width = 40;
    }
    
    public void Update(GameTime gameTime)
    {

        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }
        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
        }
        Pos_Y += gravity_accel * Elapsed_time;
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Hurt == true && Type == "koopa")
        {
            Type = "koopa_shell";
            walk_speed = 0;
            Hurt = false;
        }
        else if (Hurt == true && walk_speed == 0)
        {
            walk_speed = 300;
            Hurt = false;
        }
        else if (Hurt == true && walk_speed != 0)
        {
            walk_speed = 0;
            Hurt = false;
        }
        else
        {
            if (Anim_timer >= Anim_threshold)
            {
                Anim_current = (Anim_current + 1) % 2;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        if (Pos_X - Camera2D.X < 1000 && spawned == false)
        {
            walk_speed = -60;
            spawned = true;
        }
        if (walk_speed < 0)
            rotation = 1;
        else
            rotation = 0;
        if (Type == "koopa_shell")
            Anim_current = 2;
        spriteBatch.Draw(
        Texture,
        new Vector2(Pos_X, Pos_Y),
        SourceRect[Anim_current],
        Color.White,
        0f,
        Camera2D + new Vector2(0, 20) + Dead_anim,
        new Vector2(1, 1),
        (rotation == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
        0f
        );
    }
}