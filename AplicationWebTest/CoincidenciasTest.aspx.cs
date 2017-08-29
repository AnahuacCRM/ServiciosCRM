using Baner.Recepcion.BusinessInterfaces;
using Baner.Recepcion.BusinessLayer;
using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.DataLayer;
using Baner.Recepcion.OperationalManagement;
using Baner.Recepcion.Services.Controllers;
using Banner.Recepcion.WebApiTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AplicationWebTest
{
    public partial class CoincidenciasTest : System.Web.UI.Page
    {
        srvGestionCoincidenciasController _GestionCoincidencias;
        ILogger _ilogger;
        IBannerProcessor _bannerProcessor;
        IBannerRepository _ibannerRepository;
        IOpportunityRepository _iOpotunityRepository;
        Coincidencias ProspectoC;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                _ilogger = new DebugerLogger();
                _ibannerRepository = new BannerRepository();
                _iOpotunityRepository = new OpportunityRepository();
                _bannerProcessor = new BannerProcessor(_ilogger, _ibannerRepository, _iOpotunityRepository);
                _GestionCoincidencias = new srvGestionCoincidenciasController(_ilogger, _bannerProcessor);
            }

            //_GestionCoincidencias.Request = new HttpRequestMessage()
            //{
            //    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }

            //};
         

            
        }
        private void GetCoincidencias()
        {
            WebApiCoincidencias Coinci = new WebApiCoincidencias();
            Coinci.CoincidenciasControler();


        }
    }
}