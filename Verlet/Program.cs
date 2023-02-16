using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Verlet
{

    class Program
    {
        static void doExperiment(int n)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            double kin=0, pot=0, tot=0;
            double esr = 0.0;
            double esr2 = 0.0;
            Gas experiment = new Gas();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            experiment.Initialize(n, 0.0064);
            //for (int i = 0; i < experiment.particles.Count; i++) { Console.WriteLine(experiment.particles[i].pos.x); };
            experiment.CalculateForcesAndPotential();
            Console.WriteLine("i  t kin  pot tot tcalc");
            int num_samples = 0;
            for (int i = 0; i < 50000; i++)
            {
                experiment.Evolve(0.0064);

                kin = experiment.CalculateKinetic();
                //Console.WriteLine(experiment.particles[0].pos.x.ToString("G4") + " " + experiment.particles[0].pos.y.ToString("G4")+" "+experiment.particles[0].v.x.ToString("G5") + " " + experiment.particles[0].v.y.ToString("G5"));
                if (i < 1000) {
                    if (i % 100 == 0) experiment.Thermostat(kin);
                }
                else
                {
                    if (i % 100 == 0)
                    {
                        experiment.Thermostat(kin);

                        esr += experiment.pot;
                        esr2 += (experiment.pot * experiment.pot);
                        num_samples++;
                        //Console.Error.WriteLine(num_samples +" "+(experiment.pot).ToString("G5") + " " + (esr2 / num_samples).ToString("G5") + " " + (esr / num_samples).ToString("G5") + " " + (esr * esr / num_samples / num_samples).ToString("G5"));
                        //Console.WriteLine(experiment.pot + " " + esr + " " + esr2);
                    }
                }

                pot = experiment.pot;
                tot = kin/n + pot;
                if (i % 100 == 0)  Console.WriteLine(i+" "+experiment.t.ToString("G6", ci) + " " + (kin/n).ToString("G6", ci) + " " + (pot).ToString("G6", ci) + " " + (tot).ToString("G6", ci) + " "+(sw.ElapsedMilliseconds/1000.0).ToString("G6", ci));
            }
            //Console.Error.WriteLine((sw.ElapsedMilliseconds / 1000.0).ToString("G3", ci));
            //                Console.Error.WriteLine(num_samples+" "+ esr2.ToString("G5") + " "+ esr.ToString("G5") + " " +(esr*esr).ToString("G5"));
            double c = (esr2 / num_samples - esr * esr / (num_samples * num_samples)) / (n / 0.7 * 0.7);
            //Console.WriteLine((n / (experiment.l * experiment.l)).ToString("G3", ci) + " " + c.ToString("G3", ci)+" "+kin/n+" "+pot/n+" "+tot/n);
            //Console.WriteLine(experiment.Dumpvelo());

        }
        static void Main(string[] args)
        {
            Console.Error.WriteLine(DateTime.Now.ToString());
            Stopwatch glowny = new Stopwatch();
            glowny.Start();
            int s = 10;

            /*            for (int n = 25; n < 1200-6*s; n += 8*s)
                        {
                            Thread t1 = new System.Threading.Thread(delegate () {
                                doExperiment(n);
                            });
                            t1.Start();
                            Thread t2 = new System.Threading.Thread(delegate () {
                                doExperiment(n+s);
                            });
                            t2.Start();
                            Thread t3 = new System.Threading.Thread(delegate () {
                                doExperiment(n+2*s);
                            });
                            t3.Start();
                            Thread t4 = new System.Threading.Thread(delegate () {
                                doExperiment(n+3*s);
                            });
                            t4.Start();
                            Thread t5 = new System.Threading.Thread(delegate () {
                                doExperiment(n+4*s);
                            });
                            t5.Start();
                            Thread t6 = new System.Threading.Thread(delegate () {
                                doExperiment(n + 5*s);
                            });
                            t6.Start();
                            Thread t7 = new System.Threading.Thread(delegate () {
                                doExperiment(n + 6*s);
                            });
                            t7.Start();
                            Thread t8 = new System.Threading.Thread(delegate () {
                                doExperiment(n + 7*s);
                            });
                            t8.Start();
                            t1.Join();
                            t2.Join();
                            t3.Join();
                            t4.Join();
                            t5.Join();
                            t6.Join();
                            t7.Join();
                            t8.Join();
                            Console.Error.WriteLine("n= "+(n+7*s).ToString());
                            Console.Error.WriteLine("Zejszło " +  glowny.Elapsed.ToString());
                            //                doExperiment(n);
                        }
            */
            doExperiment(735);
            Console.Error.WriteLine("Zejszło "+glowny.ElapsedMilliseconds / 1000.0);
        }
    }
}
