using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.Util;
using System.Runtime.InteropServices;
using CosmeticImiTechCamera;

namespace CosmeticDetected
{
    class RotateRect
    {
        public RotateRect(PointF center, float width, float height, float angle)
        {
            Center = center;
            Width = width;
            Height = height;
            Angle = angle;
        }

        PointF Center;
        float Width;
        float Height;
        float Angle;
    }

    class ObjectDetector
    {
        Random crandom = new Random();
        public Image<Gray, Byte> OriginImg = null;
        public Image<Bgr, Byte> ColorImg = null;

        internal List<PointF> ObjectLocal = new List<PointF>();

        public ObjectDetector(string ImagePath)
        {
            OriginImg = new Image<Gray, byte>(ImagePath);
            OriginImg.ROI = ImiTechCamera.RoiRegion;
            ColorImg = OriginImg.Convert<Bgr, byte>();
        }

        public ObjectDetector(Image<Gray, byte> InputImg)
        {
            OriginImg = InputImg.Copy();
            //OriginImg.ROI = new Rectangle(104, 79, 1240, 828);
            ColorImg = OriginImg.Convert<Bgr, byte>();
        }

        public void ObjectDetected()
        {
            Image<Gray, Byte> thres_img = OriginImg.CopyBlank();
            ObjectLocal = new List<PointF>();
            
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            double area;
            Mat hierachy = new Mat();
            Point offset = new Point();
            Point[] StampContour;

            Point[] SideCorner = new Point[4];
            PointF Center = new PointF();
            PointF DrawPoint = new PointF();

            Cross2DF DrawCross = new Cross2DF();

            double ColorR = 0;
            double ColorG = 0;
            double ColorB = 0;

            //thres_img = OriginImg.Canny(50, 105);
            CvInvoke.Threshold(OriginImg, thres_img, 170, 255, ThresholdType.Binary);
            thres_img = thres_img.Dilate(3);
            thres_img = thres_img.Erode(3);
            thres_img = thres_img.Not();
            Point[] convex;
            float Angle = 0;

            CvInvoke.FindContours(thres_img, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone, offset);
            for (int c = 0; c < contours.Size; c++)
            {
                area = CvInvoke.ContourArea(contours[c], false);
                if (10000 <= area && area <= 50000)
                {
                    RGB_Value(out ColorR, out ColorG, out ColorB);
                    //CvInvoke.DrawContours(ColorImg, contours, c, new MCvScalar(ColorR, ColorG, ColorB), 2, LineType.EightConnected, hierachy, 2147483647, offset);
                    StampContour = contours[c].ToArray();
                    RotatedRect rrec = CvInvoke.MinAreaRect(contours[c]);
                    PointF[] pointfs = rrec.GetVertices();

                    convex = GetPolygonVertex(StampContour, 3);
                    for (int j = 0; j < convex.Length; j++)
                        CvInvoke.Circle(ColorImg, convex[j], 3, new MCvScalar(0, 255, 255, 255), 6);
                    
                    //PointF[] convex = Array.ConvertAll<Point, PointF>(StampContour, new Converter<Point, PointF>(PonitToPointF));
                    
                    /*for (int j = 0; j < pointfs.Length; j++)
                        CvInvoke.Line(ColorImg, new Point((int)pointfs[j].X, (int)pointfs[j].Y), new Point((int)pointfs[(j + 1) % 4].X, (int)pointfs[(j + 1) % 4].Y), new MCvScalar(0, 255, 0, 255), 4);*/
                    pointfs = OrderPoint(pointfs, rrec.Center);
                    SideCorner = GetObjectCorner(StampContour, pointfs, out Angle);
                    for (int j = 0; j < SideCorner.Length; j++)
                        CvInvoke.Line(ColorImg, new Point((int)SideCorner[j].X, (int)SideCorner[j].Y), new Point((int)SideCorner[(j + 1) % 4].X, (int)SideCorner[(j + 1) % 4].Y), new MCvScalar(ColorR, ColorG, ColorB), 4);

                    Center = CalCenter(SideCorner);
                    DrawPoint = CalDrawPoint(Center, Angle, 30);

                    ObjectLocal.Add(DrawPoint);

                    DrawCross = new Cross2DF(DrawPoint, 20, 20);
                    ColorImg.Draw(DrawCross, new Bgr(0, 255, 0), 2);
                }

            }


        }

        private void RGB_Value(out double R, out double G, out double B)
        {
            R = crandom.NextDouble();
            G = crandom.NextDouble();
            B = crandom.NextDouble();

            R *= 255;
            G *= 255;
            B *= 255;

            if (R + G + B < 355.0 && R < 150.0)
                R += 150;
            else if (R + G + B < 355.0 && G < 150.0)
                G += 150;
            else if (R + G + B < 355.0 && B < 150.0)
                B += 150;
        }

        private Point[] GetObjectCorner(Point[] ContourS, PointF[] RectCorner, out float Angle)
        {
            Point[] DetectTop = new Point[3];
            Point[] DetectDown = new Point[3];
            Point[] DetectLeft = new Point[3];
            Point[] DetectRight = new Point[3];

            int[] DetectHorizontal = new int[2];
            int[] DetectVertical = new int[2];


            Point[] TopPoint = new Point[2];
            Point[] DownPoint = new Point[2];
            Point[] LeftPoint = new Point[2];
            Point[] RightPoint = new Point[2];

            Point[] Corner = new Point[4];

            for (int i = 0; i < 3; i++)
            {
                DetectTop[i] = new Point(((int)RectCorner[3].X - (int)RectCorner[2].X) * (i + 2) / 5 + (int)RectCorner[2].X, ((int)RectCorner[3].Y - (int)RectCorner[2].Y) * (i + 2) / 5 + (int)RectCorner[2].Y);
                DetectDown[i] = new Point(((int)RectCorner[0].X - (int)RectCorner[1].X) * (i + 2) / 5 + (int)RectCorner[1].X, ((int)RectCorner[0].Y - (int)RectCorner[1].Y) * (i + 2) / 5 + (int)RectCorner[1].Y);
                DetectLeft[i] = new Point(((int)RectCorner[1].X - (int)RectCorner[2].X) * (i + 2) / 5 + (int)RectCorner[2].X, ((int)RectCorner[1].Y - (int)RectCorner[2].Y) * (i + 2) / 5 + (int)RectCorner[2].Y);
                DetectRight[i] = new Point(((int)RectCorner[0].X - (int)RectCorner[3].X) * (i + 2) / 5 + (int)RectCorner[3].X, ((int)RectCorner[0].Y - (int)RectCorner[3].Y) * (i + 2) / 5 + (int)RectCorner[3].Y);
            }

            for (int i = 0; i < 2; i++)
            {
                DetectHorizontal[i] = (DetectTop[i].X + DetectDown[i].X) / 2;
                DetectVertical[i] = (DetectLeft[i].Y + DetectRight[i].Y) / 2;
            }

            for (int i = 0; i < ContourS.Length; i++)
            {
                if (DetectTop[0].X == ContourS[i].X && ContourS[i].Y < DetectVertical[1])
                    TopPoint[0] = ContourS[i];
                if (DetectTop[2].X == ContourS[i].X && ContourS[i].Y < DetectVertical[1])
                    TopPoint[1] = ContourS[i];
                if (DetectDown[0].X == ContourS[i].X && ContourS[i].Y > DetectVertical[1])
                    DownPoint[0] = ContourS[i];
                if (DetectDown[2].X == ContourS[i].X && ContourS[i].Y > DetectVertical[1])
                    DownPoint[1] = ContourS[i];
                if (DetectLeft[0].Y == ContourS[i].Y && ContourS[i].X < DetectHorizontal[1])
                    LeftPoint[0] = ContourS[i];
                if (DetectLeft[2].Y == ContourS[i].Y && ContourS[i].X < DetectHorizontal[1])
                    LeftPoint[1] = ContourS[i];
                if (DetectRight[0].Y == ContourS[i].Y && ContourS[i].X > DetectHorizontal[1])
                    RightPoint[0] = ContourS[i];
                if (DetectRight[2].Y == ContourS[i].Y && ContourS[i].X > DetectHorizontal[1])
                    RightPoint[1] = ContourS[i];
            }

            Corner[0] = CalCorner(LeftPoint, TopPoint);
            Corner[1] = CalCorner(LeftPoint, DownPoint);
            Corner[2] = CalCorner(RightPoint, DownPoint);
            Corner[3] = CalCorner(RightPoint, TopPoint);

            float LeftAngle = 0;
            if ((Corner[0].X - Corner[1].X) == 0)
                LeftAngle = 90;
            else
                LeftAngle = SlopeToAngleReferHorizon((float)((Corner[0].Y - Corner[1].Y) / (Corner[0].X - Corner[1].X)));

            float RightAngle = 0;
            if ((Corner[3].X - Corner[2].X) == 0)
                RightAngle = 90;
            else
                RightAngle = SlopeToAngleReferHorizon((float)((Corner[3].Y - Corner[2].Y) / (Corner[3].X - Corner[2].X)));

            if (LeftAngle >= 0 && RightAngle <= 0)
                Angle = (LeftAngle + RightAngle + 180) / 2;
            else if (LeftAngle < 0 && RightAngle > 0)
                Angle = (LeftAngle + RightAngle + 180) / 2;
            else
            Angle = (LeftAngle + RightAngle) / 2;

            return Corner;
        }

        private PointF[] OrderPoint(PointF[] points, PointF Center)
        {
            PointF[] Corner = new PointF[4];
            for (int i = 0; i < points.Length; i++)
            {
                if (Center.X < points[i].X && Center.Y < points[i].Y)
                    Corner[0] = points[i];
                else if (Center.X > points[i].X && Center.Y < points[i].Y)
                    Corner[1] = points[i];
                else if (Center.X > points[i].X && Center.Y > points[i].Y)
                    Corner[2] = points[i];
                else if (Center.X < points[i].X && Center.Y > points[i].Y)
                    Corner[3] = points[i];
            }
            return Corner;
        }

        private Point CalCorner(Point[] SideVertical, Point[] SideHorizontal)
        {
            Point Corner = new Point();
            float m1 = 0;
            float m2 = 0;

            if (SideVertical[1].X - SideVertical[0].X == 0 && SideHorizontal[1].X - SideHorizontal[0].X != 0)
            {
                Corner.X = SideVertical[0].X;
                m2 = (SideHorizontal[1].Y - SideHorizontal[0].Y) / (SideHorizontal[1].X - SideHorizontal[0].X);
                Corner.Y = (int)(m2 * (SideVertical[0].X - SideHorizontal[0].X) + SideHorizontal[0].Y);
                return Corner;
            }
            else if (SideHorizontal[1].X - SideHorizontal[0].X == 0 && SideVertical[1].X - SideVertical[0].X != 0)
            {
                Corner.X = SideHorizontal[0].X;
                m1 = (SideVertical[1].Y - SideVertical[0].Y) / (SideVertical[1].X - SideVertical[0].X);
                Corner.Y = (int)(m1 * (SideHorizontal[0].X - SideVertical[0].X) + SideVertical[0].Y);
                return Corner;
            }
            else if (SideVertical[1].X - SideVertical[0].X == 0 && SideHorizontal[1].X - SideHorizontal[0].X == 0)
            {
                Corner.X = SideVertical[0].X;
                Corner.Y = SideVertical[0].Y;
            }

            //垂直線之後嘗試線擬合
            m1 = (float)(SideVertical[1].Y - SideVertical[0].Y) / (float)(SideVertical[1].X - SideVertical[0].X);
            m2 = -1 / m1;
            Corner = SolveCramerRule(m1, SideVertical[0], m2, SideHorizontal[0]);

            return Corner;
        }

        private Point SolveCramerRule(float Slope1, Point P1, float Slope2, Point P2)
        {
            Point SolveP = new Point();

            SolveP.X = (int)((P2.X * Slope2 - P2.Y - P1.X * Slope1 + P1.Y) / (Slope2 - Slope1));
            SolveP.Y = (int)((P2.X * Slope1 * Slope2 - Slope1 * P2.Y - P1.X * Slope1 * Slope2 + P1.Y * Slope2) / (Slope2 - Slope1));

            return SolveP;
        }

        public PointF CalCenter(Point[] Corners)
        {
            PointF Center = new PointF();

            for (int i = 0; i < Corners.Length; i++)
            {
                Center.X += Corners[i].X;
                Center.Y += Corners[i].Y;
            }
            Center.X /= (float)Corners.Length;
            Center.Y /= (float)Corners.Length;
            return Center;
        }

        private PointF CalDrawPoint(PointF center, float angle, float DistToCenter)
        {
            float rad = (float)(Math.Abs(angle) * Math.PI / 180);
            float distX = (float)(DistToCenter * Math.Cos(rad));
            float distY = (float)(DistToCenter * Math.Sin(rad));

            if (angle >= 0)
            {
                center.X -= distX;
                center.Y += distY;
            }
            else
            {
                center.X += distX;
                center.Y += distY;
            }

            return center;
        }

        private static PointF PonitToPointF(Point pot)
        {
            PointF PotF = new PointF();
            int num = 0;

            PotF.X = (float)pot.X;
            PotF.Y = (float)pot.Y;
            num++;


            return PotF;
        }

        private Point[] GetPolygonVertex(Point[] StampContours, int Mask)
        {
            float ForwardAngle = 0;
            float BackAngle = 0;
            float slope = 0;
            List<Point> Vertex = new List<Point>();

            for (int i = Mask; i < StampContours.Length; i++)
            {
                if (StampContours[(i + Mask) % StampContours.Length].X - StampContours[i].X == 0)
                    ForwardAngle = 90.0f;
                else
                {
                    slope = (float)(StampContours[(i + Mask) % StampContours.Length].Y - StampContours[i].Y) / (float)(StampContours[(i + Mask) % StampContours.Length].X - StampContours[i].X);
                    slope = (float)Math.Atan(Math.Abs(slope));
                    slope *= 180 / (float)Math.PI;
                    ForwardAngle = slope;
                }

                if (StampContours[i].X - StampContours[i - Mask].X == 0)
                    BackAngle = 90.0f;
                else
                {
                    slope = (float)(StampContours[i].Y - StampContours[i - Mask].Y) / (float)(StampContours[i].X - StampContours[i - Mask].X);
                    slope = (float)Math.Atan(Math.Abs(slope));
                    slope *= 180 / (float)Math.PI;
                    BackAngle = slope;
                }

                if (Math.Abs(ForwardAngle - BackAngle) > 35)
                    Vertex.Add(StampContours[i]);

            }
            Point[] Contours = Vertex.ToArray();
            return Contours;
        }

        /*private RotatedRect LargestRect(Point[] StampContours)
        {
            float MaxArea = 0;
            Point CandidatePointFirst = StampContours[0];
            Point CandidatePointSecond = StampContours[0];
            Point CandidatePointThird = StampContours[0];
            float Slope1=0;
            float Slope2=0;
            int Jindex = 0;
            float Length1 = 0;
            float Length2 = 0;

            for (int i = 0; i < StampContours.Length; i++)
            {
                CandidatePointFirst = StampContours[i];
                for (int j = i + 1; j < StampContours.Length; j++)
                {
                    CandidatePointSecond = StampContours[Jindex];
                    if ((float)(StampContours[Jindex].X - StampContours[i].X) != 0)
                    {
                        Slope1 = (float)(StampContours[Jindex].Y - StampContours[i].Y) / (float)(StampContours[Jindex].X - StampContours[i].X);
                        Slope2 = -1 / Slope1;
                    }
                    else
                    {
                        Slope1 = (float)StampContours[i].X;
                        Slope2 = 0;
                    }


                }
            }

        }*/

        private float Distance(float Point1X, float Point1Y, float Point2X, float Point2Y)
        {
            float distance = (float)Math.Sqrt((Point2Y - Point1Y) * (Point2Y - Point1Y) + (Point2X - Point1X) * (Point2X - Point1X));
            return distance;
        }

        private float SlopeToAngleReferHorizon(float Slope)
        {
            float Angle = (float)Math.Atan(Slope);
            Angle *= 180 / (float)Math.PI;

            return Angle;
        }



    }
}
