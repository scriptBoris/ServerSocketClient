using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCF
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        bool SetContent(string content);

        [OperationContract]
        [WebGet(UriTemplate = "/ping/")]
        string Ping();

        [OperationContract]
        [WebInvoke(Method ="GET", UriTemplate = "/echo/{s}/")]
        string Echo(string s);
    }
}
