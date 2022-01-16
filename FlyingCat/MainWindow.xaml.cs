using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlyingCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Gamefield.Focus();
            animationTimer.Interval = TimeSpan.FromMilliseconds(20);
            animationTimer.Tick += GameAnimation;
            intervallTimer.Interval = TimeSpan.FromSeconds(1);
            animationTimer.Tick += SetWallsAndClouds;

        }

        Levels level = new Levels();

        DispatcherTimer animationTimer = new DispatcherTimer();
        DispatcherTimer intervallTimer = new DispatcherTimer();


        private void SetWallsAndClouds(object sender, EventArgs e)
        {
            level.SetWalls(Gamefield);
            level.SetClouds(Gamefield);

        }


        private void GameAnimation(object sender, EventArgs e)
        {
            Score.Content = $"Score = {level.Score}";

            level.AnimateWalls(Gamefield, FlyingCat);
            level.AnimateClouds(Gamefield);
            level.AnimateCat(Gamefield, FlyingCat);

            level.LevelUp();

            if (level.GameOver)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            animationTimer.Stop();
            intervallTimer.Stop();
            NewGame.Content = "Game over!\nPress 'R' for new game";
            Gamefield.Children.Add(NewGame);
            Canvas.SetLeft(NewGame, (Gamefield.ActualWidth / 2) - (NewGame.ActualWidth /2));
            Canvas.SetTop(NewGame, (Gamefield.ActualHeight / 2) - (NewGame.ActualHeight /2));

            level.ResetGamefield(Gamefield,FlyingCat);
        }


        private void Gamefield_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.R)
            {
                animationTimer.Start();
                intervallTimer.Start();
                if (Gamefield.Children.Contains(NewGame))
                {
                    Gamefield.Children.Remove(NewGame);
                    level.Score = 0;
                }
            }
            if (e.Key==Key.Space)
            {
                level.Fly = true;
                //FlyingCat.RenderTransform = new RotateTransform(-20, FlyingCat.Width / 2, FlyingCat.Height / 2);
            }


        }

        private void Gamefield_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                level.Fly = false;
                //FlyingCat.RenderTransform = new RotateTransform(+20, FlyingCat.Width / 2, FlyingCat.Height / 2);
            }
        }

        private void Gamefield_TouchDown(object sender, TouchEventArgs e)
        {
            level.Fly = true;
            FlyingCat.RenderTransform = new RotateTransform(-20, FlyingCat.Width / 2, FlyingCat.Height / 2);
            
        }

        private void Gamefield_TouchUp(object sender, TouchEventArgs e)
        {
            level.Fly = false;
            FlyingCat.RenderTransform = new RotateTransform(+20, FlyingCat.Width / 2, FlyingCat.Height / 2);

        }
    }
}
