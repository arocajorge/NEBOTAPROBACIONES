﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    [DbConfigurationType(typeof(MyDbConfiguration))]
    public partial class Entities_general : DbContext
    {
        public Entities_general()
            : base("name=Entities_general")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ARINDEX> ARINDEX { get; set; }
        public virtual DbSet<ARPROVEEDOR> ARPROVEEDOR { get; set; }
        public virtual DbSet<TBCINV> TBCINV { get; set; }
        public virtual DbSet<TBDINV> TBDINV { get; set; }
        public virtual DbSet<BODEGAS> BODEGAS { get; set; }
        public virtual DbSet<VW_ORDENES_TRABAJO> VW_ORDENES_TRABAJO { get; set; }
        public virtual DbSet<VW_ORDENES_TRABAJO_TOTAL> VW_ORDENES_TRABAJO_TOTAL { get; set; }
        public virtual DbSet<USUARIOS> USUARIOS { get; set; }
        public virtual DbSet<SUCURSAL> SUCURSAL { get; set; }
        public virtual DbSet<CAB_BITACORA> CAB_BITACORA { get; set; }
        public virtual DbSet<DET_BITACORA> DET_BITACORA { get; set; }
        public virtual DbSet<DET_BITACORA2> DET_BITACORA2 { get; set; }
        public virtual DbSet<VW_BITACORAS> VW_BITACORAS { get; set; }
    }
}