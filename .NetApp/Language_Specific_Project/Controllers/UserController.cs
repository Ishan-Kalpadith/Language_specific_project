using Microsoft.AspNetCore.Mvc;
using Db_Manipulate.DTO;
using Db_Manipulate;
using Microsoft.AspNetCore.Authorization;
using Language_Specific_Project.Services;

namespace Language_Specific_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly LoginService _loginService;
        private readonly EditUserService _editUserService;
        private readonly GetDistanceService _getDistanceService;
        private readonly SearchUserService _searchUserService;
        private readonly GetCustomerListByZipCodeService _getCustomerListService;
        private readonly GetAllCustomerListService _getAllCustomerListService;

        public UserController(ApplicationDbContext context, IConfiguration configuration, LoginService loginService, EditUserService editUserService, GetDistanceService getDistanceService, SearchUserService searchUserService, GetCustomerListByZipCodeService getCustomerListService, GetAllCustomerListService getAllCustomerListService)
        {
            _context = context;
            _configuration = configuration;
            _loginService = loginService;
            _editUserService = editUserService;
            _getDistanceService = getDistanceService;
            _searchUserService = searchUserService;
            _getCustomerListService = getCustomerListService;
            _getAllCustomerListService = getAllCustomerListService;
        }

        // POST api/User/Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var (token, role) = _loginService.AuthenticateUser(loginDTO.Username, loginDTO.Password);

            if (token != null)
            {
                return Ok(new { access_token = token });
            }

            return Unauthorized("Invalid username or password");
        }


        // PUT api/User/EditUser/{_id}
        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpPut("EditUser/{_id}")]
        public async Task<IActionResult> EditUser(string _id, [FromBody] UserUpdateDTO userUpdate)
        {
            var (success, message) = await _editUserService.EditUserAsync(_id, userUpdate);

            if (success)
            {
                return Ok(message);
            }

            return BadRequest(message);
        }



        //GET api/User/GetDistance/_id?latitude=value&longitude=value
        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpGet("GetDistance/{_id}")]
        public IActionResult GetDistance(string _id, double latitude, double longitude)
        {
            try
            {
                var user = _context.UserDatas.FirstOrDefault(u => u._id == _id);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.latitude.HasValue && user.longitude.HasValue)
                {
                    double userLatitude = user.latitude.Value;
                    double userLongitude = user.longitude.Value;

                    double distanceInKilometers = _getDistanceService.CalculateDistance(
                        userLatitude,
                        userLongitude,
                        latitude,
                        longitude
                    );

                    return Ok(distanceInKilometers);
                }
                else
                {
                    return BadRequest("User's latitude or longitude is missing.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        //GET api/User/SearchUser?searchText=text to search
        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpGet("SearchUser")]
        public IActionResult SearchUser(string searchText)
        {
            try
            {
                var matchedUsers = _searchUserService.SearchUsers(searchText);
                return Ok(matchedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        //GET api/User/GetCustomerListByZipCode
        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpGet("GetCustomerListByZipCode")]
        public IActionResult GetCustomerListByZipCode()
        {
            try
            {
                var result = _getCustomerListService.GetCustomersByZipCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        //GET api/User/GetAllCustomerList
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllCustomerList")]
        public IActionResult GetAllCustomerList()
        {
            try
            {
                var customerList = _getAllCustomerListService.GetAllCustomersAndAddresses();
                return Ok(customerList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
