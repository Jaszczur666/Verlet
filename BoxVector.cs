using System;

public class BVec
{
    public double x;
        public double y;
	public BVec()
	{
        x = 0;
        y = 0;
	}
    public double BoxDistance(Vec another, double l)
    {
        double distx, disty;
        distx = x - another.x;
        if (distx > (l / 2)) distx = distx - l;
        if (distx < (-l / 2)) distx = distx + l;
        disty = y - another.y;
        if (disty > (l / 2)) disty = disty - l;
        if (disty < (-l / 2)) disty = disty + l;
        return sqrt(distx * distx + disty * disty);
    }
    public BVec BoxDirection(Vec another, double l)
    {

    }
}
