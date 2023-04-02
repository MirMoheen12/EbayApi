using EbayApi.AbaysideModels;
using System.Text;
using System;
using eBay.Service.Core.Soap;

namespace EbayApi.AbaysideModels
{
    public class GetSessionIDRequest
    {

        public string RuName { get; set; }
        public string ErrorLanguage { get; set; }
        public string MessageID { get; set; }
        public string Version { get; set; }
        public WarningLevelCodeType WarningLevel { get; set; }
    }
}
