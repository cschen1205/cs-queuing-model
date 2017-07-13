using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimuKit.OR.Queueing
{
    public class MCModel : BasicModel
    {
        public double mean_service_time;
        public int service_time_erlang_k;
        public double mean_interarrival_time;

        //the number of service calls that enter on average per time unit 
        private double m_lambda;
        public double lambda
        {
            get { return m_lambda; }
            set { m_lambda = value; }
        }

        public int simulated_service_count;

        //number of servers/agents
        public int s;

        private List<double> m_interarrival_times = new List<double>();
        private List<double> m_service_times = new List<double>();
        private List<double> m_waiting_times = new List<double>();
        private List<double> m_server_free_times = new List<double>();

        private DistributionModel m_interarrival_time_distribution;
        private DistributionModel m_service_time_distribution;
        
        
        public override void Build()
        {
            m_interarrival_time_distribution = new Exponential();
            m_interarrival_time_distribution.Mean = mean_interarrival_time;

            //Console.WriteLine("k: {0} lambda: {1}", service_time_erlang_k, service_time_erlang_k / mean_service_time);
            m_service_time_distribution = new Erlang(service_time_erlang_k, service_time_erlang_k / mean_service_time);


            for (int service_index=0; service_index < simulated_service_count; ++service_index)
            {
                m_interarrival_times.Add(m_interarrival_time_distribution.Next());
                m_service_times.Add(m_service_time_distribution.Next());

                //Console.WriteLine("interarrival time: {0} service_time: {1}", m_interarrival_times[service_index], m_service_times[service_index]);
                m_waiting_times.Add(0);
            }

            for (int server_index = 0; server_index < s; ++server_index)
            {
                m_server_free_times.Add(0);
            }

            double current_time = 0;
            for (int service_index = 0; service_index < simulated_service_count; ++service_index)
            {
                current_time += m_interarrival_times[service_index];
                bool is_waiting_required = true;
                for (int server_index = 0; server_index < s; ++server_index)
                {
                    if (m_server_free_times[server_index] <= current_time)
                    {
                        m_server_free_times[server_index] = current_time + m_service_times[service_index];
                        m_waiting_times[service_index] = 0;
                        is_waiting_required = false;
                        break;
                    }
                }
                if (is_waiting_required)
                {
                    double min_free_time=double.MaxValue;
                    int earliest_free_server = 0;
                    for (int server_index = 0; server_index < s; ++server_index)
                    {
                        if (min_free_time > m_server_free_times[server_index])
                        {
                            min_free_time = m_server_free_times[server_index];
                            earliest_free_server = server_index;
                        }
                    }
                    m_waiting_times[service_index] = min_free_time - current_time;
                    current_time = min_free_time;
                }
            }
        }

        public override double GetTSF(double AWT)
        {
            var result = (from w in m_waiting_times
                          where w <= AWT
                          select w);
            return (double)result.Count() / m_waiting_times.Count;
        }

        public override double GetGoS()
        {
            throw new NotImplementedException();
        }
    }
}
