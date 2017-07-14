using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueuingModels
{
    //pure loss system with infinite population
    public class ErlangB : BasicModel
    {
        //the number of service calls that enter on average per time unit 
        private double m_lambda;
        public double lambda
        {
            get { return m_lambda; }
            set { m_lambda = value; }
        }

        //the average service time of calls or average holding time 
        private double m_beta;
        public double beta
        {
            get { return m_beta; }
            set { m_beta = value; }
        }

        //load=lambda * beta
        private double m_a;
        public double a
        {
            get { return m_a; }
            set { m_a = value; }
        }

        //number of servers/agents
        private int m_s;
        public int s
        {
            get { return m_s; }
            set { m_s = value; }
        }

        public override void Build()
        {
            m_a = lambda * beta;
        }

        //Grade of Service, aka, probability of delay
        //the probability that an arbitrary caller finds all servers/agents occupied
        public override double GetGoS()
        {
            return ErlangBFormula(a, s);
        }

        public static double ErlangBFormula(double load, int number_of_servers)
        {
            double c = System.Math.Pow(load, number_of_servers) / Factorial(number_of_servers);
            double sum = 0;
            for (int j = 0; j < number_of_servers; ++j)
            {
                sum += System.Math.Pow(load, number_of_servers) / Factorial(number_of_servers);
            }
            return c / (sum + c);
        }

        public override double GetTSF(double AWT)
        {
            throw new NotImplementedException();
        }

        //average waiting time, the average amount of time that calls spend waiting
        public virtual double GetASA()
        {
            return GetGoS() * beta / (s - a);
        }
    }
}
