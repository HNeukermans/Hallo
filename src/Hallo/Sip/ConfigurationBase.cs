using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Hallo
{
    public class SipConfigurationBase
    {
        public SipConfigurationBase()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                var dvAttribute = p.GetCustomAttributes(true).OfType<DefaultValueAttribute>().FirstOrDefault();
                
                if (dvAttribute != null) p.SetValue(this, dvAttribute.Value,null);
            }
            
        }

       [CategoryAttribute("Stack")]
       [Browsable(true)] 
       [ReadOnly(false)]
       [BindableAttribute(false)]
       [DesignOnly(false)]
       [DescriptionAttribute("Enter the server bind IP endpoint.")]
       public string BindIpEndPoint { get; set; }

       [CategoryAttribute("Stack")]
       [Browsable(true)]
       [ReadOnly(false)]
       [BindableAttribute(false)]
       [DefaultValueAttribute(25)]
       [DesignOnly(false)]
       [DescriptionAttribute("Enter the maximum # of threads in the pool.")]
       public int MaxThreadPoolSize { get; set; }

       [CategoryAttribute("Stack")]
       [Browsable(true)]
       [ReadOnly(false)]
       [BindableAttribute(false)]
       [DefaultValueAttribute(5)]
       [DesignOnly(false)]
       [DescriptionAttribute("Enter the minimum # of threads in the pool.")]
       public int MinThreadPoolSize { get; set; }

       [CategoryAttribute("Stack")]
       [Browsable(true)]
       [ReadOnly(false)]
       [BindableAttribute(false)]
       [DefaultValueAttribute(true)]
       [DesignOnly(false)]
       [DescriptionAttribute("If true threadPool performance counters are enabled.")]
       public bool EnableThreadPoolPerformanceCounters { get; set; }

       [Category("Server")]
       [Browsable(true)]
       [ReadOnly(false)]
       [Bindable(false)]
       [DefaultValue(true)]
       [DesignOnly(false)]
       [Description("If true the server maintains a transaction for every request-response")]
       public bool IsStateFull { get; set; }

       [CategoryAttribute("Registrar")]
       [DisplayName("Domain")]
       [Browsable(true)]
       [ReadOnly(false)]
       [BindableAttribute(false)]
       [DefaultValueAttribute("contoso.com")]
       [DesignOnly(false)]
       [DescriptionAttribute("Enter name for the SIP registrar domain")]
       public string RegistrarDomain { get; set; }


        
    }
}
