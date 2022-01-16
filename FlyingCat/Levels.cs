using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlyingCat
{
    class Levels
    {
        public double SpeedWalls { get; set; }
        public double SpeedClouds { get; set; }
        public double Gravity { get; set; }
        public int Score { get; set; }
        public bool Fly { get; set; }
        public bool GameOver { get; set; }


        public Levels(double speedWalls=10, double speedClouds=4, double gravity=10, int score=0, bool gameOver=false)
        {
            SpeedWalls = speedWalls;
            SpeedClouds = speedClouds;
            Gravity = gravity;
            Score = score;
            GameOver = gameOver;
        }

        public void LevelUp()
        {
            switch (Score)
            {
                case 50:
                    SpeedWalls = 12;
                    SpeedClouds = 5;
                    break;
                case 100:
                    SpeedWalls = 14;
                    SpeedClouds = 6;
                    break;
                case 150:
                    SpeedWalls = 16;
                    SpeedClouds = 7;
                    break;
                case 200:
                    SpeedWalls = 18;
                    SpeedClouds = 8;
                    Gravity = 12;
                    break;

                default:
                    break;
            }
        }

        internal void ResetGamefield(Canvas gamefield, Image flyingCat)
        {
            Score = 0;
            SpeedWalls = 10;
            SpeedClouds = 3;
            Gravity = 10;
            GameOver = false;

            Random rnd = new Random();

            foreach (Rectangle item in gamefield.Children.OfType<Rectangle>())
            {
                if (item.Name == "WallTop")
                {
                    item.Height = rnd.Next(20, (int)gamefield.ActualHeight / 2);
                    Canvas.SetLeft(item, rnd.Next((int)gamefield.ActualWidth, (int)gamefield.ActualWidth * 2));
                }
                if (item.Name == "WallBottom")
                {
                    item.Height = rnd.Next(20, (int)gamefield.ActualHeight / 2);
                    Canvas.SetLeft(item, rnd.Next((int)gamefield.ActualWidth, (int)gamefield.ActualWidth * 2));
                    Canvas.SetTop(item, gamefield.ActualHeight - item.Height);
                }
            }
            
            Canvas.SetTop(flyingCat, (gamefield.ActualHeight / 2) - (flyingCat.Height / 2));
        }

        internal void AnimateCat(Canvas gamefield, Image flyingCat)
        {
            if (Fly)
            {
                Canvas.SetTop(flyingCat, Canvas.GetTop(flyingCat) - Gravity);
                flyingCat.RenderTransform = new RotateTransform(-15, flyingCat.Width, flyingCat.Height / 2);

            }
            else
            {
                Canvas.SetTop(flyingCat, Canvas.GetTop(flyingCat) + Gravity);
                flyingCat.RenderTransform = new RotateTransform(+15, flyingCat.Width, flyingCat.Height / 2);

            }
            if (Canvas.GetTop(flyingCat) < 0 || Canvas.GetTop(flyingCat) > gamefield.ActualHeight - flyingCat.ActualHeight)
            {
                GameOver = true;
            }
        }

        internal void AnimateClouds(Canvas gamefield)
        {
            foreach (Image item in gamefield.Children.OfType<Image>())
            {
                if (item.Name == "Cloud1" || item.Name == "Cloud2" || item.Name == "Cloud3")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - SpeedClouds);
                }
            }
        }

        internal void AnimateWalls(Canvas gamefield, Image flyingCat)
        {
            Rect rect1 = new Rect(Canvas.GetLeft(flyingCat), Canvas.GetTop(flyingCat), flyingCat.Width, flyingCat.Height);

            foreach (Rectangle item in gamefield.Children.OfType<Rectangle>())
            {
                Canvas.SetLeft(item, Canvas.GetLeft(item) - SpeedWalls);

                Rect rect2 = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height);

                if (rect1.IntersectsWith(rect2))
                {
                    GameOver = true;
                }

            }
        }

        internal void SetClouds(Canvas gamefield)
        {
            Random rnd = new Random();

            foreach (var item in gamefield.Children.OfType<Image>())
            {
                if ((Canvas.GetLeft(item) < -item.ActualWidth && item.Name == "Cloud1") ||
                    (Canvas.GetLeft(item) < -item.ActualWidth && item.Name == "Cloud2") ||
                    (Canvas.GetLeft(item) < -item.ActualWidth && item.Name == "Cloud3"))
                {
                    item.Height = rnd.Next(20, (int)gamefield.ActualHeight / 2);
                    Canvas.SetLeft(item, rnd.Next((int)gamefield.ActualWidth, (int)gamefield.ActualWidth * 2));
                    Canvas.SetTop(item, rnd.NextDouble() * gamefield.ActualHeight);
                }
            }
        }

        internal void SetWalls(Canvas gamefield)
        {
            Random rnd = new Random();

            foreach (var item in gamefield.Children.OfType<Rectangle>())
            {

                if (Canvas.GetLeft(item) < 0 && item.Name == "WallTop")
                {
                    item.Height = rnd.Next(20, (int)gamefield.ActualHeight / 2);
                    Canvas.SetLeft(item, rnd.Next((int)gamefield.ActualWidth, (int)gamefield.ActualWidth * 2));
                    Score += 5;
                }
                if (Canvas.GetLeft(item) < 0 && item.Name == "WallBottom")
                {
                    item.Height = rnd.Next(20, (int)gamefield.ActualHeight / 2);
                    Canvas.SetLeft(item, rnd.Next((int)gamefield.ActualWidth, (int)gamefield.ActualWidth * 2));
                    Canvas.SetTop(item, gamefield.ActualHeight - item.Height);
                    Score += 5;
                }
            }
        }
    }
}
