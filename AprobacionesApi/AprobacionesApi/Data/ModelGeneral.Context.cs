﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AprobacionesApi.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntitiesGeneral : DbContext
    {
        public EntitiesGeneral()
            : base("name=EntitiesGeneral")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<APP_LOGERROR> APP_LOGERROR { get; set; }
        public DbSet<ARINDEX> ARINDEX { get; set; }
        public DbSet<CAB_BITACORA> CAB_BITACORA { get; set; }
        public DbSet<DET_BITACORA> DET_BITACORA { get; set; }
        public DbSet<TBBI_SECDOC> TBBI_SECDOC { get; set; }
        public DbSet<TBCINV_APP_ORDENES_SALTADAS> TBCINV_APP_ORDENES_SALTADAS { get; set; }
        public DbSet<TBCINV_APPCORREOS> TBCINV_APPCORREOS { get; set; }
        public DbSet<TBDINV> TBDINV { get; set; }
        public DbSet<TBNOM_EMPLEADO> TBNOM_EMPLEADO { get; set; }
        public DbSet<USUARIOS> USUARIOS { get; set; }
        public DbSet<USUARIOS_MENU_APP> USUARIOS_MENU_APP { get; set; }
        public DbSet<SUCURSAL> SUCURSAL { get; set; }
        public DbSet<VW_ORDENES_NOMINA_APP> VW_ORDENES_NOMINA_APP { get; set; }
        public DbSet<TBNOM_ORDEN_NOMINA_CAB> TBNOM_ORDEN_NOMINA_CAB { get; set; }
        public DbSet<TBNOM_ORDEN_NOMINA_DET> TBNOM_ORDEN_NOMINA_DET { get; set; }
        public DbSet<VW_BITACORAS_CUMPLIMIENTO_APP> VW_BITACORAS_CUMPLIMIENTO_APP { get; set; }
        public DbSet<ARPROVEEDOR> ARPROVEEDOR { get; set; }
        public DbSet<VW_ORDENES_TRABAJO_APP> VW_ORDENES_TRABAJO_APP { get; set; }
        public DbSet<TBCINV> TBCINV { get; set; }
        public DbSet<TBDPED> TBDPED { get; set; }
        public DbSet<DET_BITACORA2> DET_BITACORA2 { get; set; }
        public DbSet<VW_BITACORAS2_APP> VW_BITACORAS2_APP { get; set; }
        public DbSet<VW_BITACORAS_APP> VW_BITACORAS_APP { get; set; }
        public DbSet<TBCPED> TBCPED { get; set; }
        public DbSet<TBCINV_FILTROS_APP> TBCINV_FILTROS_APP { get; set; }
        public DbSet<VW_FILTROS_APP> VW_FILTROS_APP { get; set; }
        public DbSet<VW_PEDIDOS_APP> VW_PEDIDOS_APP { get; set; }
        public DbSet<VW_PRESUPUESTO_APP> VW_PRESUPUESTO_APP { get; set; }
        public DbSet<CAB_BITACORA_PARAM> CAB_BITACORA_PARAM { get; set; }
        public DbSet<CAB_BITACORA_PARAM_OP4> CAB_BITACORA_PARAM_OP4 { get; set; }
        public DbSet<VW_ORDENES_TRABAJO_TOTAL_APP> VW_ORDENES_TRABAJO_TOTAL_APP { get; set; }
        public DbSet<VW_ORDENES_TRABAJO_TOTAL_APS> VW_ORDENES_TRABAJO_TOTAL_APS { get; set; }
    }
}
