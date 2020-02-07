using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WatchGuardNASAPhotos.Models;

namespace WatchGuardNASAPhotos.Services
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetPhotos();
    }
}
