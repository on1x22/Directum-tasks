using PersonalMeetingsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsManager.Domain
{
    internal interface IMeetingsInfoWriter
    {
        Task Write(List<Meeting> meetings);
    }
}
