using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleships
{
    public class Tile
    {
        public Vector2 tile_pos, miss_pos, hit_pos;

        public Texture2D tile_tex;

        public Rectangle tile_rec = new Rectangle(200, 200, 50, 50);
        public Rectangle tile_bb;

        Point hit_frame_size = new Point(60, 60);
        Point hit_frame_current = new Point(0, 0);
        Point hit_sheet_size = new Point(3, 1);

        Point miss_frame_size = new Point(40, 40);
        Point miss_frame_current = new Point(0, 0);
        Point miss_sheet_size = new Point(3, 1);

        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 250;

        public bool selected, taken, miss, hit;

        public Tile(Texture2D tile_tex, int tile_posX, int tile_posY)
        {
            this.tile_pos = new Vector2(tile_posX, tile_posY);
            this.tile_tex = tile_tex;
        }

        public void Update(GameTime gameTime)
        {
            tile_bb = new Rectangle((int)tile_pos.X, (int)tile_pos.Y, 50, 50);

            if (tile_bb.Contains(KeyMouseReader.mouse_position))
                selected = true;
            else
                selected = false;

            miss_pos = new Vector2(tile_pos.X + 5, tile_pos.Y + 5);
            hit_pos = new Vector2(tile_pos.X - 5, tile_pos.Y - 5);

            timeSinceLastFrame += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                ++miss_frame_current.X;
                if (miss_frame_current.X >= miss_sheet_size.X)
                {
                    miss_frame_current.X = 0;

                    ++miss_frame_current.Y;

                    if (miss_frame_current.Y >= miss_sheet_size.Y)
                        miss_frame_current.Y = 0;
                }

                ++hit_frame_current.X;
                if (hit_frame_current.X >= hit_sheet_size.X)
                {
                    hit_frame_current.X = 0;

                    ++hit_frame_current.Y;

                    if (hit_frame_current.Y >= hit_sheet_size.Y)
                        hit_frame_current.Y = 0;
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (selected)
                spriteBatch.Draw(tile_tex, tile_pos, tile_rec, Color.Black);
            else
                spriteBatch.Draw(tile_tex, tile_pos, tile_rec, Color.White);

            if (hit)
                spriteBatch.Draw(Game1.hit_sheet, hit_pos, new Rectangle(hit_frame_current.X * hit_frame_size.X, hit_frame_current.Y * hit_frame_size.Y, hit_frame_size.X, hit_frame_size.Y), Color.White);

            if(miss)
                spriteBatch.Draw(Game1.miss_sheet, miss_pos, new Rectangle(miss_frame_current.X * miss_frame_size.X, miss_frame_current.Y* miss_frame_size.Y, miss_frame_size.X, miss_frame_size.Y), Color.White);
        }

    }
}
