﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialPhotoAppMVC.Enums;
using SocialPhotoAppMVC.ViewModels;
using System.Linq;
using X.PagedList;

namespace SocialPhotoAppMVC.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<IPagedList<Photo>>> SearchPhotos(SearchPhotoVM? searchInput, int? page)
        {
            var foundPhotos = await _context.Photos
                            .Where(p => (searchInput.Title == null || p.Title.Contains(searchInput.Title)) &&
                            (searchInput.Description == null || p.Description.Contains(searchInput.Description)) &&
                            (searchInput.Category == null || p.Category == searchInput.Category))
                            .ToListAsync();


            var response = new ServiceResponse<IPagedList<Photo>>();

            if (foundPhotos.Count == 0)
            {
                response.Success = false;
                response.Message = "No photos found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await foundPhotos.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        public async Task<ServiceResponse<IPagedList<Album>>> SearchAlbums(SearchAlbumVM searchInput, int? page)
        {
            var foundAlbums = await _context.Albums.Include(a => a.User)
                .Where(p => (searchInput.Title == null || p.Title.Contains(searchInput.Title)) &&
                (searchInput.Description == null || p.Description.Contains(searchInput.Description))
                && (searchInput.Username == null || p.User.NickName.Contains(searchInput.Username)))
                .ToListAsync();


            var response = new ServiceResponse<IPagedList<Album>>();

            if (foundAlbums.Count == 0)
            {
                response.Success = false;
                response.Message = "No albums found.";
                return response;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var pagedList = await foundAlbums.ToPagedListAsync(pageNumber, pageSize);
            response.Data = pagedList;

            return response;
        }

        private async Task<IPagedList<Photo>> PaginateListOfPhotos(List<Photo> listOfPhotos, int? page, int pageSize) 
        {
            var pageNumber = (page ?? 1);
            var pagedList = await listOfPhotos.ToPagedListAsync(pageNumber, pageSize);
            return pagedList;
        }
    }
}
