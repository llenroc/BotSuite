﻿/* **************************************************************
 * Name:      BotSuite.NET
 * Version:   v2.1.0beta3
 * Purpose:   Framework for creating bots
 * Author:    wieschoo
 * Created:   16.09.2012
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2012 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BotSuite
{
    /// <summary>
    /// class for simulate mouse actions like moving or clicking
    /// </summary>
	public class Mouse
	{
		/// <summary>
		/// causes a left-click (press and release)
		/// </summary>
        /// <example>
        /// <code>
        /// Mouse.LeftClick();
        /// </code>
        /// </example>
		/// <returns></returns>
		public static void LeftClick()
		{
			LeftDown();
			Utility.Delay(10, 30);
			LeftUp();
		}

		/// <summary>
		/// press down the left mouse button
		/// </summary>
        /// <example>
        /// <code>
        /// Mouse.LeftDown();
        /// </code>
        /// </example>
		/// <returns></returns>
		public static void LeftDown()
		{
			NativeMethods.mouse_event((Int32)(NativeMethods.MouseEventFlags.LEFTDOWN), 0, 0, 0, new IntPtr(0));
		}

        /// <summary>
        /// release the left mouse button
        /// </summary>
        /// <example>
        /// <code>
        /// Mouse.LeftUp();
        /// </code>
        /// </example>
        /// <returns></returns>
		public static void LeftUp()
		{
			NativeMethods.mouse_event((Int32)(NativeMethods.MouseEventFlags.LEFTUP), 0, 0, 0, new IntPtr(0));
		}
        /// <summary>
        /// causes a right-click (press and release)
        /// </summary>
        /// <example>
        /// <code>
        /// Mouse.RightClick();
        /// </code>
        /// </example>
        /// <returns></returns>
		public static void RightClick()
		{
			RightDown();
			Utility.Delay(10, 30);
			RightUp();
		}
        /// <summary>
        /// press down the right mouse button
        /// </summary>
        /// <example>
        /// <code>
        /// Mouse.RightDown();
        /// </code>
        /// </example>
        /// <returns></returns>
		public static void RightDown()
		{
			NativeMethods.mouse_event((Int32)(NativeMethods.MouseEventFlags.RIGHTDOWN), 0, 0, 0, new IntPtr(0));
		}
        /// <summary>
        /// release the right mouse button
        /// </summary>
        /// <example>
        /// <code>
        /// Mouse.RightUp();
        /// </code>
        /// </example>
        /// <returns></returns>
		public static void RightUp()
		{
			NativeMethods.mouse_event((Int32)(NativeMethods.MouseEventFlags.RIGHTUP), 0, 0, 0, new IntPtr(0));
		}

		/// <summary>
		/// causes a mouse movement
		/// </summary>
        /// <example>
        /// <code>
        /// Point target = new Point(10,10);
        /// Mouse.Move(target,true,10);
        /// </code>
        /// </example>
        /// <param name="TargetPosition">target coordinate</param>
		/// <param name="human">prevent mouse jumps</param>
		/// <param name="steps">points of pathpolygons</param>
		/// <returns></returns>
        public static Boolean Move(Point TargetPosition, Boolean human = true, Int32 steps = 100)
		{
            if (!human)
            {
                Cursor.Position = TargetPosition;
                return true;
            }
            else
            {
                Point start = GetPosition();
                PointF iterPoint = start;

                PointF slope = new PointF(TargetPosition.X - start.X, TargetPosition.Y - start.Y);

                slope.X = slope.X / steps;
                slope.Y = slope.Y / steps;

                for (int i = 0; i < steps; i++)
                {
                    iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                    Cursor.Position = Point.Round(iterPoint);
                    Utility.Delay(5, 10);
                }

                // test if it works
                if ((Cursor.Position.X == TargetPosition.X) && (Cursor.Position.Y == TargetPosition.Y))
                    return true;
                else
                    return false;
            }
            
		}
        /// <summary>
        /// causes a mouse movement
        /// </summary>
        /// <example>
        /// <code>
        /// Mouse.Move(this.Left+10,this.Top+50,true,10);
        /// </code>
        /// </example>
		/// <param name="targetX">x coordinate of target</param>
        /// <param name="targetY">y coordinate of target</param>
        /// <param name="human">prevent mouse jumps</param>
        /// <param name="steps">points of pathpolygons</param>
		/// <returns></returns>
		public static Boolean Move(Int32 targetX, Int32 targetY, Boolean human = true, Int32 steps = 100)
		{
            return Move(new Point(targetX, targetY), human, steps);
		}

        public static Boolean Move(Rectangle R, Boolean human = true, Int32 steps = 100)
        {
            return Move(new Point(Convert.ToInt32(R.Left+(R.Width/2)), Convert.ToInt32(R.Top+(R.Height/2))), human, steps);
        }
        /// <summary>
        /// causes a drag and drop
        /// </summary>
        /// <param name="source">drag point</param>
        /// <param name="target">drop point</param>
        /// <param name="human">prevent mouse jumps</param>
        /// <param name="steps">points of pathpolygons</param>
        /// <returns></returns>
        public static void DragAndDrop(Point source, Point target, Boolean human = true, Int32 steps = 100)
        {
            Move(source, human, steps);
            LeftClick();
            if (human)
                Jiggle();
            Utility.Delay(10, 30);
            Move(target, human, steps);
            LeftClick();
        }

		/// <summary>
		/// get the current position of the mouse pointer
		/// </summary>
        /// <example>
        /// <code>
        /// Point CurPos = Point.Empty;
        /// CurPos = Mouse.GetPosition();
        /// </code>
        /// </example>
		/// <returns>Point position</returns>
		public static Point GetPosition()
		{
			return Cursor.Position;
		}

		/// <summary>
		/// causes a slightly mouse jiggle +-10 px
		/// </summary>
        /// <example>
        /// <code>
        /// Mouse.Jiggle();
        /// </code>
        /// </example>
		/// <returns></returns>
		public static void Jiggle()
		{
			Int32 xChange = Utility.Random(-10, 10);
			Int32 yChange = Utility.Random(-10, 10);

			Move(Cursor.Position.X + xChange, Cursor.Position.Y + yChange, true, 5);
			Utility.Delay(20, 60);
			Move(Cursor.Position.X - xChange, Cursor.Position.Y - yChange, true, 5);
		}

		/// <summary>
		/// move the mouse relativ to the window
		/// </summary>
        /// <example>
        /// <code>
        /// IntPtr hwnd = ... ;
        /// bool res = MoveRelativeToWindow(hwnd, 20, 35, true, 10);
        /// </code>
        /// </example>
		/// <param name="windowHandle">handle of window</param>
		/// <param name="targetX">x coordinate</param>
		/// <param name="targetY">y coordinate</param>
        /// <param name="human">prevent mouse jumps</param>
        /// <param name="steps">points of pathpolygons</param>
		/// <returns>true/false</returns>
		public static Boolean MoveRelativeToWindow(IntPtr windowHandle, Int32 targetX, Int32 targetY, Boolean human = true, Int32 steps = 100)
		{
			NativeMethods.RECT WINDOW = new NativeMethods.RECT();

			if(!NativeMethods.GetWindowRect(windowHandle, out WINDOW))
				return false;

			if(!Move(WINDOW.Left + targetX, WINDOW.Top + targetY, human, steps))
				return false;

			return true;
		}

		/// <summary>
		/// get the current position of the mouse pointer relative to a window
		/// </summary>
        /// <example>
        /// <code>
        /// Point CurPos = Point.Empty;
        /// IntPtr hwnd = ... ;
        /// CurPos = Mouse.GetPositionRelativeToWindow(hwnd);
        /// </code>
        /// </example>
		/// <param name="window">handle of window</param>
		/// <returns>Point position</returns>
		public static Point GetPositionRelativeToWindow(IntPtr window)
		{
			Point position = Cursor.Position;
			NativeMethods.RECT WINDOW = new NativeMethods.RECT();
			NativeMethods.GetWindowRect(window, out WINDOW);
			return new Point(position.X - WINDOW.Left, position.Y - WINDOW.Top);
		}

        /// <summary>
        /// returns whether the cursor is inside a rectangle or outside
        /// </summary>
        /// <param name="R">rectangle</param>
        /// <returns>inside=true/outside=false</returns>
        public static bool InRectangle(int t, int l, int b, int r)
        {
            Point P = GetPosition();
            return (
                (l<P.X) && (P.X<r)  && (t<P.Y  ) && (P.Y<b)
                );
        }
	}
}