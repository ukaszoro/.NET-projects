using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Platformer2D;

public enum TileCollision
{
    Passable = 0,
    Impassable = 1,
    Breakable = 2,
    Mysteryblock = 3,
    Deathzone = 4,
}
public class Tile
{
    public Texture2D _texture;
    public TileCollision _collision;

    public const int Width = 40;
    public const int Height = 40;

    public static readonly Vector2 Size = new Vector2(Width, Height);
    public bool Hit { get; set; }
    public bool Break { get; set; }
    public Vector2 Hit_anim = new(0,0);
    public float Anim_timer;
    public int Anim_current;
    float Anim_X = 0;
    Rectangle[] Sourcerect;
    SoundEffect[] Sound;

    public Tile(Texture2D texture, TileCollision collision, ContentManager Content)
    {
        _texture = texture;
        _collision = collision;
        Anim_current = 0;
        Sourcerect = new Rectangle[4];
        if (collision == TileCollision.Mysteryblock)
        {
            Sourcerect[0] = new(0, 0, 40, 40);
            Sourcerect[1] = new(40, 0, 40, 40);
            Sourcerect[2] = new(80, 0, 40, 40);
            Sourcerect[3] = new(120, 0, 40, 40);
        }
        Sound = new SoundEffect[3];
        Sound[0] = Content.Load<SoundEffect>("block_bump");
        Sound[1] = Content.Load<SoundEffect>("block_break");
        Sound[2] = Content.Load<SoundEffect>("coin");
    }
    public void Draw(SpriteBatch spriteBatch, int x, int y, Vector2 Camera2D, GameTime gameTime)
    {
        Vector2 tmp_pos = new(x, y);
        if (_collision == TileCollision.Mysteryblock)
        {
            if (Anim_timer >= 400 / (Anim_current+1))
            {
                Anim_current = (Anim_current + 1) % 3;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        if (Break == true)
        {
            if (Anim_X < 5)
                Sound[1].Play();
            Sourcerect[0] = new(0,0,20,20);
            Sourcerect[1] = new(20,0,20,20);
            Sourcerect[2] = new(0,20,20,20);
            Sourcerect[3] = new(20,20,20,20);

            if (Anim_X > 20)
            {
                Hit = false;
                _collision = TileCollision.Passable;
            }
            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                    Hit_anim.X = -0.1f * 3 * (i + 1) * Anim_X;
                else
                    Hit_anim.X = 0.1f * 3 * (i - 1) * Anim_X;

                if (Anim_timer >= 7)
                {
                    Hit_anim.Y = (0.01f * -Anim_X * Anim_X + 1.2f * Anim_X);
                    Anim_X += 6;
                    Anim_timer = 0;
                }
                else
                    Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    
                spriteBatch.Draw(
                _texture,
                tmp_pos * Size,
                Sourcerect[i],
                Color.White,
                0f,
                Camera2D + Hit_anim,
                Vector2.One,
                SpriteEffects.None,
                0f
                );

            }

        }
        if (Hit && !Break)
        {
            if (Anim_X < 6 && _collision != TileCollision.Mysteryblock)
                Sound[0].Play();
            else if (Anim_X < 6 && _collision == TileCollision.Mysteryblock)
                Sound[2].Play();
            if (_collision == TileCollision.Mysteryblock)
            {
                Anim_current = 3;
            }
                
            if (Anim_timer >= 6)
            {
                Hit_anim.Y = (0.01f * -Anim_X * Anim_X + 1.2f * Anim_X);
                Anim_X += 9;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        if (Anim_X > 120 && Break == false)
        {
            Hit = false;
            Hit_anim.Y = 0;
            Anim_timer = 0;
            Anim_X = 0;
            Anim_current = 0;
        }

        if (Break != true)
            spriteBatch.Draw(
            _texture,
            tmp_pos * Size,
            (_collision == TileCollision.Mysteryblock ? Sourcerect[Anim_current] : null),
            Color.White,
            0f,
            Camera2D + Hit_anim,
            Vector2.One,
            SpriteEffects.None,
            0f
            );
    }
}
public class Level
{
    private Tile[,] tiles;
    private Texture2D[] Layers;

    private const int EntityLayer = 2;

    private Vector2 start;
    private Point exit;
    ContentManager Content;
    EntityManager e_manager = new EntityManager();
    CollisionManager c_manager;
    Hud Hud;
    Vector2 Camera2D;
    Song song;

    public Player player { get; set; }
    public int Score { get; }
    public bool reachedExit { get; }
    public TimeSpan TimeRemaining { get; }
    public int Lives { get; set; }
    public int coins { get; set; }
    public bool restart;
    
    public Level(Stream fileStream, int LevelIndex, ContentManager content, int lives)
    {
        Content = content;
        TimeRemaining = TimeSpan.FromMinutes(2.0);
        Lives = lives;
        LoadMapFile(fileStream);
        Hud = new(this, Content);
        song = content.Load<Song>("theme");
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.1f;
        SoundEffect.MasterVolume = 0.2f;
    }
    private void LoadMapFile(Stream fileStream)
    {
        int width;
        List<string> lines = new List<string>();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            string line = reader.ReadLine();
            width = line.Length;
            while (line != null)
            {
                lines.Add(line);
                if (line.Length != width)
                    throw new Exception(String.Format("The lenght of line {0} is different from previous lines", lines.Count));
                line = reader.ReadLine();
            }
        }

        tiles = new Tile[width, lines.Count]; // make a grid for the map

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                char tileType = lines[y][x];
                tiles[x, y] = LoadTile(tileType, x, y);
            }
        }
    }
    private Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '*':
                return null;
            case '#':
                return LoadBlock("block", TileCollision.Impassable);
            case '=':
                return LoadBlock("brick", TileCollision.Breakable);
            case '?':
                return LoadBlock("mystery_block", TileCollision.Mysteryblock);
            case '&':
                return LoadBlock("stair", TileCollision.Impassable);
            case 'I':
                return LoadBlock("pipe_part", TileCollision.Impassable);
            case '^':
                return LoadBlock("pipe_end", TileCollision.Impassable);
            case '.':
                return LoadBlock("invisible", TileCollision.Impassable);
            case '1':
                return LoadStartTile(x, y);
            case '@':
                return LoadBlock("scenary", TileCollision.Impassable);
            case 'G':
                return LoadGoombaTile(x, y);
            case 'K':
                return LoadKoopaTile(x,y);
            case ';':
                return LoadBlock("invisible",TileCollision.Deathzone);
            default:
                throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at {1}, {2}", tileType, x, y));
        }

    }
    private Tile LoadGoombaTile(int x, int y)
    {
        Goomba goomba = new(x, y, ref Content);
        e_manager.AddEntity(goomba);
        return null;
    }
    private Tile LoadKoopaTile(int x, int y)
    {
        Koopa koopa = new(x, y, ref Content);
        e_manager.AddEntity(koopa);
        return null;
    }
    private Tile LoadBlock(string name, TileCollision collision)
    {
        return new Tile(Content.Load<Texture2D>(name), collision, Content);
    }
    private Tile LoadStartTile(int x, int y)
    {
        if (player != null)
            throw new NotSupportedException("A lavel may only have one starting point.");

        player = new Player(x, y, ref Content, Lives);
        e_manager.AddEntity(player);

        return null;
    }

    public int Height { get { return tiles.GetLength(1); } }
    public int Width { get { return tiles.GetLength(0); } }
    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                if (tiles[i, j] != null)
                    if (tiles[i, j].Break == true)
                    {
                        tiles[i, j]._texture = Content.Load<Texture2D>("brick_debre");
                    }
            }
        e_manager.Update(gameTime);
        c_manager = new(e_manager);
        c_manager.check_collision(ref tiles);
        if (player.Health < Health.Dead)
        {
            MediaPlayer.Stop();
        }
        if (!e_manager._entities.Contains(player))
        {
            restart = true;
        }
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int i = 0; i < Width; i++)
            for (int j = 0; j < Height; j++)
            {
                if (!(tiles[i, j] == null))
                    tiles[i, j].Draw(spriteBatch, i, j, Camera2D, gameTime);
            }
        e_manager.Draw(spriteBatch, gameTime, ref Camera2D);
        Hud.Draw(spriteBatch, gameTime);
    }
}

