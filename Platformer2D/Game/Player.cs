using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Platformer2D;
public enum Health
{
    Dead = 0,
    Small = 1,
    Big = 2,
    Flower = 3,
}
public enum State
{
    Idle = 0,
    Running = 1,
    InAir = 2,
    Breaking = 3,
    Crouching = 4,
}
public class Player : IGameEntity
{
    public string Type { get; }
    public int DrawOrder { get; set; }
    public int UpdateOrder { get; set; }

    public float Pos_X { get; set; } // Variables for movement
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    float max_walk_speed;
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public float max_jump;
    bool walk_input;
    float _world_gravity;
    public bool[] Collision { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public SoundEffect[] Sound;

    public Texture2D Texture { get; }
    State Player_state;    // variables for animations
    Rectangle[] SourceRect;
    float Anim_timer;
    float Anim_threshold;
    int Anim_current;
    float rotation;
    Vector2 Ded_anim; float x; // Both for displaying a nice game over animation
    bool Lock;

    public Health Health { get; set; }
    public int Player_lifes { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    float Hurt_cooldown;
    bool i_frames;
    public bool remove { get; set; }

    public Player(int x, int y, ref ContentManager Content, int lives)
    {
        Texture = Content.Load<Texture2D>("player");
        Type = "player";
        Sound = new SoundEffect[2];
        Sound[0] = Content.Load<SoundEffect>("jump");
        Sound[1] = Content.Load<SoundEffect>("mario_die");
        
        Collision = new bool[4];
        Pos_X = x * Tile.Size.X;
        Pos_Y = y * Tile.Size.Y;
        jump_speed = 0f;
        max_jump = 700f;
        gravity_accel = 0f;
        _world_gravity = 10f;

        SourceRect = new Rectangle[14];
        SourceRect[0] = new(0, 0, 40, 40); //small Idle
        SourceRect[1] = new(40, 0, 40, 40); //small Running1
        SourceRect[2] = new(80, 0, 40, 40); //small Running2
        SourceRect[3] = new(120, 0, 40, 40); //small Running3
        SourceRect[4] = new(160, 0, 40, 40); //small Jumping
        SourceRect[5] = new(200, 0, 40, 40); //small Breaking
        SourceRect[6] = new(240, 0, 40, 40); //small game over
        SourceRect[7] = new(0, 40, 40, 80); //big Idle
        SourceRect[8] = new(40, 40, 40, 80); //big Running1
        SourceRect[9] = new(80, 40, 40, 80); //big Running2
        SourceRect[10] = new(120, 40, 40, 80); //big Running3
        SourceRect[11] = new(160, 40, 40, 80); //big Jumping
        SourceRect[12] = new(200, 40, 40, 80);//big Breaking
        SourceRect[13] = new(240,40,1,1); // Empty for i-frames

        Anim_threshold = 50;
        Ded_anim = new(0, 0);
        x = 0;
        Hurt_cooldown = 0f;

        Health = Health.Small;
        Height = 40; Width = 40;
        Player_lifes = lives;
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
        if (kstate.IsKeyDown(Keys.Down))
        {
            Player_state = State.Crouching;
        }
        else if (kstate.IsKeyUp(Keys.Down))
        {
            Player_state = State.Idle;
        }
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
            player.max_walk_speed = 350;
        }
        else
        {
            player.max_walk_speed = 250;
        }
        if (kstate.IsKeyDown(Keys.D))
        {
            Health++;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Pos_X > 10 * 40 && Pos_X < 213 * 40 - 400)
            Camera2D.X = Pos_X - 400;
        if (Pos_Y > 601)
            Camera2D.Y = 720;
        else 
            Camera2D.Y = 0;
        if (Pos_X > 57 * Width && Pos_X < 60 * Width && Collision[0] && Player_state == State.Crouching)
        {
            Pos_Y = 20 * Width;
            Pos_X = 151 * Width;
        }
        if (Pos_X > 161 * Width && Pos_X < 162 * Width && Pos_Y > 29 * Width && walk_speed > 20)
        {
            Pos_Y = 9 * Width;
            Pos_X = 164.5f * Width;
        }
        
        if (Hurt == true && Hurt_cooldown <= 0)
        {
            Hurt = false;
            Hurt_cooldown = 1500;
            Health--;
            Console.Write(Health);
        }
        else if (Hurt_cooldown >= 0)
        {
            Hurt = false;
            Hurt_cooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        Check_state();
        if (Player_state == State.Idle)
            Anim_current = 0;
        if (Player_state == State.Running && !(Collision[1] || Collision[2]))
        {
            if (Anim_timer >= Anim_threshold)
            {
                if (Anim_current > 7)
                    Anim_current -= 7;
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
        if (Health > Health.Small && Anim_current < 7)
        {
            if (Height == 40)
                Pos_Y -= 40;
            Anim_current += 7;
            Height = 80;

        }
        if (Anim_current < 7)
            Height = 40;

        if (Health <= Health.Dead)
        {
            Anim_current = 6;
            walk_speed = 0;
            gravity_accel = 0; //set all movement to 0 so mario doesn't move at all
            jump_speed = 0;
            if (x < 1)
            {
                Sound[1].Play();
            }

            if (Anim_timer >= 20)
            {
                Ded_anim.Y = (0.01f * -x * x + 3 * x);
                x += 4f;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (x > 550) // good moment, not too long after death on the ground, and pretty decent when somehowe dying on top of the map
            {
                remove = true;
                //check for more lifes and if there aren't any, do a game over and close?
                if (Player_lifes < 0)
                {

                }
                Player_lifes--;
            }
        }
        if (Hurt_cooldown > 0 && Health > Health.Dead)
        {
            i_frames = i_frames ^ true;
        }
        else
            i_frames = false;
        
        spriteBatch.Draw(
        this.Texture,
        new Vector2(Pos_X, Pos_Y),
        SourceRect[Anim_current],
        (Health == Health.Flower ? Color.LightSalmon : (i_frames ? Color.PaleVioletRed : Color.White)),
        0f,
        Camera2D + Ded_anim,
        new Vector2(1, 1),
        (rotation == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally),
        0f
        );

    }
    public void Update(GameTime gameTime)
    {
        if (Health != Health.Dead)
            HandleInput(this);


        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (walk_speed > max_walk_speed)
            walk_speed -= 0.5f * Math.Abs(max_walk_speed - walk_speed);
        if (walk_speed < -max_walk_speed)
            walk_speed += 0.5f * Math.Abs(max_walk_speed + walk_speed);

        if (Collision[1] || Collision[2])
            walk_speed /= 2;

        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_jump * 1.86f)
                gravity_accel = max_jump * 1.86f;
        }
        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
            jump_speed = 0;
        }
        if (jump_speed > 0 && gravity_accel < 1.5*_world_gravity)
            Sound[0].Play();
        Pos_Y -= (jump_speed - gravity_accel) * Elapsed_time;
    }
    void Check_state()
    {
        if (!Collision[0])
            Player_state = State.InAir;
        else if ((walk_speed > 60 || walk_speed < -20) && !walk_input)
            Player_state = State.Breaking;
        else if ((walk_speed > 60 || walk_speed < -20) && !(Collision[1] || Collision[2]))
            Player_state = State.Running;
        else if (Player_state != State.Crouching)
            Player_state = State.Idle;
        //State.Running if might seem weird, but there was a bug where the animation would start constantly when running into a wall
    }
}
