using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CosmeticDetected
{
    class ConvertCoordinate
    {
        /** \brief 是否已取得手臂軸心位置 */
        internal static bool IsGetAxisPos = false;

        /** \brief 是否已計算轉換座標(X Axis) */
        static bool IsCalXAxisCoort = false;
        /** \brief 是否已計算轉換座標(Y Axis) */
        static bool IsCalYAxisCoort = false;

        /** \brief 是否已計算轉換座標 */
        internal static bool IsCalConvertCoort = false;

        /** \brief 座標轉換系數(X Axis) */
        internal static double ImgXMoveX = 0;
        internal static double ImgXMoveY = 0;
        internal static double LengthPropertionX = 0;

        /** \brief 座標轉換系數(Y Axis) */
        internal static double ImgYMoveX = 0;
        internal static double ImgYMoveY = 0;
        internal static double LengthPropertionY = 0;

        /** \brief 手臂與攝影機座標間的偏差角 */
        internal static double ArmImgAngle = 0;

        #region 計算手臂軸心位置
        internal static PointF CalRotAxisCenter(double RotAng, PointF OriginPoint, PointF OffsetPoint)
        {
            double RadianAng = RotAng * Math.PI / 180;

            double cosAng = Math.Cos(RadianAng);
            double sinAng = Math.Sin(RadianAng);

            double DetA = 2 * (1 - cosAng);
            double PointX = (OffsetPoint.X * (1 - cosAng) - OriginPoint.X * (cosAng - cosAng * cosAng) - OriginPoint.Y * (sinAng - sinAng * cosAng) + (OffsetPoint.Y * sinAng) + (OriginPoint.X * sinAng * sinAng) - (OriginPoint.Y * sinAng * cosAng)) / DetA;
            double PointY = (OffsetPoint.Y * (1 - cosAng) + OriginPoint.X * (sinAng - sinAng * cosAng) - OriginPoint.Y * (cosAng - cosAng * cosAng) - OffsetPoint.X * sinAng + OriginPoint.X * sinAng * cosAng + OriginPoint.Y * sinAng * sinAng) / DetA;

            PointF ArmImgPos = new PointF((float)PointX, (float)PointY);

            IsGetAxisPos = true;

            return ArmImgPos;
        }
        #endregion

        #region 計算手臂與影像座標轉換
        //手臂座標轉換成影像座標(X Axis)
        internal static void CalCovtoCoodinateX(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY)
        {
            LengthPropertionX = Math.Sqrt((ImgMoveX * ImgMoveX) + (ImgMoveY * ImgMoveY));

            double slope = 0;
            double angle = 0;

            //計算影像位移方向與X軸的夾角
            if (ImgMoveX != 0)
            {
                slope = ImgMoveY / ImgMoveX;
                angle = Math.Abs(slope);
                angle = Math.Atan(angle);
            }
            else
                angle = 90;

            //判斷在X軸上方或下方，作向量旋轉
            if (slope > 0)
            {
                ImgXMoveX = ArmMoveX * Math.Cos(angle) - ArmMoveY * Math.Sin(angle);
                ImgXMoveY = (ArmMoveX * Math.Sin(angle)) + ArmMoveY * Math.Cos(angle);
            }
            else
            {
                ImgXMoveX = ArmMoveX * Math.Cos(angle) + ArmMoveY * Math.Sin(angle);
                ImgXMoveY = (-(ArmMoveX * Math.Sin(angle))) + ArmMoveY * Math.Cos(angle);
            }

            if (ImgMoveX < 0)
            {
                ImgXMoveX = -ImgXMoveX;
                ImgXMoveY = -ImgXMoveY;
            }

            IsCalXAxisCoort = true;
            IsCalConvertCoort = CheckConvertCoortIsCal();
            return;
        }

        //手臂座標轉換成影像座標(Y Axis)
        internal static void CalCovtoCoodinateY(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY)
        {
            LengthPropertionY = Math.Sqrt((ImgMoveX * ImgMoveX) + (ImgMoveY * ImgMoveY));
            double slope = 0;
            double angle = 0;

            //計算影像位移方向與Y軸的夾角
            if (ImgMoveX != 0)
            {
                slope = ImgMoveY / ImgMoveX;
                angle = Math.Abs((slope - 0) / (1 + slope * 0));
                angle = Math.Atan(angle);
                angle = (Math.PI / 2) - angle;


            }
            else
                angle = 0;


            //判斷在Y軸左方或右方，作向量旋轉
            if (slope > 0)
            {
                ImgYMoveX = ArmMoveX * Math.Cos(angle) + ArmMoveY * Math.Sin(angle);
                ImgYMoveY = (-(ArmMoveX * Math.Sin(angle))) + ArmMoveY * Math.Cos(angle);
            }
            else
            {
                ImgYMoveX = ArmMoveX * Math.Cos(angle) - ArmMoveY * Math.Sin(angle);
                ImgYMoveY = (ArmMoveX * Math.Sin(angle)) + ArmMoveY * Math.Cos(angle);
            }

            if (ImgMoveY < 0)
            {
                ImgYMoveX = -ImgYMoveX;
                ImgYMoveY = -ImgYMoveY;
            }

            IsCalYAxisCoort = true;
            IsCalConvertCoort = CheckConvertCoortIsCal();
            return;
        }

        //是否以計算X-Y分量的轉換座標參數
        private static bool CheckConvertCoortIsCal()
        {
            if (IsCalXAxisCoort && IsCalYAxisCoort)
                return true;
            else
                return false;
        }

        //影像移動量座標轉換成手臂座標
        internal static void ImgCovtoArm(float ImgMoveX, float ImgMoveY, out double ArmMoveX, out double ArmMoveY)
        {
            double ProperX = ImgMoveX / LengthPropertionX;
            double ProperY = ImgMoveY / LengthPropertionY;

            double ImgXContMoveX = ProperX * ImgXMoveX;
            double ImgXContMoveY = ProperX * ImgXMoveY;

            double ImgYContMoveX = ProperY * ImgYMoveX;
            double ImgYContMoveY = ProperY * ImgYMoveY;

            ArmMoveX = ImgXContMoveX + ImgYContMoveX;
            ArmMoveY = ImgXContMoveY + ImgYContMoveY;
        }

        internal static void ArmCovtoImg(double ArmMoveX, double ArmMoveY, out float ImgMoveX, out float ImgMoveY)
        {
            double DetA = ImgXMoveX * ImgYMoveY - ImgYMoveX * ImgXMoveY;
            double ProperX = (ArmMoveX * ImgYMoveY - ArmMoveY * ImgYMoveX) / DetA;
            double ProperY = (ArmMoveY * ImgXMoveX - ArmMoveX * ImgXMoveY) / DetA;

            ImgMoveX = (float)(ProperX * LengthPropertionX);
            ImgMoveY = (float)(ProperY * LengthPropertionY);
        }

        internal static double ImgToArmAngle(double ArmMoveX, double ArmMoveY, float ImgMoveX, float ImgMoveY)
        {
            double slopeI = 0;
            double slopeA = 0;
            double angle = 0;
            if (ArmMoveX != 0 && ImgMoveX != 0)
            {
                slopeA = ArmMoveY / ArmMoveX;
                slopeI = (double)(ImgMoveY / ImgMoveX);
                angle = Math.Abs((slopeA - slopeI) / (1 + (slopeA * slopeI)));
                angle = Math.Atan(angle);
                angle = angle * 180 / Math.PI;
            }
            if (slopeA > slopeI)
                return -angle;
            else
                return angle;
        }

        #endregion


    }
}
