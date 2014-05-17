using Microsoft.Xna.Framework;

namespace Shooter
{
    public class Rectangle2D
    {
        private double x, y, width, height;

        public Rectangle2D() {}

        public Rectangle2D(double x, double y) : this(x, y, 0, 0) { }

        public Rectangle2D(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool intersects(Rectangle2D r)
        {
            return ! ( r.x >= x + width
                    || r.x + r.width <= x
                    || r.y >= y + height
                    || r.y + r.height <= y
                    );
        }

        public bool contains(double x2, double y2)
        {
            return x2 >= x && x2 < x + width && y2 >= y && y2 < y + height;
        }

        public bool contains(Vector2 v)
        {
            return contains(v.X, v.Y);
        }

        public void setLocation(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void setSize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void setFrame(double x, double y, double width, double height)
        {
            setLocation(x, y);
            setSize(width, height);
        }
    }
}
