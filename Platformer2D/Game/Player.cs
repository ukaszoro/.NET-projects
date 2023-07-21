using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Platformer2D;
public enum State
{
    Idle = 0,
    Running = 1,
    InAir = 2,
    Breaking = 3,
}
public class Player : IGameEntity
{
    public string Type { get; }
    public bool[] Collision { get; set;}
    public int DrawOrder { get; set; }
    public int UpdateOrder { get; set; }

    public Texture2D Texture { get; }
    //public Vector2 Pos { get; set; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    float max_walk_speed;
    public float jump_speed;
    public float gravity_accel;
    public float max_jump;
    bool walk_input;
    //public bool jumped;
    float _world_gravity;
    
    State Player_state;
    Rectangle[] SourceRect;
    float Anim_timer;
    float Anim_threshold;
    int Anim_current;
    float rotation;
    //GraphicsDeviceManager _graphics;

    public Player(int x, int y, ref ContentManager Content)
    {
        // tmp = new(texture.Width / 2, texture.Heighti); //to make sure pos shows the down middle point of the texture
        Texture = Content.Load<Texture2D>("player");
        Type = "player";
        Collision = new bool[4];
        //Pos = new Vector2(x, y) * Tile.Size;
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        jump_speed = 0f;
        max_jump = 700f;
        gravity_accel = 0f;
        _world_gravity = 10f;

        SourceRect = new Rectangle[6];
        SourceRect[0] = new(0,0,40,40);
        SourceRect[1] = new(40,0,40,40);
        SourceRect[2] = new(80,0,40,40);
        SourceRect[3] = new(120,0,40,40);
        SourceRect[4] = new(160,0,40,40);
        SourceRect[5] = new(200,0,40,40);
        Anim_threshold = 50;
    }

    public void HandleInput(Player player)
    {
        var kstate = Keyboard.GetState();


        if (kstate.IsKeyDown(Keys.Up) && player.Collision[0] == true && !player.Collision[3])
        {
            player.Collision[0] = false;
            player.jump_speed = player.max_jump;
        }
        // if (kstate.IsKeyUp(Keys.Up) && player1.jump_speed < 0.4f * player1.max_jump && player1.jumped) {
        //     player1.jump_speed = 0.1f * player1.max_jump;
        //     player1.jumped = false;
        // }
        // if (kstate.IsKeyDown(Keys.Down)) {
        //     Pos_Y += 10f; 
        // }
        if (kstate.IsKeyDown(Keys.Left))
        {
            player.walk_speed -= (Math.Abs(0.2f * player.walk_speed) + 15);
            walk_input = true;
        }
        if (kstate.IsKeyDown(Keys.Right))
        {
            player.walk_speed += Math.Abs(0.2f * player.walk_speed) + 15;
            walk_input = true;
        }
        if (walk_input == true && kstate.IsKeyUp(Keys.Left) && kstate.IsKeyUp(Keys.Right))
        {
            walk_input = false;
        }
        if (!kstate.IsKeyDown(Keys.Right) && !kstate.IsKeyDown(Keys.Left))
        {
            if (player.walk_speed > 10)
                player.walk_speed -= Math.Abs(0.15f * player.walk_speed) + 10;
            if (player.walk_speed < -10)
                player.walk_speed += Math.Abs(0.15f * player.walk_speed) + 10;
            if (player.walk_speed > -10 && player.walk_speed < 10)
                player.walk_speed = 0;
        }
        if (kstate.IsKeyDown(Keys.Z))
        {
            player.max_walk_speed = 500;
        }
        else
        {
            player.max_walk_speed = 300;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Pos_X > 10 * 40 && Pos_X < 216 * 40)
            Camera2D.X = Pos_X - 400;
        if (Pos_Y > 440)
            Camera2D.Y = 600;
        Check_state();
        if (Player_state == State.Idle)
            Anim_current = 0;
        if (Player_state == State.Running)
        {
            if (Anim_timer >= Anim_threshold)
            {
                Anim_current = 1 + ((Anim_current + 1) % 3);
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (walk_speed < 0)
            {
                rotation = 0;
            }
            else
                rotation = 1;
        }
        if (Player_state == State.InAir)
            Anim_current = 4;
        if (Player_state == State.Breaking)
            Anim_current = 5;

        spriteBatch.Draw(
        this.Texture,
        new Vector2(Pos_X, Pos_Y), 
        SourceRect[Anim_current],
        Color.White,
        0f,
        Camera2D,
        new Vector2(1, 1),
        (rotation == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
        0f
        );

    }
    public void Update(GameTime gameTime)
    {
        HandleInput(this);


        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (walk_speed > max_walk_speed)
            walk_speed -= 0.5f * Math.Abs(max_walk_speed - walk_speed);
        if (walk_speed < -max_walk_speed)
            walk_speed += 0.5f * Math.Abs(max_walk_speed + walk_speed);
                    
        if (Collision[1] || Collision[2])
            walk_speed = 0;
        
        Pos_X += walk_speed * Elapsed_time;
        
        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_jump * 2)
                gravity_accel = max_jump * 2;
        }
        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
            jump_speed = 0;
        }
        
        Pos_Y -= (jump_speed - gravity_accel) * Elapsed_time;
    }
    void Check_state()
    {
        if (!Collision[0])
            Player_state = State.InAir;
        else if (walk_speed != 0 && !walk_input)
            Player_state = State.Breaking;
        else if (walk_speed != 0)
            Player_state = State.Running;
        else
            Player_state = State.Idle;
    }
}
