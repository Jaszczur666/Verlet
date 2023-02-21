using System;
using static System.Math;

public class BVec
{
    public double x;
    public double y;
    public double z;
	public BVec()
	{
        x = 0;
        y = 0;
        z = 0;
	}
    public double BoxDistance(BVec another, double l)
    {
        double distx, disty,distz;
        distx = x - another.x;
        if (distx > (l/2.0)) distx -= l;
        if (distx < (-l / 2)) distx += l;
        disty = y - another.y;
        if (disty > (l / 2)) disty -= l;
        if (disty < (-l / 2)) disty += l;
        distz = z - another.z;
        if (distz > (l / 2)) distz -= l;
        if (distz < (-l / 2)) distz += l;
        //Console.Error.WriteLine(this.x + " " + this.y +" "+another.x+" "+another.y+" "+ distx + " " + disty);
        return Sqrt(distx * distx + disty * disty+distz*distz);
    }
//    public BVec BoxDirection(BVec another, double l)
  //  {
  //
   // }
}
