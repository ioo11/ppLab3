using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ppLab2
{
    public class PhysicalCircle
    {
        public int Radius { get; private set; }
        public Position Center;
        public Ellipse Circle;

        public PhysicalCircle(Ellipse ellipse, Position center)
        {
            Circle = ellipse;
            Center = center;
            Circle.Margin = new Thickness(center.x, center.y, 0, 0);
            Radius = (int)Circle.Width / 2;
        }
    }

    public class PhysicalCircleWorker
    {
        public int XSpeed { get; set; }
        public int YSpeed { get; set; }
        public PhysicalCircle Circle;
        public Position CurrentPosition;
        DispatcherTimer dispatcherTimer;
        Canvas _canvas;
        public List<PhysicalCircleWorker> anotherWorkers;

        public PhysicalCircleWorker(PhysicalCircle circle, Canvas canvas, List<PhysicalCircleWorker> anotherWorkers)
        {
            Circle = circle;
            CurrentPosition = Circle.Center;
            XSpeed = 3; // todo hardcode
            YSpeed = 3;
            _canvas = canvas;
            dispatcherTimer = new DispatcherTimer();
            this.anotherWorkers = anotherWorkers;
        }

        public void Start()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Tick += new EventHandler(changePosition);
            dispatcherTimer.Start();
        }

        public void oneStep()
        {
            CurrentPosition.x += XSpeed;
            CurrentPosition.y += YSpeed;
        }

        public void changePosition(object sender, EventArgs e)
        {
            if (CurrentPosition.x >= _canvas.Width)
            {
                XSpeed *= -1;
                while (CurrentPosition.x >= _canvas.Width)
                    oneStep();
            }
            if (CurrentPosition.x < _canvas.MinWidth)
            {
                XSpeed *= -1;
                while (CurrentPosition.x < _canvas.MinWidth)
                    oneStep();
            }
            if (CurrentPosition.y >= _canvas.Height)
            {
                YSpeed *= -1;
                while (CurrentPosition.y >= _canvas.Height)
                    oneStep();
            }
            if (CurrentPosition.y < _canvas.MinHeight)
            {
                YSpeed *= -1;
                while (CurrentPosition.y < _canvas.MinHeight)
                    oneStep();
            }
            
            foreach (PhysicalCircleWorker worker in anotherWorkers)
            {
                if (worker != this)
                    if (PhysicalCircleWorker.isCollised(this, worker))
                        collisionHandle(worker, this);
            }

            oneStep();
        }

        public static bool isCollised(PhysicalCircleWorker worker1, PhysicalCircleWorker worker2)
        {
            int minDistance = worker1.Circle.Radius + worker2.Circle.Radius;
            int realDistance = Position.distanceBetween(worker1.CurrentPosition, worker2.CurrentPosition);
            if (realDistance < minDistance)
                return true;
            return false;
        }
        
        public static void collisionHandle(PhysicalCircleWorker worker1, PhysicalCircleWorker worker2)
        {
            int temp = worker1.XSpeed;
            worker1.XSpeed = worker2.XSpeed;
            worker2.XSpeed = temp;
            temp = worker1.YSpeed;
            worker1.YSpeed = worker2.YSpeed;
            worker2.YSpeed = temp;

            worker1.oneStep();
            worker2.oneStep();
        }
    }
}
