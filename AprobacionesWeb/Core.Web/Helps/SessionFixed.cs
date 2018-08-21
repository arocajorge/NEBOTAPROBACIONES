using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Web.Helps
{
    public interface ISessionValueProvider
    {
        string ID { get; set; }
        string LINEA { get; set; }
    }

    public static class SessionFixed
    {
        private static ISessionValueProvider _sessionValueProvider;
        public static void SetSessionValueProvider(ISessionValueProvider provider)
        {
            _sessionValueProvider = provider;
        }

        public static string ID
        {
            get { return _sessionValueProvider.ID; }
            set { _sessionValueProvider.ID = value; }
        }

        public static string LINEA
        {
            get { return _sessionValueProvider.LINEA; }
            set { _sessionValueProvider.LINEA = value; }
        }       
    }

    public class WebSessionValueProvider : ISessionValueProvider
    {
        private const string _ID = "IDSESSION";
        private const string _LINEA = "LINEASESSION";
        public string ID
        {
            get { return (string)HttpContext.Current.Session[_ID]; }
            set { HttpContext.Current.Session[_ID] = value; }
        }
        public string LINEA
        {
            get { return (string)HttpContext.Current.Session[_LINEA]; }
            set { HttpContext.Current.Session[_LINEA] = value; }
        }        
    }
}