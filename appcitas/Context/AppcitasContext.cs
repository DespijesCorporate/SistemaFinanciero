using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using appcitas.Models;

namespace appcitas.Context
{
    public class AppcitasContext : DbContext
    {
        public AppcitasContext() : base("name=AppcitasContext")
        {

        }

        public DbSet<Variable> Variables { get; set; }
        public DbSet<Parametro> ParametrosDeFunciones { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Config> Configuraciones { get; set; }
        public DbSet<ConfigItem> ItemsDeConfiguracion { get; set; }
        public DbSet<Reclamo> Reclamos { get; set; }
        public DbSet<ItemDeReclamo> ItemsDeReclamo { get; set; }
        public DbSet<VariableDeItem> VariablesDeItem { get; set; }
        public DbSet<Emisores> DbSetEmisores { get; set; }
        public DbSet<Anualidad> Anualidades { get; set; }
        public DbSet<AnualidadVariableEvaluada> VariablesEvaluadasDeAnualidad { get; set; }
        public DbSet<AnualidadResultadoObtenido> ResultadoObtenidosDeAnualidad { get; set; }

        //public DbSet<Citas> CitasProgramadas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var vEntity = modelBuilder.Entity<Variable>();
            vEntity.ToTable("SGRC_Variables");

            var parametrosEntity = modelBuilder.Entity<Parametro>();
            parametrosEntity.ToTable("SGRC_Parametros");
            parametrosEntity.HasRequired(x => x.Tipo).WithMany()
                .HasForeignKey(x => new { x.ConfigID, x.TipoId })
                .WillCascadeOnDelete(false);

            var funcionEntity = modelBuilder.Entity<Funcion>();
            funcionEntity.ToTable("SGRC_Funciones");
            funcionEntity.HasRequired(f => f.TipoDeRetorno).WithMany()
                .HasForeignKey(f => new { f.ConfigId, f.FuncionTipoDeRetorno })
                .WillCascadeOnDelete(false);


            var configEntity = modelBuilder.Entity<Config>();
            configEntity.ToTable("SGRC_Config");
            configEntity.Property(x => x.Estado).HasColumnName("ConfigStatus");

            modelBuilder.Entity<ConfigItem>().ToTable("SGRC_ConfigItem");

            var reclamoEntity = modelBuilder.Entity<Reclamo>();
            reclamoEntity.ToTable("SGRC_Reclamos");

            var itemsDeReclamoEntity = modelBuilder.Entity<ItemDeReclamo>();
            itemsDeReclamoEntity.ToTable("SGRC_ItemsDeReclamos");
            itemsDeReclamoEntity.HasKey(x => new { x.ReclamoId, x.ItemDeReclamoId });
            itemsDeReclamoEntity.HasRequired(x => x.Reclamo).WithMany(r => r.ItemsDeReclamo)
                .HasForeignKey(x => x.ReclamoId);

            var variablesAEvaluarEntity = modelBuilder.Entity<VariableDeItem>();
            variablesAEvaluarEntity.ToTable("SGRC_VariablesDeItems");
            variablesAEvaluarEntity.HasRequired(x => x.ItemDeReclamo)
                .WithMany(i => i.VariablesAEvaluar)
                .HasForeignKey(x => new { x.ReclamoId, x.ItemDeReclamoId });

            var emisoresEnt = modelBuilder.Entity<Emisores>();
            emisoresEnt.ToTable("SGRC_Emisor");

            var anualidadEntity = modelBuilder.Entity<Anualidad>();
            anualidadEntity.ToTable("SGRC_Anualidades");

            var variableAnualidadEntity = modelBuilder.Entity<AnualidadVariableEvaluada>();
            variableAnualidadEntity.ToTable("SGRC_VariablesEvaluadasDeAnualidad");
            variableAnualidadEntity.HasKey(x => new { x.AnualidadId, x.VariableDeItemId });
            variableAnualidadEntity.HasRequired(x => x.Anualidad).WithMany().HasForeignKey(x => x.AnualidadId);

            var resultadoAnualidadEntity = modelBuilder.Entity<AnualidadResultadoObtenido>();
            resultadoAnualidadEntity.ToTable("SGRC_ResultadosDeAnualidad");
            resultadoAnualidadEntity.HasKey(x => x.ItemDeReclamoId);
            resultadoAnualidadEntity.HasRequired(x => x.Anualidad).WithMany().HasForeignKey(x => x.AnualidadId);

            //var citaEntity = modelBuilder.Entity<Citas>();
            //citaEntity.ToTable("SGRC_Cita");
        }
    }
}