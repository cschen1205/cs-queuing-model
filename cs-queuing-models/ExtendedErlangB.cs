using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimuKit.OR.Queueing
{
    //If a significant portion of dropped calls retry, then erlang B should 
    //be replace with extended erlang B, which also considers the rate of
    //retry
    public class ExtendedErlangB : ErlangB
    {
        private double m_retry_rate;
        public double retry_rate
        {
            get { return m_retry_rate; }
            set { m_retry_rate = value; }
        }

        //Grade of Service, aka, probability of delay
        //the probability that an arbitrary caller finds all servers/agents occupied
        public override double GetGoS()
        {
            return ExtendedErlangBFormula(a, s, retry_rate);
        }

        public static double ExtendedErlangBFormula(double load, int number_of_servers, double R)
        {
            double E0 = load; //Call the initial Erlang value E0
            double P0 = ErlangBFormula(E0, number_of_servers); //calculate P0 = GoS using regular Erlang B formula and the known values of E=load and M=load
            
            double En=E0;
            double Pn=P0;
            double Enp1 = 0;
            double Pnp1 = 0;

            int iteration = 0;
            while (System.Math.Abs(Pnp1 - Pn) > Epsilon && iteration < MaxIteration)
            {
                Enp1 = E0 + R * En * Pn;
                Pnp1 = ErlangBFormula(En, number_of_servers);
                iteration++;
            }

            return Pnp1;
        }

        private static double mEpsilon = 0.00001;
        public static double Epsilon
        {
            get { return mEpsilon;  }
            set { mEpsilon = value; }
        }

        private static int mMaxIteration = 10000000;
        public static int MaxIteration
        {
            get { return mMaxIteration; }
            set { mMaxIteration = value; }
        }

        //since extended erlang b assumes call is either dropped or retry instead of putting in the queue, therefore, the AWT-based TSF is not applicable and the 
        public override double GetTSF(double AWT)
        {
            throw new NotImplementedException();
        }

        //average waiting time, the average amount of time that calls spend waiting
        public override double GetASA()
        {
            return GetGoS() * beta / (s - a);
        }
    }
}
