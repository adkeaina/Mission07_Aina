using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission07_Aina.Models;

namespace Mission07_Aina.Controllers;

public class HomeController : Controller
{
    private JoelHiltonMovieCollectionContext _context;
    public HomeController(JoelHiltonMovieCollectionContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetToKnowJoel()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult AddNewMovie()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult AddNewMovie(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            // Re-populate categories if needed
            ViewBag.Categories = _context.Categories.ToList();
            return View(movie); // Re-render the form with validation errors
        }
        _context.Movies.Add(movie);
        _context.SaveChanges();
        return View("Confirmation", movie);
    }

    public IActionResult AllMovies()
    {
        var movies = _context.Movies.Include(movie => movie.Category).ToList();
        return View(movies);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Movie movie = _context.Movies
            .Include(movie => movie.Category)
            .Single(movie => movie.MovieId == id);
        
        ViewBag.Categories = _context.Categories.ToList();
        return View("AddNewMovie", movie);
    }

    [HttpPost]
    public IActionResult Edit(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            // Re-populate categories if needed
            ViewBag.Categories = _context.Categories.ToList();
            return View("AddNewMovie", movie);
        }
        _context.Update(movie);
        _context.SaveChanges();
        return RedirectToAction("AllMovies");
    }

    public IActionResult Delete(int id)
    {
        var movie = _context.Movies.Single(movie => movie.MovieId == id);
        _context.Movies.Remove(movie);
        _context.SaveChanges();
        
        return RedirectToAction("AllMovies");
    }
}