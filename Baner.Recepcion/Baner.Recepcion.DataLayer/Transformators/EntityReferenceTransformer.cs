using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRM;
using Baner.Recepcion.BusinessTypes;

namespace Baner.Recepcion.DataLayer.Transformators
{
    public class EntityReferenceTransformer
    {
        private readonly ICatalogRepository _catalogrepository;

        public EntityReferenceTransformer(ICatalogRepository catalogRepository)
        {
            _catalogrepository = catalogRepository;

        }

        public EntityReference GetPrograma(string programa)
        {
            var item = _catalogrepository.ListaPrograma();
            if (item.ContainsKey(programa))
                return new EntityReference(ua_programaV2.EntityLogicalName, new Guid(item[programa]));


            //var programas = _catalogrepository.ListaProgramas();
            //var item = programas.Find(p => p.CodigoCampus == campus && p.CodigoPrograma == programa);
            //if (item != null)
            //    return new EntityReference(rs_programa.EntityLogicalName, item.IdPrograma);
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Programa: {0}"
            //        , programa));

            return null;
        }



        public EntityReference GetPeriodo(string value)
        {

            var item = _catalogrepository.ListaPeriodo();
            if (item.ContainsKey(value))
                return new EntityReference(ua_periodo.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Periodo: {0}"
                    , value));
        }

        public EntityReference GetCampus(string value)
        {

            var item = _catalogrepository.ListaCampus();
            if (item.ContainsKey(value))
                return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Campus: {0}"
                    , value));
        }


        public EntityReference GetVPDISE(string value)//VPDI Sin Excepción
        {
            //var item = _catalogrepository.ListaCampus();
            //if (item.ContainsKey(value))
            //    return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[value]));
            //else
            //    return null;

            return null;


        }
        public EntityReference GetEscuela(string value)
        {
            var item = _catalogrepository.ListaEscuela();
            if (item.ContainsKey(value))
                return new EntityReference(ua_escuela.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Escuela: {0}"
                    , value));

        }
        public EntityReference GetCampusAdmision(string value)
        {
            //var item = _catalogrepository.ListaCampus();
            //if (item.ContainsKey(value))
            //    return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Campus Admision: {0}"
            //        , value));

            return null;
        }

        public EntityReference GetNivel(string value)
        {
            var item = _catalogrepository.ListaNiveles();
            if (item.ContainsKey(value))
                return new EntityReference(ua_niveles.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Nivel Educativo: {0}"
                    , value));

        }



        public EntityReference GetMunicipioId(string value)//Sin Excepción
        {
            var item = _catalogrepository.GetIdMunicipio(value);
            if (item.ContainsKey(value))
                return new EntityReference(ua_delegacion_municipio.EntityLogicalName, new Guid(item[value]));
            else
                return null;
            //throw new LookupException(
            //    string.Format("No se pudo resolver el Lookup de Estado: {0}"
            //    , value));


        }

        public EntityReference GetMunicipioAsesor(string value)//Sin Excepción
        {
            var item = _catalogrepository.GetIdMunicipio(value);
            if (item.ContainsKey(value))
                return new EntityReference(ua_delegacion_municipio_asesor.EntityLogicalName, new Guid(item[value]));
            else
                return null;
            //throw new LookupException(
            //    string.Format("No se pudo resolver el Lookup de Estado: {0}"
            //    , value));


        }

        private EntityReference GetEstadoXSinonimo(string value)
        {
            //var item = _catalogrepository.ListaEstadoXSinonimo();
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[value]));
            //else
            //{

            //    foreach (var i in item)
            //    {
            //        string[] sinonimos = i.Key.Split(',');
            //        var results = Array.FindAll(sinonimos, s => s.Equals(value));
            //        if (results != null && results.Length >= 1)
            //            if (results[0] == value)
            //                return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[i.Key]));
            //    }
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Estado: {0}"
            //        , value));
            return null;
        }


        #region Direcciones sin regresar Excepción
        public EntityReference GetEstadoSE(string value)//Sin Excepción
        {
            var item = _catalogrepository.ListaEstado();
            if (item.ContainsKey(value))
                return new EntityReference(ua_estados.EntityLogicalName, new Guid(item[value]));
            else
                return GetEstadoXSinonimoSE(value);
            //throw new LookupException(
            //    string.Format("No se pudo resolver el Lookup de Estado: {0}"
            //    , value));


        }

        private EntityReference GetEstadoXSinonimoSE(string value)//Sin Excepción
        {
            //var item = _catalogrepository.ListaEstadoXSinonimo();
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[value]));
            //else
            //{

            //    foreach (var i in item)
            //    {
            //        string[] sinonimos = i.Key.Split(',');
            //        var results = Array.FindAll(sinonimos, s => s.Equals(value));
            //        if (results != null && results.Length >= 1)
            //            if (results[0] == value)
            //                return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[i.Key]));
            //    }
            //    return null;
            //    //throw new LookupException(
            //    //    string.Format("No se pudo resolver el Lookup de Estado: {0}"
            //    //    , value));
            //}
            return null;
        }

        public EntityReference GetMunicipioSE(Municipio municipio)//Sin Excepción
        {
            var item = _catalogrepository.ListaMunicipio(municipio);
            //var item = _catalogrepository.GetIdMunicipio(municipio.CodigoMunicipio); 
            var founded = item.Find(mun =>
                        mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
                   );
            if (founded != null)
                return new EntityReference(ua_colonia.EntityLogicalName, founded.IdCRM);
            else
            {
                municipio.Estado = GetcodigoEstadoXsinonimoMun(municipio.Estado);
                var itemSinonimo = _catalogrepository.ListaMunicipio(municipio);
                var foundedwithSinonimo = itemSinonimo.Find(mun =>
                            mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
                       );
                if (foundedwithSinonimo != null)
                    return new EntityReference(ua_colonia.EntityLogicalName, foundedwithSinonimo.IdCRM);
                else
                    return null;
                //throw new LookupException(
                //    string.Format("No se pudo resolver el Lookup de Municipio: {0}"
                //    , municipio.CodigoMunicipio));
            }




        }


        #endregion

        public string GetcodigoEstadoXsinonimo(string value)
        {
            var item = _catalogrepository.ListaCodigoEstado();
            if (item.ContainsKey(value))
                return item[value];
            else
            {

                foreach (var i in item)
                {
                    string[] sinonimos = i.Key.Split(',');
                    var results = Array.FindAll(sinonimos, s => s.Equals(value));
                    if (results != null && results.Length >= 1)
                        if (results[0] == value)
                            return i.Value;
                }
                //throw new LookupException(
                //    string.Format("No se pudo resolver el Lookup de Código Estado: {0}"
                //    , value));
                return null;
            }

        }

        public string GetcodigoEstadoXsinonimoMun(string value)//Sin Excepción
        {
            var item = _catalogrepository.ListaCodigoEstado();
            if (item.ContainsKey(value))
                return item[value];
            else
            {

                foreach (var i in item)
                {
                    string[] sinonimos = i.Key.Split(',');
                    var results = Array.FindAll(sinonimos, s => s.Equals(value));
                    if (results != null && results.Length >= 1)
                        if (results[0] == value)
                            return i.Value;
                }
                return value;
                //throw new LookupException(
                //    string.Format("No se pudo resolver el Lookup de Código Estado: {0}"
                //    , value));
            }
            //return null;
        }

        public EntityReference GetMunicipio1(Municipio municipio)
        {
            //var item = _catalogrepository.ListaMunicipio(municipio);
            //var founded = item.Find(mun =>
            //            mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //       );
            //if (founded != null)
            //    return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //else
            //{
            //    municipio.Estado = GetcodigoEstadoXsinonimoMun(municipio.Estado);
            //    var itemSinonimo = _catalogrepository.ListaMunicipio(municipio);
            //    var foundedwithSinonimo = itemSinonimo.Find(mun =>
            //                mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //           );
            //    if (foundedwithSinonimo != null)
            //        return new EntityReference(rs_colonia.EntityLogicalName, foundedwithSinonimo.IdCRM);
            //    else
            //        throw new LookupException(
            //            string.Format("No se pudo resolver el Lookup de Municipio: {0}"
            //            , municipio.CodigoMunicipio));
            //}
            return null;



        }

        public EntityReference GetCarreraWeb(string value)
        {
            //var item = _catalogrepository.ListaCarreraWeb();
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_carreraweb.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Carrera Web: {0}"
            //        , value));

            return null;


        }

        public EntityReference GetVPD(string value)
        {
            //var item = _catalogrepository.ListaCampus();
            //if (item.ContainsKey(value))
            //    return new EntityReference(BusinessUnit.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de VPD: {0}"
            //        , value));

            return null;

        }

        public EntityReference GetProspectoSolicitanteBeca(Guid value)
        {
            //if (value != Guid.Empty)
            //    return new EntityReference(rs_prospecto.EntityLogicalName, value);
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Prospecto Solicitante: {0}"
            //        , value));
            return null;

        }

        public EntityReference GetColegioProcedencia(string value)
        {
            var item = _catalogrepository.ListaColegio();
            if (item.ContainsKey(value))
                return new EntityReference(ua_colegios.EntityLogicalName, new Guid(item[value]));
            else
                return null;
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Colegio de Procedencia: {0}"
            //        , value));

        }

        public EntityReference GetTipoDireccion(string value)
        {
            //var item = _catalogrepository.ListaTipoDireccion();
            //if (item.ContainsKey(value))
            //    return new EntityReference(Lead.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Tipo Direccion: {0}"
            //        , value));
            return null;
        }

        public EntityReference GetPais(string value)
        {
            var item = _catalogrepository.ListaPais();
            if (item.ContainsKey(value))
                return new EntityReference(ua_pais.EntityLogicalName, new Guid(item[value]));
            else
                return null;
            //throw new LookupException(
            //    string.Format("No se pudo resolver el Lookup de País: {0}"
            //    , value));

        }


        public EntityReference GetCodigoPostal(string value)
        {

            //var item = _catalogrepository.ListaCodigoPostal(value);
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_codigopostal.EntityLogicalName, new Guid(item[value]));
            //else
            //    return null;
            ////throw new LookupException(
            ////    string.Format("No se pudo resolver el Lookup de Codigo Postal: {0}"
            ////    , value));          
            return null;
        }


        public EntityReference GetTipoTelefono(string value)
        {
            //var item = _catalogrepository.ListaTipoTelefono();
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_tipotelefono.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Tipo Telefono: {0}"
            //        , value));
            return null;
        }

        public EntityReference GetTipoCorreo(string value)
        {
            //var item = _catalogrepository.ListaTipoCorreo();
            //if (item.ContainsKey(value))
            //    return new EntityReference(rs_tipocorreoelectronico.EntityLogicalName, new Guid(item[value]));
            //else
            //    throw new LookupException(
            //        string.Format("No se pudo resolver el Lookup de Tipo Correo: {0}"
            //        , value));
            return null;
        }

        public EntityReference GetNacionalidad(string value)
        {
            var item = _catalogrepository.ListaNacionalidad();
            if (item.ContainsKey(value))
                return new EntityReference(Lead.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Nacionalidad: {0}"
                    , value));


        }

        public EntityReference GetReligion(string value)
        {
            var item = _catalogrepository.ListaReligion();
            if (item.ContainsKey(value))
                return new EntityReference(Lead.EntityLogicalName, new Guid(item[value]));
            else
                throw new LookupException(
                    string.Format("No se pudo resolver el Lookup de Religion: {0}"
                    , value));

        }

        public EntityReference GetCarreraWebByCodigo(string value)
        {
            var item = _catalogrepository.ListaCarreraWebCodigo();
            if (item.ContainsKey(value))
                return new EntityReference(ua_carrerauniversal.EntityLogicalName, new Guid(item[value]));
            else
                return null;
            //throw new LookupException(
            //    string.Format("No se pudo resolver el Lookup de CarreraWeb: {0}"
            //    , value));

        }

        public string GetEtapaProceso(string EntityLogicalName, string Variable)
        {
            // return _catalogrepository.ObtenerEtapaProceso(EntityLogicalName, Variable);
            return null;
        }

        /// <summary>
        /// Valida que el Idbanner que se envia coincida con el que tiene registrado la Oportunidad
        /// </summary>
        /// <param name="OportunidadId"></param>
        /// <param name="Idbanner"></param>
        /// <returns></returns>
        public bool GetIdBannerByOpportunity(Guid OportunidadId, string Idbanner)
        {
            //string banner = _catalogrepository.ObtenerIdBanner(OportunidadId);
            //if (string.IsNullOrWhiteSpace(banner))
            //    throw new LookupException("El Id de oportunidad no existe");
            //if (Idbanner == banner)
            //    return true;
            //else
            //    throw new LookupException("El IdBanner de la oportunidad no coincide con el IdBanner proporcionado");
            return false;
        }


        public EntityReference GetColonia(Domicilio source)
        {
            if (!string.IsNullOrEmpty(source.Colonia))
            {
                var colonia = new Colonia()
                {
                    CP = source.CP,
                    DelegacionMunicipio = source.Municipio,
                    Estado = source.Estado,
                    Pais = source.Pais,
                    Nombre = source.Colonia
                };

                var item = _catalogrepository.ListaColonias(colonia);

                var founded = item.Find(col =>
                      col.CP == colonia.CP && col.DelegacionMunicipio == colonia.DelegacionMunicipio
                      && col.Estado == colonia.Estado && col.Pais == colonia.Pais
                      && col.Nombre == colonia.Nombre
                  );
                if (founded != null)
                    return new EntityReference(ua_colonia.EntityLogicalName, founded.IdCRM);
                else
                {

                    colonia.Estado = GetcodigoEstadoXsinonimo(colonia.Estado);
                    var itemSinonimo = _catalogrepository.ListaColonias(colonia);

                    var foundedSinonimo = itemSinonimo.Find(col =>
                          col.CP == colonia.CP && col.DelegacionMunicipio == colonia.DelegacionMunicipio
                          && col.Estado == colonia.Estado && col.Pais == colonia.Pais
                          && col.Nombre == colonia.Nombre
                      );
                    if (foundedSinonimo != null)
                        return new EntityReference(ua_colonia.EntityLogicalName, foundedSinonimo.IdCRM);
                    else
                        return null;
                }


                //if (item.ContainsKey(source.CodigoPostal + source.Colonia))
                //    return new EntityReference(rs_colonia.EntityLogicalName, new Guid(item[source.CodigoPostal + source.Colonia]));
                //else
                //    return null;
                //throw new LookupException(
                //    string.Format("No se pudo resolver el Lookup de colonia de la direccion: {0}"
                //    , source.Colonia));
            }
            else return null;

            return null;
        }



        public EntityReference GetEstado(Domicilio source)
        {
            //var item = _catalogrepository.ListaEstado();
            //if (source.Pais == "99" || source.Pais == "MEX")
            //{

            //    if (item.ContainsKey(source.Estado))
            //        return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[source.Estado]));
            //    else
            //    {
            //        string Estado = GetcodigoEstadoXsinonimo(source.Estado);
            //        if (item.ContainsKey(Estado))
            //            return new EntityReference(rs_estado.EntityLogicalName, new Guid(item[Estado]));
            //        else
            //            return null;
            //        //throw new LookupException(
            //        //    string.Format("No se pudo resolver el Lookup de Estado de la direccion {0}", source.Estado));
            //    }


            //}
            //else
            //{
            //    if (item.ContainsKey("FR"))
            //        return new EntityReference(rs_estado.EntityLogicalName, new Guid(item["FR"]));
            //    else
            //        return null;
            //    //throw new LookupException(
            //    //    string.Format("No se pudo resolver el Lookup de Estado de la direccion {0}", "FR"));
            //}
            return null;
        }


        public EntityReference GetMunicipio(Domicilio source)
        {
            //if (source != null)
            //{
            //    if (source.Pais == "99" || source.Pais == "MEX")
            //    {
            //        var municipio = new Municipio()
            //        {
            //            CodigoMunicipio = source.Municipio,
            //            Estado = source.Estado,
            //        };
            //        var item = _catalogrepository.ListaMunicipio(municipio);
            //        var founded = item.Find(mun =>
            //                    mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //               );
            //        if (founded != null)
            //            return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //        else
            //        {

            //            string Estado = GetcodigoEstadoXsinonimo(source.Estado);
            //            var municipio2 = new Municipio()
            //            {
            //                CodigoMunicipio = source.Municipio,
            //                Estado = Estado,
            //            };
            //            var itemSinonimo = _catalogrepository.ListaMunicipio(municipio2);
            //            var foundedSinonimo = itemSinonimo.Find(mun =>
            //                        mun.Estado == municipio2.Estado && mun.CodigoMunicipio == municipio2.CodigoMunicipio
            //                   );
            //            if (foundedSinonimo != null)
            //                return new EntityReference(rs_colonia.EntityLogicalName, foundedSinonimo.IdCRM);
            //            else
            //                return null;
            //        }

            //        //throw new LookupException(
            //        //    string.Format("No se pudo resolver el Lookup de Delegacion/Municipio: {0}", source.DelegacionMunicipioId));

            //    }
            //    else //Extranjero
            //    {
            //        var municipio = new Municipio()
            //        {
            //            CodigoMunicipio = "20000",
            //            Estado = "FR",
            //        };
            //        var item = _catalogrepository.ListaMunicipio(municipio);
            //        var founded = item.Find(mun =>
            //                    mun.Estado == municipio.Estado && mun.CodigoMunicipio == municipio.CodigoMunicipio
            //               );
            //        if (founded != null)
            //            return new EntityReference(rs_colonia.EntityLogicalName, founded.IdCRM);
            //        else
            //            return null;
            //        //throw new LookupException(
            //        //    string.Format("No se pudo resolver el Lookup de Delegacion/Municipio: {0}", source.Municipio));
            //    }

            //}
            //else
            //    return null;
            return null;
        }

        public EntityReference GetPaisDireccion(Domicilio source)
        {
            //if (!string.IsNullOrWhiteSpace(source.Pais))
            //{
            //    var item = _catalogrepository.ListaPais();
            //    if (item.ContainsKey(source.Pais))
            //        return new EntityReference(rs_pais.EntityLogicalName, new Guid(item[source.Pais]));
            //    else
            //        return null;
            //}
            //else
            //    return null;
            return null;
        }





        public int GetConguntoOpsiones(string sEntityLoginame, string sCampoDeConjunto, string sValorAsignar)
        {
            int valorREturn = -1;
            var item = _catalogrepository.ConjutoOpciones(sEntityLoginame, sCampoDeConjunto);
            if (item.ContainsKey(sValorAsignar))
                valorREturn = int.Parse(item[sValorAsignar]);

            return valorREturn;
        }
    }
}
