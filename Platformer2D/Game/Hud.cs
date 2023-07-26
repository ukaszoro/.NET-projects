using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer2D;

public class Hud
{
    Level Level;
    ContentManager Content;
    string text = "mario";
    SpriteFont Font;
    Texture2D Mario;
    Rectangle Mariohead = new(0,40,40,30);
    public Hud(Level level, ContentManager content)
    {
        Level = level;
        Content = content;
        Font = Content.Load<SpriteFont>("font");
        Mario = Content.Load<Texture2D>("player");
    }
    public void Update(GameTime gameTime)
    {
        
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.DrawString(Font,"Lives", new Vector2(18,0),Color.White);
        spriteBatch.Draw(Mario,new Rectangle(10,20,40,30) ,Mariohead, Color.White);
        spriteBatch.DrawString(Font,"X", new Vector2(55,30),Color.White);
        spriteBatch.DrawString(Font,Convert.ToString(Level.Lives), new Vector2(80,30),Color.White);
        spriteBatch.DrawString(Font,"Coins", new Vector2(220,0),Color.White);
        spriteBatch.DrawString(Font,Convert.ToString(Level.coins), new Vector2(245,30),Color.White);
        spriteBatch.DrawString(Font,"Score", new Vector2(420,0),Color.White);
        spriteBatch.DrawString(Font,Convert.ToString(Level.Score), new Vector2(430,30),Color.White);
        spriteBatch.DrawString(Font,"Time Left", new Vector2(620,0),Color.White);
        spriteBatch.DrawString(Font,Convert.ToString(Level.TimeRemaining.TotalSeconds), new Vector2(660,30),Color.White);
    }
}