﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Web;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        Cheeps = new();
        _repository = repository;
    }

    // public ActionResult OnGet(string author)
    // {
    //     int.TryParse(Request.Query["page"], out int page);
    //     Cheeps = _repository.GetCheepDTOsFromAuthor(author, page);
    //     return Page();
    // }

    // Ny: Skal hente dine cheeps + dem du følger
    public ActionResult OnGet(string author)
    {
        int.TryParse(Request.Query["page"], out int page);
        Cheeps = _repository.GetCheepDTOsFromAuthor(author, page);
        return Page();
    }

    public async Task<IActionResult> OnPost(CheepCreateDTO newCheep)
    {
        String email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;

        var cheep = new CheepCreateDTO { Text = newCheep.Text, Author = User.Identity!.Name!, Email = email };

        await _repository.CreateCheep(cheep);

        return Redirect($"/{User.Identity!.Name}");
    }
}