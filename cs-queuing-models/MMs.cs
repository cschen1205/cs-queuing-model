using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimuKit.OR.Queueing
{
    // interarrival time: M (exponential distribution)
    // service time: M (exponential distribution)
    // server count: s (constant)
    public class MMs : BasicModel
    {
        //mean arrival rate (expected number of arrivals per unit time) of new customers
        protected double m_lambda;
        public double lambda
        {
            get
            {
                return m_lambda;
            }
            set { m_lambda = value; }
        }

        //mean service rate for overral system (expected number of customers completing service per unit time) 
        protected double m_mu;
        public double mu
        {
            get
            {
                return m_mu;
            }
            set { m_mu = value; }
        }

        // number of servers
        protected int m_s;
        public int s
        {
            get
            {
                return m_s;
            }
            set { m_s = value; }
        }

        // utilization factor rho=lambda / (s * mu)
        protected double m_rho;
        public double rho
        {
            get
            {
                return m_rho;
            }
        }

        // expected queue length (excludes customers being served)
        protected double m_Lq;
        public double Lq
        {
            get
            {
                return m_Lq;
            }
        }

        //expected waiting time in queue (excludees service time) for each individual
        protected double m_Wq;
        public double Wq
        {
            get
            {
                return m_Wq;
            }
        }

        //expected number of customers in queueing system
        protected double m_L;
        public double L
        {
            get
            {
                return m_L;
            }
        }

        // expected value of waiting time in system (includes service time) for each individual customer.
        protected double m_W;
        public double W
        {
            get
            {
                return m_W;
            }
        }

        // probability of Wq == 0, i.e., P{Wq=0}
        protected double m_PWq0;
        public double PWq0
        {
            get
            {
                return m_PWq0;
            }
        }

        //max number of customers in a queueing system
        private int m_N;
        public int N
        {
            get { return m_N; }
            set { m_N = value; }
        }

        //load
        private double m_a;
        public double a
        {
            get { return m_a; }
            set { m_a = value; }
        }

        public override void Build()
        {
            m_rho = lambda / (s * mu);
            m_a = lambda / mu;

            double sum = 0;
            for (int n = 0; n < s; ++n)
            {
                sum += (System.Math.Pow(a, n) / Factorial(n));
            }
            double P0 = 1 / (sum + System.Math.Pow(a, s) / Factorial(s - 1) / (s - a));

            m_Lq = P0 * System.Math.Pow(a, s) * rho / (Factorial(s) * System.Math.Pow(1 - a, 2));
            m_Wq = Lq / lambda;

            m_W = Wq + 1 / mu;
            m_L = Lq + a;

            m_PWq0 = P0;
            for (int n = 1; n < s; ++n)
            {
                double P_n = System.Math.Pow(a, n) * P0 / Factorial(n);
                m_PWq0 += P_n;
            }
        }

        public override double GetTSF(double t)
        {
            return 1 - (1 - PWq0) * System.Math.Exp(-mu * (s - a) * t);
        }

        public override double GetGoS()
        {
            throw new NotImplementedException();
        }
    }
}
