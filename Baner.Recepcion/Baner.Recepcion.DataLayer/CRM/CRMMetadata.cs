using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using CRM365.Conector;
using System.Collections.Generic;


namespace Baner.Recepcion.DataLayer.CRM
{
    public class CRMMetadata
    {
        private readonly IOrganizationService _xrmServerConnection;
        public Service service { get; set; }
        public CRMMetadata()
        {
            string org = System.Configuration.ConfigurationManager.AppSettings["uri"];
            string user = System.Configuration.ConfigurationManager.AppSettings["username"];
            string pass = System.Configuration.ConfigurationManager.AppSettings["password"];

            this.service = new Service(org, "", user, pass);

            _xrmServerConnection = service.OrganizationService;
        }

        public Dictionary<int, string> RetrieveOptionsetMetadata(string entidad, string optionset)
        {
            //Dictionary to store value and text
            Dictionary<int, string> _DropdownDatasource = new Dictionary<int, string>();
            //Create request to fetch optionset
            RetrieveAttributeRequest _Request = new RetrieveAttributeRequest
            {
                EntityLogicalName = entidad,
                LogicalName = optionset,
                RetrieveAsIfPublished = true
            };

            // Execute the request


            RetrieveAttributeResponse _Response = (RetrieveAttributeResponse)_xrmServerConnection.Execute(_Request);
            OptionMetadata[] optionList = { };
            if (_Response.AttributeMetadata.GetType() == typeof(BooleanAttributeMetadata))
            {
                BooleanAttributeMetadata _BooleanAttributeMetadata = (BooleanAttributeMetadata)_Response.AttributeMetadata;
                OptionMetadataCollection o = new OptionMetadataCollection();
                o.Add(_BooleanAttributeMetadata.OptionSet.FalseOption);
                o.Add(_BooleanAttributeMetadata.OptionSet.TrueOption);
                optionList = o.ToArray();
            }
            else
            {
                PicklistAttributeMetadata _PicklistAttributeMetadata = (PicklistAttributeMetadata)_Response.AttributeMetadata;
                optionList = _PicklistAttributeMetadata.OptionSet.Options.ToArray();
            }

            foreach (OptionMetadata _Optionset in optionList)
            {
                _DropdownDatasource.Add(int.Parse(_Optionset.Value.ToString()), _Optionset.Label.UserLocalizedLabel.Label);
            }

            return _DropdownDatasource;

        }
    }
}
