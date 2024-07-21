using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;

namespace Hng.Infrastructure.Repository.Mock
{
    public class MockUserRepository : IUserRepository
    {
        public static List<User> usersBase = new List<User>();

        public MockUserRepository()
        {
            SetupData();
        }

        private void SetupData()
        {
            usersBase = new List<User>()
            {
                 new User
                 {
                     Id =new Guid("7acbba30-a989-4aa4-c702-08db3920bd4e"),
                     FirstName = "John",
                     LastName = "Doe",
                     AvatarUrl= "https://www.google.com/imgres?q=avatar%20image&imgurl=https%3A%2F%2Fwww.svgrepo.com%2Fshow%2F382100%2Ffemale-avatar-girl-face-woman-user-7.svg&imgrefurl=https%3A%2F%2Fwww.svgrepo.com%2Fsvg%2F382100%2Ffemale-avatar-girl-face-woman-user-7&docid=Bvmj8N7cUCkq_M&tbnid=6MZNQN2CbNwV2M&vet=12ahUKEwj5mJr1wriHAxWEElkFHYskDzgQM3oECB0QAA..i&w=800&h=800&hcb=2&ved=2ahUKEwj5mJr1wriHAxWEElkFHYskDzgQM3oECB0QAA",
                     Email ="johndoe@gmail.com",
                     PhoneNumber="070123456789",
                     Profile = new Profile
                     {
                        Id = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Doe",
                        PhoneNumber="070123456789",
                        UserId = new Guid("7acbba30-a989-4aa4-c702-08db3920bd4e")
                     },
                     Organizations = new List<Organization>()
                     {
                         new Organization
                         {
                              Id = Guid.NewGuid(),
                              Description = "My first company on HNG",
                              Name = "HNG Company"
                         },
                         new Organization
                         {
                              Id = Guid.NewGuid(),
                              Description = "My second company on HNG",
                              Name = "HNG Flex Style"
                         }
                     }
                 },
                 new User
                 {
                     Id = Guid.NewGuid(),
                     FirstName = "Sarah",
                     LastName = "Monday",
                     AvatarUrl= "https://www.google.com/imgres?q=avatar%20image&imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-vector%2Fyoung-smiling-man-avatar-man-with-brown-beard-mustache-hair-wearing-yellow-sweater-sweatshirt-3d-vector-people-character-illustration-cartoon-minimal-style_365941-860.jpg&imgrefurl=https%3A%2F%2Fwww.freepik.com%2Ffree-photos-vectors%2Favatar&docid=DjJcL6-DnnZi6M&tbnid=sgt84Z43v7LubM&vet=12ahUKEwj5mJr1wriHAxWEElkFHYskDzgQM3oECCMQAA..i&w=626&h=626&hcb=2&ved=2ahUKEwj5mJr1wriHAxWEElkFHYskDzgQM3oECCMQAA",
                     Email ="johndoe@gmail.com",
                     PhoneNumber="070123456789",
                     Profile = new Profile
                     {
                        Id = Guid.NewGuid(),
                        FirstName = "Sarah",
                        LastName = "Monday",
                        PhoneNumber="070123456789",
                        UserId = Guid.NewGuid()
                     },
                     Organizations = new List<Organization>()
                     {
                          new Organization
                         {
                              Id = Guid.NewGuid(),
                              Description = "A very important company",
                              Name = "Paddy & Co. Ltd"
                         }
                     }
                 },
            };
        }

        Task<User> IGenericRepository<User>.AddAsync(User entity)
        {
            usersBase.Add(entity);
            return Task.FromResult(entity);
        }

        Task<User> IGenericRepository<User>.DeleteAsync(Guid id)
        {
            var user = usersBase.FirstOrDefault(m => m.Id.Equals(id));
            if (user is not null)
            {
                usersBase.Remove(user);
            }
            return Task.FromResult(user);
        }

        Task<bool> IGenericRepository<User>.Exists(Guid id)
        {
            var isExist = usersBase.Any(m => m.Id.Equals(id));
            return Task.FromResult(isExist);
        }

        Task<ICollection<User>> IGenericRepository<User>.GetAllAsync()
        {
            var users = usersBase.ToList();
            return Task.FromResult(users as ICollection<User>);
        }

        Task<User> IGenericRepository<User>.GetAsync(Guid id)
        {
            var user = usersBase.FirstOrDefault(g => g.Id.Equals(id));
            return Task.FromResult(user);
        }

        Task<User> IUserRepository.GetUserById(Guid id)
        {
            var user = usersBase.FirstOrDefault(g => g.Id.Equals(id));
            return Task.FromResult(user);
        }

        Task IGenericRepository<User>.UpdateAsync(User entity)
        {
            var user = usersBase.FirstOrDefault(m => m.Id.Equals(entity.Id));
            if (user is not null)
            {
                usersBase.Remove(user);

                user = entity;
                usersBase.Add(user);
            }

            return Task.CompletedTask;
        }
    }
}
