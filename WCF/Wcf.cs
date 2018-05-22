using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF
{
    public class Wcf
    {
        private ServiceHost _host;

        public Wcf(string url)
        {
            try
            {
                _host = new ServiceHost(typeof(WcfService), new Uri(url));
                _host.AddServiceEndpoint(typeof(IWcfService), new WebHttpBinding(), "");
                _host.Open();
                Console.WriteLine("WCF: OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("WCF: FAIL\n"+ex.Message);
            }
        }
    }
}
