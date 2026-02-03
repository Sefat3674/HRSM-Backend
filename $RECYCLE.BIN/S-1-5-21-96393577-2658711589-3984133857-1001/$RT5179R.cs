using BLL.CommonBLL;
using BLL.MasterBLL.IRepositoryMaster;
using DAL.Data;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using static BLL.CommonBLL.Common;

namespace SmartManufacturingV2.Controllers
{
    [Route("ctlruntimedashboard")]
    public class CTLRuntimeDashboardController : Controller
    {
        private readonly IUnitOfWorkMaster _unitOfWork;
        private readonly ApplicationDbContext _db;
        public CTLRuntimeDashboardController(IUnitOfWorkMaster unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        [Route("ctlruntimedashboard")]
        public async Task<IActionResult> CTLRuntimeDashboard()
        {
            var UserId = User.FindFirst("Id");
            var RoleId = User.FindFirst("RoleId");
            ViewBag.UserId = UserId!.Value;
            ViewBag.RoleId = RoleId!.Value;
            //DataTable dt = new DataTable();
            //var result = dt.Compute("(10+1)*10", String.Empty);
            var userPermission = _unitOfWork.Common.GetUserPermissions(Convert.ToInt64(UserId!.Value));

            if (userPermission[0].Count > 0 && RoleId!.Value != "1" && RoleId!.Value != "8" && RoleId!.Value != "3")
            {
                var locationList = await _unitOfWork.Location.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false && userPermission[0].Contains(x.Id));
                ViewBag.LocationList = locationList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            else
            {
                var locationList = await _unitOfWork.Location.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false);
                ViewBag.LocationList = locationList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            if (userPermission[1].Count > 0 && RoleId!.Value != "1" && RoleId!.Value != "8" && RoleId!.Value != "3")
            {
                var businessUnitList = await _unitOfWork.BusinessUnit.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false && userPermission[1].Contains(x.Id));
                ViewBag.BusinessUnitList = businessUnitList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            else
            {
                var businessUnitList = await _unitOfWork.BusinessUnit.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false);
                ViewBag.BusinessUnitList = businessUnitList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            var motherCompanyList = await _unitOfWork.MotherCompany.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false);
            ViewBag.MotherCompanyList = motherCompanyList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View();
        }
        //[HttpGet]
        //[Route("getperminutedashboarddata")]
        //public async Task<IActionResult> GetPerMinuteDashboardData(string jsonString)
        //{
        //    try
        //    {
        //        var UserId = User.FindFirst("Id");
        //        var RoleId = User.FindFirst("RoleId");
        //        var dataArray = JsonConvert.DeserializeObject<List<string>>(jsonString);
        //        //dataArray[0] = UserId.Value.ToString();
        //        //dataArray[1] = RoleId.Value.ToString();
        //        var machineList = await _unitOfWork.Common.GetCTLRuntimeMachineList(dataArray);
        //        var idleMachineList = await _unitOfWork.Common.GetCTLIdleMachineList(dataArray);
        //        var lineChartData = await _unitOfWork.Common.GetCTLChartData(dataArray);
        //        var dashboardData = await _unitOfWork.Common.GetDashboardCaardData();
        //        var chartData = new ArrayList();
        //        var label = lineChartData.Select(x => x.Date).ToList();
        //        var machineCount = lineChartData.Select(x => x.TotalMachineCount).ToList();
        //        var runtimeCount = lineChartData.Select(x => x.TotalRuntimeInHours).ToList();
        //        chartData.Add(label);
        //        chartData.Add(machineCount);
        //        chartData.Add(runtimeCount);
        //        return Json(new { machineList, idleMachineList, chartData, success = true });
        //    }
        //    catch (Exception)
        //    {
        //        var machineList = new List<RuntimeLineChartData>();
        //        var idleMachineList = new List<IdleMachineData>();
        //        return Json(new { machineList, idleMachineList, success = false });
        //    }
        //}
        [HttpGet]
        [Route("getperminutedashboarddata")]
        public async Task<IActionResult> GetPerMinuteDashboardData(string jsonString)
        {
            try
            {
                var UserId = User.FindFirst("Id");
                var RoleId = User.FindFirst("RoleId");
                var dataArray = JsonConvert.DeserializeObject<List<string>>(jsonString);
                //dataArray[0] = UserId.Value.ToString();
                //dataArray[1] = RoleId.Value.ToString();

                // Existing functionality
                var machineList = await _unitOfWork.Common.GetCTLRuntimeMachineList(dataArray);
                var idleMachineList = await _unitOfWork.Common.GetCTLIdleMachineList(dataArray);
                var lineChartData = await _unitOfWork.Common.GetCTLChartData(dataArray);
                var dashboardData = await _unitOfWork.Common.GetDashboardCaardData();

                var chartData = new ArrayList();
                var label = lineChartData.Select(x => x.Date).ToList();
                var machineCount = lineChartData.Select(x => x.TotalMachineCount).ToList();
                var runtimeCount = lineChartData.Select(x => x.TotalRuntimeInHours).ToList();
                chartData.Add(label);
                chartData.Add(machineCount);
                chartData.Add(runtimeCount);

                // New productivity data
                var productivityData = await _unitOfWork.Common.GetProductivityDataWithConfigurableHours(dataArray,12);
                var daywiseProductivityChartData= await _unitOfWork.Common.GetDayWiseProductivity(dataArray);
                return Json(new
                {
                    machineList,
                    idleMachineList,
                    chartData,
                    productivityData,
                    dashboardData,
                    daywiseProductivityChartData,// Add the new productivity data
                    success = true
                });
            }
            catch (Exception ex)
            {
                var machineList = new List<RuntimeLineChartData>();
                var idleMachineList = new List<IdleMachineData>();
                var productivityData = new List<ProductivityData>();
                return Json(new
                {
                    machineList,
                    idleMachineList,
                    productivityData,
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("gettotaldataforruntimedashboard")]
        public async Task<IActionResult> GetTotalDataforRuntimeDashboard(string jsonString)
        {
            try
            {
                var UserId = User.FindFirst("Id");
                var RoleId = User.FindFirst("RoleId");
                var dataArray = JsonConvert.DeserializeObject<List<string>>(jsonString);
                dataArray[0] = UserId.Value.ToString();
                dataArray[1] = RoleId.Value.ToString();
                var dashData = await _unitOfWork.Common.GetTotalDataforRuntimeDashboard(dataArray);

                return Json(new { data = dashData, success = true });
            }
            catch (Exception)
            {
                var data = new List<long>() { 0, 0, 0, 0 };
                return Json(new { data = data, success = false });
            }
        }

    }
}
