using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace Verlet
{
    class Particle
    {
        public BVec pos, npos, v, nv, f, nf;

        public Particle()
        {
            pos = new BVec();
            f = new BVec();
            nf = new BVec();
            npos = new BVec();
            v = new BVec();
            nv = new BVec();


        }
    }
}
