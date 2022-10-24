using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RAS.Bootcamp.Katalog.MVC.NET.Models;
using RAS.Bootcamp.Katalog.MVC.NET.Models.Entities;

namespace RAS.Bootcamp.Katalog.MVC.NET.Controllers;

public class TransaksiController : Controller
{
    private readonly ILogger<TransaksiController> _logger;
    private readonly dbmarketContext _dbcontext;
    public TransaksiController(ILogger<TransaksiController> logger,dbmarketContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    
}