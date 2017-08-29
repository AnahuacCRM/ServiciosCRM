using Baner.Recepcion.DataInterfaces;
using Baner.Recepcion.OperationalManagement.Exceptions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XRM;

namespace Baner.Recepcion.DataLayer.Transformators
{

    public class PickListTransformer
    {
        private readonly IPickListRepository _picklistRepository;

        public PickListTransformer(IPickListRepository picklistRepository)
        {
            _picklistRepository = picklistRepository;
        }

        public OptionSetValue GetTipoAlumno(string value)
        {
            var item = _picklistRepository.ListaTipoAlumno();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Tipo Alumno: {0}"
                    , value));
        }

        public OptionSetValue GetEstatusAlumno(string value)
        {
            var item = _picklistRepository.ListaEstatusAlumno();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Estatus Alumno: {0}"
                    , value));
        }

        public OptionSetValue GetTipoAdmision(string value)
        {
            var item = _picklistRepository.ListaTipoAdmision();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Tipo Admision: {0}"
                    , value));
        }

        public OptionSetValue GetTipoDesicionAdmision(string value)
        {
            var item = _picklistRepository.ListaTipoDesicionAdmision();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Tipo Desicion Admision: {0}"
                    , value));
        }

        public OptionSetValue GetTipoAdmisionPL(string value)
        {
            var item = _picklistRepository.ListTipoAdmision(Opportunity.EntityLogicalName, "rs_tipoadmisionpl");
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist Tipo Admision PL: {0}"
                    , value));
        }

        public OptionSetValue GetOrigen(string value)
        {
            var item = _picklistRepository.ListOrigen("rs_prospecto.EntityLogicalName", "rs_origen");
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist de Origen: {0}"
                    , value));
        }

        public OptionSetValue GetTipoBeca(string value)
        {
            //var item = _picklistRepository.ListaTipoBeca(rs_becacredito.EntityLogicalName, "rs_becacredito");
            //if (item.ContainsValue(value))
            //    return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            //else
            //    throw new PickListException(
            //        string.Format("No se pudo resolver el picklist de Tipo Beca: {0}"
            //        , value));

            return null;
        }

        public OptionSetValue GetParentesco(string value)
        {
            var item = _picklistRepository.ListaParentesco();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist de Parentesco: {0}"
                    , value));
        }

        public OptionSetValue GetTipoColegio(string value)
        {
            var item = _picklistRepository.ListaTipoColegio();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist de Tipo Colegio: {0}"
                    , value));
        }

        public OptionSetValue GetEstadoCivil(string value)
        {
            var item = _picklistRepository.ListaEstadoCivil();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                throw new PickListException(
                    string.Format("No se pudo resolver el picklist de Estado Civil: {0}"
                    , value));
        }

        public OptionSetValue GetTipoContacto(string value)
        {
            var item = _picklistRepository.ListaTipoContacto();
            if (item.ContainsValue(value))
                return new OptionSetValue(item.FirstOrDefault(i => i.Value == value).Key);
            else
                return null;
                //throw new PickListException(
                //    string.Format("No se pudo resolver el picklist de Tipo Contacto {0}"
                //    , value));
        }
      

    }
}
