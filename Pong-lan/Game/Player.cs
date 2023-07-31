using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong_lan;

public class Player : IGameEntity
{
    Texture2D Texture;
    public float Horizontal_speed;
    Vector2 Pos;
    GameWindow Window;
    Rectangle Window_bounds = new(1,1,1,1);    
    int Hight;
    int Width;
    int Player_number;
    
    public Player(GraphicsDeviceManager _graphics, GameWindow window, int player)
    { 
        Window = window;
        Texture = new Texture2D(_graphics.GraphicsDevice,1,1);
        Texture.SetData<Color>(new Color[] {Color.White});
        Hight = _graphics.PreferredBackBufferHeight / 5;
        Width = Hight / 10;
        Player_number = player;
    }
    public void HandleInput(float elapsed_time)
    {
        var kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.Up))
        {
            Horizontal_speed = -Window.ClientBounds.Height / 1.2f * elapsed_time;
        }
        if (kstate.IsKeyDown(Keys.Down))
        {
            Horizontal_speed = Window.ClientBounds.Height / 1.2f * elapsed_time;
        }
        if (kstate.IsKeyUp(Keys.Up) && kstate.IsKeyUp(Keys.Down))
        {
            if (Horizontal_speed > 10)
                Horizontal_speed -= Math.Abs(0.1f * Horizontal_speed) + 20;
            else if (Horizontal_speed < -10)
                Horizontal_speed += Math.Abs(0.1f * Horizontal_speed) + 20;
            else
                Horizontal_speed = 0;
        }
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        var Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Window_bounds != Window.ClientBounds)
        {
            Pos.Y *= (float)Window.ClientBounds.Height / Window_bounds.Height;
            Window_bounds = Window.ClientBounds;
            Hight = Window_bounds.Height / 5;
            Width = Hight / 10;
            if (Player_number == 1)
                Pos.X = Window_bounds.Width / 40;
            else if (Player_number == 2)
                Pos.X = Window_bounds.Width * 39f / 40 - Width;
        }
        HandleInput(Elapsed_time);
        if (Pos.Y <= 0)
            Pos.Y = 0;
        else if (Pos.Y + Hight > Window_bounds.Height)
            Pos.Y = Window_bounds.Height - Hight;
        
        Pos.Y += Horizontal_speed;
    }
    public override void Draw(GameTime gameTimei, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            new Rectangle((int)Pos.X, (int)Pos.Y, Width, Hight),
            null,
            Color.White
            );
    }
}
