using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Platformer2D;

public class CollisionManager
{
    private readonly List<IGameEntity> _entities;

    public CollisionManager(EntityManager e_manager)
    {
        _entities = e_manager._entities;
    }

    public void check_collision(ref ITile[,] Tiles, Level level)
    {
        foreach (IGameEntity entity1 in _entities.OrderBy(e => e.UpdateOrder))
        {
            Rectangle entity1_rect =
                new((int)entity1.Pos_X, (int)entity1.Pos_Y, (int)entity1.Width, (int)entity1.Height);
            entity1.Collision[0] = false; // Collision[0] - collision with the ground
            entity1.Collision[1] = false;
            entity1.Collision[2] = false;
            entity1.Collision[3] = false;
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    if (Tiles[x, y] == null)
                        continue;
                    Rectangle Tile = new(x * 40, y * 40, 40, 40);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(entity1_rect, Tile);
                    if (Tiles[x, y]._collision != TileCollision.Passable)
                    {
                        if (depth == Vector2.Zero)
                            continue;
                        if (Tiles[x, y]._collision == TileCollision.Deathzone)
                        {
                            entity1.Health = 0;
                            continue;
                        }

                        Vector2 absDepth = new(Math.Abs(depth.X), Math.Abs(depth.Y));
                        if (absDepth.Y < absDepth.X)
                        {
                            if (depth.Y > 0)
                            {
                                entity1.Collision[3] = true;
                                entity1.Pos_Y = Tile.Bottom;
                                if (Tiles[x, y]._collision == TileCollision.Breakable ||
                                    Tiles[x, y]._collision == TileCollision.Mysteryblock && entity1.Type == "player")
                                {
                                    Tiles[x, y].Hit = true;
                                    if (entity1.Height == 80)
                                        if (Tiles[x, y]._collision == TileCollision.Breakable)
                                            Tiles[x, y].Break = true;
                                }

                                if (Tiles[x, y]._collision == TileCollision.Mysteryblock && entity1.Type == "player")
                                {
                                    if (Tiles[x, y].Contains > 0)
                                    {
                                        level.coins++;
                                    }

                                    if (Tiles[x, y].Contains < 0)
                                    {
                                        level.Spawn_powerup(Tiles[x, y].Pos * Tiles[x, y].Size);
                                    }
                                }
                            }
                            else if (depth.Y < 0)
                            {
                                entity1.Collision[0] = true;
                                entity1.Pos_Y = Tile.Top - entity1.Height + 1;
                                if (Tiles[x, y].Hit == true)
                                    entity1.Hurt_up = true;
                            }
                        }

                        else if (absDepth.Y > absDepth.X)
                        {
                            if (depth.X < 0) // Right wall collision
                            {
                                entity1.Collision[1] = true;
                                entity1.Pos_X =
                                    Tile.Left - entity1.Width +
                                    0.2f; // weird value, but without it player vibrates when walking into a wall?
                                if (entity1.Type != "player")
                                    entity1.walk_speed = -entity1.walk_speed;
                            }

                            if (depth.X > 0) // Left wall collision
                            {
                                entity1.Collision[2] = true;
                                entity1.Pos_X = Tile.Right;
                                if (entity1.Type != "player")
                                    entity1.walk_speed = -entity1.walk_speed;
                            }
                        }
                    }
                }
            }

            foreach (IGameEntity entity2 in _entities.OrderBy(e => e.UpdateOrder))
            {
                if (entity1 == entity2)
                    continue;
                if (entity1.Type == "ded" || entity2.Type == "ded")
                    continue;
                Rectangle entity2_rect = new((int)entity2.Pos_X, (int)entity2.Pos_Y, (int)entity2.Width,
                    (int)entity2.Height);
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(entity1_rect, entity2_rect);
                if (depth == Vector2.One)
                    continue;
                Vector2 absDepth = new(Math.Abs(depth.X), Math.Abs(depth.Y));

                if (depth != Vector2.Zero && entity1.Type == "player" && entity2.Type == "mushroom")
                {
                    entity1.Health = Health.Big;
                    entity2.remove = true;
                    continue;
                }
                else if (depth != Vector2.Zero && entity2.Type == "player" && entity1.Type == "mushroom")
                {
                    entity2.Health = Health.Big;
                    entity1.remove = true;
                    continue;
                }

                if (depth != Vector2.Zero && entity1.Type == "player" && entity2.Type == "fflower")
                {
                    entity1.Health = Health.Flower;
                    entity2.remove = true;
                    continue;
                }
                else if (depth != Vector2.Zero && entity2.Type == "player" && entity1.Type == "fflower")
                {
                    entity2.Health = Health.Flower;
                    entity1.remove = true;
                    continue;
                }
                else if (depth != Vector2.Zero && entity1.Type == "coin" && entity2.Type == "player")
                {
                    entity1.remove = true;
                    level.coins++;
                    continue;
                }
                else if (depth != Vector2.Zero && entity2.Type == "coin" && entity1.Type == "player")
                {
                    continue; //if they both give a coin, you get both so one of them does nothing 
                }

                if (depth != Vector2.Zero && (entity1.Type == "fireball" || entity2.Type == "fireball"))
                {
                    if (entity1.Type == "player" || entity2.Type == "player")
                        continue;
                    if (entity1.Type == "fireball" && entity2.Type == "fireball")
                        continue;
                    entity2.Hurt_up = true;
                    entity1.Hurt_up = true;
                    continue;
                }

                if (absDepth.Y < absDepth.X)
                {
                    if (depth.Y > 0) // Entity2 on top of Entity1
                    {
                        if (entity1.Type == "player")
                        {
                            entity1.Hurt = true;
                            entity1.jump_speed = 400;
                        }
                        else if (entity1.Type != "player" && entity2.Type != "player")
                            entity2.Collision[0] = true;
                    }
                    else if (depth.Y < 0) // Entity1 on top of Entity2
                    {
                        if (entity1.Type == "player")
                        {
                            entity2.Hurt = true;
                            entity1.gravity_accel = 0;
                            entity1.jump_speed = 400;
                        }
                        else if (entity1.Type != "player" && entity2.Type != "player")
                            entity1.Collision[0] = true;
                    }
                }

                else if (absDepth.Y > absDepth.X)
                {
                    // entity1.Pos_X += depth.X;
                    if (entity1.Type == "player")
                    {
                        entity1.Hurt = true;
                    }
                    else if (entity2.Type == "player")
                    {
                        entity2.Hurt = true;
                    }
                    else
                    {
                        if (depth.X < 0) // Entity1 on the left of Entity2
                        {
                            entity1.Collision[1] = true;
                            entity2.Collision[2] = true;
                            entity1.walk_speed = -entity1.walk_speed;
                            entity1.Pos_X -= 5;
                            entity2.walk_speed = -entity2.walk_speed;
                            entity2.Pos_X += 5;
                            if (entity1.Type == "koopa_shell" && entity1.walk_speed != 0)
                            {
                                entity2.Hurt_up = true;
                                entity1.walk_speed = -entity1.walk_speed;
                            }
                        }

                        if (depth.X > 0) // Entity1 on the right of Entity2
                        {
                            entity1.Collision[2] = true;
                            entity2.Collision[1] = true;
                            entity1.walk_speed = -entity1.walk_speed;
                            entity1.Pos_X -= 5;
                            entity2.walk_speed = -entity2.walk_speed;
                            entity2.Pos_X += 5;
                            if (entity1.Type == "koopa_shell" && entity1.walk_speed != 0)
                            {
                                entity2.Hurt_up = true;
                                entity1.walk_speed = -entity1.walk_speed;
                            }
                        }
                    }
                }
            }
        }
    }
}