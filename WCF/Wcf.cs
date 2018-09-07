using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
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
                var host = new WebServiceHost(typeof(WcfService), new Uri(url));
                var ep = host.AddServiceEndpoint(typeof(IWcfService), new WebHttpBinding(), "");
                host.Open();

                //try
                //{

                //    Console.WriteLine("WCF: OK");
                //}
                //catch (CommunicationException cex)
                //{
                //    Console.WriteLine("An exception occurred: {0}", cex.Message);
                //    Host.Abort();
                //}



                //_host = new ServiceHost(typeof(IWcfService), new Uri(url));

                ////var endPoint = _host.AddServiceEndpoint(typeof(IWcfService), new WebHttpBinding(), "");

                ////var behavior = new WebHttpBehavior();
                ////behavior.
                ////behavior.AddBindingParameters(endPoint, );
                ////_host.AddServiceEndpoint(typeof(IWcfService), new WebHttpBinding(), "");


                //_host.Open();

                Console.WriteLine($"WCF {host.Description.Endpoints[0].Address}... OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("WCF: FAIL\n" + ex.Message);
            }
        }
    }
}
