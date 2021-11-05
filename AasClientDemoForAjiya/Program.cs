using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAS.Client;
using AAS.Common.AssetModel;
using AAS.Common.Identity;
using AAS.Common.Parameters.MetadataParams;
using AAS.Common.Parameters.UserParams;
using Newtonsoft.Json;

namespace AasClientDemoForAjiya
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var client = new AasClient("InsertUrlOfAas", "InsertApiKey");

            //ONE TIME CREATE ONLY
            var newAsset = await client.AssetApi.AddAsset(new AddAssetParam()
            {
                Name = "TestClassroom",
                Description = "Final year students classroom",
                Properties = new List<Property>()
                {
                    new Property()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = "StudentName",
                        Description = "Student name",
                        PropertyType = PropertyType.String
                    },
                    new Property()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = "StudentDOB",
                        Description = "Date of birth of student",
                        PropertyType = PropertyType.Datetime,
                    }
                }
            });

            //To access assets created in the past
            var getAllAssets = await client.AssetApi.GetAllAssets();

            Console.WriteLine("Asset created - ");
            Console.WriteLine(JsonConvert.SerializeObject(newAsset, Formatting.Indented));
            Console.WriteLine("Press Enter to add instance data.");
            Console.Read();

            //Object created based on the properties defined previously.
            var newObj = new
            {
                StudentName = "Don",
                StudentDob = DateTime.Now
            };

            //Saving to AAS. here we are adding it to the ID of the newly created asset above.
            var addInstanceData = await client.InstanceApi.AddInstanceData(newAsset.Id, newObj,
                Guid.NewGuid().ToString("N"));
            Console.WriteLine("Newly Added Instance Data - ");
            Console.WriteLine(JsonConvert.SerializeObject(addInstanceData, Formatting.Indented));


            Console.WriteLine("Press Enter to add user. This will create a new department, role and location.");
            Console.Read();
            // Creating a user requires a department, role and location to be created beforehand. 
            var newDept = await client.DepartmentApi.AddDepartment("IT Department", "Description for department");
            var newRole = await client.RoleApi.AddRole(new RoleParam()
            {
                Name = "Senior Developer",
                Description = "SampleDescriptionForRole",
                Type = RoleType.User
            });
            var location = await client.LocationApi.AddSite("Shah Alam", "Description for Shah Alam", "12345");
            

            //After creating, they are available for querying using the code below.
            var getDept = await client.DepartmentApi.GetAllDepartments();
            var getRole = await client.RoleApi.GetAllRole();
            var getLocation = await client.LocationApi.GetSites();

            //Creating the new user.
            var newUser = await client.UserApi.AddUser(new AddUserParam()
            {
                DepartmentId = new List<string> { getDept.FirstOrDefault()?.DepartmentId },
                Role = getRole.FirstOrDefault()?.Id,
                FirstName = "Don",
                LastName = "Charles",
                Email = "email@example.com",
                Username = "don",
                LocationId = getLocation.FirstOrDefault()?.FullId,
                Password = "NewPassWord123",
                PasswordPromptChange = false,
                PhoneNumber = "0123456789",
                Position = "Developer",
                StaffId = "001"
            });

            Console.WriteLine($"User Created = {newUser.Id}");
            Console.WriteLine("Press Enter to login.");
            Console.Read();

            //Login code will throw error if invalid, ensure to try catch it.
            try
            {
                var login = await client.UserApi.Login("don", "NewPassWord123");
                Console.WriteLine($"{login.FirstName} has logged in");
                Console.WriteLine(JsonConvert.SerializeObject(login, Formatting.Indented));
            }
            catch (Exception e)
            {
                //Login fail if wrong username or password.
                Console.WriteLine(e);
                Console.WriteLine("Failed to login");
            }

            Console.WriteLine("--- END ---");
            Console.Read();
        }
    }
}