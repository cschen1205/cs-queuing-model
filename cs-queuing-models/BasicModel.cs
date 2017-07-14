using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueuingModels
{
    public abstract class BasicModel
    {
        public abstract void Build();

        //telephone service fraction, the fraction of services meeting the answer time AWT
        //function assumes that call will wait in a queue instead of being immediately dropped
        //applicable to queueing system such as Erlang C and M/M/s
        public abstract double GetTSF(double AWT);

        //Grade of Service, aka, probability of delay
        //the probability that an arbitrary caller finds all servers/agents occupied
        //this also is the probability that a call is dropped in a loss system
        //applicable to loss system such as Erlang B and Extended Erlang B
        public abstract double GetGoS();

        public static double Factorial(int n)
        {
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
    }
}
