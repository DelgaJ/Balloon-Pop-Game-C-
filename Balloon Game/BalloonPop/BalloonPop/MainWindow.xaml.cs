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

namespace BalloonPop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Create a new timer called game timer
        DispatcherTimer gameTimer = new DispatcherTimer();
        //Set initial speed to 3
        int speed = 3;
        //Set initial intervals to 90
        int intervals = 90;
        //Create a new random number generator class
        Random rand = new Random();
        //A list to remove items from the canvas
        List<Rectangle> itemRemover = new List<Rectangle>();
        //Background image texture clas
        ImageBrush backgroundImage = new ImageBrush();
        //Integer to keep track of different balloon images
        int balloonSkins;
        //This integer will be used to move the balloons left or right slightly
        int i;
        //Missed balloon count integer
        int missedBalloons;
        //Boolean to check if the game is active or not
        bool gameIsActive;
        //Score counter
        int score;
        //Create a new media player to link the pop sound to
        MediaPlayer player = new MediaPlayer();


        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundImage.ImageSource = new BitmapImage(new Uri("C:/Users/jdelg/Documents/Portfolio/Balloon Game/BalloonPop/BalloonPop/Files/background-Image.jpg"));
            MyCanvas.Background = backgroundImage;

            RestartGame();
        }


        private void canvasKeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && gameIsActive == false)
            {
                // run the reset function
                RestartGame();
            }
        }

        private void PopBalloons(object sender, MouseButtonEventArgs e)
        {
            if (gameIsActive)
            {
                if(e.OriginalSource is Rectangle)
                {
                    //if the click source is a rectangle then we will create a new rectangle
                    // and link it to the rectangle that sent the click event
                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    //load the mp3 file to the player URI
                    player.Open(new Uri("C:/Users/jdelg/Documents/Portfolio/Balloon Game/BalloonPop/BalloonPop/Files/pop_sound.mp3", UriKind.RelativeOrAbsolute));
                    player.Play();
                    MyCanvas.Children.Remove(activeRec);

                    score++;
                }
            }
        }

        private void StartGame()
        {
            gameTimer.Start();
            missedBalloons = 0;
            score = 0;
            intervals = 90;
            gameIsActive = true;
            speed = 3;
        }

        private void RestartGame()
        {
            //This function will reset the game and remove any unused item from the canvas
            //run loop to find any rectangles in this canvas
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // if found add it to the item remover list
                itemRemover.Add(x);
            }

            //check if there is any rectangles in the item remover list
            foreach (Rectangle y in itemRemover)
            {
                //if found remove it from the canvas
                MyCanvas.Children.Remove(y);
            }

            //clear everything from the list
            itemRemover.Clear();
            //start the game function
            StartGame();
        }

        private void GameEngine(object? sender, EventArgs e)
        {
            scoreLabel.Content = "Score: " + score;

            intervals -= 10;

            if (intervals < 1)
            {
             
                ImageBrush balloonImage = new ImageBrush();

                balloonSkins += 1;

                if (balloonSkins > 5)
                {
                    balloonSkins = 1;
                }

                switch (balloonSkins)
                {
                    case 1:
                        balloonImage.ImageSource = new BitmapImage(new Uri("C:\\Users\\jdelg\\Documents\\Portfolio\\Balloon Game\\BalloonPop\\BalloonPop\\Files\\balloon1.png"));
                        break;
                    case 2:
                        balloonImage.ImageSource = new BitmapImage(new Uri("C:\\Users\\jdelg\\Documents\\Portfolio\\Balloon Game\\BalloonPop\\BalloonPop\\Files\\balloon2.png"));
                        break;
                    case 3:
                        balloonImage.ImageSource = new BitmapImage(new Uri("C:\\Users\\jdelg\\Documents\\Portfolio\\Balloon Game\\BalloonPop\\BalloonPop\\Files\\balloon3.png"));
                        break;
                    case 4:
                        balloonImage.ImageSource = new BitmapImage(new Uri("C:\\Users\\jdelg\\Documents\\Portfolio\\Balloon Game\\BalloonPop\\BalloonPop\\Files\\balloon4.png"));
                        break;
                    case 5:
                        balloonImage.ImageSource = new BitmapImage(new Uri("C:\\Users\\jdelg\\Documents\\Portfolio\\Balloon Game\\BalloonPop\\BalloonPop\\Files\\balloon5.png"));
                        break;

                }

                Rectangle newBalloon = new Rectangle
                {
                    Tag = "balloon",
                    Height = 70,
                    Width = 50,
                    Fill = balloonImage
                };

                Canvas.SetLeft(newBalloon, rand.Next(50, 400));
                Canvas.SetTop(newBalloon, 600);

                MyCanvas.Children.Add(newBalloon);

                intervals = rand.Next(90, 150);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                //if the item is a rectangle and has a tag "Balloon"
                if (x is Rectangle && (string)x.Tag == "balloon")
                {
                    i = rand.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));
                }

                //if the balloon reach top of the screen
                if (Canvas.GetTop(x) < 20)
                {
                    //remove them
                    itemRemover.Add(x);
                    //add 1 missed integer
                    missedBalloons++;
                }
            }

            if (missedBalloons > 10)
            {
                gameIsActive = false;
                gameTimer.Stop();

                MessageBox.Show("You missed 10 Balloons, press space to restart");
            }

            //if score more than 20, change the speed to 6
            if(score > 20)
            {
                //change the speed to 6
                speed = 6;
            }
            
            // garbage collection
            // remove any item thats been added to the item remover list
            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
        }


    }
}
