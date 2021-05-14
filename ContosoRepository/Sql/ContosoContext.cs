//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using Contoso.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    /// <summary>
    /// Entity Framework Core DbContext for Contoso.
    /// </summary>
    public class ContosoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=contoso.db");
        }
        /// <summary>
        /// Creates a new Contoso DbContext.
        /// </summary>
        public ContosoContext(DbContextOptions<ContosoContext> options) : base(options)
        { }
      
        /// <summary>
        /// Gets disciplines DbSet.
        /// </summary>
        public DbSet<Discipline> Disciplines { get; set; }
        /// <summary>
        /// Gets groups DbSet.
        /// </summary>
        public DbSet<Group> Groups { get; set; }
        /// <summary>
        /// Gets positions DbSet.
        /// </summary>
        public DbSet<Position> Positions { get; set; }
        /// <summary>
        /// Gets teachers DbSet.
        /// </summary>
        public DbSet<Teacher> Teachers { get; set; }
        /// <summary>
        /// Gets users DbSet.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}
