﻿using Core.Info.general;
using System;
using System.Linq;

namespace Core.Data.general
{
    public class TBCINV_APPCORREOS_DATA
    {
        Entities_general db = new Entities_general();
        public TBCINV_APPCORREOS_INFO GET_ORDEN()
        {
            try
            {
                var Orden = db.TBCINV_APPCORREOS.Where(q => q.FECHA_ENVIO == null).OrderBy(q=>q.FECHA_APRO).FirstOrDefault();
                if (Orden == null)
                    return null;

                TBCINV_APPCORREOS_INFO INFO = new TBCINV_APPCORREOS_INFO
                {
                    CINV_LOGIN = Orden.CINV_LOGIN,
                    CINV_NUM = Orden.CINV_NUM,
                    CINV_TDOC = Orden.CINV_TDOC,
                    CINV_ST = Orden.CINV_ST,
                    FECHA_APRO = Orden.FECHA_APRO,
                    ROL_APRO = Orden.ROL_APRO,
                    COMENTARIO = Orden.COMENTARIO
                };

                return INFO;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public bool GUARDAR(TBCINV_APPCORREOS_INFO INFO)
        {
            try
            {
                var orden = db.TBCINV_APPCORREOS.Where(q => q.CINV_NUM == INFO.CINV_NUM && q.CINV_TDOC == INFO.CINV_TDOC && q.FECHA_ENVIO == null).FirstOrDefault();
                if (orden == null)
                    return true;

                orden.FECHA_ENVIO = DateTime.Now;
                orden.COMENTARIO = INFO.COMENTARIO;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
