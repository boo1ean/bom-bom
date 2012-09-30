namespace AirHockey.Recognition.Client.ImageProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using AForge;
    using AForge.Imaging;
    using AForge.Math.Geometry;

    using AirHockey.Recognition.Client.Data;

    using Point = System.Drawing.Point;

    public class BlobsScanner
    {
        private readonly BlobCounter blobCounter = new BlobCounter();

        readonly Dictionary<int, List<IntPoint>> hulls = new Dictionary<int, List<IntPoint>>();

        private Bitmap image;
        private Blob[] blobs;

        // Set monochromeImage to display by the control
        public IList<Polygon> ScanImage(Bitmap monochromeImage)
        {
            this.hulls.Clear();

            this.image = monochromeImage;

            this.blobCounter.ProcessImage(this.image);

            this.blobs = this.blobCounter.GetObjectsInformation();

            var grahamScan = new GrahamConvexHull();

            var polygons = new List<Polygon>();
            foreach (var blob in this.GetBlobs())
            {
                List<IntPoint> leftEdge;
                List<IntPoint> rightEdge;

                // collect edge points
                this.blobCounter.GetBlobsLeftAndRightEdges(blob, out leftEdge, out rightEdge);

                // find convex hull
                var edgePoints = new List<IntPoint>();
                edgePoints.AddRange(leftEdge);
                edgePoints.AddRange(rightEdge);

                var hull = grahamScan.FindHull(edgePoints);
                this.hulls.Add(blob.ID, hull);

                var minX = edgePoints.Min(x => x.X);
                var minY = edgePoints.Min(x => x.Y);
                var maxY = edgePoints.Max(x => x.Y);
                var maxX = edgePoints.Max(x => x.X);

                polygons.Add(new Polygon(new Point(minX, minY), new Point(maxX, maxY)));
            }
            
            return polygons;
        }

        private IEnumerable<Blob> GetBlobs()
        {
            return this.blobs.Where(it => it.Area > 20);
        }

        // Paint the control
        public void Draw(Bitmap iutputBitmap)
        {
            if (this.hulls.Count == 0)
                return;

            using (Graphics g = Graphics.FromImage(iutputBitmap))
            {
                Pen highlightPen = new Pen(Color.Red);

                foreach (Blob blob in this.GetBlobs())
                {
                    g.DrawPolygon(highlightPen, PointsListToArray(this.hulls[blob.ID]));
                }
            }
        }

        // Convert list of AForge.NET's IntPoint to array of .NET's Point
        private static System.Drawing.Point[] PointsListToArray(List<IntPoint> list)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[list.Count];

            for (int i = 0, n = list.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(list[i].X, list[i].Y);
            }

            return array;
        }
    }
}