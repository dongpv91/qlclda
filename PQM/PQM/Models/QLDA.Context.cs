﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PQM.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class project_infoEntities : DbContext
    {
        public project_infoEntities()
            : base("name=project_infoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<activity_log> activity_log { get; set; }
        public DbSet<customer> customers { get; set; }
        public DbSet<end_project> end_project { get; set; }
        public DbSet<project> projects { get; set; }
        public DbSet<quality> qualities { get; set; }
        public DbSet<staff> staffs { get; set; }
        public DbSet<team> teams { get; set; }
        public DbSet<type_project> type_project { get; set; }
        public DbSet<user> users { get; set; }
    }
}
