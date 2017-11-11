using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PaintCurves
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool elipsBottomPressed, curveButtonPressed , drawButtonPressed = false;
        private bool drawPointerPressed = false;
        Point elipsP1, elipsP2, p3 , point;
        List<Point> points = new List<Point>();
        public MainPage()
        {
            this.InitializeComponent();
            commandbar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
        }
        private void Curve_Click(object sender, RoutedEventArgs e)
        {
            curveButtonPressed = true;
            drawButtonPressed = false;
            elipsBottomPressed = false;
            points.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            elipsBottomPressed = true;
            PaintGrid.Children.Clear();
        }
        #region  Draw
        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            drawButtonPressed = true;
            elipsBottomPressed = false;
            curveButtonPressed = false;
            points.Clear();
        }

        private void PaintGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (drawButtonPressed)
            {
                drawPointerPressed = false;
                points.Clear();
            }
        }

        private void PaintGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (drawPointerPressed)
            {

                point = e.GetCurrentPoint(Window.Current.Content).Position;
                points.Add(point);
                if (points.Count == 4)
                {

                    PathFigure pthFigure = new PathFigure();
                    pthFigure.StartPoint = points[0];
                    BezierSegment pbzSeg = new BezierSegment();
                    PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                    
                    pbzSeg.Point1 = points[1];
                    pbzSeg.Point2 = points[2];
                    pbzSeg.Point3 = points[3];

                    myPathSegmentCollection.Add(pbzSeg);
                    pthFigure.Segments = myPathSegmentCollection;

                    PathFigureCollection pthFigureCollection = new PathFigureCollection();
                    pthFigureCollection.Add(pthFigure);

                    PathGeometry pthGeometry = new PathGeometry();
                    pthGeometry.Figures = pthFigureCollection;

                    Path arcPath = new Path();
                    arcPath.Stroke = new SolidColorBrush(Colors.Black);
                    arcPath.StrokeThickness = 3;
                    arcPath.Data = pthGeometry;

                    PaintGrid.Children.Add(arcPath);
                    Point temp = points[3];
                    points.Clear();
                    points.Add(temp);
                }
                }
            } 

        private void PaintGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if(curveButtonPressed)
            {
                
                Point temp;
                temp = e.GetCurrentPoint(Window.Current.Content).Position;
                points.Add(temp);
                Ellipse ellipse = new Ellipse();
                ellipse.HorizontalAlignment = HorizontalAlignment.Left;
                ellipse.VerticalAlignment = VerticalAlignment.Top;
                ellipse.Fill = new SolidColorBrush(Windows.UI.Colors.SteelBlue);
                ellipse.Height = 10;
                ellipse.Width = 10;
                ellipse.RenderTransform = new TranslateTransform()
                {
                    X = temp.X,
                    Y = temp.Y
                };

                PaintGrid.Children.Add(ellipse);
            }
            else if(drawButtonPressed)
            {
                drawPointerPressed = true;
            }
        }
#endregion
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if(curveButtonPressed && points.Count > 1 )
            {
                PathFigure pthFigure = new PathFigure();
                pthFigure.StartPoint = points[0];
                PolyBezierSegment pbzSeg = new PolyBezierSegment();
                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                if (points.Count == 2)
                {
                    pbzSeg.Points.Add(points[1]);
                    pbzSeg.Points.Add(points[1]);
                    pbzSeg.Points.Add(points[1]);
                }
                else if (points.Count == 3)
                {
                    pbzSeg.Points.Add(points[1]);
                    pbzSeg.Points.Add(points[2]);
                    pbzSeg.Points.Add(points[2]);
                }
                else if (points.Count % 3 == 1)
                {
                    for (int i = 1; i < points.Count; i++)
                    {
                        pbzSeg.Points.Add(points[i]);
                    }

                }
                else
                {
                    for (int i = 1; i < points.Count; i++)
                    {
                        pbzSeg.Points.Add(points[i]);
                    }

                    pbzSeg.Points.Add(points[points.Count - 1]);
                    if (points.Count % 3 == 2)
                    {
                        pbzSeg.Points.Add(points[points.Count - 1]);
                    }
                }

                myPathSegmentCollection.Add(pbzSeg);
                pthFigure.Segments = myPathSegmentCollection;

                PathFigureCollection pthFigureCollection = new PathFigureCollection();
                pthFigureCollection.Add(pthFigure);

                PathGeometry pthGeometry = new PathGeometry();
                pthGeometry.Figures = pthFigureCollection;

                Path arcPath = new Path();
                arcPath.Stroke = new SolidColorBrush(Colors.Black);
                arcPath.StrokeThickness = 3;
                arcPath.Data = pthGeometry;
                //arcPath.Fill = new SolidColorBrush(Colors.Yellow);

                PaintGrid.Children.Add(arcPath);
                points.Clear();

            }
           
        }


    }
}
