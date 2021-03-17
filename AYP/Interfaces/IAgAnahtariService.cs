using AYP.Entities;
using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AYP.Interfaces
{
    public interface IAgAnahatariService
    {
        ResponseModel SaveAgAnahtariTur(AgAnahtariTur agAnahtariTur);
    }
}
