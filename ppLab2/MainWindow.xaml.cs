﻿using System;
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
using System.Threading;
using System.Windows.Threading;

namespace ppLab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PhysicalCircle> flyingObjects; //todo rename
        DispatcherTimer UIDispatcher;
        List<PhysicalCircleWorker> workers;
        Random random;

        int maxSpeed = 6;
        int minSpeed = 1;

        public MainWindow()
        {
            InitializeComponent();

            UIDispatcher = new DispatcherTimer();
            workers = new List<PhysicalCircleWorker>();
            random = new Random();
            runUpdateUI();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*
                1) init circle
                2) init worker
                3) add worker to list
                4) add circle to canvas
                5) run worker
            */

            PhysicalCircle circle = new PhysicalCircle(new Ellipse
            {
                Height = 30,
                Width = 30,
                Fill = new SolidColorBrush(Color.FromArgb(255,
                    (byte)random.Next(0, 255),
                    (byte)random.Next(0, 255),
                    (byte)random.Next(0, 255))),
            }, new Position(0, 0));

            PhysicalCircleWorker worker = new PhysicalCircleWorker(circle, canvas, workers)
            {
                XSpeed = random.Next(minSpeed, maxSpeed),
                YSpeed = random.Next(minSpeed, maxSpeed),
            };

            workers.Add(worker);
            canvas.Children.Add(circle.Circle);

            worker.Start();
        }

        private void runUpdateUI()
        {
            UIDispatcher.Tick += new EventHandler(updateUI);
            UIDispatcher.Interval = new TimeSpan(0, 0, 0, 0, 10);
            UIDispatcher.Start();
        }

        private void updateUI(object sender, EventArgs e)
        {
            foreach (PhysicalCircleWorker worker in workers)
                worker.Circle.Circle.Margin = new Thickness(worker.CurrentPosition.x, worker.CurrentPosition.y, 0, 0);
        }
    }
}
