using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VPTWebApp.Models;

namespace VPTWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private const string IndividualTicketsKey = "individualTickets";
    private const string GroupTicketsKey = "groupTickets";
    private const string SectionsKey = "sections";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(new TicketViewModel());
    }

    [HttpGet]
    public IActionResult Show()
    {
        List<TicketModel> individualTickets = DeserializeTickets(HttpContext.Session.GetString(IndividualTicketsKey));
        List<TicketModel> groupTickets = DeserializeTickets(HttpContext.Session.GetString(GroupTicketsKey));
        List<SectionModel> sections = DeserializeSections(HttpContext.Session.GetString(SectionsKey));

        TicketViewModel ticketViewModel = new() 
            { IndividualTickets = individualTickets, GroupTickets = groupTickets, Sections = sections};
        ShowViewModel showViewModel = new(ticketViewModel);
        return View(showViewModel);
    }

    [HttpPost]
    public ActionResult Index(TicketViewModel model)
    {
        HttpContext.Session.SetString(IndividualTicketsKey, JsonSerializer.Serialize(model.IndividualTickets));
        HttpContext.Session.SetString(GroupTicketsKey,  JsonSerializer.Serialize(model.GroupTickets));
        HttpContext.Session.SetString(SectionsKey,  JsonSerializer.Serialize(model.Sections));
            
        return RedirectToAction("Show");
    }
    
    [HttpPost]
    public IActionResult AddGroupTicket(TicketViewModel model)
    {
        model.GroupTickets.Add(new TicketModel { OrderDate = DateTime.Now, Kids = 0, Adults = 0 });
        return Json(model);
    }

    [HttpPost]
    public IActionResult AddIndividualTicket(TicketViewModel model)
    {
        model.IndividualTickets.Add(new TicketModel { OrderDate = DateTime.Now, Kids = 0, Adults = 1 });
        return Json(model);
    }
    
    [HttpPost]
    public IActionResult AddSection(TicketViewModel model)
    {
        model.Sections.Add(new SectionModel { Rows = 3, Columns = 10 });
        return Json(model);
    }
    
    private static List<TicketModel> DeserializeTickets(string? json)
    {
        return Deserialize<TicketModel>(json);
    }
    
    private static List<SectionModel> DeserializeSections(string? json)
    {
        return Deserialize<SectionModel>(json);
    }
    
    private static List<T> Deserialize<T>(string? json)
    {
        return JsonSerializer.Deserialize<List<T>>(json ?? "") ?? new List<T>();
    }
}