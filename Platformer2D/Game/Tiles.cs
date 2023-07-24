using Microsoft.Xna.Framework;
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
    float Anim_X = 0;

    public Tile(Texture2D texture, TileCollision collision)
    {
        _texture = texture;
        _collision = collision;
    }
    public void Draw(SpriteBatch spriteBatch, int x, int y, Vector2 Camera2D, GameTime gameTime)
    {
        Vector2 tmp_pos = new(x, y);
        if (Break == true)
        {
            Rectangle[] Sourcerect = new Rectangle[4];
            Sourcerect[0] = new(0,0,20,20);
            Sourcerect[1] = new(20,0,20,20);
            Sourcerect[2] = new(0,20,20,20);
            Sourcerect[3] = new(20,20,20,20);
            Hit = false;
            _collision = TileCollision.Passable;
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
        if (Hit == true)
        {
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
        }

        if (Break != true)
            spriteBatch.Draw(
            _texture,
            tmp_pos * Size,
            null,
            Color.White,
            0f,
            Camera2D + Hit_anim,
            Vector2.One,
            SpriteEffects.None,
            0f
            );
    }
}
class Level
{
    private Tile[,] tiles;
    private Texture2D[] Layers;

    private const int EntityLayer = 2;

    private Vector2 start;
    private Point exit;
    ContentManager Content;
    EntityManager e_manager = new EntityManager();
    CollisionManager c_manager;
    Vector2 Camera2D;

    public Player player { get; set; }
    public int Score { get; }
    public bool reachedExit { get; }
    public TimeSpan TimeRemaining { get; }
    public int Lives;
    public Level(Stream fileStream, int LevelIndex, ContentManager content)
    {
        Content = content;
        TimeRemaining = TimeSpan.FromMinutes(2.0);
        Lives = 3;
        LoadMapFile(fileStream);
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
    
    private Tile LoadBlock(string name, TileCollision collision)
    {
        return new Tile(Content.Load<Texture2D>(name), collision);
    }
    private Tile LoadStartTile(int x, int y)
    {
        if (player != null)
            throw new NotSupportedException("A lavel may only have one starting point.");

        player = new Player(x, y, ref Content, ref Lives);
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
    }
}

