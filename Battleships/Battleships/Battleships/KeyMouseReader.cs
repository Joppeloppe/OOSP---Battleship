using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

static class KeyMouseReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
    public static Point mouse_position;
	
    public static bool KeyPressed(Keys key)
    {
		return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
	}

	public static bool LeftClick()
    {
		return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
	}

	public static bool RightClick()
    {
		return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
	}

    public static bool LeftPress()
    {
        return mouseState.LeftButton == ButtonState.Pressed;
    }

    public static bool RightPress()
    {
        return mouseState.RightButton == ButtonState.Pressed;
    }
    public static bool MiddleClick()
    {
        return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
    }
	public static void Update()
    {
        mouse_position = new Point(mouseState.X, mouseState.Y);

		oldKeyState = keyState;
		keyState = Keyboard.GetState();

		oldMouseState = mouseState;
		mouseState = Mouse.GetState();
	}
}