using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battleships
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D texture_sheet, hit_sheet, miss_sheet;

        public SpriteFont player_font;

        SoundEffect hit_sound;
        SoundEffect miss_sound;

        public Ship[] p1_ships, p2_ships;

        Ship ship_to_move;

        public Tile[,] p1_grid, p2_grid;

        MouseState mouse_state = Mouse.GetState();

        public int pos_x, pos_y, hit_p1, miss_p1, hit_p2, miss_p2;

        public static bool grab_p1, grab_p2, ship_placement, shoot, player_turn, play, running;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 650;
            graphics.PreferredBackBufferWidth = 1200;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture_sheet = Content.Load<Texture2D>(@"Images/battleship_sheet");
            hit_sheet = Content.Load<Texture2D>(@"Images/battleship_hit_ping_sheet");
            miss_sheet = Content.Load<Texture2D>(@"Images/battleship_miss_ping_sheet");
            player_font = Content.Load<SpriteFont>(@"Fonts/PlayersFont");
            hit_sound = Content.Load<SoundEffect>(@"Sounds/hit");
            miss_sound = Content.Load<SoundEffect>(@"Sounds/miss");

            p1_grid = new Tile[10, 10];
            for (int i = 0; i < p1_grid.GetLength(0); i++)
            {
                for (int j = 0; j < p1_grid.GetLength(1); j++)
                {
                    pos_x = i * 50 + 3;
                    pos_y = j * 50 + 3;

                    p1_grid[i, j] = new Tile(texture_sheet, pos_x, pos_y);
                }
            }

            p2_grid = new Tile[10, 10];
            for (int i = 0; i < p2_grid.GetLength(0); i++)
            {
                for (int j = 0; j < p2_grid.GetLength(1); j++)
                {
                    pos_x = i * 50 + 700 - 3;
                    pos_y = j * 50 + 2;

                    p2_grid[i, j] = new Tile(texture_sheet, pos_x, pos_y);
                }
            }


            p1_ships = new Ship[5];
            for (int i = 0; i < p1_ships.Length; i++)
            {
                pos_x = 25 + 3; //tilesize / 2 + gridoffset
                pos_y = i * 50 + 25 + 3;

                p1_ships[i] = new Ship(texture_sheet, pos_x, pos_y, new Rectangle(0, i * 50, 250 - i * 50, 50));

                if (i == 3 || i == 4)
                    p1_ships[i] = new Ship(texture_sheet, pos_x, pos_y, new Rectangle(0, i * 50, 300 - i * 50, 50));

            }

			p2_ships = new Ship[5];
			for (int j = 0; j < p2_ships.Length; j++)
			{
				pos_x = 700 + 25 - 3;
				pos_y = j * 50 + 25 + 2;

				p2_ships[j] = new Ship(texture_sheet, pos_x, pos_y, new Rectangle(0, j * 50, 250 - j * 50, 50));

				if (j == 3 || j == 4)
					p2_ships[j] = new Ship(texture_sheet, pos_x, pos_y, new Rectangle(0, j * 50, 300 - j * 50, 50));
			}
			

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            KeyMouseReader.Update();

            foreach (Ship s in p1_ships)
                s.Update(gameTime);

            foreach (Ship s in p2_ships)
                s.Update(gameTime);

            foreach (Tile g in p1_grid)
                g.Update(gameTime);

            foreach (Tile g in p2_grid)
                g.Update(gameTime);

            if (!running)
            {
                if (!ship_placement)
                {
                    if (KeyMouseReader.LeftClick() && KeyMouseReader.mouse_position.X < 500 && !grab_p1)
                    {
                        ship_to_move = ship_selected();

                        if (ship_to_move != null)
                            grab_p1 = true;

                    }

                    else if (grab_p1)
                    {
                        ship_to_move.ship_pos = new Vector2(KeyMouseReader.mouse_position.X, KeyMouseReader.mouse_position.Y);

                        if (KeyMouseReader.LeftClick())
                        {
                            if (ship_on_ship())
                            {
                                ship_to_move.ship_pos.X = (((int)ship_to_move.ship_pos.X / 50) * 50 + ship_to_move.ship_frame_size.Y / 2 + 3);
                                ship_to_move.ship_pos.Y = (((int)ship_to_move.ship_pos.Y / 50) * 50 + ship_to_move.ship_frame_size.Y / 2 + 3);

                                grab_p1 = false;
                            }

                            if (ship_to_move.ship_bounding_box.Right >= 500 || ship_to_move.ship_bounding_box.Top <= 3 || ship_to_move.ship_bounding_box.Left <= 3 || ship_to_move.ship_bounding_box.Bottom >= 500)
                                grab_p1 = true;
                        }

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && grab_p1 == false)
                        ship_placement = true;

                }



                if (ship_placement && !play)
                {
                    if (KeyMouseReader.LeftClick() && KeyMouseReader.mouse_position.X > 700 && !grab_p2)
                    {
                        ship_to_move = ship_selected();

                        if (ship_to_move != null)
                            grab_p2 = true;
                    }

                    else if (grab_p2)
                    {
                        ship_to_move.ship_pos = new Vector2(KeyMouseReader.mouse_position.X, KeyMouseReader.mouse_position.Y);

                        if (KeyMouseReader.LeftClick())
                        {
                            if (ship_on_ship())
                            {
                                ship_to_move.ship_pos.X = (((int)ship_to_move.ship_pos.X / 50) * 50 + ship_to_move.ship_frame_size.Y / 2) - 3;
                                ship_to_move.ship_pos.Y = (((int)ship_to_move.ship_pos.Y / 50) * 50 + ship_to_move.ship_frame_size.Y / 2) + 2;

                                grab_p2 = false;
                            }

                            if (ship_to_move.ship_bounding_box.Right >= 1197 || ship_to_move.ship_bounding_box.Top <= 3 || ship_to_move.ship_bounding_box.Left <= 697 || ship_to_move.ship_bounding_box.Bottom >= 500)
                                grab_p2 = true;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && grab_p2 == false)
                        play = true;
                }

                if (play == true)
                {
                    if (KeyMouseReader.LeftClick())
                    {
                        Tile tile = tile_selected();

                        if (tile != null && KeyMouseReader.mouse_position.X > 700 && KeyMouseReader.mouse_position.Y < 500 && !player_turn)
                        {
                            if (tile.hit == true || tile.miss == true)
                                shoot = false;
                            else
                                shoot = true;

                            if (shoot == true)
                            {
                                Ship temp = get_ship(tile.tile_bb);
                                if (temp != null && tile.hit == false && tile.miss == false)
                                {
                                    tile.hit = true;
                                    hit_p1 += 1;
                                    temp.ship_life -= 1;
                                    hit_sound.Play();
                                }

                                else
                                {
                                    tile.miss = true;
                                    miss_p1 += 1;
                                    miss_sound.Play();
                                }

                                player_turn = true;
                            }
                        }

                        if (tile != null && KeyMouseReader.mouse_position.X < 500 && KeyMouseReader.mouse_position.Y < 500 && player_turn)
                        {
                            if (tile.hit == true || tile.miss == true)
                                shoot = false;
                            else
                                shoot = true;

                            if (shoot == true)
                            {
                                Ship temp = get_ship(tile.tile_bb);
                                if (temp != null && tile.hit == false && tile.miss == false)
                                {
                                    tile.hit = true;
                                    hit_p2 += 1;
                                    temp.ship_life -= 1;
                                    hit_sound.Play();
                                }

                                else
                                {
                                    tile.miss = true;
                                    miss_p2 += 1;
                                    miss_sound.Play();
                                }

                                player_turn = !player_turn;
                            }
                        }

                    }

                }

                if (hit_p1 == 17 || hit_p2 == 17)
                    running = true;

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            
            foreach (Tile g1 in p1_grid)
                g1.Draw(spriteBatch);
            
            foreach (Tile g2 in p2_grid)
                g2.Draw(spriteBatch);

            if (!ship_placement)
            {
                foreach (Ship s in p1_ships)
                    s.Draw(spriteBatch);
            }

            if (ship_placement && !play)
            {
                foreach (Ship s in p2_ships)
                    s.Draw(spriteBatch);
            }

            if(play)
            {
                foreach (Ship s in p1_ships)
                {
                    if (s.ship_life == 0)
                        s.Draw(spriteBatch);
                }

                foreach (Ship s in p2_ships)
                {
                    if (s.ship_life == 0)
                        s.Draw(spriteBatch);
                }
            }

            if(running == true)
            {
                foreach (Ship s in p1_ships)
                    s.Draw(spriteBatch);

                foreach (Ship s in p2_ships)
                    s.Draw(spriteBatch);
            }

            spriteBatch.DrawString(player_font, " [ Player 1 ]\n[ Hits " + hit_p1 + " / 17 ]\n [ Misses " + miss_p1 + " ]", new Vector2(125, 520), Color.LimeGreen);
            spriteBatch.DrawString(player_font, " [ Player 2 ]\n[ Hits " + hit_p2 + " / 17 ]\n [ Misses " + miss_p2 + " ]", new Vector2(825, 520), Color.LimeGreen);

            if (hit_p1 == 17)
                spriteBatch.DrawString(player_font, " [ PLAYER 1 - WON]\n[ PLAYER 2 - LOST ]", new Vector2(440, 520), Color.LimeGreen);

            if (hit_p2 == 17)
                spriteBatch.DrawString(player_font, " [ PLAYER 2 - WON]\n[ PLAYER 1 - LOST ]", new Vector2(440, 520), Color.LimeGreen);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public Ship ship_selected()
        {
            foreach (Ship s in p1_ships)
            {
                if (s.ship_select)
                    return s;
            }

            foreach (Ship s in p2_ships)
            {
                if (s.ship_select)
                    return s;
            }

            return null;
        }


        public Tile tile_selected()
        {
            foreach (Tile t in p1_grid)
            {
                if (t.selected)
                    return t;
            }

            foreach (Tile t in p2_grid)
            {
                if (t.selected)
                    return t;
            }

            return null;
        }

        public Ship get_ship(Rectangle rec)
        {
            foreach(Ship s in p1_ships)
            {
                if (rec.Intersects(s.ship_bounding_box))
                    return s;
            }

            foreach(Ship s in p2_ships)
            {
                if (rec.Intersects(s.ship_bounding_box))
                    return s;
            }

            return null;
        }

        public bool ship_on_ship()
        {
            foreach (Ship s in p1_ships)
            {
                if(ship_to_move.ship_bounding_box.Intersects(s.ship_bounding_box) && ship_to_move != s)
                return false;
            }

            foreach (Ship s in p2_ships)
            {
                if (ship_to_move.ship_bounding_box.Intersects(s.ship_bounding_box) && ship_to_move != s)
                    return false;
            }

            return true;
        }

    }
}
