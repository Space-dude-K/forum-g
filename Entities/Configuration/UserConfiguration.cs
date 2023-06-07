using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasData(
            SeedUsers(20)
            );
        }
        private List<AppUser> SeedUsers(int usersCount)
        {
            var users = new List<AppUser>();

            for(int i = 0; i < usersCount; i++)
            {
                AppUser appUser = new()
                {
                    FirstName = "Ivan" + i,
                    LastName = "Petrov" + i,
                    Cabinet = Random.Shared.Next(999).ToString(),
                    InternalPhone = Random.Shared.Next().ToString(),
                    BirthDate = RandomDay().ToString(),
                    UserName = "G600-U" + i,
                    Email = "G600-U" + i + "@mfrb.by",
                    PhoneNumber = Random.Shared.Next().ToString() + Random.Shared.Next().ToString() + Random.Shared.Next().ToString(),
                };

                users.Add(appUser);
            }

            return users;
        }

        private Random gen = new Random();
        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

    }
}
