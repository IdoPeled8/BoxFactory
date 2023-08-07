public struct BoxSize
{
    public double Width { get; private set; }
    public double Height { get; private set; }

    public BoxSize(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override bool Equals(object obj)
    {
        if (obj is BoxSize BoxSize)
        {
            return Width == BoxSize.Width && Height == BoxSize.Height;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Width.GetHashCode();
            hash = hash * 23 + Height.GetHashCode();
            return hash;
        }

    }
}

