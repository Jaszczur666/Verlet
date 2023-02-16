using System;
using static System.Math;

public class BVec
{
    public double x;
        public double y;
	public BVec()
	{
        x = 0;
        y = 0;
	}
    public double BoxDistance(BVec another, double l)
    {
        double distx, disty;
        distx = x - another.x;
        if (distx > (l/2.0)) distx -= l;
        if (distx < (-l / 2)) distx += l;
        disty = y - another.y;
        if (disty > (l / 2)) disty -= l;
        if (disty < (-l / 2)) disty += l;
        return Sqrt(distx * distx + disty * disty);
    }
//    public BVec BoxDirection(BVec another, double l)
  //  {
  //
   // }
}
