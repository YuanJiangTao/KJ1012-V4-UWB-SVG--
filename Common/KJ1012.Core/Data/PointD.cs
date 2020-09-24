
namespace KJ1012.Core.Data
{
    public struct PointD
    {
        public static readonly PointD Empty;
        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public bool IsEmpty {
            get {
                return X == 0 && Y == 0;
            }
        }
    
        public double X { get; set; }
        public double Y { get; set; }
    }
}
