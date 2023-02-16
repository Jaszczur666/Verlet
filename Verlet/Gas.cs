using System;
using System.Collections.Generic;
using static System.Math;
using System.Text;

namespace Verlet
{
    class Gas
    {
        public List<Particle> particles;
        public readonly double l = 35.0;
        public double t = 0;
        public double pot = 0;
        readonly Random rand;
        double rnd()
        {
            return rand.NextDouble();
        }
        public Gas()
        {
            particles = new List<Particle>();
            rand = new Random();
        }
        public void Initialize(int Num_of_part, double dt)
        {
            
            int col, row;
            double kin;
            col = 0;
            row = 0;
            particles.Clear();
            for (int i = 0; i < Num_of_part; i++)
            {
                Particle tempparticle = new Particle();
                tempparticle.f.x = 0;
                tempparticle.f.y = 0;
                tempparticle.pos.x = row * 1.0 + (rnd() - 0.5) * 0.01;
                tempparticle.v.x = (tempparticle.pos.x - (row * 1.0)) / dt;
                tempparticle.pos.y = 0.866 * col + 0.5 + (rnd() - 0.5) * 0.01;
                tempparticle.v.y = (tempparticle.pos.y - 0.866 * col) / dt;
                if (col % 2 == 0) tempparticle.pos.x += 0.5;
                row++;
                if (row > (l - 1))
                {
                    row = 0;
                    col++;
                };
                particles.Add(tempparticle);
            }
            RemoveVelocity();
            kin = CalculateKinetic();
            Thermostat(kin);
        }
        public void RemoveVelocity() {
            double sumvx, sumvy;
            int size;
            size = particles.Count;
            sumvx = 0;
            sumvy = 0;
            for (int i = 0; i < size; i++)
            {
                sumvx += particles[i].v.x;
                sumvy += particles[i].v.y;
            }
            for (int i = 0; i < size; i++)
            {
                particles[i].v.x -= sumvx / size;
                particles[i].v.y -= sumvy / size;
            }
        }
        public double CalculateKinetic()
        {
            double tempkin, vx, vy;
            tempkin = 0;
            for (int i = 0; i < particles.Count; i++)
            {
                vx = particles[i].v.x;
                vy = particles[i].v.y;
                tempkin += 24 * (vx * vx + vy * vy);
            }
            return tempkin;
        }
        public void Thermostat( double kin)
        {
            double beta;
            int n;
            n = particles.Count;
            beta = Sqrt((n - 1) * 1 / kin);
            for (int i = 0; i < n; i++)
            {
                particles[i].v.x *= beta;
                particles[i].v.y *= beta;
            }

        }
        public void Evolve(double dt)
        {
            t += dt;
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].pos.x += particles[i].v.x * dt + 0.5 * dt * dt * particles[i].f.x;
                particles[i].pos.y += particles[i].v.y * dt + 0.5 * dt * dt * particles[i].f.y;
                if (particles[i].pos.x > l) particles[i].pos.x -= Floor(particles[i].pos.x / l) * l;
                if (particles[i].pos.y > l) particles[i].pos.y -= Floor(particles[i].pos.y / l) * l;
                if (particles[i].pos.x < 0) particles[i].pos.x += Floor(Abs(particles[i].pos.x / l) + 1) * l;
                if (particles[i].pos.y < 0) particles[i].pos.y += Floor(Abs(particles[i].pos.y / l) + 1) * l;
            }
            CalculateNewForcesAndPotential();
            //std::cout<<i<<std::endl;
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].v.x += 0.5 * dt * (particles[i].nf.x + particles[i].f.x);
                particles[i].v.y += 0.5 * dt * (particles[i].nf.y + particles[i].f.y);


            }
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].f.x = particles[i].nf.x;
                particles[i].f.y = particles[i].nf.y;
            }
        }
        public void CalculateNewForcesAndPotential()
        {
            double distx, disty, radial, temppot, force12, force6;
            //Console.WriteLine("New Force");
            temppot = 0;
            pot = 0;
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].nf.x = 0;
                particles[i].nf.y = 0;
            }
            for (int i = 0; i < particles.Count-1; i++)
            {
                for (int j =i+1; j < particles.Count ; j++)
                {
                    if (i != j)
                    {
                        distx = particles[i].pos.x - particles[j].pos.x;
                        if (distx > (l / 2)) distx -=  l;
                        if (distx < (-l / 2)) distx += l;
                        disty = particles[i].pos.y - particles[j].pos.y;
                        if (disty > (l / 2)) disty -= l;
                        if (disty < (-l / 2)) disty += l;
                        radial = Math.Sqrt(distx * distx + disty * disty);
                        if (radial <5)
                        {
                            force12 = Pow(radial, -12);
                            force6 = Pow(radial, -6);
                            temppot += 4.0 * (force12 - force6)/( particles.Count);
                            double tfx= 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            double tfy = 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            particles[i].nf.x += tfx;//48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].nf.x -= tfx;// 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[i].nf.y += tfy;// 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].nf.y -= tfy;// 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            //Console.WriteLine("" + i+ " "+j+" "+distx+" "+disty+" "+radial+" "+ 4.0 * (force12 - force6) +" "+ tfx/ Math.Sqrt(tfx * tfx + tfy * tfy) + " "+tfy/ Math.Sqrt(tfx * tfx + tfy * tfy) + " "+Math.Sqrt(tfx*tfx+tfy*tfy));
                        }
                    }
                }
            }
            pot = temppot;
        }
        public void CalculateForcesAndPotential()
        {
            double distx, disty, radial, temppot, force12, force6;
            
            temppot = 0;
            pot = 0;
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].f.x = 0;
                particles[i].f.y = 0;
            }
            for (int i = 0; i < particles.Count; i++)
            {
                for (int j = i + 1; j < particles.Count; j++)
                {
                    if (i != j)
                    {
                        distx = particles[i].pos.x - particles[j].pos.x;
                        if (distx > (l / 2)) distx -= l;
                        if (distx < (-l / 2)) distx += l;
                        disty = particles[i].pos.y - particles[j].pos.y;
                        if (disty > (l / 2)) disty -= l;
                        if (disty < (-l / 2)) disty += l;
                        radial = Math.Sqrt(distx * distx + disty * disty);
                        if (radial < 5)
                        {
                            force12 = Pow(radial, -12);
                            force6 = Pow(radial, -6);
                            temppot += 4.0 * (force12 - force6) / (particles.Count * particles.Count);
                            double tfx = 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            double tfy = 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            particles[i].f.x += tfx;//48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].f.x -= tfx;// 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[i].f.y += tfy;// 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].f.y -= tfy;// 48 * disty * (force12 / radial - 0.50 * force6 / radial);
//                          Console.WriteLine("" + i + " " + j + " " + distx + " " + disty + " " + radial + " " + 4.0 * (force12 - force6) + " " + tfx / Math.Sqrt(tfx * tfx + tfy * tfy) + " " + tfy / Math.Sqrt(tfx * tfx + tfy * tfy) + " " + Math.Sqrt(tfx * tfx + tfy * tfy));
                            /*
                            force12 = Pow(radial, -6.0);
                            force6 = Pow(radial, -3.0);
                            temppot += 8.0 * (force12 - force6)/(particles.Count* particles.Count);
                            particles[i].f.x += 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].f.x -= 48 * distx * (force12 / radial - 0.50 * force6 / radial);
                            particles[i].f.y += 48 * disty * (force12 / radial - 0.50 * force6 / radial);
                            particles[j].f.y -= 48 * disty * (force12 / radial - 0.50 * force6 / radial); */
                            //                            Console.WriteLine("F12 f6 " + force12 + " " + force6+ " " + 48 * disty * (force12 / radial - 0.50 * force6 / radial));
                        }
                    }
                }
            }
            pot = temppot;
        }
        public string Dumppositions()
        {
            string temp="";
            for (int i=0;i<particles.Count;i++){
                temp += particles[i].pos.x + " " + particles[i].pos.y+"\r\n";
            }
            return temp;
        }
        public string Dumpvelo()
        {
            string temp = "";
            for (int i = 0; i < particles.Count; i++)
            {
                temp += particles[i].v.x + " " + particles[i].v.y + "\r\n";
            }
            return temp;
        }
        public string Dumpdistances()
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < particles.Count; i++)
            {
                for (int j = 0; j < i; j++)
                { double distx=0, disty=0;

                    distx = particles[i].pos.x - particles[j].pos.x;
                    if (distx > (l / 2)) distx -= l;
                    if (distx < (-l / 2)) distx += l;
                    disty = particles[i].pos.y - particles[j].pos.y;
                    if (disty > (l / 2)) disty -= l;
                    if (disty < (-l / 2)) disty += l;
                    sb.Append(Math.Sqrt(Pow(distx, 2) + Pow(disty, 2)) + "\r\n");
                }
                
            }
            return sb.ToString();
        }
    }

}

