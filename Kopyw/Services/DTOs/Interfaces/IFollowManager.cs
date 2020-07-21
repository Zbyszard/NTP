﻿using Kopyw.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Services.DTOs.Interfaces
{
    public interface IFollowDTOManager
    {
        Task<FollowDTO> Add(FollowDTO newFollow);
        Task<FollowDTO> Delete(string authorId, string userId);
    }
}
