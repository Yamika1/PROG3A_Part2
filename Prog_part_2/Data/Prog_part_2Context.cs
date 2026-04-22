using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prog_part_2.Models;

namespace Prog_part_2.Data
{
    public class Prog_part_2Context : DbContext
    {
        public Prog_part_2Context (DbContextOptions<Prog_part_2Context> options)
            : base(options)
        {
        }

        public DbSet<Prog_part_2.Models.Client> Client { get; set; } = default!;
        public DbSet<Prog_part_2.Models.Contracts> Contracts { get; set; } = default!;
        public DbSet<Prog_part_2.Models.Manager> Manager { get; set; } = default!;
        public DbSet<Prog_part_2.Models.ServiceRequests> ServiceRequests { get; set; } = default!;
        public DbSet<ContractFile> ContractFiles { get; set; }
    }
}
