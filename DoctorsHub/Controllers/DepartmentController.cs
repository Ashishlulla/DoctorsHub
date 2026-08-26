using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class DepartmentController : Controller
    {

        //Private Feilds
        private readonly DepartmentApiService _departmentApiService;

        //Constructor
        public DepartmentController(DepartmentApiService departmentApiService) 
        {
            _departmentApiService = departmentApiService;
        }


        public async Task<IActionResult> Index()
        {
            var departments = await  _departmentApiService.GetDepartmentsAsync();
            return View(departments);
        }
    }
}
