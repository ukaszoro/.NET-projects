using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace Pong_lan;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private EntityManager E_manager;
    public CollisionManager C_manager;
    SpriteFont font;
    StringBuilder S_builder = new StringBuilder();
    int text_cooldown;
    bool ip_input;

    StringBuilder myTextBoxDisplayCharacters = new StringBuilder();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        E_manager = new();
        C_manager = new(E_manager._entities);
        
        
        Player player = new(_graphics,Window ,1);
        E_manager.AddEntity(player);
        player = new(_graphics,Window,2);
        E_manager.AddEntity(player);
        Ball ball = new(_graphics, Window);
        E_manager.AddEntity(ball);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here
        if (ip_input == false)
            Read_textbox();
        else
        {
        E_manager.Update(gameTime);
        C_manager.check_collision();
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0, 0, 0));
        _spriteBatch.Begin();

        if (ip_input == false)
        {
            font = Content.Load<SpriteFont>("font");
            _spriteBatch.DrawString(font, "Enter Lan ip and confirm with enter", new Vector2((Window.ClientBounds.Width / 2 - 393), (Window.ClientBounds.Height / 2 - 40)), Color.Lime);
            _spriteBatch.DrawString(font, S_builder.ToString(), new Vector2((Window.ClientBounds.Width / 2 - 200), Window.ClientBounds.Height / 2), Color.Lime);
        }
        else
            E_manager.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    protected void Read_textbox()
    {
        text_cooldown++;
        if (text_cooldown > 4)
        {
            text_cooldown = 0;
            KeyboardState state;
            state = Keyboard.GetState();
            foreach (var key in state.GetPressedKeys())
            {
                if (key == Keys.Back)
                {
                    if (S_builder.Length != 0)
                    {
                        int start = S_builder.Length - 1;
                        S_builder.Remove(start, 1);
                    }
                    continue;
                }
                if (key == Keys.Enter)
                {
                    ip_input = true;
                }
                if (S_builder.Length > 18)
                    continue;
                if ((int)key > 47 && (int)key < 58)
                    S_builder.Append((char)key);
                if (key == Keys.OemPeriod)
                    S_builder.Append('.');
            }
        }
    }
}
