using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AAS.Client;
using AAS.Common.Identity;
using AAS.Common.Parameters.UserParams;

namespace AasClientDemoForAjiya
{
    public class FirstTimeOctaneSetup
    {
        public async Task Setup()
        {
            var client = new AasClient("InsertUrlOfAas", "InsertApiKey");

            var addDept = await client.DepartmentApi.AddDepartment("TestDepartment", "Description of test department");

            var addLocation = await client.LocationApi.AddSite("TestSite", "Sample description of site", "58000");

            var addRole = await client.RoleApi.AddRole(new RoleParam()
            {
                Name = "TestRole",
                Description = "Description of a role",
                Type = RoleType.SuperAdmin
            });

            var addUser = await client.UserApi.AddUser(new AddUserParam()
            {
                DepartmentId = new List<string> { addDept.DepartmentId },
                LocationId = addLocation.FullId,
                FirstName = "Test",
                LastName = "Account",
                Email = "testacc@gmail.com",
                Role = addRole.Id,
                Password = "Abcd.12345",
                PasswordPromptChange = false,
                Username = "testacc",
                PhoneNumber = "0123456789",
                Position = "Testing Manager",
                StaffId = "TA001"

            });
            Console.WriteLine($"Created user {addUser.Id}");
        }
    }
}