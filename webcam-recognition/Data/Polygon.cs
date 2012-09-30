namespace AirHockey.Recognition.Client.Data
{
    using Point = System.Drawing.Point;

    public class Polygon
    {
        public Point MinPoint { get; set; }

        public Point MaxPoint { get; set; }

        public Polygon(Point minPoint, Point maxPoint)
        {
            MinPoint = minPoint;
            MaxPoint = maxPoint;
        }
    }
}