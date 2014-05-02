using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Coex.AppLab.Components.WindowsStore.Controls
{
    public class LineDrawnEventArgs : EventArgs
    {
        public double FromX { get; set; }
        public double FromY { get; set; }
        public double ToX { get; set; }
        public double ToY { get; set; }
    }

    public sealed partial class InkControl : UserControl
    {
        InkManager m_InkManager = new InkManager();
        private uint m_PenId;
        private uint _touchID;
        private Point _previousContactPt;
        private Point currentContactPt;
        private double x1;
        private double y1;
        private double x2;
        private double y2;
        private Color m_CurrentDrawingColor = Colors.Black;
        private double m_CurrentDrawingSize = 4;
        private string m_CurrentMode = "Ink";

        public delegate void LineDrawnEventHandler(object sender, LineDrawnEventArgs e);
        public event LineDrawnEventHandler LineDrawn;

        public InkManager CurrentManager
        {
            get
            {
                return m_InkManager;
            }
        } 

        public InkControl()
        {
            this.InitializeComponent();


            InkMode();

            InkCanvas.PointerPressed += new PointerEventHandler(OnCanvasPointerPressed);
            InkCanvas.PointerMoved += new PointerEventHandler(OnCanvasPointerMoved);
            InkCanvas.PointerReleased += new PointerEventHandler(OnCanvasPointerReleased);
            InkCanvas.PointerExited += new PointerEventHandler(OnCanvasPointerReleased); 
        }


        #region Pointer Event Handlers

        public void OnCanvasPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId == m_PenId)
            {
                Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                if (m_CurrentMode == "Erase")
                {
                    System.Diagnostics.Debug.WriteLine("Erasing : Pointer Released");

                    m_InkManager.ProcessPointerUp(pt);
                }
                else
                {
                    // Pass the pointer information to the InkManager.  
                    CurrentManager.ProcessPointerUp(pt);
                }
            }
            else if (e.Pointer.PointerId == _touchID)
            {
                // Process touch input  
            }

            _touchID = 0;
            m_PenId = 0;

            // Call an application-defined function to render the ink strokes.  

            RefreshCanvas();

            e.Handled = true;
        }

        private void OnCanvasPointerMoved(object sender, PointerRoutedEventArgs e)
        {

            if (e.Pointer.PointerId == m_PenId)
            {
                PointerPoint pt = e.GetCurrentPoint(InkCanvas);

                // Render a red line on the canvas as the pointer moves.   
                // Distance() is an application-defined function that tests  
                // whether the pointer has moved far enough to justify   
                // drawing a new line.  
                currentContactPt = pt.Position;
                x1 = _previousContactPt.X;
                y1 = _previousContactPt.Y;
                x2 = currentContactPt.X;
                y2 = currentContactPt.Y;

                // Emit the event with the coordinates, to be used by event subscribers
                if(LineDrawn != null)
                    LineDrawn(this, new LineDrawnEventArgs() { FromX = x1, FromY = y1, ToX = x2, ToY = y2 });

                var color = m_CurrentDrawingColor;
                var size = m_CurrentDrawingSize;

                if (Distance(x1, y1, x2, y2) > 2.0 && m_CurrentMode != "Erase")
                {
                    Line line = new Line()
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        StrokeThickness = size,
                        Stroke = new SolidColorBrush(color)
                    };


                    if (m_CurrentMode == "Highlight") line.Opacity = 0.4;
                    _previousContactPt = currentContactPt;

                    // Draw the line on the canvas by adding the Line object as  
                    // a child of the Canvas object.  
                    InkCanvas.Children.Add(line);
                }

                if (m_CurrentMode == "Erase")
                {
                    System.Diagnostics.Debug.WriteLine("Erasing : Pointer Update");

                    m_InkManager.ProcessPointerUpdate(pt);
                }
                else
                {
                    // Pass the pointer information to the InkManager.  
                    CurrentManager.ProcessPointerUpdate(pt);
                }
            }

            else if (e.Pointer.PointerId == _touchID)
            {
                // Process touch input  
            }


        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }

        public void OnCanvasPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // Get information about the pointer location.  
            PointerPoint pt = e.GetCurrentPoint(InkCanvas);
            _previousContactPt = pt.Position;

            // Accept input only from a pen or mouse with the left button pressed.   
            PointerDeviceType pointerDevType = e.Pointer.PointerDeviceType;
            if (pointerDevType == PointerDeviceType.Pen ||
                    pointerDevType == PointerDeviceType.Mouse &&
                    pt.Properties.IsLeftButtonPressed)
            {
                if (m_CurrentMode == "Erase")
                {
                    System.Diagnostics.Debug.WriteLine("Erasing : Pointer Pressed");

                    m_InkManager.ProcessPointerDown(pt);
                }
                else
                {
                    // Pass the pointer information to the InkManager.  
                    CurrentManager.ProcessPointerDown(pt);
                }

                m_PenId = pt.PointerId;

                e.Handled = true;
            }

            else if (pointerDevType == PointerDeviceType.Touch)
            {
                // Process touch input  
            }
        }
        #endregion

        #region Mode Functions

        // Change the color and width in the default (used for new strokes) to the values 
        // currently set in the current context. 
        private void SetDefaults(double strokeSize, Color color)
        {
            var newDrawingAttributes = new InkDrawingAttributes();
            newDrawingAttributes.Size = new Size(strokeSize, strokeSize);
            newDrawingAttributes.Color = color;
            newDrawingAttributes.FitToCurve = true;
            CurrentManager.SetDefaultDrawingAttributes(newDrawingAttributes);
        }
        private void InkMode()
        {
            m_CurrentMode = "Ink";
            m_InkManager.Mode = Windows.UI.Input.Inking.InkManipulationMode.Inking;
            SetDefaults(m_CurrentDrawingSize, m_CurrentDrawingColor);
        }
        private void EraseMode()
        {
            m_InkManager.Mode = Windows.UI.Input.Inking.InkManipulationMode.Erasing;
            m_CurrentMode = "Erase";
        }
        #endregion

        #region Rendering Functions
        private void RenderStroke(InkStroke stroke, Color color, double width, double opacity = 1)
        {
            var renderingStrokes = stroke.GetRenderingSegments();
            var path = new Windows.UI.Xaml.Shapes.Path();
            path.Data = new PathGeometry();
            ((PathGeometry)path.Data).Figures = new PathFigureCollection();
            var pathFigure = new PathFigure();
            pathFigure.StartPoint = renderingStrokes.First().Position;
            ((PathGeometry)path.Data).Figures.Add(pathFigure);
            foreach (var renderStroke in renderingStrokes)
            {
                pathFigure.Segments.Add(new BezierSegment()
                {
                    Point1 = renderStroke.BezierControlPoint1,
                    Point2 = renderStroke.BezierControlPoint2,
                    Point3 = renderStroke.Position
                });
            }

            path.StrokeThickness = width;
            path.Stroke = new SolidColorBrush(color);

            path.Opacity = opacity;

            InkCanvas.Children.Add(path);
        }

        private void RenderStrokes()
        {
            var strokes = m_InkManager.GetStrokes();

            foreach (var stroke in strokes)
            {
                if (stroke.Selected)
                {
                    RenderStroke(stroke, stroke.DrawingAttributes.Color, stroke.DrawingAttributes.Size.Width * 2);
                }
                else
                {
                    RenderStroke(stroke, stroke.DrawingAttributes.Color, stroke.DrawingAttributes.Size.Width);
                }
            }
        }

        private void RefreshCanvas()
        {
            InkCanvas.Children.Clear();
            RenderStrokes();
        }

        #endregion
    }
}
