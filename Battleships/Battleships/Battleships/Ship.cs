using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleships
{
    public class Ship
    {
        public Vector2 ship_pos, ship_origin;

        public Texture2D ship_tex;

        public Rectangle ship_rec, ship_bounding_box;

        public Point ship_frame_size;

        public static int offset;
        public int height, width, ship_life;

        public float rotation_angle;

        public bool ship_select = true;
        public bool rotated;

        public Ship(Texture2D ship_tex, int ship_posX, int ship_posY, Rectangle ship_rec)
        {
            this.ship_tex = ship_tex;
            this.ship_pos = new Vector2(ship_posX, ship_posY);
            this.ship_rec = ship_rec;
            this.height = ship_rec.Height;
            this.width = ship_rec.Width;
            offset = 25; //Tile width / 2

            ship_origin = new Vector2(offset, offset);

            ship_life = ship_rec.Width / 50;

            ship_frame_size = new Point(ship_rec.Width, ship_rec.Height);
        }

        public void Update(GameTime gameTime)
        {
            ship_bounding_box = new Rectangle((int)ship_pos.X - offset, (int)ship_pos.Y - offset, width, height);

            if (ship_bounding_box.Contains(KeyMouseReader.mouse_position))
                ship_select = true;
            else
                ship_select = false;

            if (Game1.grab_p1 == true && KeyMouseReader.mouse_position.X < 500 || Game1.grab_p2 == true && KeyMouseReader.mouse_position.X > 700)
            {
                if (KeyMouseReader.RightClick())
                {
                    if (ship_select == true)
                    {
                        rotation_angle = (rotation_angle == 0 ? MathHelper.ToRadians(90) : 0);

                        int temp = height;
                        height = width;
                        width = temp;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship_tex, ship_pos, ship_rec, Color.White, rotation_angle, ship_origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
